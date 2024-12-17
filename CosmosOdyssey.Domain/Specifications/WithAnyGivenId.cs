using System.Linq.Expressions;
using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Specifications;

public class WithAnyGivenId<T> : ISpecification<T> where T : IEntity
{
    public Guid[] Ids { get; private set; }

    public WithAnyGivenId(Guid[] ids)
    {
        Ids = ids;
    }


    public Expression<Func<T, bool>> ToExpression()
    {
        throw new NotImplementedException();
    }
}