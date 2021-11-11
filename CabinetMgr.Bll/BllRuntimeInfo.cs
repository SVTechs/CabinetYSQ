using CabinetMgr.Dal;
using Domain.Main.Domain;

namespace CabinetMgr.Bll
{
    public class BllRuntimeInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keepCount"></param>
        /// <returns></returns>
        public static int GetCardDistCount(bool keepCount = false)
        {
            return DalRuntimeInfo.GetCardDistCount(keepCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static RuntimeInfo GetRuntimeInfo(string keyName)
        {
            if (string.IsNullOrEmpty(keyName)) return null;
            return DalRuntimeInfo.GetRuntimeInfo(keyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName">键名</param>
        /// <param name="keyValue1">null为不修改</param>
        /// <param name="keyValue2">null为不修改</param>
        /// <param name="keyValue3">null为不修改</param>
        /// <param name="keyValue4">null为不修改</param>
        /// <returns></returns>
        public static int SetRuntimeInfo(string keyName, string keyValue1, string keyValue2 = null, string keyValue3 = null,
            string keyValue4 = null)
        {
            if (string.IsNullOrEmpty(keyName)) return -100;
            return DalRuntimeInfo.SetRuntimeInfo(keyName, keyValue1, keyValue2, keyValue3, keyValue4);
        }
    }
}
