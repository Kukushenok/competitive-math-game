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
      startVUs: 10,
      stages: [
        { target: 100, duration: '30s' },
        { target: 150, duration: '30s' },
        { target: 200, duration: '30s' },
        { target: 250, duration: '30s' },
        { target: 350, duration: '30s' },
        { target: 450, duration: '30s' },
        { target: 600, duration: '30s' },
        { target: 750, duration: '30s' },
        { target: 900, duration: '30s' },
        { target: 1050, duration: '30s' },
        { target: 1200, duration: '30s' },
        { target: 1400, duration: '30s' },
        { target: 1600, duration: '30s' },
        { target: 1800, duration: '30s' },
        { target: 2000, duration: '30s' },
      ],
    },
  },
};

const scenario = createScenario(constants, opts);
export const options = scenario.options;
export const setup = scenario.setup;
export default scenario.default;