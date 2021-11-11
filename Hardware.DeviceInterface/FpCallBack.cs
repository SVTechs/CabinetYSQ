using System.Drawing;

namespace Hardware.DeviceInterface
{
    public class FpCallBack
    {
        /// <summary>
        /// 采集指纹图像回调
        /// </summary>
        /// <param name="zkImage"></param>
        public delegate void OnImageRecvDelegate(Bitmap zkImage);
        public static OnImageRecvDelegate OnImageReceived = null;

        /// <summary>
        /// 指纹注册回调
        /// </summary>
        /// <param name="zkFeature"></param>
        public delegate void OnEnrollDelegate(byte []zkFeature);
        public static OnEnrollDelegate OnEnroll = null;

        /// <summary>
        /// IFace指纹注册回调
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="templateLength"></param>
        public delegate void OnManagedEnrollDelegate(int userId, int templateLength);
        public static OnManagedEnrollDelegate OnManagedEnroll = null;

        /// <summary>
        /// 指纹注册进度变化回调
        /// </summary>
        /// <param name="pressLeft"></param>
        /// <param name="score"></param>
        public delegate void OnEnrollProgressChangedDelegate(int pressLeft, int score);
        public static OnEnrollProgressChangedDelegate OnEnrollProgressChanged = null;

        /// <summary>
        /// 指纹识别回调
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="method"></param>
        public delegate void OnUserRecognisedDelegate(int userId, int method);
        public static OnUserRecognisedDelegate OnUserRecognised = null;

        /// <summary>
        /// 指纹仪触摸回调
        /// </summary>
        public delegate void OnTouchingDelegate();
        public static OnTouchingDelegate OnTouching = null;

        /// <summary>
        /// 指纹仪手指收回回调
        /// </summary>
        public delegate void OnLeavingDelegate();
        public static OnLeavingDelegate OnLeaving = null;
    }
}
