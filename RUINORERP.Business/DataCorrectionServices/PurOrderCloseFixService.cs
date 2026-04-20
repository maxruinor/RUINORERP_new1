using AutoMapper;
using CacheManager.Core;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.Business.Document.Converters;
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
    /// 采购订单结案状态修复服务
    /// 修复采购入库审核后未正确更新订单明细数量、主表未交数量和结案状态的问题
    /// </summary>
    public class PurOrderCloseFixService : DataCorrectionServiceBase
    {
        private readonly ILogger<PurEntryToPurEntryReConverter> _logger;
        private readonly ApplicationContext _appContext;
        private readonly IEntityCacheManager _cacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="appContext">应用上下文</param>
        /// <param name="unitOfWorkManage">工作单元管理器</param>
        /// <param name="cacheManager">缓存管理器</param>
        public PurOrderCloseFixService(
            ILogger<PurEntryToPurEntryReConverter> logger,
            ApplicationContext appContext,
            IUnitOfWorkManage unitOfWorkManage,
            IEntityCacheManager cacheManager)
            : base(unitOfWorkManage)
        {
            _logger = logger;
            _appContext = appContext;
            _cacheManager = cacheManager;
        }

        public override string CorrectionName => "PurOrderCloseFix";

        public override string FunctionName => "采购订单结案状态修复";

        public override string ProblemDescription =>
            "修复采购入库单审核后，采购订单相关数据未正确更新的问题。\n\n" +
            "具体问题包括：\n" +
            "1. 采购订单明细的DeliveredQuantity（已交数量）未累加入库数量\n" +
            "2. 采购订单明细的UndeliveredQty（未交数量）未扣减入库数量\n" +
            "3. 采购订单主表的TotalUndeliveredQty（总未交数量）未汇总更新\n" +
            "4. 当所有明细都已入库完成时，订单状态DataStatus未更新为8（结案）\n\n" +
            "这导致系统无法正确判断订单是否已完成，影响后续的采购统计和报表。";

        public override List<string> AffectedTables => new List<string>
        {
            "tb_PurOrder",           // 采购订单主表
            "tb_PurOrderDetail",     // 采购订单明细表
            "tb_PurEntry",           // 采购入库单主表（用于验证）
            "tb_PurEntryDetail"      // 采购入库单明细表（用于计算实际入库数量）
        };

        public override string FixLogic =>
            "修复逻辑分为以下步骤：\n\n" +
            "【步骤1】检测需要修复的订单\n" +
            "  - 查找有已审核入库单（DataStatus=4）但订单状态不是结案（DataStatus≠8）的采购订单\n" +
            "  - 根据入库单明细的PurOrder_ChildID关联到订单明细\n" +
            "  - 按PurOrder_ChildID汇总实际入库数量\n\n" +
            "【步骤2】更新订单明细数量\n" +
            "  - DeliveredQuantity = 该明细对应的所有已审核入库数量之和\n" +
            "  - UndeliveredQty = Quantity - DeliveredQuantity\n" +
            "  - 确保 DeliveredQuantity + UndeliveredQty = Quantity\n\n" +
            "【步骤3】更新订单主表未交数量\n" +
            "  - TotalUndeliveredQty = SUM(所有明细的UndeliveredQty)\n\n" +
            "【步骤4】更新订单结案状态\n" +
            "  - 如果 TotalUndeliveredQty = 0 且 TotalQty > 0\n" +
            "  - 则将 DataStatus 更新为 8（结案）\n\n" +
            "【预览功能】\n" +
            "  - 表1：显示需要修复的订单列表及当前状态\n" +
            "  - 表2：显示订单明细的数量对比（订单数量 vs 已入库数量）\n" +
            "  - 表3：显示入库单与订单明细的关联关系";

        public override string OccurrenceScenario =>
            "1. 2026年4月17-18日期间审核的采购入库单\n" +
            "2. 采购入库单审核时，代码BUG导致未正确执行数量回写逻辑\n" +
            "3. 入库明细通过PurOrder_ChildID关联订单明细，但审核时未正确累加\n" +
            "4. 订单已全部入库但状态仍为'已审核'而非'结案'\n" +
            "5. 财务对账时发现订单数量与实际入库数量不一致";

        /// <summary>
        /// 获取查询过滤器（用于动态生成查询UI）
        /// </summary>
        /// <returns>查询过滤器</returns>
        public override QueryFilter GetQueryFilter()
        {
            var queryFilter = new QueryFilter();

            // 采购单号
            var orderNo = new QueryField
            {
                FieldName = "PurOrderNo",
                Caption = "采购单号",
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
        /// <returns>采购订单实体类型</returns>
        public override Type GetQueryEntityType()
        {
            // 直接使用真实的实体类型，它包含所有数据库字段
            return typeof(tb_PurOrder);
        }

        /// <summary>
        /// 预览采购订单结案修复（支持多表）
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

                // 表3：入库单与订单明细关联
                var entryResult = await PreviewEntryDetailMappingAsync();
                results.Add(entryResult);
            }

            return results;
        }

        /// <summary>
        /// 预览需要修复的订单
        /// </summary>
        private async Task<DataPreviewResult> PreviewOrdersNeedFixAsync(
            DateTime? orderDateFrom = null,
            DateTime? orderDateTo = null,
            DateTime? entryDateFrom = null,
            DateTime? entryDateTo = null)
        {
            var result = new DataPreviewResult
            {
                TableName = "tb_PurOrder",
                Description = "需要修复结案状态的采购订单（有入库但未结案）",
                Data = CreateDataTable("tb_PurOrder")
            };

            // 添加列
            result.Data.Columns.Add("订单ID", typeof(long));
            result.Data.Columns.Add("订单编号", typeof(string));
            result.Data.Columns.Add("订单日期", typeof(DateTime?));
            result.Data.Columns.Add("当前状态", typeof(string));
            result.Data.Columns.Add("订单总数量", typeof(int));
            result.Data.Columns.Add("主表未交数量", typeof(int));
            result.Data.Columns.Add("明细已入库总数", typeof(int));
            result.Data.Columns.Add("是否应结案", typeof(string));
            result.Data.Columns.Add("关联入库单数", typeof(int));
            result.Data.Columns.Add("最早入库日期", typeof(DateTime?));

            // 构建查询条件
            var query = Db.Queryable<tb_PurOrder>()
                .Where(po => po.isdeleted == false && po.DataStatus < 8);

            // 添加订单日期范围条件
            if (orderDateFrom.HasValue)
            {
                query = query.Where(po => po.Created_at >= orderDateFrom.Value);
            }
            if (orderDateTo.HasValue)
            {
                query = query.Where(po => po.Created_at <= orderDateTo.Value.AddDays(1)); // 包含当天
            }

            var orders = await query.ToListAsync();

            int needFixCount = 0;

            foreach (var order in orders.Take(100))
            {
                // 构建入库单查询条件
                var entryQuery = Db.Queryable<tb_PurEntry>()
                    .Where(e => e.PurOrder_ID == order.PurOrder_ID
                             && e.isdeleted == false
                             && e.DataStatus == (int)DataStatus.确认);

                // 添加入库日期范围条件
                if (entryDateFrom.HasValue)
                {
                    entryQuery = entryQuery.Where(e => e.Created_at >= entryDateFrom.Value);
                }
                if (entryDateTo.HasValue)
                {
                    entryQuery = entryQuery.Where(e => e.Created_at <= entryDateTo.Value.AddDays(1));
                }

                // 检查是否有已审核的入库单
                var entryCount = await entryQuery.CountAsync();

                if (entryCount == 0) continue;

                // 获取最早入库日期
                var earliestEntryDate = await entryQuery
                    .OrderBy(e => e.Created_at)
                    .Select(e => e.Created_at)
                    .FirstAsync();

                // 获取订单明细的已入库总数
                var detailSum = await Db.Queryable<tb_PurOrderDetail>()
                    .Where(d => d.PurOrder_ID == order.PurOrder_ID)
                    .Select(d => new
                    {
                        TotalDelivered = SqlFunc.AggregateSum(d.DeliveredQuantity),
                        TotalUndelivered = SqlFunc.AggregateSum(d.UndeliveredQty),
                        TotalQty = SqlFunc.AggregateSum(d.Quantity)
                    })
                    .FirstAsync();

                if (detailSum == null) continue;

                int totalDelivered = detailSum.TotalDelivered;
                int totalUndelivered = detailSum.TotalUndelivered;
                int totalQty = detailSum.TotalQty;

                // 判断是否应该结案
                bool shouldClose = (totalQty > 0 && totalDelivered >= totalQty && totalUndelivered <= 0);

                var row = result.Data.NewRow();
                row["订单ID"] = order.PurOrder_ID;
                row["订单编号"] = order.PurOrderNo;
                row["订单日期"] = order.Created_at;
                row["当前状态"] = GetDataStatusText(order.DataStatus);
                row["订单总数量"] = order.TotalQty;
                row["主表未交数量"] = order.TotalUndeliveredQty;
                row["明细已入库总数"] = totalDelivered;
                row["是否应结案"] = shouldClose ? "是" : "否";
                row["关联入库单数"] = entryCount;
                row["最早入库日期"] = earliestEntryDate;

                result.Data.Rows.Add(row);

                if (shouldClose || order.TotalUndeliveredQty != totalUndelivered)
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
            DateTime? orderDateFrom = null,
            DateTime? orderDateTo = null,
            DateTime? entryDateFrom = null,
            DateTime? entryDateTo = null)
        {
            var result = new DataPreviewResult
            {
                TableName = "tb_PurOrderDetail",
                Description = "订单明细数量对比（订单数量 vs 已入库数量 vs 实际入库数量）",
                Data = CreateDataTable("tb_PurOrderDetail")
            };

            // 添加列
            result.Data.Columns.Add("订单编号", typeof(string));
            result.Data.Columns.Add("订单日期", typeof(DateTime?));
            result.Data.Columns.Add("明细ID", typeof(long));
            result.Data.Columns.Add("产品名称", typeof(string));
            result.Data.Columns.Add("订单数量", typeof(int));
            result.Data.Columns.Add("当前已交数", typeof(int));
            result.Data.Columns.Add("当前未交数", typeof(int));
            result.Data.Columns.Add("实际入库数", typeof(int));
            result.Data.Columns.Add("差异", typeof(int));
            result.Data.Columns.Add("是否需要修复", typeof(string));

            // 构建查询条件
            var query = Db.Queryable<tb_PurOrderDetail>()
                .InnerJoin<tb_PurOrder>((d, o) => d.PurOrder_ID == o.PurOrder_ID)
                .Where((d, o) => o.isdeleted == false && o.DataStatus < 8);

            // 添加订单日期范围条件
            if (orderDateFrom.HasValue)
            {
                query = query.Where((d, o) => o.Created_at >= orderDateFrom.Value);
            }
            if (orderDateTo.HasValue)
            {
                query = query.Where((d, o) => o.Created_at <= orderDateTo.Value.AddDays(1));
            }

            var details = await query
                .Select((d, o) => new
                {
                    o.PurOrderNo,
                    o.Created_at,
                    d.PurOrder_ChildID,
                    d.PurOrder_ID,
                    d.ProdDetailID,
                    d.Quantity,
                    d.DeliveredQuantity,
                    d.UndeliveredQty,
                    d.Location_ID
                })
                .ToListAsync();

            int needFixCount = 0;

            foreach (var detail in details.Take(100))
            {
                // 从缓存获取产品名称
                var prodDetail = _cacheManager.GetEntity<View_ProdDetail>(detail.ProdDetailID);
                string prodName = prodDetail?.CNName ?? "";
                
                // 计算实际入库数量（从入库单明细汇总，添加入库日期条件）
                var entryQuery = Db.Queryable<tb_PurEntryDetail>()
                    .InnerJoin<tb_PurEntry>((ed, e) => ed.PurEntryID == e.PurEntryID)
                    .Where((ed, e) => ed.PurOrder_ChildID == detail.PurOrder_ChildID
                                   && e.isdeleted == false
                                   && e.DataStatus == (int)DataStatus.确认);

                if (entryDateFrom.HasValue)
                {
                    entryQuery = entryQuery.Where((ed, e) => e.Created_at >= entryDateFrom.Value);
                }
                if (entryDateTo.HasValue)
                {
                    entryQuery = entryQuery.Where((ed, e) => e.Created_at <= entryDateTo.Value.AddDays(1));
                }

                var actualInQty = await entryQuery.SumAsync((ed, e) => ed.Quantity);

                int diff = actualInQty - detail.DeliveredQuantity;
                bool needFix = (diff != 0 || detail.DeliveredQuantity + detail.UndeliveredQty != detail.Quantity);

                if (needFix) needFixCount++;

                var row = result.Data.NewRow();
                row["订单编号"] = detail.PurOrderNo;
                row["订单日期"] = detail.Created_at;
                row["明细ID"] = detail.PurOrder_ChildID;
                row["产品名称"] = prodName;
                row["订单数量"] = detail.Quantity;
                row["当前已交数"] = detail.DeliveredQuantity;
                row["当前未交数"] = detail.UndeliveredQty;
                row["实际入库数"] = actualInQty;
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
        /// 预览入库单与订单明细关联
        /// </summary>
        private async Task<DataPreviewResult> PreviewEntryDetailMappingAsync(
            DateTime? entryDateFrom = null,
            DateTime? entryDateTo = null)
        {
            var result = new DataPreviewResult
            {
                TableName = "tb_PurEntryDetail",
                Description = "入库单明细与订单明细的关联关系（用于验证PurOrder_ChildID）",
                Data = CreateDataTable("tb_PurEntryDetail")
            };

            // 添加列
            result.Data.Columns.Add("入库单号", typeof(string));
            result.Data.Columns.Add("入库日期", typeof(DateTime?));
            result.Data.Columns.Add("入库明细ID", typeof(long));
            result.Data.Columns.Add("订单明细ID", typeof(long));
            result.Data.Columns.Add("产品名称", typeof(string));
            result.Data.Columns.Add("入库数量", typeof(int));
            result.Data.Columns.Add("关联状态", typeof(string));

            // 构建查询条件
            var query = Db.Queryable<tb_PurEntryDetail>()
                .InnerJoin<tb_PurEntry>((ed, e) => ed.PurEntryID == e.PurEntryID)
                .Where((ed, e) => e.isdeleted == false
                                   && e.DataStatus == (int)DataStatus.确认
                                   && ed.PurOrder_ChildID.HasValue);

            // 添加入库日期范围条件
            if (entryDateFrom.HasValue)
            {
                query = query.Where((ed, e) => e.Created_at >= entryDateFrom.Value);
            }
            if (entryDateTo.HasValue)
            {
                query = query.Where((ed, e) => e.Created_at <= entryDateTo.Value.AddDays(1));
            }

            var entryDetails = await query
                .OrderByDescending((ed, e) => e.Created_at)
                .Select((ed, e) => new
                {
                    e.PurEntryNo,
                    e.Created_at,
                    ed.PurEntryDetail_ID,
                    ed.PurOrder_ChildID,
                    ed.ProdDetailID,
                    ed.Quantity
                })
                .Take(100)
                .ToListAsync();

            foreach (var entryDetail in entryDetails)
            {
                // 从缓存获取产品名称
                var prodDetail = _cacheManager.GetEntity<View_ProdDetail>(entryDetail.ProdDetailID);
                string prodName = prodDetail?.CNName ?? "";
                
                // 验证订单明细是否存在
                var orderDetailExists = await Db.Queryable<tb_PurOrderDetail>()
                    .Where(d => d.PurOrder_ChildID == entryDetail.PurOrder_ChildID)
                    .AnyAsync();

                var row = result.Data.NewRow();
                row["入库单号"] = entryDetail.PurEntryNo;
                row["入库日期"] = entryDetail.Created_at;
                row["入库明细ID"] = entryDetail.PurEntryDetail_ID;
                row["订单明细ID"] = entryDetail.PurOrder_ChildID;
                row["产品名称"] = prodName;
                row["入库数量"] = entryDetail.Quantity;
                row["关联状态"] = orderDetailExists ? "正常" : "异常（订单明细不存在）";

                result.Data.Rows.Add(row);
            }

            result.TotalCount = entryDetails.Count;
            result.NeedFixCount = entryDetails.Count(ed => !Db.Queryable<tb_PurOrderDetail>()
                .Where(d => d.PurOrder_ChildID == ed.PurOrder_ChildID).Any());

            return result;
        }

        /// <summary>
        /// 执行采购订单结案修复
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
                AddLog(result, $"开始执行采购订单结案修复（{(testMode ? "测试模式" : "正式模式")}）...");

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
                    // 步骤1：更新订单明细的DeliveredQuantity和UndeliveredQty
                    var detailFixCount = await FixOrderDetailQuantitiesAsync(result, testMode, selectedOrderIds);
                    RecordAffectedTable(result, "tb_PurOrderDetail", detailFixCount);

                    // 步骤2：更新订单主表的TotalUndeliveredQty
                    var orderMainFixCount = await FixOrderMainUndeliveredQtyAsync(result, testMode, selectedOrderIds);
                    RecordAffectedTable(result, "tb_PurOrder", orderMainFixCount);

                    // 步骤3：更新订单结案状态
                    var closeStatusCount = await FixOrderCloseStatusAsync(result, testMode, selectedOrderIds);
                    RecordAffectedTable(result, "tb_PurOrder", closeStatusCount);

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
        /// 修复订单明细的已交数量和未交数量
        /// </summary>
        private async Task<int> FixOrderDetailQuantitiesAsync(DataFixExecutionResult result, bool testMode, List<long> selectedOrderIds = null)
        {
            AddLog(result, "步骤1：正在更新订单明细的已交数量和未交数量...");

            // 查询所有需要修复的订单明细（如果选中了订单，则只处理这些订单）
            var query = Db.Queryable<tb_PurOrderDetail>()
                .InnerJoin<tb_PurOrder>((d, o) => d.PurOrder_ID == o.PurOrder_ID)
                .Where((d, o) => o.isdeleted == false && o.DataStatus < 8);

            if (selectedOrderIds != null && selectedOrderIds.Count > 0)
            {
                query = query.Where((d, o) => selectedOrderIds.Contains(o.PurOrder_ID));
                AddLog(result, $"限定处理选中的 {selectedOrderIds.Count} 个订单");
            }

            var orderDetails = await query.ToListAsync();

            int fixedCount = 0;
            var updateList = new List<tb_PurOrderDetail>();

            foreach (var detail in orderDetails)
            {
                // 计算该明细对应的实际入库数量（从已审核的入库单汇总）
                var actualInQty = await Db.Queryable<tb_PurEntryDetail>()
                    .InnerJoin<tb_PurEntry>((ed, e) => ed.PurEntryID == e.PurEntryID)
                    .Where((ed, e) => ed.PurOrder_ChildID == detail.PurOrder_ChildID
                                   && e.isdeleted == false
                                   && e.DataStatus == (int)DataStatus.确认)
                    .SumAsync((ed, e) => ed.Quantity);

                // 如果实际入库数量与当前已交数量不一致，则需要修复
                if (actualInQty != detail.DeliveredQuantity)
                {
                    int oldDelivered = detail.DeliveredQuantity;
                    int oldUndelivered = detail.UndeliveredQty;

                    detail.DeliveredQuantity = actualInQty;
                    detail.UndeliveredQty = detail.Quantity - actualInQty;

                    if (!testMode)
                    {
                        updateList.Add(detail);
                    }

                    fixedCount++;
                    AddLog(result, $"修复明细 PurOrder_ChildID={detail.PurOrder_ChildID}: " +
                                 $"已交数 {oldDelivered} -> {detail.DeliveredQuantity}, " +
                                 $"未交数 {oldUndelivered} -> {detail.UndeliveredQty}");
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
                        .UpdateColumns(d => new { d.DeliveredQuantity, d.UndeliveredQty })
                        .ExecuteCommandAsync();

                    AddLog(result, $"批次更新：已更新 {Math.Min(i + batchSize, updateList.Count)}/{updateList.Count} 条明细");
                }

                AddLog(result, $"已更新 {updateList.Count} 条订单明细");
            }

            return fixedCount;
        }

        /// <summary>
        /// 修复订单主表的总未交数量
        /// </summary>
        private async Task<int> FixOrderMainUndeliveredQtyAsync(DataFixExecutionResult result, bool testMode, List<long> selectedOrderIds = null)
        {
            AddLog(result, "步骤2：正在更新订单主表的总未交数量...");

            // 查询所有需要更新的订单（如果选中了订单，则只处理这些订单）
            var query = Db.Queryable<tb_PurOrder>()
                .Where(o => o.isdeleted == false && o.DataStatus < 8);

            if (selectedOrderIds != null && selectedOrderIds.Count > 0)
            {
                query = query.Where(o => selectedOrderIds.Contains(o.PurOrder_ID));
                AddLog(result, $"限定处理选中的 {selectedOrderIds.Count} 个订单");
            }

            var orders = await query.ToListAsync();

            int fixedCount = 0;
            var updateList = new List<tb_PurOrder>();

            foreach (var order in orders)
            {
                // 汇总所有明细的未交数量
                var totalUndelivered = await Db.Queryable<tb_PurOrderDetail>()
                    .Where(d => d.PurOrder_ID == order.PurOrder_ID)
                    .SumAsync(d => d.UndeliveredQty);

                if (totalUndelivered != order.TotalUndeliveredQty)
                {
                    int oldTotalUndelivered = order.TotalUndeliveredQty;
                    order.TotalUndeliveredQty = totalUndelivered;

                    if (!testMode)
                    {
                        updateList.Add(order);
                    }

                    fixedCount++;
                    AddLog(result, $"修复订单 {order.PurOrderNo}: " +
                                 $"总未交数 {oldTotalUndelivered} -> {order.TotalUndeliveredQty}");
                }
            }

            if (!testMode && updateList.Count > 0)
            {
                await Db.Updateable(updateList)
                    .UpdateColumns(o => new { o.TotalUndeliveredQty })
                    .ExecuteCommandAsync();

                AddLog(result, $"已更新 {updateList.Count} 个订单主表的总未交数量");
            }

            return fixedCount;
        }

        /// <summary>
        /// 修复订单结案状态
        /// </summary>
        private async Task<int> FixOrderCloseStatusAsync(DataFixExecutionResult result, bool testMode, List<long> selectedOrderIds = null)
        {
            AddLog(result, "步骤3：正在更新订单结案状态...");

            // 查询所有未交数量为0但未结案的订单（如果选中了订单，则只处理这些订单）
            var query = Db.Queryable<tb_PurOrder>()
                .Where(o => o.isdeleted == false
                         && o.DataStatus == (int)DataStatus.确认
                         && o.TotalUndeliveredQty == 0
                         && o.TotalQty > 0);

            if (selectedOrderIds != null && selectedOrderIds.Count > 0)
            {
                query = query.Where(o => selectedOrderIds.Contains(o.PurOrder_ID));
                AddLog(result, $"限定处理选中的 {selectedOrderIds.Count} 个订单");
            }

            var ordersToClose = await query.ToListAsync();

            int closedCount = 0;
            var updateList = new List<tb_PurOrder>();

            foreach (var order in ordersToClose)
            {
                // 验证是否真的全部入库
                var detailCheck = await Db.Queryable<tb_PurOrderDetail>()
                    .Where(d => d.PurOrder_ID == order.PurOrder_ID)
                    .Select(d => new
                    {
                        TotalQty = SqlFunc.AggregateSum(d.Quantity),
                        TotalDelivered = SqlFunc.AggregateSum(d.DeliveredQuantity)
                    })
                    .FirstAsync();

                if (detailCheck != null && detailCheck.TotalQty == detailCheck.TotalDelivered)
                {
                    if (!testMode)
                    {
                        order.DataStatus = (int)DataStatus.完结;
                        order.CloseCaseOpinions = "系统自动结案（全部入库）";
                        updateList.Add(order);
                    }

                    closedCount++;
                    AddLog(result, $"订单 {order.PurOrderNo} 满足结案条件，将状态更新为结案");
                }
            }

            if (!testMode && updateList.Count > 0)
            {
                await Db.Updateable(updateList)
                    .UpdateColumns(o => new { o.DataStatus, o.CloseCaseOpinions })
                    .ExecuteCommandAsync();

                AddLog(result, $"已将 {updateList.Count} 个订单状态更新为结案");
            }

            return closedCount;
        }

        /// <summary>
        /// 验证是否可以执行
        /// </summary>
        public override async Task<(bool IsValid, string Message)> ValidateAsync()
        {
            // 检查是否有正在进行的采购业务
            var pendingEntries = await Db.Queryable<tb_PurEntry>()
                .Where(e => e.isdeleted == false && e.DataStatus == (int)DataStatus.新建)
                .CountAsync();

            if (pendingEntries > 100)
            {
                return (false, $"当前有 {pendingEntries} 个待审核的采购入库单，建议在业务低峰期执行");
            }

            // 检查需要修复的订单数量
            var ordersNeedFix = await Db.Queryable<tb_PurOrder>()
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
                8 => "结案",
                _ => $"未知({status})"
            };
        }
    }
}
