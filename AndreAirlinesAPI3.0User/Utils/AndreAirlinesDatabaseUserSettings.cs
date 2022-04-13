namespace AndreAirlinesAPI3._0User.Utils
{
    public class AndreAirlinesDatabaseUserSettings : IAndreAirlinesDatabaseUserSettings
    {
        public string UserCollectionName { get; set; } = "User";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "dbandreairlinesuser";
    }
}
