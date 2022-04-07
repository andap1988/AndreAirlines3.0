using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Models
{
    public class Airship
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Registration { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string LoginUser { get; set; }
        public string ErrorCode { get; set; }
    }
}
