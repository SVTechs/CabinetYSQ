using System;
using System.Collections;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Hardware.NCanBus
{
    public class CanbusMaster
    {
        public enum ModuleType
        {
            Net,
            Com
        };

        public ModuleType ConnectionType;
        public TcpClient _netConn = null;
        public SerialPort _comConn = null;

        public bool Connected
        {
            get
            {
                if (ConnectionType == ModuleType.Net)
                {
                    if (_netConn == null) return false;
                    return _netConn.Connected;
                }
                else if (ConnectionType == ModuleType.Com)
                {
                    if (_comConn == null) return false;
                    return _comConn.IsOpen;
                }
                return false;
            }
        }

        private static object _syncLock = new object();

        private static readonly int DoBase = 200;

        private readonly Hashtable _statusBuffer = new Hashtable(), _doBuffer = new Hashtable();

        public void StartUpdate()
        {
            Thread bufferThread = new Thread(UpdateBuffer);
            bufferThread.IsBackground = true;
            bufferThread.Start();
        }

        private string PatchBin(string origin)
        {
            while (origin.Length < 8) origin = "0" + origin;
            StringBuilder strBuild = new StringBuilder();
            for (int i = origin.Length - 1; i >= 0; i--)
            {
                strBuild.Append(origin[i]);
            }
            origin =  strBuild.ToString();
            return origin;
        }

        private void UpdateBuffer()
        {
            while (true)
            {
                try
                {
                    if (ConnectionType == ModuleType.Net)
                    {
                        NetworkStream ns = _netConn.GetStream();
                        byte[] infoBuffer = new byte[13];
                        ns.Read(infoBuffer, 0, 13);
                        int nodeId = infoBuffer[6];
                        if (nodeId >= 0x40) continue;
                        //获取DI状态
                        string status = PatchBin(Convert.ToString(infoBuffer[9], 2)) + PatchBin(Convert.ToString(infoBuffer[10], 2));
                        bool[] tempBuffer = new bool[16];
                        for (int i = 0; i < 16; i++)
                        {
                            tempBuffer[i] = status[i] == '1';
                        }
                        _statusBuffer[nodeId] = tempBuffer;
                        //获取DO状态
                        status = PatchBin(Convert.ToString(infoBuffer[11], 2)) + PatchBin(Convert.ToString(infoBuffer[12], 2));
                        tempBuffer = new bool[16];
                        for (int i = 0; i < 16; i++)
                        {
                            tempBuffer[i] = status[i] == '1';
                        }
                        _doBuffer[nodeId] = tempBuffer;
                    }
                    else
                    {
                        byte[] infoBuffer = new byte[11];
                        _comConn.Read(infoBuffer, 0, 11);
                        int nodeId = infoBuffer[0];
                        if (nodeId >= 0x40) continue;
                        //获取DI状态
                        string status = Convert.ToString(infoBuffer[7], 2) + Convert.ToString(infoBuffer[8], 2);
                        bool[] tempBuffer = new bool[16];
                        for (int i = 0; i < 16; i++)
                        {
                            tempBuffer[i] = status[i] == '1';
                        }
                        _statusBuffer[nodeId] = tempBuffer;
                        //获取DO状态
                        status = Convert.ToString(infoBuffer[9], 2) + Convert.ToString(infoBuffer[10], 2);
                        tempBuffer = new bool[16];
                        for (int i = 0; i < 16; i++)
                        {
                            tempBuffer[i] = status[i] == '1';
                        }
                        _doBuffer[nodeId] = tempBuffer;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public Hashtable GetStatusBuffer()
        {
            return _statusBuffer;
        }

        private bool[] PatchDoStatus(int nodeId, int startIndex, bool[] status)
        {
            bool[] originStatus = (bool[])_doBuffer[nodeId];
            for (int i = startIndex; i < status.Length + startIndex; i++)
            {
                originStatus[i] = status[i - startIndex];
            }
            return originStatus;
        }

        private bool[] PatchDoStatus(int nodeId, int startIndex, bool status)
        {
            bool[] originStatus = (bool[])_doBuffer[nodeId];
            originStatus[startIndex] = status;
            return originStatus;
        }

        public void WriteMultipleCoils(int nodeId, int startIndex, bool[] status)
        {
            lock (_syncLock)
            {
                try
                {
                    if (status.Length != 16) status = PatchDoStatus(nodeId, startIndex, status);
                    if (ConnectionType == ModuleType.Net)
                    {
                        byte[] dataPackage = PackageHelper.GenTcpUploadPackage(nodeId, status);
                        NetworkStream ns = _netConn.GetStream();
                        ns.Write(dataPackage, 0, dataPackage.Length);
                    }
                    else
                    {
                        byte[] dataPackage = PackageHelper.GenComUploadPackage(nodeId, status);
                        _comConn.Write(dataPackage, 0, dataPackage.Length);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }


        public void WriteSingleCoil(int nodeId, int startIndex, bool status)
        {
            lock (_syncLock)
            {
                try
                {
                    bool[] newStatus = PatchDoStatus(nodeId, startIndex, status);
                    if (ConnectionType == ModuleType.Net)
                    {
                        byte[] dataPackage = PackageHelper.GenTcpUploadPackage(nodeId, newStatus);
                        NetworkStream ns = _netConn.GetStream();
                        ns.Write(dataPackage, 0, dataPackage.Length);
                    }
                    else
                    {
                        byte[] dataPackage = PackageHelper.GenComUploadPackage(nodeId, newStatus);
                        _comConn.Write(dataPackage, 0, dataPackage.Length);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void InitRtu(int nodeId)
        {
            lock (_syncLock)
            {
                try
                {
                    byte[] packageBytes = new byte[13];
                    packageBytes[0] = 0x05;
                    packageBytes[1] = 0x00;
                    packageBytes[2] = 0x00;
                    packageBytes[3] = 0x00;
                    packageBytes[4] = 0x00;
                    packageBytes[5] = 0x01;
                    packageBytes[6] = 0x00;
                    packageBytes[7] = 0x00;
                    packageBytes[8] = 0x00;
                    packageBytes[9] = 0x00;
                    packageBytes[10] = 0x00;
                    packageBytes[11] = 0x00;
                    packageBytes[12] = 0x00;
                    //bool[] newStatus = PatchDoStatus(nodeId, startIndex, status);
                    if (ConnectionType == ModuleType.Net)
                    {
                        NetworkStream ns = _netConn.GetStream();
                        ns.Write(packageBytes, 0, packageBytes.Length);
                    }
                    else
                    {
                        _comConn.Write(packageBytes, 0, packageBytes.Length);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
