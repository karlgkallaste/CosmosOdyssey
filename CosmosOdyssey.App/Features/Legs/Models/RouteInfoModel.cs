namespace CosmosOdyssey.App.Features.Legs.Models;

public class RouteInfoModel
{
    public Guid Id { get; set; }
    public LegInfoModel From { get; set; }
    public LegInfoModel To { get; set; }
    public ProviderInfoModel[] Providers { get; set; }
    public double AveragePrice { get; set; }
}