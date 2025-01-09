using CosmosOdyssey.App.Features.Reservations.Requests;
using FluentValidation.TestHelper;

namespace CosmosOdyssey.App.Tests.Features.Reservations.Requests;

[TestFixture]
public class CreateReservationRequestValidatorTests
{
    private CreateReservationRequest.Validator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new CreateReservationRequest.Validator();
    }

    [Test]
    public void Validator_should_have_error_when_name_is_null()
    {
        var request = new CreateReservationRequest
        {
            PriceListId = Guid.NewGuid(),
            Name = null,
            Routes = new ReservationRouteModel[1]
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name is required");
    }
    
    
    [Test]
    public void Validator_should_have_error_when_firstName_is_empty()
    {
        var request = new CreateReservationRequest
        {
            PriceListId = Guid.NewGuid(),
            Name = new PersonNameModel(string.Empty, "asd"),
            Routes = new ReservationRouteModel[1]
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name.FirstName)
            .WithErrorMessage("First name cannot be empty");
    }
    
     [Test]
    public void Validator_should_have_error_when_lastName_is_empty()
    {
        var request = new CreateReservationRequest
        {
            PriceListId = Guid.NewGuid(),
            Name = new PersonNameModel("John", string.Empty),
            Routes = new ReservationRouteModel[1]
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name.LastName)
            .WithErrorMessage("Last name cannot be empty");
    }

    [Test]
    public void Validator_should_have_error_when_routes_are_empty()
    {
        var request = new CreateReservationRequest
        {
            PriceListId = Guid.NewGuid(),
            Name = new PersonNameModel("John", "Doe"),
            Routes = Array.Empty<ReservationRouteModel>()
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Routes)
            .WithErrorMessage("Routes cannot be empty");
    }

    [Test]
    public void Validator_should_not_have_error_for_valid_request()
    {
        var request = new CreateReservationRequest
        {
            PriceListId = Guid.NewGuid(),
            Name = new PersonNameModel("John", "Doe"),
            Routes = new ReservationRouteModel[1]
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_should_have_error_when_priceListId_is_default()
    {
        var request = new CreateReservationRequest
        {
            PriceListId = Guid.Empty,
            Name = new PersonNameModel("John", "Doe"),
            Routes = new ReservationRouteModel[1]
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PriceListId)
            .WithErrorMessage("Price list id cannot be empty");
    }

    [Test]
    public void Validator_should_have_error_when_firstName_and_lastName_are_empty()
    {
        var request = new CreateReservationRequest
        {
            PriceListId = Guid.NewGuid(),
            Name = new PersonNameModel(string.Empty, string.Empty),
            Routes = new ReservationRouteModel[1]
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name.FirstName)
            .WithErrorMessage("First name cannot be empty");
        result.ShouldHaveValidationErrorFor(x => x.Name.LastName)
            .WithErrorMessage("Last name cannot be empty");
    }
}