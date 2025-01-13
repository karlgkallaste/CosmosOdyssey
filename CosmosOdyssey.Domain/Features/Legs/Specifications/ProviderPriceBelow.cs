using System.Linq.Expressions;
using CosmosOdyssey.Domain.Specifications;

namespace CosmosOdyssey.Domain.Features.Legs.Specifications;

public class ProviderPriceBelow : Specification<Provider>
{
    public double PriceLimit { get; private set; }

    public ProviderPriceBelow(double priceLimit)
    {
        PriceLimit = priceLimit;
    }

    public override Expression<Func<Provider, bool>> ToExpression()
    {
        return x => x.Price < PriceLimit;
    }
}