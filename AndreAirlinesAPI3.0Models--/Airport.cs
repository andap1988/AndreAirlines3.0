using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Models
{
    public class Airport
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string IataCode { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string LoginUser { get; set; }
        public string ErrorCode { get; set; }
    }
}
