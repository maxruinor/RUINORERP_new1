using System;
using System.Net;
using System.Threading;
using SuperSocket.ClientEngine;

namespace RUINORERP.UI.SuperSocketClient
{

    /// <summary>
    /// 因为没有处理粘包 分解这些暂时不使用
    /// </summary>
    public class SocketClientHelper
    {
        public event Action Connected = () => { };
        public event Action Closed = () => { };
        public event Action<byte[], int, int> DataReceived = (data, offset, length) => { };
        public event Action<Exception> Error = (e) => { };

        private readonly AsyncTcpSession _asyncTcpSession;

        public SocketClientHelper()
        {
            _asyncTcpSession = new AsyncTcpSession();
            _asyncTcpSession.Connected += OnTcpSessionConnected;
            _asyncTcpSession.Closed += OnTcpSessionClosed;
            _asyncTcpSession.DataReceived += OnTcpSessionDataReceived;
            _asyncTcpSession.Error += OnTcpSessionError;
        }

        public void Start(string ip, int port)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                _asyncTcpSession.Close();
                while (_asyncTcpSession.IsConnected)
                {
                    Thread.Sleep(1);
                }
                _asyncTcpSession.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            });
        }

        public void Stop()
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                if (_asyncTcpSession.IsConnected)
                {
                    _asyncTcpSession.Close();
                }
            });
        }

        public void Send(byte[] data)
        {
            if (data == null)
            {
                return;
            }
            if (data.Length == 0)
            {
                return;
            }
            if (_asyncTcpSession.IsConnected)
            {
                _asyncTcpSession.Send(data, 0, data.Length);
                MainForm.Instance.PrintInfoLog($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Send data to socket server");
                Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Send data to socket server");
            }
        }

        #region TcpSession事件

        private void OnTcpSessionConnected(object sender, EventArgs e)
        {
            Connected?.Invoke();
        }

        private void OnTcpSessionClosed(object sender, EventArgs e)
        {
            Closed?.Invoke();
        }

        private void OnTcpSessionDataReceived(object sender, DataEventArgs e)
        {

            DataReceived?.Invoke(e.Data, e.Offset, e.Length);
        }

        private void OnTcpSessionError(object sender, ErrorEventArgs e)
        {
            Error?.Invoke(e.Exception);

        }

        #endregion
    }
}
