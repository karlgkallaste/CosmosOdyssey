using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Features.Routes;

public class LegRoute : IEntity
{
    public Guid Id { get; private init; }
    public Guid FromId { get; private init; }
    public RouteLocation From { get; private init; }
    public Guid ToId { get; private init; }
    public RouteLocation To { get; private init; }
    public double Distance { get; private init; }

    public static LegRoute Create(Guid id, RouteLocation from, RouteLocation to, double distance)
    {
        return new LegRoute()
        {
            Id = id,
            From = from,
            To = to,
            Distance = distance
        };
    }
}