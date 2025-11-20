namespace CompetitiveBackend.Core.Objects.Riddles
{
    public class RiddleInfo : IntIdentifiable
    {
        public int CompetitionID;
        public string Question = string.Empty;
        public List<RiddleAnswer> PossibleAnswers = [];
        public RiddleAnswer TrueAnswer;
        public RiddleInfo(int competitionID, string question, List<RiddleAnswer> possibleAnswers, RiddleAnswer trueAnswer, int? iD = null)
            : base(iD)
        {
            CompetitionID = competitionID;
            Question = question;
            PossibleAnswers = possibleAnswers;
            TrueAnswer = trueAnswer;
        }
    }
}
