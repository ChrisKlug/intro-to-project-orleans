using Orleans;

namespace LocationTracking.Grains
{
    public interface ILocationTrackingGrain : IGrainWithStringKey
    {
        Task UpdateLocation(Location location);
    }

    [GenerateSerializer]
    public class Location
    {
        [Id(0)] public DateTime DateTime { get; set; }
        [Id(1)] public double Latitude { get; set; }
        [Id(2)] public double Longitude { get; set; }
    }

    public class LocationTrackingGrain : Grain, ILocationTrackingGrain
    {
        private readonly IGrainFactory grainFactory;

        public LocationTrackingGrain(IGrainFactory grainFactory)
        {
            Console.WriteLine($"Creating LocationTrackingGrain {this.GetPrimaryKeyString()}");
            this.grainFactory = grainFactory;
        }

        public async Task UpdateLocation(Location location)
        {
            Console.WriteLine($"{this.GetPrimaryKeyString()} - {location.DateTime.ToString("HH:mm:ss")} - {location.Latitude} : {location.Longitude}");

            await grainFactory.GetGrain<IDashboardGrain>(Guid.Empty).AddPositionUpdate(this.GetPrimaryKeyString(), location);
        }
    }
}
