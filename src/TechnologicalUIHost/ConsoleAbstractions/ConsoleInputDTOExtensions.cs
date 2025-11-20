using CompetitiveBackend.BackendUsage.Objects;

namespace TechnologicalUIHost.ConsoleAbstractions
{
    internal static class ConsoleInputDTOExtensions
    {
        public static AccountCreationDTO ReadAccountCreation(this IConsoleInput input, string promt = "> ")
        {
            return new AccountCreationDTO(input.PromtInput($"Логин {promt}"), input.PromtInput($"Пароль {promt}"), input.PromtInput($"Почта {promt}"));
        }

        public static AccountLoginDTO ReadAccountLogin(this IConsoleInput input, string promt = "> ")
        {
            return new AccountLoginDTO(input.PromtInput($"Логин {promt}"), input.PromtInput($"Пароль {promt}"));
        }

        public static CompetitionDTO ReadCompetitionDTO(this IConsoleInput input, string prompt = "> ")
        {
            string? name = input.PromtInput($"Name {prompt}");
            name = string.IsNullOrWhiteSpace(name) ? null : name;

            string? description = input.PromtInput($"Description {prompt}");
            description = string.IsNullOrWhiteSpace(description) ? null : description;

            DateTime startDate = input.ReadDateTime($"Start Date {prompt}");
            DateTime endDate = input.ReadDateTime($"End Date {prompt}");

            return new CompetitionDTO(null, name, description, startDate, endDate);
        }

        public static CompetitionPatchRequestDTO ReadCompetitionUpdateRequestDTO(this IConsoleInput input, string prompt = "> ")
        {
            int id = input.ReadInt($"Введите ID соревнования {prompt}");
            string? name = input.PromtInput($"Имя (если нужно изменить) {prompt}");
            name = string.IsNullOrWhiteSpace(name) ? null : name;

            string? description = input.PromtInput($"Описание (если нужно изменить) {prompt}");
            description = string.IsNullOrWhiteSpace(description) ? null : description;

            DateTime? startDate = input.ReadNullableDateTime($"Дата начала (если нужно изменить) {prompt}");
            DateTime? endDate = input.ReadNullableDateTime($"Дата конца (если нужно изменить) {prompt}");

            return new CompetitionPatchRequestDTO(id, name, description, startDate, endDate);
        }

        public static LevelDataInfoDTO ReadLevelDataInfoDTOWithoutID(this IConsoleInput input, string prompt)
        {
            int id = input.ReadInt($"Введите ID соревнования {prompt}");
            string place = input.PromtInput($"Введите целевую платформу уровня {prompt}");
            int version = input.ReadInt($"Введите код версии уровня {prompt}");
            return new LevelDataInfoDTO(id, place, version, null);
        }

        public static LevelDataInfoDTO ReadLevelDataInfoDTO(this IConsoleInput input, string prompt)
        {
            int id = input.ReadInt($"Введите ID уровня {prompt}");
            LevelDataInfoDTO res = input.ReadLevelDataInfoDTOWithoutID(prompt);
            res.ID = id;
            return res;
        }

        public static UpdateCompetitionRewardDTO ReadUpdateCompetitionRewardDTO(this IConsoleInput input, string prompt = "> ")
        {
            int? id = input.ReadNullableInt($"ID: {prompt}");
            int rewardDescriptionID = input.ReadInt($"ID награды {prompt}");
            int choice = input.ReadInt($"Выберите тип условия\n1. По рангу\n2. По месту\nВаш выбор {prompt}");

            RankRewardConditionDTO? rankCondition = null;
            PlaceRewardConditionDTO? placeCondition = null;

            if (choice == 1)
            {
                float minRank = input.ReadFloat($"Мин. ранг {prompt}");
                float maxRank = input.ReadFloat($"Макс. ранг {prompt}");
                rankCondition = new RankRewardConditionDTO(minRank, maxRank);
            }
            else if (choice == 2)
            {
                int minPlace = input.ReadInt($"Мин. место {prompt}");
                int maxPlace = input.ReadInt($"Макс. место {prompt}");
                placeCondition = new PlaceRewardConditionDTO(minPlace, maxPlace);
            }
            else
            {
                throw new FormatException("Неправильный ConditionType");
            }

            return new UpdateCompetitionRewardDTO(id, rewardDescriptionID, rankCondition, placeCondition);
        }

