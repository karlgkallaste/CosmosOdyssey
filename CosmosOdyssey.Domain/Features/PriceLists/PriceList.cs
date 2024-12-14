using System.Collections.Immutable;

namespace CosmosOdyssey.Domain.Features.PriceLists;

public record PriceList
{
    public Guid Id { get; private set; }
    public DateTimeOffset ValidUntil { get; private set; }

    public ImmutableList<PriceListLeg> Legs { get; private set; } = ImmutableList<PriceListLeg>.Empty;


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
            _priceList.Legs = legs.ToImmutableList();
            return this;
        }

        public PriceList Build()
        {
            return _priceList;
        }
    }
}