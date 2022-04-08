using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0Ticket.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Ticket.Service
{
    public class TicketService
    {
        private readonly IMongoCollection<Ticket> _ticket;

        public TicketService(IAndreAirlinesDatabaseTicketSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _ticket = database.GetCollection<Ticket>(settings.TicketCollectionName);
        }

        public List<Ticket> Get()
        {
            List<Ticket> tickets = new();

            try
            {
                tickets = _ticket.Find(ticket => true).ToList();

                return tickets;
            }
            catch (Exception exception)
            {
                tickets.Add(new Ticket());

                if (exception.InnerException != null)
                    tickets[0].ErrorCode = exception.InnerException.Message;
                else
                    tickets[0].ErrorCode = exception.Message.ToString();

                return tickets;
            }
        }            

        public Ticket Get(string id)
        {
            Ticket ticket = new();

            if (id.Length != 24)
            {
                ticket.ErrorCode = "noLength";

                return ticket;
            }

            try
            {
                ticket = _ticket.Find<Ticket>(ticket => ticket.Id == id).FirstOrDefault();

                return ticket;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    ticket.ErrorCode = exception.InnerException.Message;
                else
                    ticket.ErrorCode = exception.Message.ToString();

                return ticket;
            }
        }            

        public async Task<Ticket> Create(Ticket ticket)
        {
            if (ticket.LoginUser == null)
            {
                ticket.ErrorCode = "noBlank";

                return ticket;
            }

            var flight = await SearchFlight.ReturnFlight(ticket.Flight);

            if (flight.ErrorCode != null)
            {
                ticket.Flight = flight;

                return ticket;
            }
            else
                ticket.Flight = flight;

            var passenger = await SearchPassenger.ReturnPassenger(ticket.Passenger);

            if (passenger.ErrorCode != null)
            {
                ticket.Passenger = passenger;

                return ticket;
            }
            else
                ticket.Passenger = passenger;

            var basePrice = await SearchBasePrice.ReturnBasePrice(ticket.BasePrice);

            if (basePrice.ErrorCode != null)
            {
                ticket.BasePrice = basePrice;

                return ticket;
            }
            else
                ticket.BasePrice = basePrice;

            var classs = await SearchClass.ReturnClass(ticket.Class);

            if (classs.ErrorCode != null)
            {
                ticket.Class = classs;

                return ticket;
            }
            else
                ticket.Class = classs;

            var user = await SearchUser.ReturnUser(ticket.LoginUser);

            if (user.ErrorCode != null)
            {
                ticket.ErrorCode = user.ErrorCode;

                return ticket;
            }
            else if (user.Sector != "ADM" && user.Sector != "USER")
            {
                ticket.ErrorCode = "noPermited";

                return ticket;
            }

            var ticketWithPrice = PriceTicket.ReturnTicketWithPrice(ticket);

            _ticket.InsertOne(ticketWithPrice);

            return ticket;
        }

        public void Update(string id, Ticket ticketIn) =>
            _ticket.ReplaceOne(ticket => ticket.Id == id, ticketIn);

        public void Remove(Ticket ticketIn) =>
            _ticket.DeleteOne(ticket => ticket.Id == ticketIn.Id);

        public void Remove(string id) =>
            _ticket.DeleteOne(ticket => ticket.Id == id);
    }
}
