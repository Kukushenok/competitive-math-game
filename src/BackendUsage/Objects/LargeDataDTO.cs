namespace CompetitiveBackend.BackendUsage.Objects
{
    public struct LargeDataDTO
    {
        public readonly byte[] Data;
        public LargeDataDTO(byte[] Data) => this.Data = Data;
    }
}
