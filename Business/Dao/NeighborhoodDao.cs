using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dao
{
    public class NeighborhoodDao : BaseDao<NeighborhoodInfo>
    {
        const string COLLECTION_NAME = "NeighborhoodInfo";
        private static NeighborhoodDao instance { get; set; }
        public static NeighborhoodDao GetInstance()
        {
            if (instance == null)
            {
                instance = new NeighborhoodDao();
            }
            return instance;
        }
        public NeighborhoodDao() : base(COLLECTION_NAME) { }
    }
}
