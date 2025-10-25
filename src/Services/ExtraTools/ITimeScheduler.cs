namespace CompetitiveBackend.Services.ExtraTools
{
    public interface ITimeScheduler
    {
        private delegate Task EventFiredEvent(TimeScheduledTaskData tsk);
        Task AddOrUpdateScheduledTask(TimeScheduledTaskData task);
        Task RemoveScheduledTask(TimeScheduledTaskData task);
        void AddSubscriber(ITimeScheduledTaskSubscriber subscriber);
        void RemoveSubscriber(ITimeScheduledTaskSubscriber subscriber);
        Task Initialize();
    }

    public interface ITimeScheduledTaskSubscriber : IDisposable
    {
        Task OnRecievedMessage(TimeScheduledTaskData data);
    }
}