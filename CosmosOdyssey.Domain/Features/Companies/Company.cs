using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Features.Companies;

public class Company : IEntity
{
    protected Company() { }

    public string Name { get; private init; } = null!;
    public Guid Id { get; private init; }

    public Company(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Company Create(Guid id, string name)
    {
        return new Company(id, name);
    }
}