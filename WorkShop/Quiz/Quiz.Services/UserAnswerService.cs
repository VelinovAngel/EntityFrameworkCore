namespace Quiz.Services
{
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;


    using Quiz.Data;
    using Quiz.Models;
    using Quiz.Services.Contracts;
    using Quiz.Services.Models;

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

        public void BulkAddUserAnswer(QuizInputModel quizInputModel)
        {
            var userAnswers = new List<UserAnswer>();

            foreach (var question in quizInputModel.Questions)
            {
                var userAnswer = new UserAnswer
                {
                    IdentityUserId = quizInputModel.UserId,
                    QuizId = quizInputModel.QuizId,
                    AnswerId = question.AnswerId,
                    QuestionId = question.QuestionId
                };

                userAnswers.Add(userAnswer);
            }

            this.applicationDbContext.AddRange(userAnswers);
            this.applicationDbContext.SaveChanges();
        }

        public int GetUserResult(string userID, int quizId)
        {
            var originalQuiz = this.applicationDbContext
                .Quizzes
                .Include(x=>x.Qestions)
                .ThenInclude(x=>x.Answers)
                .FirstOrDefault(x => x.Id == quizId);

            var userAnswers = this.applicationDbContext.UsersAnswers
                .Where(x => x.IdentityUserId == userID && x.QuizId == quizId)
                .ToList();

            int? totalPoints = 0;

            foreach (var userAnswer in userAnswers)
            {
                totalPoints += originalQuiz.Qestions
                    .FirstOrDefault(x => x.Id == userAnswer.QuestionId)
                    .Answers
                    .Where(x => x.IsCorrect)
                    .FirstOrDefault(x => x.Id == userAnswer.AnswerId)
                    ?.Points;
            }
            return totalPoints.GetValueOrDefault();
        }
    }
}
