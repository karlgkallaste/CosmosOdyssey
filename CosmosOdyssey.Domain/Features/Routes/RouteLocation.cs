using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Features.Routes;

public class RouteLocation : IEntity
{
    public string Name { get; private init; } = null!;
    public Guid Id { get; private init; }
    
    protected RouteLocation() { }

    public RouteLocation(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}