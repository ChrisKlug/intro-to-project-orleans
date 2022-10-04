using LocationTracking.Grains;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans((ctx, silo) =>
{
    silo.UseLocalhostClustering()
        .UseDashboard();

    silo.Services.AddSingletonNamedService<IGrainStorage>("DashboardStateStorage", (sp, name) =>
    {
        return new DashboardStateStorage("c:\\Temp\\OrleansStorage");
    });
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
