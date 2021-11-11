using System;
using System.IO.Ports;
using System.Text;
using NLog;

namespace Hardware.DeviceInterface
{
    public class MeasuringDevice
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static SerialPort _serialCtrl;

        public class MeasuringInfo
        {
            public int ToolId;
            public float RealValue;

            public MeasuringInfo(int toolId, float realValue)
            {
                ToolId = toolId;
                RealValue = realValue;
            }
        }

        public static int Init(int serialPort)
        {
            try
            {
                if (_serialCtrl == null || !_serialCtrl.IsOpen)
                {
                    _serialCtrl = new SerialPort("COM" + serialPort);
                    _serialCtrl.BaudRate = 9600;
                    _serialCtrl.Parity = Parity.None;
                    _serialCtrl.DataBits = 8;
                    _serialCtrl.StopBits = StopBits.One;
                    _serialCtrl.Encoding = Encoding.Default;

                    try
                    {
                        _serialCtrl.DataReceived += DataReceived;
                        _serialCtrl.Open();
                        return 1;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static int Close()
        {
            if (_serialCtrl.IsOpen)
            {
                try
                {
                    _serialCtrl.Close();
                    return 1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            return -10;
        }

        public static void GenerateTestData(int toolPosition, float dataBase)
        {
            Random rd = new Random();
            MeasuringInfo uInfo = new MeasuringInfo(toolPosition, dataBase + (float)rd.Next(1, 10) / 100);
            MeasuringCallback.OnMeasuringDataReceived?.Invoke(uInfo);
        }

        private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int datalength = _serialCtrl.BytesToRead;
            char[] dataBuffer = new char[datalength];
            _serialCtrl.Read(dataBuffer, 0, datalength);
            if (datalength <= 5)
            {
                return;
            }
            string serialData = new string(dataBuffer);
            string[] dataArray = serialData.Replace("\r", "").Replace("#", "").Split('+');
            try
            {
                MeasuringInfo uInfo = new MeasuringInfo(int.Parse(dataArray[0]), float.Parse(dataArray[1]));
                MeasuringCallback.OnMeasuringDataReceived?.Invoke(uInfo);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                // ignored
            }
        }
    }
}
