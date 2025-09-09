using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Services.Objects
{
    public interface ILargeFileProcessor
    {
        public Task<LargeData> Process(LargeData data);
    }
}
