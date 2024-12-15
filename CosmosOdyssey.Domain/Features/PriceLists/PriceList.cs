namespace CosmosOdyssey.Domain.Features.PriceLists;

public record PriceList
{
    public Guid Id { get; private set; }
    public DateTimeOffset ValidUntil { get; private set; }
    public ICollection<PriceListLeg> Legs { get; private set; } = new List<PriceListLeg>();


    public interface IBuilder
    {
        IBuilder WithId(Guid id);
        IBuilder WithValidUntil(DateTimeOffset validUntil);
        IBuilder WithLegs(params PriceListLeg[] legs);
        PriceList Build();
    }

    public class Builder : IBuilder
    {
        private PriceList _priceList = new PriceList();

        public IBuilder WithId(Guid id)
        {
            _priceList.Id = id;
            return this;
        }

        public IBuilder WithValidUntil(DateTimeOffset validUntil)
        {
            _priceList.ValidUntil = validUntil;
            return this;
        }

        public IBuilder WithLegs(params PriceListLeg[] legs)
        {
            _priceList.Legs = legs.ToList();
            return this;
        }

        public PriceList Build()
        {
            return _priceList;
        }
    }
}