using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Models
{
    public class Log
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public User User { get; set; }
        public string BeforeEntity { get; set; }
        public string AfterEntity { get; set; }
        public string Operation { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime InsertionDate { get; set; }
        public string ErrorCode { get; set; }
    }
}
