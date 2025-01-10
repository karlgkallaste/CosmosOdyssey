using CosmosOdyssey.App.Features.Legs.Models;
using CosmosOdyssey.Data;
using CosmosOdyssey.Domain;
using CosmosOdyssey.Services;
using CosmosOdyssey.Services.PriceListServices;
using FluentValidation;
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
builder.Services.AddValidatorsFromAssemblyContaining<ListFiltersModel.Validator>(); 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVite", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .RegisterDomainServices()
    .RegisterDataServices()
    .RegisterServicesServices();

var app = builder.Build();
app.UseCors("AllowVite");
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
    
    BackgroundJob.Enqueue(() => priceListService.GetLatestPriceList());
}

app.UseAuthorization();

app.MapControllers();

app.Run();