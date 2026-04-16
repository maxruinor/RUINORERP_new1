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
    /// 销售订单取消作废转换器
    /// 负责将已审核的销售订单进行取消作废处理
    /// 这是一个动作操作型转换,执行订单取消的业务操作
    /// </summary>
    [System.ComponentModel.Description("订单取消作废")]
    public class SaleOrderCancelConverter : DocumentConverterBase<tb_SaleOrder, tb_SaleOrder>
    {
        private readonly ILogger<SaleOrderCancelConverter> _logger;
        private readonly tb_SaleOrderController<tb_SaleOrder> _saleOrderController;
        private readonly RUINORERP.Model.Context.ApplicationContext _appContext;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="saleOrderController">销售订单控制器(用于调用核心取消逻辑)</param>
        public SaleOrderCancelConverter(
            ILogger<SaleOrderCancelConverter> logger,
            tb_SaleOrderController<tb_SaleOrder> saleOrderController,
            RUINORERP.Model.Context.ApplicationContext appContext)
            : base(logger)
        {
            _saleOrderController = saleOrderController ?? throw new ArgumentNullException(nameof(saleOrderController));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 转换器显示名称
        /// 直接返回菜单文本,不需要基类的格式
        /// </summary>
        public override string DisplayName => "订单取消作废";

        /// <summary>
        /// 转换操作类型:动作操作型
        /// </summary>
        public override DocumentConversionType ConversionType => DocumentConversionType.ActionOperation;

        /// <summary>
        /// 执行动作操作 - 取消订单
        /// 业务逻辑:
        /// 1. 验证订单状态和关联单据
        /// 2. 处理预收款单(删除或生成退款单)
        /// 3. 更新库存(减少拟销售量)
        /// 4. 标记订单为作废状态
        /// </summary>
        /// <param name="source">源单据:销售订单</param>
        /// <param name="target">目标单据:销售订单(可选,用于传递取消原因)</param>
        /// <returns>操作结果</returns>
        public override async Task<ActionResult> ExecuteActionOperationAsync(tb_SaleOrder source, tb_SaleOrder target = null)
        {
            if (source == null)
            {
                return ActionResult.Fail("销售订单不能为空");
            }

            // 从target.Notes中获取取消原因(如果target不为空)
            string cancelReason = target?.Notes ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(cancelReason))
            {
                return ActionResult.Fail("请提供取消原因");
            }

            try
            {
                // 调用业务层的核心取消订单逻辑
                var result = await _saleOrderController.CancelOrder(source, cancelReason);

                if (!result.Succeeded)
                {
                    return ActionResult.Fail(result.ErrorMsg);
                }

                _logger.LogInformation(
                    "销售订单 {SOrderNo} 成功取消作废,原因:{CancelReason}",
                    source.SOrderNo,
                    cancelReason);

                var actionResult = ActionResult.SuccessResult();
                actionResult.InfoMessages.Add($"销售订单 {source.SOrderNo} 已成功取消作废");
                actionResult.InfoMessages.Add($"取消原因:{cancelReason}");
                return actionResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "销售订单 {SOrderNo} 取消作废时发生错误", source?.SOrderNo);
                return ActionResult.Fail($"取消订单失败:{ex.Message}");
            }
        }

        /// <summary>
        /// 执行单据转换 - 对于动作操作型转换,此方法不执行实际转换
        /// </summary>
        /// <param name="source">源单据:销售订单</param>
        /// <returns>转换后的销售订单(不执行实际转换,返回null)</returns>
        public override async Task<tb_SaleOrder> ConvertAsync(tb_SaleOrder source)
        {
            // 对于动作操作型转换,不执行实际转换
            // 实际操作在ExecuteActionOperationAsync中处理
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 执行具体的转换逻辑 - 重写后不再使用基类的 target 参数模式
        /// </summary>
        /// <param name="source">源单据:销售订单</param>
        /// <param name="target">目标单据:销售订单(不再使用)</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_SaleOrder source, tb_SaleOrder target)
        {
            // 此方法不再使用,逻辑已移至 ExecuteActionOperationAsync
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
                    result.ErrorMessage = $"当前订单{source.SOrderNo}状态为【{(DataStatus)source.DataStatus}】,无法取消作废,只有【确认】状态的订单才能取消";
                    return result;
                }

                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过 ||
                    !source.ApprovalResults.HasValue ||
                    !source.ApprovalResults.Value)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"当前订单{source.SOrderNo}未审核通过,无法取消作废";
                    return result;
                }

                // 检查是否已结案
                if (source.DataStatus == (int)DataStatus.完结)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"当前订单{source.SOrderNo}已结案,无法直接取消,只能通过退货退款处理";
                    return result;
                }

                // 检查是否已生成出库单
                if (source.tb_SaleOuts != null && source.tb_SaleOuts.Count > 0)
                {
                    // 检查是否有已确认、已完结或已审核的出库单
                    bool hasProcessedOut = source.tb_SaleOuts.Any(c =>
                        (c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) &&
                        c.ApprovalStatus == (int)ApprovalStatus.审核通过);

                    if (hasProcessedOut)
                    {
                        result.CanConvert = false;
                        result.ErrorMessage = $"当前订单{source.SOrderNo}已生成并处理了出库单,无法直接取消,请进行退货退款处理";
                        return result;
                    }

                    // 如果有草稿状态的出库单,添加警告
                    if (source.tb_SaleOuts.Any(c => c.DataStatus == (int)DataStatus.草稿))
                    {
                        result.AddWarning("该订单存在草稿状态的出库单,取消订单后将无法继续出库操作");
                    }
                }

                // 检查预收款情况
                var prePaymentController = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                var prePayment = await prePaymentController.IsExistEntityAsync(p => p.SourceBillId == source.SOrder_ID && p.SourceBizType == (int)BizType.销售订单);
                
                if (prePayment != null)
                {
                    var paymentStatus = (PrePaymentStatus)prePayment.PrePaymentStatus;
                    
                    // 根据预收款状态给出不同提示
                    if (paymentStatus == PrePaymentStatus.待核销 ||
                        paymentStatus == PrePaymentStatus.处理中 ||
                        paymentStatus == PrePaymentStatus.混合结清 ||
                        paymentStatus == PrePaymentStatus.全额核销)
                    {
                        result.AddWarning($"该订单存在预收款单{prePayment.PreRPNO},状态为【{paymentStatus}】,取消订单时需要先处理预收款");
                        result.AddInfo("系统将根据配置自动处理预收款退款,或需要手动进行退款操作");
                    }
                }

                result.AddInfo("取消订单后将执行以下操作:");
                result.AddInfo("1. 订单状态变更为【作废】");
                result.AddInfo("2. 释放订单占用的拟销库存");
                result.AddInfo("3. 如有预收款,将根据配置自动退款或提示手动退款");
                result.AddInfo("4. 记录取消原因到订单备注");

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证销售订单取消条件时发生错误,订单号:{SOrderNo}", source?.SOrderNo);
                result.CanConvert = false;
                result.ErrorMessage = $"验证取消条件时发生错误:{ex.Message}";
            }

            return result;
        }
    }
}
