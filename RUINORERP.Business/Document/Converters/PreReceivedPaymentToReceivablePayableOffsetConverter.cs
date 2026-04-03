using AutoMapper;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.AutoMapper;
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
using tb_PurEntry = RUINORERP.Model.tb_PurEntry;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 预收付款单到应收应付款单抵扣转换器
    /// 从预收/预付款单菜单执行抵扣，选择应收/应付款单进行抵扣
    /// 转换结果：更新应收应付款单的核销状态，更新预收付款单的余额
    /// 这是一个动作操作型转换，不需要生成新单据，而是执行业务操作
    /// </summary>
    [System.ComponentModel.Description("抵扣应收付款")]
    public class PreReceivedPaymentToReceivablePayableOffsetConverter : DocumentConverterBase<tb_FM_PreReceivedPayment, tb_FM_ReceivablePayable>
    {
        private readonly ILogger<PreReceivedPaymentToReceivablePayableOffsetConverter> _logger;
        private readonly tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> _receivablePayableController;
        private readonly IMapper _mapper;
        private readonly IDocumentSelectorFactory _selectorFactory;
        private readonly ApplicationContext _appContext;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="receivablePayableController">应收应付控制器（用于调用核心抵扣逻辑）</param>
        /// <param name="mapper">对象映射器</param>
        /// <param name="selectorFactory">单据选择器工厂</param>
        /// <param name="appContext">应用上下文</param>
        public PreReceivedPaymentToReceivablePayableOffsetConverter(
            ILogger<PreReceivedPaymentToReceivablePayableOffsetConverter> logger,
            tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> receivablePayableController,
            IMapper mapper,
            IDocumentSelectorFactory selectorFactory,
            ApplicationContext appContext)
            : base(logger)
        {
            _receivablePayableController = receivablePayableController ?? throw new ArgumentNullException(nameof(receivablePayableController));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _selectorFactory = selectorFactory ?? throw new ArgumentNullException(nameof(selectorFactory));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        }

        /// <summary>
        /// 转换器显示名称
        /// 直接返回菜单文本，不需要基类的格式
        /// </summary>
        public override string DisplayName => "抵扣应收款";

        /// <summary>
        /// 转换操作类型：动作操作型
        /// </summary>
        public override DocumentConversionType ConversionType => DocumentConversionType.ActionOperation;

        /// <summary>
        /// 执行动作操作 - 抵扣应收款
        /// 业务逻辑：
        /// 1. 查找可抵扣的应收应付款单
        /// 2. 检查预收付款单余额是否足够
        /// 3. 将与预收付款单关联的订单号相同的应收应付款单排在最前面
        /// 4. 如果订单号不一致，提示用户确认
        /// 5. 执行抵扣操作
        /// </summary>
        /// <param name="source">源单据：预收付款单</param>
        /// <param name="target">目标单据：应收应付款单（必须提供）</param>
        /// <returns>操作结果</returns>
        public override async Task<ActionResult> ExecuteActionOperationAsync(tb_FM_PreReceivedPayment source, tb_FM_ReceivablePayable target = null)
        {
            if (source == null)
            {
                return ActionResult.Fail("预收付款单不能为空");
            }

            try
            {
                // 查找可抵扣的应收应付款单
                var availableReceivables = await _receivablePayableController.FindAvailableReceivablesForOffset(source);
                if (!availableReceivables.Any())
                {
                    return ActionResult.Fail($"没有找到可抵扣的应收应付款单");
                }

                // 判断是否需要弹出选择窗体
                bool needShowSelector = true;
                tb_FM_ReceivablePayable selectedReceivable = null;

                // 如果只有一条符合条件且与当前订单关联，自动抵扣
                if (availableReceivables.Count == 1)
                {
                    var singleReceivable = availableReceivables[0];
                    
                    // 检查订单号是否匹配
                    bool isOrderMatched = await CheckOrderMatchedAsync(source, singleReceivable);
                    
                    // 检查预收付款单余额是否足够
                    bool isBalanceSufficient = source.LocalBalanceAmount >= singleReceivable.LocalBalanceAmount;
                    
                    if (isOrderMatched && isBalanceSufficient)
                    {
                        // 自动抵扣，无需弹出选择窗体
                        needShowSelector = false;
                        selectedReceivable = singleReceivable;
                    }
                }

                if (needShowSelector)
                {
                    // 弹出选择窗体让用户选择
                    selectedReceivable = await ShowReceivableSelectorAsync(source, availableReceivables);
                    if (selectedReceivable == null)
                    {
                        return ActionResult.CancelResult();
                    }

                    // 检查订单号是否一致
                    var sourceOrderId = await GetOrderIdAsync(source.SourceBizType, source.SourceBillId);
                    var receivableOrderId = await GetOrderIdAsync(selectedReceivable.SourceBizType, selectedReceivable.SourceBillId);
                    
                    if (sourceOrderId.HasValue && receivableOrderId.HasValue && 
                        sourceOrderId.Value != receivableOrderId.Value)
                    {
                        var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                        var confirmResult = await System.Threading.Tasks.Task.Run(() =>
                        {
                            System.Windows.Forms.DialogResult result = System.Windows.Forms.DialogResult.No;
                            System.Windows.Forms.Application.OpenForms[0].Invoke((System.Windows.Forms.MethodInvoker)delegate
                            {
                                result = System.Windows.Forms.MessageBox.Show(
                                    $"当前【预{paymentType}单】：{source.PreRPNO}与【应{paymentType}单】：{selectedReceivable.ARAPNo}的订单号不一致，你确实要抵扣吗？",
                                    "提示",
                                    System.Windows.Forms.MessageBoxButtons.OKCancel,
                                    System.Windows.Forms.MessageBoxIcon.Information);
                            });
                            return result;
                        });

                        if (confirmResult != System.Windows.Forms.DialogResult.OK)
                        {
                            return ActionResult.CancelResult();
                        }
                    }
                }

                // 执行抵扣操作
                bool success = await _receivablePayableController.ApplyManualPaymentAllocation(
                    selectedReceivable,
                    new List<tb_FM_PreReceivedPayment> { source });

                if (!success)
                {
                    return ActionResult.Fail("抵扣操作失败，请检查数据状态");
                }

                _logger.LogInformation(
                    "预收付款单 {PreRPNO} 成功抵扣应收应付款单 {ARAPNo}",
                    source.PreRPNO,
                    selectedReceivable.ARAPNo);

                var result = ActionResult.SuccessResult();
                result.InfoMessages.Add($"预收付款单 {source.PreRPNO} 成功抵扣应收应付款单 {selectedReceivable.ARAPNo}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "预收付款单 {PreRPNO} 抵扣应收应付款单时发生错误",
                    source?.PreRPNO);
                return ActionResult.Fail($"抵扣操作失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取单据对应的订单ID
        /// 业务逻辑：
        /// 1. 预收款的来源是销售订单，预付款的来源是采购订单
        /// 2. 应收款的来源是销售出库，应付款是采购入库
        /// 3. 需要通过销售出库找到销售订单，采购入库找到采购订单
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
        /// 检查预收付款单和应收应付款单的订单号是否匹配
        /// </summary>
        /// <param name="prePayment">预收付款单</param>
        /// <param name="receivable">应收应付款单</param>
        /// <returns>是否匹配</returns>
        private async Task<bool> CheckOrderMatchedAsync(tb_FM_PreReceivedPayment prePayment, tb_FM_ReceivablePayable receivable)
        {
            try
            {
                var prePaymentOrderId = await GetOrderIdAsync(prePayment.SourceBizType, prePayment.SourceBillId);
                var receivableOrderId = await GetOrderIdAsync(receivable.SourceBizType, receivable.SourceBillId);

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
        /// 显示应收应付款单选择窗体
        /// </summary>
        /// <param name="source">预收付款单</param>
        /// <param name="availableReceivables">可抵扣的应收应付款单列表</param>
        /// <returns>用户选择的应收应付款单，如果取消则返回 null</returns>
        private async Task<tb_FM_ReceivablePayable> ShowReceivableSelectorAsync(tb_FM_PreReceivedPayment source, List<tb_FM_ReceivablePayable> availableReceivables)
        {
            try
            {
                var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                
                // 获取来源单据的订单ID
                var sourceOrderId = await GetOrderIdAsync(source.SourceBizType, source.SourceBillId);

                // 如果结果大于1，则将来源单据对应的订单号相同的应收应付款单排在前面
                if (availableReceivables.Count > 1)
                {
                    // 将订单号相同的应收应付款单排在前面
                    var sortedReceivables = availableReceivables
                        .OrderByDescending(r => IsSameOrder(r, sourceOrderId))
                        .ThenBy(r => r.BusinessDate)
                        .ToList();
                    
                    availableReceivables = sortedReceivables;
                }

                // 使用工厂创建选择器
                var selector = _selectorFactory.CreateSelector<tb_FM_ReceivablePayable>();
                selector.ConfirmButtonText = "抵扣";
                selector.AllowMultiSelect = false;

                // 使用表达式树配置列映射
                selector.ConfigureColumn(x => x.ARAPNo, "单据编号");
                selector.ConfigureColumn(x => x.LocalBalanceAmount, "金额");
                selector.ConfigureColumn(x => x.LocalBalanceAmount, "可用金额");
                selector.ConfigureColumn(x => x.CustomerVendor_ID, "客户");
                selector.ConfigureColumn(x => x.BusinessDate, "单据日期");
                selector.ConfigureColumn(x => x.SourceBizType, "来源业务");
                selector.ConfigureColumn(x => x.SourceBillNo, "来源单号");
                selector.ConfigureSummaryColumn(x => x.LocalBalanceAmount);
                selector.ConfigureSummaryColumn(x => x.LocalBalanceAmount);
                selector.InitializeSelector(availableReceivables, $"选择应{paymentType}单");

                // 在UI线程上显示选择窗体
                tb_FM_ReceivablePayable selectedReceivable = null;
                await System.Threading.Tasks.Task.Run(() =>
                {
                    System.Windows.Forms.Application.OpenForms[0].Invoke((System.Windows.Forms.MethodInvoker)delegate
                    {
                        if (selector.ShowDialog())
                        {
                            selectedReceivable = selector.SelectedItems?.FirstOrDefault();
                        }
                    });
                });

                return selectedReceivable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "显示应收应付款单选择窗体时发生错误");
                return null;
            }
        }

        /// <summary>
        /// 判断应收应付款单是否与指定订单号匹配
        /// </summary>
        /// <param name="receivable">应收应付款单</param>
        /// <param name="orderId">订单ID</param>
        /// <returns>是否匹配</returns>
        private bool IsSameOrder(tb_FM_ReceivablePayable receivable, long? orderId)
        {
            if (!orderId.HasValue)
            {
                return false;
            }

            try
            {
                switch ((BizType)receivable.SourceBizType)
                {
                    case BizType.销售订单:
                    case BizType.采购订单:
                        return receivable.SourceBillId == orderId;

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

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
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "预收付款单不能为空";
                    return result;
                }

                if (source.PrePaymentStatus != (int)PrePaymentStatus.待核销 &&
                    source.PrePaymentStatus != (int)PrePaymentStatus.处理中 &&
                    source.PrePaymentStatus != (int)PrePaymentStatus.混合结清)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.CanConvert = false;
                    result.ErrorMessage = $"预{paymentType}单 {source.PreRPNO} 状态不符合抵扣条件，当前状态为【{((PrePaymentStatus)source.PrePaymentStatus).ToString()}】，只能抵扣【待核销】、【处理中】或【混合结清】状态的预收付款单";
                    return result;
                }

                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过 ||
                    !source.ApprovalResults.HasValue ||
                    !source.ApprovalResults.Value)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"预收付款单 {source.PreRPNO} 未审核通过，无法进行抵扣操作";
                    return result;
                }

                if (source.LocalBalanceAmount <= 0)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"预收付款单 {source.PreRPNO} 的可用余额为0，无法进行抵扣操作";
                    return result;
                }

                var paymentTypeEnum = (ReceivePaymentType)source.ReceivePaymentType;
                result.AddInfo($"预{paymentTypeEnum}单 {source.PreRPNO} 可用余额为 {source.LocalBalanceAmount:F2} (本币)");
                result.AddInfo($"抵扣将生成核销记录，并更新应收应付款单和预收付款单的状态");

                await Task.CompletedTask;
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
