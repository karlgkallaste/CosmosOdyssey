namespace CosmosOdyssey.App.Features.Fares.Models;

public class FareListFiltersModel
{
    public LocationModel[] Locations { get; set; } = [];
    public CompanyModel[] Companies { get; set; } = [];
}