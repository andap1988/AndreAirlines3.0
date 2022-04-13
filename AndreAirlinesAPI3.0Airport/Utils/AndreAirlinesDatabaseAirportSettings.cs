namespace AndreAirlinesAPI3._0Airport.Utils
{
    public class AndreAirlinesDatabaseAirportSettings : IAndreAirlinesDatabaseAirportSettings
    {
        public string AirportCollectionName { get; set; } = "Airport";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "dbandreairlinesairport";
    }
}
