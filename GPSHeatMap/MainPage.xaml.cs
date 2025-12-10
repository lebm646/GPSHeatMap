using Microsoft.Maui.Devices.Sensors;
using GPSHeatMap.Services;
using GPSHeatMap.Data;
using GPSHeatMap.Models;
using MPowerKit.GoogleMaps;
using Microsoft.Maui.Graphics;
using Microsoft.VisualBasic;
using FileSystem = Microsoft.Maui.Storage.FileSystem;



namespace GPSHeatMap
{
    public partial class MainPage : ContentPage
    {
        private LocationDatabase _locationDb;

        public MainPage()
        {
            InitializeComponent();
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "location.db3");
            _locationDb = new LocationDatabase(dbPath);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var location = await GetCurrentLocationAsync();
            if (location != null)
            {
                var point = new Point(location.Latitude, location.Longitude);
                // Move the map to the current user's location
                MyMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(point, 15));
            }

            RequestAndTrackLocation();
            
        }

        private async Task<Location?> GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);
                return location;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Unable to get location: {ex.Message}", "OK");
                return null;
            }
        }

        private async void RequestAndTrackLocation()
        {
            bool granted = await LocationPermissionService.RequestLocationPermissionAsync();
            if (!granted)
            {
                await DisplayAlertAsync("Permission Denied", "Location permission is required to show your position on the map.", "OK");
                return;
            }

            var dispatcher = Application.Current?.Dispatcher;
            if (dispatcher != null)
            {
                dispatcher.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    TrackLocationAsync();
                    return true; // true = repeat timer
                });
            }
            else
            {
                await DisplayAlertAsync("Error", "Unable to access application dispatcher.", "OK");
            }
        }

        private async void TrackLocationAsync()
        {
            var location = await GetCurrentLocationAsync();
            if (location == null)
                return;

            var mapPos = new Point(location.Latitude, location.Longitude);

            await _locationDb.AddLocationAsync(new UserLocation
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Timestamp = DateTime.UtcNow
            });

            // Update heatmap
            await UpdateHeatMapAsync();
        }

        private async Task UpdateHeatMapAsync()
        {
            var locations = await _locationDb.GetAllLocationsAsync();

            if (locations == null || locations.Count == 0)
                return;

            var heatData = locations.Select(l => new WeightedLatLng(new Point(l.Latitude, l.Longitude), 1f)).ToList();

            var heatOverlay = new HeatMapTileOverlay
            {
                Data = heatData,
                Radius = 25,
                MaxIntensity = 1f
            };

            MyMap.TileOverlays = new List<HeatMapTileOverlay> { heatOverlay };
        }
    }
}
