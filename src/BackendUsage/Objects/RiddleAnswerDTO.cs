using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class RiddleAnswerDTO
    {
        public string TextAnswer { get; set; } = "";

        public RiddleAnswerDTO(string textAnswer)
        {
            TextAnswer = textAnswer;
        }
    }
}
