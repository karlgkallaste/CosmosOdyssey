using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PriceList - Leg Relationship
        modelBuilder.Entity<PriceList>()
            .Property(p => p.Legs)
            .HasColumnType("jsonb")
            .HasConversion(
                x => JsonConvert.SerializeObject(x),
                x => JsonConvert.DeserializeObject<List<Leg>>(x) ?? new List<Leg>());
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