using Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_CapDien.MongoDb.Entity
{
    public class DataAlarm : MongoBaseEntity
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonElement("DateCreate")]
        public DateTime DateCreate { get; set; }
        [BsonElement("IsSEQ")]
        public bool IsSEQ { get; set; }
        [BsonElement("Device_Id")]
        public int Device_Id { get; set; }
        [BsonElement("AMATI")]
        public string AMATI { get; set; }
        [BsonElement("AMIHU")]
        public string AMIHU { get; set; }
        [BsonElement("AMADR")]
        public string AMADR { get; set; }
        [BsonElement("AMAFL")]
        public string AMAFL { get; set; }
        [BsonElement("AMAFR")]
        public string AMAFR { get; set; }
        [BsonElement("AMIPS")]
        public string AMIPS { get; set; }
        [BsonElement("AMIAL")]
        public string AMIAL { get; set; }
        [BsonElement("AMIAH")]
        public string AMIAH { get; set; }
        [BsonElement("AMIAP")]
        public string AMIAP { get; set; }
        [BsonElement("AMIAC")]
        public string AMIAC { get; set; }
        [BsonElement("AMIGN")]
        public string AMIGN { get; set; }
        [BsonElement("AMIAR")]
        public string AMIAR { get; set; }
        [BsonElement("AMIL1")]
        public string AMIL1 { get; set; }
        [BsonElement("AMIH1")]
        public string AMIH1 { get; set; }
        [BsonElement("AMIT1")]
        public string AMIT1 { get; set; }
        [BsonElement("AMIL2")]
        public string AMIL2 { get; set; }
        [BsonElement("AMIH2")]
        public string AMIH2 { get; set; }
        [BsonElement("AMIT2")]
        public string AMIT2 { get; set; }
    }
}