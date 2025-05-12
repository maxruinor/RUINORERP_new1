using Microsoft.Extensions.Logging;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Server.SmartReminder.InvReminder;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.ReminderRuleStrategy
{
    // 3. 基础策略实现
    public class SafetyStockStrategy : IAlertStrategy
    {
        public int Priority => 1;
        private readonly INotificationService _notification;
        private readonly RuleEngineCenter _ruleCenter;
        private readonly StockCacheService _stockCache;
        private readonly ILogger<SafetyStockStrategy> _logger;
        public SafetyStockStrategy(
        RuleEngineCenter ruleCenter,
        INotificationService notification,
        StockCacheService stockCache,
        ILogger<SafetyStockStrategy> logger)
        {
            _ruleCenter = ruleCenter;
            _notification = notification;
            _stockCache = stockCache;
            _logger = logger;
        }


        public async Task CheckAsync(SafetyStockRule policy, tb_Inventory stock)
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
                if (await _ruleCenter.EvaluateAsync(policy, stock))
                {
                    //var product = await GetProductInfoAsync(policy.ProductId);
                    //var message = $"库存预警：{product.Name}（{product.Code}）当前库存 {stock.Quantity}，" +
                    //             $"安全范围 ({policy.MinStock}-{policy.MaxStock})";

                    //await _notification.SendAlertAsync(policy, message);
                }

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
        }

        public Task CheckAsync(tb_ReminderRule policy, tb_Inventory stock)
        {
            throw new NotImplementedException();
        }

        //private async Task<View_ProdDetail> GetProductInfoAsync(int productId)
        //{
        //    // 实现产品信息获取...
        //}


        //if (stock.Quantity < policy.MinStock || stock.Quantity > policy.MaxStock)
        //{
        //    //这里要从缓存中获取产品的详情
        //    var message = $"库存预警：{stock.ProdDetailID} 当前库存 {stock.Quantity}，" +
        //                 $"安全范围 ({policy.MinStock}-{policy.MaxStock})";

        //    await _notification.SendAlertAsync(policy, message);
        //}
    }


 

}
