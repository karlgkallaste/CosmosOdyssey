using CosmosOdyssey.Domain.Features.PriceLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmosOdyssey.Data.Configurations;

public class LegProviderConfiguration : IEntityTypeConfiguration<LegProvider>
{
    public void Configure(EntityTypeBuilder<LegProvider> builder)
    {
        builder.HasKey(lp => lp.Id);
        
        builder.Property(lp => lp.Price)
            .IsRequired();
    }
}