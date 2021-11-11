using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Hardware.NCanBus
{
    public class CanbusSerialMaster
    {
        public static CanbusMaster CreateRtu(SerialPort canbusPort)
        {
            CanbusMaster master = new CanbusMaster
            {
                ConnectionType = CanbusMaster.ModuleType.Com,
                _comConn = canbusPort
            };
            master.StartUpdate();
            return master;
        }
    }
}
