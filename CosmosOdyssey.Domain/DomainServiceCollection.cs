using CosmosOdyssey.Domain.Features.PriceLists.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosOdyssey.Domain;

public static class DomainServiceCollection
{
    public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePriceListCommand).Assembly));
        return services;
    }
}