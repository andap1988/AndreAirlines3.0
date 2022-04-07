using AndreAirlinesAPI3._0Models;

namespace AndreAirlinesAPI3._0Ticket.Service
{
    public class PriceTicket
    {
        public static Ticket ReturnTicketWithPrice(Ticket ticket)
        {
            decimal price = 0, percent = 0, valueClass = 0, basePrice = 0;

            // price = precobase + valorclasse - porcentagem

            percent = ticket.PromotionPercent;
            basePrice = ticket.BasePrice.Price;
            valueClass = ticket.Class.Price;

            price = basePrice + valueClass - percent;

            ticket.FullPrice = price;

            return ticket;
        }
    }
}
