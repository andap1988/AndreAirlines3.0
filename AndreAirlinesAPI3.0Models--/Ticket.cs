using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public Flight Flight { get; set; }
        public Passenger Passenger { get; set; }
        public BasePrice BasePrice { get; set; }
        public Class Class { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime RegistrationDate { get; set; }
        public decimal FullPrice { get; set; }
        public decimal PromotionPercent { get; set; }
        public string LoginUser { get; set; }
        public string ErrorCode { get; set; }
    }
}
