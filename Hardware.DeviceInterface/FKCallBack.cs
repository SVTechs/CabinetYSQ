using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hardware.DeviceInterface
{
    public class FKCallBack
    {
        public delegate void OnRecvDelegate(int anSEnrollNumber);
        public static OnRecvDelegate OnReceived = null;

        public delegate void OnUserRecognisedDelegate(int userId, int method);
        public static OnUserRecognisedDelegate OnUserRecognised = null;
    }
}
