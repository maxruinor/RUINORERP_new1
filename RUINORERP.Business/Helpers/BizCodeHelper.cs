using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.IServices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Business.Helpers
{
    /// <summary>
    /// 编号生成辅助工具类
    /// 提供带重试和降级策略的编号生成方法
    /// </summary>
    public static class BizCodeHelper
    {
        /// <summary>
        /// 带重试机制的编号生成（用于自动审核等关键流程）
        /// </summary>
        /// <param name="bizCodeService">编号生成服务</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="maxRetries">最大重试次数，默认3次</param>
        /// <param name="initialDelayMs">初始延迟毫秒数，默认500ms</param>
        /// <param name="logger">日志记录器（可选）</param>
        /// <returns>生成的编号，失败时返回null</returns>
        public static async Task<string> GenerateBizBillNoWithRetryAsync(
            IBizCodeGenerateService bizCodeService,
            BizType bizType,
            int maxRetries = 3,
            int initialDelayMs = 500,
            ILogger logger = null)
        {
            Exception lastException = null;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    string billNo = await bizCodeService.GenerateBizBillNoAsync(bizType);
                    
                    if (!string.IsNullOrEmpty(billNo))
                    {
                        if (i > 0)
                        {
                            logger?.LogInformation("编号生成第{Retry}次重试成功: {BizType}, BillNo={BillNo}", 
                                i + 1, bizType, billNo);
                        }
                        return billNo;
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    
                    if (i < maxRetries - 1)
                    {
                        // 指数退避: 500ms, 1000ms, 2000ms...
                        int delay = initialDelayMs * (int)Math.Pow(2, i);
                        logger?.LogWarning(ex, "编号生成第{Retry}次失败，{Delay}ms后重试: {BizType}", 
                            i + 1, delay, bizType);
                        
                        await Task.Delay(delay);
                    }
                    else
                    {
                        logger?.LogError(ex, "编号生成最终失败，已达最大重试次数{MaxRetries}: {BizType}", 
                            maxRetries, bizType);
                    }
                }
            }

            // 所有重试都失败，返回降级编号
            string fallbackBillNo = GenerateFallbackBillNo(bizType);
            logger?.LogWarning("使用降级编号: {BizType}, FallbackBillNo={FallbackBillNo}", 
                bizType, fallbackBillNo);
            
            return fallbackBillNo;
        }

        /// <summary>
        /// 生成降级编号（本地编号，带标识前缀）
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>降级编号</returns>
        private static string GenerateFallbackBillNo(BizType bizType)
        {
            // 格式: LOCAL-{业务类型简写}-{时间戳}-{随机数}
            string bizTypeShort = GetBizTypeShortName(bizType);
            string timestamp = DateTime.Now.ToString("yyMMddHHmmssfff");
            int random = new Random().Next(100, 999);
            
            return $"LOCAL-{bizTypeShort}-{timestamp}-{random}";
        }

        /// <summary>
        /// 获取业务类型简写
        /// </summary>
        private static string GetBizTypeShortName(BizType bizType)
        {
            return bizType switch
            {
                BizType.销售订单 => "SO",
                BizType.销售出库单 => "SOUT",
                BizType.采购订单 => "PO",
                BizType.采购入库单 => "PIN",
                BizType.请购单 => "PR",
                BizType.制令单 => "MO",
                BizType.应收款单 => "AR",
                BizType.应付款单 => "AP",
                BizType.收款单 => "RCV",
                BizType.付款单 => "PAY",
                BizType.损失确认单 => "LOSS",
                BizType.溢余确认单 => "OVER",
                _ => bizType.ToString().ToUpper()
            };
        }

        /// <summary>
        /// 带重试机制的基础信息编号生成
        /// </summary>
        public static async Task<string> GenerateBaseInfoNoWithRetryAsync(
            IBizCodeGenerateService bizCodeService,
            BaseInfoType baseInfoType,
            string paraConst = null,
            int maxRetries = 3,
            int initialDelayMs = 500,
            ILogger logger = null)
        {
            Exception lastException = null;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    string baseInfoNo = await bizCodeService.GenerateBaseInfoNoAsync(baseInfoType, paraConst);
                    
                    if (!string.IsNullOrEmpty(baseInfoNo))
                    {
                        if (i > 0)
                        {
                            logger?.LogInformation("基础信息编号生成第{Retry}次重试成功: {BaseInfoType}", 
                                i + 1, baseInfoType);
                        }
                        return baseInfoNo;
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    
                    if (i < maxRetries - 1)
                    {
                        int delay = initialDelayMs * (int)Math.Pow(2, i);
                        logger?.LogWarning(ex, "基础信息编号生成第{Retry}次失败，{Delay}ms后重试: {BaseInfoType}", 
                            i + 1, delay, baseInfoType);
                        
                        await Task.Delay(delay);
                    }
                    else
                    {
                        logger?.LogError(ex, "基础信息编号生成最终失败: {BaseInfoType}", baseInfoType);
                    }
                }
            }

            // 降级编号
            string fallbackNo = $"LOCAL-{baseInfoType}-{DateTime.Now:yyMMddHHmmssfff}-{new Random().Next(100, 999)}";
            logger?.LogWarning("使用降级基础信息编号: {BaseInfoType}, FallbackNo={FallbackNo}", 
                baseInfoType, fallbackNo);
            
            return fallbackNo;
        }
    }
}
