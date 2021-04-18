namespace Quiz.Models
{
    using System.Collections.Generic;
    public class Answer
    {
        public Answer()
        {
            this.UserAnswers = new HashSet<UserAnswer>();
        }
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsCorrect { get; set; }

        public int Points { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}