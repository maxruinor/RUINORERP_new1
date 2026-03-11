using RUINORERP.Model;
using RUINORERP.Model.Base;
using System;
using System.Collections.Generic;
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
        /// 获取源单据显示名称（从Description特性获取）
        /// </summary>
        string SourceDocumentDisplayName { get; }

        /// <summary>
        /// 获取目标单据显示名称（从Description特性获取）
        /// </summary>
        string TargetDocumentDisplayName { get; }

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
        
        /// <summary>
        /// 获取转换操作类型（单据生成型或动作操作型）
        /// </summary>
        DocumentConversionType ConversionType { get; }

        /// <summary>
        /// 执行动作操作
        /// 对于动作操作型转换，执行具体的业务操作
        /// </summary>
        /// <param name="source">源单据</param>
        /// <param name="target">目标单据（可选，某些操作需要）</param>
        /// <returns>操作结果</returns>
        Task<ActionResult> ExecuteActionOperationAsync(TSource source, TTarget target = null);
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
        /// 是否需要用户确认
        /// </summary>
        public bool RequiresUserConfirmation { get; set; }

        /// <summary>
        /// 用户确认提示信息
        /// </summary>
        public string ConfirmationMessage { get; set; }

        /// <summary>
        /// 验证错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// 验证提示信息列表
        /// 用于存储转换过程中的业务验证提示信息
        /// </summary>
        public List<string> WarningMessages { get; set; } = new List<string>();
        
        /// <summary>
        /// 验证信息列表
        /// 用于存储转换过程中的信息提示
        /// </summary>
        public List<string> InfoMessages { get; set; } = new List<string>();

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
            
        /// <summary>
        /// 添加警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        public void AddWarning(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                WarningMessages.Add(message);
            }
        }
        
        /// <summary>
        /// 添加信息提示
        /// </summary>
        /// <param name="message">信息提示</param>
        public void AddInfo(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                InfoMessages.Add(message);
            }
        }
        
        /// <summary>
        /// 获取所有提示信息（警告+信息）
        /// </summary>
        /// <returns>所有提示信息</returns>
        public List<string> GetAllMessages()
        {
            var allMessages = new List<string>();
            allMessages.AddRange(WarningMessages);
            allMessages.AddRange(InfoMessages);
            return allMessages;
        }
        
        /// <summary>
        /// 获取格式化的提示信息文本
        /// </summary>
        /// <returns>格式化的提示信息文本</returns>
        public string GetFormattedMessages()
        {
            var allMessages = GetAllMessages();
            if (allMessages.Count == 0)
            {
                return string.Empty;
            }
            
            return string.Join(Environment.NewLine, allMessages);
        }
        
        /// <summary>
        /// 是否有提示信息
        /// </summary>
        public bool HasMessages => WarningMessages.Count > 0 || InfoMessages.Count > 0;
    }
    
    /// <summary>
    /// 单据转换操作类型1
    /// </summary>
    public enum DocumentConversionType
    {
        /// <summary>
        /// 单据生成型：生成新的目标单据，需要打开编辑窗体
        /// 例如：预收付款单 → 收付款单，销售订单 → 销售出库单
        /// </summary>
        DocumentGeneration = 0,
        
        /// <summary>
        /// 动作操作型：执行特定业务操作，不需要打开新窗体
        /// 例如：预收付款单 → 抵扣应收款，应收应付单 → 使用预收付款抵扣
        /// </summary>
        ActionOperation = 1
    }
}