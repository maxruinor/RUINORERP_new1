using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RUINORERP.Common.LogHelper;
using RUINORERP.Model.Base;
using SqlSugar;
using RUINORERP.Model;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Business.CommService;

namespace RUINORERP.Business.Document
{
    /// <summary>
    /// 工作流步骤定义
    /// </summary>
    public class WorkflowStep
    {
        /// <summary>
        /// 步骤名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 步骤执行函数
        /// </summary>
        public Func<BaseEntity, BaseEntity, Task<bool>> ExecuteAsync { get; set; }
        
        /// <summary>
        /// 错误处理函数
        /// </summary>
        public Func<BaseEntity, BaseEntity, Exception, Task<bool>> ErrorHandlerAsync { get; set; }
        
        /// <summary>
        /// 是否为关键步骤
        /// </summary>
        public bool IsCritical { get; set; } = true;
        /// <summary>
        /// 执行单据联动操作（非泛型版本，供子类重写）
        /// </summary>
        /// <param name="actionId">操作ID</param>
        /// <param name="sourceDoc">源单据对象</param>
        /// <returns>操作结果</returns>
        public virtual async Task ExecuteActionAsync(string actionId, object sourceDoc)
        {
            // 默认实现，子类可以重写此方法
            throw new NotImplementedException("基类未实现此方法，请使用泛型版本或在子类中重写");
        }
    }

    /// <summary>
    /// 工作流定义
    /// </summary>
    public class ActionWorkflow
    {
        /// <summary>
        /// 工作流名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 工作流步骤列表
        /// </summary>
        public List<WorkflowStep> Steps { get; } = new List<WorkflowStep>();
        
        /// <summary>
        /// 添加步骤
        /// </summary>
        public ActionWorkflow AddStep(string name, Func<BaseEntity, BaseEntity, Task<bool>> executeAsync, 
            Func<BaseEntity, BaseEntity, Exception, Task<bool>> errorHandlerAsync = null, bool isCritical = true)
        {
            Steps.Add(new WorkflowStep
            {
                Name = name,
                ExecuteAsync = executeAsync,
                ErrorHandlerAsync = errorHandlerAsync,
                IsCritical = isCritical
            });
            return this;
        }
    }

    /// <summary>
    /// 联动操作管理器
    /// 负责协调单据联动的整个流程，包括事务处理和错误处理
    /// 增强功能：支持复杂工作流、重试机制、操作日志记录
    /// </summary>
    public class ActionManager
    {
        private readonly DocumentConverterFactory _converterFactory;
        private readonly ISqlSugarClient _db;
        private readonly ILogger<ActionManager> _logger;
        private readonly IAuditLogService _auditLogService;
        
        /// <summary>
        /// 默认重试次数
        /// </summary>
        private const int DefaultRetryCount = 3;
        
        /// <summary>
        /// 默认重试间隔(毫秒)
        /// </summary>
        private const int DefaultRetryIntervalMs = 1000;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="converterFactory">单据转换器工厂</param>
        /// <param name="db">数据库客户端</param>
        /// <param name="logger">日志记录器</param>
        public ActionManager(DocumentConverterFactory converterFactory, ISqlSugarClient db, ILogger<ActionManager> logger, IAuditLogService auditLogService)
        {
            _converterFactory = converterFactory ?? throw new ArgumentNullException(nameof(converterFactory));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        }

        /// <summary>
        /// 执行单据联动操作（非泛型版本，供子类重写）
        /// </summary>
        /// <param name="actionId">操作ID</param>
        /// <param name="sourceDoc">源单据对象</param>
        /// <returns>操作结果</returns>
        public virtual async Task ExecuteActionAsync(string actionId, object sourceDoc)
        {
            // 默认实现，子类可以重写此方法
            throw new NotImplementedException("基类未实现此方法，请使用泛型版本或在子类中重写");
        }

        /// <summary>
        /// 执行单据联动操作（增强版，支持重试）
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <param name="source">源单据对象</param>
        /// <param name="options">联动选项</param>
        /// <returns>联动结果</returns>
        public async Task<ActionResult<TTarget>> ExecuteActionAsync<TSource, TTarget>(
            TSource source,
            ActionOptions options = null)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            // 设置默认选项
            options ??= new ActionOptions();
            
            var operationId = Guid.NewGuid().ToString();
            var startTime = Stopwatch.GetTimestamp();
            _logger.LogInformation($"[{operationId}] 开始执行单据联动操作: {typeof(TSource).Name} -> {typeof(TTarget).Name}");
            
