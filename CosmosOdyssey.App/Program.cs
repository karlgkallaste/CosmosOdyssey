using CosmosOdyssey.App.Features.Fares.Models;
using CosmosOdyssey.App.Features.Legs.Models;
using CosmosOdyssey.Data;
using CosmosOdyssey.Domain;
using CosmosOdyssey.Services;
using CosmosOdyssey.Services.PriceListServices;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddTransient<ILegListItemModelProvider, LegListItemModelProvider>();



builder.Services
    .RegisterDomainServices()
    .RegisterDataServices()
    .RegisterServicesServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var priceListService = scope.ServiceProvider.GetRequiredService<IPriceListService>();
    //BackgroundJob.Schedule(() => priceListService.GetLatestPriceList(), TimeSpan.FromSeconds(10));
}

app.UseAuthorization();

app.MapControllers();

app.Run();