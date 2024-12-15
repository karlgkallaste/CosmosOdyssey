using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Services.PriceListServices.Models;

public class PriceListResponseModel
{
    public Guid Id { get; set; }
    public DateTimeOffset ValidUntil { get; set; }
    public PriceListLegResponseModel[] Legs { get; set; } = null!;
}

public class PriceListLegResponseModel
{
    public Guid Id { get; set; }
    public LegRouteInfoResponseModel[] RouteInfo { get; set; } = null!;
    public LegProviderResponseModel[] Providers { get; set; } = null!;
}

public class LegRouteInfoResponseModel
{
    public Guid Id { get; set; }
    public RouteLocationResponseModel From { get; set; } = null!;
    public RouteLocationResponseModel To { get; set; } = null!;
    public double Distance { get; set; }

    public LegRoute ToDomainObject()
    {
        return LegRoute.Create(Id, RouteLocation.Create(From.Id, From.Name), RouteLocation.Create(To.Id, To.Name),
            Distance);
    }
}

public class RouteLocationResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public class LegProviderResponseModel
{
    public Guid Id { get; set; }
    public ProviderCompanyResponseModel Company { get; set; } = null!;
    public double Price { get; set; }
    public DateTime FlightStart { get; set; }
    public DateTime FlightEnd { get; set; }

    public LegProvider ToDomainObject()
    {
        return LegProvider.Create(Id, Domain.Features.PriceLists.Company.Create(Company.Id, Company.Name), Price,
            FlightStart, FlightEnd);
    }
}

public class ProviderCompanyResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}