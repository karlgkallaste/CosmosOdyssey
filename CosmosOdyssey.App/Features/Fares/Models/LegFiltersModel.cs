using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Specifications;

namespace CosmosOdyssey.App.Features.Fares.Models;

public class LegFiltersModel
{
    public Guid[] CompanyIds { get; private set; }
    public Guid From { get; private set; }
    public Guid To { get; private set; }
    
    
    public ISpecification<PriceListLeg> ToSpecification()
    {
        
        throw new NotImplementedException();
    }
}