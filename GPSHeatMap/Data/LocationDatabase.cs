using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using GPSHeatMap.Models;


namespace GPSHeatMap.Data
{
    public class LocationDatabase
    {
        private readonly SQLiteAsyncConnection _db;

        public LocationDatabase(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<UserLocation>().Wait();
        }

        public Task<int> AddLocationAsync(UserLocation location)
        {
            return _db.InsertAsync(location);
        }
        
        public Task<List<UserLocation>> GetAllLocationsAsync()
        {
            return _db.Table<UserLocation>().ToListAsync();
        }
    }
}
