using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Core
{
    public interface ILargeFileProcessor
    {
        public Task<LargeData> Process(LargeData data);
    }
}
