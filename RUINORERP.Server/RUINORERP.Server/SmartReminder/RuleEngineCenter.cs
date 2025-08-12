using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RulesEngine.Models;
using SqlSugar;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.SmartReminder.InvReminder;
using RUINORERP.Model.Context;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WorkflowCore.Models;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Model;
using RUINORERP.Global.EnumExt;
using System.Reflection;
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.Business.ReminderService;
using RUINORERP.Model.ReminderModel.ReminderResults;

namespace RUINORERP.Server.SmartReminder
{
    public interface IRuleEngineCenter
    {
        Task<bool> EvaluateAsync(IReminderRule rule, object context);
    }



    public class RuleEngineCenter : IRuleEngineCenter
    {
        private readonly RulesEngine.RulesEngine _reEngine;
        private readonly ConcurrentDictionary<string, ScriptRunner<bool>> _roslynCache = new();
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ApplicationContext _appContext;
        private readonly ILogger<SmartReminderMonitor> _logger;
        private readonly ReminderResultManager _resultManager;

        public RuleEngineCenter(ILogger<SmartReminderMonitor> logger, ApplicationContext _AppContextData,
            IUnitOfWorkManage unitOfWorkManage, ReminderResultManager resultManager)
        {
            _logger = logger;
            _appContext = _AppContextData;
            _unitOfWorkManage = unitOfWorkManage;
            _resultManager = resultManager;
            // 初始化RulesEngine
            //var workflows = LoadWorkflowsFromDb();
            //_reEngine = new RulesEngine.RulesEngine(workflows.ToArray());
        }
        //private List<Workflow> LoadWorkflowsFromDb()
        //{
        //    // 从数据库加载RulesEngine规则
        //    return _db.Queryable<WorkflowDefinition>()
        //        .Where(w => w.IsActive)
        //        .Select(w => new Workflow
        //        {
        //            WorkflowName = w.Name,
        //            Rules = JsonConvert.DeserializeObject<List<Rule>>(w.RuleJson)
        //        }).ToList();
        //}
        public async Task<bool> EvaluateAsync(IReminderRule rule, object context)
        {
            var EngineType = (RuleEngineType)rule.RuleEngineType;

            return EngineType switch
            {
                RuleEngineType.RulesEngine => await EvaluateWithRulesEngine(rule, context),
                RuleEngineType.Roslyn => await EvaluateWithRoslyn(rule, context),
                _ => throw new NotSupportedException($"不支持的规则引擎类型: {EngineType}")
            };
        }


        public void EvaluateAndAddResult(IReminderRule rule, object context)
        {
            IReminderResult result = null;
            var ReminderBizType = (ReminderBizType)rule.ReminderBizType;
            switch (ReminderBizType)
            {
                case ReminderBizType.安全库存提醒:
                    var stockContext = context as InventoryContext;
                    List<tb_Inventory> inventories = stockContext.GetData() as List<tb_Inventory>;
                    for (int i = 0; i < inventories.Count; i++)
                    {
                        var tb_Inventory = inventories[i];



                        //if (tb_Inventory.Quantity < tb_Inventory.MinStock)
                        //{
                        //    result = new SafetyStockResult
                        //    {
                        //        RuleId = rule.RuleId,
                        //        ProductName = stockContext.ProductName,
                        //        SKU = stockContext.SKU,
                        //        CurrentStock = stockContext.CurrentStock,
                        //        MinStock = stockContext.MinStock,
                        //        MaxStock = stockContext.MaxStock,
                        //        RecommendedQuantity = stockContext.MaxStock - stockContext.CurrentStock,
                        //        Unit = stockContext.Unit
                        //    };
                        //}
                    }
                   
                    break;

                case ReminderBizType.单据审批提醒:
                    //var docContext = context as DocumentContext;
                    //if (docContext.Status == DocumentStatus.PendingApproval)
                    //{
                    //    result = new DocumentApprovalResult
                    //    {
                    //        RuleId = rule.RuleId,
                    //        DocumentType = docContext.DocumentType,
                    //        DocumentNumber = docContext.DocumentNumber,
                    //        DocumentStatus = docContext.Status.ToString(),
                    //        Creator = docContext.Creator,
                    //        CreateTime = docContext.CreateTime,
                    //        ApprovalAction = "审批",
                    //        DocumentLink = docContext.Link
                    //    };
                    //}
                    break;

                    // 其他业务类型...
            }

            if (result != null)
            {
                _resultManager.AddResult(result);
            }
        }


        private async Task<bool> EvaluateWithRulesEngine(IReminderRule rule, object context)
        {
            var result = await _reEngine.ExecuteAllRulesAsync(rule.RuleEngineType.ToString(), context);
            return result.Any(r => r.IsSuccess);
        }

        private async Task<bool> EvaluateWithRoslyn(IReminderRule rule, object context)
        {
            if (!_roslynCache.TryGetValue(rule.RuleId.ToString(), out var runner))
            {
                //var scriptOptions = ScriptOptions.Default
                //.AddReferences(Assembly.GetExecutingAssembly())
                //.AddImports("System");

                //var script = CSharpScript.Create<bool>(rule.Condition,
                //    options: scriptOptions,
                //    globalsType: typeof(RuleGlobals<>).MakeGenericType(context.GetType()));

                //runner = script.CreateDelegate();
                //_roslynCache.TryAdd(rule.RuleId.ToString(), runner);
            }

            var globals = Activator.CreateInstance(typeof(RuleGlobals<>)
                .MakeGenericType(context.GetType()), context);

            return await runner(globals);
        }
    }

    public class RuleGlobals<T>
    {
        public T Context { get; }
        public RuleGlobals(T context) => Context = context;
    }
}
