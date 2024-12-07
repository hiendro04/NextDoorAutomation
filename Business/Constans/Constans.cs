using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constans
{
    public class Constans
    {
        
    }

    public enum PROFILE_TYPE
    {
        SPAM,
        TPP
    }

    public enum PROFILE_STATUS
    {
        ACTIVE,
        BLOCK
    }

    public enum POST_STATUS
    {
        NOTSENT,
        SENT,
        CANCEL
    }
}
