using AndreAirlinesAPI3._0Airport.Utils;
using AndreAirlinesAPI3._0Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Airport.Service
{
    public class AirportService
    {
        private readonly IMongoCollection<Airport> _airport;

        public AirportService(IAndreAirlinesDatabaseAirportSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _airport = database.GetCollection<Airport>(settings.AirportCollectionName);
        }

        public List<Airport> Get()
        {
            List<Airport> airports = new();

            try
            {
                airports = _airport.Find(airport => true).ToList();

                return airports;
            }
            catch (Exception exception)
            {
                airports.Add(new Airport());

                if (exception.InnerException != null)
                    airports[0].ErrorCode = exception.InnerException.Message;
                else
                    airports[0].ErrorCode = exception.Message.ToString();

                return airports;
            }
        }

        public Airport Get(string id)
        {
            Airport airport = new();

            if (id.Length != 24)
            {
                airport.ErrorCode = "noLength";

                return airport;
            }

            try
            {
                airport = _airport.Find<Airport>(airport => airport.Id == id).FirstOrDefault();

                return airport;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    airport.ErrorCode = exception.InnerException.Message;
                else
                    airport.ErrorCode = exception.Message.ToString();

                return airport;
            }
        }

        public Airport GetIataCode(string iataCode)
        {
            Airport airport = new();

            try
            {
                airport = _airport.Find<Airport>(airport => airport.IataCode == iataCode).FirstOrDefault();

                return airport;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    airport.ErrorCode = exception.InnerException.Message;
                else
                    airport.ErrorCode = exception.Message.ToString();

                return airport;
            }
        }

        public async Task<Airport> Create(Airport airport)
        {
            var searchAirport = GetIataCode(airport.IataCode);

            if (searchAirport != null && searchAirport.ErrorCode != null)
            {
                airport.ErrorCode = searchAirport.ErrorCode;

                return airport;
            }
            else if (searchAirport != null)
            {
                airport.ErrorCode = "yesAirport";

                return airport;
            }

            var user = await SearchUser.ReturnUser(airport.LoginUser);

            if (user.ErrorCode != null)
            {
                airport.LoginUser = user.ErrorCode;

                return airport;
            }
            else if (user.Sector != "ADM")
            {
                airport.ErrorCode = "noPermited";

                return airport;
            }
            else
                _airport.InsertOne(airport);

            return airport;
        }

        public void Update(string id, Airport airportIn) =>
            _airport.ReplaceOne(airport => airport.Id == id, airportIn);

        public void Remove(Airport airportIn) =>
            _airport.DeleteOne(airport => airport.Id == airportIn.Id);

        public void Remove(string id) =>
            _airport.DeleteOne(airport => airport.Id == id);

    }
}
