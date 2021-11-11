using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxZKFPEngXControl;

namespace Hardware.DeviceInterface
{
    public class FpZkDevice
    {
        //匹配引擎
        private static AxZKFPEngX _zkEngine;

        //匹配引擎设置
        private static string _engineVer = "10";
        private static int _securityLevel = 8;

        //指纹缓存
        private static int _cachePointer = -1;

        //重入锁
        private static readonly object CaptureLock = new object();

        //连续识别锁
        private static int _lastUserId = -1;
        private static DateTime _lastUserAccess;

        //匹配分数
        private static int _lastMark = -1;

        /// <summary>
        /// 初始化匹配引擎
        /// </summary>
        /// <returns></returns>
        public static int Init(Form dockingForm)
        {
            //动态添加控件
            _zkEngine = new AxZKFPEngX();
            try
            {
                ((ISupportInitialize)_zkEngine).BeginInit();
                dockingForm.Controls.Add(_zkEngine);
                ((ISupportInitialize)_zkEngine).EndInit();
            }
            catch (Exception)
            {
                return -100;
            }
            ComponentResourceManager resources = new ComponentResourceManager(dockingForm.GetType());
            _zkEngine.OcxState = (AxHost.State)resources.GetObject("ZKFPEngX1.OcxState");
            _zkEngine.Enabled = true;
            _zkEngine.Visible = false;

            //匹配阈值
            _zkEngine.Threshold = 10;
            _zkEngine.OneToOneThreshold = 12;
            //注册按压计数
            _zkEngine.EnrollCount = 3;
            //事件响应
            _zkEngine.OnFeatureInfo += FpZk_OnEnrollProgressChanged;
            _zkEngine.OnImageReceived += FpZk_OnImageReceived;
            _zkEngine.OnEnroll += FpZk_OnEnroll;
            _zkEngine.OnCapture += FpZk_OnCapture;
            _zkEngine.OnFingerTouching += FpZk_OnFingerTouching;
            _zkEngine.OnFingerLeaving += FpZk_OnFingerLeaving;
            //初始化引擎
            if (_zkEngine.InitEngine() == 0)
            {
                //引擎版本
                _zkEngine.FPEngineVersion = _engineVer;
                //指纹库缓存
                _cachePointer = _zkEngine.CreateFPCacheDB();
                return 1;
            }
            return 0;
        }

        public enum EngineType
        {
            V9,
            V10
        }

        public static void SetEngine(EngineType engineType)
        {
            if (engineType == EngineType.V9)
            {
                _engineVer = "9";
            }
            else
            {
                _engineVer = "10";
            }
            if (_zkEngine != null) _zkEngine.FPEngineVersion = _engineVer;
        }

        /// <summary>
        /// 结束匹配引擎
        /// </summary>
        /// <returns></returns>
        public static int EndEngine()
        {
            _zkEngine.EndEngine();
            return 1;
        }

        /// <summary>
        /// 开始指纹扫描
        /// </summary>
        /// <returns></returns>
        public static int StartScan()
        {
            if (_zkEngine != null)
            {
                if (_zkEngine.IsRegister)
                {
                    _zkEngine.CancelEnroll();
                }
                _zkEngine.SetAutoIdentifyPara(false, _cachePointer, 8);
                _zkEngine.BeginCapture();
                return 1;
            }
            return -1;
        }

        public static int EndScan()
        {
            _zkEngine.CancelCapture();
            return 1;
        }

        public static int GetLastMark()
        {
            return _lastMark;
        }

        public static int StartEnroll()
        {
            try
            {
                _zkEngine.CancelEnroll();
                _zkEngine.BeginEnroll();
                return 1;
            }
            catch (Exception)
            {
                // ignored
            }
            return 0;
        }

