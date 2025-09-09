using CompetitiveBackend.Core.Objects;

namespace ImageProcessorTests
{
    public class FuncTestStructure
    {
        public bool IsPositive { get; init; }
        public string ResultStorage { get; init; }
        public string TestStorage { get; init; }
        public FuncTestStructure(string testStorage, bool isPositive, string ResultStorage)
        {
            TestStorage = testStorage;
            IsPositive = isPositive;
            this.ResultStorage = ResultStorage;
        }
        public async Task<LargeData> LoadData()
        {
            return new LargeData(await File.ReadAllBytesAsync(TestStorage));
        }
        public async Task SaveTest(LargeData data)
        {
            await File.WriteAllBytesAsync(ResultStorage, data.Data);
        }
    }
}