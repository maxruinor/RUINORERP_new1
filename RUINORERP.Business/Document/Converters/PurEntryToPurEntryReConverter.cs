// **********************************************************************
// 采购入库单到采购入库退货单转换器
// 实现采购入库单向采购入库退货单的转换逻辑
// **********************************************************************
using RUINORERP.Model;
using RUINORERP.Business.Document;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Cache;
using RUINORERP.Business.AutoMapper;
using RUINORERP.IServices;
using RUINORERP.Model.Context;
using RUINORERP.Global;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Text;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 采购入库单到采购入库退货单转换器
    /// 负责将采购入库单及其明细转换为采购入库退货单及其明细
    /// 复用业务层的核心转换逻辑，确保数据一致性
    /// </summary>
    public class PurEntryToPurEntryReConverter : DocumentConverterBase<tb_PurEntry, tb_PurEntryRe>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PurEntryToPurEntryReConverter> _logger;
        private readonly IBizCodeGenerateService _bizCodeService;
        private readonly ApplicationContext _appContext;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="mapper">AutoMapper映射器</param>
        /// <param name="bizCodeService">业务编码生成服务</param>
        /// <param name="appContext">应用程序上下文</param>
        public PurEntryToPurEntryReConverter(
            ILogger<PurEntryToPurEntryReConverter> logger,
            IMapper mapper,
            IBizCodeGenerateService bizCodeService,
            ApplicationContext appContext)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bizCodeService = bizCodeService ?? throw new ArgumentNullException(nameof(bizCodeService));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
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
        /// <param name="source">源单据：采购入库单</param>
        /// <param name="target">目标单据：采购入库退货单</param>
        /// <returns>转换后的目标单据</returns>
        protected override async Task PerformConversionAsync(tb_PurEntry source, tb_PurEntryRe target)
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

                // 生成退货单号
                target.PurEntryReNo = await _bizCodeService.GenerateBizBillNoAsync(BizType.采购退货单);
                target.ReturnDate = DateTime.Now;
                target.Notes = $"由采购入库单{source.PurEntryNo}生成";

                // 设置关联信息
                if (source.PurEntryID > 0)
                {
                    target.CustomerVendor_ID = source.CustomerVendor_ID;
                    target.PurEntryID = source.PurEntryID;
                    target.PurEntryNo = source.PurEntryNo;
                }

                // 初始化明细集合
                if (target.tb_PurEntryReDetails == null)
                {
                    target.tb_PurEntryReDetails = new List<tb_PurEntryReDetail>();
                }

                // 转换明细 - 复用业务层核心逻辑
                await ConvertDetailsAsync(source, target);

                // 重新计算汇总字段
                RecalculateSummaryFields(target);

                // 初始化实体
                BusinessHelper.Instance.InitEntity(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "采购入库单到采购入库退货单转换失败，采购入库单号：{PurEntryNo}", source.PurEntryNo);
                throw;
            }
        }

        /// <summary>
        /// 转换明细 - 复用业务层核心逻辑
        /// </summary>
        private async Task ConvertDetailsAsync(tb_PurEntry source, tb_PurEntryRe target)
        {
            var details = _mapper.Map<List<tb_PurEntryReDetail>>(source.tb_PurEntryDetails);
            var newDetails = new List<tb_PurEntryReDetail>();
            var tipsMsg = new List<string>();
            var cacheManager = _appContext.GetRequiredService<IEntityCacheManager>();

            for (int i = 0; i < details.Count; i++)
            {
                // 查找对应的入库明细
                var entryDetail = source.tb_PurEntryDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                if (entryDetail == null)
                {
                    _logger.LogWarning("找不到对应的入库明细，产品明细ID：{ProdDetailID}", details[i].ProdDetailID);
                    continue;
                }

                // 获取产品详情信息
                View_ProdDetail prodDetail = cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                if (prodDetail == null || prodDetail.GetType().Name == "Object")
                {
                    prodDetail = cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                }

                // 计算可退数量 = 入库数量 - 已退回数量
                details[i].Quantity = entryDetail.Quantity - entryDetail.ReturnedQty;
                details[i].SubtotalTrPriceAmount = (details[i].UnitPrice + details[i].CustomizedCost) * details[i].Quantity;

                // 检查是否可退
                if (details[i].Quantity > 0)
                {
                    newDetails.Add(details[i]);
                }
                else
                {
                    tipsMsg.Add($"当前行的SKU:{prodDetail?.SKU}已退回数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                }
            }

            // 检查是否所有明细都已退回
            if (newDetails.Count == 0)
            {
                tipsMsg.Add($"采购入库单:{source.PurEntryNo}已全部退回，请检查是否正在重复操作！");
            }

            // 记录提示信息
            if (tipsMsg.Count > 0)
            {
                _logger.LogInformation("转换过程中的提示信息：{Tips}", string.Join("; ", tipsMsg));
            }

            target.tb_PurEntryReDetails = newDetails;
        }

        /// <summary>
        /// 重新计算汇总字段
        /// </summary>
        private void RecalculateSummaryFields(tb_PurEntryRe target)
        {
            if (target.tb_PurEntryReDetails == null || !target.tb_PurEntryReDetails.Any())
            {
                target.TotalAmount = 0;
                target.TotalQty = 0;
                target.TotalTaxAmount = 0;
                return;
            }

            target.TotalAmount = target.tb_PurEntryReDetails.Sum(c => c.SubtotalTrPriceAmount);
            target.TotalQty = target.tb_PurEntryReDetails.Sum(c => c.Quantity);
            target.TotalTaxAmount = target.tb_PurEntryReDetails.Sum(c => c.TaxAmount);
            // 未税总金额 TotalUntaxedAmount 需要根据业务逻辑计算
        }

        /// <summary>
        /// 验证转换是否可行
        /// </summary>
        /// <param name="source">源单据</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_PurEntry source)
        {
            // 调用基类默认验证
            var result = await base.ValidateConversionAsync(source);
            
            if (result.CanConvert)
            {
                // 检查入库单状态是否为已审核
                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过 || !source.ApprovalResults.GetValueOrDefault())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "只有已审核通过的采购入库单才能生成采购退货单";
                    return result;
                }
                
                // 检查是否有明细
                if (source.tb_PurEntryDetails == null || !source.tb_PurEntryDetails.Any())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "该采购入库单没有明细，无法生成采购退货单";
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
        /// <param name="source">采购入库单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateDetailQuantitiesAsync(tb_PurEntry source, ValidationResult result)
        {
            try
            {
                int totalDetails = source.tb_PurEntryDetails.Count;
                int returnableDetails = 0;
                int nonReturnableDetails = 0;
                decimal totalReturnableQty = 0;
                var cacheManager = _appContext?.GetRequiredService<IEntityCacheManager>();
                
                // 遍历所有入库明细，检查可退数量
                foreach (var detail in source.tb_PurEntryDetails)
                {
                    // 计算可退数量 = 入库数量 - 已退回数量
                    decimal returnableQty = detail.Quantity - detail.ReturnedQty;
                    
                    if (returnableQty > 0)
                    {
                        returnableDetails++;
                        totalReturnableQty += returnableQty;
                        
                        // 如果可退数量小于原入库数量，添加部分退回提示
                        if (returnableQty < detail.Quantity)
                        {
                            var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                            string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                            
                            result.AddWarning($"产品【{prodName}】已退回数量为{detail.ReturnedQty}，可退回数量为{returnableQty}，小于原入库数量{detail.Quantity}");
                        }
                    }
                    else
                    {
                        nonReturnableDetails++;
                        
                        // 添加完全退回提示
                        var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        result.AddWarning($"产品【{prodName}】已全部退回，可退回数量为0，将忽略此明细");
                    }
                }
                
                // 添加汇总提示信息
                if (nonReturnableDetails > 0)
                {
                    result.AddInfo($"共有{nonReturnableDetails}项产品已全部退回，将在转换时忽略");
                }
                
                if (returnableDetails > 0)
                {
                    result.AddInfo($"共有{returnableDetails}项产品可退回，总可退数量为{totalReturnableQty}");
                }
                
                // 如果所有明细都已退回，添加警告但仍允许转换（让用户知道）
                if (returnableDetails == 0)
                {
                    result.AddWarning("该入库单所有明细已全部退回，转换生成的退回单将没有明细数据");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证采购入库单明细数量时发生错误，入库单号：{PurEntryNo}", source?.PurEntryNo);
                result.AddWarning("验证明细数量时发生错误，请检查数据完整性");
            }
        }
    }
}