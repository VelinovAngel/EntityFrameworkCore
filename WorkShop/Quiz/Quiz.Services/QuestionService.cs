namespace Quiz.Services
{
    using Quiz.Data;
    using Quiz.Models;
    using Quiz.Services.Contracts;
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public QuestionService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public int Add(string title, int quizId)
        {

            var question = new Question
            {
                Titile = title,
                QuizId = quizId
            };

            this.applicationDbContext.Questions.Add(question);
            this.applicationDbContext.SaveChanges();

            return question.Id;
        }
    }
}
