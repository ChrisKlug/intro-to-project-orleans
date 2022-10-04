using LocationTracking.Grains;
using LocationTracking.Simulator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System.Net;

Console.WriteLine("Simulating GPS positioning");

var host = Host.CreateDefaultBuilder()
                // Add Orleans
                .Build();
host.Start();

// Get ClusterClient

var cts = new CancellationTokenSource();
var tasks = new List<Task>();
for (int i = 1; i <= 10; i++)
{
    var name = "Simulator " + i;

    // Get Location Tracking Grain

    tasks.Add(new LocationSimulator(name, async (point) =>
    {
        // Update location

    }).Start(cts.Token));
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

cts.Cancel();
tasks.Add(host.StopAsync());
Task.WaitAll(tasks.ToArray());