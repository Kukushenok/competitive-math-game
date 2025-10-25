using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;

namespace CompetitiveBackend.BaseUsage.Converters
{
    internal static class RewardDescriptionDTOConverter
    {
        public static RewardDescription Convert(this RewardDescriptionDTO dto)
        {
            return new RewardDescription(dto.Name, dto.Description, dto.ID);
        }

        public static RewardDescriptionDTO Convert(this RewardDescription descr)
        {
            return new RewardDescriptionDTO(descr.Name, descr.Description, descr.Id);
        }
    }

    internal static class CompetitionParticipationTaskDTOConverter
    {
        public static CompetitionParticipationTaskDTO Convert(this CompetitionParticipationTask task)
        {
            return new CompetitionParticipationTaskDTO(
                task.SessionID,
                [.. from r in task.Riddles select r.Convert()]);
        }

        public static UserRiddleInfoDTO Convert(this UserRiddleInfo riddleInfo)
        {
            return new UserRiddleInfoDTO(
                riddleInfo.Question,
                [.. from r in riddleInfo.AvailableAnswers select r.Convert()]);
        }

        public static RiddleAnswerDTO Convert(this RiddleAnswer riddleAnswer)
        {
            return new RiddleAnswerDTO(riddleAnswer.TextAnswer);
        }

        public static RiddleAnswer Convert(this RiddleAnswerDTO riddleAnswer)
        {
            return new RiddleAnswer(riddleAnswer.TextAnswer);
        }

        public static CompetitionParticipationRequestDTO Convert(this CompetitionParticipationRequest request)
        {
            return new CompetitionParticipationRequestDTO(request.SessionID, [.. from r in request.Answers select r.Convert()]);
        }

        public static CompetitionParticipationRequest Convert(this CompetitionParticipationRequestDTO request, int playerID)
        {
            return new CompetitionParticipationRequest(playerID, request.SessionID, [.. from r in request.Answers select r.Convert()]);
        }

        public static ParticipationFeedbackDTO Convert(this ParticipationFeedback feedback)
        {
            return new ParticipationFeedbackDTO(feedback.Score, feedback.RightAnswersCount, feedback.TotalAnswersCount);
        }

        public static RiddleInfoDTO Convert(this RiddleInfo info)
        {
            return new RiddleInfoDTO(info.CompetitionID, info.Question, [.. from r in info.PossibleAnswers select r.Convert()], info.TrueAnswer.Convert(), info.Id);
        }

        public static RiddleInfo Convert(this RiddleInfoDTO info)
        {
            return new RiddleInfo(info.CompetitionID, info.Question, [.. from r in info.PossibleAnswers select r.Convert()], info.TrueAnswer.Convert(), info.ID);
        }

        public static RiddleGameSettingsDTO Convert(this RiddleGameSettings settings)
        {
            return new RiddleGameSettingsDTO(settings.ScoreOnRightAnswer, settings.ScoreOnBadAnswer, settings.TotalRiddles, settings.TimeLimit, settings.TimeLinearBonus);
        }

        public static RiddleGameSettings Convert(this RiddleGameSettingsDTO settings)
        {
            return new RiddleGameSettings(settings.ScoreOnRightAnswer, settings.ScoreOnBadAnswer, settings.TotalRiddles, settings.TimeLimit, settings.TimeLinearBonus);
        }
    }
}