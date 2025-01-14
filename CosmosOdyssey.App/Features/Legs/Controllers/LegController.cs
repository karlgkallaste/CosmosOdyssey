using CosmosOdyssey.App.Features.Legs.Models;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Specifications;
using CosmosOdyssey.Domain.Specifications;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace CosmosOdyssey.App.Features.Legs.Controllers;

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
    /// companies and locations (200 OK), or a (400 Bad Request) if no valid price list is found.
    /// </returns>
    [HttpGet("legs/list-filters")]
    [ProducesResponseType(typeof(LegListFilterOptionsModel), 200)]
    [ProducesResponseType(typeof(ValidationResult), 400)]
    public async Task<IActionResult> ListFilters([FromServices] IRepository<PriceList> priceListRepository)
    {
        var validPriceLists = await priceListRepository.FindAsync(new ValidUntilNotPassed(DateTime.Now));
        if (validPriceLists is null)
        {
            return BadRequest(Result.Fail("Latest prices are not found"));
        }

        var lastPriceList = validPriceLists.OrderBy(x => x.ValidUntil).First();

        var locations = lastPriceList.Legs.SelectMany(x => new[]
            {
                new LocationModel(x.Route.To.Id, x.Route.To.Name),
                new LocationModel(x.Route.From.Id, x.Route.From.Name)
            }).DistinctBy(x => x.Name)
            .ToArray();

        return Ok(new LegListFilterOptionsModel()
        {
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
    [HttpGet("legs/")]
    [ProducesResponseType(typeof(RouteListItemModel[]), 200)]
    [ProducesResponseType(typeof(ValidationResult), 400)]
    public async Task<IActionResult> List([FromServices] ILegListItemModelProvider legListProvider, [FromServices] IValidator<ListFiltersModel> validator,
        [FromServices] IRepository<PriceList> priceListRepository,
        [FromQuery] ListFiltersModel filters)
    {
        var validPriceLists = await priceListRepository.FindAsync(new ValidUntilNotPassed(DateTime.Now));
        if (validPriceLists is null)
        {
            return BadRequest(Result.Fail("Latest prices are not found"));
        }

        var lastPriceList = validPriceLists.OrderBy(x => x.ValidUntil).First();

        var validationResult = await validator.ValidateAsync(filters);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(legListProvider.Provide(lastPriceList, filters));
    }

    /// <summary>
    /// Retrieves a list of providers for a specific leg (route) based on the provided filters.
    /// The method checks the existence of a valid price list and a valid leg before filtering and sorting the providers.
    /// </summary>
    /// <param name="priceListRepository">The repository used to retrieve price lists from the data source.</param>
    /// <param name="filters">The filter criteria that will be applied to retrieve and sort the providers.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing either a list of filtered and sorted providers (200 OK) 
    /// or an error response (400 Bad Request) if the price list or leg is not found or if validation fails.
    /// </returns>
    [HttpGet("legs/providers")]
    [ProducesResponseType(typeof(ProviderInfoModel[]), 200)]
    [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
    public async Task<IActionResult> Providers([FromServices] IRepository<PriceList> priceListRepository, [FromQuery] ProviderListFiltersModel filters)
    {
        var priceList = await priceListRepository.FindAsync(new WithAnyGivenId<PriceList>(filters.PriceListId));
        if (priceList is null)
        {
            return BadRequest(Result.Fail("Latest prices are not found"));
        }

        var leg = priceList
            .SelectMany(x => x.Legs)
            .FirstOrDefault(x => x.Id == filters.LegId);

        if (leg is null)
        {
            return BadRequest(Result.Fail("Leg not found"));
        }

        var specification = filters.ToSpecification();
        var filteredProviders = leg.Providers.Where(specification.IsSatisfiedBy).AsQueryable();

        var sortingFunc = filters.ToSorting();
        if (sortingFunc != null)
        {
            filteredProviders = sortingFunc(filteredProviders);
        }

        return Ok(filteredProviders.Select(x => new ProviderInfoModel()
        {
            Id = x.Id,
            Company = new CompanyInfoModel
            {
                Id = x.Company.Id,
                Name = x.Company.Name
            },
            Price = x.Price,
            FlightStart = x.FlightStart,
            FlightEnd = x.FlightEnd
        }));
    }
}