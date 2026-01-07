using Microsoft.Extensions.Logging;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Document;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 报销单到付款单转换器
    /// 负责将报销单及其明细转换为付款单及其明细
    /// 复用业务层的核心转换逻辑（BuildPaymentRecord），确保数据一致性
    /// </summary>
    public class ExpenseClaimToPaymentRecordConverter : DocumentConverterBase<tb_FM_ExpenseClaim, tb_FM_PaymentRecord>
    {
        private readonly ILogger<ExpenseClaimToPaymentRecordConverter> _logger;
        private readonly tb_FM_PaymentRecordController<tb_FM_PaymentRecord> _paymentController;
        
        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="paymentController">付款单控制器（用于调用核心转换逻辑）</param>
        public ExpenseClaimToPaymentRecordConverter(
            ILogger<ExpenseClaimToPaymentRecordConverter> logger,
            tb_FM_PaymentRecordController<tb_FM_PaymentRecord> paymentController)
            : base(logger)
        {
            _paymentController = paymentController ?? throw new ArgumentNullException(nameof(paymentController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 转换器显示名称
        /// 使用基类实现，从Description特性获取
        /// </summary>
        public override string DisplayName => base.DisplayName;

        /// <summary>
        /// 执行单据转换 - 直接调用业务层核心逻辑 BuildPaymentRecord
        /// 重写基类方法，完全控制转换过程
        /// </summary>
        /// <param name="source">源单据：报销单</param>
        /// <returns>转换后的付款单</returns>
        public override async Task<tb_FM_PaymentRecord> ConvertAsync(tb_FM_ExpenseClaim source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 直接调用经过长期验证的 BuildPaymentRecord 方法
            return await _paymentController.BuildPaymentRecord(source);
        }
        
        /// <summary>
        /// 执行具体的转换逻辑 - 重写后不再使用基类的 target 参数模式
        /// </summary>
        /// <param name="source">源单据：报销单</param>
        /// <param name="target">目标单据：付款单（不再使用）</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_FM_ExpenseClaim source, tb_FM_PaymentRecord target)
        {
            // 此方法不再使用，逻辑已移至 ConvertAsync
            // 保留此方法以满足抽象类要求
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：报销单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_FM_ExpenseClaim source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "报销单不能为空";
                    return result;
                }

                // 检查报销单状态
                if (source.DataStatus != (int)DataStatus.确认 || 
                    source.ApprovalStatus != (int)ApprovalStatus.审核通过 ||
                    !source.ApprovalResults.HasValue || 
                    !source.ApprovalResults.Value)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"报销单{source.ClaimNo}状态不符合转换条件，只能转换已审核且状态为[已确认]的报销单";
                    return result;
                }

                // 添加关于金额计算的提示信息
                decimal claimAmount = Math.Abs(source.ClaimAmount);
                if (claimAmount > 0)
                {
                    result.AddInfo($"转换金额为{claimAmount:F2}，生成为付款单");
                }
                else
                {
                    result.AddInfo($"转换金额为0，仍将生成付款单");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证报销单转换条件时发生错误");
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }
    }
}