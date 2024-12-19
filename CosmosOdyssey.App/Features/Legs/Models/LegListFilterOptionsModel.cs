using CosmosOdyssey.App.Features.Fares.Models;

namespace CosmosOdyssey.App.Features.Legs.Models;

public class LegListFilterOptionsModel
{
    public LocationModel[] Locations { get; set; } = [];
    public CompanyModel[] Companies { get; set; } = [];
}