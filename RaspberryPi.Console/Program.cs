using System;
using System.Threading;
using Iot.Device.CpuTemperature;
using Iot.Device.HardwareMonitor;

using CpuTemperature cpuTemperature = new();
using OpenHardwareMonitor openHardwareMonitor = new();
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

    Console.WriteLine(openHardwareMonitor.GetCpuLoad().Percent);

    Thread.Sleep(1000);
}