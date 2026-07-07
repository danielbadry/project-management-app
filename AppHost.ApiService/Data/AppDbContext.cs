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
    }
}
