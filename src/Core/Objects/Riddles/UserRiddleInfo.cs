namespace CompetitiveBackend.Core.Objects.Riddles
{
    public class UserRiddleInfo
    {
        public string Question = string.Empty;
        public RiddleAnswer[] AvailableAnswers = [];
        public UserRiddleInfo(string question, RiddleAnswer[] availableAnswers)
        {
            Question = question;
            AvailableAnswers = availableAnswers;
        }
    }
}
