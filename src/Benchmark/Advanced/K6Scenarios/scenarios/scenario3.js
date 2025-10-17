import { createScenario } from './common_scenario.js';

// Constants specific to this scenario
const constants = {
  PAUSE_MIN: 1.0,
  PAUSE_MAX: 3.5,
  MAX_RETRIES: 3,
  RETRY_DELAY: 2,
  NUM_COMPETITIONS: 6,
  COMPETITION_DELAY: 1
};

// Options specific to this scenario
const opts = {
  scenarios: {
    ramping_scenario: {
      executor: 'ramping-vus',
      startVUs: 700,
      stages: [
         { target: 2000, duration: '30s' },
         { target: 400, duration: '30s' },
         { target: 400, duration: '3m' },
         { target: 0, duration: '30s' },
      ],
    },
  },
};

const scenario = createScenario(constants, opts);
export const options = scenario.options;
export const setup = scenario.setup;
export default scenario.default;