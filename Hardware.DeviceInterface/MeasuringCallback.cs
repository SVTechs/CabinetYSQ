namespace Hardware.DeviceInterface
{
    public class MeasuringCallback
    {
        /// <summary>
        /// 串口数据回调
        /// </summary>
        /// <param name="usageInfo"></param>
        public delegate void OnMeasuringDataReceivedDelegate(MeasuringDevice.MeasuringInfo usageInfo);
        public static OnMeasuringDataReceivedDelegate OnMeasuringDataReceived = null;
    }
}
