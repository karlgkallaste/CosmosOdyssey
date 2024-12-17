using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Routes;

namespace CosmosOdyssey.Domain.Features.Legs;

public class PriceListLeg : IEntity
{
    public Guid Id { get; private set; }
    public LegRoute Route { get;  private set; }
    public List<LegProvider> Providers { get; private set; } = new List<LegProvider>();


    public interface IBuilder
    {
        IBuilder WithId(Guid id);
        IBuilder WithRoute(LegRoute route);
        IBuilder WithProviders(params LegProvider[] routes);
        PriceListLeg Build();
    }

    public class Builder : IBuilder
    {
        private PriceListLeg _priceListLeg = new PriceListLeg();

        public IBuilder WithId(Guid id)
        {
            _priceListLeg.Id = id;
            return this;
        }

        public IBuilder WithRoute(LegRoute route)
        {
            _priceListLeg.Route = route;
            return this;
        }

        public IBuilder WithProviders(params LegProvider[] routes)
        {
            _priceListLeg.Providers = routes.ToList();
            return this;
        }

        public PriceListLeg Build()
        {
            return _priceListLeg;
        }
    }
}