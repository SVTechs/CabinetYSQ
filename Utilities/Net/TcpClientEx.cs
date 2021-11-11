using System;
using System.Net.Sockets;
using System.Threading;

namespace Utilities.Net
{
    public class TcpClientEx
    {
        private readonly string _hostname;
        private readonly int _port;
        private readonly int _timeoutMilliseconds;
        private TcpClient _connection;
        private bool _connected;
        private Exception _exception;

        public TcpClientEx(string hostname, int port, int timeoutMilliseconds)
        {
            _hostname = hostname;
            _port = port;
            _timeoutMilliseconds = timeoutMilliseconds;
        }

        public bool IsConnected()
        {
            if (_connection == null) return false;
            return _connection.Connected;
        }

        public TcpClient Connect()
        {
            // kick off the thread that tries to connect
            _connected = false;
            _exception = null;
            Thread thread = new Thread(BeginConnect);
            thread.IsBackground = true; // 作为后台线程处理
            // 不会占用机器太长的时间
            thread.Start();

            // 等待如下的时间
            thread.Join(_timeoutMilliseconds);

            if (_connected)
            {
                // 如果成功就返回TcpClient对象
                thread.Abort();
                return _connection;
            }
            if (_exception != null)
            {
                // 如果失败就抛出错误
                thread.Abort();
                throw _exception;
            }
            else
            {
                // 同样地抛出错误
                thread.Abort();
                string message = string.Format("TcpClient connection to {0}:{1} timed out",
                    _hostname, _port);
                throw new TimeoutException(message);
            }
        }

        public void Close()
        {
            _connection.Close();
        }

        protected void BeginConnect()
        {
            try
            {
                _connection = new TcpClient(_hostname, _port);
                // 标记成功，返回调用者
                _connected = true;
            }
            catch (Exception ex)
            {
                // 标记失败
                _exception = ex;
            }
        }
    }
}
