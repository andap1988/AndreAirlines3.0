namespace AndreAirlinesAPI3._0Flight.Utils
{
    public interface IAndreAirlinesDatabaseFlightSettings
    {
        public string FlightCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
