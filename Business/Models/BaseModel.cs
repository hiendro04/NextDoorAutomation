using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Business.Utilities;

namespace Business.Models
{
    public class BaseModel
    {
        public ObjectId _id { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        [BsonIgnore]
        public string IdStr
        {
            get
            {
                return _id.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _id = new ObjectId(value);
                }
            }
        }
        [BsonIgnore]
        public string CreatedDateStr
        {
            get
            {
                return DateUtil.DateTimeToString(CreatedDate);
            }
            set
            {
                CreatedDate = DateUtil.StringToDateTime(value);
            }
        }
    }
}
