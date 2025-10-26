using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;

namespace CompetitiveBackend.Services.RewardDescriptionService
{
    public class GameProviderService : IGameProviderService
    {
        private readonly IRiddleRepository riddleRepository;
        private readonly IRiddleSettingsRepository riddleSettingsRepository;
        private readonly IRiddleSessionManager sessionManager;
        private readonly IPlayerParticipationRepository playerParticipationRepository;
        private readonly ICompetitionRepository competitionRepository;
        private readonly IRandom random;
        public GameProviderService(
            IRiddleRepository riddleRepository,
            IRiddleSettingsRepository settngs,
            IRiddleSessionManager mng,
            IPlayerParticipationRepository serv,
            ICompetitionRepository competitionRepository,
            IRandom rnd)
        {
            this.riddleRepository = riddleRepository;
            sessionManager = mng;
            playerParticipationRepository = serv;
            random = rnd;
            riddleSettingsRepository = settngs;
            this.competitionRepository = competitionRepository;
        }

        private UserRiddleInfo CastToUser(RiddleInfo info)
        {
            List<RiddleAnswer> answers = [.. info.PossibleAnswers];
            if (answers.Count > 0)
            {
                answers.Add(info.TrueAnswer);
                answers = [.. PickRandom(answers, answers.Count)];
            }

            return new UserRiddleInfo(info.Question, [.. answers]);
        }

        private IEnumerable<T> PickRandom<T>(List<T> info, int count)
        {
            info = [.. info];
            for (int i = 0; i < count && info.Count > 0; i++)
            {
                int idx = random.NextNumber(0, info.Count);
                yield return info[idx];
                info.RemoveAt(idx);
            }
        }

        public async Task<CompetitionParticipationTask> DoPlay(int accountID, int competitionID)
        {
            var riddles = (await riddleRepository.GetRiddles(competitionID, DataLimiter.NoLimit)).ToList();
            RiddleGameSettings settings = await riddleSettingsRepository.GetRiddleSettings(competitionID);
            var gameChosenRiddles = PickRandom(riddles, settings.TotalRiddles).ToList();
            var riddleGameInfo = new RiddleGameInfo(gameChosenRiddles, competitionID, DateTime.UtcNow);
            RiddleSession sess = await sessionManager.CreateSession(riddleGameInfo);

            return new CompetitionParticipationTask(
                sess.SessionID,
                [.. gameChosenRiddles.Select(CastToUser)]);
        }

        private async Task SubmitParticipation(int userID, int competitionID, int score)
        {
            PlayerParticipation? participation = null;
            Competition c = await competitionRepository.GetCompetition(competitionID);
            DateTime current = DateTime.UtcNow;
            if (current < c.StartDate || current > c.EndDate)
            {
                throw new ChronologicalException("Could not participate; competition " + (current > c.EndDate ? "has ended" : "is not started yet"));
            }

            try
            {
                participation = await playerParticipationRepository.GetParticipation(userID, competitionID);
            }
            catch (MissingDataException)
            {
                await playerParticipationRepository.CreateParticipation(new PlayerParticipation(competitionID, userID, score, current));
            }

            if (participation != null && participation.Score < score)
            {
                await playerParticipationRepository.UpdateParticipation(new PlayerParticipation(competitionID, userID, score, current));
            }
        }

        private static bool CompareAnswers(RiddleAnswer givenAnswer, RiddleAnswer expectedAnswer)
        {
            return givenAnswer.TextAnswer.Trim() == expectedAnswer.TextAnswer.Trim();
        }

        private static int CalculateBonus(RiddleGameInfo gameInfo, RiddleGameSettings settings)
        {
            double ratio = 0;
            if (settings.TimeLimit != null)
            {
                ratio = 1.0 - ((DateTime.UtcNow - gameInfo.StartTime).TotalSeconds / settings.TimeLimit.Value.TotalSeconds);
                if (ratio < 0)
                {
                    ratio = 0;
                }
            }

            return (int)Math.Round(settings.TimeLinearBonus * ratio);
        }

        private async Task<ParticipationFeedback> Calculate(RiddleGameInfo gameInfo, CompetitionParticipationRequest req)
        {
            RiddleGameSettings settings = await riddleSettingsRepository.GetRiddleSettings(gameInfo.CompetitionID);
            if (req.Answers.Count != gameInfo.Riddles.Count)
            {
                throw new GameSessionInvalidException("riddle count is not preserved");
            }

            int rightAnswCount = 0;
            for (int i = 0; i < req.Answers.Count; i++)
            {
                if (CompareAnswers(req.Answers[i], gameInfo.Riddles[i].TrueAnswer))
                {
                    rightAnswCount++;
                }
            }

            int totalCount = gameInfo.Riddles.Count;

            int score = CalculateBonus(gameInfo, settings);
            score += (rightAnswCount * settings.ScoreOnRightAnswer) + ((totalCount - rightAnswCount) * settings.ScoreOnBadAnswer);
            return new ParticipationFeedback(
                score,
                rightAnswCount,
                totalCount);
        }

        public async Task<ParticipationFeedback> DoSubmit(CompetitionParticipationRequest request)
        {
            RiddleSession sess = await sessionManager.RetrieveSession(request.SessionID);
            int compID = sess.GameInfo.CompetitionID;
            ParticipationFeedback x = await Calculate(sess.GameInfo, request);
            await SubmitParticipation(request.PlayerID, compID, x.Score);
            return x;
        }
    }
}
