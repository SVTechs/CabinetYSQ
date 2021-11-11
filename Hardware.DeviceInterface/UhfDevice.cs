using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ReaderB;

// ReSharper disable FunctionNeverReturns

namespace Hardware.DeviceInterface
{
    public class UhfDevice
    {
        /// <summary>
        /// 扫描线程
        /// </summary>
        private static Thread _scanThread;

        /// <summary>
        /// 卡片线程锁
        /// </summary>
        private static readonly object CardLock = new object();


        /// <summary>
        /// 设备列表
        /// </summary>
        private static readonly List<DeviceInfo> DeviceList = new List<DeviceInfo>();
        public class DeviceInfo
        {
            public readonly int ComIndex;
            public readonly Hashtable DeviceCard = new Hashtable();

            public DeviceInfo(int comIndex)
            {
                ComIndex = comIndex;
            }
        }

        public class CardInfo
        {
            public string ToolId;
            public bool IsExist;
            public int LossCount;

            public CardInfo(string toolId)
            {
                ToolId = toolId;
                IsExist = false;
                LossCount = 0;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static int Init(int port)
        {
            int comIndex = 0;
            byte comAddr = 0xFF;
            DeviceInfo di = new DeviceInfo(port);
            DeviceList.Add(di);
            int result = StaticClassReaderB.OpenComPort(port, ref comAddr, 0x05, ref comIndex);
            if (result != 0)
            {
                return -100;
            }
            if (!CheckReader(port))
            {
                return -200;
            }
            if (_scanThread == null)
            {
                _scanThread = new Thread(ScanForCard) { IsBackground = true };
                _scanThread.Start();
            }
            return 0;
        }

        public static CardInfo[] GetCardList(int deviceIndex)
        {
            if (deviceIndex >= DeviceList.Count) return null;
            CardInfo[] cardList;
            lock (CardLock)
            {
                int cardCount = DeviceList[deviceIndex].DeviceCard.Count;
                cardList = new CardInfo[cardCount];
                DeviceList[deviceIndex].DeviceCard.Values.CopyTo(cardList, 0);
            }
            return cardList;
        }

        public static int AddCardToDevice(int deviceIndex, string toolId, string cardId)
        {
            if (DeviceList.Count <= deviceIndex) return 0;
            lock (CardLock)
            {
                DeviceList[deviceIndex].DeviceCard[cardId] = new CardInfo(toolId);
            }
            return 1;
        }

        /// <summary>
        /// 读卡器检查
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static bool CheckReader(int port)
        {
            byte comAddr = 0x00;
            byte[] versionInfo = new byte[2], trType = new byte[2];
            byte readerType = 0;
            byte dMaxFre = 0, dMinFre = 0, powerdBm = 0, scanTime = 0;
            int result = StaticClassReaderB.GetReaderInformation(ref comAddr, versionInfo, ref readerType, trType, ref dMaxFre,
                ref dMinFre, ref powerdBm, ref scanTime, port);
            if (result == 0)
            {
                if (readerType == 0x09)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 卡片扫描及处理
        /// </summary>
        private static void ScanForCard()
        {
            byte comAddr = 0x00;
            byte adrTid = 0, lenTid = 0, tidFlag = 0;
            byte[] epc = new byte[5000];
            while (true)
            {
                int totallen = 0, cardNum = 0, readStart = 0;
                bool[] drawerStatus = null; // CabinetDevice.IsDrawerPhysClosed();
                //判断智能柜是否连接
                if (drawerStatus == null)
                {
                    Thread.Sleep(100);
                    continue;
                }
                for (int i = 0; i < DeviceList.Count; i++)
                {
                    if (true)
                    //if (drawerStatus[i])
                    {
                        //扫描当前设备发现的卡片
                        Hashtable cardFound = new Hashtable();
                        int result = StaticClassReaderB.Inventory_G2(ref comAddr, adrTid, lenTid, tidFlag, epc,
                            ref totallen, ref cardNum, DeviceList[i].ComIndex);
                        if (result == 0xFB || (result >= 1 && result <= 4))
                        {
                            byte[] daw = new byte[totallen];
                            Array.Copy(epc, daw, totallen);
                            string fullString = ByteArrayToHexString(daw);
                            for (int j = 0; j < cardNum; j++)
                            {
                                int groupLen = daw[readStart];
                                string curEpc = fullString.Substring(readStart * 2 + 2, groupLen * 2);
                                //发现卡片事件回调
                                //if (UhfCallback.OnCardFound != null) UhfCallback.OnCardFound(DeviceList[i].ComIndex, curEpc);
                                //保存已发现卡片
                                cardFound[curEpc] = 1;
                                readStart += groupLen + 1;
                            }
                        }
                        bool isFound = false;
                        string cardId = "";
                        lock (CardLock)
                        {
                            //处理未发现卡片(判断物品是否取出)
                            foreach (DictionaryEntry card in DeviceList[i].DeviceCard)
                            {
                                if (cardFound[card.Key] == null)
                                {
                                    CardInfo ci = (CardInfo)DeviceList[i].DeviceCard[card.Key];
                                    if (ci.IsExist)
                                    {
                                        ((CardInfo)DeviceList[i].DeviceCard[card.Key]).LossCount++;
                                        if (((CardInfo)DeviceList[i].DeviceCard[card.Key]).LossCount >= 60)
                                        {
                                            ((CardInfo)DeviceList[i].DeviceCard[card.Key]).IsExist = false;
                                            ((CardInfo)DeviceList[i].DeviceCard[card.Key]).LossCount = 0;
                                            isFound = true;
                                            cardId = card.Key.ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (isFound)
                        {
                            UhfCallback.OnCardTaken(DeviceList[i].ComIndex, cardId);
                        }
                        isFound = false;
                        lock (CardLock)
                        {
                            //处理已发现卡片(判断是否为物品归还)
                            foreach (DictionaryEntry card in cardFound)
                            {
                                if (DeviceList[i].DeviceCard[card.Key] != null)
                                {
                                    CardInfo ci = (CardInfo)DeviceList[i].DeviceCard[card.Key];
                                    if (!ci.IsExist)
                                    {
                                        ((CardInfo)DeviceList[i].DeviceCard[card.Key]).IsExist = true;
                                        ((CardInfo)DeviceList[i].DeviceCard[card.Key]).LossCount = 0;
                                        isFound = true;
                                        cardId = card.Key.ToString();
                                    }
                                }
                            }
                        }
                        if (isFound)
                        {
                            UhfCallback.OnCardReturn(DeviceList[i].ComIndex, cardId);
                        }
                    }
                    else
                    {
                        lock (CardLock)
                        {
                            //抽屉开启时重置计数
                            foreach (DictionaryEntry card in DeviceList[i].DeviceCard)
                            {
                                ((CardInfo)DeviceList[i].DeviceCard[card.Key]).LossCount = 0;
                            }
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        private static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2 + 1);
            foreach (byte b in data)
                sb.Append(b.ToString("X2"));
            return sb.ToString().ToUpper();
        }
    }
}
