namespace CompetitiveBackend.Services.ExtraTools
{
    public interface ITimeScheduler
    {

        public delegate Task EventFiredEvent(TimeScheduledTaskData tsk);
        public Task AddOrUpdateScheduledTask(TimeScheduledTaskData task);
        public Task RemoveScheduledTask(TimeScheduledTaskData task);
        public void AddSubscriber(ITimeScheduledTaskSubscriber subscriber);
        public void RemoveSubscriber(ITimeScheduledTaskSubscriber subscriber);
    }
    public interface ITimeScheduledTaskSubscriber: IDisposable
    {
        public Task OnRecievedMessage(TimeScheduledTaskData data);
    }
}