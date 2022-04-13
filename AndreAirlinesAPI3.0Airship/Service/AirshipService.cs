using AndreAirlinesAPI3._0Airship.Utils;
using AndreAirlinesAPI3._0Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Airship.Service
{
    public class AirshipService
    {
        private readonly IMongoCollection<Airship> _airship;

        public AirshipService(IAndreAirlinesDatabaseAirshipSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _airship = database.GetCollection<Airship>(settings.AirshipCollectionName);
        }

        public List<Airship> Get()
        {
            List<Airship> airships = new();

            try
            {
                airships = _airship.Find(airship => true).ToList();

                return airships;
            }
            catch (Exception exception)
            {
                airships.Add(new Airship());

                if (exception.InnerException != null)
                    airships[0].ErrorCode = exception.InnerException.Message;
                else
                    airships[0].ErrorCode = exception.Message.ToString();

                return airships;
            }
        }

        public Airship Get(string id)
        {
            Airship airship = new();

            if (id.Length != 24)
            {
                airship.ErrorCode = "noLength";

                return airship;
            }

            try
            {
                airship = _airship.Find<Airship>(airship => airship.Id == id).FirstOrDefault();

                return airship;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    airship.ErrorCode = exception.InnerException.Message;
                else
                    airship.ErrorCode = exception.Message.ToString();

                return airship;
            }
        }

        public Airship GetRegistration(string registration)
        {
            Airship airship = new();

            try
            {
                airship = _airship.Find<Airship>(airship => airship.Registration == registration).FirstOrDefault();

                return airship;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    airship.ErrorCode = exception.InnerException.Message;
                else
                    airship.ErrorCode = exception.Message.ToString();

                return airship;
            }
        }

        public async Task<Airship> Create(Airship airship, string username, string token)
        {
            var user = await SearchUser.ReturnUser(username, token);

            if (user == null || user.ErrorCode != null)
            {
                airship.ErrorCode = user.ErrorCode;

                return airship;
            }

            var searchAirship = GetRegistration(airship.Registration);

            if (searchAirship != null && searchAirship.ErrorCode != null)
            {
                airship.ErrorCode = searchAirship.ErrorCode;

                return airship;
            }
            else if (searchAirship != null)
            {
                airship.ErrorCode = "yesAirport";

                return airship;
            }


            _airship.InsertOne(airship);

            Log log = new();
            log.User = user;
            log.BeforeEntity = "";
            log.AfterEntity = JsonConvert.SerializeObject(airship);
            log.Operation = "create";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
            {
                _airship.DeleteOne(airshipIn => airshipIn.Id == airship.Id);
                airship.ErrorCode = "noLog";

                return airship;
            }

            return airship;
        }

        public async Task<string> Update(string id, Airship airshipIn, string username, string token)
        { 
            var airshipBefore = GetRegistration(airshipIn.Registration);
            var user = await SearchUser.ReturnUser(username, token);

            if (user == null || user.ErrorCode != null)
                return "noUser";

            _airship.ReplaceOne(airship => airship.Id == id, airshipIn);

            Log log = new();
            log.User = user;
            log.BeforeEntity = JsonConvert.SerializeObject(airshipBefore);
            log.AfterEntity = JsonConvert.SerializeObject(airshipIn);
            log.Operation = "update";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
                _airship.ReplaceOne(airship => airship.Id == id, airshipBefore);

            return returnMsg;
        }

        public async Task<string> Remove(string id, Airship airshipIn, string username, string token)
        {
            var airshipBefore = GetRegistration(airshipIn.Registration);
            var user = await SearchUser.ReturnUser(username, token);

            if (user == null || user.ErrorCode != null)
                return "noUser";

            _airship.DeleteOne(airship => airship.Id == airshipIn.Id);

            Log log = new();
            log.User = user;
            log.BeforeEntity = JsonConvert.SerializeObject(airshipBefore);
            log.AfterEntity = "";
            log.Operation = "delete";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
                _airship.InsertOne(airshipBefore);

            return returnMsg;
        }
    }
}
