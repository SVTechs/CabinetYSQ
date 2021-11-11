using System;
using System.Configuration;
using System.Data;
using System.Net.NetworkInformation;
using CabinetMgr.Config;
using Domain.Main.Mapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using Utilities.Encryption;
using Configuration = NHibernate.Cfg.Configuration;

namespace CabinetMgr.Dal.NhUtils
{
    public class NhControl
    {
        public enum DbTarget
        {
            Local
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
        public static ISessionFactory CreateSessionFactory(DbTarget dbTarget = DbTarget.Local)
        {
            int reqDb = (int) dbTarget;
            //string connString, serverAddr;
            if (SessionFactory[reqDb] == null)
            {
                switch (reqDb)
                {
                    case 0:
                        //本地库
                        SessionFactory[reqDb] = Fluently.Configure()
                            .Database(MsSqlConfiguration.MsSql2005.ConnectionString(GetConnString(reqDb)).IsolationLevel(IsolationLevel.RepeatableRead))
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MapBorrowRecord>())
                            .ExposeConfiguration(BuildSchema)
                            .BuildSessionFactory();
                        break;
                    /*
                    case 1:
                        //Qcshkf
                        connString = GetConnString(reqDb);
                        serverAddr = GetIpAddr(connString);
                        if (IsServerAlive(serverAddr))
                        {
                            SessionFactory[reqDb] = Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connString)
                                    .IsolationLevel(IsolationLevel.RepeatableRead))
                                .Mappings(m => m.FluentMappings
                                    .AddFromAssemblyOf<MapWorkUserInfo>())
                                .ExposeConfiguration(BuildSchema)
                                .BuildSessionFactory();
                        }
                        break;
                    case 2:
                        //ShangHaiDevice1
                        connString = GetConnString(reqDb);
                        serverAddr = GetIpAddr(connString);
                        if (IsServerAlive(serverAddr))
                        {
                            SessionFactory[reqDb] = Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(GetConnString(reqDb))
                                    .IsolationLevel(IsolationLevel.RepeatableRead))
                                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MapToolPurchaseInfo>())
                                .ExposeConfiguration(BuildSchema)
                                .BuildSessionFactory();
                        }
                        break;
                    case 3:
                        //ShangHaiMeasure
                        connString = GetConnString(reqDb);
                        serverAddr = GetIpAddr(connString);
                        if (IsServerAlive(serverAddr))
                        {
                            SessionFactory[reqDb] = Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(GetConnString(reqDb))
                                    .IsolationLevel(IsolationLevel.RepeatableRead))
                                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MapProcessDefinition>())
                                .ExposeConfiguration(BuildSchema)
                                .BuildSessionFactory();
                        }
                        break;*/
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
                    dbConn = ConfigurationManager.ConnectionStrings["localDb"].ConnectionString;//AesEncryption.DecryptAuto(Env.LocalDb, Env.EncSeed);
                    break;
                case 1:
                    if (AppConfig.LocalMode == 0)
                    {
                        dbConn = AesEncryption.DecryptAuto(Env.QcshkfDb, Env.EncSeed);
                    }
                    else
                    {
                        dbConn = AesEncryption.DecryptAuto(Env.QcshkfDbLocal, Env.EncSeed);
                    }
                    break;
                case 2:
                    if (AppConfig.LocalMode == 0)
                    {
                        dbConn = AesEncryption.DecryptAuto(Env.ShangHaiDevice1Db, Env.EncSeed);
                    }
                    else
                    {
                        dbConn = AesEncryption.DecryptAuto(Env.ShangHaiDevice1DbLocal, Env.EncSeed);
                    }
                    break;
                case 3:
                    if (AppConfig.LocalMode == 0)
                    {
                        dbConn = AesEncryption.DecryptAuto(Env.ShangHaiMeasureDb, Env.EncSeed);
                    }
                    else
                    {
                        dbConn = AesEncryption.DecryptAuto(Env.ShangHaiMeasureDbLocal, Env.EncSeed);
                    }
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
            //自动建表,参数2设置为True时将删除表结构并重建
            //new SchemaExport(config).Create(false, false);
        }
    }
}
