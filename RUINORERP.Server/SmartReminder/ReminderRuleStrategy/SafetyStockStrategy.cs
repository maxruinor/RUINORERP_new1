using CacheManager.Core;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.PacketSpec.Enums.Core;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server.SmartReminder.ReminderRuleStrategy
{
    // 3. 基础策略实现
    public class SafetyStockStrategy : IReminderStrategy
    {
        public int Priority => 0;
        private readonly INotificationService _notification;
        //private readonly RuleEngineCenter _ruleCenter;
        private readonly IEntityCacheManager  _cacheManager;
        private readonly StockCacheService _stockCache;
        private readonly ILogger<SafetyStockStrategy> _logger;
        public SafetyStockStrategy(
            IEntityCacheManager cacheManager,
        //RuleEngineCenter ruleCenter,
        INotificationService notification,
        StockCacheService stockCache,
        ILogger<SafetyStockStrategy> logger)
        {
            _cacheManager = cacheManager;
            //_ruleCenter = ruleCenter;
            _notification = notification;
            _stockCache = stockCache;
            _logger = logger;
        }


        public async Task<bool> CheckAsync(tb_ReminderRule policy, tb_Inventory stock)
        {
            try
            {
                //var rule = new InventoryRule(policy); // 将策略转换为统一规则
                //if (await _ruleCenter.EvaluateAsync(rule, stock))
                //{
                //    await _notification.SendAlertAsync(policy, GenerateMessage(stock));
                //}

                // 使用缓存获取库存数据
                var currentStock = 0;//根据规则去查询要提醒的库存集合 await _stockCache.GetStockAsync(policy.);


                // 使用规则引擎进行评估
                //if (await _ruleCenter.EvaluateAsync(policy as IReminderRule, stock))
                //{
                //    //var product = await GetProductInfoAsync(policy.ProductId);
                //    //var message = $"库存预警：{product.Name}（{product.Code}）当前库存 {stock.Quantity}，" +
                //    //             $"安全范围 ({policy.MinStock}-{policy.MaxStock})";

                //    //await _notification.SendAlertAsync(policy, message);
                //}

                //var result = await _ruleEngine.EvaluateAsync(rule, stock);
                //if (result)
                //{
                //    //这里要从缓存中获取产品的详情
                //    var message = $"库存预警：产品 {policy.ProductId} 当前库存 {currentStock.Quantity}，" +
                //                 $"低于安全库存 {policy.MinStock} 或高于最大库存 {policy.MaxStock}";

                //    await _notification.SendAlertAsync(policy, message);
                //}
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "安全库存策略检查出错");
                _logger.LogError(ex, "安全库存检查失败 RuleId={RuleId}", policy.RuleId);
                // 记录失败但继续执行其他策略
            }
            return true;
        }


        public bool CanHandle(ReminderBizType reminderType)
        {
            if (reminderType == ReminderBizType.安全库存提醒)
            {
                return true;
            }
            else
                return false;
        }

        public async Task<bool> CheckAsync(IReminderRule rule, IReminderContext context)
        {
            List<tb_Inventory> inventories = context.GetData() as List<tb_Inventory>;

            var policy = rule as tb_ReminderRule;
            var stockPolicy = policy.GetConfig<SafetyStockConfig>() as SafetyStockConfig;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < inventories.Count; i++)
            {
                var stock = inventories[i];
                if (stock.Quantity < stockPolicy.MinStock || stock.Quantity > stockPolicy.MaxStock)
                {
                    var product = _cacheManager.GetEntity<View_ProdInfo>(stock.ProdDetailID);
                    if (product != null)
                    {
                        //这里要从缓存中获取产品的详情
                        var message = $"库存预警：{product.SKU}-{product.CNName} 当前库存为 {stock.Quantity}，" +
                                     $"安全范围 ({stockPolicy.MinStock}-{stockPolicy.MaxStock})";
                        sb.Append("\r\n");
                        sb.Append(message);
                    }
                    else
                    {
                        //这里要从缓存中获取产品的详情
                        var message = $"库存预警：{stock.ProdDetailID} 当前库存 {stock.Quantity}，" +
                                     $"安全范围 ({stockPolicy.MinStock}-{stockPolicy.MaxStock})";
                        sb.Append("\r\n");
                        sb.Append(message);
                    }

                }
            }

       
            await _notification.SendNotificationAsync(policy, sb.ToString(), null);
            return true;

        }


 

    }
}
