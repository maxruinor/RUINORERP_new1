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
    /// 预收付款单到收付款单转换器
    /// 负责将已生效的预收付款单及其明细转换为收付款单及其明细
    /// 复用业务层的核心转换逻辑（BuildPaymentRecord），确保数据一致性
    /// </summary>
    public class PreReceivedPaymentToPaymentRecordConverter : DocumentConverterBase<tb_FM_PreReceivedPayment, tb_FM_PaymentRecord>
    {
        private readonly ILogger<PreReceivedPaymentToPaymentRecordConverter> _logger;
        private readonly tb_FM_PaymentRecordController<tb_FM_PaymentRecord> _paymentController;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="paymentController">收付款单控制器（用于调用核心转换逻辑）</param>
        public PreReceivedPaymentToPaymentRecordConverter(
            ILogger<PreReceivedPaymentToPaymentRecordConverter> logger,
            tb_FM_PaymentRecordController<tb_FM_PaymentRecord> paymentController)
            : base(logger)
        {
            _paymentController = paymentController ?? throw new ArgumentNullException(nameof(paymentController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 转换唯一标识符
        /// </summary>
        public override string ConversionIdentifier => "Normal";


        /// <summary>
        /// 执行单据转换 - 直接调用业务层核心逻辑 BuildPaymentRecord
        /// 重写基类方法，完全控制转换过程
        /// </summary>
        /// <param name="source">源单据：预收付款单</param>
        /// <returns>转换后的收付款单</returns>
        public override async Task<tb_FM_PaymentRecord> ConvertAsync(tb_FM_PreReceivedPayment source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 直接调用经过长期验证的 BuildPaymentRecord 方法
            // isRefund = false 表示转换为收付款单（非退款）
            return await _paymentController.BuildPaymentRecord(new List<tb_FM_PreReceivedPayment> { source }, false);
        }

        /// <summary>
        /// 子类重写此方法以实现具体的业务规则验证
        /// </summary>
        protected override Task<ValidationResult> OnValidateBusinessRulesAsync(tb_FM_PreReceivedPayment source)
        {
            var result = new ValidationResult { CanConvert = true };
            var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
            // 规则：数据状态必须为“已生效”才能转收款单
            if (source.PrePaymentStatus != (int)PrePaymentStatus.已生效)
            {
                result.CanConvert = false;
                result.ErrorMessage = $"只有【已生效】状态的预收款单才能转为{paymentType}单";
            }

            return Task.FromResult(result);
        }

        /// <summary>
        /// 执行具体的转换逻辑
        /// 注意：本转换器已在 ConvertAsync 中通过 BuildPaymentRecord 完成全部逻辑，
        /// 此方法仅用于满足基类抽象要求，不再执行实际业务。
        /// </summary>
        protected override Task PerformConversionAsync(tb_FM_PreReceivedPayment source, tb_FM_PaymentRecord target)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：预收付款单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_FM_PreReceivedPayment source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "预收付款单不能为空";
                    return result;
                }

                // 检查预收付款单状态
                // 只有已生效状态的预收付款单才能转换为收付款单
                if (source.PrePaymentStatus != (int)PrePaymentStatus.已生效 ||
                    source.ApprovalStatus != (int)ApprovalStatus.审核通过 ||
                    !source.ApprovalResults.HasValue ||
                    !source.ApprovalResults.Value)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.CanConvert = false;
                    result.ErrorMessage = $"预{paymentType}单 {source.PreRPNO} 状态不符合转换条件，只能转换已审核且状态为【已生效】的预收付款单";
                    return result;
                }

                // 检查预收付款金额是否有效
                if (source.LocalPrepaidAmount <= 0 && source.ForeignPrepaidAmount <= 0)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.CanConvert = false;
                    result.ErrorMessage = $"预{paymentType}单 {source.PreRPNO} 的预收付金额无效，金额必须大于0";
                    return result;
                }

                // 添加关于金额计算的提示信息
                decimal localAmount = source.LocalPrepaidAmount;
                decimal foreignAmount = source.ForeignPrepaidAmount;
                var paymentTypeEnum = (ReceivePaymentType)source.ReceivePaymentType;

                if (foreignAmount > 0)
                {
                    result.AddInfo($"转换金额为 {localAmount:F2} (本币) / {foreignAmount:F2} (外币)，生成为{paymentTypeEnum}单");
                }
                else
                {
                    result.AddInfo($"转换金额为 {localAmount:F2} (本币)，生成为{paymentTypeEnum}单");
                }

                // 检查是否有平台来源信息
                if (source.IsFromPlatform == true)
                {
                    result.AddInfo($"该预收付款单来自平台，平台信息：{source.PrePaymentReason ?? "无"}");
                }

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证预收付款单转换条件时发生错误，预收付款单号：{PreRPNO}", source?.PreRPNO);
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }
    }
}
