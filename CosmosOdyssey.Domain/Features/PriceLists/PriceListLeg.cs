using System.Collections.Immutable;

namespace CosmosOdyssey.Domain.Features.PriceLists;

public class PriceListLeg
{
    public Guid Id { get; private set; }
    public ImmutableList<LegRoute> Routes { get; private set; } = null!;
    public ImmutableList<LegProvider> Providers { get; private set; } = null!;

    public interface IBuilder
    {
        IBuilder WithId(Guid id);
        IBuilder WithRoutes(params LegRoute[] routes);
        IBuilder WithProviders(params LegProvider[] routes);
        PriceListLeg Build();
    }

    public class Builder : IBuilder
    {
        private PriceListLeg _priceListLeg = new PriceListLeg();

        public IBuilder WithId(Guid id)
        {
            _priceListLeg.Id = id;
            return this;
        }

        public IBuilder WithRoutes(params LegRoute[] routes)
        {
            _priceListLeg.Routes = routes.ToImmutableList();
            return this;
        }

        public IBuilder WithProviders(params LegProvider[] routes)
        {
            _priceListLeg.Providers = routes.ToImmutableList();
            return this;
        }

        public PriceListLeg Build()
        {
            return _priceListLeg;
        }
    }
}

public class LegRoute
{
    public Guid Id { get; private init; }
    public RouteLocation From { get; private init; } = null!;
    public RouteLocation To { get; private init; } = null!;
    public double Distance { get; private init; }

    public static LegRoute CreateFromResponse(Guid id, RouteLocation from, RouteLocation to, double distance)
    {
        return new LegRoute()
        {
            Id = Guid.NewGuid(),
            From = from,
            To = to,
            Distance = distance
        };
    }
}

public class RouteLocation
{
    public Guid Id { get; private init; }
    public string Name { get; private init; } = null!;

    public static RouteLocation CreateFromResponse(Guid id, string name)
    {
        return new RouteLocation()
        {
            Id = id,
            Name = name
        };
    }
}

public class LegProvider
{
    public Guid Id { get; private init; }
    public Company Company { get; private init; } = null!;
    public double Price { get; private init; }
    public DateTimeOffset FlightStart { get; private init; }
    public DateTimeOffset FlightEnd { get; private init; }

    public static LegProvider CreateFromResponse(Guid id, Company company, double price, DateTimeOffset flightStart,
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

public class Company
{
    public Guid Id { get; private init; }
    public string Name { get; private init; } = null!;

    public static Company CreateFromResponse(Guid id, string name)
    {
        return new Company()
        {
            Id = id,
            Name = name
        };
    }
}