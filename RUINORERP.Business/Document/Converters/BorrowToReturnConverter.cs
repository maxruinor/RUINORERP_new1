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
using SharpYaml.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 借出单到归还单转换器
    /// 负责将借出单及其明细转换为归还单及其明细
    /// 复用业务层的核心转换逻辑，确保数据一致性
    /// </summary>
    public class BorrowToReturnConverter : DocumentConverterBase<tb_ProdBorrowing, tb_ProdReturning>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BorrowToReturnConverter> _logger;
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
        public BorrowToReturnConverter(
            ILogger<BorrowToReturnConverter> logger,
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
        /// <param name="source">源单据：借出单</param>
        /// <param name="target">目标单据：归还单</param>
        /// <returns>转换后的目标单据</returns>
        protected override async Task PerformConversionAsync(tb_ProdBorrowing source, tb_ProdReturning target)
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

                // 生成归还单号
                target.ReturnNo = await _bizCodeService.GenerateBizBillNoAsync(BizType.归还单, CancellationToken.None);
                target.ReturnDate = DateTime.Now;
                target.Notes = $"由借出单{source.BorrowNo}生成";

                // 设置关联信息
                if (source.BorrowID > 0)
                {
                    target.BorrowID = source.BorrowID;
                    target.BorrowNO = source.BorrowNo;
                    target.CustomerVendor_ID = source.CustomerVendor_ID;
                }

                // 转换主表字段 - 复用业务层核心逻辑
                ConvertMainFieldsAsync(source, target);

                // 初始化明细集合
                if (target.tb_ProdReturningDetails == null)
                {
                    target.tb_ProdReturningDetails = new List<tb_ProdReturningDetail>();
                }

                // 转换明细 - 复用业务层核心逻辑
                await ConvertDetailsAsync(source, target);

                // 重新计算汇总字段
                RecalculateSummaryFields(target);

                // 初始化实体
                BusinessHelper.Instance.InitEntity(target);

                // 设置关联的借出单对象
                target.tb_prodborrowing = source;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "借出单到归还单转换失败，借出单号：{BorrowNo}", source.BorrowNo);
                throw;
            }
        }

        /// <summary>
        /// 转换主表字段 - 复用业务层核心逻辑
        /// </summary>
        private void ConvertMainFieldsAsync(tb_ProdBorrowing source, tb_ProdReturning target)
        {
            // 复制基础字段
            target.CustomerVendor_ID = source.CustomerVendor_ID;
            target.Employee_ID = source.Employee_ID;
            target.DepartmentID = source.DepartmentID;
            target.ProjectGroup_ID= source.ProjectGroup_ID;
            // 归还单特有字段
            target.ReturnDate = DateTime.Now;
            target.BorrowID = source.BorrowID;
            target.BorrowNO = source.BorrowNo;
            
            // 以下字段初始化为0，后面重新计算
            target.TotalAmount = 0;
            target.TotalCost = 0;
            target.TotalQty = 0;
            
            // 复制备注信息
            target.Notes = source.Notes;
            
        }

        /// <summary>
        /// 转换明细 - 复用业务层核心逻辑
        /// </summary>
        private async Task ConvertDetailsAsync(tb_ProdBorrowing source, tb_ProdReturning target)
        {
            var details = _mapper.Map<List<tb_ProdReturningDetail>>(source.tb_ProdBorrowingDetails);
            var newDetails = new List<tb_ProdReturningDetail>();
            var tipsMsg = new List<string>();
            var cacheManager = _appContext.GetRequiredService<IEntityCacheManager>();

            for (int i = 0; i < details.Count; i++)
            {
                #region 每行产品ID唯一

                tb_ProdBorrowingDetail item = source.tb_ProdBorrowingDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                View_ProdDetail Prod = cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                if (Prod != null && Prod.GetType().Name != "Object" && Prod is View_ProdDetail prodDetail)
                {
                    // 产品信息已获取
                }
                else
                {
                    Prod = cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                }

                // 计算可归还数量 = 借出数量 - 已归还数量
                details[i].Qty = item.Qty - item.ReQty;
                details[i].SubtotalPirceAmount = details[i].Price * details[i].Qty;
                details[i].SubtotalCostAmount = details[i].Cost * details[i].Qty;
                
                if (details[i].Qty > 0)
                {
                    newDetails.Add(details[i]);
                }
                else
                {
                    tipsMsg.Add($"借出单{source.BorrowNo}，{item.tb_proddetail.tb_prod.CNName}已归还数为{item.ReQty}，可归还数为{details[i].Qty}，当前行数据忽略！");
                }
                #endregion
            }

            if (newDetails.Count == 0)
            {
                tipsMsg.Add($"借出单:{source.BorrowNo}已全部归还，请检查是否正在重复归还！");
                _logger.LogWarning("借出单已全部归还，借出单号：{BorrowNo}", source.BorrowNo);
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
                _logger.LogWarning("借出单转换提示信息：{Tips}", msg.ToString());
            }

            target.tb_ProdReturningDetails = newDetails;

            await Task.CompletedTask; // 满足异步方法签名要求
        }

        /// <summary>
        /// 重新计算汇总字段
        /// </summary>
        /// <param name="target">归还单</param>
        private void RecalculateSummaryFields(tb_ProdReturning target)
        {
            if (target.tb_ProdReturningDetails == null || !target.tb_ProdReturningDetails.Any())
            {
                target.TotalQty = 0;
                target.TotalAmount = 0;
                target.TotalCost = 0;
                return;
            }

            // 计算总数量
            target.TotalQty = target.tb_ProdReturningDetails.Sum(d => d.Qty);

            // 计算总金额
            target.TotalAmount = target.tb_ProdReturningDetails.Sum(c => c.SubtotalPirceAmount);

            // 计算总成本
            target.TotalCost = target.tb_ProdReturningDetails.Sum(c => c.SubtotalCostAmount);
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：借出单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_ProdBorrowing source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "借出单不能为空";
                    return result;
                }

                // 检查借出单状态
                if (source.DataStatus != (int)DataStatus.确认 || source.ApprovalStatus != (int)ApprovalStatus.审核通过)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "只能转换已确认且已审核的借出单";
                    return result;
                }

                // 检查借出单是否有明细
                if (source.tb_ProdBorrowingDetails == null || !source.tb_ProdBorrowingDetails.Any())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "借出单没有明细，无法转换";
                    return result;
                }

                // 添加明细数量业务验证
                await ValidateDetailQuantitiesAsync(source, result);
                
                // 检查是否有可归还的明细
                var hasReturnableDetails = source.tb_ProdBorrowingDetails.Any(d => d.Qty > d.ReQty);
                if (!hasReturnableDetails)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "借出单所有明细已全部归还，无需转换";
                    return result;
                }

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证借出单转换条件时发生错误，借出单号：{BorrowNo}", source?.BorrowNo);
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 验证明细数量的业务逻辑
        /// </summary>
        /// <param name="source">借出单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateDetailQuantitiesAsync(tb_ProdBorrowing source, ValidationResult result)
        {
            try
            {
                int totalDetails = source.tb_ProdBorrowingDetails.Count;
                int returnableDetails = 0;
                int nonReturnableDetails = 0;
                decimal totalReturnableQty = 0;
                var cacheManager = _appContext?.GetRequiredService<IEntityCacheManager>();
                
                // 遍历所有借出单明细，检查可归还数量
                foreach (var detail in source.tb_ProdBorrowingDetails)
                {
                    // 计算可归还数量 = 借出数量 - 已归还数量
                    decimal returnableQty = detail.Qty - detail.ReQty;
                    
                    if (returnableQty > 0)
                    {
                        returnableDetails++;
                        totalReturnableQty += returnableQty;
                        
                        // 如果可归还数量小于原借出数量，添加部分归还提示
                        if (returnableQty < detail.Qty)
                        {
                            var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                            string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                            
                            result.AddWarning($"产品【{prodName}】已归还数量为{detail.ReQty}，可归还数量为{returnableQty}，小于原借出数量{detail.Qty}");
                        }
                    }
                    else
                    {
                        nonReturnableDetails++;
                        
                        // 添加完全归还提示
                        var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        result.AddWarning($"产品【{prodName}】已全部归还，可归还数量为0，将忽略此明细");
                    }
                }
                
                // 添加汇总提示信息
                if (nonReturnableDetails > 0)
                {
                    result.AddInfo($"共有{nonReturnableDetails}项产品已全部归还，将在转换时忽略");
                }
                
                if (returnableDetails > 0)
                {
                    result.AddInfo($"共有{returnableDetails}项产品可归还，总可归还数量为{totalReturnableQty}");
                }
                
                // 如果所有明细都已归还，添加警告但仍允许转换（让用户知道）
                if (returnableDetails == 0)
                {
                    result.AddWarning("该借出单所有明细已全部归还，转换生成的归还单将没有明细数据");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证借出单明细数量时发生错误，借出单号：{BorrowNo}", source?.BorrowNo);
                result.AddWarning("验证明细数量时发生错误，请检查数据完整性");
            }
        }
    }
}