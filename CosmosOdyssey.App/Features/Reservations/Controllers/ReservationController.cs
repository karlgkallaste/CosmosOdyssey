using System.Collections.Immutable;
using CosmosOdyssey.App.Features.Reservations.Requests;
using CosmosOdyssey.Domain.Features.Reservations.Commands;
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
    [ProducesResponseType(typeof(OkResult), 200)]
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

        return Ok();
    }
}