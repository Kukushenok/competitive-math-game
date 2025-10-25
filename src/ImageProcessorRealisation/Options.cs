using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolutionInstaller;

namespace ImageProcessorRealisation
{
    public class Options
    {
        private readonly IServiceCollection collection;
        public bool ConfigurationSetUp { get; private set; }
        public Options(IServiceCollection collection)
        {
            this.collection = collection;
        }

        public Options AddConstraints(uint minSize, uint maxSize)
        {
            collection.AddSingleton<IImageConfig>(new DefaultImageConfig()
            {
                MinSize = minSize,
                MaxSize = maxSize,
            });
            ConfigurationSetUp = true;
            return this;
        }

        public Options UseConfigurationConstraints(string sectionName = "Constraints:ImageConfig")
        {
            collection.AddSingleton<IImageConfig>(x => new ConfigurationReaderConfig(x.GetRequiredService<IConfiguration>(), sectionName));
            ConfigurationSetUp = true;
            return this;
        }

        public Options UseDefaultConstraints()
        {
            AddConstraints(16, 256);
            ConfigurationSetUp = true;
            return this;
        }
    }
}
