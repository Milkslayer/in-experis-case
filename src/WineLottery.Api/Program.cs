using System.Text.Json.Serialization;
using WineLottery.Api;
using WineLottery.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(opts =>
{
    opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.AddInfrastructure();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapEndpoints();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Check if the provider is relational
            context.Database.EnsureCreated();
        await DbManager.Initialize(services);  // Seed initial tickets and wines
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
