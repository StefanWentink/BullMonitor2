using BullMonitor.Data.Mongo.Extensions;
using BullMonitor.Data.Sql.Extensions;
using BullMonitor.Analysis.Handlers.Extensions;
using BullMonitor.Analysis.Process.Extensions;
using BullMonitor.Ticker.Api.SDK.Extensions;
using SWE.Configuration.Factory;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Updaters;
using BullMonitor.Ticker.Api.SDK.Interfaces;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;

var builder = Host.CreateDefaultBuilder(args);

builder
    .ConfigureAppConfiguration(configurationBuilder =>
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        configurationBuilder.SetFiles(environmentName);
    });

builder
    .ConfigureServices((hostContext, services) =>
    {
        services
            .WithBullMonitorAnalysisHandlerServices(hostContext.Configuration)
            .WithBullMonitorAnalysisProcessServices(hostContext.Configuration)
            .WithBullMonitorSqlServices(hostContext.Configuration)
            .WithBullMonitorMongoServices(hostContext.Configuration)

            .WithBullMonitorTickerApiSdkServices(hostContext.Configuration)
            .AddSingleton<ICompanyProvider>(x => x.GetRequiredService<IHttpCompanyProvider>())
            .AddSingleton<ICompanyUpdater>(x => x.GetRequiredService<IHttpCompanyUpdater>())
        ;
    });

var app = builder.Build();

app.Run();