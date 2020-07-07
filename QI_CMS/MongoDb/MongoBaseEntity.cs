using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace Core.Entities
{
    /// <summary>
    /// Lớp cơ bản của một entity trong hệ thống.
    /// </summary>
    public class MongoBaseEntity
    {
        /// <summary>
        /// Khóa chính của entity
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        /// <summary>
        /// Thời điểm tạo mới entity
        /// </summary>
        [BsonDateTimeOptions]
        [BsonElement("CreatedTime")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Thời điểm cập nhập entity lần cuối cùng
        /// </summary>
        [BsonDateTimeOptions]
        [BsonElement("UpdatedTime")]
        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Hàm trả về object dạng json
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
