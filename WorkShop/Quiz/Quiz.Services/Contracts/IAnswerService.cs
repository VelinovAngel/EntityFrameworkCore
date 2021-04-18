namespace Quiz.Services.Contracts
{
    public interface IAnswerService
    {
        int Add(string title, int point, bool isCorrect, int questionId);
    }
}
