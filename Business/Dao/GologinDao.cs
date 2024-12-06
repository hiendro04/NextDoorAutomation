using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dao
{
    public class GologinDao : BaseDao<GologinInfo>
    {
        const string COLLECTION_NAME = "GologinInfo";
        private static GologinDao instance { get; set; }
        public static GologinDao GetInstance()
        {
            if (instance == null)
            {
                instance = new GologinDao();
            }
            return instance;
        }
        public GologinDao() : base(COLLECTION_NAME) { }
    }
}
