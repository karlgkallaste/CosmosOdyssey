using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.Legs.Specifications;
using CosmosOdyssey.Domain.Specifications;

namespace CosmosOdyssey.App.Features.Fares.Models;

public class LegFiltersModel
{
    public Guid[] CompanyIds { get; }
    public Guid From { get; }
    public Guid To { get; }
    public double Price { get; set; }
    public double Distance { get; set; }


    public Specification<Leg> ToSpecification()
    {
        var spec = Specification<Leg>.None;


        if (CompanyIds.Any())
        {
            spec += new WithAnyGivenCompanyId(CompanyIds);
        }

        return spec;
    }
}