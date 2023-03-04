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
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = _configuration.GetConnectionString("Default");
        options.UseMySql(connectionString,  ServerVersion.AutoDetect(connectionString));
    }

    public DbSet<Pub> Pubs {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<Lesson> Lessons {get; set;}
    public DbSet<Document> Documents {get; set;}

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
    }
}