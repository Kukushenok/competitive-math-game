using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using ServicesRealisation.ServicesRealisation.Validator;

namespace CompetitiveBackend.Services.CompetitionService
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IValidator<Competition> _validator;
        private readonly ICompetitionRewardScheduler _scheduler;
        public CompetitionService(ICompetitionRepository competitionRepository,
                                  IValidator<Competition> validator,
                                  ICompetitionRewardScheduler scheduler)
        {
            _competitionRepository = competitionRepository;
            _validator = validator;
            _scheduler = scheduler;
        }

        public async Task CreateCompetition(Competition c)
        {
            if (!_validator.IsValid(c, out string? msg)) throw new InvalidArgumentsException(msg!);
            if (c.StartDate < DateTime.Now) throw new ChronologicalException("Competition should be started in the future");
            int idx = await _competitionRepository.CreateCompetition(c);
            await _scheduler.OnCompetitionCreated(new Competition(c.Name, c.Description, c.StartDate, c.EndDate, idx));
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
            if (!_validator.IsValid(c, out string? msg)) throw new InvalidArgumentsException(msg!);
            if (isStarted && c.StartDate >= dt) throw new ChronologicalException("Competition cannot be delayed after its start.");
            if (isEnded && c.EndDate >= dt) throw new ChronologicalException("Competition cannot be prolonged after its end.");
            await _competitionRepository.UpdateCompetition(c);
            await _scheduler.OnCompetitionUpdated(c);
        }
    }
}
