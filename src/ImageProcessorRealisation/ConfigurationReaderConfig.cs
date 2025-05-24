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

        public uint MinSize => TryGet(nameof(MinSize), 256);

        public uint MaxSize => TryGet(nameof(MaxSize), 256);
    }
}
