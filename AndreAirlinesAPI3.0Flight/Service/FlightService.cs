using AndreAirlinesAPI3._0Flight.Utils;
using AndreAirlinesAPI3._0Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Flight.Service
{
    public class FlightService
    {
        private readonly IMongoCollection<Flight> _flight;

        public FlightService(IAndreAirlinesDatabaseFlightSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _flight = database.GetCollection<Flight>(settings.FlightCollectionName);
        }

        public List<Flight> Get()
        {
            List<Flight> flights = new();

            try
            {
                flights = _flight.Find(flight => true).ToList();

                return flights;
            }
            catch (Exception exception)
            {
                flights.Add(new Flight());

                if (exception.InnerException != null)
                    flights[0].ErrorCode = exception.InnerException.Message;
                else
                    flights[0].ErrorCode = exception.Message.ToString();

                return flights;
            }
        }

        public Flight Get(string id)
        {
            Flight flight = new();

            if (id.Length != 24)
            {
                flight.ErrorCode = "noLength";

                return flight;
            }

            try
            {
                flight = _flight.Find<Flight>(flight => flight.Id == id).FirstOrDefault();

                return flight;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    flight.ErrorCode = exception.InnerException.Message;
                else
                    flight.ErrorCode = exception.Message.ToString();

                return flight;
            }
        }

        public async Task<Flight> Create(Flight flight, string username, string token)
        {
            bool isIataCode = false;
            bool isRegistration = false;

            var user = await SearchUser.ReturnUser(username, token);

            if (user == null || user.ErrorCode != null)
            {
                flight.ErrorCode = user.ErrorCode;

                return flight;
            }

            if (flight.Origin.IataCode != null)
                isIataCode = true;

            var airportOrigin = await SearchAirport.ReturnAirport(flight.Origin, isIataCode, token);
            isIataCode = false;

            if (airportOrigin.ErrorCode != null)
            {
                flight.Origin = airportOrigin;

                return flight;
            }
            else
                flight.Origin = airportOrigin;

            if (flight.Origin.IataCode != null)
                isIataCode = true;

            var airportDestiny = await SearchAirport.ReturnAirport(flight.Destiny, isIataCode, token);

            if (airportDestiny.ErrorCode != null)
            {
                flight.Destiny = airportDestiny;

                return flight;
            }
            else
                flight.Destiny = airportDestiny;

            if (flight.Airship.Registration != null)
                isRegistration = true;

            var airship = await SearchAirship.ReturnAirship(flight.Airship, isRegistration, token);

            if (airship.ErrorCode != null)
            {
                flight.Airship = airship;

                return flight;
            }
            else
                flight.Airship = airship;

            _flight.InsertOne(flight);

            Log log = new();
            log.User = user;
            log.BeforeEntity = "";
            log.AfterEntity = JsonConvert.SerializeObject(flight);
            log.Operation = "create";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);
            // returnMsg = "ok";

            if (returnMsg != "ok")
            {
                _flight.DeleteOne(flightIn => flightIn.Id == flight.Id);
                flight.ErrorCode = "noLog";

                return flight;
            }

            return flight;
        }

        public async Task<string> Update(string id, Flight flightIn, string username, string token)
        {
            var flightBefore = Get(flightIn.Id);
            var user = await SearchUser.ReturnUser(username, token);

            if (user == null || user.ErrorCode != null)
                return "noUser";

            _flight.ReplaceOne(flight => flight.Id == id, flightIn);

            Log log = new();
            log.User = user;
            log.BeforeEntity = JsonConvert.SerializeObject(flightBefore);
            log.AfterEntity = JsonConvert.SerializeObject(flightIn);
            log.Operation = "update";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);
            // returnMsg = "ok";

            if (returnMsg != "ok")
                _flight.ReplaceOne(airport => airport.Id == id, flightBefore);

            return returnMsg;
        }

        public async Task<string> Remove(string id, Flight flightIn, string username, string token)
        {
            var flightBefore = Get(flightIn.Id);
            var user = await SearchUser.ReturnUser(username, token);

            if (user == null || user.ErrorCode != null)
                return "noUser";

            _flight.DeleteOne(flight => flight.Id == flightIn.Id);

            Log log = new();
            log.User = user;
            log.BeforeEntity = JsonConvert.SerializeObject(flightBefore);
            log.AfterEntity = "";
            log.Operation = "delete";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);
            // returnMsg = "ok";

            if (returnMsg != "ok")
                _flight.InsertOne(flightBefore);

            return returnMsg;
        }
    }
}
