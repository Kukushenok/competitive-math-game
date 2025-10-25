using System.Collections;
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ChronoServiceRealisation
{
    internal sealed class QuartzTimeScheduler : ITimeScheduler
    {
        public sealed class QuartzSimpleJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                QuartzTimeScheduler sc = (context.Scheduler.Context[nameof(QuartzTimeScheduler)] as QuartzTimeScheduler)!;
                var task = (TimeScheduledTaskData)context.JobDetail.JobDataMap.Get(nameof(TimeScheduledTaskData))!;
                await sc?.FireEvent(task)!;
            }
        }

        private readonly List<ITimeScheduledTaskSubscriber> subscribers;
        private readonly ILogger<QuartzTimeScheduler> logger;
        private readonly ISchedulerFactory schedFactory;
        public QuartzTimeScheduler(ISchedulerFactory sched, ILogger<QuartzTimeScheduler> logger)
        {
            schedFactory = sched;
            this.logger = logger;
            subscribers = [];
        }

        public async Task AddOrUpdateScheduledTask(TimeScheduledTaskData task)
        {
            IScheduler sched = await schedFactory.GetScheduler();
            if (!sched.IsStarted)
            {
                await sched.Start();
                sched.Context.TryAdd(nameof(QuartzTimeScheduler), this);
            }

            var data = new Dictionary<string, object>
            {
                { nameof(TimeScheduledTaskData), task },
            };
            IJobDetail mt = JobBuilder.Create<QuartzSimpleJob>()
                .WithIdentity(GetJobKey(task))
                .SetJobData(new JobDataMap((IDictionary)data))
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(GetTriggerKey(task))
                .ForJob(GetJobKey(task))
                .StartAt(task.FireTime)
                .Build();
            await sched.ScheduleJob(mt, [trigger], true);
            logger.LogInformation($"Added job for task with ID {task.Identifier} at time {task.FireTime}", task);
        }

        private static TriggerKey GetTriggerKey(TimeScheduledTaskData task)
        {
            return new TriggerKey($"{task.Identifier}_{task.Category}", task.Category + "_Task");
        }

        private static JobKey GetJobKey(TimeScheduledTaskData task)
        {
            return new JobKey($"{task.Identifier}_{task.Category}", task.Category + "_Job");
        }

        private async Task FireEvent(TimeScheduledTaskData task)
        {
            logger.LogInformation($"Scheduled task {task.Identifier} has fired! Suspected time: {task.FireTime}");
            await Parallel.ForEachAsync(subscribers, async (i, token) => await i.OnRecievedMessage(task));
        }

        async Task ITimeScheduler.RemoveScheduledTask(TimeScheduledTaskData task)
        {
            IScheduler sched = await schedFactory.GetScheduler();
            await sched.DeleteJob(GetJobKey(task));
        }

        public void AddSubscriber(ITimeScheduledTaskSubscriber subscriber)
        {
            subscribers.Add(subscriber);
        }

        public void RemoveSubscriber(ITimeScheduledTaskSubscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public async Task Initialize()
        {
            IScheduler sched = await schedFactory.GetScheduler();
            sched.Context.TryAdd(nameof(QuartzTimeScheduler), this);
            if (!sched.IsStarted)
            {
                await sched.Start();
            }
        }
    }
}
