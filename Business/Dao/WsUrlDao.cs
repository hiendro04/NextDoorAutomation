using Business.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dao
{
    public class WsUrlDao : BaseDao<WsUrlInfo>
    {
        const string COLLECTION_NAME = "WsUrlInfo";
        private static WsUrlDao instance { get; set; }
        public static WsUrlDao GetInstance()
        {
            if (instance == null)
            {
                instance = new WsUrlDao();
            }
            return instance;
        }
        public WsUrlDao() : base(COLLECTION_NAME) { }

        public WsUrlInfo GetLastInfo(string profileId)
        {
            var f = Builders<WsUrlInfo>.Filter.Empty;
            f &= Builders<WsUrlInfo>.Filter.Where(p => p.ProfileId == profileId);
            var s = Builders<WsUrlInfo>.Sort.Descending(p => p.CreatedDate);
            return GetCollection().Find(f).Sort(s).FirstOrDefault();
        }
    }
}
