using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Users;

namespace CosmosOdyssey.Domain.Features.Reservations;

public record Reservation : IEntity
{
    public Guid Id { get; private set; }
    public Guid PriceListId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public ICollection<ReservationRoute> Routes { get; private set; } = new List<ReservationRoute>();
    
    
    public interface IBuilder
    {
        IBuilder WithId(Guid id);
        IBuilder WithPriceListId(Guid id);
        IBuilder WithCustomer(Customer customer);
        IBuilder WithRoutes(ICollection<ReservationRoute> routes);
        Reservation Build();
    }
    
    public class Builder : IBuilder
    {
        private readonly Reservation _reservation = new();
        public IBuilder WithId(Guid id)
        {
            _reservation.Id = id;
            return this;
        }

        public IBuilder WithPriceListId(Guid id)
        {
            _reservation.PriceListId = id;
            return this;
        }

        public IBuilder WithCustomer(Customer name)
        {
            _reservation.Customer = name;
            return this;
        }

        public IBuilder WithRoutes(ICollection<ReservationRoute> routes)
        {
            _reservation.Routes = routes;
            return this;
        }

        public Reservation Build()
        {
            var clone = (Reservation)_reservation.MemberwiseClone();
            return clone;
        }
    }
}

public record ReserveRoute(Guid CompanyId, Guid LegId);
public class ReservationRoute
{
    public Guid Id { get; set; }
    public Guid ReservationId { get; set; } // Foreign key to Reservation
    public string From { get; set; }
    public string To { get; set; }
    public double TotalTimeInMinutes { get; set; }
    public double Price { get; set; }
    public string Company { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Depart { get; set; }
    
    // Navigation property
    public Reservation Reservation { get; set; }
    
}