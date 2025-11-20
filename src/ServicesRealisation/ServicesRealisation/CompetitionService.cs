using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Exceptions;
using ServicesRealisation.ServicesRealisation.Validator;

namespace CompetitiveBackend.Services.CompetitionService
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ICompetitionRepository competitionRepository;
        private readonly IValidator<Competition> validator;
        private readonly ICompetitionRewardScheduler scheduler;
        public CompetitionService(
            ICompetitionRepository competitionRepository,
            IValidator<Competition> validator,
            ICompetitionRewardScheduler scheduler)
        {
            this.competitionRepository = competitionRepository;
            this.validator = validator;
            this.scheduler = scheduler;
        }

        public async Task CreateCompetition(Competition competition)
        {
            if (!validator.IsValid(competition, out string? msg))
            {
                throw new InvalidArgumentsException(msg!);
            }

            if (competition.StartDate < DateTime.Now)
            {
                throw new ChronologicalException("Competition should be started in the future");
            }

            int idx = await competitionRepository.CreateCompetition(competition);
            await scheduler.OnCompetitionCreated(new Competition(competition.Name, competition.Description, competition.StartDate, competition.EndDate, idx));
        }

        public async Task<IEnumerable<Competition>> GetActiveCompetitions()
        {
            return await competitionRepository.GetActiveCompetitions();
        }

        public async Task<IEnumerable<Competition>> GetAllCompetitions(DataLimiter dataLimiter)
        {
            return await competitionRepository.GetAllCompetitions(dataLimiter);
        }

        public async Task<Competition> GetCompetition(int competitionID)
        {
            return await competitionRepository.GetCompetition(competitionID);
        }

        public async Task UpdateCompetition(int id, string? name, string? description, DateTime? startDate, DateTime? endDate)
        {
            Competition c = await competitionRepository.GetCompetition(id);
            DateTime dt = DateTime.Now;
            bool isStarted = c.StartDate < dt;
            bool isEnded = c.EndDate < dt;
            c = new Competition(
                name ?? c.Name,
                description ?? c.Description,
                startDate ?? c.StartDate,
                endDate ?? c.EndDate,
                c.Id);
            if (!validator.IsValid(c, out string? msg))
            {
                throw new InvalidArgumentsException(msg!);
            }

            if (isStarted && c.StartDate >= dt)
            {
                throw new ChronologicalException("Competition cannot be delayed after its start.");
            }

            if (isEnded && c.EndDate >= dt)
            {
                throw new ChronologicalException("Competition cannot be prolonged after its end.");
            }

            await competitionRepository.UpdateCompetition(c);
            await scheduler.OnCompetitionUpdated(c);
        }
    }
}
