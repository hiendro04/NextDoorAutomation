using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class GologinInfo : BaseModel
    {
        public string Name { get; set; }
        public string GologinPublicId { get; set; }
        public string Description { get; set; }
        public ObjectId UserId { get; set; }
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
