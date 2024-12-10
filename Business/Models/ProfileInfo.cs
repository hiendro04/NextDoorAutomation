using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Business.Models
{
    public class ProfileInfo : BaseModel
    {
        public string Name { get; set; }
        public ObjectId GologinId { get; set; }
        public string ProfilePublicId { get; set; }
        /// <summary>
        /// PROFILE_TYPE: 0 - SPAM, 1 - TPP 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// PROFILE_STATUS: 0 - ACTIVE, 1 - BLOCK
        /// </summary>
        public int Status { get; set; }
        public string Description { get; set; }
        public ObjectId UserId { get; set; }
        [BsonIgnore]
        public string TypeName { get; set; }
        [BsonIgnore]
        public string StatusName { get; set; }
        [BsonIgnore]
        public string Username { get; set; }
        [BsonIgnore]
        public string GologinName { get; set; }
        [BsonIgnore]
        public string GologinIdStr
        {
            get
            {
                return GologinId.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    GologinId = new ObjectId(value);
                }
            }
        }
        [BsonIgnore]
        public string UserIdStr
        {
            get
            {
                return UserId.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    UserId = new ObjectId(value);
                }
            }
        }
    }
}
