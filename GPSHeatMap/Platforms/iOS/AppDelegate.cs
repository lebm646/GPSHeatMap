using Foundation;
using Google.Maps;

namespace GPSHeatMap
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        // Provide your Google Maps API key
        MapServices.ProvideApiKey("AIzaSyAAvtrQtjEBkfKAjjoXZBABg6So_xUe6LY");
        return base.FinishedLaunching(app, options);
        }
    }