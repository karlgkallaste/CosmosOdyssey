using CosmosOdyssey.Domain.Features.PriceLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmosOdyssey.Data.Configurations;

public class LegRouteConfiguration : IEntityTypeConfiguration<LegRoute>
{
    public void Configure(EntityTypeBuilder<LegRoute> builder)
    {
        builder.HasKey(lr => lr.Id);

        builder.Property(lr => lr.Distance)
            .IsRequired();
    }
}