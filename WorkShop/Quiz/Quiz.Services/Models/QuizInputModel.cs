namespace Quiz.Services.Models
{
    using System.Collections.Generic;
    public class QuizInputModel
    {
        public string UserId { get; set; }

        public int QuizId { get; set; }

        public List<QuestionInputModel> Questions { get; set; }
    }
}
