using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dao
{
    public class ProfileDao : BaseDao<PostInfo>
    {
        const string COLLECTION_NAME = "PostInfo";
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
    }
}
