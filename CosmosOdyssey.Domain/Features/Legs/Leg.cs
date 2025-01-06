using System.Text.Json.Serialization;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Routes;

namespace CosmosOdyssey.Domain.Features.Legs;

public class Leg : IEntity
{
    public Guid Id { get; private set; }
    public Route Route { get; private set; } = null!;
    public List<Provider> Providers { get; private set; } = [];

    [JsonConstructor]
    public Leg(Guid id, Route route, List<Provider> providers)
    {
        Id = id;
        Route = route ?? throw new ArgumentNullException(nameof(route));
        Providers = providers ?? new List<Provider>();
    }
    
    protected Leg() { }
    public interface IBuilder
    {
        IBuilder WithId(Guid id);
        IBuilder WithRoute(Route route);
        IBuilder WithProviders(params Provider[] routes);
        Leg Build();
    }

    public class Builder : IBuilder
    {
        private readonly Leg _leg = new Leg();

        public IBuilder WithId(Guid id)
        {
            _leg.Id = id;
            return this;
        }

        public IBuilder WithRoute(Route route)
        {
            _leg.Route = route;
            return this;
        }

        public IBuilder WithProviders(params Provider[] routes)
        {
            _leg.Providers = routes.ToList();
            return this;
        }

        public Leg Build()
        {
            var clone = (Leg)_leg.MemberwiseClone();
            return clone;
        }
    }
}