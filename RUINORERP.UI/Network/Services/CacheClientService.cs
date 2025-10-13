using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models.Responses.Cache;
using RUINORERP.PacketSpec.Commands.Cache;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Business.Security;
using RUINORERP.PacketSpec.Models.Core;
using System.Windows.Forms;
using RUINORERP.Business.CommService;
using FastReport.Table;
using Newtonsoft.Json.Linq;
using RUINORERP.Extensions.Middlewares;
using System.Reflection;
using RUINORERP.Common.Helper;
using RUINORERP.SecurityTool;
using System.Collections;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Core.DataProcessing;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 客户端缓存服务 - 处理缓存请求和响应
    /// 实现IDisposable接口以支持资源清理
    /// </summary>
    public class CacheClientService : IDisposable
    {
        private readonly ClientCommunicationService _comm;
        private readonly ILogger<CacheClientService> _log;
        private readonly CommandPacketAdapter commandPacketAdapter;
        private readonly Dictionary<string, DateTime> _lastRequestTimes = new Dictionary<string, DateTime>();
        private readonly object _lockObj = new object();
        private IAuthorizeController authorizeController;
        private bool _disposed = false;

        /// <summary>
        /// 构造函数 - 初始化缓存客户端服务
        /// </summary>
        /// <param name="comm">通信服务</param>
        /// <param name="commandPacketAdapter">命令包适配器</param>
        /// <param name="log">日志记录器</param>
        public CacheClientService(ClientCommunicationService comm, CommandPacketAdapter _commandPacketAdapter, ILogger<CacheClientService> log = null)
        {
            _comm = comm ?? throw new ArgumentNullException(nameof(comm));
            _log = log;
            commandPacketAdapter = _commandPacketAdapter ?? throw new ArgumentNullException(nameof(_commandPacketAdapter));
            // 注册缓存响应处理
            RegisterCommandHandlers();
            try
            {
                authorizeController = Startup.GetFromFac<IAuthorizeController>();
            }
            catch { }
        }

        private void RegisterCommandHandlers()
        {
            _comm.CommandReceived += (command, data) =>
            {
                if (command.CommandId == CacheCommands.CacheDataList)
                {
                    HandleCacheResponse(data);
                }
                else if (command.CommandId == CacheCommands.CacheDataSend)
                {
                    HandleCacheResponse(data);
                }
            };
        }

        /// <summary>
        /// 请求缓存数据 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>缓存响应结果</returns>
        public async Task<BaseCommand<CacheResponse>> RequestCacheAsync(string tableName, bool forceRefresh = false, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new ArgumentException("表名不能为空", nameof(tableName));

                _log?.LogInformation("开始请求缓存数据，表名={0}，强制刷新={1}", tableName, forceRefresh);

                // 使用工厂方法创建请求对象
                var request = CacheRequest.Create(tableName, forceRefresh);
                request.LastRequestTime = GetLastRequestTime(tableName);

                // 使用统一命令处理模式，模仿登录业务流程
                return await ProcessCacheRequestAsync(request, ct);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "请求缓存过程中发生异常，表名={0}", tableName);
                // 创建一个包含异常信息的错误响应
                var errorResponse = BaseCommand<CacheResponse>.CreateError($"请求缓存过程中发生异常: {ex.Message}");
                if (ex.InnerException != null)
                {
                    errorResponse.WithMetadata("InnerException", ex.InnerException.Message);
                }
                return errorResponse;
            }
        }

        /// <summary>
        /// 统一处理缓存请求 - 模仿登录业务流程
        /// </summary>
        /// <param name="request">缓存请求</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>缓存响应结果</returns>
        private async Task<BaseCommand<CacheResponse>> ProcessCacheRequestAsync(CacheRequest request, CancellationToken ct)
        {
            try
            {
                // 使用CommandDataBuilder构建命令
                var cacheCommand = CommandDataBuilder.BuildCommand<CacheRequest, CacheResponse>(
                    CacheCommands.CacheRequest,
                    request,
                    cmd => cmd.TimeoutMs = 30000
                );

                // 使用新的方法发送命令并获取包含指令信息的响应
                var commandResponse = await _comm.SendCommandWithResponseAsync<CacheRequest, CacheResponse>(cacheCommand, commandPacketAdapter, ct, 30000);

                // 检查响应是否成功
                if (!commandResponse.IsSuccess)
                {
                    return BaseCommand<CacheResponse>.CreateError(commandResponse.Message)
                        .WithMetadata("RequestId", commandResponse.RequestId ?? string.Empty);
                }

                // 缓存请求成功后处理数据
                if (commandResponse.IsSuccess && commandResponse.ResponseData != null)
                {
                    var cacheResponse = commandResponse.ResponseData;

                    // 处理缓存数据
                    //ProcessCacheData(request.TableName, cacheResponse.CacheData);

                    // 更新请求时间
                    UpdateLastRequestTime(request.TableName);

                    if (authorizeController != null && authorizeController.GetShowDebugInfoAuthorization())
                    {
                        //MainForm.Instance.PrintInfoLog($"接收缓存成功: {request.TableName}, 数据量: {cacheResponse.CacheData?.Count ?? 0}");
                    }

                }
                else
                {
                    _log?.LogWarning("缓存请求失败，表名={0}，错误: {1}",
                        request.TableName, commandResponse.Message);
                    MainForm.Instance.PrintInfoLog($"接收缓存失败: {commandResponse.Message ?? "未知错误"}");
                }

                return commandResponse;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存请求失败，表名={0}", request.TableName);
                throw;
            }
        }

        private DateTime GetLastRequestTime(string tableName)
        {
            lock (_lockObj)
            {
                if (_lastRequestTimes.TryGetValue(tableName, out var time))
                    return time;
                return DateTime.MinValue;
            }
        }

        private void UpdateLastRequestTime(string tableName)
        {
            lock (_lockObj)
            {
                _lastRequestTimes[tableName] = DateTime.Now;
            }
        }

        /// <summary>
        /// 处理缓存响应
        /// </summary>
        /// <param name="data">缓存响应数据</param>
        private void HandleCacheResponse(object data)
        {
            try
            {
                if (data == null)
                {
                    return;
                }

                // 确保data是CacheResponse类型
                if (data is CacheResponse response)
                {
                    if (response.IsSuccess && !string.IsNullOrEmpty(response.TableName))
                    {
                        // 处理缓存数据
                        ProcessCacheData(response.TableName, response.CacheData);

                        if (authorizeController != null && authorizeController.GetShowDebugInfoAuthorization())
                        {
                        //    MainForm.Instance.PrintInfoLog($"接收缓存: {response.TableName}, 数据量: {response.CacheData?.Count ?? 0}");
                        }

                        // 如果有更多数据，继续请求
                        if (response.HasMoreData)
                        {
                            Task.Run(async () =>
                            {
                                await Task.Delay(100);
                                await RequestCacheAsync(response.TableName, false);
                            });
                        }
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"接收缓存失败: {response?.Message ?? "未知错误"}");
                    }
                }
                else
                {
                    MainForm.Instance.PrintInfoLog("处理缓存响应失败: 数据格式不正确");
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存响应失败: {Message}", ex.Message);
                MainForm.Instance.PrintInfoLog($"处理缓存响应失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cacheData">缓存数据</param>
        private void ProcessCacheData(string tableName, CacheData cacheData)
        {
            try
            {
              /*

                // 根据表名获取缓存类型
                if (BizCacheHelper.Manager.NewTableTypeList.TryGetValue(tableName, out Type type))
                {
                    // 处理JArray类型的缓存数据
                    if (cacheData.Values.FirstOrDefault() is JArray jArrayData)
                    {
                        // 直接使用JArray数据进行处理，避免重复序列化/反序列化
                        GetTableList(tableName, jArrayData);
                    }
                    else
                    {
                        // 将字典数据转换为实体列表
                        var list = ConvertDictionaryToEntityList(tableName, cacheData, type);
                        if (list != null)
                        {
                            // 更新缓存
                            MyCacheManager.Instance.UpdateEntityList(tableName, list);
                            _log?.LogInformation("缓存数据处理完成，表名={0}", tableName);
                        }
                        else
                        {
                            _log?.LogWarning("缓存数据转换失败，表名={0}", tableName);
                        }
                    }
                }
                else
                {
                    _log?.LogWarning("未找到缓存类型配置，表名={0}", tableName);
                }

                */

            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存数据失败: {Message}", ex.Message);
                MainForm.Instance.PrintInfoLog($"处理缓存数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 将字典数据转换为实体列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cacheData">缓存数据</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>转换后的实体列表</returns>
        private List<object> ConvertDictionaryToEntityList(string tableName, Dictionary<string, object> cacheData, Type entityType)
        {
            try
            {
                // 尝试直接反序列化或转换数据
                var jsonData = JsonConvert.SerializeObject(cacheData);
                var list = JsonConvert.DeserializeObject(jsonData, typeof(List<>).MakeGenericType(entityType));

                if (list is IEnumerable enumerableList)
                {
                    return enumerableList.Cast<object>().ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "转换缓存数据为实体列表失败: {Message}", ex.Message);
                return null;
            }
        }

        #region 缓存辅助方法
        /// <summary>
        /// 处理JArray类型的缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="objList">JArray类型的对象列表</param>
        private void GetTableList(string tableName, object objList)
        {
            if (objList != null && objList is JArray jArrayData)
            {
                // 获取表对应的实体类型
                if (BizCacheHelper.Manager.NewTableTypeList.TryGetValue(tableName, out Type entityType))
                {
                    // 转换JArray到实体列表
                    try
                    {
                        var entityList = ConvertJArrayToList(jArrayData, entityType);
                        if (entityList != null && entityList.Count > 0)
                        {
                            // 更新缓存
                            MyCacheManager.Instance.UpdateEntityList(tableName, entityList);
                            _log?.LogInformation("JArray缓存数据处理完成，表名={0}，数据量={1}", tableName, entityList.Count);
                        }
                    }
                    catch (Exception ex)
                    {
                        _log?.LogError(ex, "转换JArray缓存数据失败: {Message}", ex.Message);
                    }
                }
                else
                {
                    _log?.LogWarning("未找到缓存类型配置，表名={0}", tableName);
                }
            }
        }

        /// <summary>
        /// 将JArray转换为指定类型的对象列表
        /// </summary>
        /// <param name="jArray">JArray数据</param>
        /// <param name="entityType">目标实体类型</param>
        /// <returns>转换后的对象列表</returns>
        private List<object> ConvertJArrayToList(JArray jArray, Type entityType)
        {
            if (jArray == null)
                throw new ArgumentNullException(nameof(jArray));

            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            try
            {
                // 使用JToken.ToObject<T>()方法将每个JToken转换为指定类型
                MethodInfo toObjectMethod = typeof(JToken).GetMethod("ToObject", new Type[] { });
                MethodInfo genericToObjectMethod = toObjectMethod.MakeGenericMethod(entityType);

                List<object> list = new List<object>(jArray.Count);
                foreach (JToken token in jArray)
                {
                    object item = genericToObjectMethod.Invoke(token, null);
                    list.Add(item);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("转换JArray失败", ex);
            }
        }
        #endregion

        /// <summary>
        /// 传统方式请求缓存数据 - 保持向后兼容性
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>传统的响应包装</returns>
        public async Task<ResponseBase<CacheResponse>> RequestCacheTraditionalAsync(string tableName, bool forceRefresh = false, CancellationToken ct = default)
        {
            var commandResponse = await RequestCacheAsync(tableName, forceRefresh, ct);

            return new ResponseBase<CacheResponse>
            {
                Message = commandResponse.Message,
                Data = commandResponse.ResponseData,
                RequestId = commandResponse.RequestId
            };
        }



        /// <summary>
        /// 统一处理从缓存获取名称 - 模仿登录业务流程
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="id">外键ID</param>
        /// <returns>对应的名称</returns>
        private async Task<string> ProcessGetNameFromCacheAsync(string tableName, long id)
        {
            _log?.LogDebug("从缓存获取名称，表名={0}，ID={1}", tableName, id);

            // 检查表是否在缓存中
            if (!BizCacheHelper.Manager.NewTableList.ContainsKey(tableName))
            {
                _log?.LogWarning("表{0}不在缓存配置中，尝试请求缓存", tableName);
                // 如果不在缓存中，尝试请求
                await Task.Run(async () => await RequestCacheAsync(tableName));
                return string.Empty;
            }

            // 获取缓存数据
            var cacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
            if (cacheList == null)
            {
                _log?.LogWarning("表{0}的缓存数据为空", tableName);
                return string.Empty;
            }

            // 获取ID和Name列名
            var columnPair = BizCacheHelper.Manager.NewTableList[tableName];
            string idColName = columnPair.Key;
            string nameColName = columnPair.Value;

            // 查找对应ID的记录
            foreach (var item in (IEnumerable<object>)cacheList)
            {
                if (item.GetType().GetProperty(idColName)?.GetValue(item)?.ToString() == id.ToString())
                {
                    string name = item.GetType().GetProperty(nameColName)?.GetValue(item)?.ToString() ?? string.Empty;
                    _log?.LogDebug("从缓存获取名称成功，表名={0}，ID={1}，名称={2}", tableName, id, name);
                    return name;
                }
            }

            _log?.LogWarning("在表{0}的缓存中未找到ID={1}的记录", tableName, id);
            return string.Empty;
        }

        /// <summary>
        /// 清理指定表的缓存 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="tableName">表名</param>
        public void ClearCache(string tableName)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    _log?.LogWarning("清理缓存时表名为空");
                    return;
                }

                // 使用统一命令处理模式清理缓存
                ProcessClearCacheAsync(tableName).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "清理缓存失败，表名={0}", tableName);
            }
        }

        /// <summary>
        /// 统一处理清理缓存 - 模仿登录业务流程
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>任务</returns>
        private async Task ProcessClearCacheAsync(string tableName)
        {
            _log?.LogInformation("开始清理缓存，表名={0}", tableName);
            //MyCacheManager.Instance.DeleteEntity(tableName);
            _log?.LogInformation("清理缓存完成，表名={0}", tableName);
        }

        /// <summary>
        /// 清理所有缓存 - 统一使用泛型命令处理模式
        /// </summary>
        public void ClearAllCache()
        {
            try
            {
                // 使用统一命令处理模式清理所有缓存
                ProcessClearAllCacheAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "清理所有缓存失败: {Message}", ex.Message);
                MainForm.Instance.PrintInfoLog($"清理所有缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 统一处理清理所有缓存 - 模仿登录业务流程
        /// </summary>
        /// <returns>任务</returns>
        private async Task ProcessClearAllCacheAsync()
        {
            _log?.LogInformation("开始清理所有缓存");
            BizCacheHelper.Manager.CacheEntityList.Clear();
            lock (_lockObj)
            {
                _lastRequestTimes.Clear();
            }
            _log?.LogInformation("所有缓存清理完成");
        }

        /// <summary>
        /// 批量请求多个表的缓存数据 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="tableNames">表名列表</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>批量缓存响应结果</returns>
        public async Task<Dictionary<string, BaseCommand<CacheResponse>>> RequestMultipleCachesAsync(
            List<string> tableNames,
            bool forceRefresh = false,
            CancellationToken ct = default)
        {
            if (tableNames == null || tableNames.Count == 0)
            {
                throw new ArgumentException("表名列表不能为空", nameof(tableNames));
            }

            var results = new Dictionary<string, BaseCommand<CacheResponse>>();

            try
            {
                _log?.LogInformation("开始批量请求缓存数据，表数量={0}", tableNames.Count);

                // 使用统一命令处理模式批量处理请求
                return await ProcessMultipleCacheRequestsAsync(tableNames, forceRefresh, ct);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "批量请求缓存数据失败");
                // 创建一个包含异常信息的错误响应
                var errorResponse = BaseCommand<CacheResponse>.CreateError($"批量请求缓存数据失败: {ex.Message}");
                if (ex.InnerException != null)
                {
                    errorResponse.WithMetadata("InnerException", ex.InnerException.Message);
                }
                return new Dictionary<string, BaseCommand<CacheResponse>>
                {
                    { "Error", errorResponse }
                };
            }
        }

        /// <summary>
        /// 统一处理多个缓存请求 - 模仿登录业务流程
        /// </summary>
        /// <param name="tableNames">表名列表</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>批量缓存响应结果</returns>
        private async Task<Dictionary<string, BaseCommand<CacheResponse>>> ProcessMultipleCacheRequestsAsync(
            List<string> tableNames,
            bool forceRefresh,
            CancellationToken ct)
        {
            // 并行请求多个表的缓存数据
            var tasks = tableNames.Select(async tableName =>
            {
                try
                {
                    var response = await RequestCacheAsync(tableName, forceRefresh, ct);
                    return new { TableName = tableName, Response = response };
                }
                catch (Exception ex)
                {
                    _log?.LogError(ex, "请求表 {0} 的缓存失败", tableName);
                    // 创建一个包含异常信息的错误响应
                    var errorResponse = BaseCommand<CacheResponse>.CreateError($"请求表 {tableName} 的缓存失败: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        errorResponse.WithMetadata("InnerException", ex.InnerException.Message);
                    }
                    return new { TableName = tableName, Response = errorResponse };
                }
            });

            var taskResults = await Task.WhenAll(tasks);
            var results = new Dictionary<string, BaseCommand<CacheResponse>>();

            foreach (var result in taskResults)
            {
                results[result.TableName] = result.Response;
            }

            _log?.LogInformation("批量请求缓存数据完成，成功={0}，失败={1}",
                results.Values.Count(r => r != null && r.IsSuccess),
                results.Values.Count(r => r == null || !r.IsSuccess));

            return results;
        }

        /// <summary>
        /// 刷新指定表的缓存数据 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>刷新结果</returns>
        public async Task<BaseCommand<CacheResponse>> RefreshCacheAsync(string tableName, CancellationToken ct = default)
        {
            try
            {
                _log?.LogInformation("开始刷新缓存数据，表名={0}", tableName);

                // 创建刷新请求
                var request = CacheRequest.CreateRefreshRequest(tableName);

                // 使用统一命令处理模式
                return await ProcessCacheRefreshAsync(request, ct);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "刷新缓存数据失败: {Message}", ex.Message);
                MainForm.Instance.PrintInfoLog($"刷新缓存数据失败: {ex.Message}");
                // 创建一个包含异常信息的错误响应
                var errorResponse = BaseCommand<CacheResponse>.CreateError($"刷新缓存数据失败: {ex.Message}");
                if (ex.InnerException != null)
                {
                    errorResponse.WithMetadata("InnerException", ex.InnerException.Message);
                }
                return errorResponse;
            }
        }

        /// <summary>
        /// 统一处理缓存刷新 - 模仿登录业务流程
        /// </summary>
        /// <param name="request">缓存刷新请求</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>缓存响应结果</returns>
        private async Task<BaseCommand<CacheResponse>> ProcessCacheRefreshAsync(CacheRequest request, CancellationToken ct)
        {
            try
            {
                // 使用CommandDataBuilder构建命令
                var cacheCommand = CommandDataBuilder.BuildCommand<CacheRequest, CacheResponse>(
                    CacheCommands.CacheRequest,
                    request,
                    cmd => cmd.TimeoutMs = 30000
                );

                // 发送命令并等待响应
                var response = await _comm.SendCommandWithResponseAsync<CacheRequest, CacheResponse>(cacheCommand, commandPacketAdapter, ct, 30000);

                // 处理响应
                if (response.IsSuccess && response.ResponseData != null)
                {
                  //  ProcessCacheData(request.TableName, response.ResponseData.CacheData);
                    UpdateLastRequestTime(request.TableName);
                    _log?.LogInformation("刷新缓存数据成功，表名={0}", request.TableName);
                }
                else
                {
                    _log?.LogWarning("刷新缓存数据失败，表名={0}，错误: {1}", request.TableName, response.Message);
                }

                return response;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存刷新失败，表名={0}", request.TableName);
                throw;
            }
        }

        /// <summary>
        /// 清理指定表的缓存数据 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>清理结果</returns>
        public async Task<BaseCommand<CacheResponse>> ClearCacheAsync(string tableName, CancellationToken ct = default)
        {
            try
            {
                _log?.LogInformation("开始清理缓存数据，表名={0}", tableName);

                // 使用统一命令处理模式清理缓存
                return await ProcessClearCacheAsync(tableName, ct);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "清理缓存失败，表名={0}", tableName);
                // 创建一个包含异常信息的错误响应
                var errorResponse = BaseCommand<CacheResponse>.CreateError($"清理缓存失败: {ex.Message}");
                if (ex.InnerException != null)
                {
                    errorResponse.WithMetadata("InnerException", ex.InnerException.Message);
                }
                return errorResponse;
            }
        }

        /// <summary>
        /// 统一处理清理缓存 - 模仿登录业务流程
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>清理结果</returns>
        private async Task<BaseCommand<CacheResponse>> ProcessClearCacheAsync(string tableName, CancellationToken ct)
        {
            // 创建清理请求
            var request = CacheRequest.Create(tableName, true);
            request.OperationType = "Cache.Clear";

            // 使用CommandDataBuilder构建命令
            var cacheCommand = CommandDataBuilder.BuildCommand<CacheRequest, CacheResponse>(
                CacheCommands.CacheClear,
                request,
                cmd => cmd.TimeoutMs = 30000
            );

            // 发送请求并等待响应
            var response = await _comm.SendCommandWithResponseAsync<CacheRequest, CacheResponse>(
                cacheCommand, commandPacketAdapter, ct);

            if (response != null && response.IsSuccess)
            {
                _log?.LogInformation("缓存清理成功，表名={0}", tableName);

                // 清理本地缓存
                BizCacheHelper.Manager.CacheEntityList.Remove(tableName);

                // 清理最后请求时间记录
                lock (_lockObj)
                {
                    _lastRequestTimes.Remove(tableName);
                }
                return response;
            }
            else
            {
                var errorMsg = response?.Message ?? "未知错误";
                _log?.LogWarning("缓存清理失败，表名={0}，错误={1}", tableName, errorMsg);
                return BaseCommand<CacheResponse>.CreateError(errorMsg)
                    .WithMetadata("RequestId", response?.RequestId ?? string.Empty);
            }
        }

        /// <summary>
        /// 验证缓存数据是否有效 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="maxAge">最大有效时间（分钟）</param>
        /// <returns>缓存是否有效</returns>
        public bool IsCacheValid(string tableName, int maxAge = 60)
        {
            try
            {
                // 使用统一命令处理模式验证缓存有效性
                return ProcessIsCacheValidAsync(tableName, maxAge).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "验证缓存有效性失败，表名={0}", tableName);
                return false;
            }
        }

        /// <summary>
        /// 统一处理验证缓存有效性 - 模仿登录业务流程
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="maxAge">最大有效时间（分钟）</param>
        /// <returns>缓存是否有效</returns>
        private async Task<bool> ProcessIsCacheValidAsync(string tableName, int maxAge)
        {
            if (string.IsNullOrEmpty(tableName))
                return false;

            var lastRequestTime = GetLastRequestTime(tableName);
            if (lastRequestTime == DateTime.MinValue)
                return false;

            var cacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
            if (cacheList == null)
                return false;

            var timeSpan = DateTime.Now - lastRequestTime;
            return timeSpan.TotalMinutes <= maxAge;
        }

        /// <summary>
        /// 缓存预热 - 批量加载常用表数据 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="tableNames">需要预热的表名列表</param>
        /// <param name="maxParallel">最大并行数</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>预热结果统计</returns>
        public async Task<CacheWarmupResult> WarmupCacheAsync(
            List<string> tableNames,
            int maxParallel = 5,
            CancellationToken ct = default)
        {
            var result = new CacheWarmupResult
            {
                StartTime = DateTime.Now,
                TotalTables = tableNames?.Count ?? 0
            };

            try
            {
                if (tableNames == null || tableNames.Count == 0)
                {
                    _log?.LogWarning("缓存预热：表名列表为空");
                    return result;
                }

                _log?.LogInformation("开始缓存预热，表数量={0}，最大并行数={1}",
                    tableNames.Count, maxParallel);

                // 使用统一命令处理模式预热缓存
                return await ProcessWarmupCacheAsync(tableNames, maxParallel, ct);
            }
            catch (Exception ex)
            {
                result.EndTime = DateTime.Now;
                result.Duration = result.EndTime - result.StartTime;

                _log?.LogError(ex, "缓存预热过程异常");
                return new CacheWarmupResult
                {
                    FailedCount = tableNames?.Count ?? 0,
                    TotalTables = tableNames?.Count ?? 0,
                    EndTime = DateTime.Now,
                    Duration = TimeSpan.Zero
                };
            }
        }

        /// <summary>
        /// 统一处理缓存预热 - 模仿登录业务流程
        /// </summary>
        /// <param name="tableNames">需要预热的表名列表</param>
        /// <param name="maxParallel">最大并行数</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>预热结果统计</returns>
        private async Task<CacheWarmupResult> ProcessWarmupCacheAsync(
            List<string> tableNames,
            int maxParallel,
            CancellationToken ct)
        {
            var result = new CacheWarmupResult
            {
                StartTime = DateTime.Now,
                TotalTables = tableNames.Count
            };

            // 使用SemaphoreSlim控制并发数
            using var semaphore = new SemaphoreSlim(maxParallel, maxParallel);

            var tasks = tableNames.Select(async tableName =>
            {
                await semaphore.WaitAsync(ct);
                try
                {
                    var response = await RequestCacheAsync(tableName, false, ct);

                    if (response != null && response.IsSuccess)
                    {
                        Interlocked.Increment(ref result.SuccessCount);
                        _log?.LogDebug("缓存预热成功：{0}", tableName);
                    }
                    else
                    {
                        Interlocked.Increment(ref result.FailedCount);
                        _log?.LogWarning("缓存预热失败：{0}，错误={1}",
                            tableName, response?.Message ?? "未知错误");
                    }

                    return new { TableName = tableName, Success = response?.IsSuccess ?? false };
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref result.FailedCount);
                    _log?.LogError(ex, "缓存预热异常：{0}", tableName);
                    return new { TableName = tableName, Success = false };
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var warmupResults = await Task.WhenAll(tasks);
            result.EndTime = DateTime.Now;
            result.Duration = result.EndTime - result.StartTime;

            // 记录失败的表
            result.FailedTables = warmupResults
                .Where(r => !r.Success)
                .Select(r => r.TableName)
                .ToList();

            _log?.LogInformation("缓存预热完成，总计={0}，成功={1}，失败={2}，耗时={3}ms",
                result.TotalTables, result.SuccessCount, result.FailedCount,
                result.Duration.TotalMilliseconds);

            return result;
        }

        /// <summary>
        /// 获取缓存状态 - 统一使用泛型命令处理模式
        /// </summary>
        /// <returns>缓存状态信息</returns>
        public Dictionary<string, CacheStatusInfo> GetCacheStatus()
        {
            try
            {
                // 使用统一命令处理模式获取缓存状态
                return ProcessGetCacheStatusAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取缓存状态失败: {Message}", ex.Message);
                MainForm.Instance.PrintInfoLog($"获取缓存状态失败: {ex.Message}");
                return new Dictionary<string, CacheStatusInfo>();
            }
        }

        /// <summary>
        /// 统一处理获取缓存状态 - 模仿登录业务流程
        /// </summary>
        /// <returns>缓存状态信息</returns>
        private async Task<Dictionary<string, CacheStatusInfo>> ProcessGetCacheStatusAsync()
        {
            var statusInfo = new Dictionary<string, CacheStatusInfo>();

            _log?.LogDebug("开始获取缓存状态");
            foreach (var tableName in BizCacheHelper.Manager.NewTableList.Keys)
            {
                var cacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                int count = 0;
                if (cacheList != null && cacheList is IEnumerable<object> enumerable)
                {
                    count = enumerable.Count();
                }

                statusInfo[tableName] = new CacheStatusInfo
                {
                    TableName = tableName,
                    Count = count,
                    LastRequestTime = _lastRequestTimes.ContainsKey(tableName) ? _lastRequestTimes[tableName] : DateTime.MinValue
                };
            }
            _log?.LogDebug("获取缓存状态完成，共{0}个表", statusInfo.Count);

            return statusInfo;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源实现
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 清理托管资源
                    lock (_lockObj)
                    {
                        _lastRequestTimes.Clear();
                    }
                }

                // 清理非托管资源
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// 缓存状态信息
    /// </summary>
    public class CacheStatusInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 缓存数量
        /// </summary>
        public int Count { get; set; } = 0;

        /// <summary>
        /// 上次请求时间
        /// </summary>
        public DateTime LastRequestTime { get; set; } = DateTime.MinValue;
    }

    /// <summary>
    /// 缓存预热结果统计
    /// </summary>
    public class CacheWarmupResult
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 总耗时
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// 总表数量
        /// </summary>
        public int TotalTables { get; set; }

        /// <summary>
        /// 成功数量
        /// </summary>
        public int SuccessCount;

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailedCount;

        /// <summary>
        /// 失败的表名列表
        /// </summary>
        public List<string> FailedTables { get; set; } = new List<string>();

        /// <summary>
        /// 是否全部成功
        /// </summary>
        public bool IsAllSuccess => FailedCount == 0;

        /// <summary>
        /// 成功率（百分比）
        /// </summary>
        public double SuccessRate => TotalTables > 0 ? (double)SuccessCount / TotalTables * 100 : 0;
    }

    /// <summary>
    /// 常用缓存表配置
    /// </summary>
    public static class CommonCacheTables
    {
        /// <summary>
        /// 基础数据表
        /// </summary>
        public static readonly List<string> BasicDataTables = new List<string>
        {
            "tb_ProductType",
            "tb_Unit",
            "tb_Currency",
            "tb_Department",
            "tb_Location",
            "tb_Province",
            "tb_City"
        };

        /// <summary>
        /// 用户权限相关表
        /// </summary>
        public static readonly List<string> UserAuthTables = new List<string>
        {
            "tb_UserInfo",
            "tb_RoleInfo",
            "tb_MenuInfo",
            "tb_ButtonInfo",
            "tb_RoleMenu",
            "tb_RoleButton"
        };

        /// <summary>
        /// 业务配置表
        /// </summary>
        public static readonly List<string> BusinessConfigTables = new List<string>
        {
            "tb_BOMConfig",
            "tb_ProductionConfig",
            "tb_InventoryConfig",
            "tb_FinancialConfig"
        };

        /// <summary>
        /// 系统配置表
        /// </summary>
        public static readonly List<string> SystemConfigTables = new List<string>
        {
            "tb_SystemConfig",
            "tb_PrintTemplate",
            "tb_ReportTemplate",
            "tb_WorkflowConfig"
        };

        /// <summary>
        /// 获取所有常用表
        /// </summary>
        public static List<string> GetAllCommonTables()
        {
            var allTables = new List<string>();
            allTables.AddRange(BasicDataTables);
            allTables.AddRange(UserAuthTables);
            allTables.AddRange(BusinessConfigTables);
            allTables.AddRange(SystemConfigTables);
            return allTables.Distinct().ToList();
        }

        /// <summary>
        /// 根据类型获取缓存表
        /// </summary>
        public static List<string> GetTablesByType(CacheTableType type)
        {
            return type switch
            {
                CacheTableType.BasicData => BasicDataTables,
                CacheTableType.UserAuth => UserAuthTables,
                CacheTableType.BusinessConfig => BusinessConfigTables,
                CacheTableType.SystemConfig => SystemConfigTables,
                CacheTableType.All => GetAllCommonTables(),
                _ => new List<string>()
            };
        }
    }

    /// <summary>
    /// 缓存表类型枚举
    /// </summary>
    public enum CacheTableType
    {
        /// <summary>
        /// 基础数据
        /// </summary>
        BasicData,

        /// <summary>
        /// 用户权限
        /// </summary>
        UserAuth,

        /// <summary>
        /// 业务配置
        /// </summary>
        BusinessConfig,

        /// <summary>
        /// 系统配置
        /// </summary>
        SystemConfig,

        /// <summary>
        /// 所有常用表
        /// </summary>
        All
    }

    /// <summary>
    /// 缓存服务扩展方法
    /// </summary>
    public static class CacheClientServiceExtensions
    {
        /// <summary>
        /// 预热基础数据缓存
        /// </summary>
        public static Task<CacheWarmupResult> WarmupBasicDataAsync(this CacheClientService service,
            CancellationToken ct = default)
        {
            return service.WarmupCacheAsync(CommonCacheTables.BasicDataTables, 3, ct);
        }

        /// <summary>
        /// 预热用户权限缓存
        /// </summary>
        public static Task<CacheWarmupResult> WarmupUserAuthAsync(this CacheClientService service,
            CancellationToken ct = default)
        {
            return service.WarmupCacheAsync(CommonCacheTables.UserAuthTables, 2, ct);
        }

        /// <summary>
        /// 预热业务配置缓存
        /// </summary>
        public static Task<CacheWarmupResult> WarmupBusinessConfigAsync(this CacheClientService service,
            CancellationToken ct = default)
        {
            return service.WarmupCacheAsync(CommonCacheTables.BusinessConfigTables, 2, ct);
        }

        /// <summary>
        /// 预热系统配置缓存
        /// </summary>
        public static Task<CacheWarmupResult> WarmupSystemConfigAsync(this CacheClientService service,
            CancellationToken ct = default)
        {
            return service.WarmupCacheAsync(CommonCacheTables.SystemConfigTables, 2, ct);
        }

        /// <summary>
        /// 预热所有常用缓存
        /// </summary>
        public static Task<CacheWarmupResult> WarmupAllCommonAsync(this CacheClientService service,
            int maxParallel = 5, CancellationToken ct = default)
        {
            return service.WarmupCacheAsync(CommonCacheTables.GetAllCommonTables(), maxParallel, ct);
        }

        /// <summary>
        /// 检查并刷新过期缓存
        /// </summary>
        public static async Task<Dictionary<string, bool>> RefreshExpiredCachesAsync(this CacheClientService service,
            List<string> tableNames, int maxAge = 60, CancellationToken ct = default)
        {
            var results = new Dictionary<string, bool>();

            foreach (var tableName in tableNames)
            {
                try
                {
                    if (!service.IsCacheValid(tableName, maxAge))
                    {
                        var response = await service.RefreshCacheAsync(tableName, ct);
                        results[tableName] = response?.IsSuccess ?? false;
                    }
                    else
                    {
                        results[tableName] = true; // 缓存仍然有效
                    }
                }
                catch (Exception)
                {
                    results[tableName] = false;
                }
            }

            return results;
        }
    }
}