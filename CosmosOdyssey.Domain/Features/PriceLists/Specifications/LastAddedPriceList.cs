using System.Linq.Expressions;
using CosmosOdyssey.Domain.Specifications;

namespace CosmosOdyssey.Domain.Features.PriceLists.Specifications;

public class ValidUntilNotPassed : Specification<PriceList>
{
    public ValidUntilNotPassed(DateTime date)
    {
        Date = date;
    }

    public DateTime Date { get; private set; }

    public override Expression<Func<PriceList, bool>> ToExpression()
    {
        return x => x.ValidUntil > Date.ToUniversalTime(); 
    }
}