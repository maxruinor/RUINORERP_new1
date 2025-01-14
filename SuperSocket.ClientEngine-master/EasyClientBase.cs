﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuperSocket.ProtoBase;

namespace SuperSocket.ClientEngine
{
    public abstract class EasyClientBase
    {
        private IClientSession m_Session;
        private TaskCompletionSource<bool> m_ConnectTaskSource;
        private TaskCompletionSource<bool> m_CloseTaskSource;
        private bool m_Connected = false;

        protected IPipelineProcessor PipeLineProcessor { get; set; }

#if !NETFX_CORE || NETSTANDARD
        public SecurityOption Security { get; set; }
#endif

#if !SILVERLIGHT

        private EndPoint m_EndPointToBind;
        private EndPoint m_LocalEndPoint;

        public EndPoint LocalEndPoint
        {
            get
            {
                if (m_LocalEndPoint != null)
                    return m_LocalEndPoint;

                return m_EndPointToBind;
            }
            set
            {
                m_EndPointToBind = value;
            }
        }
#endif

        public bool NoDelay { get; set; }






        public int ReceiveBufferSize { get; set; }

        public IProxyConnector Proxy { get; set; }

        public Socket Socket
        {
            get
            {
                var session = m_Session;

                if (session == null)
                    return null;

                return session.Socket;
            }
        }

        public EasyClientBase()
        {

        }

        public bool IsConnected { get { return m_Connected; } }



#if AWAIT
        public async Task<bool> ConnectAsync(EndPoint remoteEndPoint)
        {
            if (PipeLineProcessor == null)
                throw new Exception("This client has not been initialized.");

            var connectTaskSrc = InitConnect(remoteEndPoint);
            return await connectTaskSrc.Task.ConfigureAwait(false);
        }
#else
        public Task<bool> ConnectAsync(EndPoint remoteEndPoint)
        {
            if (PipeLineProcessor == null)
                throw new Exception("This client has not been initialized.");

            var connectTaskSrc = InitConnect(remoteEndPoint);
            return connectTaskSrc.Task;
        }
#endif



        #region  by watson add
        public async Task<bool> ConnectAsync(EndPoint remoteEndPoint, CancellationToken cancellationToken)
        {
            if (PipeLineProcessor == null)
                throw new Exception("This client has not been initialized.");

            // 确保在连接过程中能够响应取消请求
            using (cancellationToken.Register(() => PipeLineProcessor.Reset()))
            {
                var connectTaskSrc = InitConnect(remoteEndPoint);
                try
                {
                    // 等待连接任务完成或取消
                    return await connectTaskSrc.Task.ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // 处理取消情况
                    return false;
                }
                catch (Exception ex)
                {
                    // 处理其他异常情况
                    // 可以根据需要记录日志或处理异常
                    return false;
                }
            }
        }

        // 修改InitConnect方法以接受CancellationToken参数
        private TaskCompletionSource<bool> InitConnect(EndPoint remoteEndPoint, CancellationToken cancellationToken)
        {
            var connectTaskSrc = new TaskCompletionSource<bool>();

            // 模拟异步连接操作，这里应该替换为实际的连接逻辑
            Task.Run(async () =>
            {
                try
                {
                    // 在连接过程中检查取消请求
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        // 模拟连接尝试
                        await Task.Delay(1000); // 假设这里有一个延迟来模拟连接尝试

                        // 假设连接成功
                        connectTaskSrc.SetResult(true);
                        break;
                    }
                }
                catch (OperationCanceledException)
                {
                    connectTaskSrc.SetCanceled();
                }
                catch (Exception ex)
                {
                    connectTaskSrc.SetException(ex);
                }
            }, cancellationToken); // 将CancellationToken传递给Task.Run

            return connectTaskSrc;
        }

        #endregion


        private TaskCompletionSource<bool> InitConnect(EndPoint remoteEndPoint)
        {
            var session = GetUnderlyingSession();

#if !SILVERLIGHT
            var localEndPoint = m_EndPointToBind;

            if (localEndPoint != null)
            {
                session.LocalEndPoint = m_EndPointToBind;
            }
#endif

            session.NoDelay = NoDelay;

            if (Proxy != null)
                session.Proxy = Proxy;

            session.Connected += new EventHandler(OnSessionConnected);
            session.Error += new EventHandler<ErrorEventArgs>(OnSessionError);
            session.Closed += new EventHandler(OnSessionClosed);
            session.DataReceived += new EventHandler<DataEventArgs>(OnSessionDataReceived);

            if (ReceiveBufferSize > 0)
                session.ReceiveBufferSize = ReceiveBufferSize;

            m_Session = session;

            var taskSrc = m_ConnectTaskSource = new TaskCompletionSource<bool>();

            session.Connect(remoteEndPoint);

            return taskSrc;
        }