        public static int AddFeature(byte[] feature, int userId, int regIndex)
        {
            if (regIndex != 0) userId += regIndex * FpConfig.RegDivider;
            try
            {
                return _zkEngine.AddRegTemplateToFPCacheDB(_cachePointer, userId, feature);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static int AddFeature(string feature, int userId, int regIndex)
        {
            if (regIndex != 0) userId += regIndex * FpConfig.RegDivider;
            try
            {
                return _zkEngine.AddRegTemplateStrToFPCacheDB(_cachePointer, userId, feature);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static int RemoveFeature(int userId, int regIndex)
        {
            try
            {
                if (regIndex > 10) return -100;
                if (regIndex != 0) userId += regIndex * FpConfig.RegDivider;
                if (_zkEngine.RemoveRegTemplateFromFPCacheDB(_cachePointer, userId) == 0)
                {
                    return 1;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static void MatchFeature(byte[] feature, out int userId, out int score)
        {
            userId = -1;
            int pScore = -1, pProcNum = -1;
            try
            {
                userId = _zkEngine.IdentificationInFPCacheDB(_cachePointer, feature, ref pScore, ref pProcNum);
            }
            catch (Exception)
            {
                // ignored
            }
            //过滤不良结果
            if (pScore < 5)
            {
                userId = -1;
                score = -1;
                return;
            }
            score = pScore;
        }

        public static void MatchFeature(string feature, out int userId, out int score)
        {
            userId = -1;
            int pScore = -1, pProcNum = -1;
            try
            {
                userId = _zkEngine.IdentificationFromStrInFPCacheDB(_cachePointer, feature, ref pScore, ref pProcNum);
            }
            catch (Exception)
            {
                // ignored
            }
            //过滤不良结果
            if (pScore < 5)
            {
                userId = -1;
                score = -1;
                return;
            }
            score = pScore;
        }

        private static void FpZk_OnImageReceived(object sender, IZKFPEngXEvents_OnImageReceivedEvent e)
        {
            if (FpCallBack.OnImageReceived != null)
            {
                //获取指纹图像
                Bitmap bmp = new Bitmap(_zkEngine.ImageWidth, _zkEngine.ImageHeight);
                Graphics g = Graphics.FromImage(bmp);
                int dc = g.GetHdc().ToInt32();
                _zkEngine.PrintImageAt(dc, 0, 0, bmp.Width, bmp.Height);
                g.Dispose();
                //回调
                FpCallBack.OnImageReceived(bmp);
            }
        }

        private static void FpZk_OnEnroll(object sender, IZKFPEngXEvents_OnEnrollEvent e)
        {
            if (FpCallBack.OnEnroll == null) return;
            byte[] sigGen = (byte[])_zkEngine.GetTemplate();
            if (e.actionResult)
            {
                //_zkEngine.AddRegTemplateToFPCacheDB(_cachePointer, _curRegUser, sigGen);
                FpCallBack.OnEnroll(sigGen);
            }
            else
            {
                FpCallBack.OnEnroll(null);
            }
        }

        private static void FpZk_OnEnrollProgressChanged(object sender, IZKFPEngXEvents_OnFeatureInfoEvent e)
        {
            if (_zkEngine.IsRegister)
            {
                if (_zkEngine.EnrollIndex - 1 > 0)
                {
                    if (FpCallBack.OnEnrollProgressChanged != null)
                    {
                        FpCallBack.OnEnrollProgressChanged(_zkEngine.EnrollIndex - 1, -1);
                    }
                }
            }
        }

        private static void FpZk_OnFingerTouching(object sender, EventArgs e)
        {
            if (FpCallBack.OnTouching != null)
            {
                FpCallBack.OnTouching();
            }
        }

        private static void FpZk_OnFingerLeaving(object sender, EventArgs e)
        {
            if (FpCallBack.OnLeaving != null)
            {
                FpCallBack.OnLeaving();
            }
        }

        private static void FpZk_OnCapture(object sender, IZKFPEngXEvents_OnCaptureEvent e)
        {
            if (FpCallBack.OnUserRecognised == null) return;
            //防止线程重入
            lock (CaptureLock)
            {
                int score = 0, matchCount = 0;
                //匹配指纹
                int userId = _zkEngine.IdentificationInFPCacheDB(_cachePointer, _zkEngine.GetTemplate(), ref score,
                        ref matchCount);
                //去除连续扫描成功的多余请求
                if (userId != -1)
                {
                    if (userId == _lastUserId)
                    {
                        if ((DateTime.Now - _lastUserAccess).TotalSeconds <= 5)
                        {
                            FpCallBack.OnUserRecognised(userId, 1);
                            return;
                        }
                    }
                    _lastUserId = userId;
                    _lastUserAccess = DateTime.Now;
                }
                //拒绝未达标识别结果
                if (score < _securityLevel)
                {
                    _lastMark = -1;
                    FpCallBack.OnUserRecognised(-1, 1);
                    return;
                }
                //识别成功，返回结果
                _lastMark = score;
                _lastUserId = userId % FpConfig.RegDivider;
                FpCallBack.OnUserRecognised(_lastUserId, 1);
            }
        }
    }
}
