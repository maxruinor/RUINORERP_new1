using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RUINORERP.Business;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using System.Threading;
using RUINORERP.Model.Context;
using Mapster;
using RUINORERP.Business.BizMapperService;


namespace RUINORERP.Business.CommService
{
    // 审计日志服务接口
    public interface IAuditLogService
    {
        Task LogAsync<T>(string action, T entity, string description = "") where T : class;
        Task LogAsync(string action, string description = "");
    }



    // 审计日志服务实现
    public class AuditLogService : IAuditLogService, IDisposable
    {
        private readonly ConcurrentQueue<tb_AuditLogs> _auditLogQueue = new ConcurrentQueue<tb_AuditLogs>();

        private readonly Lazy<EnhancedBizTypeMapper> _mapper;

        private readonly Lazy<BillConverterFactory> _billConverterFactory; // 缓存工厂
        private readonly Lazy<tb_AuditLogsController<tb_AuditLogs>> _AuditLogsController; // 缓存工厂
        private readonly Timer _flushTimer;
        private readonly AuditLogOptions _options;
        private readonly ILogger<AuditLogService> _logger;
        private readonly object _lockObject = new object();
        private bool _isFlushing = false;
        public ApplicationContext _appContext;
        public AuditLogService(IOptions<AuditLogOptions> options, ILogger<AuditLogService> logger, ApplicationContext appContext)
        {
            _appContext = appContext;
            _options = options.Value;
            _logger = logger;
            // 延迟解析依赖，直到第一次使用时才获取实例
            _billConverterFactory = new Lazy<BillConverterFactory>(
                () => _appContext.GetRequiredService<BillConverterFactory>());// 缓存工厂


            _mapper = new Lazy<EnhancedBizTypeMapper>(
                () => _appContext.GetRequiredService<EnhancedBizTypeMapper>());// 缓存工厂


            _AuditLogsController = new Lazy<tb_AuditLogsController<tb_AuditLogs>>(() => _appContext.GetRequiredService<tb_AuditLogsController<tb_AuditLogs>>());
            // 启动定时刷新
            _flushTimer = new Timer(FlushQueue, null, _options.FlushInterval, _options.FlushInterval);
        }

