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
                TimeScheduledTaskData task = (TimeScheduledTaskData)context.JobDetail.JobDataMap.Get(nameof(TimeScheduledTaskData))!;
                await sc?.FireEvent(task)!;
            }
        }
        private List<ITimeScheduledTaskSubscriber> _subscribers;
        ILogger<QuartzTimeScheduler> logger;
        ISchedulerFactory _schedFactory;
        public QuartzTimeScheduler(ISchedulerFactory sched, ILogger<QuartzTimeScheduler> logger)
        {
            _schedFactory = sched;
            this.logger = logger;
            _subscribers = new List<ITimeScheduledTaskSubscriber>();

        }
        public async Task AddOrUpdateScheduledTask(TimeScheduledTaskData task)
        {
            IScheduler sched = await _schedFactory.GetScheduler();
            if (!sched.IsStarted)
            {
                await sched.Start();
                sched.Context.TryAdd(nameof(QuartzTimeScheduler), this);
            }
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {nameof(TimeScheduledTaskData), task}
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
        private TriggerKey GetTriggerKey(TimeScheduledTaskData task)
        {
            return new TriggerKey($"{task.Identifier}_{task.Category}", task.Category + "_Task");
        }
        private JobKey GetJobKey(TimeScheduledTaskData task)
        {
            return new JobKey($"{task.Identifier}_{task.Category}", task.Category + "_Job");
        }
        protected async Task FireEvent(TimeScheduledTaskData task)
        {
            logger.LogInformation($"Scheduled task {task.Identifier} has fired! Suspected time: {task.FireTime}");
            await Parallel.ForEachAsync(_subscribers, async (i, token) => await i.OnRecievedMessage(task));
        }

        async Task ITimeScheduler.RemoveScheduledTask(TimeScheduledTaskData task)
        {
            IScheduler sched = await _schedFactory.GetScheduler();
            await sched.DeleteJob(GetJobKey(task));
        }

        public void AddSubscriber(ITimeScheduledTaskSubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void RemoveSubscriber(ITimeScheduledTaskSubscriber subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        public async Task Initialize()
        {
            IScheduler sched = await _schedFactory.GetScheduler();
            sched.Context.TryAdd(nameof(QuartzTimeScheduler), this);
            if (!sched.IsStarted)
            {
                await sched.Start();
            }
        }
    }
}
