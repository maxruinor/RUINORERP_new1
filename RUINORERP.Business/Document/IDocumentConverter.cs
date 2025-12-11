using RUINORERP.Model;
using RUINORERP.Model.Base;
using System;
using System.Threading.Tasks;

namespace RUINORERP.Business.Document
{
    /// <summary>
    /// 单据转换器接口
    /// 定义不同类型单据之间转换的标准方法
    /// </summary>
    /// <typeparam name="TSource">源单据类型</typeparam>
    /// <typeparam name="TTarget">目标单据类型</typeparam>
    public interface IDocumentConverter<TSource, TTarget>
        where TSource : BaseEntity
        where TTarget : BaseEntity, new()
    {
        /// <summary>
        /// 将源单据转换为目标单据
        /// </summary>
        /// <param name="source">源单据对象</param>
        /// <returns>转换后的目标单据对象</returns>
        Task<TTarget> ConvertAsync(TSource source);

        /// <summary>
        /// 验证是否可以进行转换
        /// </summary>
        /// <param name="source">源单据对象</param>
        /// <returns>验证结果，包含是否可以转换及错误信息</returns>
        Task<ValidationResult> ValidateConversionAsync(TSource source);

        /// <summary>
        /// 获取源单据类型名称
        /// </summary>
        string SourceDocumentTypeName { get; }

        /// <summary>
        /// 获取目标单据类型名称
        /// </summary>
        string TargetDocumentTypeName { get; }

        /// <summary>
        /// 获取源单据类型
        /// </summary>
        Type SourceDocumentType { get; }

        /// <summary>
        /// 获取目标单据类型
        /// </summary>
        Type TargetDocumentType { get; }

        /// <summary>
        /// 获取转换操作的显示名称
        /// </summary>
        string DisplayName { get; }
    }

    /// <summary>
    /// 转换验证结果类
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// 是否可以进行转换
        /// </summary>
        public bool CanConvert { get; set; }

        /// <summary>
        /// 验证错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ValidationResult() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="canConvert">是否可以转换</param>
        /// <param name="errorMessage">错误信息</param>
        public ValidationResult(bool canConvert, string errorMessage = null)
        {
            CanConvert = canConvert;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 创建成功的验证结果
        /// </summary>
        public static ValidationResult Success => new ValidationResult(true);

        /// <summary>
        /// 创建失败的验证结果
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        public static ValidationResult Fail(string errorMessage) =>
            new ValidationResult(false, errorMessage);
    }
}