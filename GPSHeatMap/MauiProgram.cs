using Microsoft.Extensions.Logging;
using MPowerKit.GoogleMaps;

namespace GPSHeatMap
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
            .UseMauiApp<App>()
            .UseMPowerKitGoogleMaps(
            #if IOS
                "AIzaSyAAvtrQtjEBkfKAjjoXZBABg6So_xUe6LY"
            #endif
            );

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
