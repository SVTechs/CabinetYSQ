using System;

namespace Hardware.NCanBus
{
    public class PackageHelper
    {
        public static byte[] GetQueryPackage()
        {
            return null;
        }

        public static byte[] GenComUploadPackage(int nodeId, bool []status)
        {
            if (status == null || status.Length != 16) return null;
            string binData = "";
            for (int i = 7; i >= 0; i--)
            {
                binData += status[i] ? "1" : "0";
            }
            int decData1 = Convert.ToInt32(binData, 2);
            binData = "";
            for (int i = 15; i >= 8; i--)
            {
                binData += status[i] ? "1" : "0";
            }
            int decData2 = Convert.ToInt32(binData, 2);

            byte[] packageBytes = new byte[11];

            packageBytes[0] = 0x80;
            packageBytes[1] = 0x02;
            packageBytes[2] = (byte)nodeId;

            packageBytes[3] = (byte)decData2;
            packageBytes[4] = (byte)decData1;

            packageBytes[5] = 0x00;
            packageBytes[6] = 0x00;
            packageBytes[7] = 0x00;
            packageBytes[8] = 0x00;
            packageBytes[9] = 0x00;
            packageBytes[10] = 0x00;
            return packageBytes;
        }

        public static byte[] GenTcpUploadPackage(int nodeId, bool[] status)
        {
            if (status == null || status.Length != 16) return null;
            string binData = "";
            for (int i = 7; i >= 0; i--)
            {
                binData += status[i] ? "1" : "0";
            }
            int decData1 = Convert.ToInt32(binData, 2);
            binData = "";
            for (int i = 15; i >= 8; i--)
            {
                binData += status[i] ? "1" : "0";
            }
            int decData2 = Convert.ToInt32(binData, 2);

            byte[] packageBytes = new byte[13];

            packageBytes[0] = 0x05;
            packageBytes[1] = 0x00;
            packageBytes[2] = 0x00;
            packageBytes[3] = 0x02;
            packageBytes[4] = (byte)nodeId;

            packageBytes[5] = (byte)decData2;
            packageBytes[6] = (byte)decData1;

            packageBytes[7] = 0x00;
            packageBytes[8] = 0x00;
            packageBytes[9] = 0x00;
            packageBytes[10] = 0x00;
            packageBytes[11] = 0x00;
            packageBytes[12] = 0x00;
            return packageBytes;
        }
    }
}
