using Business.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dao
{
    public class CityDao : BaseDao<CityInfo>
    {
        const string COLLECTION_NAME = "CityInfo";
        private static CityDao instance { get; set; }
        public static CityDao GetInstance()
        {
            if (instance == null)
            {
                instance = new CityDao();
            }
            return instance;
        }

        public CityDao() : base(COLLECTION_NAME) { }


        public List<CityInfo> GetByStateId(ObjectId stateId)
        {
            var f = Builders<CityInfo>.Filter.Empty;
            f &= Builders<CityInfo>.Filter.Eq(c => c.StateId, stateId);

            return GetCollection().Find(f).ToList(); 
        }
    }
}
