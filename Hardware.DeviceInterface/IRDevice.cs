using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hardware.DeviceInterface
{
    public class IRDevice
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
            catch (Exception e)
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
            TorqueCallback.OnIRTorqueDataReceived?.Invoke(uInfo);
        }

        private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = (SerialPort)sender;
            int inDataLen = serialPort.BytesToRead;
            if (inDataLen >= 22)
            {
                Byte[] inBytes = new Byte[48];
                serialPort.Read(inBytes, 0, 48);
                serialPort.ReadExisting();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < inBytes.Length; i++)
                {
                    sb.Append(((char)inBytes[i]));
                }
                string[] rslt = sb.ToString().Split(',');
                try
                {
                    if (rslt.Length >= 4)
                    {
                        string peakTorque = rslt[0];
                        string peakAngle = rslt[1];
                        bool torqueRslt = rslt[2] == "P";
                        bool angleRslt = rslt[3] == "P";
                        string recordId = rslt[4];
                        string iRId = rslt[5];
                        float tmpTorque, tmpAngle;
                        float fPeakTorque = float.TryParse(peakTorque, out tmpTorque) ? tmpTorque : 0;
                        float fPeakAngle = float.TryParse(peakAngle, out tmpAngle) ? tmpAngle : 0;
                        TorqueDevice.TorqueInfo uInfo = new TorqueDevice.TorqueInfo(recordId, iRId, torqueRslt, angleRslt, fPeakTorque, fPeakAngle);
                        //new TorqueDevice.TorqueInfo(inBytes[7], inBytes[13], inBytes[11], inBytes[12], inBytes[15], targetValue, realValue);
                        if (_lastPackage == null || !_lastPackage.IsSamePackage(uInfo))
                        {
                            _lastPackage = uInfo;
                            TorqueCallback.OnIRTorqueDataReceived?.Invoke(uInfo);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
