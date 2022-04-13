using AndreAirlinesAPI3._0Flight.Service;
using AndreAirlinesAPI3._0Flight.Utils;
using AndreAirlinesAPI3._0Models;
using System;
using Xunit;

namespace AndreAirlinesAPI3._0Flight.Test
{
    public class UnitTesteFlight
    {
        private FlightService InitializeDataBase()
        {
            var settings = new AndreAirlinesDatabaseFlightSettings();
            FlightService flightService = new(settings);
            return flightService;
        }

        [Fact]
        public void GetAll()
        {
            var flightService = InitializeDataBase();
            var flights = flightService.Get();

            var status = flights.Count > 0;
            Assert.Equal(true, status);
        }

        [Fact]
        public void GetOne()
        {
            var flightService = InitializeDataBase();
            var flight = flightService.Get("6255de84404ecbfb170f634b");
            if (flight == null) flight = new Flight();
            Assert.Equal("6255de84404ecbfb170f634b", flight.Id);
        }

        [Fact]
        public async void Create()
        {
            Flight flight = new Flight { Destiny = new Airport { IataCode="POA" }, Origin = new Airport { IataCode = "GRU" }, Airship = new Airship { Registration = "E195" }, DepartureTime = DateTime.Now.Date, DisembarkationTime = DateTime.Now.Date, LoginUser = "m1m1" };

            var flightService = InitializeDataBase();
            var returnMsg = await flightService.Create(flight, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5ODcyMjkwLCJleHAiOjE2NDk4NzU4OTAsImlhdCI6MTY0OTg3MjI5MH0.gIQ055l7Qz7gNyoXeaqE_DvagqttkPBRScFz3GZ70O4");
            Assert.Equal("POA", returnMsg.Destiny.IataCode);
        }

        [Fact]
        public async void Update()
        {
            Flight flight = new Flight { Id = "62570de1e66f283149c3892c", ErrorCode = "passou", LoginUser = "m1m1" };

            var flightService = InitializeDataBase();
            var returnMsg = await flightService.Update("62570de1e66f283149c3892c", flight, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5ODcyMjkwLCJleHAiOjE2NDk4NzU4OTAsImlhdCI6MTY0OTg3MjI5MH0.gIQ055l7Qz7gNyoXeaqE_DvagqttkPBRScFz3GZ70O4");
            Assert.Equal("ok", returnMsg);
        }
        [Fact]
        public async void Remove()
        {
            Flight flight = new Flight { Id = "62570de1e66f283149c3892c" };

            var flightService = InitializeDataBase();
            var returnMsg = await flightService.Remove("62570de1e66f283149c3892c", flight, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5ODcyMjkwLCJleHAiOjE2NDk4NzU4OTAsImlhdCI6MTY0OTg3MjI5MH0.gIQ055l7Qz7gNyoXeaqE_DvagqttkPBRScFz3GZ70O4");
            Assert.Equal("ok", returnMsg);
        }
    }
}
