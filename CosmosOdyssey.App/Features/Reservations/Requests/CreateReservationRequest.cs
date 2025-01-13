using CosmosOdyssey.Domain.Features.Reservations;
using CosmosOdyssey.Domain.Features.Users;
using FluentValidation;
using Newtonsoft.Json;

namespace CosmosOdyssey.App.Features.Reservations.Requests;

public class CreateReservationRequest
{
    public Guid PriceListId { get; set; }
    public PersonNameModel Name { get; set; }
    public ReservationRouteModel[] Routes { get; set; }


    public class Validator : AbstractValidator<CreateReservationRequest>
    {
        public Validator()
        {
            RuleFor(x => x.PriceListId).NotEmpty().WithMessage("Price list id cannot be empty");
            RuleFor(x => x.Name).NotNull().WithMessage("Name is required");
            RuleFor(x => x.Name.FirstName).NotEmpty().WithMessage("First name cannot be empty")
                .When(x => x.Name != null);
            RuleFor(x => x.Name.LastName).NotEmpty().WithMessage("Last name cannot be empty").When(x => x.Name != null);
            RuleFor(x => x.Routes)
                .NotEmpty().NotNull().WithMessage("Route providers are required")
                .Must((request, routes) => AreStartTimesValid(routes))
                .WithMessage(
                    "Each route's start time must be greater than the last arrival time of the previous route.");
        }
    }

    private static bool AreStartTimesValid(ReservationRouteModel[] routes)
    {
        if (routes == null || routes.Length == 0)
            return false;

        for (int i = 1; i < routes.Length; i++)
        {
            var previousRoute = routes[i - 1];
            var currentRoute = routes[i];

            if (previousRoute == null || currentRoute == null)
                return false;

            if (currentRoute.Departure <= previousRoute.Arrival)
                return false;
        }

        return true;
    }
}

public record PersonNameModel(string FirstName, string LastName)
{
    public Customer ToDomainObject()
    {
        return new Customer(Guid.NewGuid(), FirstName, LastName);
    }
}

public class ReservationRouteModel

{
    public Guid CompanyId { get; set; }
    public Guid LegId { get; set; }
    public double Price { get; set; }
    public DateTimeOffset Departure { get; set; }
    public DateTimeOffset Arrival { get; set; }

    public ReservationRouteModel()
    {
    }

    public ReserveRoute ToDomainObject()
    {
        return new ReserveRoute(CompanyId, LegId);
    }
}