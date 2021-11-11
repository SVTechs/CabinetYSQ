using System.Data;

namespace UpdateControl
{
    public class Util_Data
    {
        public static bool IsDataValid(DataSet ds)
        {
            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                return true;
            }
            return false;
        }
    }
}

