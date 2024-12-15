using CosmosOdyssey.Data.Configurations;
using CosmosOdyssey.Domain.Features.PriceLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CosmosOdyssey.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<PriceList> PriceLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PriceListConfiguration());
        modelBuilder.ApplyConfiguration(new PriceListLegConfiguration());
        modelBuilder.ApplyConfiguration(new LegRouteConfiguration());
        modelBuilder.ApplyConfiguration(new LegProviderConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}