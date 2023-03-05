using Microsoft.EntityFrameworkCore;
using Devhunt.Models; 

namespace Devhunt.Data;

#nullable disable
public class AppDbContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        Database.EnsureCreated();

    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = _configuration.GetConnectionString("Default");
        options.UseMySql(connectionString,  ServerVersion.AutoDetect(connectionString));
        Database.EnsureCreated();
    }

    public DbSet<Pub> Pubs {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<Lesson> Lessons {get; set;}
    public DbSet<ListTemp> ListTemps {get; set;}
    public DbSet<Document> Documents {get; set;}
    public DbSet<Reaction> Reactions {get; set;}
    public DbSet<Commentaire> Commentaires {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Nmat).IsUnique();
        });
        modelBuilder.Entity<Commentaire>(entity =>
        {
            entity.HasIndex(e => e.ComsID).IsUnique();
        });
        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasIndex(e => e.DocID).IsUnique();
        });
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasIndex(e => e.LessonID).IsUnique();
        });
        modelBuilder.Entity<Pub>(entity =>
        {
            entity.HasIndex(e => e.PubID).IsUnique();
        });
        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasIndex(e => e.ReactID).IsUnique();
        });
    }
}