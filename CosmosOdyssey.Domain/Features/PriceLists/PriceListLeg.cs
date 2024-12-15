namespace CosmosOdyssey.Domain.Features.PriceLists;

public class PriceListLeg
{
    public Guid Id { get; private set; }
    public Guid PriceListId { get; private set; }
    public ICollection<LegRoute> Routes { get; private set; } = null!;
    public ICollection<LegProvider> Providers { get; private set; } = null!;


    public interface IBuilder
    {
        IBuilder WithId(Guid id);
        IBuilder WithRoutes(params LegRoute[] routes);
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

        public IBuilder WithRoutes(params LegRoute[] routes)
        {
            _priceListLeg.Routes = routes.ToList();
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