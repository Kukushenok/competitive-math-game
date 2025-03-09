namespace CompetitiveBackend.Core
{
    public record DataLimiter(int PartitionIndex, int Partition)
    {
        public static readonly DataLimiter NoLimit = new DataLimiter(0, 0);
    }
}
