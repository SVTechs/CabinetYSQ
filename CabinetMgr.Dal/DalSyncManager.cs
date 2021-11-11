using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using CabinetMgr.Dal.NhUtils;
using Domain.Main.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;

namespace CabinetMgr.Dal
{
    public class DalSyncManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static string[] GetIdList(DataSet infoSet)
        {
            DataTable dt = infoSet.Tables[0];
            string[] idList = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                idList[i] = (string)dt.Rows[i]["Id"];
            }
            return idList;
        }

        /// <summary>
        /// 更新数据为已上传状态
        /// </summary>
        /// <param name="tableType"></param>
        /// <param name="infoSet"></param>
        /// <returns></returns>
        public static int SetAsUploaded(Type tableType, DataSet infoSet)
        {
            try
            {
                //生成ID列表
                string[] idList = GetIdList(infoSet);
                if (idList == null) return 0;
                //获取ServerId/SyncStatus属性
                PropertyInfo syncType = tableType.GetProperty("SyncStatus");
                if (syncType == null) return -201;
                //更新同步标识
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(tableType)
                        .Add(Restrictions.In("Id", idList));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            for (int i = 0; i < recordList.Count; i++)
                            {
                                syncType.SetValue(recordList[i], 1, null);
                                session.SaveOrUpdate(recordList[i]);
                            }
                            transaction.Commit();
                        }
                    }
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(tableType.FullName + " Failed:" + ex.Message);
            }
            return -200;
        }

        /// <summary>
        /// 获取待上传数据
        /// </summary>
        /// <param name="tableType"></param>
        /// <returns></returns>
        public static DataSet GetLocalData(Type tableType)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(tableType)
                        .Add(Restrictions.Eq("SyncStatus", 0))
                        .AddOrder(Order.Asc("Id"))
                        .SetFirstResult(0)
                        .SetMaxResults(50);
                    IList resultList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(resultList))
                    {
                        return SqlDataHelper.ConvertToDataSet(tableType, resultList);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        /// <summary>
        /// 保存下载数据
        /// </summary>
        /// <param name="tableType"></param>
        /// <param name="infoSet"></param>
        /// <returns></returns>
        public static int UpdateData(Type tableType, DataSet infoSet)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //添加及更新项目
                        for (int i = 0; i < infoSet.Tables[0].Rows.Count; i++)
                        {
                            bool hasError;
                            DataRow dr = infoSet.Tables[0].Rows[i];
                            object ci = GetItemByServerId(session, tableType, infoSet.Tables[0].Rows[i]["Id"].ToString(), out hasError);
                            if (hasError)
                            {
                                return -250;
                            }
                            if (ci != null)
                            {
                                int updResult = UpdateItem(session, tableType, ci, dr);
                                if (updResult <= 0) return -201;
                            }
                            else
                            {
                                int addResult = AddItem(session, tableType, dr);
                                if (addResult <= 0) return -201;
                            }
                        }
                        transaction.Commit();
                        return 1;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }

        /// <summary>
        /// 以服务器ID获取项目信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="tableType"></param>
        /// <param name="serverId"></param>
        /// <param name="hasError"></param>
        /// <returns></returns>
        public static object GetItemByServerId(ISession session, Type tableType, string serverId, out bool hasError)
        {
            hasError = false;
            try
            {
                ICriteria pQuery = session.CreateCriteria(tableType)
                    .Add(Restrictions.Eq("Id", serverId));
                IList roleList = pQuery.List();
                if (SqlDataHelper.IsDataValid(roleList))
                {
                    return roleList[0];
                }
            }
            catch (Exception ex)
            {
                hasError = true;
                Logger.Error(ex);
            }
            return null;
        }


        /// <summary>
        /// 添加项目信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="tableType"></param>
        /// <param name="newInfo"></param>
        /// <returns></returns>
        private static int AddItem(ISession session, Type tableType, DataRow newInfo)
        {
            try
            {
                object itemInstance = tableType.InvokeMember("", BindingFlags.CreateInstance, null, null, null);
                PropertyInfo[] properties = tableType.GetProperties();
                if (properties.Length > 0)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        if (newInfo.Table.Columns.Contains(properties[i].Name))
                        {
                            try
                            {
                                if (properties[i].Name.Equals("Createtime"))
                                {
                                    properties[i].SetValue(itemInstance, DateTime.Now, null);
                                    continue;
                                }
                                if (properties[i].Name.Equals("Updatetime") && (DateTime)newInfo[properties[i].Name] == DateTime.MinValue)
                                {
                                    properties[i].SetValue(itemInstance, new DateTime(1900, 1 ,1), null);
                                    continue;
                                }
                                if (newInfo[properties[i].Name] == System.DBNull.Value)
                                {
                                    continue;
                                }
                                if (newInfo[properties[i].Name] is long && properties[i].PropertyType == typeof(int))
                                {
                                    properties[i].SetValue(itemInstance, Convert.ToInt32(newInfo[properties[i].Name]), null);
                                }
                                else if (newInfo[properties[i].Name] is long && properties[i].PropertyType == typeof(short))
                                {
                                    properties[i].SetValue(itemInstance, Convert.ToInt16(newInfo[properties[i].Name]), null);
                                }
                                else if (newInfo[properties[i].Name] is double && properties[i].PropertyType == typeof(float))
                                {
                                    properties[i].SetValue(itemInstance, Convert.ToSingle(newInfo[properties[i].Name]), null);
                                }
                                else
                                {
                                    properties[i].SetValue(itemInstance, newInfo[properties[i].Name], null);
                                }
                            }
                            catch (Exception ex)
                            {
                                // ignored
                            }
                        }
                    }
                }
                session.Save(itemInstance);
                //session.Flush();
                return 1;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }

        /// <summary>
        /// 更新项目信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="tableType"></param>
        /// <param name="originInfo"></param>
        /// <param name="newInfo"></param>
        /// <returns></returns>
        private static int UpdateItem(ISession session, Type tableType, object originInfo, DataRow newInfo)
        {
            try
            {
                PropertyInfo[] properties = tableType.GetProperties();
                if (properties.Length > 0)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        if (properties[i].Name.Equals("Id"))
                        {
                            continue;
                        }
                        if (properties[i].Name.Equals("ServerId"))
                        {
                            properties[i].SetValue(originInfo, Convert.ToInt32(newInfo["Id"]), null);
                        }
                        if (properties[i].Name.Equals("Updatetime") || properties[i].Name.Equals("LastChanged"))
                        {
                            properties[i].SetValue(originInfo, DateTime.Now, null);
                        }
                        else if (newInfo.Table.Columns.Contains(properties[i].Name))
                        {
                            try
                            {
                                if (newInfo[properties[i].Name] == System.DBNull.Value)
                                {
                                    continue;
                                }
                                if (newInfo[properties[i].Name] is long && properties[i].PropertyType == typeof(int))
                                {
                                    properties[i].SetValue(originInfo, Convert.ToInt32(newInfo[properties[i].Name]), null);
                                }
                                else if (newInfo[properties[i].Name] is long && properties[i].PropertyType == typeof(short))
                                {
                                    properties[i].SetValue(originInfo, Convert.ToInt16(newInfo[properties[i].Name]), null);
                                }
                                else if (newInfo[properties[i].Name] is double && properties[i].PropertyType == typeof(float))
                                {
                                    properties[i].SetValue(originInfo, Convert.ToSingle(newInfo[properties[i].Name]), null);
                                }
                                else
                                {
                                    properties[i].SetValue(originInfo, newInfo[properties[i].Name], null);
                                }
                            }
                            catch (Exception ex)
                            {
                                // ignored
                            }
                        }
                    }
                }
                session.Update(originInfo);
                //session.Flush();
                return 1;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }
    }
    public class SQLWatcher : EmptyInterceptor
    {
        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            Console.WriteLine(sql.ToString());
            return base.OnPrepareStatement(sql);
        }
        //public virtual NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        //{
        //    return sql;
        //}
    }
}
