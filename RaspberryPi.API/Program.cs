using Iot.Device.CpuTemperature;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RaspberryPi.API;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/getraspberryinfo", (HttpContext httpContext) =>
{
    using CpuTemperature cpuTemperature = new();
    string temp = "";
    if (cpuTemperature.IsAvailable)
    {
        var temperature = cpuTemperature.ReadTemperatures();
        foreach (var entry in temperature)
        {
            if (!double.IsNaN(entry.Temperature.DegreesCelsius))
            {
                temp = $"{entry.Temperature.DegreesCelsius} °C";
            }
            else
            {
                temp = "Unable to read Temperature.";
            }
        }
    }
    else
    {
        temp = $"CPU temperature is not available";
    }

    CpuUsage cpuUsage = new();
    var usage = cpuUsage.ReadCpuUsage();

    var retorno = new { Temperature = temp, Usage = usage };
})
.WithName("GetRaspberryInfo");

app.Run();