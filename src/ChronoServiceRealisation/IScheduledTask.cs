namespace ChronoServiceRealisation
{
    public struct ScheduledTask
    {
        public DateTime PerformDate;
        public string Id;
    }
    public interface IScheduler
    {
        public delegate Task STFired(ScheduledTask task);
        public event STFired OnFired;
        public Task Schedule(ScheduledTask task);
    }
}
