namespace AndreAirlinesAPI3._0Passenger.Utils
{
    public interface IAndreAirlinesDatabasePassengerSettings
    {
        public string PassengerCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
