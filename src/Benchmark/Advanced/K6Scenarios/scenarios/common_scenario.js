import { gsetp } from '../common/setup.js';
import { 
  registrationTime, getQuestionsTime, submitAnswersTime, 
  errorRate, activeUsers 
} from '../common/metrics.js';
import { 
  generateUniqueLogin, generateEmail, generatePassword, 
  generateAnswers, retryRequest, getNonExpiredCompetitionId 
} from '../common/functions.js';
import http from 'k6/http';
import { check, sleep } from 'k6';

// Default constants that can be overridden
const DEFAULT_CONSTANTS = {
  PAUSE_MIN: 1.0,
  PAUSE_MAX: 3.5,
  MAX_RETRIES: 3,
  RETRY_DELAY: 2,
  NUM_COMPETITIONS: 6,
  COMPETITION_DELAY: 1
};

// Export a function that creates a scenario with custom constants and options
export function createScenario(customConstants = {}, optionsConfig = {}) {
  // Merge default constants with custom ones
  const constants = { ...DEFAULT_CONSTANTS, ...customConstants };
  
  // Default options
  const defaultOptions = {
    thresholds: {
      http_req_failed: ['rate<0.01'],
      http_req_duration: ['p(95)<200'],
    },
  };
  
  // Merge options
  const options = { ...defaultOptions, ...optionsConfig };
  return {
    options,
    setup: function() { return gsetp(constants.NUM_COMPETITIONS, constants.COMPETITION_DELAY) },
    default: function(data) {
      activeUsers.add(1);
       
      const competitionId = getNonExpiredCompetitionId(data.competitions);
      if (!competitionId) {
        console.log('No available competition, skipping VU');
        activeUsers.add(-1);
        return;
      }

      let token;
      
      // Registration
      const registrationSuccess = retryRequest((attempt) => {
        const login = generateUniqueLogin(__VU, __ITER, attempt);
        const registrationPayload = {
          login: login,
          password: generatePassword(),
          email: generateEmail(login)
        };
        
        console.log(`Attempting registration with login: ${login}`);
        
        const regRes = http.post(
          'http://competitivebackend:8080/api/v1/auth/register',
          JSON.stringify(registrationPayload),
          { headers: { 'Content-Type': 'application/json' } }
        );
        
        if (check(regRes, { 'registration status 200': (r) => r.status === 200 })) {
          const regData = regRes.json();
          if (regData.token) {
            token = regData.token;
            registrationTime.add(regRes.timings.duration);
            console.log(`Registration successful for: ${login}`);
            return { success: true, data: regData };
          }
        }
        
        console.log(`Registration failed for: ${login}, status: ${regRes.status}`);
        
        if (regRes.status === 400 || regRes.status === 409) {
          return { success: false, retry: true };
        }
        
        errorRate.add(1);
        return { success: false, retry: false };
      });

      if (!registrationSuccess.success) {
        console.log('Registration failed after retries');
        activeUsers.add(-1);
        return;
      }

      // Get questions
      const questionsSuccess = retryRequest(() => {
        const questionsRes = http.get(
          `http://competitivebackend:8080/api/v1/competitions/${competitionId}/game_session`,
          { headers: { 'Bearer': `${token}` } }
        );
        
        if (questionsRes.status === 400) {
          console.log(`Competition ${competitionId} might be expired (400 response), skipping VU`);
          return { success: false, retry: false };
        }
        
        if (check(questionsRes, { 
          'get questions status 200': (r) => r.status === 200,
          'got session ID': (r) => r.json('sessionID') !== undefined
        })) {
          getQuestionsTime.add(questionsRes.timings.duration);
          return { success: true, data: questionsRes.json() };
        }
        
        console.log(`Get questions failed, status: ${questionsRes.status}`);
        errorRate.add(1);
        return { success: false, retry: true };
      });

      if (!questionsSuccess.success) {
        console.log('Get questions failed after retries');
        activeUsers.add(-1);
        return;
      }

      const questionsData = questionsSuccess.data;
      const sessionId = questionsData.sessionID;

      // Random pause with configurable bounds
      sleep(Math.random() * (constants.PAUSE_MAX - constants.PAUSE_MIN) + constants.PAUSE_MIN);

      // Submit answers
      const submitSuccess = retryRequest(() => {
        const answers = generateAnswers(questionsData.riddles);
        const payload = {
            sessionID: sessionId,
            answers: answers
        };
        const submitRes = http.post(
          `http://competitivebackend:8080/api/v1/competitions/${competitionId}/game_session`,
          JSON.stringify(payload),
          { headers: { 
            'Content-Type': 'application/json',
            'Bearer': `${token}`
          } }
        );
        
        if (submitRes.status === 400) {
          console.log(`ERROR ${submitRes.body}`);
          return { success: true, retry: false };
        }
        
        if (check(submitRes, { 'submit answers status 200': (r) => r.status === 200 })) {
          submitAnswersTime.add(submitRes.timings.duration);
          console.log('Answers submitted successfully');
          return { success: true };
        }
        
        console.log(`Submit answers failed, status: ${submitRes.status}`);
        errorRate.add(1);
        return { success: false, retry: true };
      });

      activeUsers.add(-1);
    }
  };
}