using CosmosOdyssey.App.Features.Legs.Models;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Specifications;
using FluentResults;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CosmosOdyssey.App.Features.Legs.Controllers;

//TODO: RENAME LEGS -> ROUTES
[ApiController]
[Route("[controller]")]
public class LegController : ControllerBase
{
    /// <summary>
    /// Retrieves a list of filter options for legs (routes), including available companies and locations,
    /// based on the most recent valid price list.
    /// </summary>
    /// <param name="priceListRepository">
    /// The repository used to query and retrieve valid price lists, which contain the route and provider details.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a <see cref="LegListFilterOptionsModel"/> with the available 
    /// companies and locations (200 OK), or a 400 Bad Request if no valid price list is found.
    /// </returns>
    [HttpGet("list-filters")]
    [ProducesResponseType(typeof(LegListFilterOptionsModel), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    public async Task<IActionResult> ListFilters([FromServices] IRepository<PriceList> priceListRepository)
    {
        var validPriceLists = await priceListRepository.FindAsync(new ValidUntilNotPassed(DateTime.Now.AddDays(-1)));
        if (validPriceLists is null)
        {
            return BadRequest();
        }

        var lastPriceList = validPriceLists.OrderBy(x => x.ValidUntil).First();
        var companies = lastPriceList.Legs
            .SelectMany(x => x.Providers.Select(x => new CompanyModel(x.Company.Id, x.Company.Name)))
            .DistinctBy(x => x.Name).ToArray();

        var locations = lastPriceList.Legs.SelectMany(x => new[]
            {
                new LocationModel(x.Route.To.Id, x.Route.To.Name),
                new LocationModel(x.Route.From.Id, x.Route.From.Name)
            }).DistinctBy(x => x.Name)
            .ToArray();

        return Ok(new LegListFilterOptionsModel()
        {
            Companies = companies,
            Locations = locations
        });
    }

    /// <summary>
    /// Retrieves a list of legs (routes) based on the specified filter criteria.
    /// Validates the provided filters and ensures data is retrieved from the most recent valid price list.
    /// </summary>
    /// <param name="legListProvider">
    /// A service that provides leg (route) data based on the specified filters and the most recent valid price list.
    /// </param>
    /// <param name="validator">
    /// A validation service that checks the validity of the provided filter criteria.
    /// </param>
    /// <param name="priceListRepository">
    /// The repository used to query and retrieve valid price lists.
    /// </param>
    /// <param name="filters">
    /// The filter criteria used to retrieve specific legs (routes), such as source, destination, or provider details.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a list of routes that match the specified filter criteria (200 OK)
    /// or an error response (400 Bad Request) if validation fails or no valid price list is found.
    /// </returns>
    [HttpGet("list")]
    [ProducesResponseType(typeof(RouteListItemModel[]), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    public async Task<IActionResult> Legs([FromServices] ILegListItemModelProvider legListProvider,
        [FromServices] IValidator<ListFiltersModel> validator,
        [FromServices] IRepository<PriceList> priceListRepository,
        [FromQuery] ListFiltersModel filters)
    {
        var validPriceLists = await priceListRepository.FindAsync(new ValidUntilNotPassed(DateTime.Now.AddDays(-1)));
        if (validPriceLists is null)
        {
            return BadRequest(Result.Fail("No fares available"));
        }

        var lastPriceList = validPriceLists.OrderBy(x => x.ValidUntil).First();

        var validationResult = await validator.ValidateAsync(filters);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(legListProvider.Provide(lastPriceList, filters));
    }
}