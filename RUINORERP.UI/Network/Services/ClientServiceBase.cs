using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    public abstract class ClientServiceBase
    {
        protected readonly IClientCommunicationService Comm;
        protected readonly ClientCommandDispatcher Dispatcher;

        protected ClientServiceBase(IClientCommunicationService comm,
                                    ClientCommandDispatcher dispatcher)
        {
            Comm = comm ?? throw new ArgumentNullException(nameof(comm));
            Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        /// <summary>
        /// 一句话完成“发命令-等响应-转ApiResponse”
        /// </summary>
        protected Task<ApiResponse<TResp>> SendAsync<TReq, TResp>(
            CommandId cmd,
            TReq data,
            string okMsg = "操作成功",
            int timeoutMs = 30000,
            CancellationToken ct = default)
        {
            return Comm.SendCommandAsync<TReq, TResp>(cmd, data, ct, timeoutMs)
                      .ContinueWith(t =>
                      {
                          if (t.IsFaulted)
                              return ApiResponse<TResp>.Failure($"通信异常：{t.Exception.InnerException?.Message}");
                          var r = t.Result;
                          return r.Success
                              ? ApiResponse<TResp>.CreateSuccess(r.Data, okMsg)
                              : ApiResponse<TResp>.Failure(r.Message, r.Code);
                      }, ct);
        }
    }
}
