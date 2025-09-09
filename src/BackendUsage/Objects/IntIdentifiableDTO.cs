using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class IntIdentifiableDTO
    {
        public int? ID { get; set; }
        public IntIdentifiableDTO(int? ID)
        {
            this.ID = ID;
        }
        public IntIdentifiableDTO()
        {
            ID = null;
        }
    }
}
