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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 制令单到领料单转换器
    /// 负责将制令单及其明细转换为领料单及其明细
    /// 复用业务层的核心转换逻辑，确保数据一致性
    /// </summary>
    public class ManufacturingOrderToMaterialRequisitionConverter : DocumentConverterBase<tb_ManufacturingOrder, tb_MaterialRequisition>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ManufacturingOrderToMaterialRequisitionConverter> _logger;
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
        /// <param name="authorizeController">授权控制器</param>
        public ManufacturingOrderToMaterialRequisitionConverter(
            ILogger<ManufacturingOrderToMaterialRequisitionConverter> logger,
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
        }

        /// <summary>
        /// 转换器显示名称
        /// 使用基类实现，从Description特性获取
        /// </summary>
        public override string DisplayName => base.DisplayName;

        /// <summary>
        /// 执行具体的转换逻辑 - 复用业务层核心逻辑
        /// </summary>
        /// <param name="source">源单据：制令单</param>
        /// <param name="target">目标单据：领料单</param>
        /// <returns>转换后的目标单据</returns>
        protected override async Task PerformConversionAsync(tb_ManufacturingOrder source, tb_MaterialRequisition target)
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
                target.Created_at = null;
                target.Created_by = null;

                // 生成领料单号（如果有则使用现有的）
                if (string.IsNullOrEmpty(target.MaterialRequisitionNO))
                {
                    target.MaterialRequisitionNO = await _bizCodeService.GenerateBizBillNoAsync(BizType.生产领料单, CancellationToken.None);
                }
                target.DeliveryDate = DateTime.Now;
                target.Notes = $"由制令单{source.MONO}生成";

                // 设置关联信息
                if (source.MOID > 0)
                {
                    target.MOID = source.MOID;
                    target.MONO = source.MONO;
                    target.Outgoing = source.IsOutSourced;
                    target.CustomerVendor_ID = source.CustomerVendor_ID_Out;
                }

                // 设置预计产量
                target.ExpectedQuantity = source.ManufacturingQty - source.QuantityDelivered;

                // 转换主表字段 - 复用业务层核心逻辑
                ConvertMainFieldsAsync(source, target);

                // 初始化明细集合
                if (target.tb_MaterialRequisitionDetails == null)
                {
                    target.tb_MaterialRequisitionDetails = new List<tb_MaterialRequisitionDetail>();
                }

                // 转换明细 - 复用业务层核心逻辑
                await ConvertDetailsAsync(source, target);

                // 设置关联的制令单对象
                target.tb_manufacturingorder = source;

                // 初始化实体
                BusinessHelper.Instance.InitEntity(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "制令单到领料单转换失败，制令单号：{MONO}", source.MONO);
                throw;
            }
        }

        /// <summary>
        /// 转换主表字段 - 复用业务层核心逻辑
        /// </summary>
        private void ConvertMainFieldsAsync(tb_ManufacturingOrder source, tb_MaterialRequisition target)
        {
            // 复制基础字段
            target.Employee_ID = source.Employee_ID;
            target.DepartmentID = source.DepartmentID;
            
            // 领料单特有字段
            target.DeliveryDate = DateTime.Now;
            target.MOID = source.MOID;
            target.MONO = source.MONO;
            target.Outgoing = source.IsOutSourced;
            
            // 复制备注信息
            target.Notes = source.Notes;
            
            // 创建信息
            target.Created_by = source.Created_by;
            target.Created_at = DateTime.Now;
            target.Modified_by = source.Modified_by;
            target.Modified_at = DateTime.Now;
        }

        /// <summary>
        /// 转换明细 - 复用业务层核心逻辑
        /// </summary>
        private async Task ConvertDetailsAsync(tb_ManufacturingOrder source, tb_MaterialRequisition target)
        {
            var details = _mapper.Map<List<tb_MaterialRequisitionDetail>>(source.tb_ManufacturingOrderDetails);
            var newDetails = new List<tb_MaterialRequisitionDetail>();
            var tipsMsg = new List<string>();
            var cacheManager = _appContext.GetRequiredService<IEntityCacheManager>();

            for (int i = 0; i < details.Count; i++)
            {
                tb_ManufacturingOrderDetail sourceDetail = source.tb_ManufacturingOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID && c.Location_ID == details[i].Location_ID);
                
                if (sourceDetail == null)
                {
                    tipsMsg.Add($"制令单明细不存在，ProdDetailID: {details[i].ProdDetailID}, Location_ID: {details[i].Location_ID}");
                    continue;
                }

                // 计算应发数量：应发数量 = 应发送数量 + 损耗数量 - 已实际发送数量
                decimal shouldQty = sourceDetail.ShouldSendQty + sourceDetail.WastageQty - sourceDetail.ActualSentQty;

                // 处理数量规则：
                // - 等于0时保持0不变
                // - 大于0且小于1时设为1
                // - 大于等于1时向上取整（如1.1取2，2.0保持2）
                if (shouldQty == 0)
                {
                    // 保持0不变
                }
                else if (shouldQty < 1)
                {
                    shouldQty = 1;
                }
                else
                {
                    // 对decimal类型使用Math.Ceiling需要先转换为double，再转回decimal
                    shouldQty = (decimal)Math.Ceiling((double)shouldQty);
                }
                details[i].PrimaryKeyID = 0;
                // 设置应发数量
                details[i].ShouldSendQty = shouldQty.ToInt();

                // 获取库存信息
                var inv = cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                if (inv != null && inv is View_ProdDetail prodDetail)
                {
                    // 获取该库位的库存数量
                    var inventory = cacheManager.GetEntity<View_Inventory>(details[i].Location_ID);
                    if (inventory != null)
                    {
                        if (inventory.Quantity >= details[i].ShouldSendQty)
                        {
                            details[i].ActualSentQty = details[i].ShouldSendQty;
                        }
                        else
                        {
                            details[i].ActualSentQty = 0; // 仓库不够时，暂时默认为0，让手动输入
                        }
                        details[i].CanQuantity = inventory.Quantity.Value;
                    }
                }

                // 设置制令单明细关联ID
                details[i].ManufacturingOrderDetailRowID = sourceDetail.MOCID;

                // 只添加应发数量大于0的明细
                if (details[i].ShouldSendQty > 0)
                {
                    newDetails.Add(details[i]);
                }
                else
                {
                    // 如果指定了是补领则显示
                    if (target.ReApply)
                    {
                        target.ReApply = true;
                        newDetails.Add(details[i]);
                    }
                }
            }

            // 记录提示信息
            if (tipsMsg.Count > 0)
            {
                StringBuilder msg = new StringBuilder();
                foreach (var item in tipsMsg)
                {
                    msg.Append(item).Append("\r\n");
                }
                
                // 在实际应用中，这里可能需要通过事件或其他方式向UI传递提示信息
                // 这里我们记录日志
                _logger.LogWarning("制令单转换提示信息：{Tips}", msg.ToString());
            }

            target.tb_MaterialRequisitionDetails = newDetails;

            // 设置项目组信息（如果有关联的生产计划）
            if (source.PDID.HasValue)
            {
                try
                {
                    var productionDemand = await _appContext.Db.Queryable<tb_ProductionDemand>()
                        .Includes(a => a.tb_productionplan)
                        .Where(c => c.PDID == source.PDID)
                        .SingleAsync();

                    if (productionDemand?.tb_productionplan != null)
                    {
                        target.ProjectGroup_ID = productionDemand.tb_productionplan.ProjectGroup_ID;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "获取生产计划项目组信息失败，制令单号：{MONO}", source.MONO);
                }
            }

            await Task.CompletedTask; // 满足异步方法签名要求
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：制令单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_ManufacturingOrder source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "制令单不能为空";
                    return result;
                }

                // 检查制令单状态
                if (source.DataStatus != (int)DataStatus.确认)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "只能转换已确认的制令单";
                    return result;
                }

                // 检查制令单是否有明细
                if (source.tb_ManufacturingOrderDetails == null || !source.tb_ManufacturingOrderDetails.Any())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "制令单没有明细，无法转换";
                    return result;
                }

                // 检查是否有需要领料的明细
                var hasRequisitionableDetails = source.tb_ManufacturingOrderDetails.Any(d => 
                    d.ShouldSendQty + d.WastageQty - d.ActualSentQty > 0);
                if (!hasRequisitionableDetails)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "制令单所有明细已全部领料，无需转换";
                    return result;
                }

                // 添加明细数量业务验证
                await ValidateDetailQuantitiesAsync(source, result);

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证制令单转换条件时发生错误，制令单号：{MONO}", source?.MONO);
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 验证明细数量的业务逻辑
        /// </summary>
        /// <param name="source">制令单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateDetailQuantitiesAsync(tb_ManufacturingOrder source, ValidationResult result)
        {
            try
            {
                int totalDetails = source.tb_ManufacturingOrderDetails.Count;
                int requisitionableDetails = 0;
                int nonRequisitionableDetails = 0;
                decimal totalRequisitionableQty = 0;
                var cacheManager = _appContext?.GetRequiredService<IEntityCacheManager>();
                
                // 遍历所有制令单明细，检查可领料数量
                foreach (var detail in source.tb_ManufacturingOrderDetails)
                {
                    // 计算可领料数量 = 应发送数量 + 损耗数量 - 已实际发送数量
                    decimal requisitionableQty = detail.ShouldSendQty + detail.WastageQty - detail.ActualSentQty;
                    
                    if (requisitionableQty > 0)
                    {
                        requisitionableDetails++;
                        totalRequisitionableQty += requisitionableQty;
                        
                        // 获取产品信息
                        var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        // 添加领料提示信息
                        result.AddInfo($"产品【{prodName}】可领料数量为{requisitionableQty}");
                    }
                    else
                    {
                        nonRequisitionableDetails++;
                        
                        // 获取产品信息
                        var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        result.AddWarning($"产品【{prodName}】已全部领料，可领料数量为0，将忽略此明细");
                    }
                }
                
                // 添加汇总提示信息
                if (nonRequisitionableDetails > 0)
                {
                    result.AddInfo($"共有{nonRequisitionableDetails}项产品已全部领料，将在转换时忽略");
                }
                
                if (requisitionableDetails > 0)
                {
                    result.AddInfo($"共有{requisitionableDetails}项产品可领料，总可领料数量为{totalRequisitionableQty}");
                }
                
                // 如果所有明细都已领料，添加警告但仍允许转换（让用户知道）
                if (requisitionableDetails == 0)
                {
                    result.AddWarning("该制令单所有明细已全部领料，转换生成的领料单将没有明细数据");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证制令单明细数量时发生错误，制令单号：{MONO}", source?.MONO);
                result.AddWarning("验证明细数量时发生错误，请检查数据完整性");
            }
        }
    }
}