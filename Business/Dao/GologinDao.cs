using Business.Models;
using MongoDB.Driver;
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

        public List<GologinInfo> Search(out int total, int pageSize = 10, int pageIndex = 1, string textSearch = null, int type = -1)
        {
            var f = Builders<GologinInfo>.Filter.Empty;
            if (!string.IsNullOrWhiteSpace(textSearch))
            {
                f &= Builders<GologinInfo>.Filter.Where(p => p.Name.ToLower().Contains(textSearch.ToLower()));
            }

            total = (int)GetCollection().CountDocuments(f);

            var S = Builders<GologinInfo>.Sort.Descending(p => p.CreatedDate);

            if (pageIndex > 0 && pageIndex > 0)
            {
                return GetCollection().Find(f).Sort(S)
                    .Skip((pageIndex - 1) * pageSize).Limit(pageSize)
                    .ToList();
            }
            return GetCollection().Find(f).Sort(S).ToList();
        }
    }
}
