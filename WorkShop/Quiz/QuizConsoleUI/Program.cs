namespace QuizConsoleUI
{
    using System;
    using System.IO;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Quiz.Data;
    using Quiz.Services;
    using Quiz.Services.Contracts;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var quizService = serviceProvider.GetService<IUserAnswerService>();
            var quiz = quizService.GetUserResult("3f7c18cc-6d09-49e0-b401-927b295c9131", 1);

            Console.WriteLine(quiz);

            //var questionService = serviceProvider.GetService<IQuestionService>();
            //questionService.Add("What is Entity Framework Core?", 1);

            //var answerService = serviceProvider.GetService<IAnswerService>();
            //answerService.Add("It is a ORM", 5, true, 1);
            //answerService.Add("It is a MicroORM", 0, false, 1);

            //var userAnswer = serviceProvider.GetService<IUserAnswerService>();
            //userAnswer.AddUserAnswer("3f7c18cc-6d09-49e0-b401-927b295c9131", 1, 1, 2);

        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IQuizService, QuizService>();

            services.AddTransient<IQuestionService, QuestionService>();

            services.AddTransient<IAnswerService, AnswerService>();

            services.AddTransient<IUserAnswerService, UserAnswerService>();


        }
    }
}
