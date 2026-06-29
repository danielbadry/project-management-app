using AppHost.ApiService.Entities.Identity;
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
}