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
    /// 复用业务层的核心转换逻辑（BuildProdReturningFromBorrow），确保数据一致性
    /// </summary>
    public class BorrowToReturnConverter : DocumentConverterBase<tb_ProdBorrowing, tb_ProdReturning>
    {
        private readonly ILogger<BorrowToReturnConverter> _logger;
        private readonly tb_ProdReturningController<tb_ProdReturning> _prodReturningController;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="prodReturningController">归还单控制器（用于调用核心转换逻辑）</param>
        public BorrowToReturnConverter(
            ILogger<BorrowToReturnConverter> logger,
            tb_ProdReturningController<tb_ProdReturning> prodReturningController)
            : base(logger)
        {
            _prodReturningController = prodReturningController ?? throw new ArgumentNullException(nameof(prodReturningController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 转换器显示名称
        /// 使用基类实现，从Description特性获取
        /// </summary>
        public override string DisplayName => base.DisplayName;

        /// <summary>
        /// 执行单据转换 - 直接调用业务层核心逻辑 BuildProdReturningFromBorrow
        /// 重写基类方法，完全控制转换过程
        /// </summary>
        /// <param name="source">源单据：借出单</param>
        /// <returns>转换后的归还单</returns>
        public override async Task<tb_ProdReturning> ConvertAsync(tb_ProdBorrowing source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 直接调用经过长期验证的 BuildProdReturningFromBorrow 方法
            return await _prodReturningController.BuildProdReturningFromBorrow(source);
        }
        
        /// <summary>
        /// 执行具体的转换逻辑 - 重写后不再使用基类的 target 参数模式
        /// </summary>
        /// <param name="source">源单据：借出单</param>
        /// <param name="target">目标单据：归还单（不再使用）</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_ProdBorrowing source, tb_ProdReturning target)
        {
            // 此方法不再使用，逻辑已移至 ConvertAsync
            // 保留此方法以满足抽象类要求
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
                            result.AddWarning($"产品已归还数量为{detail.ReQty}，可归还数量为{returnableQty}，小于原借出数量{detail.Qty}");
                        }
                    }
                    else
                    {
                        nonReturnableDetails++;
                        
                        // 添加完全归还提示
                        result.AddWarning($"产品已全部归还，可归还数量为0，将忽略此明细");
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