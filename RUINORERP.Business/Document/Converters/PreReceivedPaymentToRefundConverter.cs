using Microsoft.Extensions.Logging;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Document;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 预收付款单退还余款转换器
    /// 将预收/付款单的余额退还,生成红字收/付款单
    /// 这是一个单据生成型转换,会生成新的收付款单(金额为负数)
    /// </summary>
    public class PreReceivedPaymentToRefundConverter : DocumentConverterBase<tb_FM_PreReceivedPayment, tb_FM_PaymentRecord>
    {
        private readonly ILogger<PreReceivedPaymentToRefundConverter> _logger;
        private readonly tb_FM_PaymentRecordController<tb_FM_PaymentRecord> _paymentController;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="paymentController">收付款单控制器(用于调用核心转换逻辑)</param>
        public PreReceivedPaymentToRefundConverter(
            ILogger<PreReceivedPaymentToRefundConverter> logger,
            tb_FM_PaymentRecordController<tb_FM_PaymentRecord> paymentController)
            : base(logger)
        {
            _paymentController = paymentController ?? throw new ArgumentNullException(nameof(paymentController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 转换唯一标识符
        /// </summary>
        public override string ConversionIdentifier => "Refund";

        /// <summary>
        /// 转换器显示名称
        /// 注意:由于预收付款单的 ReceivePaymentType 在运行时才能确定,
        /// 这里返回通用文本,实际菜单显示由 DocumentConverterFactory 根据具体单据动态调整
        /// </summary>
        public override string DisplayName => "退还余款";

        /// <summary>
        /// 转换操作类型:单据生成型
        /// </summary>
        public override DocumentConversionType ConversionType => DocumentConversionType.DocumentGeneration;

        /// <summary>
        /// 执行单据转换 - 直接调用业务层核心逻辑 BuildPaymentRecord
        /// 重写基类方法,完全控制转换过程
        /// </summary>
        /// <param name="source">源单据:预收付款单</param>
        /// <returns>转换后的收付款单(红字)</returns>
        public override async Task<tb_FM_PaymentRecord> ConvertAsync(tb_FM_PreReceivedPayment source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 直接调用经过长期验证的 BuildPaymentRecord 方法
            // isRefund = true 表示转换为退款单(红字)
            return await _paymentController.BuildPaymentRecord(new List<tb_FM_PreReceivedPayment> { source }, true);
        }

        /// <summary>
        /// 子类重写此方法以实现具体的业务规则验证
        /// </summary>
        protected override Task<ValidationResult> OnValidateBusinessRulesAsync(tb_FM_PreReceivedPayment source)
        {
            var result = new ValidationResult { CanConvert = true };
            var currentStatus = (PrePaymentStatus)source.PrePaymentStatus;

            // 规则：数据状态必须为“待核销”或“处理中”才能退还余款
            if (currentStatus != PrePaymentStatus.待核销 && currentStatus != PrePaymentStatus.处理中)
            {
                result.CanConvert = false;
                result.ErrorMessage = $"当前状态【{currentStatus}】不支持退还余款，仅支持【待核销】或【处理中】状态";
            }

            // 额外检查：必须有余额可退
            if (result.CanConvert && source.LocalBalanceAmount <= 0)
            {
                result.CanConvert = false;
                result.ErrorMessage = "该预收款单没有可退余额";
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
        /// <param name="source">源单据:预收付款单</param>
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
                // 只有待核销、处理中、混合结清状态的预收付款单才能退还余款
                var currentStatus = (PrePaymentStatus)source.PrePaymentStatus;
                if (currentStatus != PrePaymentStatus.待核销 &&
                    currentStatus != PrePaymentStatus.处理中 &&
                    currentStatus != PrePaymentStatus.混合结清)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.CanConvert = false;
                    result.ErrorMessage = $"预{paymentType}单 {source.PreRPNO} 状态不符合退还余款条件,当前状态为【{currentStatus.ToString()}】,只能退还【待核销】、【处理中】或【混合结清】状态的预收付款单";
                    return result;
                }

                // 检查审核状态
                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过 ||
                    !source.ApprovalResults.HasValue ||
                    !source.ApprovalResults.Value)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"预收付款单 {source.PreRPNO} 未审核通过,无法退还余款";
                    return result;
                }

                // 检查是否有可退余额
                if (source.LocalBalanceAmount <= 0 && source.ForeignBalanceAmount <= 0)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.CanConvert = false;
                    result.ErrorMessage = $"预{paymentType}单 {source.PreRPNO} 没有可退余额(本币余额:{source.LocalBalanceAmount:F2},外币余额:{source.ForeignBalanceAmount:F2})";
                    return result;
                }

                // 检查是否已结案
                if (currentStatus == PrePaymentStatus.结案)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"预收付款单 {source.PreRPNO} 已结案,不能退还余款,请先反结案";
                    return result;
                }

                // 添加关于金额计算的提示信息
                decimal localBalance = source.LocalBalanceAmount;
                decimal foreignBalance = source.ForeignBalanceAmount;
                var paymentTypeEnum = (ReceivePaymentType)source.ReceivePaymentType;

                if (foreignBalance > 0)
                {
                    result.AddInfo($"可退金额为 {localBalance:F2} (本币) / {foreignBalance:F2} (外币),将生成红字{paymentTypeEnum}单");
                }
                else
                {
                    result.AddInfo($"可退金额为 {localBalance:F2} (本币),将生成红字{paymentTypeEnum}单");
                }

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证预收付款单退还余款条件时发生错误,预收付款单号:{PreRPNO}", source?.PreRPNO);
                result.CanConvert = false;
                result.ErrorMessage = $"验证退还余款条件时发生错误:{ex.Message}";
            }

            return result;
        }
    }
}
