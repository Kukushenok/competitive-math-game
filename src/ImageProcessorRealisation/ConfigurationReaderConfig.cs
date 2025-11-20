using ImageProcessorRealisation;
using Microsoft.Extensions.Configuration;

namespace SolutionInstaller
{
    internal sealed class ConfigurationReaderConfig : IImageConfig
    {
        private readonly IConfiguration conf;
        private readonly string section;
        public ConfigurationReaderConfig(IConfiguration conf, string sectionName)
        {
            section = sectionName;
            this.conf = conf;
        }

        private uint TryGet(string name, uint deflt)
        {
            return uint.TryParse(conf.GetSection(section)[name], out uint result) ? result : deflt;
        }

        public uint MinSize => TryGet(nameof(MinSize), 32);

        public uint MaxSize => TryGet(nameof(MaxSize), 256);
    }
}
