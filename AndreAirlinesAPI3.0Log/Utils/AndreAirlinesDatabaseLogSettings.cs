﻿namespace AndreAirlinesAPI3._0Log.Utils
{
    public class AndreAirlinesDatabaseLogSettings : IAndreAirlinesDatabaseLogSettings
    {
        public string LogCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
