using CompetitiveBackend.Core;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;

namespace CompetitiveBackend.Services.CompetitionService
{
    public class CompetitionService : BaseService<CompetitionService>, ICompetitionService
    {
        private readonly ICompetitionRepository _competitionRepository;
        public CompetitionService(ILogger<CompetitionService> logger, ICompetitionRepository competitionRepository) : base(logger)
        {
            _competitionRepository = competitionRepository;
        }

        public Task CreateCompetition(Competition c)
        {
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
            c = new Competition(name ?? c.Name,
                description ?? c.Description,
                startDate ?? c.StartDate,
                endDate ?? c.EndDate,
                c.Id);
            await _competitionRepository.UpdateCompetition(c);
        }
    }
}
