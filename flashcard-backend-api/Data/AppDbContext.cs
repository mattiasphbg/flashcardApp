using flashcard.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System; 

namespace flashcard_backend_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Flashcard> Flashcards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flashcard>().HasData(
                new Flashcard { Id = Guid.Parse("c1d1a1b1-c1d1-e1f1-0123-456789abcdef"), Question = "What is the capital of France?", Answer = "Paris", CreatedDate = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc) },
                new Flashcard { Id = Guid.Parse("d2e2f2b2-c2d2-e2f2-5678-90abcdef0123"), Question = "What is the largest planet in our solar system?", Answer = "Jupiter", CreatedDate = DateTimeKind.Utc == DateTimeKind.Utc ? DateTime.SpecifyKind(new DateTime(2023, 1, 2), DateTimeKind.Utc) : new DateTime(2023, 1, 2) },
                new Flashcard { Id = Guid.Parse("e3f3a3b3-c3d3-e3f3-abcd-012345abcdef"), Question = "Who wrote 'Romeo and Juliet'?", Answer = "William Shakespeare", CreatedDate = DateTime.SpecifyKind(new DateTime(2023, 1, 3), DateTimeKind.Utc) },
                new Flashcard { Id = Guid.Parse("f4a4a4b4-c4d4-e4f4-7890-123456abcdef"), Question = "What is the chemical symbol for gold?", Answer = "Au", CreatedDate = DateTime.SpecifyKind(new DateTime(2023, 1, 4), DateTimeKind.Utc) }, // Ändrat 'g' till 'a'
                new Flashcard { Id = Guid.Parse("a5b5c5d5-e5f5-4321-8901-234567abcdef"), Question = "In what year did World War II end?", Answer = "1945", CreatedDate = DateTime.SpecifyKind(new DateTime(2023, 1, 5), DateTimeKind.Utc) } // Justerat den tredje gruppen till 4 siffror
            );
        }
    }
}
