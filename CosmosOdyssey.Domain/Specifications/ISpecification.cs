using System.Linq.Expressions;

namespace CosmosOdyssey.Domain.Specifications;

public abstract class Specification<T> : ISpecification<T>
{
    // Static None specification (matches everything)
    public static Specification<T> None => new NoneSpecification<T>();
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        return ToExpression().Compile().Invoke(entity);
    }

    // Overloading the += operator to combine specifications with logical AND
    public static Specification<T> operator +(Specification<T> left, Specification<T> right)
    {
        return left.And(right);
    }

    // Logical AND
    public Specification<T> And(Specification<T> other)
    {
        return new AndSpecification<T>(this, other);
    }
}

public class NoneSpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        return x => true; // Match everything
    }
}

public class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(leftExpression, parameter),
            Expression.Invoke(rightExpression, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
    bool IsSatisfiedBy(T entity);
}