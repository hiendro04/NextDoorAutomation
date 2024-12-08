using Business.Models;
using MongoDB.Driver;

namespace Business.Dao
{
    public class PostDao : BaseDao<PostInfo>
    {
        const string COLLECTION_NAME = "PostInfo";
        private static PostDao instance { get; set; }
        public static PostDao GetInstance()
        {
            if (instance == null)
            {
                instance = new PostDao();
            }
            return instance;
        }
        public PostDao() : base(COLLECTION_NAME) { }

        public List<PostInfo> Search(out int total, int pageSize = 10, int pageIndex = 1, string textSearch = null, 
                                    DateTime? fromTime = null, DateTime? toTime = null, int status = -1)
        {
            var f = Builders<PostInfo>.Filter.Empty;
            if (!string.IsNullOrWhiteSpace(textSearch))
            {
                f &= Builders<PostInfo>.Filter.Where(p => p.NeighborhoodName.Contains(textSearch));
            }
            if(fromTime != null && toTime != null)
            {
                f &= Builders<PostInfo>.Filter.Where(p => p.PostedTime != null && (p.PostedTime >= fromTime && p.PostedTime <= toTime));
            }
            if(status > -1)
            {
                f &= Builders<PostInfo>.Filter.Eq(p => p.Status, status);
            }

            total = (int)GetCollection().CountDocuments(f);

            var S = Builders<PostInfo>.Sort.Descending(p => p.TimePosted);

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
