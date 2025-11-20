using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using ImageProcessorRealisation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using XUnitLoggingProvider;

namespace ImageProcessorTests
{
    public class FuncTestInitFixture : IDisposable
    {
        public FuncTestInitFixture()
        {
            ImageTestingDataManager.InitFuncTestData();
        }

        public void Dispose()
        {
        }
    }

    [Collection("Sequential")]
    public class ImageTester : IClassFixture<FuncTestInitFixture>
    {
        private readonly IImageProcessor processor;
        private readonly ILogger logger;
        public ImageTester(FuncTestInitFixture fixture, ITestOutputHelper helper)
        {
            IServiceCollection coll = new ServiceCollection();
            coll.AddMajickImageRescaler((options) =>
            {
                ImageTestingDataManager.SetupConstraints(options);
            });
            coll.UseXUnitLogging(helper);
            ServiceProvider s = coll.BuildServiceProvider();
            processor = s.GetRequiredService<IImageProcessor>()!;
            logger = s.GetRequiredService<ILoggerProvider>().CreateLogger("Base");
        }

        [Theory(DisplayName = "FuncTestExecution")]
        [ClassData(typeof(ImageTestingDataManager))]
        public async Task ExecuteFuncTest(FuncTestStructure test)
        {
            logger.LogInformation($"Loading image: {test.TestStorage}");
            if (test.IsPositive)
            {
                logger.LogInformation($"Excepting positive result");
                LargeData result = await processor.Process(await test.LoadData());
                await test.SaveTest(result);
                logger.LogInformation($"Output was saved: {test.ResultStorage}");
            }
            else
            {
                logger.LogInformation($"Excepting negative result");
                await Assert.ThrowsAnyAsync<ServiceException>(async ()
                    => await processor.Process(await test.LoadData()));
            }
        }
    }
}