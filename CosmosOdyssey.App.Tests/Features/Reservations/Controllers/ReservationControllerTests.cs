using System.Collections.Immutable;
using CosmosOdyssey.App.Features.Reservations.Controllers;
using CosmosOdyssey.App.Features.Reservations.Requests;
using CosmosOdyssey.Domain.Features.Reservations;
using CosmosOdyssey.Domain.Features.Reservations.Commands;
using CosmosOdyssey.Domain.Features.Routes;
using CosmosOdyssey.Domain.Features.Users;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CosmosOdyssey.App.Tests.Features.Reservations.Controllers;

public class ReservationControllerTests
{
    private Mock<IValidator<CreateReservationRequest>> _validatorMock;
    private Mock<IMediator> _mediatorMock;

    private ReservationController _sut;

    [SetUp]
    public void Setup()
    {
        _validatorMock = new Mock<IValidator<CreateReservationRequest>>();
        _mediatorMock = new Mock<IMediator>();
        _sut = new ReservationController();
    }

    [Test]
    public async Task Returns_badRequest_if_validation_fails()
    {
        var request = Builder<CreateReservationRequest>.CreateNew().Build();

        _validatorMock.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult
            {
                Errors =
                [
                    new ValidationFailure("Name.FirstName", "First name cannot be empty"),
                ]
            });

        // Act
        var result = await _sut.Create(_mediatorMock.Object, _validatorMock.Object, request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        _mediatorMock.Verify(x => x.Send(It.IsAny<CreateReservationCommand>(), default), Times.Never);
    }

    [Test]
    public async Task Returns_badRequest_if_command_fails()
    {
        var request = Builder<CreateReservationRequest>.CreateNew()
            .With(x => x.Routes, new[]
            {
               Builder<ReservationRouteModel>.CreateNew().Build()
            })
            .With(x => x.Name, new PersonNameModel("Test", "Tester")).Build();

        _validatorMock.Setup(x => x.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateReservationCommand>(), default))
            .ReturnsAsync(Result.Fail(new Error("Failed")));

        // Act
        var result = await _sut.Create(_mediatorMock.Object, _validatorMock.Object, request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        _mediatorMock.Verify(x => x.Send(It.IsAny<CreateReservationCommand>(), default), Times.Once);
    }

    [Test]
    public async Task Returns_ok_if_command_succeeds()
    {
        var request = Builder<CreateReservationRequest>.CreateNew()
            .With(x => x.Routes, new[]
            {
                Builder<ReservationRouteModel>.CreateNew().Build()
            })
            .With(x => x.Name, new PersonNameModel("Test", "Tester")).Build();

        _validatorMock.Setup(x => x.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateReservationCommand>(), default))
            .ReturnsAsync(Result.Ok());

        // Act
        var result = await _sut.Create(_mediatorMock.Object, _validatorMock.Object, request);

        // Assert
        result.Should().BeOfType<OkResult>();
        _mediatorMock.Verify(x => x.Send(It.IsAny<CreateReservationCommand>(), default), Times.Once);
    }
}