using AndreAirlinesAPI3._0Frontend.Models;
using AndreAirlinesAPI3._0Frontend.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace AndreAirlinesAPI3._0Frontend.Service
{
    public class AirportService
    {
        private readonly IMongoCollection<AirportMongo> _airport;

        public AirportService(IAndreAirlinesDatabaseAirportSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _airport = database.GetCollection<AirportMongo>(settings.AirportCollectionName);
        }

        public List<AirportMongo> Get()
        {
            List<AirportMongo> airports = new();

            airports = _airport.Find(airport => true).ToList();

            return airports;
        }
    }
}
