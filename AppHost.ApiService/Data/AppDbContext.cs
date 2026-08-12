using AppHost.ApiService.Entities.Auth;
using AppHost.ApiService.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;

namespace AppHost.ApiService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Projects> Projects => Set<Projects>();
    public DbSet<Story> Stories => Set<Story>();
    public DbSet<SubTask> SubTasks => Set<SubTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique()
            .HasDatabaseName("UX_Users_Email");

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Username)
            .IsUnique()
            .HasDatabaseName("UX_Users_Username");

        modelBuilder.Entity<Story>()
            .HasOne(story => story.Owner)
            .WithMany(user => user.OwnedStories)
            .HasForeignKey(story => story.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Story>()
            .HasOne(story => story.AssignedUser)
            .WithMany(user => user.AssignedStories)
            .HasForeignKey(story => story.AssignedId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Story>()
            .HasOne(story => story.Project)
            .WithMany(project => project.ProjectStories)
            .HasForeignKey(story => story.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
