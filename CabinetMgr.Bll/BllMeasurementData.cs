using System;
using System.Collections.Generic;
using CabinetMgr.Bll.MeasureServiceRef;
using CabinetMgr.Config;
using CabinetMgr.Dal;
using Domain.Main.Domain;
using NLog;

namespace CabinetMgr.Bll
{
    public class BllMeasurementData
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly RepairService MeasureService = new RepairService();

        public static IList<RepairProcessEntity> SearchMachineCode()
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                return MeasureService.ListCodeData(Env.DataType);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public static RepairProcessEntity GetRepairProcessDataEx(string machineCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                RepairProcessEntity[] re = MeasureService.GetProcessList(machineCode, Env.DataType);
                return re?[0];
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public static RepairProcessEntity GetRepairProcessData(string machineCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                RepairProcessEntity[] re = MeasureService.ListRepairProcessData(machineCode, Env.DataType);
                return re?[0];
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public static bool CheckStandardCode(string standardCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                return MeasureService.SearchPartNum(standardCode, Env.DataType);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        public static IList<WorkingProcessEntity> GetProcessList(string machineCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                return MeasureService.GetWorkingProcessListByDeviceCode(machineCode, Env.DataType);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public static bool AddStandardCode(string machineCode, string partName, string processType)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                return MeasureService.WritePartNum(machineCode, partName, processType, Env.DataType);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        public static bool AddWrenchInfo(string magtaCode, string targetValue, string realValue, string userId,
            string jobResult, DateTime startTime, DateTime endTime, string trainType, string trainNum,
            string stepId, string pairId)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                return MeasureService.AddWrenchInfo(magtaCode, targetValue, realValue, userId, jobResult, 
                    startTime, endTime, "", trainType, trainNum, stepId, pairId, stepId, Env.DataType);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        public static bool AddQfcInfo(string toolCode, string realValue, string userId, string jobResult, DateTime startTime, 
            DateTime endTime, string trainType, string trainNum, string stepId, string pairId)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                return MeasureService.AddQianfenchiInfo(userId, toolCode, realValue, startTime, endTime, "", trainType,
                    trainNum, stepId, pairId, stepId, jobResult, Env.DataType);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        public static IList<WrenchInfoEntity> GetWrenchInfo(string stepId)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                return MeasureService.GetWorkingWrenchInfoListByStepId(stepId, Env.DataType);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public static bool UpdateStepInfo(WorkingStepEntity stepInfo)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                MeasureService.UpdateWorkingStep(stepInfo.Id, stepInfo.Status, stepInfo.StepPeopleId, stepInfo.StepPeopleName);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        public static bool UpdateWrenchInfo(WrenchInfoEntity wrenchInfo)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                MeasureService.UpdateWrenchInfo(wrenchInfo);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        public static bool DeleteWrenchInfo(WrenchInfoEntity wrenchInfo)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppConfig.MeasureServiceUrl))
                {
                    MeasureService.Url = AppConfig.MeasureServiceUrl;
                }
                MeasureService.DelWrenchInfo(wrenchInfo);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        public static int AddMeasurementData(string workUserInfoId, string deviceCode, string trainType,
            string trainNum, string process, string toolCode, int toolType, string defaultJobValue,
            string dataValue, int jobResult, string taskUser)
        {
            return DalMeasurementData.AddMeasurementData(workUserInfoId, deviceCode, trainType,
                trainNum, process, toolCode, toolType, defaultJobValue, taskUser);
        }

        public static int DeleteAll()
        {
            return DalMeasurementData.DeleteAll();
        }

        public static int SaveResult(string toolCode, float dataValue)
        {
            if (string.IsNullOrEmpty(toolCode)) return -100;
            return DalMeasurementData.SaveResult(toolCode, dataValue);
        }

        public static MeasurementData GetMeasurementData(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return null;
            return DalMeasurementData.GetMeasurementData(recordId);
        }

        public static MeasurementData GetMeasurementDataByToolCode(string toolId)
        {
            if (string.IsNullOrEmpty(toolId)) return null;
            return DalMeasurementData.GetMeasurementDataByToolCode(toolId);
        }
    }
}
