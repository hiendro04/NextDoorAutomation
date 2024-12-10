using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class StateInfo : BaseModel
    {
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string ReferenceLink { get; set; }
        public int CityCount { get; set; }
    }
}
