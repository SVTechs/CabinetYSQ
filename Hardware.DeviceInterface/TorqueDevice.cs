using System;

namespace Hardware.DeviceInterface
{
    public class TorqueDevice
    {
        public class TorqueInfo
        {
            public int RecordId;
            public int PackageId;
            public int ReceiverId;
            public int ToolId;
            public int BoltId;
            public float TargetValue;
            public float RealValue;
            public string Id;
            public string IRId;
            public bool TorqueRslt;
            public bool AngleRslt;
            public float PeakTorque;
            public float PeakAngle;

            public TorqueInfo(int recordId, int packageId, int receiverId, int toolId, int boltId, float targetValue,
                float realValue)
            {
                RecordId = recordId;
                PackageId = packageId;
                ReceiverId = receiverId;
                ToolId = toolId;
                BoltId = boltId;
                TargetValue = targetValue;
                RealValue = realValue;
            }

            public TorqueInfo(string id, string iRId, bool torqueRslt, bool angleRslt,
                float peakTorque, float peakAngle)
            {
                Id = id;
                IRId = iRId;
                TorqueRslt = torqueRslt;
                AngleRslt = angleRslt;
                PeakTorque = peakTorque;
                PeakAngle = peakAngle;
            }

            public bool IsSamePackage(TorqueInfo ui)
            {
                if (RecordId == ui.RecordId && PackageId == ui.PackageId && ReceiverId == ui.ReceiverId
                    && Math.Abs(RealValue - ui.RealValue) < 0.001)
                {
                    return true;
                }
                return false;
            }
        }

        public static int InitMagtaDevice(int magtaPort)
        {
            return MagtaDevice.Init(magtaPort, 76800, 0, 8, 1);
        }

        public static int InitIRDevice(int IRPort)
        {
            return IRDevice.Init(IRPort, 9600, 0, 8, 1);
        }
    }
}
