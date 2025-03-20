using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Collections;

namespace ChronoServiceRealisation
{

    internal class QuartzTimeScheduler: ITimeScheduler
    {
        public class QuartzSimpleJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                QuartzTimeScheduler sc = (context.Scheduler.Context[nameof(QuartzTimeScheduler)] as QuartzTimeScheduler)!;
                TimeScheduledTaskData task = (TimeScheduledTaskData)context.Get(nameof(TimeScheduledTaskData))!;
                await sc?.FireEvent(task)!;
            }
        }
        private List<ITimeScheduledTaskSubscriber> _subscribers;
        ILogger logger;
        IScheduler CoreScheduler;
        public QuartzTimeScheduler(IScheduler sched, ILogger<QuartzTimeScheduler> logger)
        {
            CoreScheduler = sched;
            this.logger = logger;
            _subscribers = new List<ITimeScheduledTaskSubscriber>();
            CoreScheduler.Context.Add(nameof(QuartzTimeScheduler), this);
        }
        public async Task AddOrUpdateScheduledTask(TimeScheduledTaskData task)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {nameof(TimeScheduledTaskData), task}
            };
            IJobDetail mt = JobBuilder.Create<QuartzSimpleJob>()
                .WithIdentity(GetJobKey(task))
                .SetJobData(new JobDataMap((IDictionary)data))
                .Build();
            await CoreScheduler.AddJob(mt, true);
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(GetTriggerKey(task))
                .ForJob(GetJobKey(task))
                .StartAt(task.FireTime)
                .Build();
            if (await CoreScheduler.RescheduleJob(GetTriggerKey(task), trigger) == null)
            {
                logger.LogInformation($"Created job for task with ID {task} at time {task.FireTime}", task);
                await CoreScheduler.ScheduleJob(trigger);
            }
            else
            {
                logger.LogInformation($"Rescheduled job for task with ID {task} at time {task.FireTime}", task);
            }
        }
        private TriggerKey GetTriggerKey(TimeScheduledTaskData task)
        {
            return new TriggerKey($"{task.Identifier}", task.Category + "_Task");
        }
        private JobKey GetJobKey(TimeScheduledTaskData task)
        {
            return new JobKey($"{task.Identifier}", task.Category + "_Job");
        }
        protected async Task FireEvent(TimeScheduledTaskData task)
        {
            logger.LogInformation($"Scheduled task {task.Identifier} has fired! Suspected time: {task.FireTime}");
            await Parallel.ForEachAsync(_subscribers, async (i, token) => await i.OnRecievedMessage(task));
        }

        async Task ITimeScheduler.RemoveScheduledTask(TimeScheduledTaskData task)
        {
            await CoreScheduler.DeleteJob(GetJobKey(task));
        }

        public void AddSubscriber(ITimeScheduledTaskSubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void RemoveSubscriber(ITimeScheduledTaskSubscriber subscriber)
        {
            _subscribers.Remove(subscriber);
        }
    }
}
