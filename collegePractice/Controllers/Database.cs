using collegePractice.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=surus.db.elephantsql.com;Port=5432;Database=dvisoaoz;Username=dvisoaoz;Password=JpMPUKnM4QN2gVwt3C-HU_a9U6qQ3DWb");
    }
}