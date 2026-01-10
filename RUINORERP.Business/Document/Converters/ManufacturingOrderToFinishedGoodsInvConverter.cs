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
    /// 制令单到缴库单转换器
    /// 负责将制令单及其明细转换为缴库单及其明细
    /// 复用业务层的核心转换逻辑，确保数据一致性
    /// </summary>
    public class ManufacturingOrderToFinishedGoodsInvConverter : DocumentConverterBase<tb_ManufacturingOrder, tb_FinishedGoodsInv>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ManufacturingOrderToFinishedGoodsInvConverter> _logger;
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
        public ManufacturingOrderToFinishedGoodsInvConverter(
            ILogger<ManufacturingOrderToFinishedGoodsInvConverter> logger,
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
        /// <param name="target">目标单据：缴库单</param>
        /// <returns>转换后的目标单据</returns>
        protected override async Task PerformConversionAsync(tb_ManufacturingOrder source, tb_FinishedGoodsInv target)
        {
            try
            {
                // 使用AutoMapper进行基础映射
                _mapper.Map(source, target);

                // 重置状态字段 - 与业务层保持一致
                target.ApprovalOpinions = "快捷转单";
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

                // 生成缴库单号
                target.DeliveryBillNo = await _bizCodeService.GenerateBizBillNoAsync(BizType.缴库单, CancellationToken.None);
                target.DeliveryDate = DateTime.Now;
                target.Notes = $"由制令单{source.MONO}生成";

                // 设置关联信息
                if (source.MOID > 0)
                {
                    target.MOID = source.MOID;
                    target.MONo = source.MONO;
                    target.DepartmentID = source.DepartmentID;
                    target.IsOutSourced = source.IsOutSourced;
                    
                    // 设置外发工厂
                    if (source.IsOutSourced)
                    {
                        target.CustomerVendor_ID = source.CustomerVendor_ID_Out;
                    }
                    else
                    {
                        target.CustomerVendor_ID = null;
                    }
                }

                // 转换主表字段 - 复用业务层核心逻辑
                ConvertMainFieldsAsync(source, target);

                // 初始化明细集合
                if (target.tb_FinishedGoodsInvDetails == null)
                {
                    target.tb_FinishedGoodsInvDetails = new List<tb_FinishedGoodsInvDetail>();
                }

                // 转换明细 - 复用业务层核心逻辑
                await ConvertDetailsAsync(source, target);

                // 重新计算汇总字段
                RecalculateSummaryFields(target);

                // 初始化实体
                BusinessHelper.Instance.InitEntity(target);

                // 设置关联的制令单对象
                target.tb_manufacturingorder = source;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "制令单到缴库单转换失败，制令单号：{MONO}", source.MONO);
                throw;
            }
        }

        /// <summary>
        /// 转换主表字段 - 复用业务层核心逻辑
        /// </summary>
        private void ConvertMainFieldsAsync(tb_ManufacturingOrder source, tb_FinishedGoodsInv target)
        {
            // 复制基础字段
            target.Employee_ID = source.Employee_ID;
            target.DepartmentID = source.DepartmentID;
            
            // 缴库单特有字段
            target.DeliveryDate = DateTime.Now;
            target.MOID = source.MOID;
            target.MONo = source.MONO;
            target.IsOutSourced = source.IsOutSourced;
            
            // 以下字段初始化为0，后面重新计算
            target.TotalQty = 0;
            
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
        private async Task ConvertDetailsAsync(tb_ManufacturingOrder source, tb_FinishedGoodsInv target)
        {
            var newDetails = new List<tb_FinishedGoodsInvDetail>();
            var tipsMsg = new List<string>();
            var cacheManager = _appContext.GetRequiredService<IEntityCacheManager>();

            // 一个制令单就一个成品，就一行数据
            tb_FinishedGoodsInvDetail newDetail = _mapper.Map<tb_FinishedGoodsInvDetail>(source);
            newDetail.PrimaryKeyID=0; // 新增明细，主键ID设为0
            // 计算应缴数量 = 生产数量 - 已交付数量
            newDetail.PayableQty = source.ManufacturingQty - source.QuantityDelivered;
            newDetail.Qty = 0; // 实缴数量初始化为0，由用户手动输入
            newDetail.UnpaidQty = newDetail.PayableQty - newDetail.Qty; // 未缴数量 = 应缴数量 - 实缴数量
            newDetail.Location_ID = source.Location_ID;
            
            // 计算单位成本
            // 根据制令单的时间费用假设全缴库时算出单位时间，再手动输入实缴时再算
            newDetail.NetWorkingHours = Math.Round(source.WorkingHour / source.ManufacturingQty, 4);
            newDetail.NetMachineHours = Math.Round(source.MachineHour / source.ManufacturingQty, 4);
            newDetail.MaterialCost = Math.Round(source.TotalMaterialCost / source.ManufacturingQty, 4);
            newDetail.ManuFee = Math.Round(source.TotalManuFee / source.ManufacturingQty, 4);
            newDetail.ApportionedCost = Math.Round(source.ApportionedCost / source.ManufacturingQty, 4);
            
            newDetail.UnitCost = newDetail.MaterialCost + newDetail.ManuFee + newDetail.ApportionedCost;
            newDetail.ProductionAllCost = Math.Round(newDetail.UnitCost * newDetail.Qty, 4);

            if (newDetail.PayableQty > 0)
            {
                newDetails.Add(newDetail);
            }
            else
            {
                tipsMsg.Add($"制令单:{source.MONO}已全部缴库，请检查数据！");
                _logger.LogWarning("制令单已全部缴库，制令单号：{MONO}", source.MONO);
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

            target.tb_FinishedGoodsInvDetails = newDetails;

            await Task.CompletedTask; // 满足异步方法签名要求
        }

        /// <summary>
        /// 重新计算汇总字段
        /// </summary>
        /// <param name="target">缴库单</param>
        private void RecalculateSummaryFields(tb_FinishedGoodsInv target)
        {
            if (target.tb_FinishedGoodsInvDetails == null || !target.tb_FinishedGoodsInvDetails.Any())
            {
                target.TotalQty = 0;
                return;
            }

            // 计算总数量
            target.TotalQty = target.tb_FinishedGoodsInvDetails.Sum(d => d.Qty);
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

                // 检查制令单是否有可缴库的数量
                if (source.ManufacturingQty <= source.QuantityDelivered)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "制令单已全部缴库，无需转换";
                    return result;
                }

                // 检查是否有可缴库的数量
                if (source.QuantityDelivered < source.ManufacturingQty)
                {
                    int payableQty = source.ManufacturingQty - source.QuantityDelivered;
                    result.AddInfo($"制令单可缴库数量为{payableQty}，小于生产数量{source.ManufacturingQty}");
                }

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
    }
}