        private TcpClientSession GetUnderlyingSession()
        {
#if NETFX_CORE && !NETSTANDARD
            return new AsyncTcpSession();
#else
            var security = Security;

            if (security == null)
            {
                return new AsyncTcpSession();
            }

#if SILVERLIGHT
            // no SSL/TLS enabled
            if (!security.EnabledSslProtocols)
            {
                return new AsyncTcpSession();
            }

            return new SslStreamTcpSession();
#else
            // no SSL/TLS enabled
            if (security.EnabledSslProtocols == System.Security.Authentication.SslProtocols.None)
            {
                return new AsyncTcpSession();
            }

            return new SslStreamTcpSession()
            {
                Security = security
            };
#endif
#endif
        }


        public void Send(byte[] data)
        {
            if (m_Connected)
            {
                Send(new ArraySegment<byte>(data, 0, data.Length));
            }

        }

        public void Send(ArraySegment<byte> segment)
        {
            var session = m_Session;

            if (!m_Connected || session == null)
                throw new Exception("The socket is not connected.");

            session.Send(segment);
        }

        public void Send(List<ArraySegment<byte>> segments)
        {
            var session = m_Session;

            if (!m_Connected || session == null)
                throw new Exception("The socket is not connected.");

            session.Send(segments);
        }

#if AWAIT
        public async Task<bool> Close()
        {
            var session = m_Session;

            if (session != null && m_Connected)
            {
                var closeTaskSrc = new TaskCompletionSource<bool>();
                m_CloseTaskSource = closeTaskSrc;
                session.Close();
                return await closeTaskSrc.Task.ConfigureAwait(false);
            }

            return await Task.FromResult(false);
        }
#else
        public Task<bool> Close()
        {
            var session = m_Session;
            
            if(session != null && m_Connected)
            {
                var closeTaskSrc = new TaskCompletionSource<bool>();
                m_CloseTaskSource = closeTaskSrc;
                session.Close();
                return closeTaskSrc.Task;
            }

            return new Task<bool>(() => false);
        }
#endif

        // ClientSession 这个类中设置了缓存大小 public const int DefaultReceiveBufferSize = 4096; 
        void OnSessionDataReceived(object sender, DataEventArgs e)
        {
            ProcessResult result;

            try
            {
                result = PipeLineProcessor.Process(new ArraySegment<byte>(e.Data, e.Offset, e.Length));
            }
            catch (Exception exc)
            {
                OnError(exc);
                m_Session.Close();
                return;
            }

            if (result.State == ProcessState.Error)
            {
                m_Session.Close();
                return;
            }
            else if (result.State == ProcessState.Cached)
            {
                // allocate new receive buffer if the previous one was cached
                var session = m_Session;

                if (session != null)
                {
                    var bufferSetter = session as IBufferSetter;

                    if (bufferSetter != null)
                    {
                        bufferSetter.SetBuffer(new ArraySegment<byte>(new byte[session.ReceiveBufferSize]));
                    }
                }
            }

            if (result.Packages != null && result.Packages.Count > 0)
            {
                foreach (var item in result.Packages)
                {
                    HandlePackage(item);
                }
            }
        }

        void OnSessionError(object sender, ErrorEventArgs e)
        {
            if (!m_Connected)
            {
                FinishConnectTask(false);
            }

            OnError(e);
        }

        bool FinishConnectTask(bool result)
        {
            var connectTaskSource = m_ConnectTaskSource;

            if (connectTaskSource == null)
                return false;

            if (Interlocked.CompareExchange(ref m_ConnectTaskSource, null, connectTaskSource) == connectTaskSource)
            {
                connectTaskSource.SetResult(result);
                return true;
            }

            return false;
        }

        private void OnError(Exception e)
        {
            OnError(new ErrorEventArgs(e));
        }

        private void OnError(ErrorEventArgs args)
        {
            var handler = Error;

            if (handler != null)
                handler(this, args);
        }

        public event EventHandler<ErrorEventArgs> Error;

        void OnSessionClosed(object sender, EventArgs e)
        {
            m_Connected = false;

#if !SILVERLIGHT
            m_LocalEndPoint = null;
#endif

            var pipelineProcessor = PipeLineProcessor;

            if (pipelineProcessor != null)
                pipelineProcessor.Reset();

            var handler = Closed;

            if (handler != null)
                handler(this, EventArgs.Empty);


            var closeTaskSrc = m_CloseTaskSource;

            if (closeTaskSrc != null)
            {
                if (Interlocked.CompareExchange(ref m_CloseTaskSource, null, closeTaskSrc) == closeTaskSrc)
                {
                    closeTaskSrc.SetResult(true);
                }
            }
        }

        public event EventHandler Closed;

        void OnSessionConnected(object sender, EventArgs e)
        {
            m_Connected = true;

#if !SILVERLIGHT
            TcpClientSession session = sender as TcpClientSession;
            if (session != null)
            {
                m_LocalEndPoint = session.LocalEndPoint;
            }
#endif

            FinishConnectTask(true);

            var handler = Connected;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler Connected;

        protected abstract void HandlePackage(IPackageInfo package);
    }
}
