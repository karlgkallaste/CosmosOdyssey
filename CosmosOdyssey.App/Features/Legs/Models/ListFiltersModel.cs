using FluentValidation;

namespace CosmosOdyssey.App.Features.Legs.Models;

public class ListFiltersModel
{
    public string From { get; set; }
    public string To { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }


    public class Validator : AbstractValidator<ListFiltersModel>
    {
        public Validator()
        {
            RuleFor(x => x.From).NotEmpty().WithMessage("FromId cannot be empty");
            RuleFor(x => x.To).NotEmpty().WithMessage("FromId cannot be empty");
        }
    }
}