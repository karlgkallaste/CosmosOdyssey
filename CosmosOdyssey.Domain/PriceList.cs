namespace CosmosOdyssey.Domain;

public class PriceList
{
    public Guid Id { get; private set; }
    public DateTimeOffset ValidUntil { get; private set; }
}