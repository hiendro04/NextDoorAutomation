using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dao
{
    public class StateDao : BaseDao<StateInfo>
    {
        const string COLLECTION_NAME = "StateInfo";
        private static StateDao instance { get; set; }
        public static StateDao GetInstance()
        {
            if (instance == null)
            {
                instance = new StateDao();
            }
            return instance;
        }
        public StateDao() : base(COLLECTION_NAME) { }
    }
}
