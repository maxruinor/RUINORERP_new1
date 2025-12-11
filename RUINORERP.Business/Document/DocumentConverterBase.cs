using System;
using System.Reflection;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace RUINORERP.Business.Document
{
    /// <summary>
    /// 单据转换器基类
    /// 提供通用的单据转换功能实现
    /// </summary>
    /// <typeparam name="TSource">源单据类型</typeparam>
    /// <typeparam name="TTarget">目标单据类型</typeparam>
    public abstract class DocumentConverterBase<TSource, TTarget> : IDocumentConverter<TSource, TTarget>
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
        /// </summary>
        public virtual Task<ValidationResult> ValidateConversionAsync(TSource source)
        {
            return Task.FromResult(ValidationResult.Success);
        }

        /// <summary>
        /// 执行单据转换
        /// </summary>
        /// <param name="source">源单据对象</param>
        /// <returns>转换后的目标单据对象</returns>
        public async Task<TTarget> ConvertAsync(TSource source)
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
        /// </summary>
        public virtual string DisplayName
        {
            get
            {
                try
                {
                    return $"{SourceDocumentDisplayName}转{TargetDocumentDisplayName}";
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "获取转换操作显示名称失败，使用默认格式");
                    return $"{typeof(TSource).Name}转{typeof(TTarget).Name}";
                }
            }
        }
    }
}