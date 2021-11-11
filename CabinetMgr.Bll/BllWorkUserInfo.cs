using System.Collections;
using CabinetMgr.Dal;

namespace CabinetMgr.Bll
{
    public class BllWorkUserInfo
    {
        public static IList SearchWorkUserInfo(string workUserId)
        {
            if (string.IsNullOrEmpty(workUserId)) return null;
            return DalWorkUserInfo.SearchWorkUserInfo(workUserId);
        }

        public static int ResetAllStatus()
        {
            return DalWorkUserInfo.ResetAllStatus();
        }

        public static int SetAsAquired(string id, string userName)
        {
            if (string.IsNullOrEmpty(id)) return -100;
            return DalWorkUserInfo.SetAsAquired(id, userName);
        }

        public static int SetAsAquired(string[] idArray, string userName)
        {
            if (idArray == null || idArray.Length == 0) return -100;
            return DalWorkUserInfo.SetAsAquired(idArray, userName);
        }

        public static int SetAsTaken(int[] positionArray)
        {
            if (positionArray == null || positionArray.Length == 0) return -100;
            return DalWorkUserInfo.SetAsTaken(positionArray);
        }

        public static int SetAsReturned(string id)
        {
            if (string.IsNullOrEmpty(id)) return -100;
            return DalWorkUserInfo.SetAsReturned(id);
        }

        public static int SetAsReturned(string[] idArray)
        {
            if (idArray == null || idArray.Length == 0) return -100;
            return DalWorkUserInfo.SetAsReturned(idArray);
        }
    }
}
