using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public struct LargeDataDTO
    {
        public byte[] Data { get; set; }
        public LargeDataDTO(byte[] data)
        {
            Data = data;
        }
    }
}
