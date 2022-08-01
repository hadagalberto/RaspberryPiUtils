using System;
using System.Threading;
using Iot.Device.CpuTemperature;
using Iot.Device.HardwareMonitor;
using RaspberryPi.Console;

using CpuTemperature cpuTemperature = new();
var client = new CpuUsage();
var memory = new MemoryMetricsClient();
Console.WriteLine("Press any key to quit");

while (!Console.KeyAvailable)
{
    if (cpuTemperature.IsAvailable)
    {
        var temperature = cpuTemperature.ReadTemperatures();
        foreach (var entry in temperature)
        {
            if (!double.IsNaN(entry.Temperature.DegreesCelsius))
            {
                Console.WriteLine($"Temperature from {entry.Sensor}: {entry.Temperature.DegreesCelsius} °C");
            }
            else
            {
                Console.WriteLine("Unable to read Temperature.");
            }
        }
    }
    else
    {
        Console.WriteLine($"CPU temperature is not available");
    }

    var metrics = client.ReadCpuUsage();

    Console.WriteLine("All Usage: " + metrics.CpuUsage);

    var memoryMetrics = memory.GetMetrics();

    Console.WriteLine("Memory: " + memoryMetrics);

    Thread.Sleep(1000);
}