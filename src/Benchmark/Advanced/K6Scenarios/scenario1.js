import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend, Rate, Counter, Gauge } from 'k6/metrics';

// Кастомные метрики
const registrationTime = new Trend('k6_registration_time_ms');
const getQuestionsTime = new Trend('k6_get_questions_time_ms');
const submitAnswersTime = new Trend('k6_submit_answers_time_ms');
const errorRate = new Rate('k6_error_rate');
const retryCounter = new Counter('k6_retries_total');
const activeUsers = new Gauge('k6_vus_active');

// Конфигурируемые параметры
const PAUSE_MIN = 1;
const PAUSE_MAX = 10;
const RETRY_DELAY = 2;
const MAX_RETRIES = 3;
export function setup() {
    console.log('SLEEPING')
    sleep(10)
    const competitionRes = http.post('http://competitivebackend:8080/competition_ensurance/5');
    let competitionId = '1';
    if(competitionRes.success) {
        competitionId = competitionRes.body.trim();
        console.log(`Created competition with ID: ${competitionId}`);
    }
    return { competitionId: competitionId };
}

// Генерация уникального логина
function generateUniqueLogin(vu, iter, attempt = 0) {
  const chars = 'abcdefghijklmnopqrstuvwxyz0123456789';
  let base = 'user_';
  
  // Add random characters
  for (let i = 0; i < 10; i++) {
    base += chars.charAt(Math.floor(Math.random() * chars.length));
  }
  
  const suffix = `_${vu}_${iter}`;
  
  // Обрезаем до 32 символов с учетом суффикса
  let login = base.slice(0, 30 - suffix.length) + suffix;
  
  if (attempt > 0) {
    login = login.slice(0, 30 - 7) + `_retry${attempt}`;
  }
  
  return login;
}

// Генерация валидного email
function generateEmail(login) {
  return `myemail@email.email`
}

// Генерация случайного пароля
function generatePassword() {
  const lower = 'abcdefghijklmnopqrstuvwxyz';
  const upper = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  const numbers = '0123456789';
  const special = '!@#$%^&*';
  
  let password = '';
  
  // Ensure at least one of each type
  password += lower.charAt(Math.floor(Math.random() * lower.length));
  password += upper.charAt(Math.floor(Math.random() * upper.length));
  password += numbers.charAt(Math.floor(Math.random() * numbers.length));
  password += special.charAt(Math.floor(Math.random() * special.length));
  
  // Add remaining characters
  const allChars = lower + upper + numbers + special;
  for (let i = 0; i < 8; i++) {
    password += allChars.charAt(Math.floor(Math.random() * allChars.length));
  }
  
  // Shuffle the password
  return password.split('').sort(() => 0.5 - Math.random()).join('');
}

// Генерация ответов
function generateAnswers(riddles) {
  return riddles.map(riddle => {
    if (riddle.availableAnswers && riddle.availableAnswers.length > 0) {
      const randomIndex = Math.floor(Math.random() * riddle.availableAnswers.length);
      return {
        textAnswer: riddle.availableAnswers[randomIndex].textAnswer
      };
    } else {
      return {
        textAnswer: riddle.question.substring(0, 100) // Обрезаем если слишком длинный
      };
    }
  });
}

// Функция для повторной попытки
function retryRequest(requestFn, maxRetries = MAX_RETRIES) {
  for (let attempt = 0; attempt < maxRetries; attempt++) {
    const result = requestFn(attempt);
    if (result.success) {
      return result;
    }
    
    if (attempt < maxRetries - 1 && result.retry !== false) {
      retryCounter.add(1);
      sleep(RETRY_DELAY);
    }
  }
  return { success: false };
}

export const options = {
    thresholds: {
        http_req_failed: ['rate<0.01'], // http errors should be less than 1%
        http_req_duration: ['p(95)<200'], // 95% of requests should be below 200ms
    },
    scenarios: {
    my_scenario1: {
         executor: 'ramping-vus',
      startVUs: 10,

      // Pre-allocate necessary VUs.
      stages: [
                // Start 300 iterations per `timeUnit` for the first minute.
                { target: 20, duration: '30s' },

                // Linearly ramp-up to starting 600 iterations per `timeUnit` over the following two minutes.
                { target: 30, duration: '30s' },

                // Continue starting 600 iterations per `timeUnit` for the following four minutes.
                { target: 40, duration: '30s' },

                // Linearly ramp-down to starting 60 iterations per `timeUnit` over the last two minutes.
                { target: 50, duration: '30s' },
                { target: 60, duration: '30s' },
                { target: 70, duration: '30s' },
                { target: 80, duration: '30s' },
                { target: 90, duration: '30s' },
                { target: 100, duration: '1m' },
            ],
        },
    },
};

export default function (data) {
  activeUsers.add(1);
   
  const competitionId = data.competitionId;
  let token;
  // 1. Регистрация с retry
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
    
    // Retry только при конфликте логина
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

  // 2. Получение вопросов
  const questionsSuccess = retryRequest(() => {
    const questionsRes = http.get(
      `http://competitivebackend:8080/api/v1/competitions/${competitionId}/game_session`,
      { headers: { 'Bearer': `${token}` } }
    );
    
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

  // 3. Случайная пауза
  sleep(Math.random() * (PAUSE_MAX - PAUSE_MIN) + PAUSE_MIN);

  // 4. Отправка ответов
  const submitSuccess = retryRequest(() => {
    const answers = generateAnswers(questionsData.riddles);
    
    const submitRes = http.post(
      `http://competitivebackend:8080/api/v1/competitions/${competitionId}/game_session/${sessionId}`,
      JSON.stringify(answers),
      { headers: { 
        'Content-Type': 'application/json',
        'Bearer': `${token}`
      } }
    );
    
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