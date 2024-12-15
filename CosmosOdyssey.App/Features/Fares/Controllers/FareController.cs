using CosmosOdyssey.App.Features.Fares.Models;
using CosmosOdyssey.Domain.Features.PriceLists;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CosmosOdyssey.App.Features.Fares.Controllers;

[ApiController]
[Route("[controller]")]
public class FareController : ControllerBase
{
    private readonly IPriceListRepository _priceListRepository;

    public FareController(IPriceListRepository priceListRepository)
    {
        _priceListRepository = priceListRepository;
    }

    [HttpGet("list-filters")]
    [ProducesResponseType(typeof(FareListFiltersModel), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    public void ListFilters()
    {
        throw new NotImplementedException();
    }

    [HttpGet("list")]
    public Task<IActionResult> List()
    {
        // Get latest price list
        // Check filters
        // return model
        throw new NotImplementedException();
    }
}