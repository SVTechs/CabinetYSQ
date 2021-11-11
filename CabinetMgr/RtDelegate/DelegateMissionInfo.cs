using Hardware.DeviceInterface;

namespace CabinetMgr.RtDelegate
{
    public class DelegateMissionInfo
    {
        public delegate void RefreshMissionDelegate();

        public static RefreshMissionDelegate RefreshMission = null;

        public static TorqueCallback.OnTorqueDataReceivedDelegate OnTorqueDataReceived = null;

        public delegate void UpdateMachineCodeDelegate(string machineCode);

        public static UpdateMachineCodeDelegate UpdateMachineCode = null;



        public delegate void OnToolTakenDelegate(int toolPosition);

        public static OnToolTakenDelegate OnToolTaken = null;

        public delegate void OnToolReturnDelegate(int toolPosition);

        public static OnToolReturnDelegate OnToolReturn = null;
    }
}
