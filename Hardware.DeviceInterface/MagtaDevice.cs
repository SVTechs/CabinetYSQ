using System;
using System.IO.Ports;

namespace Hardware.DeviceInterface
{
    public class MagtaDevice
    {
        private static SerialPort _serialCtrl;
        private static TorqueDevice.TorqueInfo _lastPackage;

        public static int Init(int serialPort, int iBaudRate, int iParity, int iDataBits, int iStopBits)
        {
            try
            {
                _serialCtrl = new SerialPort("COM" + serialPort);
                _serialCtrl.BaudRate = iBaudRate;
                switch (iParity)
                {
                    case 0:
                        _serialCtrl.Parity = Parity.None;
                        break;
                    case 1:
                        _serialCtrl.Parity = Parity.Odd;
                        break;
                    case 2:
                        _serialCtrl.Parity = Parity.Even;
                        break;
                }
                _serialCtrl.DataBits = iDataBits;
                switch (iStopBits)
                {
                    case 0:
                        _serialCtrl.StopBits = StopBits.None;
                        break;
                    case 1:
                        _serialCtrl.StopBits = StopBits.One;
                        break;
                    case 2:
                        _serialCtrl.StopBits = StopBits.OnePointFive;
                        break;
                    case 3:
                        _serialCtrl.StopBits = StopBits.Two;
                        break;
                }
                _serialCtrl.Handshake = Handshake.None;
                _serialCtrl.DtrEnable = true;
                _serialCtrl.RtsEnable = true;
                _serialCtrl.DataReceived += DataReceived;
                _serialCtrl.Open();
                return 1;
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

        public static void GenerateTestData(int position, float dataBase)
        {
            Random rd = new Random();
            TorqueDevice.TorqueInfo uInfo = new TorqueDevice.TorqueInfo(1, 1, 1, position, 1, dataBase, dataBase + (float)rd.Next(1, 10) / 100);
            TorqueCallback.OnTorqueDataReceived?.Invoke(uInfo);
        }

        private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = (SerialPort)sender;
            int inDataLen = serialPort.BytesToRead;
            if (inDataLen >= 48)
            {
                Byte[] inBytes = new Byte[48];
                serialPort.Read(inBytes, 0, 48);
                serialPort.ReadExisting();
                if (inBytes[0] == 165 && inBytes[1] == 165)
                {
                    float targetValue = (float)((inBytes[17] * 256 + inBytes[16]) / Math.Pow(10, inBytes[18]));
                    float realValue = (float)((inBytes[21] * 256 + inBytes[20]) / Math.Pow(10, inBytes[22]));
                    if (targetValue > 0 && realValue > 0)
                    {
                        TorqueDevice.TorqueInfo uInfo = new TorqueDevice.TorqueInfo(inBytes[7], inBytes[13], inBytes[11], inBytes[12], inBytes[15], targetValue, realValue);
                        if (_lastPackage == null || !_lastPackage.IsSamePackage(uInfo))
                        {
                            _lastPackage = uInfo;
                            TorqueCallback.OnTorqueDataReceived?.Invoke(uInfo);
                        }
                    }
                    Byte[] outBytes = { 0x5A, 0x5A, inBytes[12], inBytes[13], inBytes[7] };
                    serialPort.Write(outBytes, 0, 5);
                }
            }
        }
    }
}
