using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class WsUrlInfo : BaseModel
    {
        public string Name { get; set; }
        public string ProfileId { get; set; }
        public string WsUrl { get; set; }
        public int DelayTime { get; set; }
    }
}
