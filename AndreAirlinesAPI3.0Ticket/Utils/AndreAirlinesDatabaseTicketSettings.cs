namespace AndreAirlinesAPI3._0Ticket.Utils
{
    public class AndreAirlinesDatabaseTicketSettings : IAndreAirlinesDatabaseTicketSettings
    {
        public string TicketCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
