namespace CosmosOdyssey.App.Features.Legs.Models;

public class ProviderInfoModel
{
    public Guid Id { get; set; }
    public CompanyInfoModel Company { get; set; }
    public double Price { get; set; }
    public DateTime FlightStart { get; set; }
    public DateTime FlightEnd { get; set; }
}