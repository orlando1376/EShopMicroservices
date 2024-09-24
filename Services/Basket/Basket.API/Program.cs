using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);

    // Indica que el campo UserName será la llave primaria
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
// Utilizamos el nuget Scrutor para poder registrar el decorador de BasketRepository
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// Registramo el servicio de Redis como Cache distribuido
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket";
});

// Manejo de excepciones personalizadas
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Añadir servicios de HealthChecks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!) // Requiere el nueget AspNetCore.HealthChecks.NpgSql
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!); // Requiere el nueget AspNetCore.HealthChecks.Redis

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

// Manejo de excepciones personalizadas
app.UseExceptionHandler(options => { });

// Ruta de healthCheck
app.UseHealthChecks("/health",
    // Convertir la salida en un formato JSON mas legible
    new HealthCheckOptions // Requiere el nuget AspNetCore.HealthChecks.UI.Client
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
