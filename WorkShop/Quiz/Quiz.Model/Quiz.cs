using System.Collections.Generic;

namespace Quiz.Models
{
    public class Quiz
    {
        public Quiz()
        {
            this.Qestions = new HashSet<Question>();
            this.UserAnswers = new HashSet<UserAnswer>();
        }
        public int Id { get; set; }

        public string Titile { get; set; }

        public ICollection<Question> Qestions { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
