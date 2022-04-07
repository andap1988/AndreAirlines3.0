using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AndreAirlinesAPI3._0Models
{
    public class Flight
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public Airport Destiny { get; set; }
        public Airport Origin { get; set; }
        public Airship Airship { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime DepartureTime { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime DisembarkationTime { get; set; }
        public string LoginUser { get; set; }
        public string ErrorCode { get; set; }
    }
}
