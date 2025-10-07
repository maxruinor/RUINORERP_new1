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

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 客户端缓存服务 - 处理缓存请求和响应
    /// </summary>
    public class CacheClientService
    {
        private readonly ClientCommunicationService _comm;
        private readonly ILogger<CacheClientService> _log;
        private readonly Dictionary<string, DateTime> _lastRequestTimes = new Dictionary<string, DateTime>();
        private readonly object _lockObj = new object();
        private IAuthorizeController authorizeController;

        public CacheClientService(ClientCommunicationService comm, ILogger<CacheClientService> log = null)
        {
            _comm = comm ?? throw new ArgumentNullException(nameof(comm));
            _log = log;

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
            _comm.CommandReceived += (commandId, data) =>
            {
                if (commandId == CacheCommands.CacheDataList)
                {
                    HandleCacheResponse(data);
                }
                else if (commandId == CacheCommands.CacheDataSend)
                {
                    HandleCacheResponse(data);
                }
            };
        }

        /// <summary>
        /// 请求缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="ct">取消令牌</param>
        public async Task<CacheResponse> RequestCacheAsync(string tableName, bool forceRefresh = false, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new ArgumentException("表名不能为空", nameof(tableName));

                _log?.LogDebug("请求缓存数据，表名={0}，强制刷新={1}", tableName, forceRefresh);

                // 使用工厂方法创建请求对象
                var request = CacheRequest.Create(tableName, forceRefresh);
                request.ClientVersion = Application.ProductVersion;
                request.LastRequestTime = GetLastRequestTime(tableName);

                var baseCommand = CommandDataBuilder.BuildCommand<CacheRequest, CacheResponse>(CacheCommands.CacheRequest, request);

                // 使用IClientCommunicationService发送请求
                var response = await _comm.SendCommandAsync<CacheRequest, CacheResponse>(
                    baseCommand,
                    ct);

                // 更新请求时间
                UpdateLastRequestTime(tableName);

                if (response != null && response.IsSuccess)
                {
                    ProcessCacheData(tableName, response.CacheData);

                    if (authorizeController != null && authorizeController.GetShowDebugInfoAuthorization())
                    {
                        MainForm.Instance.PrintInfoLog($"接收缓存: {tableName}, 数据量: {response.CacheData.Count}");
                    }
                }
                else if (response != null)
                {
                    MainForm.Instance.PrintInfoLog($"接收缓存失败: {response.Message ?? "未知错误"}");
                }

                return response;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "请求缓存失败: {Message}", ex.Message);
                MainForm.Instance.PrintInfoLog($"请求缓存失败: {ex.Message}");
                return null;
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
                            MainForm.Instance.PrintInfoLog($"接收缓存: {response.TableName}, 数据量: {response.CacheData?.Count ?? 0}");
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
        private void ProcessCacheData(string tableName, Dictionary<string, object> cacheData)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName) || cacheData == null || cacheData.Count == 0)
                {
                    _log?.LogWarning("缓存数据处理失败: 表名或数据为空");
                    return;
                }

                // 根据表名获取缓存类型
                if (BizCacheHelper.Manager.NewTableTypeList.TryGetValue(tableName, out Type type))
                {
                    _log?.LogInformation("开始处理缓存数据，表名={0}，数据量={1}", tableName, cacheData.Count);

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
        /// 从缓存中获取外键对应的名称
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="id">外键ID</param>
        /// <returns>对应的名称</returns>
        public string GetNameFromCache(string tableName, long id)
        {
            try
            {
                _log?.LogDebug("从缓存获取名称，表名={0}，ID={1}", tableName, id);

                // 检查表是否在缓存中
                if (!BizCacheHelper.Manager.NewTableList.ContainsKey(tableName))
                {
                    _log?.LogWarning("表{0}不在缓存配置中，尝试请求缓存", tableName);
                    // 如果不在缓存中，尝试请求
                    Task.Run(async () => await RequestCacheAsync(tableName));
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
            catch (Exception ex)
            {
                _log?.LogError(ex, "从缓存获取名称失败: {Message}", ex.Message);
                MainForm.Instance.PrintInfoLog($"从缓存获取名称失败: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 清理指定表的缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        public void ClearCache(string tableName)
        {
            try
            {
                _log?.LogInformation("开始清理表{0}的缓存", tableName);
                BizCacheHelper.Manager.CacheEntityList.Remove(tableName);
                lock (_lockObj)
                {
                    if (_lastRequestTimes.ContainsKey(tableName))
                    {
                        _lastRequestTimes[tableName] = DateTime.MinValue;
                    }
                }
                _log?.LogInformation("表{0}的缓存清理完成", tableName);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "清理缓存失败: {Message}", ex.Message);
                MainForm.Instance.PrintInfoLog($"清理缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 清理所有缓存
        /// </summary>
        public void ClearAllCache()
        {
            try
            {
                _log?.LogInformation("开始清理所有缓存");
                BizCacheHelper.Manager.CacheEntityList.Clear();
                lock (_lockObj)
                {
                    _lastRequestTimes.Clear();
                }
                _log?.LogInformation("所有缓存清理完成");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "清理所有缓存失败: {Message}", ex.Message);
                MainForm.Instance.PrintInfoLog($"清理所有缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取缓存状态
        /// </summary>
        /// <returns>缓存状态信息</returns>
        public Dictionary<string, CacheStatusInfo> GetCacheStatus()
        {
            var statusInfo = new Dictionary<string, CacheStatusInfo>();

            try
            {
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
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取缓存状态失败: {Message}", ex.Message);
                MainForm.Instance.PrintInfoLog($"获取缓存状态失败: {ex.Message}");
            }

            return statusInfo;
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
}