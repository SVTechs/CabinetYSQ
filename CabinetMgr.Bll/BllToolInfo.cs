using System;
using System.Collections;
using System.IO;
using CabinetMgr.Bll.ToolServiceRef;
using CabinetMgr.Config;
using CabinetMgr.Dal;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Utilities.DbHelper;
using Utilities.Json;

namespace CabinetMgr.Bll
{
    public class BllToolInfo
    {
        private static readonly ToolService ToolService = new ToolService();

        public static int AddToolInfoFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return -100;
            int affCount = 0;
            using (FileStream excelFs = File.OpenRead(filePath))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(excelFs);
                HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0);
                workbook.MissingCellPolicy = MissingCellPolicy.CREATE_NULL_AS_BLANK;
                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    HSSFRow kRow = (HSSFRow)sheet.GetRow(row);
                    if (kRow != null && kRow.Cells.Count > 0)
                    {
                        try
                        {
                            if (kRow.Cells.Count < 3) continue;
                            string position = GetCellString(kRow.GetCell(0));
                            string name = GetCellString(kRow.GetCell(1));
                            string spec = GetCellString(kRow.GetCell(2));
                            int toolTypeId = 0;
                            string toolType = "";
                            if (name.Contains("扭力扳手"))
                            {
                                toolTypeId = 5;
                                toolType = "扭力扳手";
                            }
                            affCount += DalToolInfo.AddToolInfo(name, position, spec, toolTypeId, toolType, position, null, null, 0, 0, 0,
                                int.Parse(position),
                                null, 100, "年", "", "", "");
                        }
                        catch (Exception)
                        {
                            workbook.Close();
                            return -100000 - row;
                        }
                    }
                }
                workbook.Close();
            }
            return affCount;
        }

        private static string GetCellString(ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue.TrimStart().TrimEnd();
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
            }
            return "";
        }

        public static int AddToolInfo(string toolName, string toolCode, string toolSpec, string toolType, string hardwareId, string cardId,
                    string standardValue, float deviationPositive, float deviationNegative, int toolPositionType,
                    int toolPosition, string toolGrid, float checkInterval, string checkIntervalType, string toolManager,
                    string comment, string operatorName)
        {
            if (string.IsNullOrEmpty(toolName) || toolPositionType < 0 || toolPosition < 0) return -100;
            return DalToolInfo.AddToolInfo(toolName, toolCode, toolSpec, 0, toolType, hardwareId, cardId, standardValue,
                deviationPositive, deviationNegative,
                toolPositionType, toolPosition, toolGrid, checkInterval, checkIntervalType, toolManager, comment,
                operatorName);
        }

        public static int ModifyToolInfo(string identId, string toolName, string toolCode, string toolSpec,
            string toolType, string hardwareId, string cardId, string standardValue, float deviationPositive, float deviationNegative,
            int toolPositionType, int toolPosition, string toolGrid, float checkInterval, string checkIntervalType,
            string toolManager, string comment, string operatorName)
        {
            if (string.IsNullOrEmpty(identId) || string.IsNullOrEmpty(toolName) || toolPositionType < 0 || toolPosition < 0) return -100;
            return DalToolInfo.ModifyToolInfo(identId, toolName, toolCode, toolSpec, toolType, hardwareId, cardId, standardValue,
                deviationPositive,
                deviationNegative, toolPositionType, toolPosition, toolGrid, checkInterval, checkIntervalType,
                toolManager, comment, operatorName);
        }

        public static int DeleteToolInfo(string identId)
        {
            if (string.IsNullOrEmpty(identId)) return -100;
            return DalToolInfo.DeleteToolInfo(identId);
        }

        public static int SetToolStatus(string toolCode, int status, string operatorName)
        {
            if (string.IsNullOrEmpty(toolCode)) return -100;
            return DalToolInfo.SetToolStatus(toolCode, status, operatorName);
        }

        public static int SetToolStatusById(string toolId, int status, string operatorName)
        {
            if (string.IsNullOrEmpty(toolId)) return -100;
            return DalToolInfo.SetToolStatusById(toolId, status, operatorName);
        }

        public static int UpdateRtStatus(CabinetCallback.ToolStatus[] toolStatusList)
        {
            return DalToolInfo.UpdateRtStatus(toolStatusList);
        }

        public static int SetRtStatus(string toolCode, int status)
        {
            return DalToolInfo.SetRtStatus(toolCode, status);
        }

        public static ToolInfo GetToolInfoByHardwareId(string hardwareId)
        {
            if (string.IsNullOrEmpty(hardwareId)) return null;
            return DalToolInfo.GetToolInfoByHardwareId(hardwareId);
        }

        public static ToolInfo GetToolInfoById(string toolId)
        {
            if (string.IsNullOrEmpty(toolId)) return null;
            return DalToolInfo.GetToolInfoById(toolId);
        }

        public static ToolInfo GetToolInfo(string toolCode)
        {
            if (string.IsNullOrEmpty(toolCode)) return null;
            return DalToolInfo.GetToolInfo(toolCode);
        }

        public static ToolInfo GetToolInfoByCard(string cardId)
        {
            return DalToolInfo.GetToolInfoByCard(cardId);
        }

        public static ToolInfo GetUpperToolInfo(int toolPosition)
        {
            if (toolPosition < 0) return null;
            return DalToolInfo.GetUpperToolInfo(toolPosition);
        }

        public static IList GetToolCategory()
        {
            return DalToolInfo.GetToolCategory();
        }

        public static IList SearchDrawerToolInfo()
        {
            return DalToolInfo.SearchDrawerToolInfo();
        }

        public static IList SearchToolInfo(int toolPositionType, int toolPosition, string toolName, string toolSpec, string toolType)
        {
            return DalToolInfo.SearchToolInfo(toolPositionType, toolPosition, toolName, toolSpec, toolType);
        }

        public static IList SearchChangedToolInfo()
        {
            return DalToolInfo.SearchChangedToolInfo();
        }

        public static IList SearchExpiredToolInfo(int toolPositionType)
        {
            return DalToolInfo.SearchExpiredToolInfo(toolPositionType);
        }

        public static IList SearchServerToolInfo(string lastSyncTime, string cabinetName)
        {
            //ToolService.Url = "http://localhost:57847/ToolService.asmx";
            if (!string.IsNullOrEmpty(AppConfig.ToolServiceUrl)) ToolService.Url = AppConfig.ToolServiceUrl;
            string toolData = ToolService.GetAllTool(DateTime.Parse(lastSyncTime), cabinetName);
            JArray jArray = ConvertJson.ParseJArray(toolData);
            if (SqlDataHelper.IsJsonDataValid(jArray))
            {
                IList toolList = new ArrayList();
                for (int i = 0; i < jArray.Count; i++)
                {
                    JObject obj = (JObject)jArray[i];
                    ToolInfo ti = new ToolInfo()
                    {
                        ServerIdent = obj["Id"].ToString(),
                        ToolName = obj["toolName"]?.ToString(),
                        ToolTypeId = obj["ItemCategoryId"]?.ToString(),
                        ToolType = obj["toolType"]?.ToString(),
                        ToolSpec = obj["Spec"]?.ToString(),
                        Comment = obj["Alias"]?.ToString(),//obj["Remark"].ToString(),
                        Operator = string.IsNullOrEmpty(obj["Creator"]?.ToString()) ? obj["LastModifier"].ToString() : obj["Creator"].ToString(),
                        OperateTime = string.IsNullOrEmpty(obj["CreatedTime"]?.ToString()) ? DateTime.Parse(obj["LastModifiedTime"].ToString()) : DateTime.Parse(obj["CreatedTime"].ToString()),
                        //ToolPosition = (obj["CabinetName"] ?? "").ToString() == AppConfig.CabinetName ? -1 : -500
                    };
                    toolList.Add(ti);
                }
                return toolList;
            }
            return null;
        }
    }
}
