using System.Linq.Expressions;
using CosmosOdyssey.Domain.Specifications;

namespace CosmosOdyssey.Domain.Features.Reservations.Specifications;

public class WithCustomerName : ISpecification<Reservation>
{
    public string LastName { get; private set; }

    public WithCustomerName(string lastName)
    {
        LastName = lastName;
    }

    public Expression<Func<Reservation, bool>> ToExpression()
    {
        return x => x.Customer.LastName.Equals(LastName);
    }
}