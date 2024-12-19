using System.Linq.Expressions;
using CosmosOdyssey.Domain.Specifications;

namespace CosmosOdyssey.Domain.Features.Legs.Specifications;

public class WithAnyGivenCompanyId : Specification<Leg>
{
    public Guid[] CompanyIds { get; private set; }

    public WithAnyGivenCompanyId(params Guid[] companyIds)
    {
        CompanyIds = companyIds;
    }

    public override Expression<Func<Leg, bool>> ToExpression()
    {
        return x => x.Providers.Any(y => CompanyIds.Contains(y.Id));
    }
}