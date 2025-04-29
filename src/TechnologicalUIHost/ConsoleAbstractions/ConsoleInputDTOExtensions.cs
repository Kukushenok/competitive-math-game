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

        public static CompetitionUpdateRequestDTO ReadCompetitionUpdateRequestDTO(this IConsoleInput input, string prompt = "> ")
        {
            int id = input.ReadInt($"ID {prompt}");
            string? name = input.PromtInput($"Имя (если нужно изменить) {prompt}");
            name = string.IsNullOrWhiteSpace(name) ? null : name;

            string? description = input.PromtInput($"Описание (если нужно изменить) {prompt}");
            description = string.IsNullOrWhiteSpace(description) ? null : description;

            DateTime? startDate = input.ReadNullableDateTime($"Дата начала (если нужно изменить) {prompt}");
            DateTime? endDate = input.ReadNullableDateTime($"Дата конца (если нужно изменить) {prompt}");

            return new CompetitionUpdateRequestDTO(id, name, description, startDate, endDate);
        }

        public static UpdateCompetitionRewardDTO ReadUpdateCompetitionRewardDTO(this IConsoleInput input, string prompt = "> ")
        {
            int? id = input.ReadNullableInt($"ID (optional) {prompt}");
            int rewardDescriptionID = input.ReadInt($"Reward Description ID {prompt}");
            int choice = input.ReadInt($"Выберите тип условия\n1. По рангу\n2. По месту\nВаш выбор {prompt}");

            RankRewardConditionDTO? rankCondition = null;
            PlaceRewardConditionDTO? placeCondition = null;

            if (choice == 1)
            {
                float minRank = input.ReadFloat($"Min Rank {prompt}");
                float maxRank = input.ReadFloat($"Max Rank {prompt}");
                rankCondition = new RankRewardConditionDTO(minRank, maxRank);
            }
            else if (choice == 2)
            {
                int minPlace = input.ReadInt($"Min Place {prompt}");
                int maxPlace = input.ReadInt($"Max Place {prompt}");
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

        public static PlayerRewardDTO ReadPlayerRewardDTO(this IConsoleInput input, string prompt = "> ")
        {
            int playerID = input.ReadInt($"ID игрока {prompt}");
            int rewardDescriptionID = input.ReadInt($"Reward Description ID {prompt}");
            int? grantedCompetitionID = input.ReadNullableInt($"Granted Competition ID (optional) {prompt}");
            DateTime? grantDate = input.ReadNullableDateTime($"Grant Date (optional) {prompt}");
            string? name = input.PromtInput($"Name (optional) {prompt}");
            name = string.IsNullOrWhiteSpace(name) ? null : name;

            string? description = input.PromtInput($"Description (optional) {prompt}");
            description = string.IsNullOrWhiteSpace(description) ? null : description;

            int? id = input.ReadNullableInt($"ID (optional) {prompt}");

            return new PlayerRewardDTO(playerID, rewardDescriptionID, grantedCompetitionID, grantDate, name, description, id);
        }

        public static RewardDescriptionDTO ReadRewardDescriptionDTO(this IConsoleInput input, string prompt = "> ")
        {
            string name = input.PromtInput($"Название награды {prompt}");
            string description = input.PromtInput($"Описание награды {prompt}");
            return new RewardDescriptionDTO(name, description, null);
        }
    }
}