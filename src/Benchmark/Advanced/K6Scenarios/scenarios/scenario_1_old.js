import { createScenario } from './common_scenario.js';

// Constants specific to this scenario
const constants = {
  PAUSE_MIN: 1.0,
  PAUSE_MAX: 3.5,
  MAX_RETRIES: 3,
  RETRY_DELAY: 2,
  NUM_COMPETITIONS: 9,
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
        { target: 200, duration: '30s' },
        { target: 250, duration: '30s' },
        { target: 300, duration: '30s' },
        { target: 350, duration: '30s' },
        { target: 400, duration: '30s' },
        { target: 500, duration: '30s' },
        { target: 600, duration: '30s' },
        { target: 700, duration: '30s' },
        { target: 800, duration: '30s' },
        { target: 0, duration: '30s' },
      ],
    },
  },
};

const scenario = createScenario(constants, opts);
export const options = scenario.options;
export const setup = scenario.setup;
export default scenario.default;