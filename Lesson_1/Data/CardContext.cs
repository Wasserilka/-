#nullable disable
using Microsoft.EntityFrameworkCore;
using Lesson_1_1.Models;

public class CardContext : DbContext
    {
        public CardContext (DbContextOptions<CardContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Card> Card { get; set; }
    }
