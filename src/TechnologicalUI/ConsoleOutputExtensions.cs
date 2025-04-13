using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnologicalUI
{
    public static class ConsoleOutputExtensions
    {
        public static void PrintIfNotNull<T>(this IConsoleOutput output, string label, T? value) where T : struct
        {
            if (value.HasValue)
                output.PromtOutput($"{label}: {value.Value}");
            else output.PromtOutput($"{label}: <отсутствует>");
        }

        public static void PrintIfNotNull(this IConsoleOutput output, string label, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                output.PromtOutput($"{label}: {value}");
            else output.PromtOutput($"{label}: <отсутствует>");
        }
        public static void Print(this IConsoleOutput output, string label, DateTime? dt)
        {
            TimeZoneInfo timeInfo = TimeZoneInfo.Local;
            if(dt != null) dt = TimeZoneInfo.ConvertTimeFromUtc(dt!.Value, timeInfo);
            output.PrintIfNotNull(label,dt?.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
        }
    }

    internal static class ConsoleOutputDTOExtensions
    {
        public static void Print(this IConsoleOutput output, IntIdentifiableDTO dto)
        {
            output.PrintIfNotNull("Идентификатор", dto.ID);
        }

        public static void Print(this IConsoleOutput output, AccountLoginDTO dto)
        {
            output.PromtOutput("=== Данные для входа ===");
            output.PrintIfNotNull("Логин", dto.Login);
            output.PrintIfNotNull("Пароль", "[скрыто]");
        }

        public static void Print(this IConsoleOutput output, AuthSuccessResultDTO dto)
        {
            output.PromtOutput("=== Успешная аутентификация ===");
            output.PrintIfNotNull("Токен", dto.Token);
            output.PrintIfNotNull("Роль", dto.RoleName);
            output.PromtOutput($"ID аккаунта: {dto.AccountID}");
        }

        public static void Print(this IConsoleOutput output, CompetitionDTO dto)
        {
            output.PromtOutput("=== Соревнование ===");
            output.Print((IntIdentifiableDTO)dto);
            output.PrintIfNotNull("Название", dto.Name);
            output.PrintIfNotNull("Описание", dto.Description);
            output.Print("Дата начала", dto.StartDate);
            output.Print("Дата окончания", dto.EndDate);
        }

        public static void Print(this IConsoleOutput output, UpdateCompetitionRewardDTO dto)
        {
            output.PromtOutput("=== Условия награды ===");
            output.Print((IntIdentifiableDTO)dto);
            output.PromtOutput($"ID описания награды: {dto.RewardDescriptionID}");

            if (dto.ConditionByRank != null)
            {
                output.PromtOutput("Условие по рангу:");
                output.PromtOutput($"  Минимальный ранг: {dto.ConditionByRank.MinRank}");
                output.PromtOutput($"  Максимальный ранг: {dto.ConditionByRank.MaxRank}");
            }

            if (dto.ConditionByPlace != null)
            {
                output.PromtOutput("Условие по месту:");
                output.PromtOutput($"  Минимальное место: {dto.ConditionByPlace.MinPlace}");
                output.PromtOutput($"  Максимальное место: {dto.ConditionByPlace.MaxPlace}");
            }
        }

        public static void Print(this IConsoleOutput output, LargeDataDTO dto)
        {
            output.PromtOutput("=== Большие бинарные данные ===");
            output.PromtOutput($"Размер данных: {dto.Data.Length} байт");
            output.SaveData(dto.Data, "");
        }

        public static void Print(this IConsoleOutput output, PlayerProfileDTO dto)
        {
            output.PromtOutput("=== Профиль игрока ===");
            output.Print((IntIdentifiableDTO)dto);
            output.PrintIfNotNull("Имя", dto.Name);
            output.PrintIfNotNull("Описание", dto.Description);
        }

        public static void Print(this IConsoleOutput output, RewardDescriptionDTO dto)
        {
            output.PromtOutput("=== Описание награды ===");
            output.Print((IntIdentifiableDTO)dto);
            output.PrintIfNotNull("Название", dto.Name);
            output.PrintIfNotNull("Описание", dto.Description);
        }

        public static void Print(this IConsoleOutput output, DataLimiterDTO dto)
        {
            output.PromtOutput("=== Настройки пагинации ===");
            output.PromtOutput($"Страница: {dto.Page}");
            output.PromtOutput($"Количество записей: {dto.Count}");
        }

        public static void Print(this IConsoleOutput output, PlayerRewardDTO dto)
        {
            output.PromtOutput("=== Награда игрока ===");
            output.Print((IntIdentifiableDTO)dto);
            output.PromtOutput($"ID игрока: {dto.PlayerID}");
            output.PromtOutput($"ID описания награды: {dto.RewardDescriptionID}");
            output.PrintIfNotNull("ID соревнования", dto.GrantedCompetitionID);
            output.Print("Дата выдачи", dto.GrantDate);
            output.PrintIfNotNull("Название", dto.Name);
            output.PrintIfNotNull("Описание", dto.Description);
        }
        public static void Print(this IConsoleOutput output, PlayerParticipationDTO participation)
        {
            output.PromtOutput($"Игрок\t{participation.AccountID,5}\tСорев.\t{participation.Competition,5}\tОчки\t{participation.Score,5}");
        }
        public static void PrintEnumerable<T>(this IConsoleOutput output, IEnumerable<T> result, Action<IConsoleOutput, T> printCmd)
        {
            T[] results = result.ToArray();
            if (results.Length == 0) output.PromtOutput("Список пуст!");
            else
            {
                foreach (T val in results) printCmd(output, val);
            }
        }
    }
}
