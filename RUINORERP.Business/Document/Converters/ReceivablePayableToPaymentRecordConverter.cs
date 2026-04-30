using AutoMapper;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Document;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 应收应付款单到收付款单转换器
    /// 负责将应收应付款单及其明细转换为收付款单及其明细
    /// 复用业务层的核心转换逻辑（BuildPaymentRecord），确保数据一致性
    /// </summary>
    public class ReceivablePayableToPaymentRecordConverter : DocumentConverterBase<tb_FM_ReceivablePayable, tb_FM_PaymentRecord>
    {
        private readonly ILogger<ReceivablePayableToPaymentRecordConverter> _logger;
        private readonly tb_FM_PaymentRecordController<tb_FM_PaymentRecord> _paymentController;
        private readonly tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> _receivablePayableController;
        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="paymentController">收付款单控制器（用于调用核心转换逻辑）</param>
        public ReceivablePayableToPaymentRecordConverter(
            ILogger<ReceivablePayableToPaymentRecordConverter> logger,
            tb_FM_PaymentRecordController<tb_FM_PaymentRecord> paymentController,
            tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> receivablePayableController)
            : base(logger)
        {
            _paymentController = paymentController ?? throw new ArgumentNullException(nameof(paymentController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _receivablePayableController = receivablePayableController;
        }

        /// <summary>
        /// 转换唯一标识符
        /// </summary>
        public override string ConversionIdentifier => "Normal";

        /// <summary>
        /// 执行单据转换 - 直接调用业务层核心逻辑 BuildPaymentRecord
        /// 重写基类方法，完全控制转换过程
        /// </summary>
        /// <param name="source">源单据：应收应付款单</param>
        /// <returns>转换后的收付款单</returns>
        public override async Task<tb_FM_PaymentRecord> ConvertAsync(tb_FM_ReceivablePayable source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 直接调用经过长期验证的 BuildPaymentRecord 方法
            return await _paymentController.BuildPaymentRecord(new List<tb_FM_ReceivablePayable> { source });
        }
        
        /// <summary>
        /// 执行具体的转换逻辑 - 重写后不再使用基类的 target 参数模式
        /// </summary>
        /// <param name="source">源单据：应收应付款单</param>
        /// <param name="target">目标单据：收付款单（不再使用）</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_FM_ReceivablePayable source, tb_FM_PaymentRecord target)
        {
            // 此方法不再使用，逻辑已移至 ConvertAsync
            // 保留此方法以满足抽象类要求
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：应收应付款单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_FM_ReceivablePayable source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "应收应付款单不能为空";
                    return result;
                }

                // 检查审核状态 - 必须已审核通过
                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"应收应付款单{source.ARAPNo}未审核通过，只能转换已审核的单据";
                    return result;
                }

                // 检查支付状态 - 必须是待支付或部分支付
                if (source.ARAPStatus != (int)ARAPStatus.待支付 &&
                    source.ARAPStatus != (int)ARAPStatus.部分支付)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"应收应付款单{source.ARAPNo}支付状态为【{(ARAPStatus)source.ARAPStatus}】，只能转换【待支付】或【部分支付】状态的单据";
                    return result;
                }

                // 检查是否已结案(结案后不允许再收付款)
                if (source.ARAPStatus == (int)ARAPStatus.全部支付 || source.ARAPStatus == (int)ARAPStatus.已冲销)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"应收应付款单{source.ARAPNo}已结案，不允许再进行收付款操作";
                    return result;
                }

                // 检查是否有可抵扣的预收付款单
                await ValidateAvailableAdvancesAsync(source, result);

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证应收应付款单转换条件时发生错误");
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 验证是否有可抵扣的预收付款单
        /// </summary>
        /// <param name="source">应收应付款单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateAvailableAdvancesAsync(tb_FM_ReceivablePayable source, ValidationResult result)
        {
            try
            {
                // 查找可抵扣的预收付款单
                var sourceList = new List<tb_FM_ReceivablePayable> { source };
                var availableAdvancePairs = await _receivablePayableController.FindAvailableAdvances(sourceList);

                if (availableAdvancePairs.Any())
                {
                    // 从KeyValuePair中提取预收付款单列表并展平
                    var availableAdvances = availableAdvancePairs.SelectMany(kvp => kvp.Value).ToList();

                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    var totalAvailableAmount = availableAdvances.Sum(x => x.LocalBalanceAmount);
                    var advanceCount = availableAdvances.Count;
                    
                    // 设置需要用户确认
                    result.RequiresUserConfirmation = true;
                    result.ConfirmationMessage = $"检测到有 {advanceCount} 张可抵扣的预{paymentType}单，总可用金额为 {totalAvailableAmount:F2} 元。\r\n" +
                        $"优先通过抵扣核销。特殊情况才会跳过预收付款，选择再次收付款。\r\n" +
                        $"是否确认要直接生成收付款单，而不是先进行抵扣操作？";
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证可抵扣预收付款单时发生错误");
                result.AddWarning("验证可抵扣预收付款单时发生错误，请检查数据完整性");
            }
        }
    }
}