using Gpx;

namespace LocationTracking.Simulator
{
    public class LocationSimulator
    {
        private readonly string name;
        private readonly Func<GpxPoint, Task> callback;

        public LocationSimulator(string name, Func<GpxPoint, Task> callback)
        {
            this.name = name;
            this.callback = callback;
        }

        public Task Start(CancellationToken cancellationToken)
            => Task.Run(async () => {

                var points = GetGpxPoints();
                var startTime = points[0].Time!;

                for (int i = 0; i < points.Length; i = i + 10)
                {
                    var point = points[i];
                    await callback(point);
                    if (i < points.Length - 1)
                    {
                        try
                        {
                            await Task.Delay(1000, cancellationToken);
                        }
                        catch (TaskCanceledException) { }
                    }
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
            });

        private GpxPoint[] GetGpxPoints()
        {
            GpxPoint[] points = new GpxPoint[0];

            using (var input = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "Data", name.Replace(" ", "-") + ".gpx")))
            using (var reader = new GpxReader(input))
            {
                while (reader.Read())
                {
                    if (reader.ObjectType == GpxObjectType.Track)
                    {
                        points = reader.Track.Segments.SelectMany(x => x.TrackPoints.ToGpxPoints().ToArray()).ToArray();
                    }
                }
            }

            return points;
        }
    }
}
