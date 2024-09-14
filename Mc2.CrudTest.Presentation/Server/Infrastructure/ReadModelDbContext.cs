using Mc2.CrudTest.Presentation.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Presentation.Server.Infrastructure;

// creating another Dbcontext for reading in CQRS
public class ReadModelDbContext : DbContext
{
    public DbSet<EventBase> CustomerEvents { get; set; }

    public ReadModelDbContext(DbContextOptions<ReadModelDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventBase>().ToView("events_view").HasNoKey().Ignore(e => e.Data).Ignore(e => e.EventId)
            .HasDiscriminator<string>("event_type")
            .HasValue<CustomerCreatedEvent>("customer_create")
            .HasValue<CustomerUpdatedEvent>("customer_update");
            
        modelBuilder.Entity<EventBase>().Property(e => e.Email).HasColumnName("Email");
        modelBuilder.Entity<EventBase>().Property(e => e.FirstName).HasColumnName("FirstName");
        modelBuilder.Entity<EventBase>().Property(e => e.LastName).HasColumnName("LastName");
        modelBuilder.Entity<EventBase>().Property(e => e.DateOfBirth).HasColumnName("DateOfBirth");
        modelBuilder.Entity<EventBase>().Property(e => e.BankAccount).HasColumnName("BankAccount");
        modelBuilder.Entity<EventBase>().Property(e => e.PhoneNumber).HasColumnName("PhoneNumber");

        modelBuilder.Entity<EventBase>().ToView("events_view").HasNoKey().Ignore(e => e.Data).Ignore(e => e.EventId)
            .HasDiscriminator<string>("event_type").HasValue<CustomerDeletedEvent>("customer_delete");
        // Additional configuration as needed
    }
}