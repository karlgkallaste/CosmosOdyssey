using FluentValidation;

namespace CosmosOdyssey.App.Features.Legs.Models;

public class ListFiltersModel
{
    public string From { get; set; }
    public string To { get; set; }
    public DateTimeOffset DepartureDate { get; set; }


    public class Validator : AbstractValidator<ListFiltersModel>
    {
        public Validator()
        {
            RuleFor(x => x.From).NotEmpty().WithMessage("From cannot be empty");
            RuleFor(x => x.To).NotEmpty().WithMessage("To cannot be empty");
            RuleFor(x => x.DepartureDate).NotEmpty().WithMessage("DepartureDate cannot be empty");
            RuleFor(x => x).Must(x => x.From != x.To).WithMessage("From and To cannot be the same");
        }
    }
}