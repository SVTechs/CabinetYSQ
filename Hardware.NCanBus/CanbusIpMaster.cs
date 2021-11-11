using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Utilities.Net;

namespace Hardware.NCanBus
{
    public class CanbusIpMaster
    {
        public static CanbusMaster CreateIp(TcpClient canbusClient)
        {
            CanbusMaster master = new CanbusMaster
            {
                ConnectionType = CanbusMaster.ModuleType.Net,
                _netConn = canbusClient
            };
            master.StartUpdate();
            return master;
        }
    }
}
