namespace RepositoriesTests
{
    internal sealed class FileDumper
    {
        private readonly string corePath;
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
            await File.WriteAllTextAsync(Path.Combine(corePath, $"{name}.txt"), contents);
        }
    }
}
