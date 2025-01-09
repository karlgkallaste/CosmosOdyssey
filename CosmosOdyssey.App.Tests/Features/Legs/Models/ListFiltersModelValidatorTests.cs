using CosmosOdyssey.App.Features.Legs.Models;
using FluentValidation.TestHelper;

namespace CosmosOdyssey.App.Tests.Features.Legs.Models;

[TestFixture]
public class ListFiltersModelValidatorTests
{
    private ListFiltersModel.Validator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new ListFiltersModel.Validator();
    }

    [Test]
    public void Validator_Should_Have_Error_When_From_Is_Empty()
    {
        var model = new ListFiltersModel { From = "", To = "validTo" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.From)
            .WithErrorMessage("From cannot be empty");
    }

    [Test]
    public void Validator_Should_Have_Error_When_To_Is_Empty()
    {
        var model = new ListFiltersModel { From = "validFrom", To = "" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.To).WithErrorMessage("To cannot be empty");
    }


    [Test]
    public void Validator_Should_Not_Have_Error_When_From_And_To_Are_Valid()
    {
        var model = new ListFiltersModel { From = "validFrom", To = "validTo" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.From);
        result.ShouldNotHaveValidationErrorFor(x => x.To);
    }
}