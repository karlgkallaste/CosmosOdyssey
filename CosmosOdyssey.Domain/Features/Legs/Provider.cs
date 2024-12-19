using CosmosOdyssey.Domain.Features.Companies;
using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Features.Legs;

public class Provider : IEntity
{
    public Guid Id { get; private init; }
    public Company Company { get; private init; } = null!;
    public double Price { get; private init; }
    public DateTime FlightStart { get; private init; }
    public DateTime FlightEnd { get; private init; }

    public Provider(Guid id, Company company, double price, DateTime flightStart, DateTime flightEnd)
    {
        Id = id;
        Company = company;
        Price = price;
        FlightStart = flightStart;
        FlightEnd = flightEnd;
    }
}