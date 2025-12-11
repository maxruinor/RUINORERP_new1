using Microsoft.Extensions.Logging;
using RUINORERP.Business.CommService;
using RUINORERP.Common.LogHelper;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Base.StatusManager;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI;

namespace RUINORERP.Business.Document
{


    /// <summary>
    /// 单据操作管理器
    /// 负责管理单据的各种操作，包括转换、保存等
    /// </summary>
    public class ActionManager
    {
        private readonly DocumentConverterFactory _converterFactory;
        private readonly ISqlSugarClient _db;
        private readonly ILogger<ActionManager> _logger;
        private readonly Model.Base.StatusManager.IUnifiedStateManager _stateManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="converterFactory">单据转换器工厂</param>
        /// <param name="db">数据库客户端</param>
        /// <param name="logger">日志记录器</param>
        public ActionManager(DocumentConverterFactory converterFactory, ISqlSugarClient db, IUnifiedStateManager stateManager,
            ILogger<ActionManager> logger)
        {
            _converterFactory = converterFactory ?? throw new ArgumentNullException(nameof(converterFactory));
            _stateManager = stateManager;
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        /// 执行联动操作
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <param name="source">源单据实例</param>
        /// <param name="options">操作选项</param>
        /// <returns>操作结果</returns>
        public async Task<ActionResult<TTarget>> ExecuteActionAsync<TSource, TTarget>(
            TSource source, 
            ActionOptions options = null) 
            where TSource : BaseEntity, new() 
            where TTarget : BaseEntity, new()
        {
            try
            {
                options = options ?? new ActionOptions();
                
                // 验证源单据
                var validationResult = await _converterFactory.ValidateConversionAsync<TSource, TTarget>(source);
                if (!validationResult.CanConvert)
                {
                    // 使用FromValidationResult方法创建失败结果，保留所有验证信息
                    return ActionResult<TTarget>.FromValidationResult(validationResult);
                }

                // 获取转换器
                var converter = _converterFactory.GetConverter<TSource, TTarget>();
                if (converter == null)
                {
                    return ActionResult<TTarget>.Fail($"找不到从 {typeof(TSource).Name} 到 {typeof(TTarget).Name} 的转换器");
                }

                // 执行转换
                var target = await converter.ConvertAsync(source);
                if (target == null)
                {
                    return ActionResult<TTarget>.Fail("转换失败，目标单据为空");
                }

                // 保存目标单据（如果需要）
                if (options.SaveTarget)
                {
                    await SaveDocumentAsync(target);
                }

                // 使用FromValidationResult方法创建成功结果，保留所有验证信息
                return ActionResult<TTarget>.FromValidationResult(validationResult, target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"执行联动操作失败: {typeof(TSource).Name} -> {typeof(TTarget).Name}");
                return ActionResult<TTarget>.Fail($"执行失败: {ex.Message}");
            }
        }





        /// <summary>
        /// 保存单据到数据库
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
        /// 获取可用的联动操作列表
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
                SourceDocumentDisplayName = c.SourceDocumentDisplayName,
                TargetDocumentDisplayName = c.TargetDocumentDisplayName,
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
                    bool canReview = _stateManager.CanExecuteAction<BaseEntity>(source, Global.MenuItemEnums.审核);
                    if (canReview)
                    {
                        // 如果单据未审核，清空可转换操作列表
                        result.Clear();
                    }
                }
            }

            return result.ToList();
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






    }

    /// <summary>
    /// 联动选项类
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
        /// 转换上下文对象
        /// </summary>
        public object ConversionContext { get; set; }
    }

    /// <summary>
    /// 操作选项类
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
        /// 源单据显示名称（从Description特性获取）
        /// </summary>
        public string SourceDocumentDisplayName { get; set; }

        /// <summary>
        /// 目标单据显示名称（从Description特性获取）
        /// </summary>
        public string TargetDocumentDisplayName { get; set; }

        /// <summary>
        /// 转换器类型
        /// </summary>
        public Type ConverterType { get; set; }
    }

    /// <summary>
    /// 操作结果类
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
        /// 警告信息列表
        /// </summary>
        public List<string> WarningMessages { get; set; } = new List<string>();

        /// <summary>
        /// 信息提示列表
        /// </summary>
        public List<string> InfoMessages { get; set; } = new List<string>();

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static ActionResult<T> SuccessResult(T data) => new ActionResult<T>
        {
            Success = true,
            Data = data
        };

        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        public static ActionResult<T> Fail(string errorMessage) => new ActionResult<T>
        {
            Success = false,
            ErrorMessage = errorMessage
        };

        /// <summary>
        /// 从验证结果创建ActionResult
        /// </summary>
        /// <param name="validationResult">验证结果</param>
        /// <param name="data">结果数据</param>
        /// <returns>ActionResult实例</returns>
        public static ActionResult<T> FromValidationResult(ValidationResult validationResult, T data = null)
        {
            return new ActionResult<T>
            {
                Success = validationResult.CanConvert,
                ErrorMessage = validationResult.ErrorMessage,
                Data = data,
                WarningMessages = validationResult.WarningMessages?.ToList() ?? new List<string>(),
                InfoMessages = validationResult.InfoMessages?.ToList() ?? new List<string>()
            };
        }

        /// <summary>
        /// 获取所有消息
        /// </summary>
        /// <returns>所有消息列表</returns>
        public List<string> GetAllMessages()
        {
            var allMessages = new List<string>();
            
            if (!string.IsNullOrEmpty(ErrorMessage))
                allMessages.Add(ErrorMessage);
                
            if (WarningMessages != null && WarningMessages.Any())
                allMessages.AddRange(WarningMessages);
                
            if (InfoMessages != null && InfoMessages.Any())
                allMessages.AddRange(InfoMessages);
                
            return allMessages;
        }

        /// <summary>
        /// 获取格式化的消息字符串
        /// </summary>
        /// <returns>格式化的消息字符串</returns>
        public string GetFormattedMessages()
        {
            var allMessages = GetAllMessages();
            if (!allMessages.Any()) return string.Empty;
            
            return string.Join(Environment.NewLine, allMessages);
        }

        /// <summary>
        /// 是否有任何消息
        /// </summary>
        public bool HasMessages => !string.IsNullOrEmpty(ErrorMessage) || 
                                 (WarningMessages != null && WarningMessages.Any()) || 
                                 (InfoMessages != null && InfoMessages.Any());
    }
}