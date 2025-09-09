using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolutionInstaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessorRealisation
{
    public class Options
    {
        private IServiceCollection collection;
        public bool ConfigurationSetUp { get; private set; } = false;
        public Options(IServiceCollection collection)
        {
            this.collection = collection;
        }

        public Options AddConstraints(uint minSize, uint maxSize)
        {
            collection.AddSingleton<IImageConfig>(new DefaultImageConfig()
            {
                MinSize = minSize,
                MaxSize = maxSize
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
