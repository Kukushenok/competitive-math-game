namespace CompetitiveBackend.Services.ExtraTools
{
    [Serializable]
    public struct TimeScheduledTaskData
    {
        public int Identifier = 0;
        public string Category = string.Empty;
        public DateTimeOffset FireTime;
        public string Data = string.Empty;
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
