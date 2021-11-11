﻿using System;

namespace Hardware.NModbus4.Device
{
    internal class TcpConnectionEventArgs : EventArgs
    {
        public TcpConnectionEventArgs(string endPoint)
        {
            if (endPoint == null)
                throw new ArgumentNullException("endPoint");
            if (endPoint == string.Empty)
                throw new ArgumentException(Resources.EmptyEndPoint);

            EndPoint = endPoint;
        }

        public string EndPoint { get; set; }
    }
}
