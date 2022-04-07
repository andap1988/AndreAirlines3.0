namespace AndreAirlinesAPI3._0Ticket.Utils
{
    public interface IAndreAirlinesDatabaseTicketSettings
    {
        public string TicketCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
