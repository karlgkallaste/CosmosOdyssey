using CosmosOdyssey.Domain.Features.PriceLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmosOdyssey.Data.Configurations;

public class RouteLocationConfiguration : IEntityTypeConfiguration<RouteLocation>
{
    public void Configure(EntityTypeBuilder<RouteLocation> builder)
    {
        builder.HasKey(pll => pll.Id);
        builder.HasOne<PriceListLeg>()
            .WithMany()
            .HasForeignKey(r => r.RouteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}