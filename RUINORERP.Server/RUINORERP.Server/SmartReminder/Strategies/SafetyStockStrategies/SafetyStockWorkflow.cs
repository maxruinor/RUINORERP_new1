using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using SqlSugar;
using Autofac;
using Newtonsoft.Json;
using CacheManager.Core;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Model;
using RUINORERP.Global.EnumExt;
using Newtonsoft.Json.Linq;
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.Server.Workflow.WFReminder;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Workflow.WFScheduled;

namespace RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies
{
    #region 数据模型
    /// <summary>
    /// 安全库存计算参数
    /// </summary>
    public class SafetyStockData
    {

        // 使用配置对象替代所有可配置参数
        public SafetyStockConfig Config { get; set; } = new SafetyStockConfig();

        /// <summary>
        /// 计算结果
        /// </summary>
        public Dictionary<long, SafetyStockResult> Results { get; set; } = new Dictionary<long, SafetyStockResult>();


        /// <summary>
        /// 工作流ID  仅保留运行时数据
        /// </summary>
        public string WorkflowId { get; set; }
 

        // 临时存储当前产品的销售数据
        public List<SalesHistory> CurrentSalesData { get; set; }

        // 临时存储当前产品的计算结果
        public SafetyStockResult CurrentResult { get; set; }

        // 添加产品信息缓存字典
        public Dictionary<long, View_Inventory> ProductInfoCache { get; set; } = new Dictionary<long, View_Inventory>();


    }

    /// <summary>
    /// 安全库存计算结果
    /// </summary>
    public class SafetyStockResult
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal AverageDailyDemand { get; set; }
        public decimal DemandStandardDeviation { get; set; }
        public decimal SafetyStockLevel { get; set; }
        public bool NeedAlert { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 销售历史数据
    /// </summary>
    public class SalesHistory
    {
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }
    }
    #endregion

    #region 工作流步骤
    /// <summary>
    /// 初始化参数步骤
    /// </summary>
    public class InitializeParameters : StepBody
    {
        public SafetyStockConfig Config { get; set; }

        public List<long> ProductIds { get; set; }

        ///// <summary>
        ///// 计算的周期
        ///// </summary>
        //public int CalculationPeriodDays { get; set; }

        ///// <summary>
        ///// 采购提前期
        ///// </summary>
        //public int PurchaseLeadTimeDays { get; set; }


        ///// <summary>
        ///// 服务水平系数 (1.28=90%, 1.64=95%, 2.33=99%
        ///// </summary>
        //public double ServiceLevelFactor { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as SafetyStockData;
            data.Config = Config; // 传递配置到工作流数据
                                  // 设置默认值
            Config.CalculationPeriodDays = Config.CalculationPeriodDays <= 0
                ? 90 : Config.CalculationPeriodDays;

            Config.PurchaseLeadTimeDays = Config.PurchaseLeadTimeDays <= 0
                ? 7 : Config.PurchaseLeadTimeDays;

            Config.ServiceLevelFactor = Config.ServiceLevelFactor <= 0
                ? 1.64 : Config.ServiceLevelFactor;


            // 如果没有指定产品ID，则获取所有需要监控的产品
            if (ProductIds == null || !ProductIds.Any())
            {
                ISqlSugarClient sugarScope = Startup.GetFromFac<ISqlSugarClient>();
                // 记录工作流完成日志
                using (var db = sugarScope)
                {
                    var policies = db.Queryable<tb_ReminderRule>()
                                 .Where(p => p.IsEnabled)
                                 .Where(c => c.ReminderBizType == (int)ReminderBizType.安全库存提醒)
                                 .ToList();
                    foreach (var item in policies)
                    {
                        // 使用示例
                        JObject obj = SafeParseJson(item.JsonConfig);
                        SafetyStockConfig safetyStockConfig = obj.ToObject<SafetyStockConfig>();

                        ProductIds.AddRange(safetyStockConfig.ProductIds); // safetyStockConfig.ProductIds
                    }
                    // 批量查询所有产品信息
                    var products = db.Queryable<View_Inventory>()
                        .Where(p => ProductIds.Contains(p.ProdDetailID.Value))
                        .ToList();

                    // 存入缓存字典
                    foreach (var product in products)
                    {
                        (context.Workflow.Data as SafetyStockData).ProductInfoCache[product.ProdDetailID.Value] = product;
                    }
                }

            }

