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
    /// 对账单到收付款单转换器
    /// 负责将对账单及其明细转换为收付款单及其明细
    /// 复用业务层的核心转换逻辑（BuildPaymentRecord），确保数据一致性
    /// </summary>
    public class StatementToPaymentRecordConverter : DocumentConverterBase<tb_FM_Statement, tb_FM_PaymentRecord>
    {
        private readonly ILogger<StatementToPaymentRecordConverter> _logger;
        private readonly tb_FM_PaymentRecordController<tb_FM_PaymentRecord> _paymentController;
        
        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="paymentController">收付款单控制器（用于调用核心转换逻辑）</param>
        public StatementToPaymentRecordConverter(
            ILogger<StatementToPaymentRecordConverter> logger,
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
        /// <param name="source">源单据：对账单</param>
        /// <returns>转换后的收付款单</returns>
        public override async Task<tb_FM_PaymentRecord> ConvertAsync(tb_FM_Statement source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 直接调用经过长期验证的 BuildPaymentRecord 方法
            return await _paymentController.BuildPaymentRecord(new List<tb_FM_Statement> { source });
        }
        
        /// <summary>
        /// 执行具体的转换逻辑 - 重写后不再使用基类的 target 参数模式
        /// </summary>
        /// <param name="source">源单据：对账单</param>
        /// <param name="target">目标单据：收付款单（不再使用）</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_FM_Statement source, tb_FM_PaymentRecord target)
        {
            // 此方法不再使用，逻辑已移至 ConvertAsync
            // 保留此方法以满足抽象类要求
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：对账单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_FM_Statement source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "对账单不能为空";
                    return result;
                }

                // 检查对账单状态
                if (source.StatementStatus != (int)StatementStatus.确认 && 
                    source.StatementStatus != (int)StatementStatus.部分结算 ||
                    source.ApprovalStatus != (int)ApprovalStatus.审核通过 ||
                    !source.ApprovalResults.HasValue || 
                    !source.ApprovalResults.Value)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.CanConvert = false;
                    result.ErrorMessage = $"对账单{source.StatementNo}状态不符合转换条件，只能转换已审核且状态为[已确认]或[部分结算]的对账单";
                    return result;
                }

                // 检查是否为余额对账模式且总金额为零
                bool isBalanceStatement = (StatementType)source.StatementType == StatementType.余额对账;
                bool isTotalAmountZero = Math.Abs(source.ClosingBalanceLocalAmount) < 0.01m;

                if (isBalanceStatement && isTotalAmountZero)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"余额对账模式下，期末余额为0时不允许转单。如需核销，请使用【红蓝单对冲核销】功能。";
                    return result;
                }

                // 添加关于金额计算的提示信息
                decimal absAmount = Math.Abs(source.ClosingBalanceLocalAmount);
                if (absAmount > 0)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.AddInfo($"转换金额为{absAmount:F2}，生成为{paymentType}单");
                }
                else
                {
                    result.AddInfo($"转换金额为0，仍将按对账单类型生成收付款单");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证对账单转换条件时发生错误");
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }
    }
}