using System;

namespace Hardware.DeviceInterface
{
    public class CabinetCallback
    {
        /// <summary>
        /// 工具拿取回调
        /// </summary>
        /// <param name="toolNo"></param>
        public delegate void OnToolTakenDelegate(int toolNo);
        public static OnToolTakenDelegate OnToolTaken = null;

        /// <summary>
        /// 工具归还回调
        /// </summary>
        /// <param name="toolNo"></param>
        public delegate void OnToolReturnDelegate(int toolNo);
        public static OnToolReturnDelegate OnToolReturn = null;

        /// <summary>
        /// 智能柜初始化回调
        /// </summary>
        /// <param name="err"></param>
        public delegate void OnInitDoneDelegate(string err);
        public static OnInitDoneDelegate OnInitDone = null;

        /// <summary>
        /// 智能柜关门回调
        /// </summary>
        /// <param name="err"></param>
        public delegate void OnCabinetClosedDelegate();
        public static OnCabinetClosedDelegate OnCabinetClosed = null;

        /// <summary>
        /// 工具状态列表
        /// </summary>
        private static readonly ToolStatus[] ToolStatusList = new ToolStatus[256];
        public class ToolStatus
        {
            /// <summary>
            /// 工具状态 0=离位 1=在位
            /// </summary>
            public int Status;
            /// <summary>
            /// 等待领取
            /// </summary>
            public int WaitStatus;
            /// <summary>
            /// 警告
            /// </summary>
            public int AlertStatus;
            /// <summary>
            /// 警告超时
            /// </summary>
            public DateTime AlertExpire;
            /// <summary>
            /// 维修
            /// </summary>
            public int RepairStatus;
            /// <summary>
            /// 待检
            /// </summary>
            public int CheckStatus;

            public ToolStatus()
            {
                Status = 0;
            }
        }
    }
}
