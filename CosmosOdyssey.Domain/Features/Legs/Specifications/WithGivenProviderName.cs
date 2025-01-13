using System.Linq.Expressions;
using CosmosOdyssey.Domain.Specifications;

namespace CosmosOdyssey.Domain.Features.Legs.Specifications;

public class WithGivenProviderName : Specification<Provider>
{
    public string Name { get; private set; }

    public WithGivenProviderName(string name)
    {
        Name = name;
    }


    public override Expression<Func<Provider, bool>> ToExpression()
    {
        return x => x.Company.Name.Contains(Name, StringComparison.CurrentCultureIgnoreCase);
    }
}