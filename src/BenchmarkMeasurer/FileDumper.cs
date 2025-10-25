namespace BenchmarkMeasurer
{
    public class FileDumper
    {
        private readonly string corePath;
        private readonly bool appendMode = true;
        public FileDumper(string corePath, bool appendMode = true)
        {
            this.corePath = corePath;
            this.appendMode = appendMode;
            if (!Directory.Exists(corePath))
            {
                Directory.CreateDirectory(corePath);
            }
        }

        public FileDumper Clone(bool appendMode = true)
        {
            return new FileDumper(corePath, appendMode);
        }

        public async Task Dump(string name, string contents)
        {
            if (appendMode)
            {
                await File.AppendAllTextAsync(Path.Combine(corePath, $"{name}.txt"), contents);
            }
            else
            {
                await File.WriteAllTextAsync(Path.Combine(corePath, $"{name}.txt"), contents);
            }
        }
    }
}
