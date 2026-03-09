using AutoMapper;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.AutoMapper;
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
    /// 预收付款单到应收应付款单抵扣转换器
    /// 负责将预收付款单用于抵扣应收应付款单
    /// 转换结果：更新应收应付款单的核销状态，更新预收付款单的余额
    /// 这是一个动作操作型转换，不需要生成新单据，而是执行业务操作
    /// </summary>
    [System.ComponentModel.Description("预收付款单抵扣应收款")]
    public class PreReceivedPaymentToReceivablePayableOffsetConverter : DocumentConverterBase<tb_FM_PreReceivedPayment, tb_FM_ReceivablePayable>
    {
        private readonly ILogger<PreReceivedPaymentToReceivablePayableOffsetConverter> _logger;
        private readonly tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> _receivablePayableController;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="receivablePayableController">应收应付控制器（用于调用核心抵扣逻辑）</param>
        /// <param name="mapper">对象映射器</param>
        public PreReceivedPaymentToReceivablePayableOffsetConverter(
            ILogger<PreReceivedPaymentToReceivablePayableOffsetConverter> logger,
            tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> receivablePayableController,
            IMapper mapper)
            : base(logger)
        {
            _receivablePayableController = receivablePayableController ?? throw new ArgumentNullException(nameof(receivablePayableController));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
        /// 菜单项显示文本（重写基类，用于联动菜单显示）
        /// </summary>
        public override string MenuItemText => "抵扣应收款";

        /// <summary>
        /// 执行单据转换 - 调用业务层核心抵扣逻辑
        /// 对于动作操作型转换，此方法不执行实际转换，实际操作在UI层处理
        /// </summary>
        /// <param name="source">源单据：预收付款单</param>
        /// <returns>转换后的应收应付款单（用于打开编辑窗体）</returns>
        public override async Task<tb_FM_ReceivablePayable> ConvertAsync(tb_FM_PreReceivedPayment source)
        {
            // 对于动作操作型转换，不执行实际转换
            // 实际操作在UI层的PerformActionOperationAsync中处理
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 执行具体的转换逻辑 - 执行抵扣操作
        /// 此方法由UI层调用，传入已选择的应收应付款单
        /// </summary>
        /// <param name="source">源单据：预收付款单</param>
        /// <param name="target">目标单据：应收应付款单</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_FM_PreReceivedPayment source, tb_FM_ReceivablePayable target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "预收付款单不能为空");
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target), "应收应付款单不能为空");
            }

            try
            {
                // 调用业务层的核心抵扣逻辑
                bool success = await _receivablePayableController.ApplyManualPaymentAllocation(
                    target,
                    new List<tb_FM_PreReceivedPayment> { source });

                if (!success)
                {
                    throw new InvalidOperationException("抵扣操作失败，请检查数据状态");
                }

                _logger.LogInformation(
                    "预收付款单 {PreRPNO} 成功抵扣应收应付款单 {ARAPNo}",
                    source.PreRPNO,
                    target.ARAPNo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "预收付款单 {PreRPNO} 抵扣应收应付款单 {ARAPNo} 时发生错误",
                    source?.PreRPNO,
                    target?.ARAPNo);
                throw;
            }
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
                // 只有待核销或部分核销状态的预收付款单才能进行抵扣
                if (source.PrePaymentStatus != (int)PrePaymentStatus.待核销 &&
                    source.PrePaymentStatus != (int)PrePaymentStatus.部分核销)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.CanConvert = false;
                    result.ErrorMessage = $"预{paymentType}单 {source.PreRPNO} 状态不符合抵扣条件，当前状态为【{((PrePaymentStatus)source.PrePaymentStatus).ToString()}】，只能抵扣【待核销】或【部分核销】状态的预收付款单";
                    return result;
                }

                // 检查是否已审核
                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过 ||
                    !source.ApprovalResults.HasValue ||
                    !source.ApprovalResults.Value)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"预收付款单 {source.PreRPNO} 未审核通过，无法进行抵扣操作";
                    return result;
                }

                // 检查预收付款余额是否有效
                if (source.LocalBalanceAmount <= 0)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"预收付款单 {source.PreRPNO} 的可用余额为0，无法进行抵扣操作";
                    return result;
                }

                // 添加提示信息
                var paymentTypeEnum = (ReceivePaymentType)source.ReceivePaymentType;
                result.AddInfo($"预{paymentTypeEnum}单 {source.PreRPNO} 可用余额为 {source.LocalBalanceAmount:F2} (本币)");
                result.AddInfo($"抵扣将生成核销记录，并更新应收应付款单和预收付款单的状态");

                // 查找可抵扣的应收应付款单，给出建议
                var availableReceivables = await _receivablePayableController.FindAvailableReceivablesForOffset(source);
                if (availableReceivables.Any())
                {
                    result.AddInfo($"找到 {availableReceivables.Count} 张可抵扣的应{paymentTypeEnum}单，请在编辑窗体中选择要抵扣的单据");
                }
                else
                {
                    result.AddWarning($"没有找到可抵扣的应{paymentTypeEnum}单，无法进行抵扣操作");
                    result.CanConvert = false;
                    result.ErrorMessage = $"没有找到可抵扣的应{paymentTypeEnum}单";
                }

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证预收付款单抵扣条件时发生错误，预收付款单号：{PreRPNO}", source?.PreRPNO);
                result.CanConvert = false;
                result.ErrorMessage = $"验证抵扣条件时发生错误：{ex.Message}";
            }

            return result;
        }
    }
}