            // 记录操作开始日志
            await _auditLogService.LogAsync("DocumentAction:Start", source, $"操作ID:{operationId} - 开始执行联动操作");
            
            // 使用重试机制执行操作
            var result = await RetryAsync<TSource, TTarget>(async () => {
                return await ExecuteActionInternalAsync<TSource, TTarget>(source, options, operationId);
            }, options.RetryCount ?? DefaultRetryCount, options.RetryIntervalMs ?? DefaultRetryIntervalMs, operationId);
            
            var endTime = Stopwatch.GetTimestamp();
            var duration = (endTime - startTime) * 1000.0 / Stopwatch.Frequency;
            
            if (result.Success)
            {
                _logger.LogInformation($"[{operationId}] 单据联动操作执行成功，耗时: {duration:F2}ms");
                await _auditLogService.LogAsync("DocumentAction:Success", source, $"操作ID:{operationId} - 操作成功，耗时: {duration:F2}ms");
            }
            else
            {
                _logger.LogError($"[{operationId}] 单据联动操作执行失败: {result.ErrorMessage}");
                await _auditLogService.LogAsync("DocumentAction:Fail", source, $"操作ID:{operationId} - {result.ErrorMessage}");
            }
            
            return result;
        }

        /// <summary>
        /// 内部执行联动操作
        /// </summary>
        private async Task<ActionResult<TTarget>> ExecuteActionInternalAsync<TSource, TTarget>(
            TSource source,
            ActionOptions options,
            string operationId)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            try
            {
                // 验证转换条件
                _logger.LogDebug($"[{operationId}] 验证转换条件");
                var validationResult = await _converterFactory.ValidateConversionAsync<TSource, TTarget>(source);
                if (!validationResult.CanConvert)
                {
                    _logger.LogWarning($"[{operationId}] 转换条件验证失败: {validationResult.ErrorMessage}");
                    return ActionResult<TTarget>.Fail(validationResult.ErrorMessage);
                }
                
                TTarget target = null;
                
                // 开始事务
                if (options.UseTransaction)
                {
                    await _db.Ado.BeginTranAsync();
                    try
                    {
                        // 执行转换
                        target = await ExecuteConversionWithWorkflowAsync<TSource, TTarget>(source, options, operationId);
                        
                        // 保存目标单据
                        if (options.SaveTarget && target != null)
                        {
                            await SaveDocumentAsync(target);
                            _logger.LogDebug($"[{operationId}] 目标单据已保存");
                        }
                        
                        // 提交事务
                        await _db.Ado.CommitTranAsync();
                        _logger.LogDebug($"[{operationId}] 事务提交成功");
                    }
                    catch (Exception ex)
                    {
                        // 回滚事务
                        await _db.Ado.RollbackTranAsync();
                        _logger.LogError(ex, $"[{operationId}] 单据联动事务失败");
                        return ActionResult<TTarget>.Fail($"事务处理失败: {ex.Message}");
                    }
                }
                else
                {
                    // 无事务执行
                    target = await ExecuteConversionWithWorkflowAsync<TSource, TTarget>(source, options, operationId);
                    
                    if (options.SaveTarget && target != null)
                    {
                        await SaveDocumentAsync(target);
                        _logger.LogDebug($"[{operationId}] 目标单据已保存");
                    }
                }
                
                // 执行后置操作
                if (options.PostAction != null && target != null)
                {
                    _logger.LogDebug($"[{operationId}] 执行后置操作");
                    await options.PostAction(source, target);
                }
                
                return ActionResult<TTarget>.SuccessResult(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{operationId}] 单据联动执行失败");
                return ActionResult<TTarget>.Fail($"执行失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 执行带工作流的转换
        /// </summary>
        private async Task<TTarget> ExecuteConversionWithWorkflowAsync<TSource, TTarget>(
            TSource source,
            ActionOptions options,
            string operationId)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            // 如果指定了自定义工作流
            if (options.Workflow != null && options.Workflow.Steps.Any())
            {
                _logger.LogDebug($"[{operationId}] 使用自定义工作流执行转换，步骤数: {options.Workflow.Steps.Count}");
                
                // 创建目标单据
                var target = new TTarget();
                
                // 执行工作流步骤
                for (int i = 0; i < options.Workflow.Steps.Count; i++)
                {
                    var step = options.Workflow.Steps[i];
                    _logger.LogDebug($"[{operationId}] 执行工作流步骤 {i+1}/{options.Workflow.Steps.Count}: {step.Name}");
                    
                    try
                    {
                        bool success = await step.ExecuteAsync(source, target);
                        
                        if (!success)
                        {
                            _logger.LogWarning($"[{operationId}] 工作流步骤 {step.Name} 执行失败");
                            throw new InvalidOperationException($"工作流步骤 '{step.Name}' 执行失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"[{operationId}] 工作流步骤 {step.Name} 异常");
                        
                        // 如果有错误处理函数，尝试处理错误
                        if (step.ErrorHandlerAsync != null)
                        {
                            bool handled = await step.ErrorHandlerAsync(source, target, ex);
                            if (handled)
                            {
                                _logger.LogWarning($"[{operationId}] 工作流步骤 {step.Name} 错误已处理");
                                continue;
                            }
                        }
                        
                        // 如果是关键步骤，抛出异常中断执行
                        if (step.IsCritical)
                        {
                            throw new InvalidOperationException($"关键工作流步骤 '{step.Name}' 执行失败", ex);
                        }
                        else
                        {
                            _logger.LogWarning($"[{operationId}] 非关键工作流步骤 {step.Name} 执行失败，继续执行");
                        }
                    }
                }
                
                return target;
            }
            else
            {
                // 使用默认转换
                _logger.LogDebug($"[{operationId}] 使用默认转换器执行转换");
                return await _converterFactory.ConvertAsync<TSource, TTarget>(source, options.ConversionContext);
            }
        }

        /// <summary>
        /// 保存单据到数据库（增强版）
        /// </summary>
        /// <typeparam name="T">单据类型</typeparam>
        /// <param name="document">单据对象</param>
        private async Task SaveDocumentAsync<T>(T document) where T : BaseEntity, new()
        {
            if (document.PrimaryKeyID > 0)
            {
                // 更新操作
                await _db.Updateable(document).ExecuteCommandAsync();
                _logger.LogDebug($"已更新单据: {typeof(T).Name}, ID: {document.PrimaryKeyID}");
            }
            else
            {
                // 新增操作
                await _db.Insertable(document).ExecuteCommandAsync();
                _logger.LogDebug($"已新增单据: {typeof(T).Name}");
            }
        }

        /// <summary>
        /// 获取可用的联动操作列表（增强版）
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <param name="source">源单据实例（可选）</param>
        /// <returns>可用操作列表</returns>
        public List<ActionOption> GetAvailableActions<TSource>(TSource source = null) where TSource : BaseEntity
        {
            var conversions = _converterFactory.GetAvailableConversions<TSource>();
            var result = conversions.ConvertAll(c => new ActionOption
            {
                DisplayName = c.DisplayName,
                ActionType = "Convert",
                TargetType = c.TargetDocumentType,
                SourceType = c.SourceDocumentType,
                Priority = c.Priority,
                ConverterType = c.ConverterType
            });
            
            // 如果提供了源单据实例，可以根据单据状态过滤操作
            if (source != null)
            {
                // 这里可以添加基于单据状态的过滤逻辑
                // 例如：已审核的单据不能再次转换等
                _logger.LogDebug($"根据单据状态过滤可用操作，源单据类型: {typeof(TSource).Name}, ID: {source.PrimaryKeyID}");
                
                // 添加基于状态的过滤逻辑
                // 只有已审核的单据才能进行转换操作
                if (source is BaseEntity)
                {
                    // 假设Status属性表示单据状态，1表示已审核
                    var status = GetPropertyValue<int>(source, "Status");
                    if (status != 1)
                    {
                        // 如果单据未审核，清空可转换操作列表
                        result.Clear();
                    }
                }
            }
            
            return result.OrderByDescending(o => o.Priority).ToList();
        }

        /// <summary>
        /// 安全获取属性值
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值</returns>
        private T GetPropertyValue<T>(object obj, string propertyName)
        {
            if (obj == null) return default(T);
            
            var property = obj.GetType().GetProperty(propertyName);
            if (property == null) return default(T);
            
            try
            {
                var value = property.GetValue(obj);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 重试执行异步操作
        /// </summary>
        private async Task<ActionResult<TTarget>> RetryAsync<TSource, TTarget>(
            Func<Task<ActionResult<TTarget>>> operation,
            int retryCount,
            int retryIntervalMs,
            string operationId)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            int attempt = 0;
            Exception lastException = null;
            
            while (attempt <= retryCount)
            {
                attempt++;
                
                try
                {
                    if (attempt > 1)
                    {
                        _logger.LogWarning($"[{operationId}] 尝试重试操作，第 {attempt}/{retryCount+1} 次");
                        // 等待重试间隔
                        await Task.Delay(retryIntervalMs);
                    }
                    
                    var result = await operation();
                    if (result.Success)
                    {
                        return result;
                    }
                    
                    // 如果操作返回失败但不是异常，记录错误并继续重试
                    lastException = new InvalidOperationException(result.ErrorMessage);
                    _logger.LogWarning($"[{operationId}] 操作尝试失败: {result.ErrorMessage}");
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    _logger.LogWarning(ex, $"[{operationId}] 操作尝试异常");
                }
            }
            
            // 所有重试都失败
            string errorMessage = lastException != null 
                ? $"操作失败，已重试 {retryCount} 次: {lastException.Message}"
                : $"操作失败，已重试 {retryCount} 次";
            
            _logger.LogError(lastException, $"[{operationId}] 所有重试都失败");
            return ActionResult<TTarget>.Fail(errorMessage);
        }



        /// <summary>
        /// 创建默认工作流
        /// </summary>
        public ActionWorkflow CreateDefaultWorkflow<TSource, TTarget>()
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            return new ActionWorkflow
            {
                Name = $"默认工作流: {typeof(TSource).Name} -> {typeof(TTarget).Name}"
            }
            .AddStep("验证源单据", async (source, target) => {
                // 验证源单据状态等
                _logger.LogDebug($"验证源单据: {source.GetType().Name}, ID: {source.PrimaryKeyID}");
                return true;
            })
            .AddStep("执行转换", async (source, target) => {
                // 使用转换器执行转换
                var converter = _converterFactory.GetConverter<TSource, TTarget>();
                var result = await converter.ConvertAsync((TSource)source);
                
                // 复制属性到目标对象
                var properties = typeof(TTarget).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite);
                
                foreach (var property in properties)
                {
                    try
                    {
                        var value = property.GetValue(result);
                        property.SetValue(target, value);
                    }
                    catch { /* 忽略无法复制的属性 */ }
                }
                
                return true;
            })
            .AddStep("验证目标单据", async (source, target) => {
                // 验证转换后的目标单据
                _logger.LogDebug($"验证目标单据: {target.GetType().Name}");
                return true;
            });
        }
    }
    
    /// <summary>
    /// 联动选项类（增强版）
    /// </summary>
    public class ActionOptions
    {
        /// <summary>
        /// 是否使用事务
        /// </summary>
        public bool UseTransaction { get; set; } = true;
        
        /// <summary>
        /// 是否保存目标单据
        /// </summary>
        public bool SaveTarget { get; set; } = true;
        
        /// <summary>
        /// 后置操作委托
        /// </summary>
        public Func<BaseEntity, BaseEntity, Task> PostAction { get; set; }
        
        /// <summary>
        /// 转换上下文对象
        /// </summary>
        public object ConversionContext { get; set; }
        
        /// <summary>
        /// 工作流定义
        /// </summary>
        public ActionWorkflow Workflow { get; set; }
        
        /// <summary>
        /// 重试次数
        /// </summary>
        public int? RetryCount { get; set; }
        
        /// <summary>
        /// 重试间隔(毫秒)
        /// </summary>
        public int? RetryIntervalMs { get; set; }
        
        /// <summary>
        /// 是否启用详细日志
        /// </summary>
        public bool EnableDetailedLogging { get; set; } = false;
    }
    
    /// <summary>
    /// 操作选项类（增强版）
    /// </summary>
    public class ActionOption
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// 操作类型
        /// </summary>
        public string ActionType { get; set; }
        
        /// <summary>
        /// 源单据类型
        /// </summary>
        public string SourceType { get; set; }
        
        /// <summary>
        /// 目标单据类型
        /// </summary>
        public string TargetType { get; set; }
        
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; } = 100;
        
        /// <summary>
        /// 转换器类型
        /// </summary>
        public Type ConverterType { get; set; }
    }
    
    /// <summary>
    /// 操作结果类（增强版）
    /// </summary>
    /// <typeparam name="T">结果数据类型</typeparam>
    public class ActionResult<T> where T : class
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// 结果数据
        /// </summary>
        public T Data { get; set; }
        
        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public double ExecutionTimeMs { get; set; }
        
        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; }
        
        /// <summary>
        /// 操作ID
        /// </summary>
        public string OperationId { get; set; }
        
        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static ActionResult<T> SuccessResult(T data) => new ActionResult<T>
        {
            Success = true,
            Data = data,
            OperationId = Guid.NewGuid().ToString()
        };
        
        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        public static ActionResult<T> Fail(string errorMessage) => new ActionResult<T>
        {
            Success = false,
            ErrorMessage = errorMessage,
            OperationId = Guid.NewGuid().ToString()
        };
    }
}