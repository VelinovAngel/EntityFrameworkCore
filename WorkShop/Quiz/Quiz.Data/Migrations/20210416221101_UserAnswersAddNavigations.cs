using Microsoft.EntityFrameworkCore.Migrations;

namespace Quiz.Data.Migrations
{
    public partial class UserAnswersAddNavigations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersAnswers",
                table: "UsersAnswers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsersAnswers");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "UsersAnswers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersAnswers",
                table: "UsersAnswers",
                columns: new[] { "IdentityUserId", "QuizId" });

            migrationBuilder.CreateIndex(
                name: "IX_UsersAnswers_AnswerId",
                table: "UsersAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAnswers_QuestionId",
                table: "UsersAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAnswers_QuizId",
                table: "UsersAnswers",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAnswers_Answers_AnswerId",
                table: "UsersAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAnswers_AspNetUsers_IdentityUserId",
                table: "UsersAnswers",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAnswers_Questions_QuestionId",
                table: "UsersAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAnswers_Quizzes_QuizId",
                table: "UsersAnswers",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAnswers_Answers_AnswerId",
                table: "UsersAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAnswers_AspNetUsers_IdentityUserId",
                table: "UsersAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAnswers_Questions_QuestionId",
                table: "UsersAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAnswers_Quizzes_QuizId",
                table: "UsersAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersAnswers",
                table: "UsersAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UsersAnswers_AnswerId",
                table: "UsersAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UsersAnswers_QuestionId",
                table: "UsersAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UsersAnswers_QuizId",
                table: "UsersAnswers");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "UsersAnswers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UsersAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersAnswers",
                table: "UsersAnswers",
                columns: new[] { "UserId", "QuizId" });
        }
    }
}
