using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0Passenger.Utils;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Passenger.Service
{
    public class PassengerService
    {
        private readonly IMongoCollection<Passenger> _passenger;

        public PassengerService(IAndreAirlinesDatabasePassengerSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _passenger = database.GetCollection<Passenger>(settings.PassengerCollectionName);
        }

        public List<Passenger> Get()
        {
            List<Passenger> passengers = new();

            try
            {
                passengers = _passenger.Find(passenger => true).ToList();

                return passengers;
            }
            catch (Exception exception)
            {
                passengers.Add(new Passenger());

                if (exception.InnerException != null)
                    passengers[0].ErrorCode = exception.InnerException.Message;
                else
                    passengers[0].ErrorCode = exception.Message.ToString();

                return passengers;
            }
        }            

        public Passenger Get(string id)
        {
            Passenger passenger = new();

            if (id.Length != 24)
            {
                passenger.ErrorCode = "noLength";

                return passenger;
            }

            try
            {
                passenger = _passenger.Find<Passenger>(passenger => passenger.Id == id).FirstOrDefault();

                return passenger;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    passenger.ErrorCode = exception.InnerException.Message;
                else
                    passenger.ErrorCode = exception.Message.ToString();

                return passenger;
            }
        }   

        public Passenger GetCpf(string cpf)
        {
            Passenger passenger = new();

            try
            {
                passenger = _passenger.Find<Passenger>(passenger => passenger.Cpf == cpf).FirstOrDefault();

                return passenger;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    passenger.ErrorCode = exception.InnerException.Message;
                else
                    passenger.ErrorCode = exception.Message.ToString();

                return passenger;
            }
        }            

        public async Task<Passenger> Create(Passenger passenger)
        {
            if (passenger.LoginUser == null)
            {
                passenger.ErrorCode = "noBlank";

                return passenger;
            }

            if (passenger.PassportCode == null)
            {
                passenger.ErrorCode = "noPassport";

                return passenger;
            }

            bool isValid = VerifyCpf.IsValidCpf(passenger.Cpf);

            if (!isValid)
            {
                passenger.ErrorCode = "noCpf";

                return passenger;
            }

            var searchPassenger = GetCpf(passenger.Cpf);

            if (searchPassenger != null)
            {
                passenger.ErrorCode = "yesPassenger";

                return passenger;
            }

            var user = await SearchUser.ReturnUser(passenger.LoginUser);

            if (user.ErrorCode != null)
            {
                passenger.ErrorCode = user.ErrorCode;

                return passenger;
            }
            else if (user.Sector != "ADM" && user.Sector != "USER")
            {
                passenger.ErrorCode = "noPermited";

                return passenger;
            }
            else
                _passenger.InsertOne(passenger);

            Log log = new();
            log.User = user;
            log.BeforeEntity = "";
            log.AfterEntity = JsonConvert.SerializeObject(passenger);
            log.Operation = "create";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
            {
                _passenger.DeleteOne(passengerIn => passengerIn.Id == passenger.Id);
                passenger.ErrorCode = "noLog";

                return passenger;
            }

            return passenger;
        }

        public async Task<string> Update(string id, Passenger passengerIn, User user)
        {
            var passengerBefore = Get(passengerIn.Id);

            _passenger.ReplaceOne(passenger => passenger.Id == id, passengerIn);

            Log log = new();
            log.User = user;
            log.BeforeEntity = JsonConvert.SerializeObject(passengerBefore);
            log.AfterEntity = JsonConvert.SerializeObject(passengerIn);
            log.Operation = "update";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
                _passenger.ReplaceOne(passenger => passenger.Id == id, passengerBefore);

            return returnMsg;
        }

        public void Remove(Passenger passengerIn) =>
            _passenger.DeleteOne(passenger => passenger.Id == passengerIn.Id);

        public void Remove(string id) =>
            _passenger.DeleteOne(passenger => passenger.Id == id);
    }
}
