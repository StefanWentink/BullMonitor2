using BullMonitor.Data.Sql.Extensions;
using BullMonitor.TickerValue.Handlers.Extensions;
using BullMonitor.TickerValue.Process.Extensions;
using SWE.Configuration.Factory;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
var builder = Host.CreateDefaultBuilder(args);

builder
    .ConfigureHostConfiguration(x => {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        x.AddJsonFiles(environmentOptional: false);
        x.AddJsonFiles(environmentName, true);
        x.AddEnvironmentVariables();
    });

builder
    .ConfigureServices((hostContext, services) =>
    {
        services
            .WithBullMonitorTickerValueProcessServices(hostContext.Configuration)
            .WithBullMonitorSqlServices(hostContext.Configuration)
            .WithBullMonitorTickerValueHandlerServices(hostContext.Configuration)
        ;
    });

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();