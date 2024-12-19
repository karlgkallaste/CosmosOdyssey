using System.Linq.Expressions;
using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Specifications;

public class WithAnyGivenId<T> : Specification<T> where T : IEntity
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        throw new NotImplementedException();
    }
}