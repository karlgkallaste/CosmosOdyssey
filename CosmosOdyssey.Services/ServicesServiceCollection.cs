using CosmosOdyssey.Domain.Features.PriceLists.Commands;
using CosmosOdyssey.Services.PriceListServices;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosOdyssey.Services;

public static class ServicesServiceCollection
{
    public static IServiceCollection RegisterServicesServices(this IServiceCollection services)
    {
        services.AddHangfire(config => config
            .UsePostgreSqlStorage(options => 
                options.UseNpgsqlConnection("Host=localhost;Database=cosmos_odyssey;Username=cosmos;Password=odyssey")));

        services.AddHangfireServer();
        services.AddHttpClient();
        services.AddScoped<IPriceListService, PriceListService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePriceListCommand).Assembly));

        return services;
    }
}