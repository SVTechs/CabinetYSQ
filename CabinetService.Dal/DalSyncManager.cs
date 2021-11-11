using System;
using System.Collections;
using System.Data;
using System.Reflection;
using CabinetService.Config;
using CabinetService.Dal.NhUtils;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using Utilities.FileHelper;

namespace CabinetService.Dal
{
    public class DalSyncManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 获取变化数据
        /// </summary>
        /// <param name="tableType"></param>
        /// <param name="lastChanged"></param>
        /// <param name="dbTarget"></param>
        /// <returns></returns>
        public static DataSet GetChangedData(Type tableType, DateTime lastChanged, NhControl.DbTarget dbTarget)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(dbTarget);
                using (var session = sessionFactory.OpenSession())
                {
                    string createTimeColumn = "";
                    PropertyInfo[] properties = tableType.GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        if (properties[i].Name.ToUpper().Equals("CREATETIME"))
                        {
                            createTimeColumn = properties[i].Name;
                            break;
                        }
                    }
                    ICriteria pQuery = session.CreateCriteria(tableType);
                    if (string.IsNullOrEmpty(createTimeColumn))
                    {
                        pQuery = pQuery.Add(Restrictions.Ge("Updatetime", lastChanged));
                    }
                    else
                    {
                        pQuery = pQuery.Add(Restrictions.Or(Restrictions.Ge("Updatetime", lastChanged),
                            Restrictions.Ge(createTimeColumn, lastChanged)));
                    }           
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
        /// 保存数据于服务端
        /// </summary>
        /// <param name="tableType"></param>
        /// <param name="infoSet"></param>
        /// <param name="dataOwner"></param>
        /// <param name="dbTarget"></param>
        /// <returns></returns>
        public static int SaveData(Type tableType, DataSet infoSet, string dataOwner, NhControl.DbTarget dbTarget)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(dbTarget);
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //添加及更新项目
                        for (int i = 0; i < infoSet.Tables[0].Rows.Count; i++)
                        {
                            bool hasError;
                            DataRow dr = infoSet.Tables[0].Rows[i];
                            object ci = null;
                            string serverId = infoSet.Tables[0].Rows[i]["Id"].ToString();
                            if (!string.IsNullOrEmpty(serverId))
                            {
                                ci = GetItemByServerId(session, tableType, serverId, out hasError);
                                if (hasError)
                                {
                                    return -250;
                                }
                            }
                            if (ci != null)
                            {
                                int updResult = UpdateItem(session, tableType, ci, dr);
                                if (updResult <= 0) return -210;
                            }
                            else
                            {
                                int addResult = AddItem(session, tableType, dr, dataOwner);
                                if (addResult <= 0) return -220;
                            }
                        }
                        transaction.Commit();
                        return infoSet.Tables[0].Rows.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
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
        /// <param name="dataOwner"></param>
        /// <returns></returns>
        private static int AddItem(ISession session, Type tableType, DataRow newInfo, string dataOwner)
        {
            try
            {
                object itemInstance = tableType.InvokeMember("", BindingFlags.CreateInstance, null, null, null);
                PropertyInfo[] properties = tableType.GetProperties();
                if (properties.Length > 0)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        if (properties[i].Name.ToUpper().Equals("DATAOWNER"))
                        {
                            properties[i].SetValue(itemInstance, dataOwner, null);
                        }
                        if (properties[i].Name.ToUpper().Equals("RETURNTIME") && (DateTime)newInfo[properties[i].Name] == DateTime.MinValue)
                        {
                            properties[i].SetValue(itemInstance, Env.MinTime, null);
                        }
                        else if (newInfo.Table.Columns.Contains(properties[i].Name))
                        {
                            try
                            {
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
                            catch (Exception)
                            {
                                // ignored
                            }
                        }
                    }
                }
                session.SaveOrUpdate(itemInstance);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
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
                        if (properties[i].Name.ToUpper().Equals("ID"))
                        {
                            continue;
                        }
                        if (properties[i].Name.ToUpper().Equals("RETURNTIME") && (DateTime)newInfo[properties[i].Name] == DateTime.MinValue)
                        {
                            properties[i].SetValue(originInfo, Env.MinTime, null);
                            continue;
                        }
                        if (newInfo.Table.Columns.Contains(properties[i].Name))
                        {
                            try
                            {
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
                            catch (Exception)
                            {
                                // ignored
                            }
                        }
                    }
                }
                session.SaveOrUpdate(originInfo);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }
    }
}
