using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using ImageProcessorRealisation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
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
        private IImageProcessor processor;
        private ILogger logger;
        public ImageTester(FuncTestInitFixture fixture, ITestOutputHelper helper)
        {
            IServiceCollection coll = new ServiceCollection();
            coll.AddMajickImageRescaler((options) =>
            {
                options.AddConstraints(
                    ImageTestingDataManager.MinWidth,
                    ImageTestingDataManager.MaxWidth,
                    ImageTestingDataManager.MinHeight,
                    ImageTestingDataManager.MaxHeight
                    );
            });
            coll.UseXUnitLogging(helper);
            ServiceProvider s = coll.BuildServiceProvider();
            processor = s.GetService<IImageProcessor>()!;
            logger = s.GetService<ILogger>()!;
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