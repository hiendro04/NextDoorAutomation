using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class NeighborhoodInfo : BaseModel
    {
        public string Name { get; set; }
        public string ReferenceLink { get; set; }
        public int NeighborCount { get; set; }
        public int PostOfWeek { get; set; }
        public ObjectId StateId { get; set; }
        public ObjectId CityId { get; set; }
        [BsonIgnore]
        public bool IsSelected { get; set; }
        [BsonIgnore]
        public string StateName { get; set; }
        [BsonIgnore]
        public string CityName { get; set; }
        [BsonIgnore]
        public string StateIdStr
        {
            get
            {
                return StateId.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    StateId = new ObjectId(value);
                }
            }
        }
        [BsonIgnore]
        public string CityIdStr
        {
            get
            {
                return CityId.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    CityId = new ObjectId(value);
                }
            }
        }
    }
}
