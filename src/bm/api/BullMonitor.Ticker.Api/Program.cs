using BullMonitor.Ticker.Api;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Updaters;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using BullMonitor.Ticker.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using SWE.Configuration.Factory;
using SWE.Extensions.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder
    .Host
    .ConfigureHostConfiguration(configurationBuilder => {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        configurationBuilder.SetFiles(environmentName);
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

var companyGroup = app.MapGroup("/company");

companyGroup.MapGet("/"                     , StaticProgram.Hello);
companyGroup.MapGet("/get"                  , StaticProgram.Get);
companyGroup.MapGet("/getknownbyall"        , StaticProgram.GetKnownByAll);
companyGroup.MapGet("/getknownbyzacks"      , StaticProgram.GetKnownByZacks);
companyGroup.MapGet("/getknownbytipranks"   , StaticProgram.GetKnownByTipRanks);
companyGroup.MapGet("/getbyid/{id}"         , StaticProgram.GetById);
companyGroup.MapGet("/getbycode/{code}"     , StaticProgram.GetByCode);
companyGroup.MapPut("/setknownbyzacks"      , StaticProgram.SetKnownByZacks);
companyGroup.MapPut("/setknownbytipRanks"   , StaticProgram.SetKnownByTipRanks);

app.Run();