namespace CompetitiveBackend.Services.ExtraTools
{
    [Serializable]
    public struct TimeScheduledTaskData
    {
        public int Identifier = 0;
        public string Category = "";
        public DateTimeOffset FireTime;
        public string Data = "";
        public TimeScheduledTaskData()
        {
        }
        public TimeScheduledTaskData(int identifier, string category, DateTimeOffset fireTime, string data)
        {
            Identifier = identifier;
            FireTime = fireTime;
            Data = data;
            Category = category;
        }
    }
}
