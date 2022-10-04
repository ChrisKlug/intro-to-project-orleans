using Gpx;
using LocationTracking.Grains;

namespace LocationTracking.Simulator
{
    public static class GpxPointExtensions
    {
        public static Location ToLocation(this GpxPoint point)
            => new Location { DateTime = DateTime.UtcNow, Latitude = point.Latitude, Longitude = point.Longitude };
    }
}
