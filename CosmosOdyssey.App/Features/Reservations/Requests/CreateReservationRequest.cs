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
            RuleFor(x => x.Name.FirstName).NotEmpty().WithMessage("First name cannot be empty").When(x => x.Name != null);
            RuleFor(x => x.Name.LastName).NotEmpty().WithMessage("Last name cannot be empty").When(x => x.Name != null);
            RuleFor(x => x.Routes).NotEmpty().WithMessage("Routes cannot be empty");
        }
    }
}

public record PersonNameModel(string FirstName, string LastName)
{
    public Customer ToDomainObject()
    {
        return new Customer(Guid.Empty, FirstName, LastName);
    }
}

public record ReservationRouteModel(Guid CompanyId, Guid LegId, double Price, DateTimeOffset Departure, DateTimeOffset Arrival)
{
    public ReserveRoute ToDomainObject()
    {
        return new ReserveRoute(CompanyId, LegId);
    }
}