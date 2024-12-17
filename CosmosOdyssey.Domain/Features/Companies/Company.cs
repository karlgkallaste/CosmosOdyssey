using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Features.Companies;

public class Company : IEntity
{
    public Guid Id { get; private init; }
    public string Name { get; private init; } = null!;

    public static Company Create(Guid id, string name)
    {
        return new Company()
        {
            Id = id,
            Name = name,
        };
    }
}