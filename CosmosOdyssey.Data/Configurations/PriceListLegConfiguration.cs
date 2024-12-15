using CosmosOdyssey.Domain.Features.PriceLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmosOdyssey.Data.Configurations;

public class PriceListLegConfiguration : IEntityTypeConfiguration<PriceListLeg>
{
    public void Configure(EntityTypeBuilder<PriceListLeg> builder)
    {
        builder.HasKey(pll => pll.Id);
        builder.HasOne<PriceList>()
            .WithMany(pl => pl.Legs)
            .HasForeignKey(pll => pll.PriceListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}