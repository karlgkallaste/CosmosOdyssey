using CosmosOdyssey.Domain.Features.Legs;

namespace CosmosOdyssey.Domain.Features.PriceLists;

public record PriceList : IEntity
{
    public DateTime ValidUntil { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<Leg> Legs { get; private set; } = new();
    public Guid Id { get; private set; }


    public interface IBuilder
    {
        IBuilder WithId(Guid id);
        IBuilder WithValidUntil(DateTime validUntil);
        IBuilder WithLegs(params Leg[] legs);
        IBuilder WithCreatedAt(DateTime createdAt);
        PriceList Build();
    }

    public class Builder : IBuilder
    {
        private readonly PriceList _priceList = new();

        public IBuilder WithId(Guid id)
        {
            _priceList.Id = id;
            return this;
        }

        public IBuilder WithValidUntil(DateTime validUntil)
        {
            _priceList.ValidUntil = validUntil;
            return this;
        }

        public IBuilder WithLegs(params Leg[] legs)
        {
            _priceList.Legs = legs.ToList();
            return this;
        }

        public IBuilder WithCreatedAt(DateTime createdAt)
        {
            _priceList.CreatedAt = createdAt.ToUniversalTime();
            return this;
        }

        public PriceList Build()
        {
            _priceList.Id = Guid.NewGuid();
            return _priceList;
        }
    }
}