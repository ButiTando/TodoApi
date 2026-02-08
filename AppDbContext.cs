using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Entities;

namespace TodoApi.Data;

public class AppDbContext : DbContext
{

    public DbSet<User> Users {get; set;} = null!;
    public DbSet<UTask> UTasks {get; set;} = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UTask>().HasIndex(t => t.Name);
    }

}
