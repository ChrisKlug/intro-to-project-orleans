using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationTracking.Grains
{
    public interface IDashboardGrain : IGrainWithGuidKey
    {
        Task AddPositionUpdate(string clientId, Location location);
        Task Subscribe(IDashboardObserver observer);
        Task Unsubscribe(IDashboardObserver observer);
    }

    public interface IDashboardObserver : IGrainObserver
    {
        void OnPositionsUpdated((string ClientId, Location Location)[] positions);
    }

    public class DashboardGrain : Grain, IDashboardGrain
    {
        private HashSet<IDashboardObserver> observers = new();
        private IDisposable timer;
        private IPersistentState<DashboardState> state;

        public DashboardGrain([PersistentState("DashboardState", "DashboardStateStorage")] IPersistentState<DashboardState> state)
        {
            Console.WriteLine("Creating DashboardGrain");

            timer = RegisterTimer(SendUpdates, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

            this.state = state;
        }

        public Task AddPositionUpdate(string clientId, Location location)
        {
            if (!CurrentPositions.ContainsKey(clientId))
            {
                CurrentPositions.Add(clientId, location);
            }
            else
            {
                CurrentPositions[clientId] = location;
            }

            return Task.CompletedTask;
        }

        public Task Subscribe(IDashboardObserver observer)
        {
            observers.Add(observer);
            return Task.CompletedTask;
        }

        public Task Unsubscribe(IDashboardObserver observer)
        {
            observers.Remove(observer);
            return Task.CompletedTask;
        }

        private async Task SendUpdates(object state)
        {
            Console.WriteLine("Sending updates...");

            var positions = CurrentPositions.Select(x => (ClientId: x.Key, Location: x.Value)).ToArray();
            foreach (var item in observers)
            {
                item.OnPositionsUpdated(positions);
            }
            await this.state.WriteStateAsync();
        }

        private Dictionary<string, Location> CurrentPositions => state.State.LastKnownPositions;

        public class DashboardState
        {
            public Dictionary<string, Location> LastKnownPositions { get; set; } = new Dictionary<string, Location>();
        }
    }
}
