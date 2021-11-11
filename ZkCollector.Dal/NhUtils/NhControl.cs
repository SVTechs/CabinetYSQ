using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using Domain.ServerMain.Mapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using Utilities.Encryption;
using Utilities.FileHelper;
using ZkCollector.Config;

namespace ZkCollector.Dal.NhUtils
{
    public class NhControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public enum DbTarget
        {
            Qcsh
        }

        //数据库连接池设置
        public static Boolean EnablePooling = false;
        public static int MinPoolSize = 3;
        public static int MaxPoolSize = 10;
        public static int PoolTimeOut = 4;

        private static readonly ISessionFactory[] SessionFactory = new ISessionFactory[10];

        /// <summary>
        /// 生成NHibernate SessionFactory
        /// </summary>
        /// <param name="dbTarget"></param>
        /// <returns></returns>
        public static ISessionFactory CreateSessionFactory(DbTarget dbTarget = DbTarget.Qcsh)
        {
            int reqDb = (int) dbTarget;
            if (SessionFactory[reqDb] == null)
            {
                switch (reqDb)
                {
                    case 0:
                        //QCSH
                        SessionFactory[reqDb] = Fluently.Configure()
                            .Database(MsSqlConfiguration.MsSql2005.ConnectionString(GetConnString(reqDb)).IsolationLevel(IsolationLevel.RepeatableRead))
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MapUserInfo>())
                            .ExposeConfiguration(BuildSchema)
                            .BuildSessionFactory();
                        break;
                }
            }
            return SessionFactory[reqDb];
        }

        private static string GetIpAddr(string connString)
        {
            string baseStr = connString.Replace(" ", "").ToUpper();
            int spIndex = baseStr.IndexOf("DATASOURCE");
            int spEnd = baseStr.IndexOf(";", spIndex);
            string ipAddr = baseStr.Substring(spIndex + 11, spEnd - spIndex - 11);
            return ipAddr;
        }

        private static bool IsServerAlive(string ipAddr)
        {
            if (ipAddr.Length < 4 || ipAddr.ToUpper() == "LOCALHOST")
            {
                return true;
            }
            Ping ping = new Ping();
            for (int i = 0; i < 2; i++)
            {
                PingReply pingReply = ping.Send(ipAddr, 1000);
                if (pingReply?.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            return false;
        }

        private static string GetConnString(int reqDb)
        {
            string dbConn = "";
            switch (reqDb)
            {
                case 0:
                    dbConn = AesEncryption.DecryptAuto(Env.QcshDb, Env.EncSeed);
                    break;   
            }
            if (EnablePooling)
            {
                if (!dbConn.EndsWith(";")) dbConn += ";";
                dbConn += string.Format("Pooling=True;Min Pool Size={0};Max Pool Size={1};Connection Timeout={2};", MinPoolSize,
                    MaxPoolSize, PoolTimeOut);
            }
            else
            {
                if (!dbConn.EndsWith(";")) dbConn += ";";
                dbConn += string.Format("Connection Timeout={0};", PoolTimeOut);
            }
            return dbConn;
        }

        private static void BuildSchema(Configuration config)
        {
            config.SetProperty(NHibernate.Cfg.Environment.CommandTimeout, PoolTimeOut.ToString());
            //自动建表,参数2设置为True时将删除表建构并重建
            //new SchemaExport(config).Create(false, false);
        }
    }
}
