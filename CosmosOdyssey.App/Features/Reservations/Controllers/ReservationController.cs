using System.Collections.Immutable;
using CosmosOdyssey.App.Features.Reservations.Models;
using CosmosOdyssey.App.Features.Reservations.Requests;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Reservations;
using CosmosOdyssey.Domain.Features.Reservations.Commands;
using CosmosOdyssey.Domain.Features.Reservations.Specifications;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CosmosOdyssey.App.Features.Reservations.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    [HttpPost("create")]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
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


    [HttpPost("list")]
    [ProducesResponseType(typeof(ReservationListItemModel[]), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    public async Task<IActionResult> List([FromQuery] string lastName,
        [FromServices] IRepository<Reservation> reservationRepository)
    {
        var reservations = await reservationRepository.FindAsync(new WithCustomerName(lastName));

        return Ok(reservations.Select(x => new ReservationListItemModel(x)));
    }

    [HttpPost("{id}")]
    [ProducesResponseType(typeof(ReservationDetailsModel), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
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