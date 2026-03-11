using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineVoting.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administrators",
                keyColumn: "AdminId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Candidates",
                keyColumn: "CandidateId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Candidates",
                keyColumn: "CandidateId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Elections",
                keyColumn: "ElectionId",
                keyValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Administrators",
                columns: new[] { "AdminId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Elections",
                columns: new[] { "ElectionId", "EndDate", "Name", "StartDate", "Status" },
                values: new object[] { 1, new DateTime(2026, 3, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), "Student Council Election 2026", new DateTime(2026, 3, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Ongoing" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CITNo", "DOB", "Email", "IsCandidate", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "CIT123456", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", false, "Admin User", "Password123", "Admin" },
                    { 2, "CIT654321", new DateTime(2000, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "john@example.com", false, "John Doe", "Password123", "Voter" },
                    { 3, "CIT987654", new DateTime(1999, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane@example.com", false, "Jane Smith", "Password123", "Voter" }
                });

            migrationBuilder.InsertData(
                table: "Candidates",
                columns: new[] { "CandidateId", "Age", "CITNo", "Education", "ElectionId", "Logo", "Manifesto", "Name", "Party", "ProfilePicture", "VoteCount" },
                values: new object[,]
                {
                    { 1, 25, "CIT1001", "BSc Computer Science", 1, "logo1.png", "I promise transparency", "Alice Candidate", "Party A", "profile1.png", 0 },
                    { 2, 27, "CIT1002", "BBA", 1, "logo2.png", "I will improve student facilities", "Bob Candidate", "Party B", "profile2.png", 0 }
                });
        }
    }
}
