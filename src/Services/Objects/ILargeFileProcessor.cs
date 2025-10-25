using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Services.Objects
{
    public interface ILargeFileProcessor
    {
        Task<LargeData> Process(LargeData data);
    }
}
