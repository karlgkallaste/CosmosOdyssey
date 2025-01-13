using CosmosOdyssey.App.Features.Reservations.Requests;
using CosmosOdyssey.Domain.Features.Reservations;

namespace CosmosOdyssey.App.Features.Reservations.Models;

public class ReservationDetailsModel
{
    public Guid Id { get; set; }
    public PersonNameModel Customer { get; set; }
    public ReservationRouteDetailsModel[] Routes { get; set; }
}

public interface IReservationProvider
{
    public ReservationDetailsModel Provide(Reservation reservation);
}

public class ReservationProvider : IReservationProvider
{
    public ReservationDetailsModel Provide(Reservation reservation)
    {
        return new ReservationDetailsModel()
        {
            Id = reservation.Id,
            Customer = new PersonNameModel(reservation.Customer.FirstName, reservation.Customer.LastName),
            Routes = reservation.Routes.Select(x => new ReservationRouteDetailsModel()
            {
                Time = x.TotalTimeInMinutes / 60,
                Price = x.Price,
                From = x.From,
                To = x.To
            }).ToArray()
        };
    }
}

public class ReservationRouteDetailsModel
{
    public double Time { get; set; }
    public double Price { get; set; }
    public string From { get; set; }
    public string To { get; set; }
}