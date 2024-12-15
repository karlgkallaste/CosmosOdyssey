namespace CosmosOdyssey.Domain.Features.PriceLists;

public class Company
{
    public Guid Id { get; private init; }
    public Guid CompanyId { get; private init; } // Foreign key

    public string Name { get; private init; } = null!;

    public static Company Create(Guid id, string name)
    {
        return new Company()
        {
            Id = id,
            Name = name
        };
    }
}