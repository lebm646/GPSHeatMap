using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GPSHeatMap.Models
{
    public class UserLocation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
