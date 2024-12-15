namespace CosmosOdyssey.Domain.Features.PriceLists;

public class LegProvider
{
    public Guid Id { get; private init; }
    public Guid PriceListId { get; private set; }
    public Company Company { get; private init; } = null!;
    public double Price { get; private init; }
    public DateTimeOffset FlightStart { get; private init; }
    public DateTimeOffset FlightEnd { get; private init; }

    public static LegProvider Create(Guid id, Company company, double price, DateTimeOffset flightStart,
        DateTimeOffset flightEnd)
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