using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Features.Routes;

public class RouteLocation : IEntity
{
    public Guid Id { get; private init; }
    public string Name { get; private init; } = null!;

    public static RouteLocation Create(Guid id, string name)
    {
        return new RouteLocation()
        {
            Id = id,
            Name = name
        };
    }
}