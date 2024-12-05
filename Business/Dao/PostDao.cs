using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
