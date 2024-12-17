using CosmosOdyssey.Domain.Features.Companies;
using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Features.Legs;

public class LegProvider : IEntity
{
    public Guid Id { get; set; }
    public Company Company { get; set; } = null!;
    public double Price { get; set; }
    public DateTime FlightStart { get; set; }
    public DateTime FlightEnd { get; set; }

    public static LegProvider Create(Guid id, Company company, double price, DateTime flightStart,
        DateTime flightEnd)
    {
        return new LegProvider()
        {
            Id = id,
            Company = company,
            Price = price,
            FlightStart = flightStart,
            FlightEnd = flightEnd
        };
    }
}