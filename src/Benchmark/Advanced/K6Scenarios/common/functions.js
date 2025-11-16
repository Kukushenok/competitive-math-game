import { sleep } from 'k6';
import { retryCounter, MAX_RETRIES, RETRY_DELAY } from './metrics.js';

export function generateUniqueLogin(vu, iter, attempt = 0) {
  const chars = 'abcdefghijklmnopqrstuvwxyz0123456789';
  let base = 'user_';
  
  for (let i = 0; i < 10; i++) {
    base += chars.charAt(Math.floor(Math.random() * chars.length));
  }
  
  const suffix = `_${vu}_${iter}`;
  let login = base.slice(0, 30 - suffix.length) + suffix;
  
  if (attempt > 0) {
    login = login.slice(0, 30 - 7) + `_retry${attempt}`;
  }
  
  return login;
}

export function generateEmail(login) {
  return `myemail@email.email`;
}

export function generatePassword() {
  const lower = 'abcdefghijklmnopqrstuvwxyz';
  const upper = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  const numbers = '0123456789';
  const special = '!@#$%^&*';
  
  let password = '';
  password += lower.charAt(Math.floor(Math.random() * lower.length));
  password += upper.charAt(Math.floor(Math.random() * upper.length));
  password += numbers.charAt(Math.floor(Math.random() * numbers.length));
  password += special.charAt(Math.floor(Math.random() * special.length));
  
  const allChars = lower + upper + numbers + special;
  for (let i = 0; i < 8; i++) {
    password += allChars.charAt(Math.floor(Math.random() * allChars.length));
  }
  
  return password.split('').sort(() => 0.5 - Math.random()).join('');
}

export function generateAnswers(riddles) {
  return riddles.map(riddle => {
    if (riddle.availableAnswers && riddle.availableAnswers.length > 0) {
      const randomIndex = Math.floor(Math.random() * riddle.availableAnswers.length);
      return {
        textAnswer: riddle.availableAnswers[randomIndex].textAnswer
      };
    } else {
      return {
        textAnswer: riddle.question.substring(0, 100)
      };
    }
  });
}

export function retryRequest(requestFn, maxRetries = MAX_RETRIES) {
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

export function getNonExpiredCompetitionId(competitions) {
  const now = Date.now();
  const availableCompetitions = competitions.filter(comp => now < comp.expirationTime);
  
  if (availableCompetitions.length === 0) {
    console.log('No non-expired competitions available');
    return null;
  }
  
  const randomIndex = Math.floor(Math.random() * availableCompetitions.length);
  const selectedCompetition = availableCompetitions[randomIndex];
  
  const timeRemaining = Math.ceil((selectedCompetition.expirationTime - now) / 1000);
  console.log(`Selected competition ${selectedCompetition.id} with ${timeRemaining}s remaining`);
  
  return selectedCompetition.id;
}