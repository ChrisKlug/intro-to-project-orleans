using LocationTracking.Grains;
using LocationTracking.Simulator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System.Net;
using System.Net.Http.Json;

Console.WriteLine("Simulating GPS positioning");

var host = Host.CreateDefaultBuilder()
                .UseOrleansClient((ctx, client) =>
                {
                    client.UseLocalhostClustering();

                     // client.UseStaticClustering(new IPEndPoint(IPAddress.Loopback, 30000));

                     // client.UseAzureStorageClustering(options => options.ConfigureTableServiceClient(Environment.GetEnvironmentVariable("POIAzure")));
                })
                .Build();
host.Start();

var clusterClient = host.Services.GetRequiredService<IClusterClient>();

var cts = new CancellationTokenSource();
var tasks = new List<Task>();
for (int i = 1; i <= 10; i++)
{
    var name = "Simulator " + i;

    var grain = clusterClient.GetGrain<ILocationTrackingGrain>(name);

    tasks.Add(new LocationSimulator(name, async (point) =>
    {
        await grain.UpdateLocation(point.ToLocation());
        // await new HttpClient().PostAsJsonAsync($"https://localhost:7149/api/client/{name}/locations", point.ToLocation());

    }).Start(cts.Token));
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

cts.Cancel();
tasks.Add(host.StopAsync());
Task.WaitAll(tasks.ToArray());