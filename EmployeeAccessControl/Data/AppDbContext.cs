using Microsoft.EntityFrameworkCore;
using WebApplication6.Models.Entities;

namespace WebApplication6.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Shift> Shifts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Дополнительная настройка если нужна будет позже
    }
}