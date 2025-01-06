using CosmosOdyssey.Domain.Features.Users;
using FluentValidation;

namespace CosmosOdyssey.App.Features.Reservations.Requests;

public class CreateReservationRequest
{
    public Guid PriceListId { get; set; }
    public PersonNameModel Name { get; set; }
   // public ReservationRoute[] Routes { get; set; }


    public class Validator : AbstractValidator<CreateReservationRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Name.FirstName).NotEmpty().WithMessage("First name cannot be empty");
            RuleFor(x => x.Name.LastName).NotEmpty().WithMessage("Last name cannot be empty");
       //     RuleFor(x => x.Routes).NotEmpty().WithMessage("Routes cannot be empty");
        }
    }
}

public record PersonNameModel(string FirstName, string LastName)
{
    public Customer ToDomainObject()
    {
        return new Customer(Guid.NewGuid(), FirstName, LastName);
    }
}

public record ReservationRouteModel(Guid LegId, Guid CompanyId);