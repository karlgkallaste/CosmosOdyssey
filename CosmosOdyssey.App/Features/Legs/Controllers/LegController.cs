using CosmosOdyssey.App.Features.Legs.Models;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Specifications;
using CosmosOdyssey.Domain.Specifications;
using FluentResults;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
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
    [HttpGet("list")]
    [ProducesResponseType(typeof(RouteListItemModel[]), 200)]
    [ProducesResponseType(typeof(ValidationResult), 400)]
    public async Task<IActionResult> Legs([FromServices] ILegListItemModelProvider legListProvider,
        [FromServices] IValidator<ListFiltersModel> validator,
        [FromServices] IRepository<PriceList> priceListRepository,
        [FromQuery] ListFiltersModel filters)
    {
        
        
        var validPriceLists = await priceListRepository.FindAsync(new ValidUntilNotPassed(DateTime.Now.AddDays(-1)));
        if (validPriceLists is null)
        {
            return BadRequest(new ValidationResult());
        }

        var validationResult = await validator.ValidateAsync(filters);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var lastPriceList = validPriceLists.OrderBy(x => x.ValidUntil).First();

        return Ok(legListProvider.Provide(lastPriceList, filters));
    }

    [HttpGet("filter-providers")]
    [ProducesResponseType(typeof(ProviderInfoModel[]), 200)]
    [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
    public async Task<IActionResult> LegProvidersList([FromServices] IRepository<PriceList> priceListRepository,
        [FromQuery] ProviderListFiltersModel filters)
    {
        var priceList = await priceListRepository.FindAsync(new WithAnyGivenId<PriceList>(filters.PriceListId));
        if (priceList is null)
        {
            return BadRequest();
        }

        var leg = priceList
            .SelectMany(x => x.Legs)
            .FirstOrDefault(x => x.Id == filters.LegId);

        if (leg is null)
        {
            return BadRequest();
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

    public class SearchFiltersModel
    {
    }
}
