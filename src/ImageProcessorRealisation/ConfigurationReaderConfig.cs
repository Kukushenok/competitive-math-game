using ImageProcessorRealisation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionInstaller
{
    class ConfigurationReaderConfig : IImageConfig
    {
        private IConfiguration conf;
        private string _section;
        public ConfigurationReaderConfig(IConfiguration conf, string sectionName)
        {
            _section = sectionName;
            this.conf = conf;
        }
        private uint TryGet(string name, uint deflt)
        {
            return uint.TryParse(conf.GetSection(_section)[name], out uint result) ? result : deflt;
        }

        public uint MaxWidth => TryGet(nameof(MaxWidth), 256);

        public uint MaxHeight => TryGet(nameof(MaxHeight), 256);

        public uint MinWidth => TryGet(nameof(MinWidth), 32);

        public uint MinHeight => TryGet(nameof(MinHeight), 32);
    }
}
