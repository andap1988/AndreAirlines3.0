using AndreAirlinesAPI3._0Airport.Service;
using AndreAirlinesAPI3._0Airport.Utils;
using AndreAirlinesAPI3._0Models;
using System;
using Xunit;

namespace AndreAirlinesAPI3._0Airport.Test
{
    public class UnitTestAirport
    {
        private AirportService InitializeDataBase()
        {
            var settings = new AndreAirlinesDatabaseAirportSettings();
            AirportService airportService = new(settings);
            return airportService;
        }

        [Fact]
        public void GetAll()
        {
            var airportService = InitializeDataBase();
            var airports = airportService.Get();

            var status = airports.Count > 0;
            Assert.Equal(true, status);
        }

        [Fact]
        public void GetOne()
        {
            var airportService = InitializeDataBase();
            var airport = airportService.Get("6252ca91fd5ca45886042378");
            if (airport == null) airport = new Airport();
            Assert.Equal("6252ca91fd5ca45886042378", airport.Id);
        }

        [Fact]
        public void GetIataCode()
        {
            var airportService = InitializeDataBase();
            var airport = airportService.GetIataCode("POA");
            if (airport == null) airport = new Airport();
            Assert.Equal("Aeroporto Internacional de Porto Alegre", airport.Name);
        }

        [Fact]
        public async void Create()
        {
            Airport airport = new Airport { IataCode = "XPTO" };

            var airportService = InitializeDataBase();
            var returnMsg = await airportService.Create(airport, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5Nzk0NDg1LCJleHAiOjE2NDk3OTgwODUsImlhdCI6MTY0OTc5NDQ4NX0.UAZqo7rYr61JnKSg8k3NMiWt1S-97hpavuQOPI4nBqs");
            Assert.Equal("XPTO", returnMsg.IataCode);
        }

        [Fact]
        public async void Update()
        {
            Airport airport = new Airport { Id = "6255f4377677b01004f2ff55", IataCode = "ZZZZZ" };

            var airportService = InitializeDataBase();
            var returnMsg = await airportService.Update("6255f4377677b01004f2ff55", airport, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5Nzk0NDg1LCJleHAiOjE2NDk3OTgwODUsImlhdCI6MTY0OTc5NDQ4NX0.UAZqo7rYr61JnKSg8k3NMiWt1S-97hpavuQOPI4nBqs");
            Assert.Equal("ok", returnMsg);
        }
        [Fact]
        public async void Remove()
        {
            Airport airport = new Airport { Id = "6255f4377677b01004f2ff55", IataCode = "ZZZZZ" };

            var airportService = InitializeDataBase();
            var returnMsg = await airportService.Remove("6255f4377677b01004f2ff55", airport, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5Nzk0NDg1LCJleHAiOjE2NDk3OTgwODUsImlhdCI6MTY0OTc5NDQ4NX0.UAZqo7rYr61JnKSg8k3NMiWt1S-97hpavuQOPI4nBqs");
            Assert.Equal("ok", returnMsg);
        }
    }
}
