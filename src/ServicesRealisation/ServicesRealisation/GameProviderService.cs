using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.AspNetCore.Server.HttpSys;

namespace CompetitiveBackend.Services.RewardDescriptionService
{
    public class GameProviderService : IGameProviderService
    {
        private IRiddleRepository riddleRepository;
        private IRiddleSettingsRepository riddleSettingsRepository;
        private IRiddleSessionManager sessionManager;
        private IPlayerParticipationRepository _playerParticipationRepository;
        private ICompetitionRepository _competitionRepository;
        private IRandom random;
        public GameProviderService(IRiddleRepository riddleRepository, 
            IRiddleSettingsRepository settngs, IRiddleSessionManager mng, 
            IPlayerParticipationRepository serv, ICompetitionRepository competitionRepository,
            IRandom rnd)
        {
            this.riddleRepository = riddleRepository;
            sessionManager = mng;
            _playerParticipationRepository = serv;
            random = rnd;
            riddleSettingsRepository = settngs;
            _competitionRepository = competitionRepository;
        }
        private UserRiddleInfo CastToUser(RiddleInfo info)
        {
            List<RiddleAnswer> answers = new List<RiddleAnswer>(info.PossibleAnswers);
            if(answers.Count > 0)
            {
                answers.Add(info.TrueAnswer);
                answers = PickRandom(answers,answers.Count).ToList();
            }
            return new UserRiddleInfo(info.Question, answers.ToArray());
        }
        private IEnumerable<T> PickRandom<T>(List<T> info, int count)
        {
            info = [.. info];
            for (int i = 0; i < count && info.Count > 0; i++)
            {
                var idx = random.Next(0, info.Count);
                yield return info[idx];
                info.RemoveAt(idx);
            }
        }
        public async Task<CompetitionParticipationTask> DoPlay(int accountID, int competitionID)
        {
            var riddles = (await riddleRepository.GetRiddles(competitionID, DataLimiter.NoLimit)).ToList();
            var settings = await riddleSettingsRepository.GetRiddleSettings(competitionID);
            var gameChosenRiddles = PickRandom(riddles, settings.TotalRiddles).ToList();
            var riddleGameInfo = new RiddleGameInfo(gameChosenRiddles, competitionID, DateTime.UtcNow);
            var sess = await sessionManager.CreateSession(riddleGameInfo);
            
            return new CompetitionParticipationTask(
                sess.SessionID,
                gameChosenRiddles.Select(CastToUser).ToList()
                );
        }
        private async Task SubmitParticipation(int userID, int competitionID, int score)
        {
            PlayerParticipation? participation = null;
            Competition c = await _competitionRepository.GetCompetition(competitionID);
            DateTime current = DateTime.UtcNow;
            if (current < c.StartDate || current > c.EndDate)
            {
                throw new ChronologicalException("Could not participate; competition " + (current > c.EndDate ? "has ended" : "is not started yet"));
            }
            try
            {
                participation = await _playerParticipationRepository.GetParticipation(userID, competitionID);
            }
            catch (MissingDataException)
            {
                await _playerParticipationRepository.CreateParticipation(new PlayerParticipation(competitionID, userID, score, current));
            }
            if (participation != null && participation.Score < score)
            {
                await _playerParticipationRepository.UpdateParticipation(new PlayerParticipation(competitionID, userID, score, current));
            }
        }
        private bool CompareAnswers(RiddleAnswer givenAnswer, RiddleAnswer expectedAnswer)
        {
            return givenAnswer.TextAnswer.Trim() == expectedAnswer.TextAnswer.Trim();
        }
        private async Task<ParticipationFeedback> Calculate(RiddleGameInfo gameInfo, CompetitionParticipationRequest req)
        {
            var settings = await riddleSettingsRepository.GetRiddleSettings(gameInfo.CompetitionID);
            if(req.Answers.Count != gameInfo.Riddles.Count)
            {
                throw new GameSessionInvalidException("riddle count is not preserved");
            }
            int rightAnswCount = 0;
            for(int i = 0; i < req.Answers.Count; i++)
            {
                if (CompareAnswers(req.Answers[i], gameInfo.Riddles[i].TrueAnswer)) rightAnswCount++;
            }
            int totalCount = gameInfo.Riddles.Count;
            double ratio = (DateTime.UtcNow - gameInfo.StartTime).TotalSeconds / settings.TimeLimit.Second;
            int score = (int)(Math.Round(settings.TimeLinearBonus * ratio));
            score += rightAnswCount * settings.ScoreOnRightAnswer + (totalCount - rightAnswCount) * settings.ScoreOnBadAnswer;
            return new ParticipationFeedback(
                score,
                rightAnswCount,
                totalCount
                );
        }
        public async Task<ParticipationFeedback> DoSubmit(CompetitionParticipationRequest request)
        {
            var sess = await sessionManager.RetrieveSession(request);
            int compID = sess.GameInfo.CompetitionID;
            var x = await Calculate(sess.GameInfo, request);
            await SubmitParticipation(request.PlayerID, compID, x.Score);
            return x;
        }
    }
}
