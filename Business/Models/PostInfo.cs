using Business.Utilities;
using MongoDB.Bson;
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
        /// <summary>
        /// POST_STATUS: 0 - NOTSENT, 1 - SENT, 2 - CANCEL
        /// </summary>
        public int Status { get; set; }
        public ObjectId UserUpdateId { get; set; }
        public string UserUpdateName { get; set; }

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

        [BsonIgnore]
        public string UserUpdateIdStr
        {
            get
            {
                return UserUpdateId.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    UserUpdateId = ObjectId.Parse(value);
                }
            }
        }
    }
}
