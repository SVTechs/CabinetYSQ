using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utilities.DbHelper;

namespace Utilities.Json
{
    //JSON转换类
    public class ConvertJson
    {
        /**
         * 生成JQGrid条件解析用的前缀HashMap
         * @return HashMap
        */
        private static Hashtable GenPrefixTable()
        {
            Hashtable map = new Hashtable();
            map.Add("eq", " = '");
            map.Add("ne", " != '");
            map.Add("lt", " < '");
            map.Add("le", " <= '");
            map.Add("gt", " > '");
            map.Add("ge", " >= '");
            map.Add("nu", " ");
            map.Add("nn", " ");
            map.Add("in", " = '");
            map.Add("ni", " != '");
            map.Add("bw", " like '");
            map.Add("bn", " not like '");
            map.Add("ew", " like '%");
            map.Add("en", " not like '%");
            map.Add("cn", " like '%");
            map.Add("nc", " not like '%");
            return map;
        }

        /**
         * 生成JQGrid条件解析用的后缀HashMap
         * @return HashMap
         */
        private static Hashtable GenSuffixTable()
        {
            Hashtable map = new Hashtable();
            map.Add("eq", "'");
            map.Add("ne", "'");
            map.Add("lt", "'");
            map.Add("le", "'");
            map.Add("gt", "'");
            map.Add("ge", "'");
            map.Add("nu", " is null");
            map.Add("nn", " is not null");
            map.Add("in", "'");
            map.Add("ni", "'");
            map.Add("bw", "%'");
            map.Add("bn", "%'");
            map.Add("ew", "'");
            map.Add("en", "'");
            map.Add("cn", "%'");
            map.Add("nc", "%'");
            return map;
        }

        /**
         * 解析JQGrid传送的数据并生成排序及条件语句返回
         * @param request HttpServletRequest
         * @return HashMap(包含排序方式orderMethod及条件whereFilter)
         */
        public static Hashtable ParseJqGridJson(NameValueCollection formData)
        {
            Hashtable resMap = new Hashtable();
            Hashtable jqMap = GenPrefixTable(), jqMapSuffix = GenSuffixTable();
            string orderMethod = formData["sidx"];
            if (orderMethod != null && orderMethod.Length != 0)
            {
                orderMethod += " " + formData["sord"];
            }
            string whereFilter = "";
            try
            {
                string wFilterStr = formData["filters"];
                if (wFilterStr != null && wFilterStr.Length != 0)
                {
                    whereFilter += "(";
                    Dictionary<string, object> decBase = JsonConvert.DeserializeObject<Dictionary<string, object>>(wFilterStr);
                    string groupOp = decBase["groupOp"].ToString();
                    JArray tempArray = (JArray)decBase["rules"];
                    for (int i = 0; i < tempArray.Count; i++)
                    {
                        JObject curObject = (JObject)tempArray[i];
                        string oPs = curObject.Value<string>("op");
                        whereFilter += curObject.Value<string>("field") + jqMap[oPs] + curObject.Value<string>("data") + jqMapSuffix[oPs];
                        if (i != tempArray.Count - 1)
                        {
                            whereFilter += " " + groupOp + " ";
                        }
                    }
                    whereFilter += ")";
                }
            }
            catch (Exception)
            {
                // ignored
            }
            resMap.Add("orderMethod", orderMethod);
            resMap.Add("whereFilter", whereFilter);
            return resMap;
        }

        public static Hashtable ParseJqGridJsonHt(NameValueCollection formData)
        {
            Hashtable resMap = new Hashtable();
            //解析JQGrid排序
            List<HibernateHelper.OrderEntity> orderCollection = new List<HibernateHelper.OrderEntity>();
            string orderColumn = formData["sidx"];
            if (!string.IsNullOrEmpty(orderColumn))
            {
                HibernateHelper.OrderEntity oe = new HibernateHelper.OrderEntity();
                oe.ColumnName = orderColumn;
                oe.Order = formData["sord"];
                orderCollection.Add(oe);
            }
            //解析JQGrid条件
            List<HibernateHelper.WhereEntity> whereCollection = new List<HibernateHelper.WhereEntity>();
            try
            {
                string wFilterStr = formData["filters"];
                if (!string.IsNullOrEmpty(wFilterStr))
                {
                    Dictionary<string, object> decBase = JsonConvert.DeserializeObject<Dictionary<string, object>>(wFilterStr);

                    HibernateHelper.WhereEntity oe = new HibernateHelper.WhereEntity();
                    oe.GroupOp = decBase["groupOp"].ToString().ToUpper();
                    JArray tempArray = (JArray)decBase["rules"];
                    for (int i = 0; i < tempArray.Count; i++)
                    {
                        JObject curObject = (JObject)tempArray[i];
                        oe.ColumnOp = curObject.Value<string>("op").ToUpper();
                        oe.ColumnName = curObject.Value<string>("field");
                        oe.ColumnValue = curObject.Value<string>("data");
                    }
                    whereCollection.Add(oe);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            resMap.Add("orderMethod", orderCollection);
            resMap.Add("whereFilter", whereCollection);
            return resMap;
        }

        public static JObject ParseJson(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData))
                return null;
            return (JObject)JsonConvert.DeserializeObject(jsonData);
        }

        public static JArray ParseJArray(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData))
                return null;
            return (JArray)JsonConvert.DeserializeObject(jsonData);
        }

        public static DataSet JsonToDataSet(string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            return JsonConvert.DeserializeObject<DataSet>(json);
        }

        public static DataTable JsonToDataTable(string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            return JsonConvert.DeserializeObject<DataTable>(json);
        }

        public static string DataSetToJson(DataSet ds)
        {
            if (ds == null)
                return null;
            return JsonConvert.SerializeObject(ds);
        }

        public static string ObjectToJson(object ds)
        {
            if (ds == null)
                return null;
            return JsonConvert.SerializeObject(ds);
        }

        public static string DataTableToJson(DataTable ds)
        {
            if (ds == null)
                return null;
            return JsonConvert.SerializeObject(ds);
        }

        public static string HashTableToJson(Hashtable ht)
        {
            return JsonConvert.SerializeObject(ht);
        }

        public static Hashtable JsonToHashtable(string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            return JsonConvert.DeserializeObject<Hashtable>(json);
        }

        public static string ListToJson(ArrayList data)
        {
            if (data == null)
                return null;
            return JsonConvert.SerializeObject(data);
        }

        public static string ListToJson(List<string> data)
        {
            if (data == null)
                return null;
            return JsonConvert.SerializeObject(data);
        }
    }
}