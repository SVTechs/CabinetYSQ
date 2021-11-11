using System.Collections;
using System.Collections.Generic;
using CabinetMgr.Config;
using Domain.Main.Domain;
using Utilities.DbHelper;

namespace CabinetMgr.Dal
{
    public class DalRedisUserinfo
    {
        public static IList SearchUser()
        {
            RedisHelper.Init(Env.RedisUserDb, Env.RedisUserDbPort);
            List<RedisUserInfo> userList = RedisHelper.GetList<RedisUserInfo>("measureUserList");
            return userList;
        }
    }
}
