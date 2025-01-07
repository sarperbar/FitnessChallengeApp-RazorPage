using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fitnessChallenge.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChallengeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Challenges",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Challenges",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Difficulty",
                table: "Challenges",
                newName: "DifficultyLevel");

            migrationBuilder.RenameColumn(
                name: "ChallengeId",
                table: "Challenges",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Challenges",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Challenges",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DifficultyLevel",
                table: "Challenges",
                newName: "Difficulty");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Challenges",
                newName: "ChallengeId");
        }
    }
}
