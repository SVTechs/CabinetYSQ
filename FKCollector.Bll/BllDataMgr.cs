using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FKCollector.Dal;

namespace FKCollector.Bll
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
