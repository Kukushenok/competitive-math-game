namespace CompetitiveBackend.Core.Objects
{
    public record DataLimiter(int PartitionIndex, int Partition)
    {
        public static readonly DataLimiter NoLimit = new(0, 0);
        public bool HasNoLimit => Partition <= 0;
        public int FirstIndex => PartitionIndex * Partition;
        public int LastIndex => (PartitionIndex + 1) * Partition;
    }
}
