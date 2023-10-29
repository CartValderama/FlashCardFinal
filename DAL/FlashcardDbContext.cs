using FlashCardDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlashCardDemo.DAL;

public class FlashcardDbContext : IdentityDbContext<FlashCardDemoUser>
{
    public FlashcardDbContext(DbContextOptions<FlashcardDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<Deck> Decks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    // taken from codewithgukesh-resource

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

}

