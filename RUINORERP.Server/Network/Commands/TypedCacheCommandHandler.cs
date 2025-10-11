using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Commands
{
    public class TypedCacheCommandHandler<T> : BaseCommandHandler where T : class, new()
    {
        private readonly ILogger<TypedCacheCommandHandler<T>> _logger;
        private readonly ISessionService _sessionService;
        private readonly CachePaginationService _cachePaginationService;

        public TypedCacheCommandHandler(
            ILogger<TypedCacheCommandHandler<T>> logger,
            ISessionService sessionService,
            CachePaginationService cachePaginationService)
        {
            _logger = logger;
            _sessionService = sessionService;
            _cachePaginationService = cachePaginationService;
            SetSupportedCommands(CacheCommands.CacheDataList.FullCode);
        }

        protected override async Task<BaseCommand<IResponse>> OnHandleAsync(
            QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (cmd.Command is TypedCacheCommand<T> typedCommand)
                {
                    return await HandleTypedCacheRequestAsync(typedCommand, cmd.Packet.ExecutionContext, cancellationToken);
                }

                return BaseCommand<IResponse>.CreateError("不支持的缓存命令类型");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理强类型缓存请求失败");
                return BaseCommand<IResponse>.CreateError($"处理缓存请求失败: {ex.Message}");
            }
        }

        private async Task<BaseCommand<IResponse>> HandleTypedCacheRequestAsync(
            TypedCacheCommand<T> command,
            CmdContext executionContext,
            CancellationToken cancellationToken)
        {
            var request = command.Request;

            // 1. 验证请求
            if (string.IsNullOrEmpty(request.TableName))
            {
                return BaseCommand<IResponse>.CreateError("表名不能为空");
            }

            // 2. 检查缓存有效性或强制刷新
            if (request.ForceRefresh || !IsCacheValid(request.TableName, request.LastRequestTime))
            {
                await RefreshTypedCacheAsync(request.TableName, cancellationToken);
            }

            // 3. 获取强类型缓存数据
            var cacheData = await GetTypedCacheDataAsync<T>(request.TableName, request.PageIndex, request.PageSize);

            if (cacheData == null)
            {
                return BaseCommand<IResponse>.CreateError($"缓存数据不存在: {request.TableName}");
            }

            // 4. 构建响应
            var response = new TypedCacheResponse<T>
            {
                CacheData = cacheData,
                IsSuccess = true,
                Message = "强类型缓存数据获取成功"
            };

            // 5. 处理分页数据
            if (request.PageSize > 0)
            {
                // 调用CachePaginationService获取分页缓存数据
                var pagedResult = await _cachePaginationService.GetPagedCacheDataAsync<T>(
                    request.TableName,
                    request.PageIndex,
                    request.PageSize,
                    null, // filterConditions
                    null, // orderBy
                    false, // descending
                    request.ForceRefresh, // forceRefresh
                    cancellationToken);

                response.PagedData = pagedResult;
                response.IsPartial = true;
            }

            return BaseCommand<IResponse>.CreateSuccess(response);
        }

        private async Task<CacheData<T>> GetTypedCacheDataAsync<T>(string tableName, int pageIndex, int pageSize)
        {
            try
            {
                // 获取缓存数据，传入表名作为参数
                var cacheList = BizCacheHelper.Instance.GetEntity<T>(tableName);
                if (cacheList == null) return null;

                // 转换为强类型列表
                var typedList = ConvertToTypedList<T>(cacheList);

                // 应用分页
                if (pageSize > 0)
                {
                    typedList = typedList?
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .ToList();
                }

                return new CacheData<T>
                {
                    TableName = tableName,
                    Data = typedList ?? new List<T>(),
                    CacheTime = DateTime.Now,
                    ExpirationTime = DateTime.Now.AddHours(1),
                    Version = GetCacheVersion(tableName)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取强类型缓存数据失败: {TableName}", tableName);
                return null;
            }
        }

        private List<T> ConvertToTypedList<T>(object cacheData)
        {
            if (cacheData is JArray jArray)
            {
                return jArray.ToObject<List<T>>();
            }

            if (cacheData is IEnumerable<object> enumerable)
            {
                return enumerable.Cast<T>().ToList();
            }

            if (cacheData is IList list)
            {
                return list.Cast<T>().ToList();
            }

            return null;
        }

        private async Task RefreshTypedCacheAsync(string tableName, CancellationToken cancellationToken)
        {
            // 调用现有的缓存刷新逻辑
            BizCacheHelper.Instance.SetDictDataSource(tableName, true);
            await Task.Delay(100, cancellationToken); // 等待缓存刷新
        }

        private bool IsCacheValid(string tableName, DateTime lastRequestTime)
        {
            var cacheInfo = MyCacheManager.Instance.CacheInfoList.Get(tableName) as CacheInfo;
            return cacheInfo?.LastUpdateTime > lastRequestTime;
        }

        private string GetCacheVersion(string tableName)
        {
            return DateTime.Now.Ticks.ToString();
        }
    }
}
