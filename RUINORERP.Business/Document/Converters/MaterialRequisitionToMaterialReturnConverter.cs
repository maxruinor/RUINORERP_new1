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
    /// 领料单到退料单转换器
    /// 负责将领料单及其明细转换为退料单及其明细
    /// 复用业务层的核心转换逻辑，确保数据一致性
    /// </summary>
    public class MaterialRequisitionToMaterialReturnConverter : DocumentConverterBase<tb_MaterialRequisition, tb_MaterialReturn>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialRequisitionToMaterialReturnConverter> _logger;
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
        public MaterialRequisitionToMaterialReturnConverter(
            ILogger<MaterialRequisitionToMaterialReturnConverter> logger,
            IMapper mapper,
            IBizCodeGenerateService bizCodeService,
            ApplicationContext appContext,
            AuthorizeController authorizeController)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bizCodeService = bizCodeService ?? throw new ArgumentNullException(nameof(bizCodeService));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _authorizeController = authorizeController ?? throw new ArgumentNullException(nameof(authorizeController));
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
        /// <param name="source">源单据：领料单</param>
        /// <param name="target">目标单据：退料单</param>
        /// <returns>转换后的目标单据</returns>
        protected override async Task PerformConversionAsync(tb_MaterialRequisition source, tb_MaterialReturn target)
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

                // 设置退料日期
                target.ReturnDate = DateTime.Now;
                target.Notes = $"由领料单{source.MaterialRequisitionNO}生成";

                // 设置外发加工信息
                target.Outgoing = source.Outgoing;
                if (source.CustomerVendor_ID.HasValue)
                {
                    target.CustomerVendor_ID = source.CustomerVendor_ID.Value;
                }

                // 设置领料单关联信息
                target.MR_ID = source.MR_ID;
                target.MaterialRequisitionNO = source.MaterialRequisitionNO;
                target.tb_materialrequisition = source;

                // 生成退料单号
                target.BillNo = await _bizCodeService.GenerateBizBillNoAsync(BizType.退料单, CancellationToken.None);

                // 初始化明细集合
                if (target.tb_MaterialReturnDetails == null)
                {
                    target.tb_MaterialReturnDetails = new List<tb_MaterialReturnDetail>();
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
                _logger.LogError(ex, "领料单到退料单转换失败，领料单号：{MaterialRequisitionNO}", source.MaterialRequisitionNO);
                throw;
            }
        }

        /// <summary>
        /// 转换明细 - 复用业务层核心逻辑
        /// </summary>
        private async Task ConvertDetailsAsync(tb_MaterialRequisition source, tb_MaterialReturn target)
        {
            var details = _mapper.Map<List<tb_MaterialReturnDetail>>(source.tb_MaterialRequisitionDetails);
            var newDetails = new List<tb_MaterialReturnDetail>();
            var tipsMsg = new List<string>();
            var cacheManager = _appContext.GetRequiredService<IEntityCacheManager>();

            for (int i = 0; i < details.Count; i++)
            {
                #region 每行产品ID唯一
                tb_MaterialRequisitionDetail sourceItem = source.tb_MaterialRequisitionDetails
                    .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);

                // 只转换未完全退回的明细
                if (sourceItem.ReturnQty != sourceItem.ActualSentQty)
                {
                    // 计算可退数量 = 实发数量 - 已退数量
                    int returnableQty = sourceItem.ActualSentQty - sourceItem.ReturnQty;
                    
                    // 设置退料数量为可退数量
                    details[i].Quantity = returnableQty;
                    
                    // 设置成本和价格
                    details[i].Cost = sourceItem.Cost;
                    details[i].Price = sourceItem.Price;
                    
                    // 设置其他必要属性
                    //details[i]. = returnableQty * sourceItem.Cost;
                    //details[i].SubtotalAmount = returnableQty * sourceItem.Price;
                    
                    // 如果是补领单，允许添加退料数量为0的明细
                    bool allowZeroQty = source.ReApply && returnableQty == 0;
                    
                    if (details[i].Quantity > 0 || allowZeroQty)
                    {
                        newDetails.Add(details[i]);
                    }
                    else
                    {
                        var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(details[i].ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{details[i].ProdDetailID}";
                        
                        tipsMsg.Add($"领料单{source.MaterialRequisitionNO}，{prodName}已退数量为{sourceItem.ReturnQty}，可退数量为{details[i].Quantity}，当前行数据忽略！");
                    }
                }
                #endregion
            }

            if (newDetails.Count == 0)
            {
                tipsMsg.Add($"领料单:{source.MaterialRequisitionNO}已全部退料，请检查是否正在重复退料！");
                _logger.LogWarning("领料单已全部退料，领料单号：{MaterialRequisitionNO}", source.MaterialRequisitionNO);
            }

            target.tb_MaterialReturnDetails = newDetails;

            // 记录提示信息
            if (tipsMsg.Any())
            {
                // 保留重要日志，移除简单信息记录
            }

            await Task.CompletedTask; // 满足异步方法签名要求
        }

        /// <summary>
        /// 重新计算汇总字段
        /// </summary>
        /// <param name="target">退料单</param>
        private void RecalculateSummaryFields(tb_MaterialReturn target)
        {
            if (target.tb_MaterialReturnDetails == null || !target.tb_MaterialReturnDetails.Any())
            {
                target.TotalQty = 0;
                target.TotalAmount = 0;
                target.TotalCostAmount = 0;
                return;
            }

            // 计算总数量
            target.TotalQty = target.tb_MaterialReturnDetails.Sum(d => d.Quantity);

            // 计算总成本
            target.TotalCostAmount = target.tb_MaterialReturnDetails.Sum(c => c.Cost * c.Quantity);
            
            // 计算总金额（如果有销售额相关字段）
            target.TotalAmount = target.tb_MaterialReturnDetails.Sum(c => c.Price * c.Quantity);
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：领料单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_MaterialRequisition source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "领料单不能为空";
                    return result;
                }

                // 检查领料单状态
                if (source.DataStatus != (int)DataStatus.确认 || source.ApprovalStatus != (int)ApprovalStatus.审核通过)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "只能转换已确认且已审核的领料单";
                    return result;
                }

                // 检查领料单是否有明细
                if (source.tb_MaterialRequisitionDetails == null || !source.tb_MaterialRequisitionDetails.Any())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "领料单没有明细，无法转换";
                    return result;
                }

                // 添加明细数量业务验证
                await ValidateDetailQuantitiesAsync(source, result);
                
                // 检查是否有可退料的明细
                var hasReturnableDetails = source.tb_MaterialRequisitionDetails.Any(d => d.ReturnQty < d.ActualSentQty);
                if (!hasReturnableDetails)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "领料单所有明细已全部退料，无需转换";
                    return result;
                }

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证领料单转换条件时发生错误，领料单号：{MaterialRequisitionNO}", source?.MaterialRequisitionNO);
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }
        
        /// <summary>
        /// 验证明细数量的业务逻辑
        /// </summary>
        /// <param name="source">领料单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateDetailQuantitiesAsync(tb_MaterialRequisition source, ValidationResult result)
        {
            try
            {
                int totalDetails = source.tb_MaterialRequisitionDetails.Count;
                int returnableDetails = 0;
                int nonReturnableDetails = 0;
                decimal totalReturnableQty = 0;
                var cacheManager = _appContext?.GetRequiredService<IEntityCacheManager>();
                
                // 遍历所有领料单明细，检查可退料数量
                foreach (var detail in source.tb_MaterialRequisitionDetails)
                {
                    // 计算可退料数量 = 实发数量 - 已退数量
                    decimal returnableQty = detail.ActualSentQty - detail.ReturnQty;
                    
                    if (returnableQty > 0)
                    {
                        returnableDetails++;
                        totalReturnableQty += returnableQty;
                        
                        // 如果可退料数量小于实发数量，添加部分退料提示
                        if (returnableQty < detail.ActualSentQty)
                        {
                            var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                            string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                            
                            result.AddWarning($"产品【{prodName}】已退数量为{detail.ReturnQty}，可退数量为{returnableQty}，小于实发数量{detail.ActualSentQty}");
                        }
                    }
                    else
                    {
                        nonReturnableDetails++;
                        
                        // 添加完全退料提示
                        var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        result.AddWarning($"产品【{prodName}】已全部退料，可退数量为0，将忽略此明细");
                    }
                }
                
                // 添加汇总提示信息
                if (nonReturnableDetails > 0)
                {
                    result.AddInfo($"共有{nonReturnableDetails}项产品已全部退料，将在转换时忽略");
                }
                
                if (returnableDetails > 0)
                {
                    result.AddInfo($"共有{returnableDetails}项产品可退料，总可退数量为{totalReturnableQty}");
                }
                
                // 如果所有明细都已退料，添加警告但仍允许转换（让用户知道）
                if (returnableDetails == 0)
                {
                    result.AddWarning("该领料单所有明细已全部退料，转换生成的退料单将没有明细数据");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证领料单明细数量时发生错误，领料单号：{MaterialRequisitionNO}", source?.MaterialRequisitionNO);
                result.AddWarning("验证明细数量时发生错误，请检查数据完整性");
            }
        }
    }
}