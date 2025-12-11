// **********************************************************************
// 销售订单到销售出库单转换器
// 实现销售订单向销售出库单的转换逻辑
// **********************************************************************
using RUINORERP.Model;
using RUINORERP.Business.Document;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using RUINORERP.Model.Context;
using RUINORERP.IServices;
using System.Threading;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 销售订单到销售出库单转换器
    /// 负责将销售订单及其明细转换为销售出库单及其明细
    /// 复用原有业务逻辑，确保转换过程的业务一致性
    /// </summary>
    public class SaleOrderToSaleOutConverter : DocumentConverterBase<tb_SaleOrder, tb_SaleOut>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SaleOrderToSaleOutConverter> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly AuthorizeController _authorizeController;
        private readonly ApplicationContext _appContext;

        /// <summary>
        /// 构造函数，注入必要的依赖服务
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="serviceProvider">服务提供者</param>
        public SaleOrderToSaleOutConverter(
            ILogger<SaleOrderToSaleOutConverter> logger, IServiceProvider serviceProvider)
            : base(logger)
        {
            _serviceProvider = serviceProvider;
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _authorizeController = serviceProvider.GetRequiredService<AuthorizeController>();
            _appContext = serviceProvider.GetRequiredService<ApplicationContext>();
        }

     
        /// <summary>
        /// 转换器显示名称
        /// 使用基类实现，从Description特性获取
        /// </summary>
        public override string DisplayName => base.DisplayName;

        /// <summary>
        /// 执行具体的转换逻辑
        /// 复用原有控制器的转换逻辑，确保业务一致性
        /// </summary>
        /// <param name="source">源销售订单</param>
        /// <param name="target">目标销售出库单</param>
        /// <returns>转换后的目标单据</returns>
        protected override async Task PerformConversionAsync(tb_SaleOrder source, tb_SaleOut target)
        {
            try
            {
                _logger?.LogInformation($"开始销售订单到销售出库单转换，订单ID: {source.SOrder_ID}");

                // 使用AutoMapper进行基础属性映射
                _mapper.Map(source, target);

                // 重置业务状态，确保新单据处于正确状态
                ResetBusinessStatus(target);

                // 生成出库单号
                await GenerateSaleOutNoAsync(target);

                // 处理明细转换（核心逻辑）
                await ConvertOrderDetailsAsync(source, target);

                // 处理运费分摊
                await AllocateFreightAsync(source, target);

                // 设置关联信息
                target.SOrder_ID = source.SOrder_ID;
                target.SaleOrderNo = source.SOrderNo;
                target.PlatformOrderNo = source.PlatformOrderNo;
                target.IsFromPlatform = source.IsFromPlatform;
                target.CustomerVendor_ID = source.CustomerVendor_ID;

                // 设置日期信息
                target.OutDate = DateTime.Now;
                target.DeliveryDate = DateTime.Now;

                // 计算汇总信息
                await CalculateSummaryAsync(target);

                // 初始化实体
                BusinessHelper.Instance.InitEntity(target);

                _logger?.LogInformation($"销售订单到销售出库单转换完成，生成出库单号: {target.SaleOutNo}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"销售订单转换失败，订单ID: {source.SOrder_ID}");
                throw new InvalidOperationException($"销售订单转换失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 重置业务状态，确保新单据处于草稿状态
        /// </summary>
        private void ResetBusinessStatus(tb_SaleOut target)
        {
            target.ApprovalOpinions = "由销售订单自动生成";
            target.ApprovalResults = null;
            target.DataStatus = (int)DataStatus.草稿;
            target.ApprovalStatus = (int)ApprovalStatus.未审核;
            target.Approver_at = null;
            target.Approver_by = null;
            target.PrintStatus = 0;
            target.ActionStatus = ActionStatus.新增;
            target.Modified_at = null;
            target.Modified_by = null;
        }

        /// <summary>
        /// 生成出库单号
        /// </summary>
        private async Task GenerateSaleOutNoAsync(tb_SaleOut target)
        {
            var bizCodeService = _serviceProvider.GetRequiredService<IBizCodeGenerateService>();
            target.SaleOutNo = await bizCodeService.GenerateBizBillNoAsync(BizType.销售出库单, CancellationToken.None);
        }

        /// <summary>
        /// 转换订单明细（核心逻辑）
        /// 复用原有控制器的明细处理逻辑
        /// </summary>
        private async Task ConvertOrderDetailsAsync(tb_SaleOrder source, tb_SaleOut target)
        {
            if (source.tb_SaleOrderDetails == null || !source.tb_SaleOrderDetails.Any())
            {
                throw new InvalidOperationException("销售订单没有明细，无法生成销售出库单");
            }

            var details = _mapper.Map<List<tb_SaleOutDetail>>(source.tb_SaleOrderDetails);
            var newDetails = new List<tb_SaleOutDetail>();
            var tipsMsg = new List<string>();

            // 处理多行相同产品的情况
            for (int i = 0; i < details.Count; i++)
            {
                View_ProdDetail obj = null;
                
                // 检查是否有重复产品
                var duplicateProducts = details
                    .Select(c => c.ProdDetailID)
                    .ToList()
                    .GroupBy(x => x)
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .ToList();

                bool hasDuplicate = duplicateProducts.Contains(details[i].ProdDetailID);

                if (hasDuplicate && details[i].SaleOrderDetail_ID > 0)
                {
                    await ProcessDuplicateProductDetail(source, details, i, newDetails, tipsMsg);
                }
                else
                {
                    await ProcessUniqueProductDetail(source, details, i, newDetails, tipsMsg);
                }
            }

            // 检查是否所有明细都已出库
            if (newDetails.Count == 0)
            {
                tipsMsg.Add($"订单:{source.SOrderNo}已全部出库，请检查是否正在重复出库！");
                _logger?.LogWarning($"销售订单{source.SOrderNo}已全部出库，可能正在重复出库");
            }

            // 处理运费逻辑
            await ProcessFreightLogic(source, target, newDetails, tipsMsg);

            target.tb_SaleOutDetails = newDetails;
            target.TotalQty = newDetails.Sum(c => c.Quantity);
        }

        /// <summary>
        /// 处理重复产品明细
        /// </summary>
        private async Task ProcessDuplicateProductDetail(tb_SaleOrder source, List<tb_SaleOutDetail> details, int index, 
            List<tb_SaleOutDetail> newDetails, List<string> tipsMsg)
        {
            var detail = details[index];
            var orderDetail = source.tb_SaleOrderDetails.FirstOrDefault(c => 
                c.ProdDetailID == detail.ProdDetailID &&
                c.Location_ID == detail.Location_ID &&
                c.SaleOrderDetail_ID == detail.SaleOrderDetail_ID);

            if (orderDetail == null) return;

            // 获取成本信息
            await SetDetailCostInfo(detail, orderDetail);

            // 计算可出库数量
            detail.Quantity = orderDetail.Quantity - orderDetail.TotalDeliveredQty;
            detail.SubtotalTransAmount = detail.TransactionPrice * detail.Quantity;
            detail.SubtotalCostAmount = (detail.Cost + detail.CustomizedCost) * detail.Quantity;

            if (detail.Quantity > 0)
            {
                newDetails.Add(detail);
            }
            else
            {
                var prodDetail = await GetProductDetailInfo(detail.ProdDetailID);
                tipsMsg.Add($"销售订单{source.SOrderNo}，{prodDetail?.CNName} {prodDetail?.Specifications}已出库数为{orderDetail.TotalDeliveredQty}，可出库数为{detail.Quantity}，当前行数据忽略！");
            }
        }

        /// <summary>
        /// 处理唯一产品明细
        /// </summary>
        private async Task ProcessUniqueProductDetail(tb_SaleOrder source, List<tb_SaleOutDetail> details, int index,
            List<tb_SaleOutDetail> newDetails, List<string> tipsMsg)
        {
            var detail = details[index];
            var orderDetail = source.tb_SaleOrderDetails.FirstOrDefault(c =>
                c.ProdDetailID == detail.ProdDetailID &&
                c.Location_ID == detail.Location_ID &&
                c.SaleOrderDetail_ID == detail.SaleOrderDetail_ID);

            if (orderDetail == null) return;

            // 获取成本信息
            await SetDetailCostInfo(detail, orderDetail);

            // 计算可出库数量
            detail.Quantity = detail.Quantity - orderDetail.TotalDeliveredQty;
            detail.SubtotalTransAmount = detail.TransactionPrice * detail.Quantity;
            detail.SubtotalCostAmount = (detail.Cost + detail.CustomizedCost) * detail.Quantity;

            if (detail.Quantity > 0)
            {
                newDetails.Add(detail);
            }
            else
            {
                var prodDetail = await GetProductDetailInfo(detail.ProdDetailID);
                tipsMsg.Add($"当前订单的SKU:{prodDetail?.SKU} {prodDetail?.CNName}已出库数量为{detail.Quantity}，当前行数据将不会加载到明细！");
            }
        }

        /// <summary>
        /// 设置明细成本信息
        /// </summary>
        private async Task SetDetailCostInfo(tb_SaleOutDetail detail, tb_SaleOrderDetail orderDetail)
        {
            detail.Cost = orderDetail.Cost;
            detail.CustomizedCost = orderDetail.CustomizedCost;

            // 如果订单中没有成本，尝试从库存中获取
            if (detail.Cost == 0)
            {
                var prodDetail = await GetProductDetailInfo(detail.ProdDetailID);
                if (prodDetail != null && prodDetail.Inv_Cost.HasValue)
                {
                    detail.Cost = prodDetail.Inv_Cost.Value;
                }
            }
        }

        /// <summary>
        /// 获取产品详细信息
        /// </summary>
        private async Task<View_ProdDetail> GetProductDetailInfo(long prodDetailId)
        {
            return await Task.FromResult(Cache.EntityCacheHelper.GetEntity<View_ProdDetail>(prodDetailId));
        }

        /// <summary>
        /// 处理运费逻辑
        /// </summary>
        private async Task ProcessFreightLogic(tb_SaleOrder source, tb_SaleOut target, List<tb_SaleOutDetail> newDetails, List<string> tipsMsg)
        {
            // 如果这个订单已经有出库单，则第二次运费为0
            if (source.tb_SaleOuts != null && source.tb_SaleOuts.Count > 0)
            {
                if (source.FreightIncome > 0)
                {
                    tipsMsg.Add($"当前订单已经有出库记录，运费收入已经计入前面出库单，当前出库运费收入为零！");
                    target.FreightIncome = 0;
                }
                else
                {
                    tipsMsg.Add($"当前订单已经有出库记录！");
                }
            }

            // 默认认为订单中的运费收入就是实际发货的运费成本
            if (target.FreightIncome > 0)
            {
                target.FreightCost = target.FreightIncome;
            }
        }

        /// <summary>
        /// 运费分摊
        /// </summary>
        private async Task AllocateFreightAsync(tb_SaleOrder source, tb_SaleOut target)
        {
            if (target.FreightIncome <= 0 || target.tb_SaleOutDetails == null || !target.tb_SaleOutDetails.Any())
            {
                return;
            }

            // 根据系统设置中的分摊规则来分配运费收入到明细
            if (_appContext.SysConfig.FreightAllocationRules == (int)FreightAllocationRules.产品数量占比)
            {
                // 单个产品分摊运费 = 整单运费 ×（该产品数量 ÷ 总产品数量）
                foreach (var item in target.tb_SaleOutDetails)
                {
                    item.AllocatedFreightIncome = target.FreightIncome * (item.Quantity.ToDecimal() / source.TotalQty.ToDecimal());
                    item.AllocatedFreightIncome = item.AllocatedFreightIncome.ToRoundDecimalPlaces(_authorizeController.GetMoneyDataPrecision());
                    item.FreightAllocationRules = _appContext.SysConfig.FreightAllocationRules;
                }
            }
        }

        /// <summary>
        /// 计算汇总信息
        /// </summary>
        private async Task CalculateSummaryAsync(tb_SaleOut target)
        {
            if (target.tb_SaleOutDetails == null || !target.tb_SaleOutDetails.Any())
            {
                return;
            }

            // 计算总成本、总金额等汇总信息
            target.TotalCost = target.tb_SaleOutDetails.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity);
            target.TotalCost = target.TotalCost + target.FreightCost;

            target.TotalTaxAmount = target.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount);
            target.TotalTaxAmount = target.TotalTaxAmount.ToRoundDecimalPlaces(_authorizeController.GetMoneyDataPrecision());

            target.TotalAmount = target.tb_SaleOutDetails.Sum(c => c.TransactionPrice * c.Quantity);
            target.TotalAmount = target.TotalAmount + target.FreightIncome;
        }

        /// <summary>
        /// 验证转换条件是否满足
        /// </summary>
        /// <param name="source">源销售订单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_SaleOrder source)
        {
            // 调用基类默认验证
            var result = await base.ValidateConversionAsync(source);
            
            if (result.CanConvert)
            {
                // 验证销售订单是否存在
                if (source == null || source.SOrder_ID <= 0)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "销售订单不存在或无效";
                    return result;
                }

                // 验证销售订单状态是否允许生成出库单
                if (source.ApprovalStatus != (int)ApprovalStatus.已审核)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "仅已审核的销售订单可以生成销售出库单";
                    return result;
                }

                // 验证销售订单是否有明细
                if (source.tb_SaleOrderDetails == null || !source.tb_SaleOrderDetails.Any())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "销售订单没有明细，无法生成销售出库单";
                    return result;
                }

                // 添加明细数量业务验证
                await ValidateDetailQuantitiesAsync(source, result);
            }
            
            return result;
        }
        
        /// <summary>
        /// 验证明细数量的业务逻辑
        /// </summary>
        /// <param name="source">销售订单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateDetailQuantitiesAsync(tb_SaleOrder source, ValidationResult result)
        {
            try
            {
                int totalDetails = source.tb_SaleOrderDetails.Count;
                int deliverableDetails = 0;
                int nonDeliverableDetails = 0;
                decimal totalDeliverableQty = 0;
                
                // 遍历所有订单明细，检查可出库数量
                foreach (var detail in source.tb_SaleOrderDetails)
                {
                    // 计算可出库数量 = 订单数量 - 已出库数量
                    decimal deliverableQty = detail.Quantity - detail.TotalDeliveredQty;
                    
                    if (deliverableQty > 0)
                    {
                        deliverableDetails++;
                        totalDeliverableQty += deliverableQty;
                        
                        // 如果可出库数量小于原订单数量，添加部分出库提示
                        if (deliverableQty < detail.Quantity)
                        {
                            var prodInfo = Cache.EntityCacheHelper.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                            string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                            
                            result.AddWarning($"产品【{prodName}】已出库数量为{detail.TotalDeliveredQty}，可出库数量为{deliverableQty}，小于原订单数量{detail.Quantity}");
                        }
                    }
                    else
                    {
                        nonDeliverableDetails++;
                        
                        // 添加完全出库提示
                        var prodInfo = Cache.EntityCacheHelper.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        result.AddWarning($"产品【{prodName}】已全部出库，可出库数量为0，将忽略此明细");
                    }
                }
                
                // 添加汇总提示信息
                if (nonDeliverableDetails > 0)
                {
                    result.AddInfo($"共有{nonDeliverableDetails}项产品已全部出库，将在转换时忽略");
                }
                
                if (deliverableDetails > 0)
                {
                    result.AddInfo($"共有{deliverableDetails}项产品可出库，总可出库数量为{totalDeliverableQty}");
                }
                
                // 如果所有明细都已出库，设置错误但仍允许转换（让用户知道）
                if (deliverableDetails == 0)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "销售订单所有明细已全部出库，无法再次生成出库单";
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证销售订单明细数量时发生错误，订单号：{SOrderNo}", source?.SOrderNo);
                result.AddWarning("验证明细数量时发生错误，请检查数据完整性");
            }
        }
    }
}