namespace Quiz.Services.Contracts
{
    using Quiz.Services.Models;
    public interface IQuizService
    {
        void Add(string title);
        public QuizViewModel GetQuizById(int quizId);
    }
}
