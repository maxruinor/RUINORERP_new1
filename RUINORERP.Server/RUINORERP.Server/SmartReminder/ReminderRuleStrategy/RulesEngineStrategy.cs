using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Model;
using RulesEngine;

namespace RUINORERP.Server.SmartReminder.ReminderRuleStrategy
{
    public class RulesEngineStrategy : IAlertStrategy
    {
        private readonly RulesEngine.RulesEngine _engine;

        public RulesEngineStrategy()
        {
            //var workflow = new Workflow
            //{
            //    WorkflowName = "CustomRules",
            //    Rules = LoadRulesFromDatabase()
            //};
            //_engine = new RulesEngine.RulesEngine(new[] { workflow });
        }

        /// <summary>
        /// 优先级属性
        /// </summary>
        public int Priority => 0;

        public async Task CheckAsync(tb_ReminderRule policy, tb_Inventory stock)
        {
            var result = await _engine.ExecuteAllRulesAsync("CustomRules", stock);
            // 处理规则结果
        }
    }
}
