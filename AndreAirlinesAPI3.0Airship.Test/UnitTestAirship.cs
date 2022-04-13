using AndreAirlinesAPI3._0Airship.Service;
using AndreAirlinesAPI3._0Airship.Utils;
using AndreAirlinesAPI3._0Models;
using System;
using Xunit;

namespace AndreAirlinesAPI3._0Airship.Test
{
    public class UnitTestAirship
    {
        private AirshipService InitializeDataBase()
        {
            var settings = new AndreAirlinesDatabaseAirshipSettings();
            AirshipService airshipService = new(settings);
            return airshipService;
        }

        [Fact]
        public void GetAll()
        {
            var airshipService = InitializeDataBase();
            var airships = airshipService.Get();

            var status = airships.Count > 0;
            Assert.Equal(true, status);
        }

        [Fact]
        public void GetOne()
        {
            var airshipService = InitializeDataBase();
            var airship = airshipService.Get("6252c58dc850f86881815db2");
            if (airship == null) airship = new Airship();
            Assert.Equal("6252c58dc850f86881815db2", airship.Id);
        }

        [Fact]
        public void GetRegistration()
        {
            var airshipService = InitializeDataBase();
            var airship = airshipService.GetRegistration("E195");
            if (airship == null) airship = new Airship();
            Assert.Equal("Embraer E-Jets E195", airship.Name);
        }
    }
}
