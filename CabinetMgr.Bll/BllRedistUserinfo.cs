using System.Collections;
using CabinetMgr.Dal;

namespace CabinetMgr.Bll
{
    public class BllRedistUserinfo
    {
        public static IList SearchUser()
        {
            return DalRedisUserinfo.SearchUser();
        }
    }
}
