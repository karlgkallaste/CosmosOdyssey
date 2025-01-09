using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Features.Routes;

public class Route : IEntity
{
    public Guid Id { get; private set; }
    public RouteLocation To { get; private set; }
    public RouteLocation From { get; private set; }
    public double Distance { get; private set; }

    
    protected Route(){}
    public Route(Guid id, RouteLocation from, RouteLocation to, double distance)
    {
        Id = id;
        From = from;
        To = to;
        Distance = distance;
    }
}