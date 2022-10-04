using LocationTracking.Grains;
using Microsoft.Extensions.Hosting;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Storage;
using System.Diagnostics;
using System.Net;

var host = Host.CreateDefaultBuilder()
            // Add Orleans
            .Build();

await host.StartAsync();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

await host.StopAsync();