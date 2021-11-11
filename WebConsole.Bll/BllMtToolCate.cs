using System.Collections.Generic;
using Domain.MtToolDb.Domain;
using Utilities.DbHelper;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllMtToolCate
    {
        public static IList<MtToolCate> SearchMtToolCate()
        {
            return DalMtToolCate.SearchMtToolCate();
        }

        public static int GetMtToolCateCount()
        {
            return DalMtToolCate.GetMtToolCateCount();
        }

        public static MtToolCate GetMtToolCate(int id)
        {
            return DalMtToolCate.GetMtToolCate(id);
        }

        public static int AddMtToolCate(string no, string name, int fatherID)
        {
            return DalMtToolCate.AddMtToolCate(no, name, fatherID);
        }

        public static int ModifyMtToolCate(int iD, string no, string name, int fatherID)
        {
            return DalMtToolCate.ModifyMtToolCate(iD, no, name, fatherID);
        }

        public static int BatchUpdateMtToolCate(IList<MtToolCate> recordList)
        {
            if (!SqlDataHelper.IsDataValid(recordList)) return -100;
            return DalMtToolCate.BatchUpdateMtToolCate(recordList);
        }

        public static int UpdateMtToolCate(MtToolCate itemRecord)
        {
            if (itemRecord == null) return -100;
            return DalMtToolCate.UpdateMtToolCate(itemRecord);
        }

        public static int DeleteMtToolCate(int id)
        {
            return DalMtToolCate.DeleteMtToolCate(id);
        }
    }
}
