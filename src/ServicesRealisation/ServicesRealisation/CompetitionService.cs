using CompetitiveBackend.Core;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Exceptions;
using Microsoft.Extensions.Logging;

namespace CompetitiveBackend.Services.CompetitionService
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ICompetitionRepository _competitionRepository;
        public CompetitionService(ICompetitionRepository competitionRepository)
        {
            _competitionRepository = competitionRepository;
        }

        public Task CreateCompetition(Competition c)
        {
            if (c.StartDate >= c.EndDate) throw new InvalidArgumentsException("Start cannot be after end");
            if (c.StartDate < DateTime.Now) throw new InvalidArgumentsException("Competition should be started in the future");
            return _competitionRepository.CreateCompetition(c);
        }

        public async Task<IEnumerable<Competition>> GetActiveCompetitions()
        {
            return await _competitionRepository.GetActiveCompetitions();
        }

        public async Task<IEnumerable<Competition>> GetAllCompetitions(DataLimiter dataLimiter)
        {
            return await _competitionRepository.GetAllCompetitions(dataLimiter);
        }

        public async Task<Competition> GetCompetition(int competitionID)
        {
            return await _competitionRepository.GetCompetition(competitionID);
        }

        public async Task<LargeData> GetCompetitionLevel(int competitionID)
        {
            return await _competitionRepository.GetCompetitionLevel(competitionID);
        }

        public async Task SetCompetitionLevel(int competitionID, LargeData levelData)
        {
            await _competitionRepository.SetCompetitionLevel(competitionID, levelData);
        }

        public async Task UpdateCompetition(int id, string? name, string? description, DateTime? startDate, DateTime? endDate)
        {
            Competition c = await _competitionRepository.GetCompetition(id);
            DateTime dt = DateTime.Now;
            bool isStarted = c.StartDate < dt;
            bool isEnded = c.EndDate < dt;
            c = new Competition(name ?? c.Name,
                description ?? c.Description,
                startDate ?? c.StartDate,
                endDate ?? c.EndDate,
                c.Id);
            if (c.StartDate >= c.EndDate) throw new InvalidArgumentsException("Start cannot be after end");
            if (isStarted && c.StartDate >= dt) throw new InvalidArgumentsException("Competition cannot be delayed after its start.");
            if (isEnded && c.EndDate >= dt) throw new InvalidArgumentsException("Competition cannot be prolonged after its end. Create a new one!");
            await _competitionRepository.UpdateCompetition(c);
        }
    }
}
