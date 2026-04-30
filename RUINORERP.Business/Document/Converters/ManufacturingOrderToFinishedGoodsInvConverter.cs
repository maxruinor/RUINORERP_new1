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
using RUINORERP.Business.Helpers;

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
        /// 转换唯一标识符
        /// </summary>
        public override string ConversionIdentifier => "Normal";

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
                // 调用业务层核心转换方法
                var controller = _appContext.GetRequiredService<tb_FinishedGoodsInvController<tb_FinishedGoodsInv>>();
                var result = await controller.CreateFromManufacturingOrderAsync(source);
                
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(result.ErrorMsg);
                }
                
                // 将转换结果复制到目标对象
                var convertedEntity = result.ReturnObject;
                
                // 复制主表字段
                target.MOID = convertedEntity.MOID;
                target.MONo = convertedEntity.MONo;
                target.DepartmentID = convertedEntity.DepartmentID;
                target.Employee_ID = convertedEntity.Employee_ID;
                target.IsOutSourced = convertedEntity.IsOutSourced;
                target.CustomerVendor_ID = convertedEntity.CustomerVendor_ID;
                target.DataStatus = convertedEntity.DataStatus;
                target.ApprovalStatus = convertedEntity.ApprovalStatus;
                target.ApprovalResults = convertedEntity.ApprovalResults;
                target.ApprovalOpinions = convertedEntity.ApprovalOpinions;
                target.PrintStatus = convertedEntity.PrintStatus;
                target.ActionStatus = convertedEntity.ActionStatus;
                target.DeliveryDate = convertedEntity.DeliveryDate;
                target.Notes = convertedEntity.Notes;
                target.Created_at = convertedEntity.Created_at;
                target.Created_by = convertedEntity.Created_by;
                
                // 复制明细
                target.tb_FinishedGoodsInvDetails = convertedEntity.tb_FinishedGoodsInvDetails;
                
                // 关联制令单
                target.tb_manufacturingorder = convertedEntity.tb_manufacturingorder;
                
                // 生成缴库单号
                target.DeliveryBillNo = await BizCodeHelper.GenerateBizBillNoWithRetryAsync(
                    _bizCodeService, BizType.缴库单, maxRetries: 3, initialDelayMs: 500, logger: _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "制令单到缴库单转换失败,制令单号: {MONO}", source.MONO);
                throw;
            }
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

                // 检查制令单状态 - 必须已审核通过
                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "只能转换已审核通过的制令单";
                    return result;
                }

                // 检查数据状态 - 必须为确认状态
                if (source.DataStatus != (int)DataStatus.确认)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "制令单数据状态异常，无法转换";
                    return result;
                }

                // 检查是否已结案(结案后不允许再缴库)
                if (source.DataStatus == (int)DataStatus.完结)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "制令单已结案,不允许再生成缴库单";
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