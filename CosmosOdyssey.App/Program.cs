using System.Net;
using CosmosOdyssey.App.Features.Legs.Models;
using CosmosOdyssey.App.Features.Reservations.Models;
using CosmosOdyssey.Data;
using CosmosOdyssey.Domain;
using CosmosOdyssey.Services;
using CosmosOdyssey.Services.Services;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddTransient<ILegListItemModelProvider, LegListItemModelProvider>();
builder.Services.AddTransient<IReservationProvider, ReservationProvider>();
builder.Services.AddTransient<IPriceListService, PriceListService>();
builder.Services.AddValidatorsFromAssemblyContaining<ListFiltersModel.Validator>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVite", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173",
                "http://frontend:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddHangfire(config => config
    .UsePostgreSqlStorage(options =>
        options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddHangfireServer();

builder.Services
    .RegisterDomainServices()
    .RegisterDataServices()
    .RegisterServicesServices();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5000); // Ensure it listens on port 5000
});

var app = builder.Build();
app.UseCors("AllowVite");
app.UseStaticFiles();
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    var priceListService = scope.ServiceProvider.GetRequiredService<IPriceListService>();
    BackgroundJob.Enqueue(() => priceListService.GetLatestPriceList());
    RecurringJob.AddOrUpdate<IPriceListService>("DeleteExcessPriceLists", service => service.DeleteExcess(), "*/30 * * * *" // At every 30th minute
    );
}

app.UseAuthorization();

app.MapControllers();

app.Run();