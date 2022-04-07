namespace AndreAirlinesAPI3._0Airport.Utils
{
    public interface IAndreAirlinesDatabaseAirportSettings
    {
        public string AirportCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
