using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxAXIMAGELib;
using AXIMAGELib;
using NLog;
using Utilities.Net;
using zkemkeeper;

namespace Hardware.DeviceInterface
{
    public class FKIFace
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //=============== VerifyMode of GeneralLogData ===============//
        public enum enumGLogVerifyMode
        {
            LOG_FPVERIFY = 1,                   // Fp Verify
            LOG_PASSVERIFY = 2,                 // Pass Verify
            LOG_CARDVERIFY = 3,                 // Card Verify
            LOG_FPPASS_VERIFY = 4,              // Pass+Fp Verify
            LOG_FPCARD_VERIFY = 5,              // Card+Fp Verify
            LOG_PASSFP_VERIFY = 6,              // Pass+Fp Verify
            LOG_CARDFP_VERIFY = 7,              // Card+Fp Verify
            LOG_JOB_NO_VERIFY = 8,              // Job number Verify
            LOG_CARDPASS_VERIFY = 9,            // Card+Pass Verify

            LOG_FACEVERIFY = 20,                // Face Verify
            LOG_FACECARDVERIFY = 21,            // Face+Card Verify
            LOG_FACEPASSVERIFY = 22,            // Face+Pass Verify
            LOG_CARDFACEVERIFY = 23,            // Card+Face Verify
            LOG_PASSFACEVERIFY = 24,            // Pass+Face Verify
        };

        //=============== IOMode of GeneralLogData ===============//
        public enum enumGLogIOMode
        {
            LOG_IOMODE_IO = 0,
            LOG_IOMODE_IN1 = 1,
            LOG_IOMODE_OUT1 = 2,
            LOG_IOMODE_IN2 = 3,
            LOG_IOMODE_OUT2 = 4,
            LOG_IOMODE_IN3 = 5,
            LOG_IOMODE_OUT3 = 6,
            LOG_IOMODE_IN4 = 7,
            LOG_IOMODE_OUT4 = 8,
        };
        public enum enumGLogDoorMode
        {

            LOG_CLOSE_DOOR = 1,                // Door Close
            LOG_OPEN_HAND = 2,                 // Hand Open
            LOG_PROG_OPEN = 3,                 // Open by PC
            LOG_PROG_CLOSE = 4,                // Close by PC
            LOG_OPEN_IREGAL = 5,               // Illegal Open
            LOG_CLOSE_IREGAL = 6,              // Illegal Close
            LOG_OPEN_COVER = 7,                // Cover Open
            LOG_CLOSE_COVER = 8,               // Cover Close
            LOG_OPEN_DOOR = 9,                 // Door Open
            LOG_OPEN_DOOR_THREAT = 10,         // Door Open
            LOG_FORCE_OPEN_DOOR = 11,                 // Door Open
            LOG_FORCE_CLOSE_DOOR_ = 12,         // Door Open
        }

        public enum enumVerifyKind
        {
            VK_NONE = 0,
            VK_FP = 1,
            VK_PASS = 2,
            VK_CARD = 3,
            VK_FACE = 4,
            VK_FINGERVEIN = 5,
            VK_IRIS = 6,
            VK_PALMVEIN = 7,
            VK_VOICE = 8,
        }

        public const int RUN_SUCCESS = 1;
        public const int RUNERR_NOSUPPORT = 0;
        public const int RUNERR_UNKNOWNERROR = -1;
        public const int RUNERR_NO_OPEN_COMM = -2;
        public const int RUNERR_WRITE_FAIL = -3;
        public const int RUNERR_READ_FAIL = -4;
        public const int RUNERR_INVALID_PARAM = -5;
        public const int RUNERR_NON_CARRYOUT = -6;
        public const int RUNERR_DATAARRAY_END = -7;
        public const int RUNERR_DATAARRAY_NONE = -8;
        public const int RUNERR_MEMORY = -9;
        public const int RUNERR_MIS_PASSWORD = -10;
        public const int RUNERR_MEMORYOVER = -11;
        public const int RUNERR_DATADOUBLE = -12;
        public const int RUNERR_MANAGEROVER = -14;
        public const int RUNERR_FPDATAVERSION = -15;

        public const string gstrNoDevice = "No Device";

        //==========================================//
        private bool mbOpenFlag;
        private int fnCount = 0;
    }
}


