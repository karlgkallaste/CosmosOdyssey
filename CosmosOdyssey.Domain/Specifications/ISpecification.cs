using System.Linq.Expressions;

namespace CosmosOdyssey.Domain.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
}