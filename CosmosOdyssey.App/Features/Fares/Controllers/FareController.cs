using CosmosOdyssey.App.Features.Fares.Models;
using CosmosOdyssey.Domain.Features.Companies;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.Routes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CosmosOdyssey.App.Features.Fares.Controllers;

[ApiController]
[Route("[controller]")]
public class FareController : ControllerBase
{
    private readonly IRepository<PriceList> _priceListRepository;
    private readonly ILegInfoProvider _legInfoProvider;

    public FareController(
        IRepository<PriceList> priceListRepository, ILegInfoProvider legInfoProvider)
    {
        _priceListRepository = priceListRepository;
        _legInfoProvider = legInfoProvider;
    }

    [HttpGet("list-filters")]
    [ProducesResponseType(typeof(FareListFiltersModel), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    public async Task<IActionResult> ListFilters()
    {
        throw new NotImplementedException();
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromBody] LegFiltersModel model)
    {
        throw new NotImplementedException();
    }
}