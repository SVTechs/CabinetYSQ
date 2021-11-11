namespace Hardware.DeviceInterface
{
    public class TorqueCallback
    {
        /// <summary>
        /// 串口数据回调
        /// </summary>
        /// <param name="usageInfo"></param>
        public delegate void OnTorqueDataReceivedDelegate(TorqueDevice.TorqueInfo usageInfo);
        public static OnTorqueDataReceivedDelegate OnTorqueDataReceived = null;
        public delegate void OnIRTorqueDataReceivedDelegate(TorqueDevice.TorqueInfo usageInfo);
        public static OnIRTorqueDataReceivedDelegate OnIRTorqueDataReceived = null;
    }
}
