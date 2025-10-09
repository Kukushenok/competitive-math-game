using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.MenuCommand;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Commands
{
    class GameParticipationUseCaseCommands : AuthRequiringCommands
    {
        private IGamePlayUseCase _gameplay;

        public GameParticipationUseCaseCommands(IGamePlayUseCase gameplay, IAuthCache cache) : base("Сыграть в игру", cache)
        {
            _gameplay = gameplay;
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Начать игру", TaskDecorator.Sync(Play), IsAuthed);
        }

        private async Task Play(IConsole console)
        {
            using var g = await Auth(_gameplay);
            int compID = console.ReadInt("Введите ID соревнования: ");
            var gameplay = await g.DoPlay(compID);
            var answers = new List<RiddleAnswerDTO>();
            int r = 0;
            int total = answers.Count;
            foreach (var p in gameplay.Riddles)
            {
                console.PromtOutput($"ВОПРОС {r}/{total}: {p.Question}");
                string answer = "";
                if (p.AvailableAnswers.Length == 0)
                {
                    answer = console.PromtInput("Введите ответ: ");
                }
                else
                {
                    for(int j = 0; j < p.AvailableAnswers.Length; j++)
                    {
                        console.PromtOutput($"{j + 1}: {p.AvailableAnswers[j].TextAnswer}");
                    }
                    int t = console.ReadInt("Введите индекс ответа: ") - 1;
                    if (t < 0) t = 0;
                    t %= p.AvailableAnswers.Length;
                    answer = p.AvailableAnswers[t].TextAnswer;
                }
                answers.Add(new RiddleAnswerDTO(answer));
            }
            CompetitionParticipationRequestDTO dto = new CompetitionParticipationRequestDTO(
                0,
                gameplay.SessionID,
                answers
                );
            var result = await g.DoSubmit(dto);
            console.PromtOutput($"КОЛ-ВО ОЧКОВ: {result.Score}");
            console.PromtOutput($"СТАТИСТИКА ОТВЕТОВ: {result.RightAnswersCount}/{result.TotalAnswersCount}");
        }
    }
    class GameManagementUseCaseCommands : AuthRequiringCommands
    {
        private IGameManagementUseCase _gameplay;

        public GameManagementUseCaseCommands(IGameManagementUseCase gameplay, IAuthCache cache) : base("Настроить игру", cache)
        {
            _gameplay = gameplay;
        }
        private async Task GetRiddles(IConsole console)
        {
            using var x = await Auth(_gameplay);
            int idx = console.ReadInt("Введите ID соревнования: ");
            foreach(var riddle in await x.GetRiddles(idx, console.ReadDataLimiterDTO()))
            {
                console.Print(riddle);
            }
        }
        private async Task GetSettings(IConsole console)
        {
            using var x = await Auth(_gameplay);
            int idx = console.ReadInt("Введите ID соревнования: ");
            var settings = await x.GetSettings(idx);
            console.Print(settings);
        }
        private async Task UpdateSettings(IConsole console)
        {
            using var x = await Auth(_gameplay);
            int idx = console.ReadInt("Введите ID соревнования: ");
            var p = console.ReadRiddleGameSettingsDTO();
            await x.UpdateSettings(idx, p);
        }
        private async Task DeleteRiddle(IConsole console)
        {
            using var x = await Auth(_gameplay);
            int d = console.ReadInt("Введите ID загадки:");
            await x.DeleteRiddle(d);
        }
        private async Task CreateRiddle(IConsole console)
        {
            using var x = await Auth(_gameplay);
            await x.CreateRiddle(console.ReadRiddleInfoDTO());
        }
        private async Task UpdateRiddle(IConsole console)
        {
            using var x = await Auth(_gameplay);
            await x.UpdateRiddle(console.ReadRiddleInfoDTO());
        }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Задать настройки", TaskDecorator.Sync(UpdateSettings), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Получить настройки", TaskDecorator.Sync(GetSettings), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Добавить загадку", TaskDecorator.Sync(CreateRiddle), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Обновить загадку", TaskDecorator.Sync(UpdateRiddle), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Получить все загадки", TaskDecorator.Sync(GetRiddles), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Удалить загадку", TaskDecorator.Sync(DeleteRiddle), IsAuthed);
        }
    }
}
