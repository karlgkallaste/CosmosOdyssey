using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
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


        modelBuilder.Entity<Reservation>(entity =>
        {
            // Primary key for Reservation
            entity.HasKey(r => r.Id);

            // Configure the foreign key for PriceList
            entity.HasOne<PriceList>()
                .WithMany() // Assuming PriceList can have many Reservations
                .HasForeignKey(r => r.PriceListId)
                .OnDelete(DeleteBehavior.Cascade);


            // Configure the relationship with Customer
            entity.HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(r => r.Routes)
                .WithOne(route => route.Reservation)
                .HasForeignKey(route => route.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete Routes when Reservation is deleted
        });

        modelBuilder.Entity<ReservationRoute>(entity =>
        {
            // Primary key for ReservationRoute
            entity.HasKey(route => route.Id);

            // Define the relationship with Reservation
            entity.HasOne(route => route.Reservation)
                .WithMany(reservation => reservation.Routes)
                .HasForeignKey(route => route.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint on ReservationId, From, and To
            entity.HasIndex(route => new { route.ReservationId, route.From, route.To })
                .IsUnique()
                .HasDatabaseName("IX_ReservationRoute_UniqueFromToPerReservation");
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