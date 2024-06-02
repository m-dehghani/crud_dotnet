using Mc2.CrudTest.Presentation.Shared.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Presentation.Infrastructure;

// creating another Dbcontext for reading in CQRS
public class ReadModelDbContext : DbContext
{
    public DbSet<CustomerViewModel> CustomerEvents { get; set; }

    public ReadModelDbContext(DbContextOptions<ReadModelDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerViewModel>().ToTable("Events").HasNoKey();
        // Additional configuration as needed
    }
}