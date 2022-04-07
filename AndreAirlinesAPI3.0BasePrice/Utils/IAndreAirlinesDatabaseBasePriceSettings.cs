namespace AndreAirlinesAPI3._0BasePrice.Utils
{
    public interface IAndreAirlinesDatabaseBasePriceSettings
    {
        public string BasePriceCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
