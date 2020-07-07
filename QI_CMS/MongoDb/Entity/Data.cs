using Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ES_CapDien.Models.Entity
{
    [BsonIgnoreExtraElements]
    public class Data : MongoBaseEntity
    {        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonElement("DateCreate")]
        public DateTime DateCreate { get; set; }
        [BsonElement("IsSEQ")]
        public bool IsSEQ { get; set; }

        [BsonElement("Device_Id")]
        public int Device_Id { get; set; }

        [BsonElement("BTI")]
        public double BTI { get; set; }

        [BsonElement("BT1")]
        public double BT1 { get; set; }

        [BsonElement("BHU")]
        public double BHU { get; set; }

        [BsonElement("BTO")]
        public double BTO { get; set; }

        [BsonElement("BDR")]
        public double BDR { get; set; }

        [BsonElement("BFL")]
        public double BFL { get; set; }

        [BsonElement("BFR")]
        public double BFR { get; set; }

        [BsonElement("BPS")]
        public double BPS { get; set; }

        [BsonElement("BAV")]
        public double BAV { get; set; }

        [BsonElement("BAP")]
        public double BAP { get; set; }

        [BsonElement("BAC")]
        public double BAC { get; set; }

        [BsonElement("BAF")]
        public double BAF { get; set; }

        [BsonElement("BV1")]
        public double BV1 { get; set; }

        [BsonElement("BC1")]
        public double BC1 { get; set; }

        [BsonElement("BV2")]
        public double BV2 { get; set; }

        [BsonElement("BC2")]
        public double BC2 { get; set; }

        [BsonElement("BT2")]
        public double BT2 { get; set; }

        [BsonElement("BSE")]
        public double BSE { get; set; }

        [BsonElement("BA1")]
        public double BA1 { get; set; }

        [BsonElement("BB1")]
        public double BB1 { get; set; }

        [BsonElement("BA2")]
        public double BA2 { get; set; }

        [BsonElement("BB2")]
        public double BB2 { get; set; }

        [BsonElement("BA3")]
        public double BA3 { get; set; }

        [BsonElement("BB3")]
        public double BB3 { get; set; }

        [BsonElement("BA4")]
        public double BA4 { get; set; }

        [BsonElement("BB4")]
        public double BB4 { get; set; }

        [BsonElement("BFA")]
        public double BFA { get; set; }

        [BsonElement("BFD")]
        public double BFD { get; set; }

        [BsonElement("BPW")]
        public double BPW { get; set; }

        [BsonElement("BWS")]
        public double BWS { get; set; }
    }
}