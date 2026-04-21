using System;
using System.Reflection;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using RUINORERP.Business.Cache;

namespace RUINORERP.Business.Document
{
    /// <summary>
    /// 单据转换器基类
    /// 提供通用的单据转换功能实现
    /// </summary>
    /// <typeparam name="TSource">源单据类型</typeparam>
    /// <typeparam name="TTarget">目标单据类型</typeparam>
    public abstract class DocumentConverterBase<TSource, TTarget> : IDocumentConverter<TSource, TTarget>, IConverterMeta
        where TSource : BaseEntity
        where TTarget : BaseEntity, new()
    {

        
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected readonly ILogger _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        protected DocumentConverterBase(ILogger<DocumentConverterBase<TSource, TTarget>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// 默认验证通过
        /// 增强版：结合统一状态管理体系进行校验
        /// </summary>
        public virtual async Task<ValidationResult> ValidateConversionAsync(TSource source)
        {
            // 1. 基础业务验证（子类重写）- 优先级最高
            var businessValidation = await OnValidateBusinessRulesAsync(source);
            if (!businessValidation.CanConvert)
            {
                return businessValidation;
            }

            // 2. 状态管理验证（如果源单据支持状态管理）
            if (source is BaseEntity baseEntity && baseEntity.StateManager != null)
            {
                try
                {
                    // 询问状态管理器：在当前状态下，是否允许执行“联动”动作？
                    var canExecute = baseEntity.StateManager.CanExecuteActionWithMessage(baseEntity, Global.MenuItemEnums.联动);
                    
                    if (!canExecute.CanExecute)
                    {
                        // 记录日志但不一定直接返回失败，因为子类可能已经处理了更细致的逻辑
                        _logger.LogDebug("状态管理器建议禁止转换: {Message}", canExecute.Message);
                        
                        // 只有当状态管理器给出明确的“终态”或“锁定”提示时，才在基类层面拦截
                        if (canExecute.Message.Contains("终态") || canExecute.Message.Contains("锁定"))
                        {
                            return ValidationResult.Fail(canExecute.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "状态管理验证异常，回退到仅业务验证");
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// 子类重写此方法以实现具体的业务规则验证（如金额、必填项等）
        /// </summary>
        protected virtual Task<ValidationResult> OnValidateBusinessRulesAsync(TSource source)
        {
            return Task.FromResult(ValidationResult.Success);
        }

        /// <summary>
        /// 执行单据转换
        /// 虚方法，允许子类重写以实现自定义转换逻辑
        /// </summary>
        /// <param name="source">源单据对象</param>
        /// <returns>转换后的目标单据对象</returns>
        public virtual async Task<TTarget> ConvertAsync(TSource source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 创建目标单据实例
            var target = new TTarget();

            // 复制基础字段
            CopyBaseFields(source, target);

            // 执行具体转换逻辑
            await PerformConversionAsync(source, target);
            // 初始化实体的创建时间
            BusinessHelper.Instance.InitEntity(target);
            return target;
        }

        /// <summary>
        /// 复制基础字段
        /// </summary>
        /// <param name="source">源单据对象</param>
        /// <param name="target">目标单据对象</param>
        protected virtual void CopyBaseFields(TSource source, TTarget target)
        {
            // 设置创建时间（如果属性存在）
            if (target.GetType().GetProperty("CreatedTime") != null)
            {
                target.GetType().GetProperty("CreatedTime").SetValue(target, DateTime.Now);
            }
            // 设置审核状态为待审核（如果属性存在）
            //if (target.GetType().GetProperty("Status") != null)
            //{
            //    target.GetType().GetProperty("Status").SetValue(target, "待审核");
            //}
        }

        /// <summary>
        /// 执行具体转换逻辑
        /// 由子类实现
        /// </summary>
        /// <param name="source">源单据对象</param>
        /// <param name="target">目标单据对象</param>
        protected abstract Task PerformConversionAsync(TSource source, TTarget target);

        /// <summary>
        /// 获取源单据类型名称
        /// </summary>
        public virtual string SourceDocumentTypeName => typeof(TSource).Name;

        /// <summary>
        /// 获取目标单据类型名称
        /// </summary>
        public virtual string TargetDocumentTypeName => typeof(TTarget).Name;

        /// <summary>
        /// 获取实体的显示名称（从Description特性获取）
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>实体显示名称，如果未找到Description特性则返回类型名称</returns>
        protected virtual string GetEntityDisplayName(Type entityType)
        {
            try
            {
                if (entityType == null)
                {
                    _logger.LogWarning("实体类型为空");
                    return "未知实体";
                }

                // 获取Description特性
                var descriptionAttr = entityType.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttr != null && !string.IsNullOrEmpty(descriptionAttr.Description))
                {
                    return descriptionAttr.Description;
                }

                // 如果没有Description特性或Description为空，返回类型名称
                _logger.LogDebug("实体 {EntityType} 未找到Description特性或Description为空，使用类型名称", entityType.Name);
                return entityType.Name;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取实体 {EntityType} 显示名称失败，使用类型名称", entityType.Name);
                return entityType.Name;
            }
        }

        /// <summary>
        /// 获取源单据显示名称（从Description特性获取）
        /// </summary>
        public virtual string SourceDocumentDisplayName => GetEntityDisplayName(typeof(TSource));

        /// <summary>
        /// 获取目标单据显示名称（从Description特性获取）
        /// </summary>
        public virtual string TargetDocumentDisplayName => GetEntityDisplayName(typeof(TTarget));


        /// <summary>
        /// 获取源单据类型
        /// </summary>
        public virtual Type SourceDocumentType => typeof(TSource);

        /// <summary>
        /// 获取目标单据类型
        /// </summary>
        public virtual Type TargetDocumentType => typeof(TTarget);

        /// <summary>
        /// 获取转换操作的显示名称
        /// 
        /// 设计原则:
        /// - 基类默认返回 null,表示"未指定",由工厂层根据实例数据动态生成
        /// - 子类可以重写此属性,返回固定的业务文本(如"退还余款")
        /// 
        /// 重要说明:
        /// - 基类无法访问源单据实例数据(如 ReceivePaymentType),因此不尝试动态生成
        /// - 动态显示名称由 DocumentConverterFactory 根据实例数据计算
        /// - UI 层应使用工厂返回的 ConversionOption.DisplayName,而非直接访问此属性
        /// 
        /// 使用示例:
        ///   // 场景1:需要固定文本(不随单据类型变化) - 重写
        ///   public override string DisplayName => "订单取消作废";
        ///   
        ///   // 场景2:需要动态文本(随ReceivePaymentType变化) - 不重写,返回null
        ///   // 工厂层会根据实例的 ReceivePaymentType 动态生成"收款"或"付款"
        /// </summary>
        public virtual string DisplayName => null;

        /// <summary>
        /// 获取转换唯一标识符
        /// 默认为 null，子类可以根据业务逻辑重写（如 "Normal", "Refund", "Offset"）
        /// 
        /// 重要说明:
        /// - 返回 null 表示不使用标识符区分,工厂层会使用 "Source:Target" 作为Key
        /// - 返回非空字符串时,工厂层会使用 "Source:Target:Identifier" 作为Key
        /// - 子类应直接重写此属性,而不是重写 GetConversionIdentifier() 方法
        /// </summary>
        public virtual string ConversionIdentifier => null;
        
        /// <summary>
        /// 获取转换操作类型（单据生成型或动作操作型）
        /// 默认为单据生成型
        /// </summary>
        public virtual DocumentConversionType ConversionType => DocumentConversionType.DocumentGeneration;

        /// <summary>
        /// 执行动作操作
        /// 对于动作操作型转换，执行具体的业务操作
        /// 默认实现抛出 NotImplementedException，子类需要重写
        /// </summary>
        /// <param name="source">源单据</param>
        /// <param name="target">目标单据（可选，某些操作需要）</param>
        /// <returns>操作结果</returns>
        public virtual Task<ActionResult> ExecuteActionOperationAsync(TSource source, TTarget target = null)
        {
            throw new NotImplementedException($"转换器 {GetType().Name} 未实现 ExecuteActionOperationAsync 方法");
        }

    }
}
