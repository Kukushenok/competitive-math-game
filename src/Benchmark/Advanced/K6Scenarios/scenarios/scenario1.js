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
      startVUs: 20,
      stages: [
        { target: 40, duration: '30s' },
        { target: 70, duration: '30s' },
        { target: 100, duration: '30s' },
        { target: 130, duration: '30s' },
        { target: 160, duration: '30s' },
        { target: 190, duration: '30s' },
        { target: 230, duration: '30s' },
        { target: 260, duration: '30s' },
        { target: 300, duration: '30s' },
      ],
    },
  },
};

const scenario = createScenario(constants, opts);
export const options = scenario.options;
export const setup = scenario.setup;
export default scenario.default;