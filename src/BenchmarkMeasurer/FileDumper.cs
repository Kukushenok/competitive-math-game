using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkMeasurer
{
    internal class FileDumper
    {
        private string corePath;
        public FileDumper(string corePath)
        {
            this.corePath = corePath;
            if (!Directory.Exists(corePath))
            {
                Directory.CreateDirectory(corePath);
            }
        }
        public async Task Dump(string name, string contents)
        {
            await File.AppendAllTextAsync(Path.Combine(corePath, $"{name}.txt"), contents);
        }
    }
}
