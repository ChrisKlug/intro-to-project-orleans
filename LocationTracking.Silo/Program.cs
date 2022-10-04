using LocationTracking.Grains;
using Microsoft.Extensions.Hosting;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Storage;
using System.Diagnostics;
using System.Net;

var host = Host.CreateDefaultBuilder()
            .UseOrleans((ctx, silo) =>
            {
                silo.UseLocalhostClustering();

                //var siloCount = Process.GetProcesses().Count(x => x.ProcessName == "LocationTracking.Silo");
                //var portSpread = siloCount == 1 ? 0 : new Random().Next(1000);

                //silo.UseDevelopmentClustering(new IPEndPoint(IPAddress.Loopback, 11111))
                //    .ConfigureEndpoints(IPAddress.Loopback, 11111 + portSpread, 30000 + portSpread);

                //silo.UseAzureStorageClustering(options => options.ConfigureTableServiceClient(Environment.GetEnvironmentVariable("POIAzure")))
                //    .ConfigureEndpoints(IPAddress.Loopback, 11111 + portSpread, 30000 + portSpread);


                silo.Services.AddSingletonNamedService<IGrainStorage>("DashboardStateStorage", (sp, name) =>
                {
                    return new DashboardStateStorage("c:\\Temp\\OrleansStorage");
                });
            })
            .Build();

await host.StartAsync();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

await host.StopAsync();