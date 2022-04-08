using MongoDB.Bson.Serialization.Attributes;

namespace AndreAirlinesAPI3._0Frontend.Models
{
    public class AirportMongo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string IataCode { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string LoginUser { get; set; }
    }
}
