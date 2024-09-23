using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);

    // Validaciones de comportamiento
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));

    // Registro de información
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddCarter();

// Siembre de datos iniciales. Solo si se está en Desarrollo
if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

// FuentValidation
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);    
}).UseLightweightSessions();

// Manejo de excepciones personalizadas
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Añadir servicios de HealthChecks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!); // Requiere el nueget AspNetCore.HealthChecks.NpgSql

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