            return ExecutionResult.Next();
        }


        public JObject SafeParseJson(string json)
        {
            try
            {
                return JObject.Parse(json);
            }
            catch (JsonReaderException)
            {
                return new JObject(); // 返回空对象或 null
            }
        }
    }

    /// <summary>
    /// 获取销售历史数据步骤
    /// </summary>
    public class GetSalesHistory : StepBodyAsync
    {
        private readonly ILogger<GetSalesHistory> logger;
        public GetSalesHistory(ILogger<GetSalesHistory> _logger)
        {
            logger = _logger;
        }
        public long ProductId { get; set; }

        //public List<SalesHistory> SalesData { get; set; }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as SafetyStockData;
            int days = data.Config.CalculationPeriodDays;

            ISqlSugarClient sugarScope = Startup.GetFromFac<ISqlSugarClient>();
            using (var db = sugarScope)
            {
                var endDate = DateTime.Now.Date;
                var startDate = endDate.AddDays(-days);

                data.CurrentSalesData = await db.Queryable<View_SaleOutItems>()
                    .Where(i => i.ProdDetailID == ProductId
                                && i.OutDate.Value >= startDate
                                && i.OutDate.Value < endDate)
                                                                       // .GroupBy(i => (i.OutDate.Value.ToShortDateString()) // 分组键
                                                                       .GroupBy(i => i.OutDate.Value.Date) // 分组键
                    .Select(g => new SalesHistory  // 使用 g 代表分组结果
                    {
                        Date = g.OutDate.Value.Date, // 使用分组键作为日期
                        Quantity = (decimal)SqlFunc.AggregateSum(g.Quantity)
                    })
                    .OrderBy(i => i.Date)
                    .ToListAsync();
            }
            logger.Error($"GetSalesHistory：{ProductId}");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 计算安全库存步骤
    /// </summary>
    public class CalculateSafetyStock : StepBody
    {
        private readonly ILogger<CalculateSafetyStock> logger;
        public CalculateSafetyStock(ILogger<CalculateSafetyStock> _logger)
        {
            logger = _logger;
        }
        public long ProductId { get; set; }


        public List<SalesHistory> SalesData { get; set; }
        public SafetyStockResult Result { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {

            var data = context.Workflow.Data as SafetyStockData;
            var config = data.Config;


            Result = new SafetyStockResult { ProductId = ProductId };

            //logger.Error($"CalculateSafetyStock：{ProductId}");

            //ISqlSugarClient sugarScope = Startup.GetFromFac<ISqlSugarClient>();
            //// 获取产品信息和当前库存
            //using (var db = sugarScope)
            //{
            //    var product = db.Queryable<View_Inventory>().First(p => p.ProdDetailID == ProductId);
            //    if (product != null)
            //    {
            //        Result.ProductName = product.CNName;
            //    }

            //    // 获取当前库存
            //    Result.CurrentStock = product?.Quantity ?? 0;
            //}

            // === 从缓存获取产品信息 ===
            if ((context.Workflow.Data as SafetyStockData).ProductInfoCache.TryGetValue(
                ProductId,
                out View_Inventory product))
            {
                Result.ProductName = product.CNName;
                Result.CurrentStock = product?.Quantity ?? 0;
            }
            else
            {
                // 缓存中找不到的备选方案
                Result.ProductName = "未知产品";
                Result.CurrentStock = 0;
            }

            // 计算日均需求量
            if (SalesData != null && SalesData.Any())
            {
                // 计算日均需求
                var totalDays = (DateTime.Now.Date - SalesData.Min(d => d.Date)).TotalDays;
                var totalQuantity = SalesData.Sum(d => d.Quantity);
                Result.AverageDailyDemand = totalDays > 0 ? (decimal)(totalQuantity / (decimal)totalDays) : 0;

                // 计算需求标准差
                if (SalesData.Count > 1)
                {
                    double sumOfSquares = SalesData.Sum(d => Math.Pow((double)(d.Quantity - Result.AverageDailyDemand), 2));
                    Result.DemandStandardDeviation = (decimal)Math.Sqrt(sumOfSquares / (SalesData.Count - 1));
                }
                else
                {
                    Result.DemandStandardDeviation = 0;
                }
                // 计算安全库存 = 服务水平系数 × 需求标准差 × √采购提前期
                Result.SafetyStockLevel = (decimal)(config.ServiceLevelFactor
                    * (double)Result.DemandStandardDeviation
                    * Math.Sqrt(config.PurchaseLeadTimeDays));


                // 检查手动指定库存
                //if (config.ManualSafetyStockLevel)
                //{
                //    safetyStock = config.ManualSafetyStockLevel;
                //}


                // 检查是否需要提醒（当前库存低于安全库存）
                Result.NeedAlert = Result.CurrentStock < Result.SafetyStockLevel;
                Result.Message = Result.NeedAlert
                    ? $"产品 {Result.ProductName} 库存不足，当前库存: {Result.CurrentStock}, 安全库存: {Result.SafetyStockLevel}"
                    : $"产品 {Result.ProductName} 库存正常，当前库存: {Result.CurrentStock}, 安全库存: {Result.SafetyStockLevel}";

                logger.Error($"结果{Result.ProductId}{Result.Message}");
            }
            else
            {
                Result.Message = $"产品 {Result.ProductName} 没有足够的销售历史数据";
                Result.NeedAlert = false;
            }

            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 发送提醒步骤
    /// </summary>
    public class SendAlertNotification : StepBodyAsync
    {
        //private readonly IConnectionMultiplexer _redis;
        //private readonly ICacheManager<object> _cache;

        public SafetyStockResult Result { get; set; }

        //public SendAlertNotification(IConnectionMultiplexer redis, ICacheManager<object> cache)
        //{
        //    _redis = redis;
        //    _cache = cache;
        //}

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            if (Result?.NeedAlert == true)
            {
                // 实现提醒逻辑...
            }

            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 完成步骤
    /// </summary>
    public class CompleteSafetyStockCalculation : StepBody
    {
        public Dictionary<long, SafetyStockResult> Results { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // 可以在这里添加汇总报告等逻辑
            var alertCount = Results?.Count(r => r.Value.NeedAlert) ?? 0;
            frmMain.Instance.PrintInfoLog($"安全库存计算完成，共检查 {Results?.Count ?? 0} 个产品，需要提醒的产品有 {alertCount} 个");

            //ISqlSugarClient sugarScope = Startup.GetFromFac<ISqlSugarClient>();
            //// 记录工作流完成日志
            //using (var db = sugarScope)
            //{
            //    db.Insertable(new WorkflowLog
            //    {
            //        WorkflowType = "SafetyStockCalculation",
            //        WorkflowId = context.Workflow.Id,
            //        Status = "完成",
            //        Message = $"安全库存计算完成，共检查 {Results?.Count ?? 0} 个产品，需要提醒的产品有 {alertCount} 个",
            //        CreateTime = DateTime.Now
            //    }).ExecuteCommand();
            //}
            return ExecutionResult.Next();
        }
    }
    #endregion

    #region 工作流定义

    /// <summary>
    /// 定期根据规则去检测安全库存的参数这个周期较长，
    /// 默认是90天,当前工作流是先计算安全库存，然后发送提醒，然后更新数据库。
    /// 安全库存会保存到数据库中，下次启动时从数据库中读取，只进行检测，不进行计算。
    /// </summary>
    public class SafetyStockWorkflow : IWorkflow<SafetyStockData>
    {
        private readonly ILogger<ReminderWorkflow> _logger;

        public SafetyStockWorkflow(ILogger<ReminderWorkflow> logger)
        {
            _logger = logger;
        }
        public string Id => "SafetyStockWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<SafetyStockData> builder)
        {
            builder
                .StartWith<InitializeParameters>()
                    .Input(step => step.Config, data => data.Config)
                    .Input(step => step.ProductIds, data => data.Config.ProductIds)

                .ForEach(data => data.Config.ProductIds)
                    .Do(foreachBuilder => foreachBuilder
                        // 获取销售历史数据
                        .StartWith<GetSalesHistory>()
                        .Input(step => step.ProductId, (data, context) => (long)context.Item)
                        //.Input(step => step.CalculationPeriodDays, data => data.CalculationPeriodDays)
                        //.Output(data => data.CurrentSalesData, step => step.SalesData)
                        //.Output(data => data.TempProductId, step => step.ProductId)
                        // 计算安全库存
                        .Then<CalculateSafetyStock>()
                        .Input(step => step.ProductId, (data, context) => (long)context.Item)
                        //.Input(step => step.PurchaseLeadTimeDays, data => data.PurchaseLeadTimeDays)
                        //.Input(step => step.ServiceLevelFactor, data => data.ServiceLevelFactor)
                        //.Input(step => step.SalesData, data => data.CurrentSalesData)
                        //.Output(data => data.CurrentResult, step => step.Result)

                        // 保存结果到字典
                        .Then<SaveResultToDictionaryStep>()
                      .Input(step => step.ProductId, (data, context) => (long)context.Item)
                      //.Input(step => step.ProductId, (data, context) => (long)context.Item)
                      //.Input(step => step.Result, data => data.CurrentResult)
                      //.Input(step => step.ResultsDictionary, data => data.Results)
                      // 发送提醒
                      .Then<SendAlertNotification>()
                    //.Input(step => step.Result, data => data.CurrentResult)
                    )

                // 完成处理
                .Then<CompleteSafetyStockCalculation>();
            //.Input(step => step.Results, data => data.Results);
        }



    }



    // 新增步骤：保存结果到字典
    public class SaveResultToDictionaryStep : StepBody
    {
        private readonly ILogger<SaveResultToDictionaryStep> logger;
        public SaveResultToDictionaryStep(ILogger<SaveResultToDictionaryStep> _logger)
        {
            logger = _logger;
        }
        public long ProductId { get; set; }
        public SafetyStockResult Result { get; set; }
        public Dictionary<long, SafetyStockResult> ResultsDictionary { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (ResultsDictionary == null)
                ResultsDictionary = new Dictionary<long, SafetyStockResult>();

            if (Result != null)
                ResultsDictionary[ProductId] = Result;


            return ExecutionResult.Next();
        }
    }
    #endregion

    #region 工作流注册与启动
    public static class SafetyStockWorkflowConfig
    {
        public static void RegisterWorkflow(IServiceCollection services)
        {
            // 注册步骤和工作流
            services.AddTransient<SafetyStockWorkflow>();
            services.AddTransient<InitializeParameters>();
            services.AddTransient<GetSalesHistory>();
            services.AddTransient<CalculateSafetyStock>();
            services.AddTransient<SendAlertNotification>();
            services.AddTransient<CompleteSafetyStockCalculation>();
            services.AddTransient<SaveResultToDictionaryStep>();
        }

        /// <summary>
        /// 启动安全库存计算工作流（每天凌晨2点执行）
        /// </summary>
        public static async Task<bool> ScheduleDailySafetyStockCalculation(IWorkflowHost host)
        {

            try
            {
                // 注册工作流
                host.RegisterWorkflow<SafetyStockWorkflow, SafetyStockData>();

                // 计算下次执行时间
                var now = DateTime.Now;
                var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 18, 22, 0);
                if (nextRunTime <= now)
                    nextRunTime = nextRunTime.AddDays(1);

                // 计算时间间隔
                var interval = nextRunTime - now;

                // 首次延迟执行
                var timer = new System.Timers.Timer(interval.TotalMilliseconds);
                timer.Elapsed += async (sender, e) =>
                {
                    frmMain.Instance.PrintInfoLog($"开始工作流执行: {System.DateTime.Now.ToString()}");
                    try
                    {
                        // 执行工作流
                        //await host.StartWorkflow<SafetyStockData>("SafetyStockWorkflow", new SafetyStockData());
                        var configs = GetEnabledSafetyStockConfigs();

                        foreach (var config in configs)
                        {
                            await host.StartWorkflow("SafetyStockWorkflow", new SafetyStockData
                            {
                                Config = config
                            });
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"工作流执行错误: {ex.Message}");
                    }

                    // 改为每天执行一次
                    timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
                };
                timer.Start();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"工作流注册错误: {ex.Message}");
                return false;
            }
        }

        private static List<SafetyStockConfig> GetEnabledSafetyStockConfigs()
        {
            using var db = Startup.GetFromFac<ISqlSugarClient>();
            return db.Queryable<tb_ReminderRule>()
                .Where(r => r.IsEnabled && r.ReminderBizType == (int)ReminderBizType.安全库存提醒)
                .Select(r => JsonConvert.DeserializeObject<SafetyStockConfig>(r.JsonConfig))
                .ToList();
        }

    }
    #endregion

}
