using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.Business.Processor;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Model.QueryDto;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.Business.DataCorrectionServices
{
    /// <summary>
    /// 销售订单完结状态修复服务
    /// 修复销售出库单审核后未正确更新订单明细出库数量、订单未正确完结的问题
    /// </summary>
    public class SaleOrderCloseFixService : DataCorrectionServiceBase
    {
        private readonly ILogger<SaleOrderCloseFixService> _logger;
        private readonly ApplicationContext _appContext;
        private readonly IEntityCacheManager _cacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="appContext">应用上下文</param>
        /// <param name="unitOfWorkManage">工作单元管理器</param>
        /// <param name="cacheManager">缓存管理器</param>
        public SaleOrderCloseFixService(
            ILogger<SaleOrderCloseFixService> logger,
            ApplicationContext appContext,
            IUnitOfWorkManage unitOfWorkManage,
            IEntityCacheManager cacheManager)
            : base(unitOfWorkManage)
        {
            _logger = logger;
            _appContext = appContext;
            _cacheManager = cacheManager;
        }

        public override string CorrectionName => "SaleOrderCloseFix";

        public override string FunctionName => "销售订单完结状态修复";

        public override string ProblemDescription =>
            "修复销售出库单审核后，销售订单相关数据未正确更新的问题。\n\n" +
            "具体问题包括：\n" +
            "1. 销售订单明细的TotalDeliveredQty（订单出库数）未累加实际出库数量\n" +
            "2. 当所有明细都已出库完成时，订单状态DataStatus未更新为8（完结）\n\n" +
            "这导致系统无法正确判断订单是否已完成，影响后续的销售统计和报表。";

        public override List<string> AffectedTables => new List<string>
        {
            "tb_SaleOrder",           // 销售订单主表
            "tb_SaleOrderDetail",     // 销售订单明细表
            "tb_SaleOut",             // 销售出库单主表（用于验证）
            "tb_SaleOutDetail"        // 销售出库单明细表（用于计算实际出库数量）
        };

        public override string FixLogic =>
            "修复逻辑分为以下步骤：\n\n" +
            "【步骤1】检测需要修复的订单\n" +
            "  - 查找有已审核出库单（DataStatus=4）但订单状态不是完结（DataStatus≠8）的销售订单\n" +
            "  - 根据出库单明细的SaleOrderDetail_ID关联到订单明细\n" +
            "  - 按SaleOrderDetail_ID汇总实际出库数量\n\n" +
            "【步骤2】更新订单明细出库数量\n" +
            "  - TotalDeliveredQty = 该明细对应的所有已审核出库数量之和\n" +
            "  - 确保 TotalDeliveredQty <= Quantity\n\n" +
            "【步骤3】更新订单完结状态\n" +
            "  - 如果 SUM(TotalDeliveredQty) >= TotalQty 且 TotalQty > 0\n" +
            "  - 则将 DataStatus 更新为 8（完结）\n\n" +
            "【预览功能】\n" +
            "  - 表1：显示需要修复的订单列表及当前状态\n" +
            "  - 表2：显示订单明细的数量对比（订单数量 vs 已出库数量）\n" +
            "  - 表3：显示出库单与订单明细的关联关系";

        public override string OccurrenceScenario =>
            "1. 2026年4月17-18日期间审核的销售出库单\n" +
            "2. 销售出库单审核时，代码BUG导致未正确执行数量回写逻辑\n" +
            "3. 出库明细通过SaleOrderDetail_ID关联订单明细，但审核时未正确累加\n" +
            "4. 订单已全部出库但状态仍为'已审核'而非'完结'\n" +
            "5. 财务对账时发现订单数量与实际出库数量不一致";

        /// <summary>
        /// 获取查询过滤器（用于动态生成查询UI）
        /// </summary>
        /// <returns>查询过滤器</returns>
        public override QueryFilter GetQueryFilter()
        {
            var queryFilter = new QueryFilter();

            // 订单编号
            var orderNo = new QueryField
            {
                FieldName = "SOrderNo",
                Caption = "订单编号",
                AdvQueryFieldType = AdvQueryProcessType.stringLike,
                IsVisible = true,
                ColDataType = typeof(string)
            };
            queryFilter.QueryFields.Add(orderNo);

            // 创建时间范围（使用 Created_at 字段）
            var createdDateFrom = new QueryField
            {
                FieldName = "Created_at",
                Caption = "订单开始日期",
                AdvQueryFieldType = AdvQueryProcessType.datetimeRange,
                IsVisible = true,
                ColDataType = typeof(DateTime?),
                Default1 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"), // 默认1个月前
                EnableDefault1 = true
            };
            queryFilter.QueryFields.Add(createdDateFrom);

            // 订单状态
            var dataStatus = new QueryField
            {
                FieldName = "DataStatus",
                Caption = "订单状态",
                AdvQueryFieldType = AdvQueryProcessType.EnumSelect,
                IsVisible = true,
                ColDataType = typeof(int),
                FieldType = QueryFieldType.CmbEnum,
                QueryFieldDataPara = new QueryFieldEnumData
                {
                    EnumType = typeof(DataStatus),
                    EnumValueColName = "DataStatus",
                    AddSelectItem = true
                }
            };
            queryFilter.QueryFields.Add(dataStatus);

            return queryFilter;
        }

        /// <summary>
        /// 获取查询使用的实体类型
        /// </summary>
        /// <returns>销售订单实体类型</returns>
        public override Type GetQueryEntityType()
        {
            // 直接使用真实的实体类型，它包含所有数据库字段
            return typeof(tb_SaleOrder);
        }

        /// <summary>
        /// 预览销售订单完结修复（支持多表）
        /// </summary>
        public override async Task<List<DataPreviewResult>> PreviewAsync(Dictionary<string, object> parameters = null)
        {
            var results = new List<DataPreviewResult>();

            // 获取查询条件参数
            DateTime? createdDateFrom = null;
            DateTime? createdDateTo = null;

            if (parameters != null)
            {
                // Created_at_Start 和 Created_at_End 是 datetimeRange 类型自动生成的属性
                if (parameters.ContainsKey("Created_at_Start") && parameters["Created_at_Start"] is DateTime dt1)
                    createdDateFrom = dt1;
                if (parameters.ContainsKey("Created_at_End") && parameters["Created_at_End"] is DateTime dt2)
                    createdDateTo = dt2;
            }

            // 表1：需要修复的订单列表
            var ordersResult = await PreviewOrdersNeedFixAsync(createdDateFrom, createdDateTo);
            results.Add(ordersResult);

            // 表2：订单明细数量对比
            if (ordersResult.NeedFixCount > 0)
            {
                var detailsResult = await PreviewOrderDetailsComparisonAsync(createdDateFrom, createdDateTo);
                results.Add(detailsResult);

                // 表3：出库单与订单明细关联
                var outResult = await PreviewOutDetailMappingAsync();
                results.Add(outResult);
            }

            return results;
        }

        /// <summary>
        /// 预览需要修复的订单
        /// </summary>
        private async Task<DataPreviewResult> PreviewOrdersNeedFixAsync(
            DateTime? createdDateFrom = null,
            DateTime? createdDateTo = null)
        {
            var result = new DataPreviewResult
            {
                TableName = "tb_SaleOrder",
                Description = "需要修复完结状态的销售订单（有出库但未完结）",
                Data = CreateDataTable("tb_SaleOrder")
            };

            // 添加列
            result.Data.Columns.Add("订单ID", typeof(long));
            result.Data.Columns.Add("订单编号", typeof(string));
            result.Data.Columns.Add("订单日期", typeof(DateTime?));
            result.Data.Columns.Add("当前状态", typeof(string));
            result.Data.Columns.Add("订单总数量", typeof(int));
            result.Data.Columns.Add("明细已出库总数", typeof(int));
            result.Data.Columns.Add("是否应完结", typeof(string));
            result.Data.Columns.Add("关联出库单数", typeof(int));
            result.Data.Columns.Add("最早出库日期", typeof(DateTime?));

            // 构建查询条件
            var query = Db.Queryable<tb_SaleOrder>()
                .Where(so => so.isdeleted == false && so.DataStatus < 8);

            // 添加创建时间范围条件
            if (createdDateFrom.HasValue)
            {
                query = query.Where(so => so.Created_at >= createdDateFrom.Value);
            }
            if (createdDateTo.HasValue)
            {
                query = query.Where(so => so.Created_at <= createdDateTo.Value.AddDays(1)); // 包含当天
            }

            var orders = await query.ToListAsync();

            int needFixCount = 0;

            foreach (var order in orders.Take(100))
            {
                // 构建出库单查询条件
                var outQuery = Db.Queryable<tb_SaleOut>()
                    .Where(o => o.SOrder_ID == order.SOrder_ID
                             && o.isdeleted == false
                             && o.DataStatus == (int)DataStatus.确认);

                // 检查是否有已审核的出库单
                var outCount = await outQuery.CountAsync();

                if (outCount == 0) continue;

                // 获取最早出库日期
                var earliestOutDate = await outQuery
                    .OrderBy(o => o.OutDate)
                    .Select(o => o.OutDate)
                    .FirstAsync();

                // 获取订单明细的已出库总数
                var detailSum = await Db.Queryable<tb_SaleOrderDetail>()
                    .Where(d => d.SOrder_ID == order.SOrder_ID)
                    .Select(d => new
                    {
                        TotalDelivered = SqlFunc.AggregateSum(d.TotalDeliveredQty),
                        TotalQty = SqlFunc.AggregateSum(d.Quantity)
                    })
                    .FirstAsync();

                if (detailSum == null) continue;

                int totalDelivered = detailSum.TotalDelivered;
                int totalQty = detailSum.TotalQty;

                // 判断是否应该完结
                bool shouldClose = (totalQty > 0 && totalDelivered >= totalQty);

                var row = result.Data.NewRow();
                row["订单ID"] = order.SOrder_ID;
                row["订单编号"] = order.SOrderNo;
                row["订单日期"] = order.Created_at;
                row["当前状态"] = GetDataStatusText(order.DataStatus);
                row["订单总数量"] = order.TotalQty;
                row["明细已出库总数"] = totalDelivered;
                row["是否应完结"] = shouldClose ? "是" : "否";
                row["关联出库单数"] = outCount;
                row["最早出库日期"] = earliestOutDate;

                result.Data.Rows.Add(row);

                if (shouldClose)
                {
                    needFixCount++;
                }
            }

            result.TotalCount = result.Data.Rows.Count;
            result.NeedFixCount = needFixCount;
            result.Data = LimitRows(result.Data, 100);

            return result;
        }

        /// <summary>
        /// 预览订单明细数量对比
        /// </summary>
        private async Task<DataPreviewResult> PreviewOrderDetailsComparisonAsync(
            DateTime? createdDateFrom = null,
            DateTime? createdDateTo = null)
        {
            var result = new DataPreviewResult
            {
                TableName = "tb_SaleOrderDetail",
                Description = "订单明细数量对比（订单数量 vs 已出库数量 vs 实际出库数量）",
                Data = CreateDataTable("tb_SaleOrderDetail")
            };

            // 添加列
            result.Data.Columns.Add("订单编号", typeof(string));
            result.Data.Columns.Add("订单日期", typeof(DateTime?));
            result.Data.Columns.Add("明细ID", typeof(long));
            result.Data.Columns.Add("产品名称", typeof(string));
            result.Data.Columns.Add("订单数量", typeof(int));
            result.Data.Columns.Add("当前已出库数", typeof(int));
            result.Data.Columns.Add("实际出库数", typeof(int));
            result.Data.Columns.Add("差异", typeof(int));
            result.Data.Columns.Add("是否需要修复", typeof(string));

            // 构建查询条件
            var query = Db.Queryable<tb_SaleOrderDetail>()
                .InnerJoin<tb_SaleOrder>((d, o) => d.SOrder_ID == o.SOrder_ID)
                .Where((d, o) => o.isdeleted == false && o.DataStatus < 8);

            // 添加订单日期范围条件
            if (createdDateFrom.HasValue)
            {
                query = query.Where((d, o) => o.Created_at >= createdDateFrom.Value);
            }
            if (createdDateTo.HasValue)
            {
                query = query.Where((d, o) => o.Created_at <= createdDateTo.Value.AddDays(1));
            }

            var details = await query
                .Select((d, o) => new
                {
                    o.SOrderNo,
                    o.Created_at,
                    d.SaleOrderDetail_ID,
                    d.SOrder_ID,
                    d.ProdDetailID,
                    d.Quantity,
                    d.TotalDeliveredQty
                })
                .ToListAsync();

            int needFixCount = 0;

            foreach (var detail in details.Take(100))
            {
                // 从缓存获取产品名称
                var prodDetail = _cacheManager.GetEntity<View_ProdDetail>(detail.ProdDetailID);
                string prodName = prodDetail?.CNName ?? "";
                
                // 计算实际出库数量（从出库单明细汇总）
                var actualOutQty = await Db.Queryable<tb_SaleOutDetail>()
                    .InnerJoin<tb_SaleOut>((od, o) => od.SaleOut_MainID == o.SaleOut_MainID)
                    .Where((od, o) => od.SaleOrderDetail_ID == detail.SaleOrderDetail_ID
                                   && o.isdeleted == false
                                   && o.DataStatus == (int)DataStatus.确认)
                    .SumAsync((od, o) => od.Quantity);

                int diff = actualOutQty - detail.TotalDeliveredQty;
                bool needFix = (diff != 0);

                if (needFix) needFixCount++;

                var row = result.Data.NewRow();
                row["订单编号"] = detail.SOrderNo;
                row["订单日期"] = detail.Created_at;
                row["明细ID"] = detail.SaleOrderDetail_ID;
                row["产品名称"] = prodName;
                row["订单数量"] = detail.Quantity;
                row["当前已出库数"] = detail.TotalDeliveredQty;
                row["实际出库数"] = actualOutQty;
                row["差异"] = diff;
                row["是否需要修复"] = needFix ? "是" : "否";

                result.Data.Rows.Add(row);
            }

            result.TotalCount = details.Count;
            result.NeedFixCount = needFixCount;
            result.Data = LimitRows(result.Data, 100);

            return result;
        }

        /// <summary>
        /// 预览出库单与订单明细关联
        /// </summary>
        private async Task<DataPreviewResult> PreviewOutDetailMappingAsync()
        {
            var result = new DataPreviewResult
            {
                TableName = "tb_SaleOutDetail",
                Description = "出库单明细与订单明细的关联关系（用于验证SaleOrderDetail_ID）",
                Data = CreateDataTable("tb_SaleOutDetail")
            };

            // 添加列
            result.Data.Columns.Add("出库单号", typeof(string));
            result.Data.Columns.Add("出库日期", typeof(DateTime?));
            result.Data.Columns.Add("出库明细ID", typeof(long));
            result.Data.Columns.Add("订单明细ID", typeof(long));
            result.Data.Columns.Add("产品名称", typeof(string));
            result.Data.Columns.Add("出库数量", typeof(int));
            result.Data.Columns.Add("关联状态", typeof(string));

            // 构建查询条件
            var query = Db.Queryable<tb_SaleOutDetail>()
                .InnerJoin<tb_SaleOut>((od, o) => od.SaleOut_MainID == o.SaleOut_MainID)
                .Where((od, o) => o.isdeleted == false
                                   && o.DataStatus == (int)DataStatus.确认
                                   && od.SaleOrderDetail_ID.HasValue);

            var outDetails = await query
                .OrderByDescending((od, o) => o.OutDate)
                .Select((od, o) => new
                {
                    o.SaleOutNo,
                    o.OutDate,
                    od.SaleOutDetail_ID,
                    od.SaleOrderDetail_ID,
                    od.ProdDetailID,
                    od.Quantity
                })
                .Take(100)
                .ToListAsync();

            foreach (var outDetail in outDetails)
            {
                // 从缓存获取产品名称
                var prodDetail = _cacheManager.GetEntity<View_ProdDetail>(outDetail.ProdDetailID);
                string prodName = prodDetail?.CNName ?? "";
                
                // 验证订单明细是否存在
                var orderDetailExists = await Db.Queryable<tb_SaleOrderDetail>()
                    .Where(d => d.SaleOrderDetail_ID == outDetail.SaleOrderDetail_ID)
                    .AnyAsync();

                var row = result.Data.NewRow();
                row["出库单号"] = outDetail.SaleOutNo;
                row["出库日期"] = outDetail.OutDate;
                row["出库明细ID"] = outDetail.SaleOutDetail_ID;
                row["订单明细ID"] = outDetail.SaleOrderDetail_ID;
                row["产品名称"] = prodName;
                row["出库数量"] = outDetail.Quantity;
                row["关联状态"] = orderDetailExists ? "正常" : "异常（订单明细不存在）";

                result.Data.Rows.Add(row);
            }

            result.TotalCount = outDetails.Count;
            result.NeedFixCount = outDetails.Count(od => !Db.Queryable<tb_SaleOrderDetail>()
                .Where(d => d.SaleOrderDetail_ID == od.SaleOrderDetail_ID).Any());

            return result;
        }

        /// <summary>
        /// 执行销售订单完结修复
        /// </summary>
        public override async Task<DataFixExecutionResult> ExecuteAsync(bool testMode = true, Dictionary<string, object> parameters = null)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new DataFixExecutionResult
            {
                Success = false,
                ExecutionTime = DateTime.Now
            };

            try
            {
                AddLog(result, $"开始执行销售订单完结修复（{(testMode ? "测试模式" : "正式模式")}）...");

                // 获取选中的ID列表（如果有）
                List<long> selectedOrderIds = null;
                if (parameters != null && parameters.ContainsKey("SelectedIds"))
                {
                    selectedOrderIds = parameters["SelectedIds"] as List<long>;
                    if (selectedOrderIds != null && selectedOrderIds.Count > 0)
                    {
                        AddLog(result, $"用户选中了 {selectedOrderIds.Count} 个订单进行修复");
                    }
                    else
                    {
                        selectedOrderIds = null; // 清空，表示全选
                    }
                }

                // 验证
                var (isValid, message) = await ValidateAsync();
                if (!isValid)
                {
                    result.ErrorMessage = message;
                    return result;
                }

                // 执行修复（带事务）
                await ExecuteWithTransactionAsync(async () =>
                {
                    // 步骤1：更新订单明细的TotalDeliveredQty
                    var detailFixCount = await FixOrderDetailQuantitiesAsync(result, testMode, selectedOrderIds);
                    RecordAffectedTable(result, "tb_SaleOrderDetail", detailFixCount);

                    // 步骤2：更新订单完结状态
                    var closeStatusCount = await FixOrderCloseStatusAsync(result, testMode, selectedOrderIds);
                    RecordAffectedTable(result, "tb_SaleOrder", closeStatusCount);

                    result.Success = true;
                    return result;

                }, testMode);

                if (testMode)
                {
                    AddLog(result, "测试模式：未实际修改数据库");
                }

                AddLog(result, $"修复完成，共影响 {result.AffectedTables.Count} 个表");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                AddLog(result, $"执行失败：{ex.Message}");
                AddLog(result, $"堆栈跟踪：{ex.StackTrace}");
            }
            finally
            {
                stopwatch.Stop();
                result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                AddLog(result, $"总耗时：{result.ElapsedMilliseconds}ms");
            }

            return result;
        }

        /// <summary>
        /// 修复订单明细的已出库数量
        /// </summary>
        private async Task<int> FixOrderDetailQuantitiesAsync(DataFixExecutionResult result, bool testMode, List<long> selectedOrderIds = null)
        {
            AddLog(result, "步骤1：正在更新订单明细的已出库数量...");

            // 查询所有需要修复的订单明细（如果选中了订单，则只处理这些订单）
            var query = Db.Queryable<tb_SaleOrderDetail>()
                .InnerJoin<tb_SaleOrder>((d, o) => d.SOrder_ID == o.SOrder_ID)
                .Where((d, o) => o.isdeleted == false && o.DataStatus < 8);

            if (selectedOrderIds != null && selectedOrderIds.Count > 0)
            {
                query = query.Where((d, o) => selectedOrderIds.Contains(o.SOrder_ID));
                AddLog(result, $"限定处理选中的 {selectedOrderIds.Count} 个订单");
            }

            var orderDetails = await query.ToListAsync();

            int fixedCount = 0;
            var updateList = new List<tb_SaleOrderDetail>();

            foreach (var detail in orderDetails)
            {
                // 计算该明细对应的实际出库数量（从已审核的出库单汇总）
                var actualOutQty = await Db.Queryable<tb_SaleOutDetail>()
                    .InnerJoin<tb_SaleOut>((od, o) => od.SaleOut_MainID == o.SaleOut_MainID)
                    .Where((od, o) => od.SaleOrderDetail_ID == detail.SaleOrderDetail_ID
                                   && o.isdeleted == false
                                   && o.DataStatus == (int)DataStatus.确认)
                    .SumAsync((od, o) => od.Quantity);

                // 如果实际出库数量与当前已出库数量不一致，则需要修复
                if (actualOutQty != detail.TotalDeliveredQty)
                {
                    int oldDelivered = detail.TotalDeliveredQty;

                    detail.TotalDeliveredQty = actualOutQty;

                    if (!testMode)
                    {
                        updateList.Add(detail);
                    }

                    fixedCount++;
                    AddLog(result, $"修复明细 SaleOrderDetail_ID={detail.SaleOrderDetail_ID}: " +
                                 $"已出库数 {oldDelivered} -> {detail.TotalDeliveredQty}");
                }
            }

            if (!testMode && updateList.Count > 0)
            {
                // 分批更新，每批500条
                int batchSize = 500;
                for (int i = 0; i < updateList.Count; i += batchSize)
                {
                    var batch = updateList.Skip(i).Take(batchSize).ToList();
                    await Db.Updateable(batch)
                        .UpdateColumns(d => new { d.TotalDeliveredQty })
                        .ExecuteCommandAsync();

                    AddLog(result, $"批次更新：已更新 {Math.Min(i + batchSize, updateList.Count)}/{updateList.Count} 条明细");
                }

                AddLog(result, $"已更新 {updateList.Count} 条订单明细");
            }

            return fixedCount;
        }

        /// <summary>
        /// 修复订单完结状态
        /// </summary>
        private async Task<int> FixOrderCloseStatusAsync(DataFixExecutionResult result, bool testMode, List<long> selectedOrderIds = null)
        {
            AddLog(result, "步骤2：正在更新订单完结状态...");

            // 查询所有需要更新的订单（如果选中了订单，则只处理这些订单）
            var query = Db.Queryable<tb_SaleOrder>()
                .Where(o => o.isdeleted == false && o.DataStatus < 8);

            if (selectedOrderIds != null && selectedOrderIds.Count > 0)
            {
                query = query.Where(o => selectedOrderIds.Contains(o.SOrder_ID));
                AddLog(result, $"限定处理选中的 {selectedOrderIds.Count} 个订单");
            }

            var orders = await query.ToListAsync();

            int closedCount = 0;
            var updateList = new List<tb_SaleOrder>();

            foreach (var order in orders)
            {
                // 汇总所有明细的已出库数量
                var detailCheck = await Db.Queryable<tb_SaleOrderDetail>()
                    .Where(d => d.SOrder_ID == order.SOrder_ID)
                    .Select(d => new
                    {
                        TotalQty = SqlFunc.AggregateSum(d.Quantity),
                        TotalDelivered = SqlFunc.AggregateSum(d.TotalDeliveredQty)
                    })
                    .FirstAsync();

                if (detailCheck != null && detailCheck.TotalQty > 0 && detailCheck.TotalDelivered >= detailCheck.TotalQty)
                {
                    if (!testMode)
                    {
                        order.DataStatus = (int)DataStatus.完结;
                        order.CloseCaseOpinions = "系统自动完结（全部出库）";
                        updateList.Add(order);
                    }

                    closedCount++;
                    AddLog(result, $"订单 {order.SOrderNo} 满足完结条件，将状态更新为完结");
                }
            }

            if (!testMode && updateList.Count > 0)
            {
                await Db.Updateable(updateList)
                    .UpdateColumns(o => new { o.DataStatus, o.CloseCaseOpinions })
                    .ExecuteCommandAsync();

                AddLog(result, $"已将 {updateList.Count} 个订单状态更新为完结");
            }

            return closedCount;
        }

        /// <summary>
        /// 验证是否可以执行
        /// </summary>
        public override async Task<(bool IsValid, string Message)> ValidateAsync()
        {
            // 检查是否有正在进行的销售业务
            var pendingOuts = await Db.Queryable<tb_SaleOut>()
                .Where(o => o.isdeleted == false && o.DataStatus == (int)DataStatus.新建)
                .CountAsync();

            if (pendingOuts > 100)
            {
                return (false, $"当前有 {pendingOuts} 个待审核的销售出库单，建议在业务低峰期执行");
            }

            // 检查需要修复的订单数量
            var ordersNeedFix = await Db.Queryable<tb_SaleOrder>()
                .Where(o => o.isdeleted == false && o.DataStatus < 8)
                .CountAsync();

            if (ordersNeedFix > 10000)
            {
                return (false, $"需要检查的订单数量过多（{ordersNeedFix}个），建议分批执行或联系开发人员");
            }

            return (true, $"验证通过，共有 {ordersNeedFix} 个订单需要检查");
        }

        /// <summary>
        /// 获取数据状态文本
        /// </summary>
        private string GetDataStatusText(int status)
        {
            return status switch
            {
                0 => "新建",
                1 => "草稿",
                2 => "提交",
                4 => "已审核",
                8 => "完结",
                _ => $"未知({status})"
            };
        }
    }
}
