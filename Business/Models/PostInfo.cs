using Business.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class PostInfo : BaseModel
    {
        public string CustomerName { get; set; }
        public string CustomerAvatarUrl { get; set; }
        public string CustomerProfileUrl { get; set; }
        public string NeighborhoodName { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? PostedTime { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        [BsonIgnore]
        public string PostedTimeStr
        {
            get
            {
                return DateUtil.DateTimeToString(PostedTime);
            }
            set
            {
                PostedTime = DateUtil.StringToDateTime(value);
            }
        }
    }
}
