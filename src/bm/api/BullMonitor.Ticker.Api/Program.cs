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

companyGroup.MapGet("/", StaticCompanyProgram.Hello);
companyGroup.MapGet("/get", StaticCompanyProgram.Get);
companyGroup.MapGet("/getknownbyall", StaticCompanyProgram.GetKnownByAll);
companyGroup.MapGet("/getknownbyzacks", StaticCompanyProgram.GetKnownByZacks);
companyGroup.MapGet("/getknownbytipranks", StaticCompanyProgram.GetKnownByTipRanks);
companyGroup.MapGet("/getbyid/{id}", StaticCompanyProgram.GetById);
companyGroup.MapGet("/getbycode/{code}", StaticCompanyProgram.GetByCode);
companyGroup.MapPut("/setknownbyzacks", StaticCompanyProgram.SetKnownByZacks);
companyGroup.MapPut("/setknownbytipRanks", StaticCompanyProgram.SetKnownByTipRanks);

var valueGroup = app.MapGroup("/value");

valueGroup.MapGet("/", StaticValueProgram.Hello);
valueGroup.MapGet("/get", StaticValueProgram.Get);
valueGroup.MapGet("/getknownbyall", StaticValueProgram.GetKnownByAll);
valueGroup.MapGet("/getknownbyzacks", StaticValueProgram.GetKnownByZacks);
valueGroup.MapGet("/getknownbytipranks", StaticValueProgram.GetKnownByTipRanks);
valueGroup.MapGet("/getbyid/{id}", StaticValueProgram.GetById);
valueGroup.MapGet("/getbycode/{code}", StaticValueProgram.GetByCode);

app.Run();