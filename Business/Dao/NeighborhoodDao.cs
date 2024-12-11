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

        public List<NeighborhoodInfo> Search(out int total, int pageSize = 10, int pageIndex = 1, string textSearch = null)
        {
            var f = Builders<NeighborhoodInfo>.Filter.Empty;
            if (!string.IsNullOrWhiteSpace(textSearch))
            {
                f &= Builders<NeighborhoodInfo>.Filter.Where(p => p.Name.ToLower().Contains(textSearch.ToLower()));
            }

            total = (int)GetCollection().CountDocuments(f);

            var S = Builders<NeighborhoodInfo>.Sort.Descending(p => p._id);

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
