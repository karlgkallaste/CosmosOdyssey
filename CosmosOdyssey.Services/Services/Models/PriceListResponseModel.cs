using CosmosOdyssey.Domain.Features.Companies;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.Routes;
using Newtonsoft.Json;

namespace CosmosOdyssey.Services.Services.Models;

public class CompanyResponseModel
{
    [JsonProperty("id")] public Guid Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }
}

public class LocationResponseModel
{
    [JsonProperty("id")] public Guid Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }
}

public class LegResponseModel
{
    [JsonProperty("id")] public Guid Id { get; set; }

    [JsonProperty("routeInfo")] public RouteInfoResponseModel RouteInfo { get; set; }

    [JsonProperty("providers")] public List<ProviderResponseModel> Providers { get; set; }
}

public class ProviderResponseModel
{
    [JsonProperty("id")] public Guid Id { get; set; }

    [JsonProperty("company")] public CompanyResponseModel Company { get; set; }

    [JsonProperty("price")] public double Price { get; set; }

    [JsonProperty("flightStart")] public DateTime FlightStart { get; set; }

    [JsonProperty("flightEnd")] public DateTime FlightEnd { get; set; }

    public Provider ToDomainObject()
    {
        return new Provider(Id, new Company(Company.Id, Company.Name), Price, FlightStart, FlightEnd);
    }
}

public class PriceListResponseModel
{
    [JsonProperty("id")] public Guid Id { get; set; }

    [JsonProperty("validUntil")] public DateTime ValidUntil { get; set; }

    [JsonProperty("legs")] public List<LegResponseModel> Legs { get; set; }
}

public class RouteInfoResponseModel
{
    [JsonProperty("id")] public Guid Id { get; set; }

    [JsonProperty("from")] public LocationResponseModel From { get; set; }

    [JsonProperty("to")] public LocationResponseModel To { get; set; }

    [JsonProperty("distance")] public double Distance { get; set; }

    public Route ToDomainObject()
    {
        return new Route(Id, new RouteLocation(From.Id, From.Name), new RouteLocation(To.Id, To.Name), Distance);
    }
}