namespace CosmosOdyssey.Domain.Features.PriceLists;

public class PriceList
{
    public Guid Id { get; private set; }
    public DateTimeOffset ValidUntil { get; private set; }
}