        public async Task LogAsync<T>(string action, T entity, string description = "") where T : class
        {
            if (!_options.EnableAudit) return;

            try
            {
                tb_AuditLogs auditLog = CreateAuditLog(action, entity, description);
                _auditLogQueue.Enqueue(auditLog);

                // 检查队列是否达到批量大小
                if (_auditLogQueue.Count >= _options.BatchSize)
                {
                    await FlushQueueAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建审计日志失败");
            }
        }

        public async Task LogAsync(string action, string description = "")
        {
            if (!_options.EnableAudit) return;

            try
            {
                tb_AuditLogs auditLog = CreateAuditLog(action, description);
                _auditLogQueue.Enqueue(auditLog);

                // 检查队列是否达到批量大小
                if (_auditLogQueue.Count >= _options.BatchSize)
                {
                    await FlushQueueAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建审计日志失败");
            }
        }

        private tb_AuditLogs CreateAuditLog<T>(string action, T entity, string description) where T : class
        {
            if (_appContext.CurUserInfo == null)
                throw new InvalidOperationException("当前用户信息为空");

            tb_AuditLogs auditLog = new tb_AuditLogs
            {
                UserName = _appContext.CurUserInfo.UserInfo.UserName,
                Employee_ID = _appContext.CurUserInfo.UserInfo.Employee_ID,
                ActionType = action,
                ActionTime = DateTime.Now,
                Notes = description
            };

            try
            {
                BizTypeMapper mapper = new BizTypeMapper();
                var BizType = mapper.GetBizType(typeof(T).Name);
                if (BizType == Global.BizType.默认数据)
                {
                    var t = typeof(T);
                    // 检查是否有 SugarTable 特性
                    bool hasSugarTable = SugarAttributeHelper.HasSugarTableAttribute(typeof(T));
                    if (hasSugarTable)
                    {
                        // 获取 Description 值
                        string tabledescription = SugarAttributeHelper.GetTypeDescription(t);
                        auditLog.OldState = tabledescription;

                        // 获取完整特性信息
                        //  var (hasAttr, desc, tableName) = SugarAttributeHelper.GetTypeAttributes(t);
                    }

                }
                //BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();
                //CommBillData cbd = bcf.GetBillData<T>(entity);


                // 直接使用缓存的工厂，避免重复解析
                // 使用时才解析依赖
                //var factory = _billConverterFactory.Value;
                //CommBillData cbd = factory.GetBillData<T>(entity);

                var bizType = _mapper.Value.GetBizType(typeof(T), entity);
                auditLog.ObjectType = (int)bizType;

                // 获取字段配置
                var (idField, noField) = _mapper.Value.GetEntityFieldValue<long>(typeof(T), entity);
                auditLog.ObjectId = idField;
                auditLog.ObjectNo = noField;

      
                var dataContent = EntityDataExtractor.ExtractDataContent(entity);
                auditLog.DataContent = dataContent;


                // 使用反射获取需要审计的字段
                //auditLog.DataContent = GetAuditDataContent(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取审计对象信息失败");
                auditLog.Notes += $" | 获取对象信息失败: {ex.Message}";
            }



            return auditLog;
        }

        private tb_AuditLogs CreateAuditLog(string action, string description)
        {
            if (_appContext.CurUserInfo == null ||
                string.IsNullOrEmpty(_appContext.CurUserInfo.UserInfo.UserName))
                return null;

            tb_AuditLogs auditLog = new tb_AuditLogs
            {
                UserName = _appContext.CurUserInfo.UserInfo.UserName,
                Employee_ID = _appContext.CurUserInfo.UserInfo.Employee_ID,
                ActionType = action,
                ActionTime = DateTime.Now,
                Notes = description,
                ObjectType = -1
            };

            return auditLog;
        }

        // 使用反射和特性获取需要审计的字段
        private string GetAuditDataContent(object entity, bool ignoreNullValues = true)
        {
            if (entity == null) return string.Empty;

            try
            {
                var entityType = entity.GetType();
                var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var auditData = new Dictionary<string, object>();

                foreach (var property in properties)
                {
                    // 检查是否有AuditField特性
                    var auditAttr = property.GetCustomAttribute<AuditFieldAttribute>();
                    if (auditAttr != null && auditAttr.IsAudited)
                    {
                        var displayName = !string.IsNullOrEmpty(auditAttr.DisplayName)
                            ? auditAttr.DisplayName
                            : property.Name;

                        var value = property.GetValue(entity);

                        // 忽略空值的逻辑
                        if (ignoreNullValues && IsValueNullOrEmpty(value))
                            continue;

                        auditData[displayName] = value;
                    }

                    // 检查是否有SugarColumn特性且不是忽略列
                    var sugarAttr = property.GetCustomAttribute<SqlSugar.SugarColumn>();
                    if (sugarAttr != null && !sugarAttr.IsIgnore && auditAttr == null)
                    {
                        // 默认审计所有非忽略的SugarColumn字段
                        var displayName = !string.IsNullOrEmpty(sugarAttr.ColumnDescription)
                            ? sugarAttr.ColumnDescription
                            : property.Name;

                        var value = property.GetValue(entity);

                        // 忽略空值的逻辑
                        if (ignoreNullValues && IsValueNullOrEmpty(value))
                            continue;

                        auditData[displayName] = value;
                    }
                }

                return JsonConvert.SerializeObject(auditData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "序列化审计数据失败");
                return $"序列化审计数据失败: {ex.Message}";
            }
        }

        // 辅助方法：判断值是否为null或空字符串
        private bool IsValueNullOrEmpty(object value)
        {
            if (value == null)
                return true;

            if (value is string strValue)
                return string.IsNullOrEmpty(strValue);

            return false;
        }

        // 定时刷新队列
        private void FlushQueue(object state)
        {
            try
            {
                FlushQueueAsync().Wait();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "定时刷新审计日志队列失败");
            }
        }

        // 异步刷新队列
        private async Task FlushQueueAsync()
        {
            // 使用互斥锁确保同一时间只有一个线程在刷新队列
            if (_isFlushing) return;

            lock (_lockObject)
            {
                if (_isFlushing) return;
                _isFlushing = true;
            }

            try
            {
                if (_auditLogQueue.IsEmpty) return;

                var logsToSave = new List<tb_AuditLogs>();
                int count = 0;

                // 从队列中取出最多BatchSize条日志
                while (count < _options.BatchSize && _auditLogQueue.TryDequeue(out var log))
                {
                    logsToSave.Add(log);
                    count++;
                }

                if (logsToSave.Any())
                {
                    try
                    {
                        // 批量写入数据库
                        await _appContext.Db.CopyNew()
                            .Insertable(logsToSave)
                            .ExecuteReturnSnowflakeIdListAsync();

                        _logger.LogDebug($"成功批量写入{logsToSave.Count}条审计日志");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "批量写入审计日志失败，将重新入队,错误日志" + ex.Message);

                        // 写入失败，将日志重新入队
                        foreach (var log in logsToSave)
                        {            //验证：一般是内容太长：
                            var validator = _AuditLogsController.Value.BaseValidator(log);
                            if (validator.IsValid)
                            {
                                _auditLogQueue.Enqueue(log);
                            }
                            else
                            {
                                _logger.LogError("日志入队时 没有通过验证。不再记录。", validator.Errors);
                            }

                        }
                    }
                }
            }
            finally
            {
                _isFlushing = false;
            }
        }