        public static CreateCompetitionRewardDTO ReadCreateCompetitionRewardDO(this IConsoleInput input, string promt = "> ")
        {
            int compID = input.ReadInt("Введите ID соревнования: ");
            UpdateCompetitionRewardDTO dto = input.ReadUpdateCompetitionRewardDTO(promt);
            return new CreateCompetitionRewardDTO(dto.ID, dto.RewardDescriptionID, compID, dto.ConditionByRank, dto.ConditionByPlace);
        }

        public static DataLimiterDTO ReadDataLimiterDTO(this IConsoleInput input, string prompt = "> ")
        {
            int page = (input.ReadNullableInt($"Номер страницы {prompt}") ?? 1) - 1;
            int count = input.ReadNullableInt($"Количество {prompt}") ?? 0;
            return new DataLimiterDTO(page, count);
        }

        public static LargeDataDTO ReadLargeDataDTO(this IConsoleInput input, string prompt = "> ")
        {
            return new LargeDataDTO(input.ReadData(prompt));
        }

        public static PlayerParticipationDTO ReadPlayerParticipationDTO(this IConsoleInput input, string prompt = "> ")
        {
            int accountID = input.ReadInt($"ID аккаунта {prompt}");
            int competitionID = input.ReadInt($"ID соревнования {prompt}");
            int score = input.ReadInt($"Очки {prompt}");
            return new PlayerParticipationDTO(accountID, competitionID, score, DateTime.UtcNow);
        }

        public static PlayerProfileDTO ReadPlayerProfileDTO(this IConsoleInput input, string prompt = "> ")
        {
            string? name = input.PromtInput($"Имя {prompt}");
            name = string.IsNullOrWhiteSpace(name) ? null : name;

            string? description = input.PromtInput($"Описание {prompt}");
            description = string.IsNullOrWhiteSpace(description) ? null : description;

            return new PlayerProfileDTO(name, description, null);
        }

        public static RewardDescriptionDTO ReadRewardDescriptionDTO(this IConsoleInput input, string prompt = "> ")
        {
            string name = input.PromtInput($"Название награды {prompt}");
            string description = input.PromtInput($"Описание награды {prompt}");
            return new RewardDescriptionDTO(name, description, null);
        }

        public static RiddleGameSettingsDTO ReadRiddleGameSettingsDTO(this IConsoleInput input, string prompt = "> ")
        {
            int count = input.ReadInt($"Кол-во заданий {prompt}");
            int pScore = input.ReadInt($"Кол-во очков за правильный ответ {prompt}");
            int fScore = input.ReadInt($"Кол-во очков за неправильный ответ {prompt}");
            TimeSpan? totalTime = input.ReadNullableTimeSpan($"Время игры (если предусматривается) {prompt}");
            int timeScore = input.ReadInt($"Очки за время {prompt}");
            return new RiddleGameSettingsDTO(pScore, fScore, count, totalTime, timeScore);
        }

        public static RiddleInfoDTO ReadRiddleInfoDTO(this IConsoleInput input, string promt = "> ")
        {
            int? iD = input.ReadNullableInt($"ID {promt}");
            int compID = input.ReadInt($"ID соревнования: {promt}");
            string question = input.PromtInput($"Вопрос {promt}");
            string answer = input.PromtInput($"Ответ {promt}");
            string? otherAnswer = null;
            List<string> otherAnswers = [];
            do
            {
                otherAnswer = input.PromtInput($"+ Альтернативный ответ (возможно скип) {promt}");
                otherAnswer = string.IsNullOrWhiteSpace(otherAnswer) ? null : otherAnswer;
                if (otherAnswer != null)
                {
                    otherAnswers.Add(otherAnswer);
                }
            }
            while (otherAnswer != null);
            return new RiddleInfoDTO(
                compID,
                question,
                [.. from s in otherAnswers select new RiddleAnswerDTO(s)],
                new RiddleAnswerDTO(answer),
                iD);
        }
    }
}