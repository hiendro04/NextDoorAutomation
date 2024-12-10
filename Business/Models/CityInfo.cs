using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class CityInfo : BaseModel
    {
        public string Name { get; set; }
        public string ReferenceLink { get; set; }
        public int NeighborhoodCount { get; set; }
        public ObjectId StateId { get; set; }
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
    }
}
