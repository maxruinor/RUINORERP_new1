using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Server.SmartReminder.ReminderContext;
using RulesEngine;

namespace RUINORERP.Server.SmartReminder.ReminderRuleStrategy
{
    public class RulesEngineStrategy : IReminderStrategy
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

        public bool CanHandle(ReminderBizType reminderType)
        {
            throw new NotImplementedException();
        }

        //public async Task CheckAsync(IReminderRule policy, tb_Inventory stock)
        //{
        //    var result = await _engine.ExecuteAllRulesAsync("CustomRules", stock);
        //    // 处理规则结果
        //}

        public async  Task CheckAsync(IReminderRule rule, IReminderContext context)
        {
            var result = await _engine.ExecuteAllRulesAsync("CustomRules", context);
            // 处理规则结果
        }
    }
}
