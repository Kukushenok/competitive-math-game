namespace CompetitiveBackend.Core.Objects.Riddles
{
    public class RiddleInfo : IntIdentifiable
    {
        public int CompetitionID;
        public string Question = "";
        public List<RiddleAnswer> PossibleAnswers = new List<RiddleAnswer>();
        public RiddleAnswer TrueAnswer;
        public RiddleInfo(int competitionID, string question, List<RiddleAnswer> possibleAnswers, RiddleAnswer trueAnswer, int? ID = null): base(ID)
        {
            CompetitionID = competitionID;
            Question = question;
            PossibleAnswers = possibleAnswers;
            TrueAnswer = trueAnswer;
        }
    }
}
