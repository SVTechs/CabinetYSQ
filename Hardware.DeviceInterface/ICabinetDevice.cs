using System;
using System.Collections.Generic;

namespace Hardware.DeviceInterface
{
    public interface ICabinetDevice
    {
        void CanDoorOpen(bool open);

        void EmulateToolTaken(int toolPosition);

        void EmulateToolReturn(int toolPosition);

        void SetDrawerUnlockTime(int unlockTime);

        int AddCheckerMapping(int origin, int dest);

        int AddOperatorMapping(int origin, int dest);

        int AddToolCheckerDevice(string toolCheckerIp,
            int toolCheckerPort, ushort toolCheckerCapacity, ushort toolCheckerStartIndex);

        int AddToolCheckerComDevice(int comPort, ushort toolCheckerCapcity, ushort toolCheckerStartIndex);

        int AddToolLedOperatorDevice(string toolLedOperatorIp,
            int toolLedOperatorPort, ushort toolLedOperatorCapacity, ushort toolLedOperatorStartIndex);

        int InitDevice(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string cabinetCheckerIp,
            string cabinetOperatorIp, int cabinetCheckerPort = 502, int cabinetOperatorPort = 502,
            bool debugMode = false);

        int InitDevice(ushort toolCount, ushort drawerCount, int cabinetCheckerComPort,
            int cabinetOperatorComPort, bool debugMode = false);

        int InitDevicePure(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string cabinetCheckerIp,
            string cabinetOperatorIp, int cabinetCheckerPort = 502, int cabinetOperatorPort = 502,
            bool debugMode = false);

        void SetLedMapping(int lightPoint, int yellowPoint, int redPoint);

        bool IsInitDone();

        int UnlockDrawer(bool[] status, bool updateSignal = true);

        int UnlockDrawer(int drawerNo, bool status, bool updateSignal = true);

        int UnlockDoor(bool status, bool updateSignal = true);

        int LockDoor(bool status, bool updateSignal = true);

        bool[] IsDoorUnlocking();

        int OpenLight(int lightNo, bool status);

        int OpenAllLight(bool status);

        int OpenLedGreen(bool status);

        int OpenLedYellow(bool status);

        int OpenLedRed(bool status);

        int OpenAlarm(bool status);

        CabinetCallback.ToolStatus[] GetToolStatus();

        int SetToolLedStatus(bool[] status);

        void SetDrawerUnlock(int status);

        bool[] GetToolLedStatus();

        void StartToolAlert(int ledNo, DateTime expireTime);

        void EndToolAlert(int ledNo);

        void SetDrawerAlertStatus(int ledNo, bool enableAlert);

        void SetDrawerLedOpenStatus(int ledNo, bool isOpened);

        void SetToolLedWaitStatus(int ledNo, bool isWaiting);

        void SetToolLedWaiting(List<int> ledNo);

        void SetToolLedRepairStatus(int ledNo, bool isRepairing);

        void SetToolLedCheckStatus(int ledNo, bool needCheck);

        void ResetToolLed();

        //For CanBus
        int AddToolCheckerDevice(string toolCheckerIp,
            int toolCheckerPort, ushort nodeId, ushort toolCheckerCapacity, ushort toolCheckerStartIndex);

        int AddToolCheckerComDevice(int comPort, ushort nodeId, ushort toolCheckerCapcity,
            ushort toolCheckerStartIndex);

        int AddToolLedOperatorDevice(string toolLedOperatorIp,
            int toolLedOperatorPort, ushort nodeId, ushort toolLedOperatorCapacity, ushort toolLedOperatorStartIndex);

        int AddToolLedOperatorDevice(int comPort, ushort nodeId, ushort toolLedOperatorCapacity,
            ushort toolLedOperatorStartIndex);

        int InitDevice(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string canBusIp, int canBusPort,
            int cabinetCheckerNode = 1, int cabinetOperatorNode = 1, bool debugMode = false);

        int InitDevice(ushort toolCount, ushort drawerCount, int canBusComPort,
            int cabinetCheckerNode = 1, int cabinetOperatorNode = 1, bool debugMode = false);

        int InitDevicePure(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string canBusIp,
            int canBusPort,
            int cabinetCheckerNode = 1, int cabinetOperatorNode = 1, bool debugMode = false);
    }
}
