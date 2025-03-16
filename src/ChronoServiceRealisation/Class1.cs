using Quartz;

namespace ChronoServiceRealisation
{
    public class Class1
    {
        public void DoSomething(DateTime time)
        {
            TriggerBuilder.Create()
                            .WithIdentity("hello")
                            .StartAt(time)
                            .Build();
        }
    }
}
