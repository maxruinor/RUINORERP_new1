using FastReport.DevComponents.DotNetBar;
using LiveChartsCore.Geo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.BaseForm;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Monitoring.Auditing
{
    public class AuditLogHelper_old
    {
        private static AuditLogHelper_old m_instance;

        public static AuditLogHelper_old Instance
        {
            get
            {
                if (m_instance == null)
                {
                    Initialize();
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }


        /// <summary>
        /// 对象实例化
        /// </summary>
        public static void Initialize()
        {
            m_instance = new AuditLogHelper_old();
        }
        public async void CreateAuditLog<T>(string action, T entity, string description) where T : class
        {

            //将操作记录保存到数据库中
            tb_AuditLogs auditLog = new tb_AuditLogs();
            auditLog.UserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;
            auditLog.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
            auditLog.ActionType = action;

            BizTypeMapper mapper = new BizTypeMapper();
            //var BizTypeText = mapper.GetBizType(typeof(T).Name).ToString();
            var BizType = mapper.GetBizType(typeof(T).Name);
            try
            {
                BillConverterFactory bcf = MainForm.Instance.AppContext.GetRequiredService<BillConverterFactory>();
                CommBillData cbd = bcf.GetBillData<T>(entity);
                auditLog.ObjectType = (int)BizType;
                //string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
                //long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
                //auditLog.ObjectId = pkid;
                auditLog.ObjectId = cbd.BillID;
                auditLog.ObjectNo = cbd.BillNo;
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"审计日志保存记录时失败:{ex.Message}", Global.UILogType.错误);
            }

            if (description.IsNotEmptyOrNull())
            {
                auditLog.Notes = description;
            }
            auditLog.ActionTime = DateTime.Now;
            try
            {
                // 延迟执行日志插入操作 防止死锁？
                await Task.Delay(150); // 延迟100毫秒
                await MainForm.Instance.AppContext.Db.CopyNew().Insertable<tb_AuditLogs>(auditLog).ExecuteReturnEntityAsync();
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"审计日志{GetAuditLogProperties(auditLog)}记录失败:{ex.Message}", Global.UILogType.错误);
            }

        }

        public void CreateAuditLog<T>(string action, T entity) where T : class
        {
            CreateAuditLog<T>(action, entity, "");
        }
        //创建一个审计功能的方法，将单据的操作记录下来
        public async void CreateAuditLog(string action, string description)
        {

            //将操作记录保存到数据库中
            //BizTypeMapper mapper = new BizTypeMapper();
            ////var BizTypeText = mapper.GetBizType(typeof(T).Name).ToString();
            //var BizType = mapper.GetBizType(typeof(T).Name);

            BillConverterFactory bcf = MainForm.Instance.AppContext.GetRequiredService<BillConverterFactory>();

            //将操作记录保存到数据库中
            tb_AuditLogs auditLog = new tb_AuditLogs();
            if (MainForm.Instance.AppContext.CurUserInfo == null)
            {
                return;
            }
            auditLog.UserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;
            if (auditLog.UserName == null)
            {
                return;
            }
            auditLog.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
            auditLog.ActionType = action;
            auditLog.ObjectType = -1;// (int)BizType;

            if (description.IsNotEmptyOrNull())
            {
                auditLog.Notes = description;
            }
            auditLog.ActionTime = DateTime.Now;
            try
            {
                await MainForm.Instance.AppContext.Db.CopyNew().Insertable<tb_AuditLogs>(auditLog).ExecuteReturnEntityAsync();
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"审计日志{GetAuditLogProperties(auditLog)}记录失败:{ex.Message}", Global.UILogType.错误);
            }

        }


        public string GetAuditLogProperties(tb_AuditLogs auditLog)
        {
            string rs = string.Empty;
            if (auditLog == null)
            {
                return rs;
            }
            PropertyInfo[] properties = auditLog.GetType().GetProperties();
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo property in properties)
            {
                foreach (Attribute attr in property.GetCustomAttributes(true))
                {
                    if (attr is SqlSugar.SugarColumn entityAttr)
                    {
                        if (null != entityAttr)
                        {
                            try
                            {
                                var value = property.GetValue(auditLog);
                                sb.Append($"{property.Name}: {value}");
                            }
                            catch (Exception ex)
                            {
                                MainForm.Instance.logger.LogError("GetAuditLogProperties。", ex);
                            }
                        }
                    }
                }
            }
            rs = sb.ToString();
            return rs;
        }

    }



    /*
      private readonly IAuditLogService _auditLogService;

    public ProductController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    // 记录审计日志
            await _auditLogService.LogAsync("创建产品", product, "管理员创建了新产品");

     */

    // 审计日志帮助类（适配原有接口，逐步迁移到服务）
    public class AuditLogHelper
    {
        private readonly IAuditLogService _auditLogService;
        // 改为公共构造函数
        public AuditLogHelper(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }


        //public static AuditLogHelper Instance => _lazyInstance.Value;
        //private static readonly Lazy<AuditLogHelper> _lazyInstance =
        // new Lazy<AuditLogHelper>(() => new AuditLogHelper());
        //private AuditLogHelper()
        //{
        //    _auditLogService = MainForm.Instance.AppContext.GetRequiredService<IAuditLogService>();
        //}

        public async void CreateAuditLog<T>(string action, T entity, string description) where T : class
        {
            try
            {
                await _auditLogService.LogAsync(action, entity, description);
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"审计日志记录失败:{ex.Message}", Global.UILogType.错误);
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
                MainForm.Instance.uclog.AddLog($"审计日志记录失败:{ex.Message}", Global.UILogType.错误);
            }
        }



    }


    // 审计日志配置类
    public class AuditLogOptions
    {
        public int BatchSize { get; set; } = 5; // 批量写入大小
        public int FlushInterval { get; set; } = 3000; // 自动刷新间隔(毫秒)
        public bool EnableAudit { get; set; } = true; // 是否启用审计
    }

    // 审计字段特性
    [AttributeUsage(AttributeTargets.Property)]
    public class AuditFieldAttribute : Attribute
    {
        public bool IsAudited { get; set; } = true;
        public string DisplayName { get; set; }
    }

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

        private readonly Lazy<BillConverterFactory> _billConverterFactory; // 缓存工厂
        private readonly Timer _flushTimer;
        private readonly AuditLogOptions _options;
        private readonly ILogger<AuditLogService> _logger;
        private readonly object _lockObject = new object();
        private bool _isFlushing = false;

        public AuditLogService(IOptions<AuditLogOptions> options, ILogger<AuditLogService> logger)
        {
            _options = options.Value;
            _logger = logger;
            // 延迟解析依赖，直到第一次使用时才获取实例
            _billConverterFactory = new Lazy<BillConverterFactory>(
                () => MainForm.Instance.AppContext.GetRequiredService<BillConverterFactory>());// 缓存工厂
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
            if (MainForm.Instance.AppContext.CurUserInfo == null)
                throw new InvalidOperationException("当前用户信息为空");

            tb_AuditLogs auditLog = new tb_AuditLogs
            {
                UserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName,
                Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID,
                ActionType = action,
                ActionTime = DateTime.Now,
                Notes = description
            };

            try
            {
                BizTypeMapper mapper = new BizTypeMapper();
                var BizType = mapper.GetBizType(typeof(T).Name);

                //BillConverterFactory bcf = MainForm.Instance.AppContext.GetRequiredService<BillConverterFactory>();
                //CommBillData cbd = bcf.GetBillData<T>(entity);
                // 直接使用缓存的工厂，避免重复解析
                // 使用时才解析依赖
                var factory = _billConverterFactory.Value;

                CommBillData cbd = factory.GetBillData<T>(entity);


                auditLog.ObjectType = (int)BizType;
                auditLog.ObjectId = cbd.BillID;
                auditLog.ObjectNo = cbd.BillNo;
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
            if (MainForm.Instance.AppContext.CurUserInfo == null ||
                string.IsNullOrEmpty(MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName))
                return null;

            tb_AuditLogs auditLog = new tb_AuditLogs
            {
                UserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName,
                Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID,
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
                        await MainForm.Instance.AppContext.Db.CopyNew()
                            .Insertable(logsToSave)
                            .ExecuteReturnSnowflakeIdListAsync();

                        _logger.LogDebug($"成功批量写入{logsToSave.Count}条审计日志");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "批量写入审计日志失败，将重新入队");

                        // 写入失败，将日志重新入队
                        foreach (var log in logsToSave)
                        {
                            _auditLogQueue.Enqueue(log);
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
