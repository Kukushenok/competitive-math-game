using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class IntIdentifiableDTO
    {
        public int? ID { get; set; }
        public IntIdentifiableDTO(int? iD)
        {
            ID = iD;
        }

        public IntIdentifiableDTO()
        {
            ID = null;
        }
    }
}
