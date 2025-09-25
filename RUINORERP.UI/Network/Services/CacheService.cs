using RUINORERP.PacketSpec.Commands.Cache;
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
    public class CacheService : ClientServiceBase
    {
        public CacheService(IClientCommunicationService comm,
                                ClientCommandDispatcher dispatcher)
            : base(comm, dispatcher) { }


        public Task<ApiResponse<object>> RequestCacheAsync(string key, CancellationToken ct = default)
            => SendAsync<object, object>(
                CacheCommands.CacheRequest,
                new { CacheKey = key, Timestamp = DateTime.UtcNow },
                "缓存已返回",30000,
                 ct);

        public Task<ApiResponse<bool>> SyncCacheAsync(string key, object data, CancellationToken ct = default)
            => SendAsync<object, bool>(
                CacheCommands.CacheSync,
                new { CacheKey = key, Data = data, Timestamp = DateTime.UtcNow },
                "同步成功", 30000,
                 ct);

        public Task<ApiResponse<bool>> InvalidateCacheAsync(string key, CancellationToken ct = default)
            => SendAsync<object, bool>(
                CacheCommands.CacheInvalidate,
                new { CacheKey = key, Timestamp = DateTime.UtcNow },
                "缓存已失效", 30000,
                 ct);

        public Task<ApiResponse<bool>> FullSyncAsync(CancellationToken ct = default)
            => SendAsync<object, bool>(
                DataSyncCommands.FullSync,
                new { SyncType = "Full", Timestamp = DateTime.UtcNow },
                "全量同步完成", 30000,
                 ct);
    }
}
