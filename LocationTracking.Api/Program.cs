using Orleans.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans((ctx, silo) =>
{
    silo.UseLocalhostClustering();
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
