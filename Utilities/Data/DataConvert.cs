using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utilities.Data
{
    public class DataConvert
    {
        public static bool Gmt2Local(string gmt, out DateTime gmtTime)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            return DateTime.TryParseExact(gmt, "r", enUS, DateTimeStyles.None, out gmtTime);
        }

        public static IList<T> DataSetToIList<T>(DataSet info)
        {
            DataTable dt = info.Tables[0];

            IList<T> result = new List<T>();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (pi.Name.Equals(dt.Columns[i].ColumnName))
                        {
                            if (dt.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, dt.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
    }
}
