using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Models
{
    public abstract class Person
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime DateBirth { get; set; }
        public string Email { get; set; }
        public virtual Address Address { get; set; }
        public string LoginUser { get; set; }
        public string ErrorCode { get; set; }
    }
}
