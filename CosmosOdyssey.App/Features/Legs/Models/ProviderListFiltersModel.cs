using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.Legs.Specifications;
using CosmosOdyssey.Domain.Specifications;

namespace CosmosOdyssey.App.Features.Legs.Models;

public class ProviderListFiltersModel
{
    public Guid LegId { get; set; }
    public Guid PriceListId { get; set; }

    public double? PriceLimit { get; set; }

    public DateTimeOffset? ArriveBy { get; set; }
    public string? CompanyName { get; set; }
    public string? SortBy { get; set; }

    public Specification<Provider> ToSpecification()
    {
        var spec = Specification<Provider>.None;

        if (!string.IsNullOrWhiteSpace(CompanyName))
        {
            spec += new WithGivenProviderName(CompanyName);
        }

        if (PriceLimit.HasValue)
        {
            spec += new ProviderPriceBelow(PriceLimit.Value);
        }

        if (ArriveBy.HasValue)
        {
            spec += new ProviderArrivesBy(ArriveBy.Value);
        }

        return spec;
    }

    public Func<IQueryable<Provider>, IOrderedQueryable<Provider>> ToSorting()
    {
        if (string.IsNullOrWhiteSpace(SortBy))
        {
            return null;
        }

        return SortBy.ToLower() switch
        {
            "price" => (providers) => providers.OrderBy(p => p.Price),
            "flightstart" => (providers) => providers.OrderBy(p => p.FlightStart),
            "flightend" => (providers) => providers.OrderBy(p => p.FlightEnd),
            _ => (providers) => providers.OrderBy(p => p.Price), // Default to sorting by price if unknown
        };
    }
}