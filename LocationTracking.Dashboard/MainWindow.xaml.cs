using LocationTracking.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maps.MapControl.WPF;
using Orleans;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace LocationTracking.Dashboard
{
    public partial class MainWindow : Window
    {
        private IHost? host = null;
        private Dictionary<string, Pushpin> pins = new Dictionary<string, Pushpin>();

        public MainWindow()
        {
            InitializeComponent();
            map.CredentialsProvider = new ApplicationIdCredentialsProvider(Environment.GetEnvironmentVariable("POIMap"));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            host = new HostBuilder()
                        .UseOrleansClient((ctx, client) => {
                            client.UseLocalhostClustering();
                        })
                        .Build();

            host.StartAsync().ContinueWith(x => {
                OnHostStarted(host).Ignore();
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (host != null)
            {
                e.Cancel = true;
                OnHostStopping(host)
                    .ContinueWith(x => host.StopAsync())
                    .ContinueWith(x => {
                        Dispatcher.BeginInvoke(() =>
                        {
                            host = null;
                            this.Close();
                        });
                    });
                return;
            }
            base.OnClosing(e);
        }

        private async Task OnHostStarted(IHost host)
        {
            // Add observer
        }
        private async Task OnHostStopping(IHost host)
        {
            // Remove observer
        }
        private void UpdatePositions((string ClientId, Grains.Location Location)[] positions)
        {
            Dispatcher.BeginInvoke(() =>
            {
                foreach (var item in positions)
                {
                    if (!pins.ContainsKey(item.ClientId))
                    {
                        pins.Add(item.ClientId, new Pushpin());
                        map.Children.Add(pins[item.ClientId]);
                    }
                    pins[item.ClientId].Location = new Microsoft.Maps.MapControl.WPF.Location(item.Location.Latitude, item.Location.Longitude);
                }
            });
        }

        // Add Dashboard Observer
    }
}
