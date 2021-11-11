using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Domain.ServerMain.Mapping;
using Domain.ZWStock.Mapping;
//using Domain.ShangHaiDevice1.Mapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using Utilities.Encryption;
using WebConsole.Config;
using Configuration = NHibernate.Cfg.Configuration;

namespace WebConsole.Dal.NhUtils
{
    public class NhControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public enum DbTarget
        {
            CabinetServer,
            Qcdevice,
            ShangHaiDevice1,
            ShanHaiMeasure,
            ToolDb,
            ZwStockDb
        }

        //数据库连接池设置
        public static Boolean EnablePooling = false;
        public static int MinPoolSize = 10;
        public static int MaxPoolSize = 100;
        public static int PoolTimeOut = 60;

        private static readonly ISessionFactory[] SessionFactory = new ISessionFactory[10];

        public static ISessionFactory CreateSessionFactory(DbTarget dbTarget = DbTarget.CabinetServer)
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
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MapPageFunctions>())
                            .ExposeConfiguration(BuildSchema)
                            .BuildSessionFactory();
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 4:
                        break;
                    case 5:
                        //ZwStockDb
                        SessionFactory[reqDb] = Fluently.Configure()
                            .Database(MsSqlConfiguration.MsSql2008.ConnectionString(GetConnString(reqDb))
                                .IsolationLevel(IsolationLevel.RepeatableRead))
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MapItemMasters>())
                            .ExposeConfiguration(BuildSchema)
                            .BuildSessionFactory();
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
                    if (Env.LocalMode == 0)
                    {
                        dbConn = ConfigurationManager.ConnectionStrings["CabinetServer"].ConnectionString;
                        //dbConn = AesEncryption.DecryptAuto(Env.CabinetServerDb, Env.EncSeed);
                    }
                    else
                    {
                        dbConn = ConfigurationManager.ConnectionStrings["CabinetServerLocal"].ConnectionString;
                        //dbConn = AesEncryption.DecryptAuto(Env.CabinetServerDbLocal, Env.EncSeed);
                    }
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 4:
                    break;
                case 5:
                    if (Env.LocalMode == 0)
                    {
                        dbConn = ConfigurationManager.ConnectionStrings["ZwStock"].ConnectionString;
                    }
                    else
                    {
                        dbConn = ConfigurationManager.ConnectionStrings["ZwStockLocal"].ConnectionString;
                    }
                    //dbConn = AesEncryption.DecryptAuto(Env.ZwStockDb, Env.EncSeed);
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
            catch (Exception e)
            {
                Logger.Error(e);
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
