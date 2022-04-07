namespace AndreAirlinesAPI3._0BasePrice.Utils
{
    public class AndreAirlinesDatabaseBasePriceSettings : IAndreAirlinesDatabaseBasePriceSettings
    {
        public string BasePriceCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
