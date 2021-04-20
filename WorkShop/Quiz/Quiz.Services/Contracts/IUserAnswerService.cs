﻿namespace Quiz.Services.Contracts
{
    public interface IUserAnswerService
    {
        void AddUserAnswer(string userId, int quizId, int questionId, int answerId);

        int GetUserResult(string userID, int quizId);
    }
}