        public void Dispose()
        {
            _flushTimer?.Dispose();
            // 确保应用程序关闭前刷新所有日志
            FlushQueueAsync().Wait();
        }
    }

    public interface IFMAuditLogService
    {
        Task LogAsync<T>(string action, T entity, string description = "") where T : class;
        Task LogAsync(string action, string description = "");
    }

    // 审计日志帮助类（适配原有接口，逐步迁移到服务）
    public class FMAuditLogHelper
    {

        private readonly IFMAuditLogService _auditLogService;
        // 改为公共构造函数
        public FMAuditLogHelper(IFMAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        public async void CreateAuditLog<T>(string action, T entity, string description) where T : class
        {
            try
            {
                await _auditLogService.LogAsync(action, entity, description);
            }
            catch (Exception ex)
            {
                //
            }
        }

        public void CreateAuditLog<T>(string action, T entity) where T : class
        {
            CreateAuditLog(action, entity, "");
        }

        public async void CreateAuditLog(string action, string description)
        {
            try
            {
                await _auditLogService.LogAsync(action, description);
            }
            catch (Exception ex)
            {
                //
            }
        }

    }

    // 审计日志服务实现
    public class FMAuditLogService : IFMAuditLogService, IDisposable
    {
        private readonly ConcurrentQueue<tb_FM_AuditLogs> _auditLogQueue = new ConcurrentQueue<tb_FM_AuditLogs>();

        private readonly Lazy<BillConverterFactory> _billConverterFactory; // 缓存工厂
        private readonly Lazy<tb_FM_AuditLogsController<tb_FM_AuditLogs>> _AuditLogsController; // 缓存工厂
        private readonly Timer _flushTimer;
        private readonly AuditLogOptions _options;
        private readonly ILogger<FMAuditLogService> _logger;
        private readonly object _lockObject = new object();
        private bool _isFlushing = false;
        public ApplicationContext _appContext;

        private readonly Lazy<EnhancedBizTypeMapper> _mapper;


