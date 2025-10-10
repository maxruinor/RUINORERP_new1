using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using RUINORERP.Server.SmartReminder.InvReminder;


namespace RUINORERP.Server.SmartReminder
{
    public class HybridRuleEngine
    {
        private readonly RulesEngine.RulesEngine _reEngine;
        private readonly Script _roslynEngine;

        public HybridRuleEngine(RulesEngine.RulesEngine reEngine)
        {
            _reEngine = reEngine;
            _roslynEngine = CSharpScript.Create("");
        }

        public async Task<bool> CheckRulesAsync(InventoryAlertContext context)
        {
            // 第一阶段：RulesEngine检查
            //var reResult = await _reEngine.ExecuteAllRulesAsync(context);
            //if (reResult.Any(r => r.IsSuccess))
            //{
            //    return true;
            //}

            //// 第二阶段：Roslyn脚本检查
            //var script = GetCustomRuleScript(context.ProductType);
            //var roslynResult = await _roslynEngine.ExecuteAsync<bool>(script, context);

            //return roslynResult;
            return true;
        }

        private string GetCustomRuleScript(string productType)
        {
            // 这里可以根据产品类型返回不同的脚本逻辑
            return @"
                // 示例脚本：检查库存是否低于安全库存的20%
                input.CurrentStock < input.MinStock * 0.8;
            ";
        }
        //public async Task CheckRulesAsync(InventoryContext context)
        //{
        //    // 第一阶段：RulesEngine检查
        //    var reResult = await _reEngine.ExecuteAsync(context);
        //    if (reResult.IsTriggered) return;

        //    // 第二阶段：复杂规则检查
        //    var script = GetCustomRuleScript(context.ProductType);
        //    var roslynResult = await _roslynEngine.EvaluateAsync(script, context);

        //    if (roslynResult) TriggerAlert();
        //}
    }
}
