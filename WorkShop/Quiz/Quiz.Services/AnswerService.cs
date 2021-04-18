namespace Quiz.Services
{
    using Quiz.Data;
    using Quiz.Models;
    using Quiz.Services.Contracts;
    public class AnswerService : IAnswerService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AnswerService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public int Add(string title, int point, bool isCorrect, int questionId)
        {
            var answer = new Answer
            {
                Title = title,
                Points = point,
                IsCorrect = isCorrect,
                QuestionId = questionId      
            };

            this.applicationDbContext.Answers.Add(answer);
            this.applicationDbContext.SaveChanges();

            return answer.Id;
        }
    }
}
