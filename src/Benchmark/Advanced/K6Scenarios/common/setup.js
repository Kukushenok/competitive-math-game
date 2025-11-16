import http from 'k6/http';
import { sleep } from 'k6';

export function gsetp(num_comps, comp_delay) {
  console.log(`MY ${num_comps} AND ${comp_delay} ARE HERE`)
  sleep(15);
  console.log('Starting competition setup...');
  const competitions = [];
  
  for (let i = 0; i < num_comps; i++) {
    const minutes = i + 1;
    
    const competitionRes = http.post(`http://competitivebackend:8080/competition_ensurance/${minutes + 1}`);
    
    if (competitionRes.status == 200) {
      const competitionId = competitionRes.body.trim();
      const creationTime = Date.now();
      const expirationTime = creationTime + (minutes * 60 * 1000);
      
      competitions.push({
        id: competitionId,
        creationTime: creationTime,
        expirationTime: expirationTime,
        durationMinutes: minutes
      });
      
      console.log(`Created competition ${i + 1}/${num_comps}: ID=${competitionId}, duration=${minutes}min`);
    } else {
      console.log(`Failed to create competition ${i + 1}/${num_comps}: ${competitionRes.status}`);
    }
    
    if (i < num_comps - 1) {
      sleep(comp_delay);
    }
  }
  
  console.log(`Setup completed. Created ${competitions.length} competitions.`);
  return { competitions: competitions };
}