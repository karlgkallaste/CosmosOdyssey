using System.Collections.Immutable;
using CosmosOdyssey.App.Features.Reservations.Models;
using CosmosOdyssey.App.Features.Reservations.Requests;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Reservations;
using CosmosOdyssey.Domain.Features.Reservations.Commands;
using CosmosOdyssey.Domain.Features.Reservations.Specifications;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CosmosOdyssey.App.Features.Reservations.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    /// <summary>
    /// Creates a new reservation based on the provided request data.
    /// Validates the request and processes the reservation creation command.
    /// </summary>
    /// <param name="mediator">The mediator used to send the reservation creation command.</param>
    /// <param name="validator">The validator to ensure the request data is valid.</param>
    /// <param name="request">The reservation creation request containing required data.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// - A 200 OK response with the created reservation's unique ID on success.
    /// - A 400 Bad Request response with validation or command errors on failure.
    /// </returns>
    [HttpPost("/reservations")]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(typeof(ValidationResult), 400)]
    public async Task<IActionResult> Create([FromServices] IMediator mediator,
        [FromServices] IValidator<CreateReservationRequest> validator, [FromBody] CreateReservationRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var commandResult = await mediator.Send(new CreateReservationCommand(request.PriceListId,
            request.Name.ToDomainObject(), request.Routes.Select(x => x.ToDomainObject()).ToImmutableList()));

        if (commandResult.IsFailed)
        {
            return BadRequest(commandResult.Errors);
        }

        return Ok(commandResult.Value);
    }

    /// <summary>
    /// Retrieves a list of reservations filtered by the customer's last name.
    /// </summary>
    /// <param name="lastName">The last name of the customer to filter reservations.</param>
    /// <param name="reservationRepository">The repository used to query reservations.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// - A 200 OK response with a list of reservations matching the filter criteria.
    /// - A 400 Bad Request response if an error occurs during the process.
    /// </returns>
    [HttpGet("/reservations")]
    [ProducesResponseType(typeof(ReservationListItemModel[]), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    public async Task<IActionResult> List([FromQuery] string lastName,
        [FromServices] IRepository<Reservation> reservationRepository)
    {
        var reservations = await reservationRepository.FindAsync(new WithCustomerName(lastName));

        return Ok(reservations.Select(x => new ReservationListItemModel(x)));
    }

    /// <summary>
    /// Retrieves detailed information about a specific reservation by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the reservation to retrieve.</param>
    /// <param name="reservationRepository">The repository used to access reservation data.</param>
    /// <param name="reservationProvider">A service that transforms the reservation into a detailed model.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// - A 200 OK response with the detailed reservation data if found.
    /// - A 404 Not Found response if the reservation does not exist.
    /// - A 400 Bad Request response if an error occurs during the process.
    /// </returns>
    [HttpGet("/reservations/{id}")]
    [ProducesResponseType(typeof(ReservationDetailsModel), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    [ProducesResponseType(typeof(NotFound), 404)]
    public async Task<IActionResult> Get(Guid id, [FromServices] IRepository<Reservation> reservationRepository,
        [FromServices] IReservationProvider reservationProvider)
    {
        var reservation = await reservationRepository.GetByIdAsync(id);

        if (reservation == null)
        {
            return NotFound();
        }

        return Ok(reservationProvider.Provide(reservation));
    }
}