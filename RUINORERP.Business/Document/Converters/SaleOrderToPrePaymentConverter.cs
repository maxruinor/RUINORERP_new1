using Microsoft.Extensions.Logging;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Document;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Model.UI;
using System;
using System.Threading.Tasks;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 销售订单到预收款单转换器
    /// 负责将销售订单转换为预收款单,用于收取客户预付款
    /// 这是一个动作操作型转换,需要UI层先获取预收金额,然后执行生成操作
    /// </summary>
    [System.ComponentModel.Description("预收货款")]
    public class SaleOrderToPrePaymentConverter : DocumentConverterBase<tb_SaleOrder, tb_FM_PreReceivedPayment>
    {
        private readonly ILogger<SaleOrderToPrePaymentConverter> _logger;
        private readonly tb_SaleOrderController<tb_SaleOrder> _saleOrderController;
        private readonly ApplicationContext _appContext;
        private readonly IAmountInputSelectorFactory _amountSelectorFactory;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="saleOrderController">销售订单控制器(用于调用核心转换逻辑)</param>
        /// <param name="appContext">应用上下文</param>
        /// <param name="amountSelectorFactory">金额输入选择器工厂</param>
        public SaleOrderToPrePaymentConverter(
            ILogger<SaleOrderToPrePaymentConverter> logger,
            tb_SaleOrderController<tb_SaleOrder> saleOrderController,
            ApplicationContext appContext,
            IAmountInputSelectorFactory amountSelectorFactory)
            : base(logger)
        {
            _saleOrderController = saleOrderController ?? throw new ArgumentNullException(nameof(saleOrderController));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _amountSelectorFactory = amountSelectorFactory ?? throw new ArgumentNullException(nameof(amountSelectorFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 转换器显示名称
        /// 直接返回菜单文本,不需要基类的格式
        /// </summary>
        public override string DisplayName => "预收货款";

        /// <summary>
        /// 转换操作类型:动作操作型(需要先输入金额)
        /// </summary>
        public override DocumentConversionType ConversionType => DocumentConversionType.ActionOperation;

        /// <summary>
        /// 执行动作操作 - 预收货款
        /// 业务逻辑:
        /// 1. 弹出金额输入框获取预收金额
        /// 2. 验证订单状态和金额
        /// 3. 调用ManualPrePayment生成预收款单
        /// 4. UI层根据返回的预收款单打开编辑界面
        /// </summary>
        /// <param name="source">源单据:销售订单</param>
        /// <param name="target">目标单据:预收款单(未使用)</param>
        /// <returns>操作结果</returns>
        public override async Task<ActionResult> ExecuteActionOperationAsync(tb_SaleOrder source, tb_FM_PreReceivedPayment target = null)
        {
            if (source == null)
            {
                return ActionResult.Fail("销售订单不能为空");
            }

            try
            {
                // 计算建议金额(订单总额 - 已收定金)
                decimal suggestedAmount = source.TotalAmount - source.Deposit;
                
                // 创建金额输入选择器
                var selector = _amountSelectorFactory.CreateSelector();
                selector.SelectorTitle = "预收货款";
                selector.PromptText = $"请输入预收金额\n订单总额:{source.TotalAmount:F2}\n已收定金:{source.Deposit:F2}";
                selector.MinAmount = 0.01m;
                selector.MaxAmount = 0; // 无上限,允许超额
                selector.AllowExcess = true; // 允许超额收款
                selector.SuggestedAmount = suggestedAmount > 0 ? suggestedAmount : (decimal?)null;

                // 在UI线程上显示输入窗体
                bool userConfirmed = false;
                decimal prepaidAmount = 0;
                
                await System.Threading.Tasks.Task.Run(() =>
                {
                    System.Windows.Forms.Application.OpenForms[0].Invoke((System.Windows.Forms.MethodInvoker)delegate
                    {
                        if (selector.ShowDialog())
                        {
                            userConfirmed = true;
                            prepaidAmount = selector.InputAmount;
                        }
                    });
                });

                // 用户取消操作
                if (!userConfirmed)
                {
                    return ActionResult.CancelResult();
                }

                // 验证金额
                if (prepaidAmount <= 0)
                {
                    return ActionResult.Fail("预收金额必须大于0");
                }

                // 检查是否超额
                if (suggestedAmount > 0 && prepaidAmount > suggestedAmount)
                {
                    _logger.LogWarning(
                        "销售订单 {SOrderNo} 预收金额{PrepaidAmount}超过建议金额{SuggestedAmount}",
                        source.SOrderNo,
                        prepaidAmount,
                        suggestedAmount);
                }

                // 调用业务层的核心转换逻辑
                var result = await _saleOrderController.ManualPrePayment(prepaidAmount, source);

                if (!result.Succeeded)
                {
                    return ActionResult.Fail(result.ErrorMsg);
                }

                _logger.LogInformation(
                    "销售订单 {SOrderNo} 成功生成预收款单 {PreRPNO},预收金额:{Amount}",
                    source.SOrderNo,
                    result.ReturnObject?.PreRPNO,
                    prepaidAmount);

                var actionResult = ActionResult.SuccessResult();
                actionResult.InfoMessages.Add($"销售订单 {source.SOrderNo} 已成功生成预收款单");
                actionResult.InfoMessages.Add($"预收金额:{prepaidAmount:F2}");
                
                // 将生成的预收款单存储到ActionResult的扩展数据中
                // 注意:需要在ActionResult中添加一个扩展属性来存储返回对象
                // 这里暂时通过InfoMessages传递关键信息
                actionResult.InfoMessages.Add($"预收款单号:{result.ReturnObject?.PreRPNO}");
                
                return actionResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "销售订单 {SOrderNo} 预收货款时发生错误", source?.SOrderNo);
                return ActionResult.Fail($"预收货款失败:{ex.Message}");
            }
        }

        /// <summary>
        /// 执行具体的转换逻辑 - 重写后不再使用基类的 target 参数模式
        /// </summary>
        /// <param name="source">源单据:销售订单</param>
        /// <param name="target">目标单据:预收款单(不再使用)</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_SaleOrder source, tb_FM_PreReceivedPayment target)
        {
            // 此方法不再使用,逻辑已移至 ConvertAsync
            // 保留此方法以满足抽象类要求
            await Task.CompletedTask;
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据:销售订单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_SaleOrder source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null || source.SOrder_ID <= 0)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "销售订单不存在或无效";
                    return result;
                }

                // 检查订单状态 - 必须是已确认且已审核通过的订单
                if (source.DataStatus != (int)DataStatus.确认)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"当前订单{source.SOrderNo}状态为【{(DataStatus)source.DataStatus}】,无法进行预收款,只有【确认】状态的订单才能预收款";
                    return result;
                }

                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过 ||
                    !source.ApprovalResults.HasValue ||
                    !source.ApprovalResults.Value)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"当前订单{source.SOrderNo}未审核通过,无法进行预收款";
                    return result;
                }

                // 检查付款方式配置
                if (_appContext.PaymentMethodOfPeriod == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "系统未配置付款方式信息,请先配置付款方式";
                    return result;
                }

                // 检查账期付款方式的特殊规则
                if (source.Paytype_ID == _appContext.PaymentMethodOfPeriod.Paytype_ID)
                {
                    // 如果是账期付款方式,必须是未付款状态
                    if (source.PayStatus != (int)PayStatus.未付款)
                    {
                        result.CanConvert = false;
                        result.ErrorMessage = $"付款方式为账期的订单必须是未付款状态才能预收款,当前付款状态为【{(PayStatus)source.PayStatus}】";
                        return result;
                    }
                }
                else
                {
                    // 如果不是账期付款方式,但状态是未付款,则不符合规则
                    if (source.PayStatus == (int)PayStatus.未付款)
                    {
                        result.CanConvert = false;
                        result.ErrorMessage = "未付款订单的付款方式必须是账期";
                        return result;
                    }
                }

                // 添加订单基本信息提示
                result.AddInfo($"订单号:{source.SOrderNo}");
                result.AddInfo($"客户:{source.tb_customervendor?.CVName ?? "未知"}");
                result.AddInfo($"订单总金额:{source.TotalAmount:F2}");
                result.AddInfo($"已收定金:{source.Deposit:F2}");
                
                // 计算可收金额上限
                decimal maxPrepayment = source.TotalAmount - source.Deposit;
                if (maxPrepayment > 0)
                {
                    result.AddInfo($"建议预收金额不超过:{maxPrepayment:F2}(订单总额-已收定金)");
                }
                else
                {
                    result.AddWarning("订单已全额收款或超额收款,继续预收款将产生超额收款");
                }

                result.AddInfo("预收款后将生成预收款单,可在财务模块中查看和管理");

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证销售订单预收款条件时发生错误,订单号:{SOrderNo}", source?.SOrderNo);
                result.CanConvert = false;
                result.ErrorMessage = $"验证预收款条件时发生错误:{ex.Message}";
            }

            return result;
        }
    }
}
