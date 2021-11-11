using System;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using Utilities.DbHelper;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllToolInfo
    {

        public static IList<ToolInfo> SearchToolInfo()
        {
            return DalToolInfo.SearchToolInfo();
        }

        public static IList<ToolInfo> SearchToolInfo(int toolPositionType, int toolPosition, string toolName,
            string toolSpec, string cabinetName)
        {
            return DalToolInfo.SearchToolInfo(toolPositionType, toolPosition, toolName, toolSpec, cabinetName);
        }

        public static IList<ToolInfo> SearchToolInfo(string toolName, string toolSpec, string cabinetName, int dataStart, int dataCount)
        {
            return DalToolInfo.SearchToolInfo(toolName, toolSpec, cabinetName, dataStart, dataCount);
        }

        public static int GetToolInfoCount()
        {
            return DalToolInfo.GetToolInfoCount();
        }

        public static int GetToolInfoCount(string toolName, string toolSpec, string dataOwner)
        {
            return DalToolInfo.GetToolInfoCount(toolName, toolSpec, dataOwner);
        }

        public static ToolInfo GetToolInfo(string id)
        {
            return DalToolInfo.GetToolInfo(id);
        }

        public static IList<string> GetToolType()
        {
            return DalToolInfo.GetToolType();
        }

        public static IList<string> GetToolNameList()
        {
            return DalToolInfo.GetToolNameList();
        }

        public static int AddToolInfo(
            string toolName,
            string toolCode,
            string toolSpec,
            string toolType,
            string hardwareId,
            string cardId,
            string standardRange,
            decimal deviationPositive,
            decimal deviationNegative,
            int toolPositionType,
            int toolPosition,
            string toolGrid,
            DateTime checkTime,
            DateTime nextCheckTime,
            float checkInterval,
            string checkIntervalType,
            string toolManager,
            string comment,
            string toolOperator,
            DateTime operateTime,
            int rtStatus,
            int toolStatus,
            string dataOwner)
        {
            return DalToolInfo.AddToolInfo(toolName, toolCode, toolSpec, toolType, hardwareId, cardId, standardRange, deviationPositive, deviationNegative, toolPositionType, toolPosition, toolGrid, checkTime, nextCheckTime, checkInterval, checkIntervalType, toolManager, comment, toolOperator, operateTime, rtStatus, toolStatus, dataOwner);
        }

        public static int ModifyToolInfo(
            string id,
            string toolName,
            string toolCode,
            string toolSpec,
            string toolType,
            string hardwareId,
            string cardId,
            string standardRange,
            decimal deviationPositive,
            decimal deviationNegative,
            int toolPositionType,
            int toolPosition,
            string toolGrid,
            DateTime checkTime,
            DateTime nextCheckTime,
            float checkInterval,
            string checkIntervalType,
            string toolManager,
            string comment,
            string toolOperator,
            DateTime operateTime,
            int rtStatus,
            int toolStatus,
            string dataOwner)
        {
            return DalToolInfo.ModifyToolInfo(id, toolName, toolCode, toolSpec, toolType, hardwareId, cardId, standardRange, deviationPositive, deviationNegative, toolPositionType, toolPosition, toolGrid, checkTime, nextCheckTime, checkInterval, checkIntervalType, toolManager, comment, toolOperator, operateTime, rtStatus, toolStatus, dataOwner);
        }

        public static int BatchUpdateToolInfo(IList<ToolInfo> recordList)
        {
            if (!SqlDataHelper.IsDataValid(recordList)) return -100;
            return DalToolInfo.BatchUpdateToolInfo(recordList);
        }

        public static int UpdateToolInfo(ToolInfo itemRecord)
        {
            if (itemRecord == null) return -100;
            return DalToolInfo.UpdateToolInfo(itemRecord);
        }

        public static int DeleteToolInfo(string id)
        {
            return DalToolInfo.DeleteToolInfo(id);
        }
    }
}
