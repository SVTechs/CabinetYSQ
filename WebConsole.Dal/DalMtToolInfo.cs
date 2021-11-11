using System;
using System.Collections;
using System.Collections.Generic;
using Domain.MtToolDb.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using Utilities.FileHelper;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalMtToolInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<MtToolInfo> SearchMtToolInfo(string toolCode, string toolName, string toolSpec, int toolCate,
            int dataStart, int dataCount)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ToolDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(MtToolInfo));
                    if (!string.IsNullOrEmpty(toolCode))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("CodeNo", toolCode, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(toolName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("Name", toolName, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(toolSpec))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("Model", toolSpec, MatchMode.Anywhere));
                    }
                    if (toolCate >= 0)
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("BCategoryId", toolCate));
                    }
                    pQuery = pQuery.AddOrder(Order.Asc("Id"));
                    pQuery.SetFirstResult(dataStart);
                    pQuery.SetMaxResults(dataCount);
                    return pQuery.List<MtToolInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int GetMtToolInfoCount(string toolCode, string toolName, string toolSpec, int toolCate)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ToolDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(MtToolInfo));
                    if (!string.IsNullOrEmpty(toolCode))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("CodeNo", toolCode, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(toolName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("Name", toolName, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(toolSpec))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("Model", toolSpec, MatchMode.Anywhere));
                    }
                    if (toolCate >= 0)
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("BCategoryId", toolCate));
                    }
                    return (int)pQuery.SetProjection(Projections.RowCount()).UniqueResult();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static MtToolInfo GetMtToolInfo(int id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ToolDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(MtToolInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (MtToolInfo)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int AddMtToolInfo(
            string codeNo,
            int bCodeTypeId,
            string name,
            string model,
            int bCategoryId,
            string brandName,
            int bUnitId,
            string place,
            string remark,
            string pym,
            int month,
            int publishManId,
            DateTime publishTime,
            int state,
            int isCheck,
            int isDetect, string cabinetName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ToolDb);
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        MtToolInfo itemRecord = new MtToolInfo
                        {
                            CodeNo = codeNo,
                            BCodeTypeId = bCodeTypeId,
                            Name = name,
                            Model = model,
                            BCategoryId = bCategoryId,
                            BrandName = brandName,
                            BUnitId = bUnitId,
                            Place = place,
                            Remark = remark,
                            Pym = pym,
                            Month = month,
                            PublishManId = publishManId,
                            PublishTime = publishTime,
                            State = state,
                            IsCheck = isCheck,
                            IsDetect = isDetect,
                            CabinetName = cabinetName
                        };
                        session.SaveOrUpdate(itemRecord);
                        transaction.Commit();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int ModifyMtToolInfo(
            int id,
            string codeNo,
            int bCodeTypeId,
            string name,
            string model,
            int bCategoryId,
            string brandName,
            int bUnitId,
            string place,
            string remark,
            string pym,
            int month,
            int publishManId,
            DateTime publishTime,
            int state,
            int isCheck,
            int isDetect, string cabinetName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ToolDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(MtToolInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            MtToolInfo itemRecord = (MtToolInfo)result[0];
                            itemRecord.CodeNo = codeNo;
                            itemRecord.BCodeTypeId = bCodeTypeId;
                            itemRecord.Name = name;
                            itemRecord.Model = model;
                            itemRecord.BCategoryId = bCategoryId;
                            itemRecord.BrandName = brandName;
                            itemRecord.BUnitId = bUnitId;
                            itemRecord.Place = place;
                            itemRecord.Remark = remark;
                            itemRecord.Pym = pym;
                            itemRecord.Month = month;
                            itemRecord.PublishManId = publishManId;
                            itemRecord.PublishTime = publishTime;
                            itemRecord.State = state;
                            itemRecord.IsCheck = isCheck;
                            itemRecord.IsDetect = isDetect;
                            itemRecord.CabinetName = cabinetName;
                            session.SaveOrUpdate(itemRecord);
                            transaction.Commit();
                            return 1;
                        }
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int BatchUpdateMtToolInfo(IList<MtToolInfo> recordList)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ToolDb);
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        for (int i = 0; i < recordList.Count; i++)
                        {
                            session.Update(recordList[i]);
                        }
                        transaction.Commit();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int UpdateMtToolInfo(MtToolInfo itemRecord)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ToolDb);
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Update(itemRecord);
                        transaction.Commit();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int DeleteMtToolInfo(int id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ToolDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(MtToolInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            MtToolInfo itemRecord = (MtToolInfo)result[0];
                            session.Delete(itemRecord);
                            transaction.Commit();
                            return 1;
                        }
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }
    }
}
