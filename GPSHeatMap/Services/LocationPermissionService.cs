using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.ApplicationModel;

namespace GPSHeatMap.Services
{
    public static class LocationPermissionService
    {
        public static async Task<bool> RequestLocationPermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            return status == PermissionStatus.Granted;
        }
    }
}
