using System.Linq.Expressions;
using CosmosOdyssey.Domain.Features.PriceLists;

namespace CosmosOdyssey.Domain.Specifications;

public class WithAnyGivenId<T> : Specification<T> where T : IEntity
{
    public Guid Id { get; private set; }

    public WithAnyGivenId(Guid id)
    {
        Id = id;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        return e => e.Id == Id;
    }
}