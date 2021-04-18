namespace Quiz.Services
{
    using Quiz.Data;
    using Quiz.Models;
    using Quiz.Services.Contracts;
    public class UserAnswerService : IUserAnswerService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserAnswerService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public void AddUserAnswer(string userId, int quizId, int questionId, int answerId)
        {
            var userAnswer = new UserAnswer
            {
                IdentityUserId = userId,
                QuizId = quizId,
                QuestionId = questionId,
                AnswerId = answerId
            };

            this.applicationDbContext.UsersAnswers.Add(userAnswer);
            this.applicationDbContext.SaveChanges();
        }
    }
}
