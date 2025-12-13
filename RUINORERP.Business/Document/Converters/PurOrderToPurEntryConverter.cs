using AutoMapper;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Document;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
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
    /// 采购订单到采购入库单转换器
    /// 负责将采购订单及其明细转换为采购入库单及其明细
    /// 复用业务层的核心转换逻辑，确保数据一致性
    /// </summary>
    public class PurOrderToPurEntryConverter : DocumentConverterBase<tb_PurOrder, tb_PurEntry>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PurOrderToPurEntryConverter> _logger;
        private readonly IBizCodeGenerateService _bizCodeService;
        private readonly ApplicationContext _appContext;
        private readonly AuthorizeController _authorizeController;
        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="mapper">AutoMapper映射器</param>
        /// <param name="bizCodeService">业务编码生成服务</param>
        /// <param name="appContext">应用程序上下文</param>
        /// <param name="prodDetailService">产品详情服务</param>
        /// <param name="inventoryService">库存服务</param>
        public PurOrderToPurEntryConverter(
            ILogger<PurOrderToPurEntryConverter> logger,
            IMapper mapper,
            IBizCodeGenerateService bizCodeService,
            ApplicationContext appContext,
            AuthorizeController authorizeController)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bizCodeService = bizCodeService ?? throw new ArgumentNullException(nameof(bizCodeService));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _authorizeController= authorizeController ?? throw new ArgumentNullException(nameof(authorizeController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 转换器显示名称
        /// 使用基类实现，从Description特性获取
        /// </summary>
        public override string DisplayName => base.DisplayName;
        
        /// <summary>
        /// 执行具体的转换逻辑 - 复用业务层核心逻辑
        /// </summary>
        /// <param name="source">源单据：采购订单</param>
        /// <param name="target">目标单据：采购入库单</param>
        /// <returns>转换后的目标单据</returns>
        protected override async Task PerformConversionAsync(tb_PurOrder source, tb_PurEntry target)
        {
            try
            {

                // 使用AutoMapper进行基础映射
                _mapper.Map(source, target);

                // 重置状态字段 - 与业务层保持一致
                target.ApprovalOpinions = "";
                target.ApprovalResults = null;
                target.DataStatus = (int)DataStatus.草稿;
                target.ApprovalStatus = (int)ApprovalStatus.未审核;
                target.Approver_at = null;
                target.Approver_by = null;
                target.PrintStatus = 0;
                target.ActionStatus = ActionStatus.新增;
                target.Modified_at = null;
                target.Modified_by = null;
                target.Created_by = null;
                target.Created_at = null;

                // 生成入库单号
                target.PurEntryNo = await _bizCodeService.GenerateBizBillNoAsync(BizType.采购入库单, CancellationToken.None);
                target.EntryDate = DateTime.Now;
                target.Notes = $"由采购订单{source.PurOrderNo}生成";

                // 设置关联信息
                if (source.PurOrder_ID > 0)
                {
                    target.CustomerVendor_ID = source.CustomerVendor_ID;
                    target.PurOrder_NO = source.PurOrderNo;
                }

                // 转换主表字段 - 复用业务层核心逻辑
                ConvertMainFieldsAsync(source, target);

                // 初始化明细集合
                if (target.tb_PurEntryDetails == null)
                {
                    target.tb_PurEntryDetails = new List<tb_PurEntryDetail>();
                }

                // 转换明细 - 复用业务层核心逻辑
                await ConvertDetailsAsync(source, target);

                // 处理运费逻辑
                ProcessFreightLogic(source, target);

                // 重新计算汇总字段
                RecalculateSummaryFields(target);
                
                // 要添加外币金额的运费
                target.ForeignTotalAmount = target.ForeignTotalAmount + target.ForeignShipCost;

                // 初始化实体
                BusinessHelper.Instance.InitEntity(target);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "采购订单到采购入库单转换失败，采购订单号：{PurOrderNo}", source.PurOrderNo);
                throw;
            }
        }

        /// <summary>
        /// 转换主表字段 - 复用业务层核心逻辑
        /// </summary>
        private void ConvertMainFieldsAsync(tb_PurOrder source, tb_PurEntry target)
        {
            // 复制基础字段
            target.PurOrder_ID = source.PurOrder_ID;
            target.PayStatus=source.PayStatus;
            target.Paytype_ID=source.Paytype_ID;
            target.PurOrder_NO = source.PurOrderNo;
            target.CustomerVendor_ID = source.CustomerVendor_ID;
            target.Employee_ID = source.Employee_ID;
            target.DepartmentID = source.DepartmentID;
            target.Currency_ID = source.Currency_ID;
            target.ExchangeRate = source.ExchangeRate;
            target.EntryDate = DateTime.Now;
            target.IsIncludeTax = source.IsIncludeTax;
            // 以下字段初始化为0，后面重新计算
            target.TotalAmount = 0;
            target.TotalTaxAmount = 0;
            target.TotalUntaxedAmount = 0;
            target.TotalQty = 0;
            target.ShipCost = source.ShipCost;
            target.Notes = source.Notes; 
            target.Created_by = source.Created_by;
            target.Created_at = DateTime.Now;
            target.Modified_by = source.Modified_by;
            target.Modified_at = DateTime.Now;
          
        }

        /// <summary>
        /// 转换明细 - 复用业务层核心逻辑
        /// </summary>
        private async Task ConvertDetailsAsync(tb_PurOrder source, tb_PurEntry target)
        {
            var details = _mapper.Map<List<tb_PurEntryDetail>>(source.tb_PurOrderDetails);
            var newDetails = new List<tb_PurEntryDetail>();
            var tipsMsg = new List<string>();
            var cacheManager = _appContext.GetRequiredService<IEntityCacheManager>();

            // 检查是否有重复的产品ID
            var duplicateProductIds = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

            for (int i = 0; i < details.Count; i++)
            {
                if (duplicateProductIds.Count > 0 && details[i].PurOrder_ChildID > 0)
                {
                    #region 产品ID可能大于1行，共用料号情况
                    tb_PurOrderDetail item = source.tb_PurOrderDetails
                        .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                        && c.Location_ID == details[i].Location_ID
                        && c.PurOrder_ChildID == details[i].PurOrder_ChildID);

                    View_ProdDetail Prod = cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                    if (Prod != null && Prod.GetType().Name != "Object" && Prod is View_ProdDetail prodDetail)
                    {
                        // 产品信息已获取
                    }
                    else
                    {
                        Prod = cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                    }

                    details[i].Quantity = item.Quantity - item.DeliveredQuantity; // 已经交数量去掉
                    details[i].SubtotalAmount = (details[i].UnitPrice + details[i].CustomizedCost) * details[i].Quantity;
                    details[i].SubtotalUntaxedAmount = (details[i].UntaxedUnitPrice + details[i].UntaxedCustomizedCost) * details[i].Quantity;
                    
                    if (details[i].Quantity > 0)
                    {
                        newDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"订单{source.PurOrderNo}，{Prod.CNName + Prod.Specifications}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                    }
                    #endregion
                }
                else
                {
                    #region 每行产品ID唯一
                    tb_PurOrderDetail item = source.tb_PurOrderDetails
                        .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                        && c.Location_ID == details[i].Location_ID);

                    View_ProdDetail Prod = cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                    if (Prod != null && Prod.GetType().Name != "Object" && Prod is View_ProdDetail prodDetail)
                    {
                        // 产品信息已获取
                    }
                    else
                    {
                        Prod = cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                    }

                    details[i].Quantity = item.Quantity - item.DeliveredQuantity; // 已经交数量去掉
                    details[i].SubtotalAmount = details[i].UnitPrice * details[i].Quantity;
                    details[i].SubtotalUntaxedAmount = details[i].UntaxedUnitPrice * details[i].Quantity;
                    
                    if (details[i].Quantity > 0)
                    {
                        newDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"订单{source.PurOrderNo}，{Prod.CNName}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                    }
                    #endregion
                }
            }

            if (newDetails.Count == 0)
            {
                tipsMsg.Add($"采购订单:{source.PurOrderNo}已全部入库，请检查是否正在重复入库！");
                _logger.LogWarning("采购订单已全部入库，采购订单号：{PurOrderNo}", source.PurOrderNo);
            }

            target.tb_PurEntryDetails = newDetails;

            // 记录提示信息
            if (tipsMsg.Any())
            {
                // 保留重要日志，移除简单信息记录
            }

            await Task.CompletedTask; // 满足异步方法签名要求
        }

 
 
       

        /// <summary>
        /// 处理运费逻辑
        /// </summary>
        private void ProcessFreightLogic(tb_PurOrder source, tb_PurEntry target)
        {
            // 如果源单据有运费，则复制运费信息
            if (source.ShipCost > 0)
            {
                target.ShipCost = source.ShipCost;
            }
            else
            {
                target.ShipCost = 0;
            }


            #region 分摊成本计算

            target.TotalQty = target.tb_PurEntryDetails.Sum(d => d.Quantity);

            //默认认为 订单中的运费收入 就是实际发货的运费成本， 可以手动修改覆盖
            if (target.ShipCost > 0)
            {

                //根据系统设置中的分摊规则来分配运费收入到明细。
                if (_appContext.SysConfig.FreightAllocationRules == (int)FreightAllocationRules.产品数量占比)
                {
                    // 单个产品分摊运费 = 整单运费 ×（该产品数量 ÷ 总产品数量） 
                    foreach (var item in target.tb_PurEntryDetails)
                    {
                        item.AllocatedFreightCost = target.ShipCost * (item.Quantity / target.TotalQty);
                        item.AllocatedFreightCost = item.AllocatedFreightCost.ToRoundDecimalPlaces(_authorizeController.GetMoneyDataPrecision());
                        item.FreightAllocationRules = _appContext.SysConfig.FreightAllocationRules;
                    }
                }
            }


            #endregion

        }

        /// <summary>
        /// 重新计算汇总字段
        /// </summary>
        /// <param name="target">采购入库单</param>
        private void RecalculateSummaryFields(tb_PurEntry target)
        {
            if (target.tb_PurEntryDetails == null || !target.tb_PurEntryDetails.Any())
            {
                target.TotalQty = 0;
                target.TotalAmount = 0;
                target.TotalTaxAmount = 0;
                target.TotalUntaxedAmount = 0;
                return;
            }

            // 计算总数量
            target.TotalQty = target.tb_PurEntryDetails.Sum(d => d.Quantity);

            // 计算总金额（含税）
            target.TotalAmount = target.tb_PurEntryDetails.Sum(c => (c.UnitPrice + c.CustomizedCost) * c.Quantity);

            // 计算总税额
            target.TotalTaxAmount = target.tb_PurEntryDetails.Sum(c => c.TaxAmount);

            // 计算不含税总金额
            target.TotalUntaxedAmount = target.tb_PurEntryDetails.Sum(c => (c.UntaxedUnitPrice + c.UntaxedCustomizedCost) * c.Quantity);

            // 加上运费
            target.TotalAmount = target.TotalAmount + target.ShipCost;


        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：采购订单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_PurOrder source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "采购订单不能为空";
                    return result;
                }

                // 检查采购订单状态
                if (source.DataStatus != (int)DataStatus.确认 || source.ApprovalStatus != (int)ApprovalStatus.审核通过)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "只能转换已确认且已审核的采购订单";
                    return result;
                }

                // 检查采购订单是否有明细
                if (source.tb_PurOrderDetails == null || !source.tb_PurOrderDetails.Any())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "采购订单没有明细，无法转换";
                    return result;
                }

                // 添加明细数量业务验证
                await ValidateDetailQuantitiesAsync(source, result);
                
                // 检查是否有可入库的明细
                var hasEntryableDetails = source.tb_PurOrderDetails.Any(d => d.Quantity > d.DeliveredQuantity);
                if (!hasEntryableDetails)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "采购订单所有明细已全部入库，无需转换";
                    return result;
                }

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证采购订单转换条件时发生错误，采购订单号：{PurOrderNo}", source?.PurOrderNo);
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }
        
        /// <summary>
        /// 验证明细数量的业务逻辑
        /// </summary>
        /// <param name="source">采购订单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateDetailQuantitiesAsync(tb_PurOrder source, ValidationResult result)
        {
            try
            {
                int totalDetails = source.tb_PurOrderDetails.Count;
                int entryableDetails = 0;
                int nonEntryableDetails = 0;
                decimal totalEntryableQty = 0;
                var cacheManager = _appContext?.GetRequiredService<IEntityCacheManager>();
                
                // 遍历所有订单明细，检查可入库数量
                foreach (var detail in source.tb_PurOrderDetails)
                {
                    // 计算可入库数量 = 订单数量 - 已入库数量
                    decimal entryableQty = detail.Quantity - detail.DeliveredQuantity;
                    
                    if (entryableQty > 0)
                    {
                        entryableDetails++;
                        totalEntryableQty += entryableQty;
                        
                        // 如果可入库数量小于原订单数量，添加部分入库提示
                        if (entryableQty < detail.Quantity)
                        {
                            var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                            string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                            
                            result.AddWarning($"产品【{prodName}】已入库数量为{detail.DeliveredQuantity}，可入库数量为{entryableQty}，小于原订单数量{detail.Quantity}");
                        }
                    }
                    else
                    {
                        nonEntryableDetails++;
                        
                        // 添加完全入库提示
                        var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        result.AddWarning($"产品【{prodName}】已全部入库，可入库数量为0，将忽略此明细");
                    }
                }
                
                // 添加汇总提示信息
                if (nonEntryableDetails > 0)
                {
                    result.AddInfo($"共有{nonEntryableDetails}项产品已全部入库，将在转换时忽略");
                }
                
                if (entryableDetails > 0)
                {
                    result.AddInfo($"共有{entryableDetails}项产品可入库，总可入库数量为{totalEntryableQty}");
                }
                
                // 如果所有明细都已入库，添加警告但仍允许转换（让用户知道）
                if (entryableDetails == 0)
                {
                    result.AddWarning("该采购订单所有明细已全部入库，转换生成的入库单将没有明细数据");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证采购订单明细数量时发生错误，采购订单号：{PurOrderNo}", source?.PurOrderNo);
                result.AddWarning("验证明细数量时发生错误，请检查数据完整性");
            }
        }
    }
}