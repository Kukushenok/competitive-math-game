namespace CompetitiveBackend.BackendUsage.Objects
{
    public class DataLimiterDTO
    {
        public readonly int Page;
        public readonly int Count;
        public DataLimiterDTO(int page, int count)
        {
            Page = page;
            Count = count;
        }
    }
}
