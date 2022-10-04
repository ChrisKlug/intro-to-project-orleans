using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime;
using Orleans.Storage;

namespace LocationTracking.Grains
{
    public class DashboardStateStorage : IGrainStorage
    {
        private readonly string storagePath;

        public DashboardStateStorage(string storagePath)
        {
            this.storagePath = storagePath;
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
        }

        public Task ClearStateAsync<T>(string grainType, GrainReference grainReference, IGrainState<T> grainState)
        {
            throw new NotImplementedException();
        }

        public async Task ReadStateAsync<T>(string grainType, GrainReference grainReference, IGrainState<T> grainState)
        {
            if (typeof(T) == typeof(LocationDashboardGrain.DashboardState))
            {
                var fileName = Path.Combine(storagePath, "dashboardState.json");
                if (File.Exists(fileName))
                {
                    grainState.State = JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(fileName))!;
                }
            }
        }

        public async Task WriteStateAsync<T>(string grainType, GrainReference grainReference, IGrainState<T> grainState)
        {
            if (typeof(T) == typeof(LocationDashboardGrain.DashboardState))
            {
                var fileName = Path.Combine(storagePath, "dashboardState.json");
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                await File.WriteAllTextAsync(fileName, JsonConvert.SerializeObject(grainState.State));
            }
        }
    }
}
