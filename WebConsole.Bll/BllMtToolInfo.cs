using System;
using System.Collections.Generic;
using Domain.MtToolDb.Domain;
using Utilities.DbHelper;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllMtToolInfo
    {
        public static IList<MtToolInfo> SearchMtToolInfo(string toolCode, string toolName, string toolSpec, int toolCate,
            int dataStart, int dataCount)
        {
            return DalMtToolInfo.SearchMtToolInfo(toolCode, toolName, toolSpec, toolCate, dataStart, dataCount);
        }

        public static int GetMtToolInfoCount(string toolCode, string toolName, string toolSpec, int toolCate)
        {
            return DalMtToolInfo.GetMtToolInfoCount(toolCode, toolName, toolSpec, toolCate);
        }

        public static MtToolInfo GetMtToolInfo(int id)
        {
            return DalMtToolInfo.GetMtToolInfo(id);
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
            return DalMtToolInfo.AddMtToolInfo(codeNo, bCodeTypeId, name, model, bCategoryId, brandName, 
                bUnitId, place, remark, pym, month, publishManId, publishTime, state, isCheck, isDetect, cabinetName);
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
            return DalMtToolInfo.ModifyMtToolInfo(id, codeNo, bCodeTypeId, name, model, bCategoryId, brandName, 
                bUnitId, place, remark, pym, month, publishManId, publishTime, state, isCheck, isDetect, cabinetName);
        }

        public static int BatchUpdateMtToolInfo(IList<MtToolInfo> recordList)
        {
            if (!SqlDataHelper.IsDataValid(recordList)) return -100;
            return DalMtToolInfo.BatchUpdateMtToolInfo(recordList);
        }

        public static int UpdateMtToolInfo(MtToolInfo itemRecord)
        {
            if (itemRecord == null) return -100;
            return DalMtToolInfo.UpdateMtToolInfo(itemRecord);
        }

        public static int DeleteMtToolInfo(int id)
        {
            return DalMtToolInfo.DeleteMtToolInfo(id);
        }
    }
}
