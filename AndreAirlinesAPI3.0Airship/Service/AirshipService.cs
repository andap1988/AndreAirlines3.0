using AndreAirlinesAPI3._0Airship.Utils;
using AndreAirlinesAPI3._0Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

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


        public Airship Create(Airship airship)
        {
            var searchAirship = GetRegistration(airship.Registration);

            if (searchAirship.ErrorCode != null)
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

            return airship;
        }

        public void Update(string id, Airship airshipIn) =>
            _airship.ReplaceOne(airship => airship.Id == id, airshipIn);

        public void Remove(Airship airshipIn) =>
            _airship.DeleteOne(airship => airship.Id == airshipIn.Id);

        public void Remove(string id) =>
            _airship.DeleteOne(airship => airship.Id == id);

    }
}
