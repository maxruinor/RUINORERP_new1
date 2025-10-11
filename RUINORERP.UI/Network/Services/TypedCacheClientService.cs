using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    public class TypedCacheClientService
    {
        private readonly ClientCommunicationService _comm;
        private readonly CommandPacketAdapter _packetAdapter;
        private readonly ILogger<TypedCacheClientService> _logger;

        public TypedCacheClientService(
            ClientCommunicationService comm,
            CommandPacketAdapter packetAdapter,
            ILogger<TypedCacheClientService> logger)
        {
            _comm = comm;
            _packetAdapter = packetAdapter;
            _logger = logger;
        }

        // 获取强类型缓存数据
        public async Task<CacheData<T>> GetTypedCacheAsync<T>(
            string tableName,
            bool forceRefresh = false,
            CancellationToken ct = default)
        {
            try
            {
                var command = new TypedCacheCommand<T>(tableName, forceRefresh);
                var response = await _comm.SendCommandWithResponseAsync<TypedCacheRequest<T>, TypedCacheResponse<T>>(
                    command, _packetAdapter, ct);

                if (response.IsSuccess && response.ResponseData?.CacheData != null)
                {
                    return response.ResponseData.CacheData;
                }

                throw new InvalidOperationException($"获取强类型缓存失败: {response.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取强类型缓存数据失败: {TableName}", tableName);
                throw;
            }
        }

        // 获取分页强类型缓存数据
        public async Task<PagedCacheData<T>> GetPagedTypedCacheAsync<T>(
            string tableName,
            int pageIndex,
            int pageSize,
            CancellationToken ct = default)
        {
            try
            {
                var command = new TypedCacheCommand<T>(tableName, pageIndex, pageSize);
                var response = await _comm.SendCommandWithResponseAsync<TypedCacheRequest<T>, TypedCacheResponse<T>>(
                    command, _packetAdapter, ct);

                if (response.IsSuccess && response.ResponseData?.PagedData != null)
                {
                    return response.ResponseData.PagedData;
                }

                throw new InvalidOperationException($"获取分页缓存失败: {response.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取分页强类型缓存数据失败: {TableName}", tableName);
                throw;
            }
        }

        // 批量获取多个表的强类型缓存
        public async Task<Dictionary<string, CacheData<T>>> GetMultipleTypedCachesAsync<T>(
            List<string> tableNames,
            CancellationToken ct = default)
        {
            var tasks = tableNames.ToDictionary(
                tableName => tableName,
                tableName => GetTypedCacheAsync<T>(tableName, false, ct));

            await Task.WhenAll(tasks.Values);

            return tasks.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Result);
        }
    }
}
