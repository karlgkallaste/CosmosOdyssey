using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;

namespace CosmosOdyssey.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<PriceList> PriceLists { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

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

            // Configure the foreign key for Customer
            entity.HasOne(r => r.Customer)
                .WithMany() // Assuming Customer can have many Reservations
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship with ReservationRoute
            entity.HasMany(r => r.Routes)
                .WithOne(route => route.Reservation)
                .HasForeignKey(route => route.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<ReservationRoute>(entity =>
        {
            // Primary key for ReservationRoute
            entity.HasKey(route => route.Id);

            // Configure foreign key relationship with Reservation
            entity.HasOne(route => route.Reservation)
                .WithMany(r => r.Routes)
                .HasForeignKey(route => route.ReservationId);
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