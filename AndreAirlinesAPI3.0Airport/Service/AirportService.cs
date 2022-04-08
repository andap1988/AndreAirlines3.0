using AndreAirlinesAPI3._0Airport.Utils;
using AndreAirlinesAPI3._0Models;
using MongoDB.Driver;
using Newtonsoft.Json;
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
            if (airport.LoginUser == null)
            {
                airport.ErrorCode = "noBlank";

                return airport;
            }            

            var user = await SearchUser.ReturnUser(airport.LoginUser);

            if (user.ErrorCode != null)
            {
                airport.ErrorCode = user.ErrorCode;

                return airport;
            }
            else if (user.Sector != "ADM")
            {
                airport.ErrorCode = "noPermited";

                return airport;
            }
            else
                _airport.InsertOne(airport);

            Log log = new();
            log.User = user;
            log.BeforeEntity = "";
            log.AfterEntity = JsonConvert.SerializeObject(airport);
            log.Operation = "create";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
            {
                _airport.DeleteOne(airportIn => airportIn.Id == airport.Id);
                airport.ErrorCode = "noLog";

                return airport;
            }

            return airport;
        }

        public async Task<string> Update(string id, Airport airportIn, User user)
        {
            var airportBefore = GetIataCode(airportIn.IataCode);
            
            _airport.ReplaceOne(airport => airport.Id == id, airportIn);

            Log log = new();
            log.User = user;
            log.BeforeEntity = JsonConvert.SerializeObject(airportBefore);
            log.AfterEntity = JsonConvert.SerializeObject(airportIn);
            log.Operation = "update";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
                _airport.ReplaceOne(airport => airport.Id == id, airportBefore);

            return returnMsg;
        }

        public void Remove(Airport airportIn) =>
            _airport.DeleteOne(airport => airport.Id == airportIn.Id);

        public void Remove(string id) =>
            _airport.DeleteOne(airport => airport.Id == id);

    }
}
