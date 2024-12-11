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
    public class ProfileDao : BaseDao<ProfileInfo>
    {
        const string COLLECTION_NAME = "ProfileInfo";
        private static ProfileDao instance { get; set; }
        public static ProfileDao GetInstance()
        {
            if (instance == null)
            {
                instance = new ProfileDao();
            }
            return instance;
        }
        public ProfileDao() : base(COLLECTION_NAME) { }

        public List<ProfileInfo> Search(out int total, int pageSize = 10, int pageIndex = 1, string textSearch = null, string gologinIdStr = "", int type = -1)
        {
            var f = Builders<ProfileInfo>.Filter.Empty;
            if (!string.IsNullOrWhiteSpace(textSearch))
            {
                f &= Builders<ProfileInfo>.Filter.Where(p => p.Name.ToLower().Contains(textSearch.ToLower()));
            }

            if(!string.IsNullOrEmpty(gologinIdStr))
            {
                f &= Builders<ProfileInfo>.Filter.Eq(p => p.GologinId, ObjectId.Parse(gologinIdStr));
            }


            if (type > -1)
            {
                f &= Builders<ProfileInfo>.Filter.Eq(p => p.Type, type);
            }

            total = (int)GetCollection().CountDocuments(f);

            var S = Builders<ProfileInfo>.Sort.Descending(p => p.CreatedDate);

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
