using Microsoft.Maui.Devices.Sensors;
using GPSHeatMap.Services;
using MPowerKit.GoogleMaps;
using Microsoft.Maui.Graphics;
using Microsoft.VisualBasic;

namespace GPSHeatMap
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

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
                dispatcher.StartTimer(TimeSpan.FromSeconds(5), () =>
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

            // Move the map to the current user's location
            MyMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(mapPos, 15));

            // Create a Pin and set it as the map's pins (clear previous)
            var pin = new Pin
            {
                Position = mapPos,
                Title = "You are here"
            };

            if (MyMap.Pins is System.Collections.IList existingPins)
            {
                existingPins.Clear();
                existingPins.Add(pin);
            }
            else
            {
                MyMap.Pins = new System.Collections.ObjectModel.ObservableCollection<Pin> { pin };
            }
        }
    }
}
