using CosmosOdyssey.App.Features.Reservations.Requests;
using CosmosOdyssey.Domain.Features.Reservations;

namespace CosmosOdyssey.App.Features.Reservations.Models;

public class ReservationListItemModel
{
    public Guid Id { get; set; }
    public PersonNameModel Name { get; set; }
    public string From { get; set; }
    public string To { get; set; }

    public ReservationListItemModel(Reservation reservation)
    {
        var routes = reservation.Routes.OrderBy(x => x.Depart).ToArray();
        Id = reservation.Id;
        Name = new PersonNameModel(reservation.Customer.FirstName, reservation.Customer.LastName);
        From = routes.First().From;
        To = routes.Last().To;
    }
}