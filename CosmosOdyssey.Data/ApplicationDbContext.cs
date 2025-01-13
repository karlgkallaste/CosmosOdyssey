using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CosmosOdyssey.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<PriceList> PriceLists { get; set; }
    public DbSet<Reservation> Reservations { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)); // Suppress the transaction warning
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PriceList>()
            .Property(p => p.Legs)
            .HasColumnType("jsonb")
            .HasConversion(
                x => JsonConvert.SerializeObject(x),
                x => JsonConvert.DeserializeObject<List<Leg>>(x) ?? new List<Leg>());


        // Reservation Configuration
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasOne<PriceList>()
                .WithMany() // Assuming PriceList can have many Reservations
                .HasForeignKey(r => r.PriceListId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(r => r.Routes)
                .WithOne(route => route.Reservation)
                .HasForeignKey(route => route.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete Routes when Reservation is deleted
        });

        // ReservationRoute Configuration
        modelBuilder.Entity<ReservationRoute>(entity =>
        {
            // Primary key for ReservationRoute
            entity.HasKey(route => route.Id); // Keep Id as the primary key

            // Define the foreign key for Reservation
            entity.HasOne(route => route.Reservation)
                .WithMany(reservation => reservation.Routes)
                .HasForeignKey(route => route.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=cosmos_odyssey;Username=cosmos;Password=odyssey");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}