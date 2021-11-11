using System.Threading;
using ZkCollector.Dal;

namespace ZkCollector.Bll
{
    public class BllDataMgr
    {
        public static void InitConn()
        {
            Thread dbInitThread = new Thread(InitConnThd) { IsBackground = true };
            dbInitThread.Start();
        }

        private static void InitConnThd()
        {
            DalDataMgr.InitConn();
        }
    }
}
