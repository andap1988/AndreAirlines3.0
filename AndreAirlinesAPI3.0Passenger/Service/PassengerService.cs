﻿using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0Passenger.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

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

        public Passenger Create(Passenger passenger)
        {
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

            _passenger.InsertOne(passenger);

            return passenger;
        }

        public void Update(string id, Passenger passengerIn) =>
            _passenger.ReplaceOne(passenger => passenger.Id == id, passengerIn);

        public void Remove(Passenger passengerIn) =>
            _passenger.DeleteOne(passenger => passenger.Id == passengerIn.Id);

        public void Remove(string id) =>
            _passenger.DeleteOne(passenger => passenger.Id == id);
    }
}