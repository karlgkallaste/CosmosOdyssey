namespace CosmosOdyssey.Domain.Features.PriceLists;

public class RouteLocation
{
    public Guid Id { get; private init; }
    public Guid RouteId  { get; private init; }
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