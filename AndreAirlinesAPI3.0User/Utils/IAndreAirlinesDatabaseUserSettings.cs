namespace AndreAirlinesAPI3._0User.Utils
{
    public interface IAndreAirlinesDatabaseUserSettings
    {
        public string UserCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
