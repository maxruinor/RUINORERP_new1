using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::RUINORERP.Common.CustomAttribute;
using global::RUINORERP.Global;
using System.Collections.Concurrent;
using System.Threading;

namespace RUINORERP.UI.ClientCmdService
{


    namespace RUINORERP.UI.ClientCmdService
    {
        /// <summary>
        /// 管理服务器返回结果的事件管理器
        /// </summary>
        [NoWantIOC]
        public class ServerResponseEventManager : IDisposable, IExcludeFromRegistration
        {
            private static readonly Lazy<ServerResponseEventManager> _instance = new Lazy<ServerResponseEventManager>(() => new ServerResponseEventManager(), LazyThreadSafetyMode.ExecutionAndPublication);

            // 用于存储不同事件的处理程序
            private ConcurrentDictionary<string, ServerResponseHandler> _responseHandlers = new ConcurrentDictionary<string, ServerResponseHandler>();

            private ServerResponseEventManager()
            {
            }

            public static ServerResponseEventManager Instance
            {
                get
                {
                    return _instance.Value;
                }
            }

            public ConcurrentDictionary<string, ServerResponseHandler> ResponseHandlers
            {
                get { return _responseHandlers; }
            }

            /// <summary>
            /// 添加事件处理程序
            /// </summary>
            /// <param name="requestId">请求的唯一标识符</param>
            /// <param name="handler">事件处理程序</param>
            public void AddResponseHandler(string requestId, ServerResponseHandler handler)
            {
                _responseHandlers.AddOrUpdate(
                    requestId,
                    handler,
                    (key, existingHandler) => Delegate.Combine(existingHandler, handler) as ServerResponseHandler
                );
            }

            /// <summary>
            /// 移除事件处理程序
            /// </summary>
            /// <param name="requestId">请求的唯一标识符</param>
            /// <param name="handler">事件处理程序</param>
            public void RemoveResponseHandler(string requestId, ServerResponseHandler handler)
            {
                if (_responseHandlers.TryGetValue(requestId, out ServerResponseHandler existingHandler))
                {
                    ServerResponseHandler newHandler = (ServerResponseHandler)Delegate.Remove(existingHandler, handler);

                    if (newHandler == null)
                    {
                        _responseHandlers.TryRemove(requestId, out _);
                    }
                    else
                    {
                        _responseHandlers.TryUpdate(requestId, newHandler, existingHandler);
                    }
                }
            }

            /// <summary>
            /// 触发事件
            /// </summary>
            /// <param name="requestId">请求的唯一标识符</param>
            /// <param name="response">服务器返回的结果</param>
            public void RaiseResponseEvent(string requestId, ServerResponseEventArgs response)
            {
                if (_responseHandlers.TryGetValue(requestId, out ServerResponseHandler handler))
                {
                    handler(null, response);
                }
            }

            public void Dispose()
            {
                // 清理资源
                _responseHandlers.Clear();
            }
        }

        /// <summary>
        /// 服务器返回结果的事件处理程序委托
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public delegate void ServerResponseHandler(object sender, ServerResponseEventArgs e);

        /// <summary>
        /// 服务器返回结果的事件参数
        /// </summary>
        public class ServerResponseEventArgs : EventArgs
        {
            public string RequestId { get; set; } // 请求的唯一标识符
            public bool IsSuccess { get; set; } // 是否成功
            public string Message { get; set; } // 返回的消息
            public byte[] Data { get; set; } // 返回的数据
        }
    }
}
