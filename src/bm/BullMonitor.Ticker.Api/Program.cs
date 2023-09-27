using BullMonitor.Ticker.Api.Extensions;
using SWE.Configuration.Factory;
using SWE.Extensions.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder
    .Host
    .ConfigureHostConfiguration(x => {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        x.AddJsonFiles(environmentOptional: false);
        x.AddJsonFiles(environmentName, true);
        x.AddJsonFiles("local", true);
        x.AddJsonFiles("mine", true);
        x.AddEnvironmentVariables();
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.WithBullMonitorTickerApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app
    .MapGet("/", (HttpContext context, IHostEnvironment environment, IConfiguration configuration) =>
    {
        var name = context
            .User?
            .Claims
            .FirstOrDefault(x => x.Type.Equals("Username"))
            ?.Value
            ?? "anonymous";

        var apiName = "BullMonitor.Ticker.Api";

        var assembly = builder
            .GetType()
            .Assembly;


        assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute));

        return new[]
        {
                    $"Hello '{name}' from '{ apiName }'.",
                    context?.User?.Identity?.IsAuthenticated == true
                        ? "You are authenticated."
                        : "You are not authenticated.",
                    $"Your {nameof(IHostEnvironment.ApplicationName)} is {environment.ApplicationName}.",
                    $"Your {nameof(IHostEnvironment.EnvironmentName)} is {environment.EnvironmentName}.",
                    $"SHA: { assembly.GetGitHashFromInformationalVersion() }.",
                };
    });

app.Run();