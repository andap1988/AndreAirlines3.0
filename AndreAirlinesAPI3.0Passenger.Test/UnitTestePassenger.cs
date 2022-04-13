using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0Passenger.Service;
using AndreAirlinesAPI3._0Passenger.Utils;
using System;
using Xunit;

namespace AndreAirlinesAPI3._0Passenger.Test
{
    public class UnitTestePassenger
    {
        private PassengerService InitializeDataBase()
        {
            var settings = new AndreAirlinesDatabasePassengerSettings();
            PassengerService passengerService = new(settings);
            return passengerService;
        }

        [Fact]
        public void GetAll()
        {
            var passengerService = InitializeDataBase();
            var passengers = passengerService.Get();

            var status = passengers.Count > 0;
            Assert.Equal(true, status);
        }

        [Fact]
        public void GetOne()
        {
            var passengerService = InitializeDataBase();
            var passenger = passengerService.Get("6252ff5ead342581bf79cec3");
            if (passenger == null) passenger = new Passenger();
            Assert.Equal("6252ff5ead342581bf79cec3", passenger.Id);
        }

        [Fact]
        public void GetCpf()
        {
            var passengerService = InitializeDataBase();
            var passenger = passengerService.GetCpf("87148394074");
            if (passenger == null) passenger = new Passenger();
            Assert.Equal("87148394074", passenger.Cpf);
        }

        [Fact]
        public async void Create()
        {
            Passenger passenger = new Passenger { Cpf = "42644122007", PassportCode = "aaa" };

            var passengerService = InitializeDataBase();
            var returnMsg = await passengerService.Create(passenger, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5Nzk0NDg1LCJleHAiOjE2NDk3OTgwODUsImlhdCI6MTY0OTc5NDQ4NX0.UAZqo7rYr61JnKSg8k3NMiWt1S-97hpavuQOPI4nBqs");
            Assert.Equal("aaa", returnMsg.PassportCode);
        }

        [Fact]
        public async void Update()
        {
            Passenger passenger = new Passenger { Id = "6255ef0482187471cbba3376", Cpf = "42644122007", PassportCode = "bbbbbb" };

            var passengerService = InitializeDataBase();
            var returnMsg = await passengerService.Update("6255ef0482187471cbba3376", passenger, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5Nzk0NDg1LCJleHAiOjE2NDk3OTgwODUsImlhdCI6MTY0OTc5NDQ4NX0.UAZqo7rYr61JnKSg8k3NMiWt1S-97hpavuQOPI4nBqs");
            Assert.Equal("ok", returnMsg);
        }
        [Fact]
        public async void Remove()
        {
            Passenger passenger = new Passenger { Id = "6255ef0482187471cbba3376", Cpf = "42644122007", PassportCode = "aaa" };

            var passengerService = InitializeDataBase();
            var returnMsg = await passengerService.Remove("6255ef0482187471cbba3376", passenger, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5Nzk0NDg1LCJleHAiOjE2NDk3OTgwODUsImlhdCI6MTY0OTc5NDQ4NX0.UAZqo7rYr61JnKSg8k3NMiWt1S-97hpavuQOPI4nBqs");
            Assert.Equal("ok", returnMsg);
        }
    }
}
