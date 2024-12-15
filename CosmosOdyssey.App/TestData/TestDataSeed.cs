using CosmosOdyssey.Data;
using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Commands;
using MediatR;

namespace CosmosOdyssey.App.TestData;

public class TestDataSeed
{
    private readonly IMediator _mediator;
    private readonly PriceList.IBuilder _priceListBuilder;
    private readonly PriceListLeg.IBuilder _priceListLegBuilder;
    private readonly ApplicationDbContext _dbcontext;

    public TestDataSeed(IMediator mediator, PriceList.IBuilder priceListBuilder,
        PriceListLeg.IBuilder priceListLegBuilder, ApplicationDbContext dbcontext)
    {
        _mediator = mediator;
        _priceListBuilder = priceListBuilder;
        _priceListLegBuilder = priceListLegBuilder;
        _dbcontext = dbcontext;
    }

    public async Task SeedAsync()
    {
        if (_dbcontext.PriceLists.Any())
        {
            Console.WriteLine("Skipping database seed");
            return;
        }

        for (int i = 0; i < 1; i++)
        {
            var priceListId = Guid.NewGuid();
            var legId = Guid.NewGuid();
            var priceListLeg = _priceListLegBuilder
                .WithId(legId)
                .WithRoutes(LegRoute.Create(Guid.NewGuid(), RouteLocation.Create(Guid.NewGuid(), "From"),
                    RouteLocation.Create(Guid.NewGuid(), "To"), 50))
                .WithProviders(LegProvider.Create(Guid.NewGuid(), Company.Create(Guid.NewGuid(), "Company"), 5,
                    DateTimeOffset.UtcNow, DateTimeOffset.UtcNow))
                .Build();

            var priceList = _priceListBuilder
                .WithId(priceListId)
                .WithValidUntil(DateTimeOffset.UtcNow.AddDays(1))
                .WithLegs(priceListLeg)
                .Build();

            _dbcontext.ChangeTracker.Clear();

            var result = await _mediator.Send(new CreatePriceListCommand(priceList));

            if (result.IsFailed)
            {
                throw new Exception("");
            }

            await _dbcontext.SaveChangesAsync();
        }
    }
}