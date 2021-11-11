using System;
using System.Text;
using System.Threading;

namespace Hardware.DeviceInterface
{
    public class MifareReader
    {
        private static int _curDevice = -1;
        private static string _cardPwd = "FFFFFFFFFFFF";

        public static int InitDevice(bool reset = false, short port = 1, int baud = 115200)
        {
            if (!reset)
            {
                if (_curDevice >= 0) return 1;
            }
            //连接设备并设置波特率
            _curDevice = MifareReaderWarpper.rf_init(port, baud);
            if (_curDevice < 0) return -100;
            return 1;
        }

        public static int GetDeviceStatus()
        {
            return _curDevice;
        }

        public static int SetMainPassword(string mainPwd)
        {
            if (string.IsNullOrEmpty(mainPwd) || mainPwd.Length != 12) return -100;
            //加载主密码
            _cardPwd = mainPwd;
            int result = MifareReaderWarpper.rf_load_key_hex(_curDevice, 0, 0, _cardPwd);
            if (result != 0) return -200;
            return 1;
        }

        private static int FindCard()
        {
            UInt16 tagtype;
            byte size;
            uint snr;
            MifareReaderWarpper.rf_reset(_curDevice, 3);
            short st = MifareReaderWarpper.rf_request(_curDevice, 1, out tagtype);
            if (st < 0)
            {
                ShutdownDevice();
                return -510;
            }
            else if (st > 0)
            {
                return -501;
            }

            st = MifareReaderWarpper.rf_anticoll(_curDevice, 0, out snr);
            if (st != 0)
            {
                return -502;
            }

            st = MifareReaderWarpper.rf_select(_curDevice, snr, out size);
            if (st != 0)
            {
                return -503;
            }
            return 1;
        }

        private static int ShutdownDevice()
        {
            try
            {
                MifareReaderWarpper.rf_exit(_curDevice);
            }
            catch (Exception)
            {
                // ignored
            }
            _curDevice = -1;
            return 1;
        }

        private static int Authentication(int sectorId, string sectorPwd)
        {
            //加载密码
            int result = MifareReaderWarpper.rf_load_key_hex(_curDevice, 4, sectorId, sectorPwd);
            if (result != 0) return -130;

            //验证该扇区密码
            result = MifareReaderWarpper.rf_authentication(_curDevice, 4, sectorId);
            if (result != 0) return -200;

            return 1;
        }

        public static int EncryptSector(int sectorId, string originPwd, string sectorPwd)
        {
            //寻卡
            int result = FindCard();
            if (result <= 0) return -120;
            //加载密码
            if (string.IsNullOrEmpty(originPwd)) originPwd = "FFFFFFFFFFFF";
            result = Authentication(sectorId, originPwd);
            if (result <= 0) return -130;
            //密码检查
            byte[] buff = new byte[16];
            if (string.IsNullOrEmpty(sectorPwd) || sectorPwd.Length != 12) return -710;
            //编码密码
            byte[] key = Encoding.ASCII.GetBytes(sectorPwd);
            MifareReaderWarpper.a_hex(key, buff, 12);
            //控制位
            buff[6] = 0xFF;
            buff[7] = 0x07;
            buff[8] = 0x80;
            buff[9] = 0x69;

            //设置密码B
            for (int i = 10; i < 16; i++)
            {
                buff[i] = buff[i - 10];
            }
            result = MifareReaderWarpper.rf_write(_curDevice, sectorId * 4 + 3, buff);
            if (result == 0)
            {
                return 1;
            }
            return 0;
        }

        private static int Read(int sectorId, int blockId, out string cardData)
        {
            int i;
            byte[] data = new byte[16];
            byte[] buff = new byte[32];
            cardData = "";

            for (i = 0; i < 16; i++)
                data[i] = 0;
            for (i = 0; i < 32; i++)
                buff[i] = 0;
            int result = MifareReaderWarpper.rf_read(_curDevice, sectorId * 4 + blockId, data);
            if (result == 0)
            {
                MifareReaderWarpper.hex_a(data, buff, 16);
                cardData = Encoding.ASCII.GetString(buff);
                return 1;
            }
            return -601;
        }

        private static int Write(int sectorId, int blockId, string cardData)
        {
            byte[] databuff = new byte[16];
            byte[] buff = Encoding.ASCII.GetBytes(cardData);
            MifareReaderWarpper.a_hex(buff, databuff, 32);
            int result = MifareReaderWarpper.rf_write(_curDevice, sectorId * 4 + blockId, databuff);
            if (result == 0)
            {
                return 1;
            }
            return 0;
        }

        public static int WriteCard(int sectorId, int blockId, string hexData, string sectorPwd)
        {
            try
            {
                //不可大于32个字符
                if (string.IsNullOrEmpty(hexData) || hexData.Length > 32) return -100;
                //如果字符串有非法字符
                foreach (var curChar in hexData)
                {
                    if (curChar > 127) return -101;
                }

                int result = -1;
                bool reset = false;
                for (int i = 0; i < 5; i++)
                {
                    //设备初始化
                    if (_curDevice < 0) InitDevice(reset);
                    if (_curDevice < 0)
                    {
                        reset = true;
                        result = -110;
                        Thread.Sleep(100);
                        continue;
                    }
                    //寻卡
                    result = FindCard();
                    if (result <= 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    //加载密码
                    result = Authentication(sectorId, sectorPwd);
                    if (result <= 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    //写卡
                    result = Write(sectorId, blockId, hexData);
                    if (result <= 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    result = 1;
                    break;
                }
                return result;
            }
            catch (URFException urfEx)
            {
                MifareReaderWarpper.rf_exit(_curDevice);
                _curDevice = -1;
                throw new Exception(ErrorManager.CheckError(urfEx.ErrorCode));
            }
            catch (ArgumentException ex)
            {
                MifareReaderWarpper.rf_exit(_curDevice);
                _curDevice = -1;
                throw new Exception(ex.Message);
            }
            finally
            {
                MifareReaderWarpper.rf_halt((short)_curDevice);
            }
        }

        /// <summary>
        /// 从M1卡中读取指定长度的字节
        /// </summary>
        public static int ReadCard(int sectorId, int blockId, string sectorPwd, out string cardData)
        {
            cardData = null;
            try
            {
                int result = -1;
                bool reset = false;
                for (int i = 0; i < 5; i++)
                {
                    //设备初始化
                    if (_curDevice < 0) InitDevice(reset);
                    if (_curDevice < 0)
                    {
                        reset = true;
                        result = -110;
                        Thread.Sleep(100);
                        continue;
                    }
                    //寻卡
                    result = FindCard();
                    if (result <= 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    //加载密码
                    result = Authentication(sectorId, sectorPwd);
                    if (result <= 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    //读卡
                    result = Read(sectorId, blockId, out cardData);
                    if (result <= 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    result = 1;
                    break;
                }
                return result;
            }
            catch (URFException urfEx)
            {
                MifareReaderWarpper.rf_exit(_curDevice);
                _curDevice = -1;
                throw new Exception(ErrorManager.CheckError(urfEx.ErrorCode));
            }
            catch (ArgumentException ex)
            {
                MifareReaderWarpper.rf_exit(_curDevice);
                _curDevice = -1;
                throw new Exception(ex.Message);
            }
            finally
            {
                MifareReaderWarpper.rf_halt((short)_curDevice);
            }
        }
    }
}
