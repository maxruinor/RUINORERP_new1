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
    /// 应收应付款单到预收付款单抵扣转换器
    /// 从应收/应付款单菜单执行抵扣，选择预收/预付款单进行抵扣
    /// 转换结果：更新应收应付款单的核销状态，更新预收付款单的余额
    /// </summary>
    [System.ComponentModel.Description("使用预收付款抵扣")]
    public class ReceivablePayableToPreReceivedPaymentOffsetConverter : DocumentConverterBase<tb_FM_ReceivablePayable, tb_FM_PreReceivedPayment>
    {
        private readonly ILogger<ReceivablePayableToPreReceivedPaymentOffsetConverter> _logger;
        private readonly tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> _receivablePayableController;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="receivablePayableController">应收应付控制器（用于调用核心抵扣逻辑）</param>
        public ReceivablePayableToPreReceivedPaymentOffsetConverter(
            ILogger<ReceivablePayableToPreReceivedPaymentOffsetConverter> logger,
            tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> receivablePayableController)
            : base(logger)
        {
            _receivablePayableController = receivablePayableController ?? throw new ArgumentNullException(nameof(receivablePayableController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 转换器显示名称
        /// 使用基类实现，从Description特性获取
        /// </summary>
        public override string DisplayName => base.DisplayName;

        /// <summary>
        /// 转换操作类型：动作操作型
        /// </summary>
        public override DocumentConversionType ConversionType => DocumentConversionType.ActionOperation;

        /// <summary>
        /// 执行动作操作 - 抵扣应付款
        /// </summary>
        /// <param name="source">源单据：应收应付款单</param>
        /// <param name="target">目标单据：预收付款单（必须提供）</param>
        /// <returns>操作结果</returns>
        public override async Task<ActionResult> ExecuteActionOperationAsync(tb_FM_ReceivablePayable source, tb_FM_PreReceivedPayment target = null)
        {
            if (source == null)
            {
                return ActionResult.Fail("应收应付款单不能为空");
            }

            if (target == null)
            {
                return ActionResult.Fail("请选择要使用的预收付款单");
            }

            try
            {
                // 调用业务层的核心抵扣逻辑
                bool success = await _receivablePayableController.ApplyManualPaymentAllocation(
                    source,
                    new List<tb_FM_PreReceivedPayment> { target });

                if (!success)
                {
                    return ActionResult.Fail("抵扣操作失败，请检查数据状态");
                }

                _logger.LogInformation(
                    "应收应付款单 {ARAPNo} 成功使用预收付款单 {PreRPNO} 抵扣",
                    source.ARAPNo,
                    target.PreRPNO);

                var result = ActionResult.SuccessResult();
                result.InfoMessages.Add($"应收应付款单 {source.ARAPNo} 成功使用预收付款单 {target.PreRPNO} 抵扣");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "应收应付款单 {ARAPNo} 使用预收付款单 {PreRPNO} 抵扣时发生错误",
                    source?.ARAPNo,
                    target?.PreRPNO);
                return ActionResult.Fail($"抵扣操作失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 执行单据转换 - 调用业务层核心抵扣逻辑
        /// 重写基类方法，完全控制转换过程
        /// </summary>
        /// <param name="source">源单据：应收应付款单</param>
        /// <returns>转换后的预收付款单（用于打开编辑窗体）</returns>
        public override async Task<tb_FM_PreReceivedPayment> ConvertAsync(tb_FM_ReceivablePayable source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 注意：这里需要用户选择要使用的预收付款单
            // 由于转换器是纯业务逻辑，实际的选择应该由UI层处理
            // 这里抛出异常，提示需要在UI层选择预收付款单
            throw new InvalidOperationException("抵扣操作需要在UI层选择用于抵扣的预收付款单。请使用编辑窗体的抵扣功能。");
        }

        /// <summary>
        /// 执行具体的转换逻辑 - 执行抵扣操作
        /// 此方法由UI层调用，传入已选择的预收付款单列表
        /// </summary>
        /// <param name="source">源单据：应收应付款单</param>
        /// <param name="target">目标单据：预收付款单</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_FM_ReceivablePayable source, tb_FM_PreReceivedPayment target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "应收应付款单不能为空");
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target), "预收付款单不能为空");
            }

            try
            {
                // 调用业务层的核心抵扣逻辑
                bool success = await _receivablePayableController.ApplyManualPaymentAllocation(
                    source,
                    new List<tb_FM_PreReceivedPayment> { target });

                if (!success)
                {
                    throw new InvalidOperationException("抵扣操作失败，请检查数据状态");
                }

                _logger.LogInformation(
                    "应收应付款单 {ARAPNo} 成功使用预收付款单 {PreRPNO} 抵扣",
                    source.ARAPNo,
                    target.PreRPNO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "应收应付款单 {ARAPNo} 使用预收付款单 {PreRPNO} 抵扣时发生错误",
                    source?.ARAPNo,
                    target?.PreRPNO);
                throw;
            }
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

                // 检查应收应付款单状态
                // 只有待支付或部分支付状态的应收应付款单才能进行抵扣
                if (source.ARAPStatus != (int)ARAPStatus.待支付 &&
                    source.ARAPStatus != (int)ARAPStatus.部分支付)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.CanConvert = false;
                    result.ErrorMessage = $"应{paymentType}单 {source.ARAPNo} 状态不符合抵扣条件，当前状态为【{((ARAPStatus)source.ARAPStatus).ToString()}】，只能抵扣【待支付】或【部分支付】状态的应{paymentType}单";
                    return result;
                }

                // 检查是否已审核
                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过 ||
                    !source.ApprovalResults.HasValue ||
                    !source.ApprovalResults.Value)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"应收应付款单 {source.ARAPNo} 未审核通过，无法进行抵扣操作";
                    return result;
                }

                // 检查应收应付余额是否有效
                if (source.LocalBalanceAmount <= 0)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"应收应付款单 {source.ARAPNo} 的余额为0，无法进行抵扣操作";
                    return result;
                }

                // 添加提示信息
                var paymentTypeEnum = (ReceivePaymentType)source.ReceivePaymentType;
                result.AddInfo($"应{paymentTypeEnum}单 {source.ARAPNo} 未核销余额为 {source.LocalBalanceAmount:F2} (本币)");
                result.AddInfo($"抵扣将生成核销记录，并更新应收应付款单和预收付款单的状态");

                // 查找可用的预收付款单，给出建议
                var availableAdvances = await _receivablePayableController.FindAvailableAdvances(source);

                if (availableAdvances.Any())
                {
                    var totalAvailableAmount = availableAdvances.Sum(x => x.LocalBalanceAmount);
                    result.AddInfo($"找到 {availableAdvances.Count} 张可用的预{paymentTypeEnum}单，总可用金额为 {totalAvailableAmount:F2} (本币)");

                    if (totalAvailableAmount < source.LocalBalanceAmount)
                    {
                        result.AddWarning($"可用预{paymentTypeEnum}单总金额 {totalAvailableAmount:F2} 小于应{paymentTypeEnum}单余额 {source.LocalBalanceAmount:F2}，只能部分抵扣");
                    }
                }
                else
                {
                    result.AddWarning($"没有找到可用的预{paymentTypeEnum}单，无法进行抵扣操作");
                    result.CanConvert = false;
                    result.ErrorMessage = $"没有找到可用的预{paymentTypeEnum}单";
                }

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证应收应付款单抵扣条件时发生错误，应收应付款单号：{ARAPNo}", source?.ARAPNo);
                result.CanConvert = false;
                result.ErrorMessage = $"验证抵扣条件时发生错误：{ex.Message}";
            }

            return result;
        }
    }
}
