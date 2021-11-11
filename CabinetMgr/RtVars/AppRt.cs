using System.Collections.Generic;
using Domain.Main.Domain;
using Hardware.DeviceInterface;

namespace CabinetMgr.RtVars
{
    public class AppRt
    {
        //public static List<RedisUserInfo> UserList = null;
        public static UserInfo CurUser = null;

        public static bool IsInit = true;

        public static bool MissionMode = false;

        public static string CheckingTool = "";

        public static object RequireLock = new object();
        public static List<int> RequiredList = new List<int>();
    }
}