        public FMAuditLogService(IOptions<AuditLogOptions> options, ILogger<FMAuditLogService> logger, ApplicationContext appContext)
        {
            _appContext = appContext;
            _options = options.Value;
            _logger = logger;
            // 延迟解析依赖，直到第一次使用时才获取实例
            _billConverterFactory = new Lazy<BillConverterFactory>(
                () => appContext.GetRequiredService<BillConverterFactory>());// 缓存工厂


            _mapper = new Lazy<EnhancedBizTypeMapper>(
              () => _appContext.GetRequiredService<EnhancedBizTypeMapper>());// 缓存工厂



            _AuditLogsController = new Lazy<tb_FM_AuditLogsController<tb_FM_AuditLogs>>(() => appContext.GetRequiredService<tb_FM_AuditLogsController<tb_FM_AuditLogs>>());
            // 启动定时刷新
            _flushTimer = new Timer(FlushQueue, null, _options.FlushInterval, _options.FlushInterval);
        }

        public async Task LogAsync<T>(string action, T entity, string description = "") where T : class
        {
            if (!_options.EnableAudit) return;

            try
            {
                tb_FM_AuditLogs auditLog = CreateAuditLog(action, entity, description);
                _auditLogQueue.Enqueue(auditLog);

                // 检查队列是否达到批量大小
                if (_auditLogQueue.Count >= _options.BatchSize)
                {
                    await FlushQueueAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建财务审计日志失败");
            }
        }

        public async Task LogAsync(string action, string description = "")
        {
            if (!_options.EnableAudit) return;

            try
            {
                tb_FM_AuditLogs auditLog = CreateAuditLog(action, description);
                _auditLogQueue.Enqueue(auditLog);

                // 检查队列是否达到批量大小
                if (_auditLogQueue.Count >= _options.BatchSize)
                {
                    await FlushQueueAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建财务审计日志失败");
            }
        }

        private tb_FM_AuditLogs CreateAuditLog<T>(string action, T entity, string description) where T : class
        {
            if (_appContext.CurUserInfo == null)
                throw new InvalidOperationException("当前用户信息为空");

            tb_FM_AuditLogs auditLog = new tb_FM_AuditLogs
            {
                UserName = _appContext.CurUserInfo.UserInfo.UserName,
                Employee_ID = _appContext.CurUserInfo.UserInfo.Employee_ID,
                ActionType = action,
                ActionTime = DateTime.Now,
                Notes = description
            };

            try
            {
                //BizTypeMapper mapper = new BizTypeMapper();
                //var BizType = mapper.GetBizType(typeof(T).Name);

                //BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();
                //CommBillData cbd = bcf.GetBillData<T>(entity);
                // 直接使用缓存的工厂，避免重复解析
                // 使用时才解析依赖
                //var factory = _billConverterFactory.Value;
                //CommBillData cbd = factory.GetBillData<T>(entity);

                var bizType = _mapper.Value.GetBizType(typeof(T), entity);
                auditLog.ObjectType = (int)bizType;


                // 获取字段配置
                var (idField, noField) = _mapper.Value.GetEntityFieldValue<long>(typeof(T), entity);
                auditLog.ObjectId = idField;
                auditLog.ObjectNo = noField;
           
        
                var dataContent = EntityDataExtractor.ExtractDataContent(entity);
                auditLog.DataContent = dataContent;
                // 使用反射获取需要审计的字段
                //auditLog.DataContent = GetAuditDataContent(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取财务审计对象信息失败");
                auditLog.Notes += $" | 获取对象信息失败: {ex.Message}";
            }



            return auditLog;
        }

        private tb_FM_AuditLogs CreateAuditLog(string action, string description)
        {
            if (_appContext.CurUserInfo == null ||
                string.IsNullOrEmpty(_appContext.CurUserInfo.UserInfo.UserName))
                return null;

            tb_FM_AuditLogs auditLog = new tb_FM_AuditLogs
            {
                UserName = _appContext.CurUserInfo.UserInfo.UserName,
                Employee_ID = _appContext.CurUserInfo.UserInfo.Employee_ID,
                ActionType = action,
                ActionTime = DateTime.Now,
                Notes = description,
                ObjectType = -1
            };

            return auditLog;
        }

        // 使用反射和特性获取需要审计的字段
        private string GetAuditDataContent(object entity, bool ignoreNullValues = true)
        {
            if (entity == null) return string.Empty;

            try
            {
                var entityType = entity.GetType();
                var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var auditData = new Dictionary<string, object>();

                foreach (var property in properties)
                {
                    // 检查是否有AuditField特性
                    var auditAttr = property.GetCustomAttribute<AuditFieldAttribute>();
                    if (auditAttr != null && auditAttr.IsAudited)
                    {
                        var displayName = !string.IsNullOrEmpty(auditAttr.DisplayName)
                            ? auditAttr.DisplayName
                            : property.Name;

                        var value = property.GetValue(entity);

                        // 忽略空值的逻辑
                        if (ignoreNullValues && IsValueNullOrEmpty(value))
                            continue;

                        auditData[displayName] = value;
                    }

                    // 检查是否有SugarColumn特性且不是忽略列
                    var sugarAttr = property.GetCustomAttribute<SqlSugar.SugarColumn>();
                    if (sugarAttr != null && !sugarAttr.IsIgnore && auditAttr == null)
                    {
                        // 默认审计所有非忽略的SugarColumn字段
                        var displayName = !string.IsNullOrEmpty(sugarAttr.ColumnDescription)
                            ? sugarAttr.ColumnDescription
                            : property.Name;

                        var value = property.GetValue(entity);

                        // 忽略空值的逻辑
                        if (ignoreNullValues && IsValueNullOrEmpty(value))
                            continue;

                        auditData[displayName] = value;
                    }
                }

                return JsonConvert.SerializeObject(auditData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "序列化财务审计数据失败");
                return $"序列化财务审计数据失败: {ex.Message}";
            }
        }

        // 辅助方法：判断值是否为null或空字符串
        private bool IsValueNullOrEmpty(object value)
        {
            if (value == null)
                return true;

            if (value is string strValue)
                return string.IsNullOrEmpty(strValue);

            return false;
        }

        // 定时刷新队列
        private void FlushQueue(object state)
        {
            try
            {
                FlushQueueAsync().Wait();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "定时刷新财务审计日志队列失败");
            }
        }

        // 异步刷新队列
        private async Task FlushQueueAsync()
        {
            // 使用互斥锁确保同一时间只有一个线程在刷新队列
            if (_isFlushing) return;

            lock (_lockObject)
            {
                if (_isFlushing) return;
                _isFlushing = true;
            }

            try
            {
                if (_auditLogQueue.IsEmpty) return;

                var logsToSave = new List<tb_FM_AuditLogs>();
                int count = 0;

                // 从队列中取出最多BatchSize条日志
                while (count < _options.BatchSize && _auditLogQueue.TryDequeue(out var log))
                {
                    logsToSave.Add(log);
                    count++;
                }

                if (logsToSave.Any())
                {
                    try
                    {
                        // 批量写入数据库
                        await _appContext.Db.CopyNew()
                            .Insertable(logsToSave)
                            .ExecuteReturnSnowflakeIdListAsync();

                        _logger.LogDebug($"成功批量写入{logsToSave.Count}条财务审计日志");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "批量写入财务审计日志失败，将重新入队,错误日志" + ex.Message);

                        // 写入失败，将日志重新入队
                        foreach (var log in logsToSave)
                        {            //验证：一般是内容太长：
                            var validator = _AuditLogsController.Value.BaseValidator(log);
                            if (validator.IsValid)
                            {
                                _auditLogQueue.Enqueue(log);
                            }
                            else
                            {
                                _logger.LogError("财务日志入队时 没有通过验证。不再记录。", validator.Errors);
                            }

                        }
                    }
                }
            }
            finally
            {
                _isFlushing = false;
            }
        }

        public void Dispose()
        {
            _flushTimer?.Dispose();
            // 确保应用程序关闭前刷新所有日志
            FlushQueueAsync().Wait();
        }
    }



}
