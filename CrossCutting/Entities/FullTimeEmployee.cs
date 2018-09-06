using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    [BsonIgnoreExtraElements] // https://medium.com/@hanjchen/mongo-weekly-mongdb-insertion-and-data-modeling-in-c-2e5d70388a0a
    public class FullTimeEmployee : Employee
    {
        [BsonElement("Salary")]
        public int Salary { get; set; }
    }
}
