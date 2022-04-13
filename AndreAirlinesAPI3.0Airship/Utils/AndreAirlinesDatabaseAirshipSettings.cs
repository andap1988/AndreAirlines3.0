namespace AndreAirlinesAPI3._0Airship.Utils
{
    public class AndreAirlinesDatabaseAirshipSettings : IAndreAirlinesDatabaseAirshipSettings
    {
        public string AirshipCollectionName { get; set; } = "Airship";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "dbandreairlinesairship";
    }
}
