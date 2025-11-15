using System;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Model.Base;

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
            if (target.GetType().GetProperty("Status") != null)
            {
                target.GetType().GetProperty("Status").SetValue(target, "待审核");
            }
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
        public virtual string SourceDocumentType => typeof(TSource).Name;

        /// <summary>
        /// 获取目标单据类型名称
        /// </summary>
        public virtual string TargetDocumentType => typeof(TTarget).Name;

        /// <summary>
        /// 获取转换操作的显示名称
        /// </summary>
        public virtual string DisplayName => $"{SourceDocumentType}转{TargetDocumentType}";
    }
}