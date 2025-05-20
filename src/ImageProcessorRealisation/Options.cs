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

        public Options AddConstraints(uint minWidth, uint maxWidth, uint minHeight, uint maxHeight)
        {
            collection.AddSingleton<IImageConfig>(new DefaultImageConfig()
            {
                MaxHeight = maxHeight,
                MinHeight = minHeight,
                MaxWidth = maxWidth,
                MinWidth = minWidth
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
            AddConstraints(16, 256, 16, 256);
            ConfigurationSetUp = true;
            return this;
        }
    }
}
