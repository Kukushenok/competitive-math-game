import { Trend, Rate, Counter, Gauge } from 'k6/metrics';

export const registrationTime = new Trend('k6_registration_time_ms');
export const getQuestionsTime = new Trend('k6_get_questions_time_ms');
export const submitAnswersTime = new Trend('k6_submit_answers_time_ms');
export const errorRate = new Rate('k6_error_rate');
export const retryCounter = new Counter('k6_retries_total');
export const activeUsers = new Gauge('k6_vus_active');

export const PAUSE_MIN = 1.0;
export const PAUSE_MAX = 3.5;
export const RETRY_DELAY = 2;
export const MAX_RETRIES = 3;
export const NUM_COMPETITIONS = 6;
export const COMPETITION_DELAY = 1;