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
    }
}
