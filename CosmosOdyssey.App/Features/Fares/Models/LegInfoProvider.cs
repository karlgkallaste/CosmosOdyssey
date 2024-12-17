using CosmosOdyssey.Domain.Features.Legs;

namespace CosmosOdyssey.App.Features.Fares.Models;

public interface ILegInfoProvider
{
    LegInfoModel Provide(PriceListLeg priceListLeg);
}

public class LegInfoProvider : ILegInfoProvider
{
    public LegInfoModel Provide(PriceListLeg priceListLeg)
    {
        return new LegInfoModel()
        {
            From = priceListLeg.Route.From.Name,
            To = priceListLeg.Route.To.Name,
            Distance = priceListLeg.Route.Distance,
            Providers = priceListLeg.Providers.Select(x => new LegProviderModel
            {
                Name = x.Company.Name,
                Departs = x.FlightStart.Date,
                Arrives = x.FlightEnd.Date,
                Price = x.Price
            }).ToArray()
        };
    }
}

public class LegInfoModel
{
    public string From { get; set; }
    public string To { get; set; }
    public double Distance { get; set; }
    public LegProviderModel[] Providers { get; set; }
}

public class LegProviderModel
{
    public string Name { get; set; }
    public DateTimeOffset Departs { get; set; }
    public DateTimeOffset Arrives { get; set; }
    public double Price { get; set; }
}