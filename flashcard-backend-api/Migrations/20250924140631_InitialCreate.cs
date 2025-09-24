using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace flashcard_backend_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Flashcards",
                columns: new[] { "Id", "Answer", "CreatedDate", "LastModifiedDate", "Question" },
                values: new object[,]
                {
                    { new Guid("a5b5c5d5-e5f5-4321-8901-234567abcdef"), "1945", new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "In what year did World War II end?" },
                    { new Guid("c1d1a1b1-c1d1-e1f1-0123-456789abcdef"), "Paris", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "What is the capital of France?" },
                    { new Guid("d2e2f2b2-c2d2-e2f2-5678-90abcdef0123"), "Jupiter", new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "What is the largest planet in our solar system?" },
                    { new Guid("e3f3a3b3-c3d3-e3f3-abcd-012345abcdef"), "William Shakespeare", new DateTime(2023, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "Who wrote 'Romeo and Juliet'?" },
                    { new Guid("f4a4a4b4-c4d4-e4f4-7890-123456abcdef"), "Au", new DateTime(2023, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "What is the chemical symbol for gold?" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcards");
        }
    }
}
