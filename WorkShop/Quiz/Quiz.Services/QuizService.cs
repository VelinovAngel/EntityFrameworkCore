namespace Quiz.Services
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    using Quiz.Data;
    using Quiz.Models;
    using Quiz.Services.Contracts;
    using Quiz.Services.Models;

    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public QuizService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void Add(string title)
        {
            var quiz = new Quiz
            {
                Titile = title
            };

            this.applicationDbContext.Quizzes.Add(quiz);
            this.applicationDbContext.SaveChanges();
        }

        public QuizViewModel GetQuizById(int quizId)
        {
            var quiz = this.applicationDbContext.Quizzes
                .Include(x=>x.Qestions)
                .ThenInclude(x=>x.Answers)
                .FirstOrDefault(x => x.Id == quizId);

            var quizViewModel = new QuizViewModel
            {
                Id = quiz.Id,
                Title = quiz.Titile,
                Questions = quiz.Qestions.Select(x => new QuestionViewModel
                {
                    Id = x.Id,
                    Title = x.Titile,
                    Answers = x.Answers.Select(a => new AnswerViewModel
                    {
                        Id = a.Id,
                        Title = a.Title
                    })
                })
            };

            return quizViewModel;
        }
    }
}
