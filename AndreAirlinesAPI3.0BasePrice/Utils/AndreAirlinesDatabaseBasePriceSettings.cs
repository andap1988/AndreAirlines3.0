namespace AndreAirlinesAPI3._0BasePrice.Utils
{
    public class AndreAirlinesDatabaseBasePriceSettings : IAndreAirlinesDatabaseBasePriceSettings
    {
        public string BasePriceCollectionName { get; set; } = "BasePrice";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "dbandreairlinesbaseprice";
    }
}
