using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class DataLimiterDTO
    {
        public int Page { get; set; }
        public int Count { get; set; }
        public DataLimiterDTO(int page, int count)
        {
            Page = page;
            Count = count;
        }
    }
}
