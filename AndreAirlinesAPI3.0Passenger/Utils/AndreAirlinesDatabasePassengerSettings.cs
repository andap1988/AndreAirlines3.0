namespace AndreAirlinesAPI3._0Passenger.Utils
{
    public class AndreAirlinesDatabasePassengerSettings : IAndreAirlinesDatabasePassengerSettings
    {
        public string PassengerCollectionName { get; set; } = "Passenger";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "dbandreairlinespassenger";
    }
}
