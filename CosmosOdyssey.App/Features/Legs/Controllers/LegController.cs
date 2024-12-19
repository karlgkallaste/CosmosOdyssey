using CosmosOdyssey.App.Features.Fares.Models;
using CosmosOdyssey.App.Features.Legs.Models;
using CosmosOdyssey.Domain.Features.Legs;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Specifications;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CosmosOdyssey.App.Features.Legs.Controllers;

public class LegController : ControllerBase
{
    [HttpGet("list-filters")]
    [ProducesResponseType(typeof(LegListFilterOptionsModel), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    public async Task<IActionResult> ListFilters([FromServices] IRepository<PriceList> priceListRepository)
    {
        var validPriceLists = await priceListRepository.FindAsync(new ValidUntilNotPassed(DateTime.Now.AddDays(-1)));
        if (!validPriceLists.Any())
        {
            return BadRequest();
        }

        var lastPriceList = validPriceLists.OrderBy(x => x.ValidUntil).First();
        var companies = lastPriceList.Legs
            .SelectMany(x => x.Providers.Select(x => new CompanyModel(x.Company.Id, x.Company.Name))).ToArray();

        var locations = lastPriceList.Legs.SelectMany(x => new[]
            {
                new LocationModel(x.Route.To.Id, x.Route.To.Name),
                new LocationModel(x.Route.From.Id, x.Route.From.Name)
            })
            .ToArray();

        return Ok(new LegListFilterOptionsModel()
        {
            Companies = companies,
            Locations = locations
        });
    }
    [HttpGet("list")]
    [ProducesResponseType(typeof(LegListFilterOptionsModel), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    public async Task<IActionResult> Legs([FromServices] ILegListItemModelProvider legListProvider, [FromServices] IRepository<PriceList> priceListRepository, [FromBody] ListFiltersModel? filters)
    {
        var validPriceLists = await priceListRepository.FindAsync(new ValidUntilNotPassed(DateTime.Now.AddDays(-1)));
        if (!validPriceLists.Any())
        {
            return BadRequest();
        }

        var lastPriceList = validPriceLists.OrderBy(x => x.ValidUntil).First();

        if (filters == null)
        {
            return Ok(legListProvider.Provide(lastPriceList));
        }
        else
        {
            
        }
        
        return Ok(legListProvider.Provide(lastPriceList));
        
    }
}