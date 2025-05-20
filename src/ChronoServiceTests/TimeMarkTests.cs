using ChronoServiceRealisation;
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Quartz;
using System.Diagnostics;
using Xunit.Abstractions;
using XUnitLoggingProvider;
namespace ChronoServiceTests
{
   
    public class MockSubscriber : ITimeScheduledTaskSubscriber
    {
        public static TimeSpan ACCEPTABLE_LATENCY = new TimeSpan(0, 0, 0, 0, 500);
        public TimeScheduledTaskData? Expected;
        public bool Recieved { get; private set; } = false;
        private ITimeScheduler? _sched;
        public int TriggerCount { get; private set; }
        private bool onlyOnce;
        private bool disposed;
        public MockSubscriber(ITimeScheduler? sched = null, TimeScheduledTaskData? expected = null, bool onlyOnce = false)
        {
            Expected = expected;
            if (sched != null)
            {
                _sched = sched;
                _sched.AddSubscriber(this);
            }
            this.onlyOnce = onlyOnce;
            TriggerCount = 0;
            disposed = false;
        }
        public void Dispose()
        {
            if(_sched != null) _sched.RemoveSubscriber(this);
            _sched = null;
            disposed = true;
        }

        public Task OnRecievedMessage(TimeScheduledTaskData data)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Assert.False(disposed, "I am unsubscribed!");
            if (Expected != null)
            {
                Assert.Equal(Expected?.Identifier, data.Identifier);
                Assert.Equal(Expected?.Category, data.Category);
                Assert.Equal(Expected?.FireTime, data.FireTime);
                Assert.Equal(Expected?.Data, data.Data);
            }
            TimeSpan delta = (now - data.FireTime);
            Assert.True(delta > TimeSpan.Zero, "Fired sooner than expected");
            Assert.True(delta < ACCEPTABLE_LATENCY, "Fired too late");
            Recieved = true;
            TriggerCount++;
            Debug.WriteLine("Triggered!");
            if (onlyOnce) Dispose();
            return Task.CompletedTask;
        }
    }

    public class TimeMarkTests
    {
        ITimeScheduler sc;
        public TimeMarkTests(ITestOutputHelper helper)
        {
            IServiceCollection coll = new ServiceCollection();
            coll.AddQuartzTimeScheduler();
            coll.UseXUnitLogging(helper);
            ServiceProvider provider = coll.BuildServiceProvider(true);
            sc = provider.GetService<ITimeScheduler>()!;
            sc.Initialize().GetAwaiter().GetResult();
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task WaitingIsExactly(int seconds)
        {
            TimeScheduledTaskData data = FromTimeOffset(0, TimeSpan.FromSeconds(seconds));
            using MockSubscriber sub = new MockSubscriber(sc, data);
            await sc.AddOrUpdateScheduledTask(data);
            await Task.Delay(seconds * 1000 + 500);
            Assert.True(sub.Recieved);
        }
        public static TimeScheduledTaskData FromTimeOffset(int idx, TimeSpan offset)
        {
            return new TimeScheduledTaskData(idx, "Category", DateTimeOffset.Now + offset, "Hello! " + offset.ToString());
        }
        [Fact]
        public async Task RescheduledOne()
        {
            using MockSubscriber subscriber = new MockSubscriber(sc);
            for (int i = 1; i < 5; i++)
            {
                await sc.AddOrUpdateScheduledTask(FromTimeOffset(0, TimeSpan.FromSeconds(i)));
            }
            await Task.Delay(5500);
            Assert.True(subscriber.Recieved);
        }
        [Fact]
        public async Task MultipleJobs()
        {
            using MockSubscriber subscriber = new MockSubscriber(sc);
            for (int i = 1; i < 5; i++)
            {
                await sc.AddOrUpdateScheduledTask(FromTimeOffset(i, TimeSpan.FromSeconds(i)));
            }
            await Task.Delay(5500);
            Assert.True(subscriber.Recieved);
            Assert.Equal(4, subscriber.TriggerCount);
        }
        [Fact]
        public async Task MultipleJobsWithDifferentCategories()
        {
            using MockSubscriber subscriber = new MockSubscriber(sc);
            for (int i = 1; i < 5; i++)
            {
                TimeScheduledTaskData dt = FromTimeOffset(i, TimeSpan.FromSeconds(i));
                dt.Category = "OtherCategory";
                TimeScheduledTaskData dt2 = FromTimeOffset(i, TimeSpan.FromSeconds(i));
                await sc.AddOrUpdateScheduledTask(dt);
                await sc.AddOrUpdateScheduledTask(dt2);
            }
            await Task.Delay(5500);
            Assert.True(subscriber.Recieved);
            Assert.Equal(8, subscriber.TriggerCount);
        }
        [Fact]
        public async Task UnsubscribeWorking()
        {
            using MockSubscriber subscriber = new MockSubscriber(sc);
            for (int i = 1; i < 5; i++)
            {
                await sc.AddOrUpdateScheduledTask(FromTimeOffset(i, TimeSpan.FromSeconds(i)));
            }
            await Task.Delay(2500);
            sc.RemoveSubscriber(subscriber);
            await Task.Delay(2000);
            Assert.True(subscriber.Recieved);
            Assert.Equal(2, subscriber.TriggerCount);
        }
    }
}