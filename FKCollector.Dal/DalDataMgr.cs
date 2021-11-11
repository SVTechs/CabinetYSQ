using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FKCollector.Dal.NhUtils;
using NHibernate;
using NLog;

namespace FKCollector.Dal
{
    public class DalDataMgr
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 空操作，连接加速用
        /// </summary> <returns></returns>
        public static int InitConn()
        {
            try
            {
                var sessionFactoryDb0 = NhControl.CreateSessionFactory();
                using (var session = sessionFactoryDb0.OpenSession())
                {
                    ISQLQuery pQuery = session.CreateSQLQuery(" select 1 ");
                    pQuery.List();
                }
                return 1;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }
    }
}
