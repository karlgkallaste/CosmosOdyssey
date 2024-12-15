namespace CosmosOdyssey.Domain.Features.PriceLists;

public class LegRoute
{
    public Guid Id { get; private init; }

    public Guid PriceListId { get; private set; }
    // public RouteLocation From { get; private init; } = null!;
    // public RouteLocation To { get; private init; } = null!;
    public double Distance { get; private init; }

    public static LegRoute Create(Guid id, RouteLocation from, RouteLocation to, double distance)
    {
        return new LegRoute()
        {
            Id = id,
            //From = from,
            //To = to,
            Distance = distance
        };
    }
}