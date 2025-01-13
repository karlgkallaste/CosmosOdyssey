using System.Linq.Expressions;
using CosmosOdyssey.Domain.Specifications;

namespace CosmosOdyssey.Domain.Features.Legs.Specifications;

public class ProviderArrivesBy : Specification<Provider>
{
    public DateTimeOffset ArriveBy { get; private set; }

    public ProviderArrivesBy(DateTimeOffset arriveBy)
    {
        ArriveBy = arriveBy;
    }

    public override Expression<Func<Provider, bool>> ToExpression()
    {
        return x => x.FlightEnd.ToUniversalTime().Date > ArriveBy.UtcDateTime;
    }
}