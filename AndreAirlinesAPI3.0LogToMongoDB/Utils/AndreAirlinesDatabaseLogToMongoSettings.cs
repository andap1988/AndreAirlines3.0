namespace AndreAirlinesAPI3._0LogToMongoDB.Utils
{
    public class AndreAirlinesDatabaseLogToMongoSettings : IAndreAirlinesDatabaseLogToMongoSettings
    {
        public string LogCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
