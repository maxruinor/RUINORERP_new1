using Microsoft.Extensions.Logging;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Document;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Lib.UI;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tb_SaleOut = RUINORERP.Model.tb_SaleOut;

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
        private readonly IDocumentSelectorFactory _selectorFactory;
        private readonly ApplicationContext _appContext;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="receivablePayableController">应收应付控制器（用于调用核心抵扣逻辑）</param>
        /// <param name="selectorFactory">单据选择器工厂</param>
        /// <param name="appContext">应用上下文</param>
        public ReceivablePayableToPreReceivedPaymentOffsetConverter(
            ILogger<ReceivablePayableToPreReceivedPaymentOffsetConverter> logger,
            tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> receivablePayableController,
            IDocumentSelectorFactory selectorFactory,
            ApplicationContext appContext)
            : base(logger)
        {
            _receivablePayableController = receivablePayableController ?? throw new ArgumentNullException(nameof(receivablePayableController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _selectorFactory = selectorFactory ?? throw new ArgumentNullException(nameof(selectorFactory));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        }

        /// <summary>
        /// 转换器显示名称
        /// 直接返回菜单文本，不需要基类的格式
        /// </summary>
        public override string DisplayName => "使用预收款抵扣";

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

            try
            {
                // 查找可用的预收付款单
                //如果结果大于1，则默认启动多选模式，并且将来源单据对应的订单号相同的预收付款单排在前面，选中订单号相同的预收付款单
                var availableAdvances = await _receivablePayableController.FindAvailableAdvances(source);
                if (!availableAdvances.Any())
                {
                    return ActionResult.Fail($"没有找到可用的预收付款单");
                }

                // 检查预收付款单的可用余额是否足够
                decimal totalAvailableAmount = availableAdvances.Sum(x => x.LocalBalanceAmount);
                if (totalAvailableAmount < source.LocalBalanceAmount)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    var confirmResult = await System.Threading.Tasks.Task.Run(() =>
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.DialogResult.No;
                        System.Windows.Forms.Application.OpenForms[0].Invoke((System.Windows.Forms.MethodInvoker)delegate
                        {
                            result = System.Windows.Forms.MessageBox.Show(
                                $"可抵扣的预{paymentType}单总余额【{totalAvailableAmount:F2}】元小于应{paymentType}单余额【{source.LocalBalanceAmount:F2}】元。\r\n" +
                                $"是否继续进行部分抵扣？",
                                "余额不足确认",
                                System.Windows.Forms.MessageBoxButtons.YesNo,
                                System.Windows.Forms.MessageBoxIcon.Question);
                        });
                        return result;
                    });

                    if (confirmResult == System.Windows.Forms.DialogResult.No)
                    {
                        return ActionResult.CancelResult();
                    }
                }

                // 判断是否需要弹出选择窗体
                bool needShowSelector = true;
                List<tb_FM_PreReceivedPayment> selectedAdvances = null;

                // 如果只有一条符合条件且与当前订单关联，自动抵扣
                if (availableAdvances.Count == 1)
                {
                    var singleAdvance = availableAdvances[0];
                    
                    // 检查订单号是否匹配
                    bool isOrderMatched = await CheckOrderMatchedAsync(source, singleAdvance);
                    
                    if (isOrderMatched)
                    {
                        // 自动抵扣，无需弹出选择窗体
                        needShowSelector = false;
                        selectedAdvances = new List<tb_FM_PreReceivedPayment> { singleAdvance };
                    }
                }

                if (needShowSelector)
                {
                    // 弹出选择窗体让用户选择
                    selectedAdvances = await ShowAdvanceSelectorAsync(source, availableAdvances);
                    if (selectedAdvances == null || selectedAdvances.Count == 0)
                    {
                        return ActionResult.CancelResult();
                    }
                }

                // 执行抵扣操作
                bool success = await _receivablePayableController.ApplyManualPaymentAllocation(
                    source,
                    selectedAdvances);

                if (!success)
                {
                    return ActionResult.Fail("抵扣操作失败，请检查数据状态");
                }

                _logger.LogInformation(
                    "应收应付款单 {ARAPNo} 成功使用预收付款单抵扣",
                    source.ARAPNo);

                var result = ActionResult.SuccessResult();
                result.InfoMessages.Add($"应收应付款单 {source.ARAPNo} 成功使用预收付款单抵扣");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "应收应付款单 {ARAPNo} 使用预收付款单抵扣时发生错误",
                    source?.ARAPNo);
                return ActionResult.Fail($"抵扣操作失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取单据对应的订单ID
        /// </summary>
        /// <param name="sourceBizType">来源业务类型</param>
        /// <param name="sourceBillId">来源单据ID</param>
        /// <returns>订单ID，如果不是订单类型或无法获取则返回null</returns>
        private async Task<long?> GetOrderIdAsync(int? sourceBizType, long? sourceBillId)
        {
            if (!sourceBizType.HasValue || !sourceBillId.HasValue)
            {
                return null;
            }

            try
            {
                switch ((BizType)sourceBizType.Value)
                {
                    case BizType.销售订单:
                    case BizType.采购订单:
                        return sourceBillId;

                    case BizType.销售出库单:
                        var saleOut = await _appContext.Db.Queryable<tb_SaleOut>()
                            .Includes(c => c.tb_saleorder)
                            .Where(c => c.SaleOut_MainID == sourceBillId.Value)
                            .FirstAsync();
                        return saleOut?.tb_saleorder?.SOrder_ID;

                    case BizType.采购入库单:
                        var purEntry = await _appContext.Db.Queryable<tb_PurEntry>()
                            .Includes(c => c.tb_purorder)
                            .Where(c => c.PurEntryID == sourceBillId.Value)
                            .FirstAsync();
                        return purEntry?.tb_purorder?.PurOrder_ID;

                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取订单ID时发生错误，来源业务类型：{SourceBizType}，来源单据ID：{SourceBillId}", 
                    sourceBizType, sourceBillId);
                return null;
            }
        }

        /// <summary>
        /// 检查应收应付款单和预收付款单的订单号是否匹配
        /// </summary>
        /// <param name="receivable">应收应付款单</param>
        /// <param name="prePayment">预收付款单</param>
        /// <returns>是否匹配</returns>
        private async Task<bool> CheckOrderMatchedAsync(tb_FM_ReceivablePayable receivable, tb_FM_PreReceivedPayment prePayment)
        {
            try
            {
                var receivableOrderId = await GetOrderIdAsync(receivable.SourceBizType, receivable.SourceBillId);
                var prePaymentOrderId = await GetOrderIdAsync(prePayment.SourceBizType, prePayment.SourceBillId);

                return prePaymentOrderId.HasValue && receivableOrderId.HasValue && 
                       prePaymentOrderId.Value == receivableOrderId.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查订单号匹配时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 显示预收付款单选择窗体
        /// </summary>
        /// <param name="source">应收应付款单</param>
        /// <param name="availableAdvances">可用的预收付款单列表</param>
        /// <returns>用户选择的预收付款单列表，如果取消则返回 null</returns>
        private async Task<List<tb_FM_PreReceivedPayment>> ShowAdvanceSelectorAsync(tb_FM_ReceivablePayable source, List<tb_FM_PreReceivedPayment> availableAdvances)
        {
            try
            {
                var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                
                // 使用工厂创建选择器
                var selector = _selectorFactory.CreateSelector<tb_FM_PreReceivedPayment>();
                selector.ConfirmButtonText = "抵扣";
                selector.AllowMultiSelect = true;

                // 使用表达式树配置列映射
                selector.ConfigureColumn(x => x.PreRPNO, "单据编号");
                selector.ConfigureColumn(x => x.LocalPrepaidAmount, "金额");
                selector.ConfigureColumn(x => x.LocalBalanceAmount, "可用金额");
                selector.ConfigureColumn(x => x.CustomerVendor_ID, "客户");
                selector.ConfigureColumn(x => x.PrePayDate, "付款日期");
                selector.ConfigureColumn(x => x.SourceBizType, "来源业务");
                selector.ConfigureColumn(x => x.SourceBillNo, "来源单号");
                selector.ConfigureSummaryColumn(x => x.LocalPrepaidAmount);
                selector.ConfigureSummaryColumn(x => x.LocalBalanceAmount);
                selector.InitializeSelector(availableAdvances, $"选择预{paymentType}单");

                // 在UI线程上显示选择窗体
                List<tb_FM_PreReceivedPayment> selectedAdvances = null;
                await System.Threading.Tasks.Task.Run(() =>
                {
                    System.Windows.Forms.Application.OpenForms[0].Invoke((System.Windows.Forms.MethodInvoker)delegate
                    {
                        if (selector.ShowDialog())
                        {
                            selectedAdvances = selector.SelectedItems;
                        }
                    });
                });

                return selectedAdvances;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "显示预收付款单选择窗体时发生错误");
                return null;
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
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "应收应付款单不能为空";
                    return result;
                }

                if (source.ARAPStatus != (int)ARAPStatus.待支付 &&
                    source.ARAPStatus != (int)ARAPStatus.部分支付)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.CanConvert = false;
                    result.ErrorMessage = $"应{paymentType}单 {source.ARAPNo} 状态不符合抵扣条件，当前状态为【{((ARAPStatus)source.ARAPStatus).ToString()}】，只能抵扣【待支付】或【部分支付】状态的应{paymentType}单";
                    return result;
                }

                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过 ||
                    !source.ApprovalResults.HasValue ||
                    !source.ApprovalResults.Value)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"应收应付款单 {source.ARAPNo} 未审核通过，无法进行抵扣操作";
                    return result;
                }

                if (source.LocalBalanceAmount <= 0)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"应收应付款单 {source.ARAPNo} 的余额为0，无法进行抵扣操作";
                    return result;
                }

                var paymentTypeEnum = (ReceivePaymentType)source.ReceivePaymentType;
                result.AddInfo($"应{paymentTypeEnum}单 {source.ARAPNo} 未核销余额为 {source.LocalBalanceAmount:F2} (本币)");
                result.AddInfo($"抵扣将生成核销记录，并更新应收应付款单和预收付款单的状态");

                await Task.CompletedTask;
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
