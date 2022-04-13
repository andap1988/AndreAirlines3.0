namespace AndreAirlinesAPI3._0Flight.Utils
{
    public class AndreAirlinesDatabaseFlightSettings : IAndreAirlinesDatabaseFlightSettings
    {
        public string FlightCollectionName { get; set; } = "Flight";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "dbandreairlinesflight";
    }
}
