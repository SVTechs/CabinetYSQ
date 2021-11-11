using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using CabinetService.Config;
//using Domain.Qcdevice.Mapping;
using Domain.ServerMain.Mapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using Utilities.Encryption;
using Utilities.FileHelper;
using Configuration = NHibernate.Cfg.Configuration;

namespace CabinetService.Dal.NhUtils
{
    public class NhControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public enum DbTarget
        {
            Local,
            Qcshkf,
            ShangHaiDevice1,
            ShanHaiMeasure,
            Qcdevice
        }

        //数据库连接池设置
        public static Boolean EnablePooling = false;
        public static int MinPoolSize = 10;
        public static int MaxPoolSize = 100;
        public static int PoolTimeOut = 60;

        private static readonly ISessionFactory[] SessionFactory = new ISessionFactory[10];

        /// <summary>
        /// 生成NHibernate SessionFactory
        /// </summary>
        /// <param name="dbTarget"></param>
        /// <returns></returns>
        public static ISessionFactory CreateSessionFactory(DbTarget dbTarget = DbTarget.Local)
        {
            int reqDb = (int)dbTarget;
            if (SessionFactory[reqDb] == null)
            {
                switch (reqDb)
                {
                    case 0:
                        //CabinetServer
                        SessionFactory[reqDb] = Fluently.Configure()
                            .Database(MsSqlConfiguration.MsSql2005.ConnectionString(GetConnString(reqDb)).IsolationLevel(IsolationLevel.RepeatableRead))
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MapPageMenus>())
                            .ExposeConfiguration(BuildSchema)
                            .BuildSessionFactory();
                        //SessionFactory[reqDb] = Fluently.Configure()
                        //    .Database(MsSqlConfiguration.MsSql2005.ConnectionString(GetConnString(reqDb)).IsolationLevel(IsolationLevel.RepeatableRead))
                        //    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MapPageMenus>())
                        //    .ExposeConfiguration(BuildSchema)
                        //    .BuildSessionFactory();
                        break;
                    case 1:
                        //Qcshkf
                        //SessionFactory[reqDb] = Fluently.Configure()
                        //    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(GetConnString(reqDb)).IsolationLevel(IsolationLevel.RepeatableRead))
                        //    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Domain.Qcshkf.Mapping.MapUserInfo>())
                        //    .ExposeConfiguration(BuildSchema)
                        //    .BuildSessionFactory();
                        break;
                    case 4:
                        //Qcdevice
                        //SessionFactory[reqDb] = Fluently.Configure()
                        //    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(GetConnString(reqDb)).IsolationLevel(IsolationLevel.RepeatableRead))
                        //    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MapUserInfo>())
                        //    .ExposeConfiguration(BuildSchema)
                        //    .BuildSessionFactory();
                        break;
                }
            }
            return SessionFactory[reqDb];
        }

        private static string GetConnString(int reqDb)
        {
            string dbConn = "";
            switch (reqDb)
            {
                case 0:
                    dbConn = AesEncryption.DecryptAuto(Env.CabinetServerDb, Env.EncSeed);
                    break;
                case 1:
                    dbConn = AesEncryption.DecryptAuto(Env.QcshkfDb, Env.EncSeed);
                    break;
                case 4:
                    dbConn = AesEncryption.DecryptAuto(Env.QcdeviceDb, Env.EncSeed);
                    break;
            }
            if (EnablePooling)
            {
                if (!dbConn.EndsWith(";")) dbConn += ";";
                dbConn += string.Format("Pooling=True;Min Pool Size={0};Max Pool Size={1};Connection Timeout={2}", MinPoolSize,
                    MaxPoolSize, PoolTimeOut);
            }
            return dbConn;
        }

        private static void BuildSchema(Configuration config)
        {
            //自动建表,参数2设置为True时将删除表建构并重建
            //new SchemaExport(config).Create(false, false);
        }

        /// <summary>
        /// JQGrid条件解析
        /// </summary>
        /// <param name="extFilter"></param>
        /// <returns></returns>
        public static ICriterion GenerateFilter(List<HibernateHelper.WhereEntity> extFilter)
        {
            try
            {
                if (extFilter == null || extFilter.Count == 0) return null;
                ICriterion basicCriterion = null;
                for (int i = 0; i < extFilter.Count; i++)
                {
                    ICriterion curCriterion;
                    switch (extFilter[i].ColumnOp.ToUpper())
                    {
                        case "EQ":
                            curCriterion = Restrictions.Eq(extFilter[i].ColumnName, extFilter[i].ColumnValue);
                            break;
                        case "NE":
                            curCriterion =
                                Restrictions.Not(Restrictions.Eq(extFilter[i].ColumnName, extFilter[i].ColumnValue));
                            break;
                        case "LT":
                            curCriterion = Restrictions.Lt(extFilter[i].ColumnName, extFilter[i].ColumnValue);
                            break;
                        case "LE":
                            curCriterion = Restrictions.Le(extFilter[i].ColumnName, extFilter[i].ColumnValue);
                            break;
                        case "GT":
                            curCriterion = Restrictions.Gt(extFilter[i].ColumnName, extFilter[i].ColumnValue);
                            break;
                        case "GE":
                            curCriterion = Restrictions.Ge(extFilter[i].ColumnName, extFilter[i].ColumnValue);
                            break;
                        case "NU":
                            curCriterion = Restrictions.IsNull(extFilter[i].ColumnName);
                            break;
                        case "NN":
                            curCriterion = Restrictions.IsNotNull(extFilter[i].ColumnName);
                            break;
                        case "IN":
                            string[] iValues = extFilter[i].ColumnValue.Split(',');
                            curCriterion = Restrictions.In(extFilter[i].ColumnName, iValues);
                            break;
                        case "NI":
                            string[] niValues = extFilter[i].ColumnValue.Split(',');
                            curCriterion = Restrictions.Not(Restrictions.In(extFilter[i].ColumnName, niValues));
                            break;
                        case "BW":
                            curCriterion = Restrictions.Like(extFilter[i].ColumnName, extFilter[i].ColumnValue,
                                MatchMode.End);
                            break;
                        case "BN":
                            curCriterion =
                                Restrictions.Not(Restrictions.Like(extFilter[i].ColumnName, extFilter[i].ColumnValue,
                                    MatchMode.End));
                            break;
                        case "EW":
                            curCriterion = Restrictions.Like(extFilter[i].ColumnName, extFilter[i].ColumnValue,
                                MatchMode.Start);
                            break;
                        case "EN":
                            curCriterion =
                                Restrictions.Not(Restrictions.Like(extFilter[i].ColumnName, extFilter[i].ColumnValue,
                                    MatchMode.Start));
                            break;
                        case "CN":
                            curCriterion = Restrictions.Like(extFilter[i].ColumnName, extFilter[i].ColumnValue,
                                MatchMode.Anywhere);
                            break;
                        case "NC":
                            curCriterion =
                                Restrictions.Not(Restrictions.Like(extFilter[i].ColumnName, extFilter[i].ColumnValue,
                                    MatchMode.Anywhere));
                            break;
                        default:
                            curCriterion = null;
                            break;
                    }
                    if (basicCriterion == null)
                    {
                        basicCriterion = curCriterion;
                    }
                    else
                    {
                        switch (extFilter[i].GroupOp.ToUpper())
                        {
                            case "AND":
                                basicCriterion = Restrictions.And(basicCriterion, curCriterion);
                                break;
                            case "OR":
                                basicCriterion = Restrictions.Or(basicCriterion, curCriterion);
                                break;
                        }
                    }
                }
                return basicCriterion;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static List<Order> GenerateOrder(List<HibernateHelper.OrderEntity> extOrder)
        {
            if (extOrder == null || extOrder.Count == 0) return null;
            List<Order> fullOrder = new List<Order>();
            for (int i = 0; i < extOrder.Count; i++)
            {
                Order curOrder;
                switch (extOrder[i].Order.ToUpper())
                {
                    case "ASC":
                        curOrder = Order.Asc(extOrder[i].ColumnName);
                        break;
                    case "DESC":
                        curOrder = Order.Desc(extOrder[i].ColumnName);
                        break;
                    default:
                        curOrder = Order.Asc(extOrder[i].ColumnName);
                        break;
                }
                fullOrder.Add(curOrder);
            }
            return fullOrder;
        }
    }
}
