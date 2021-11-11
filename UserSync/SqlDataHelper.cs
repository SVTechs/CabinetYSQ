using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace UserSync
{
    public class SqlDataHelper
    {
        public static bool IsDataValid(DataSet ds)
        {
            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsDataValid(IList ls)
        {
            if (ls != null && ls.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsDataValid<T>(IList<T> ls)
        {
            if (ls != null && ls.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsDataValid(ArrayList ls)
        {
            if (ls != null && ls.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// DataSet装换为泛型集合 
        /// <typeparam name="T"></typeparam> 
        /// <param name="pDataSet">DataSet</param> 
        /// <param name="pTableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static IList<T> DataSetToIList<T>(DataSet pDataSet, int pTableIndex)
    {
        if (pDataSet == null || pDataSet.Tables.Count < 0)
            return null;
        if (pTableIndex > pDataSet.Tables.Count - 1)
            return null;
        if (pTableIndex < 0)
            pTableIndex = 0;
 
        DataTable pData = pDataSet.Tables[pTableIndex];
        // 返回值初始化 
        IList<T> result = new List<T>();
        for (int j = 0; j < pData.Rows.Count; j++)
        {
            T t = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                for (int i = 0; i < pData.Columns.Count; i++)
                {
                    // 属性与字段名称一致的进行赋值 
                    if (pi.Name.Equals(pData.Columns[i].ColumnName))
                    {
                        // 数据库NULL值单独处理 
                        if (pData.Rows[j][i] != DBNull.Value)
                        {
                            if (pData.Rows[j][i] is long && pi.PropertyType == typeof(int))
                            {
                                pi.SetValue(t, Convert.ToInt32(pData.Rows[j][i]), null);
                            }
                            else if (pData.Rows[j][i] is long && pi.PropertyType == typeof(short))
                            {
                                pi.SetValue(t, Convert.ToInt16(pData.Rows[j][i]), null);
                            }
                            else if (pData.Rows[j][i] is double && pi.PropertyType == typeof(float))
                            {
                                pi.SetValue(t, Convert.ToInt16(pData.Rows[j][i]), null);
                            }
                            else
                            {
                                pi.SetValue(t, pData.Rows[j][i], null);
                            }
                        }
                        else
                        {
                            pi.SetValue(t, null, null);
                        }
                        break;
                    }
                }
            }
            result.Add(t);
        }
        return result;
    }

        /// <summary>
        /// Ilist 转换成 DataSet
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataSet ConvertToDataSet<T>(IList list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;
            PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (T t in list)
            {
                if (t == null)
                {
                    continue;
                }
                row = dt.NewRow();
                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    PropertyInfo pi = myPropertyInfo[i];
                    if (pi.PropertyType.FullName.StartsWith("System.Collections")) continue;
                    string name = pi.Name;
                    if (dt.Columns[name] == null)
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }
                    row[name] = pi.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>
        /// Ilist 转换成 DataSet
        /// </summary>
        /// <param name="tableType"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataSet ConvertToDataSet(Type tableType, IList list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(tableType.Name);
            DataColumn column;
            DataRow row;
            PropertyInfo[] myPropertyInfo = tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var t in list)
            {
                if (t == null)
                {
                    continue;
                }
                row = dt.NewRow();
                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    PropertyInfo pi = myPropertyInfo[i];
                    if (pi.PropertyType.FullName.StartsWith("System.Collections")) continue;
                    if (!pi.PropertyType.FullName.StartsWith("System.")) continue;
                    string name = pi.Name;
                    if (dt.Columns[name] == null)
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }
                    row[name] = pi.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            return ds;
        }
    }
}
