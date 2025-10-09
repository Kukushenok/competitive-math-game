using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.Core.Objects.Riddles
{
    public class UserRiddleInfo
    {
        public string Question = "";
        public RiddleAnswer[] AvailableAnswers = Array.Empty<RiddleAnswer>();
        public UserRiddleInfo(string question, RiddleAnswer[] availableAnswers)
        {
            Question = question;
            AvailableAnswers = availableAnswers;
        }
    }
}
