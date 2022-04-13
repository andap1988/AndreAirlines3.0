using AndreAirlinesAPI3._0Airship.Service;
using AndreAirlinesAPI3._0Airship.Utils;
using System;
using Xunit;

namespace AndreAirlinesAPI3._0Airship.Test
{
    public class Class1
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

    }
}
