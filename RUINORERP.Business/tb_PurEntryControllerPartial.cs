
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/01/2023 18:04:35
// **************************************
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using FluentValidation.Results;
using RUINORERP.Services;

using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINORERP.Global;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.Security;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using RUINORERP.Business.CommService;
using AutoMapper;
using RUINORERP.Global.EnumExt;
using SharpYaml.Tokens;

namespace RUINORERP.Business
{
    public partial class tb_PurEntryController<T>
    {

        /// <summary>
        /// 返回批量审核的结果1
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_PurEntry entity = ObjectEntity as tb_PurEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            
            try
            {
                // ========== 第一阶段: 预处理验证(无事务) ==========
                
                // 1.1 基础验证 - 检查重复审核
                var existingEntity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                    .Where(c => c.PurEntryID == entity.PurEntryID)
                    .Select(c => new { c.DataStatus, c.ApprovalStatus, c.ApprovalResults })
                    .FirstAsync();

                if (existingEntity != null && 
                    existingEntity.DataStatus == (int)DataStatus.确认 && 
                    existingEntity.ApprovalStatus == (int)ApprovalStatus.审核通过)
                {
                    rs.ErrorMsg = "采购入库单已经审核通过，不能重复审核！";
                    return rs;
                }

                // 1.2 加载依赖数据
                if (entity.tb_PurEntryDetails == null)
                {
                    entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                         .Includes(b => b.tb_PurEntryDetails)
                         .Where(c => c.PurEntryID == entity.PurEntryID)
                         .SingleAsync();
                }

                // 1.3 基础业务规则验证
                if (entity.TotalQty.Equals(entity.tb_PurEntryDetails.Sum(c => c.Quantity)) == false)
                {
                    rs.ErrorMsg = $"采入入库数量与明细之和不相等!请检查数据后重试！";
                    return rs;
                }

                // 1.4 加载并验证采购订单(如果关联)
                if (entity.PurOrder_ID.HasValue && entity.PurOrder_ID.Value > 0)
                {
                    entity.tb_purorder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                         .Includes(a => a.tb_PurEntries, b => b.tb_PurEntryDetails)
                         .Includes(t => t.tb_PurOrderDetails)
                         .AsNavQueryable()
                         .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                         .Where(c => c.PurOrder_ID == entity.PurOrder_ID)
                         .SingleAsync();

                    if (entity.tb_purorder == null)
                    {
                        rs.ErrorMsg = $"没有找到对应的采购订单!请检查数据后重试！";
                        return rs;
                    }

                    // 验证供应商一致性
                    if (entity.CustomerVendor_ID != entity.tb_purorder.CustomerVendor_ID)
                    {
                        rs.ErrorMsg = $"入库供应商和采购订单供应商不同!请检查数据后重试！";
                        return rs;
                    }

                    // 验证订单状态
                    bool isOrderConfirmed = entity.tb_purorder.DataStatus == (int)DataStatus.确认;
                    bool isApproved = entity.tb_purorder.ApprovalResults.HasValue && entity.tb_purorder.ApprovalResults.Value;

                    if (!isOrderConfirmed || !isApproved)
                    {
                        rs.ErrorMsg = $"{entity.tb_purorder.PurOrderNo} 请确认采购订单状态为【确认】已审核，并且审核结果为已通过!请检查数据后重试！";
                        return rs;
                    }

                    // 验证产品归属
                    foreach (var child in entity.tb_PurEntryDetails)
                    {
                        if (!entity.tb_purorder.tb_PurOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID))
                        {
                            rs.ErrorMsg = $"入库明细中有产品不属于当前订单!请检查数据后重试！";
                            return rs;
                        }
                    }

                    // 验证数量超限
                    var validationResult = ValidatePurOrderQuantity(entity);
                    if (!validationResult.Succeeded)
                    {
                        return validationResult;
                    }

                    // 更新订单明细的已入库数量和未入库数量
                    UpdatePurOrderDeliveryQty(entity);

                    // 计算订单未交数量和结案状态
                    CalculateOrderDeliveryStatus(entity);
                }

                // 1.5 预加载库存和价格记录(性能优化)
                var preloadResult = await PreloadInventoryAndPricesAsync(entity);
                var inventoryGroups = preloadResult.InventoryGroups;
                var invUpdateList = preloadResult.InvUpdateList;
                var transactionList = preloadResult.TransactionList;
                var priceRecords = preloadResult.PriceRecords;
                var bomDetails = preloadResult.BOMDetails;
                var boms = preloadResult.BOMs;

                // ========== 第二阶段: 事务内执行核心业务 ==========
                _unitOfWorkManage.BeginTran();
                
                try
                {
                    // 2.1 更新采购订单状态(如果关联)
                    if (entity.tb_purorder != null)
                    {
                        int orderCounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder)
                            .UpdateColumns(it => new { it.TotalUndeliveredQty, it.DataStatus, it.CloseCaseOpinions })
                            .ExecuteCommandAsync();

                        int poCounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder.tb_PurOrderDetails)
                            .UpdateColumns(it => new { it.DeliveredQuantity, it.UndeliveredQty })
                            .ExecuteCommandAsync();

                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Debug($"{entity.PurEntryNo} ==> {entity.PurOrder_NO} 订单更新成功");
                        }
                    }

                    // 2.2 更新库存
                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (invUpdateList.Count > 0 && counter == 0)
                    {
                        throw new Exception("入库时，库存更新数据为0，更新失败！");
                    }

                    // 2.3 记录库存流水
                    tb_InventoryTransactionController<tb_InventoryTransaction> tranController = 
                        _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                    await tranController.BatchRecordTransactionsWithRetry(transactionList);

                    // 2.4 更新BOM成本(如果有)
                    if (bomDetails.Any())
                    {
                        DbHelper<tb_BOM_SDetail> bomDetailDbHelper = _appContext.GetRequiredService<DbHelper<tb_BOM_SDetail>>();
                        var bomDetailCounter = await bomDetailDbHelper.BaseDefaultAddElseUpdateAsync(bomDetails);
                        if (bomDetailCounter == 0)
                        {
                            throw new Exception("入库时，配方明细成本更新数据为0，更新失败！");
                        }
                    }

                    if (boms.Count > 0)
                    {
                        DbHelper<tb_BOM_S> bomDbHelper = _appContext.GetRequiredService<DbHelper<tb_BOM_S>>();
                        var bomCounter = await bomDbHelper.BaseDefaultAddElseUpdateAsync(boms);
                        if (bomCounter == 0)
                        {
                            throw new Exception("入库时，配方主表成本更新数据为0，更新失败！");
                        }
                    }

                    // 2.5 更新价格记录
                    if (priceRecords.Count > 0)
                    {
                        DbHelper<tb_PriceRecord> priceRecordDbHelper = _appContext.GetRequiredService<DbHelper<tb_PriceRecord>>();
                        var priceRecordCounter = await priceRecordDbHelper.BaseDefaultAddElseUpdateAsync(priceRecords);
                        if (priceRecordCounter == 0)
                        {
                            throw new Exception("入库时，采购价格历史记录更新数据为0，更新失败！");
                        }
                    }

                    // 2.6 更新入库单状态
                    entity.DataStatus = (int)DataStatus.确认;
                    entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                    BusinessHelper.Instance.ApproverEntity(entity);
                    
                    int updateCounter = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                        .UpdateColumns(it => new { it.DataStatus, it.ApprovalStatus, it.ApprovalResults, it.ApprovalOpinions })
                        .ExecuteCommandAsync();

                    // 提交主事务
                    _unitOfWorkManage.CommitTran();
                    _logger.LogInformation($"采购入库单{entity.PurEntryNo}审核：主事务提交成功");

                    // ========== 第三阶段: 后置处理(独立事务) ==========
                    AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                    if (authorizeController.EnableFinancialModule())
                    {
                        var financeResult = await ProcessFinanceAfterApprovalAsync(entity);
                        if (financeResult.Succeeded)
                        {
                            rs.ReturnObjectAsOtherEntity = financeResult.ReturnObject;
                        }
                        else
                        {
                            // 财务处理失败不影响主业务流程，记录警告日志
                            _logger.LogWarning($"采购入库单{entity.PurEntryNo}审核：主事务成功，但财务处理失败 - {financeResult.ErrorMsg}");
                        }
                    }

                    rs.ReturnObject = entity as T;
                    rs.Succeeded = true;
                    return rs;
                }
                catch (Exception ex)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, $"事务回滚{entity.PurEntryNo}: {ex.Message}");
                rs.Succeeded = false;
                rs.ErrorMsg = ex.Message;
                return rs;
            }
        }



        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_PurEntry entity = ObjectEntity as tb_PurEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            try
            {
                //判断是否能反审? 意思是。我这个入库单错了。但是你都当入库成功进行了后面的操作了，现在要反审，那肯定不行。所以，要判断，
                if (entity.tb_PurEntryRes != null
                    && (entity.tb_PurEntryRes.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_PurEntryRes.Any(c => c.ApprovalStatus == (int)ApprovalStatus.审核通过)))
                {
                    rs.ErrorMsg = "存在已确认或已完结，或已审核的【采购入库退回单】，不能反审核  ";
                    return rs;
                }



                var ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal PurQtySum, bool? IsGift,
                    decimal UntaxedUnitPrice, DateTime LatestStorageTime)>();

                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var prodDetailIds = entity.tb_PurEntryDetails.Select(c => c.ProdDetailID).Distinct().ToList();
                var requiredPairs = entity.tb_PurEntryDetails
                    .Select(c => new { c.ProdDetailID, c.Location_ID })
                    .Distinct()
                    .ToHashSet();

                var inventoryList = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_Inventory>()
                    .Where(i => prodDetailIds.Contains(i.ProdDetailID))
                    .ToListAsync();

                inventoryList = inventoryList.Where(i => requiredPairs.Contains(new { i.ProdDetailID, i.Location_ID })).ToList();
                var invDict = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                #endregion

                // 声明价格记录 Lookup（用于后续循环中获取最新价格）
                ILookup<long, tb_PriceRecord> priceRecordLookup = null;

                #region 【优化】批量预加载采购价格记录，避免循环内查询（事务外）
                // 使用 ToLookup 支持同一产品多条历史价格记录
                if (entity.tb_purorder != null && entity.tb_purorder.Employee_ID > 0 && prodDetailIds.Count > 0)
                {
                    var priceRecordList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_PriceRecord>()
                        .Where(c => c.Employee_ID == entity.tb_purorder.Employee_ID &&
                                    prodDetailIds.Contains(c.ProdDetailID))
                        .OrderByDescending(c => c.PurDate)
                        .ToListAsync();

                    priceRecordLookup = priceRecordList.ToLookup(p => p.ProdDetailID);
                }
                #endregion

                // 遍历销售订单明细，聚合数据
                foreach (var child in entity.tb_PurEntryDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentEntryQty = child.Quantity;
                    DateTime currentStorageTime = DateTime.Now;

                    // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        //实际 期初已经有数据了，则要
                        // ✅ 从预加载字典获取（死锁优化）
                        if (!invDict.TryGetValue(key, out var inv) || inv == null)
                        {
                            rs.ErrorMsg = $"产品{child.ProdDetailID}在仓库{child.Location_ID}中无库存数据，无法反审核";
                            rs.Succeeded = false;
                            return rs;
                        }
                        BusinessHelper.Instance.EditEntity(inv);
                        #endregion

                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            PurQtySum: currentEntryQty, // 首次累加
                            IsGift: child.IsGift,
                            UntaxedUnitPrice: child.UntaxedUnitPrice,
                            LatestStorageTime: currentStorageTime
                        );
                        inventoryGroups[key] = group;
                    }
                    else
                    {
                        // ✅ P1修复：改进加权平均单价计算，减少精度累积误差
                        // 原公式：((currentEntryQty * child.UntaxedUnitPrice) + (group.UntaxedUnitPrice * group.PurQtySum)) / (group.PurQtySum + currentEntryQty)
                        // 问题：在多次累加时会产生浮点数精度累积误差
                        // 修复：使用decimal高精度类型，并确保除法运算在最后一步进行
                        
                        decimal totalCost = (currentEntryQty * child.UntaxedUnitPrice) + (group.UntaxedUnitPrice * group.PurQtySum);
                        decimal totalQty = group.PurQtySum + currentEntryQty;
                        
                        if (totalQty > 0)
                        {
                            // 保留4位小数，符合财务精度要求
                            group.UntaxedUnitPrice = Math.Round(totalCost / totalQty, 4, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            group.UntaxedUnitPrice = child.UntaxedUnitPrice;
                        }
                        
                        group.PurQtySum += currentEntryQty;

                        // 取最新出库时间（若当前时间更新，则覆盖）
                        group.LatestStorageTime = System.DateTime.Now;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                List<tb_BOM_SDetail> BOM_SDetails = new List<tb_BOM_SDetail>();
                List<tb_BOM_S> BOMs = new List<tb_BOM_S>();
                List<tb_PriceRecord> PriceRecords = new List<tb_PriceRecord>();
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();

                // 处理分组数据，更新库存记录的各字段
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;

                    #region 计算成本（反审核）
                    //直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                    //平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                    //先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。适用范围：适用于存货的实物流转比较符合先进先出的假设，比如食品、药品等有保质期限制的商品，先购进的存货会先发出销售。

                    //数据来源可以是多种多样的，例如：
                    //采购价格：从供应商处购买产品或物品时的价格。
                    //生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                    //市场价格：参考市场上类似产品或物品的价格。
                    
                    // ✅ P0修复：保存原始数量，确保成本计算使用正确的基准
                    int originalQty = inv.Quantity;
                    
                    if (group.Value.IsGift.HasValue && !group.Value.IsGift.Value && group.Value.UntaxedUnitPrice > 0)
                    {
                        // AntiCostCalculation 内部会使用 inv.Quantity（即 originalQty）进行反算
                        CommService.CostCalculations.AntiCostCalculation(_appContext, inv, group.Value.PurQtySum.ToInt(), group.Value.UntaxedUnitPrice);

                        var ctrbom = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
                        // 递归更新所有上级BOM的成本
                        await ctrbom.UpdateParentBOMsAsync(group.Key.ProdDetailID, inv.Inv_Cost);
                    }

                    #endregion

                    #region 更新采购价格

                    //注意这里的人是指采购订单录入的人。不是采购入库的人。
                    // ✅ 从预加载的 Lookup 中获取最新价格（按时间倒序，第一条即为最新）
                    tb_PriceRecord priceRecord = null;
                    if (priceRecordLookup != null)
                    {
                        var priceRecordsForProduct = priceRecordLookup[group.Key.ProdDetailID];
                        if (priceRecordsForProduct != null && priceRecordsForProduct.Any())
                        {
                            priceRecord = priceRecordsForProduct.First();
                        }
                    }
                    if (priceRecord == null)
                    {
                        priceRecord = new tb_PriceRecord();
                        priceRecord.ProdDetailID = group.Key.ProdDetailID;
                    }
                    priceRecord.Employee_ID = entity.tb_purorder.Employee_ID;
                    if (group.Value.UntaxedUnitPrice != priceRecord.PurPrice)
                    {
                        priceRecord.PurPrice = group.Value.UntaxedUnitPrice;
                        priceRecord.PurDate = System.DateTime.Now;
                        PriceRecords.Add(priceRecord);
                    }


                    #endregion

                    // 记录变动前的库存数量（在更新之前）
                    int beforeQty = originalQty;

                    // ✅ P0修复：成本计算完成后再更新数量，保持时序一致
                    // 累加数值字段
                    inv.On_the_way_Qty += group.Value.PurQtySum.ToInt();
                    inv.Quantity = originalQty - group.Value.PurQtySum.ToInt();
                    inv.LatestStorageTime = System.DateTime.Now;
                    // 计算衍生字段（如总成本）
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity; // 需确保 Inv_Cost 有值
                    invUpdateList.Add(inv);

                    // 创建反向库存流水记录
                    tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                    transaction.ProdDetailID = inv.ProdDetailID;
                    transaction.Location_ID = inv.Location_ID;
                    transaction.BizType = (int)BizType.采购入库单;
                    transaction.ReferenceId = entity.PurEntryID;
                    transaction.ReferenceNo = entity.PurEntryNo;
                    transaction.BeforeQuantity = beforeQty; // 变动前的库存数量
                    transaction.QuantityChange = -group.Value.PurQtySum.ToInt(); // 反审核减少库存
                    transaction.AfterQuantity = inv.Quantity;
                    transaction.UnitCost = inv.Inv_Cost;
                    transaction.TransactionTime = DateTime.Now;
                    transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;

                    View_ProdDetail obj = _cacheManager.GetEntity<View_ProdDetail>(inv.ProdDetailID);
                    if (obj != null)
                    {
                        transaction.Notes = $"采购入库单反审核：{entity.PurEntryNo}，产品：{obj.SKU}-{obj.CNName}";
                    }
                    else
                    {
                        transaction.Notes = $"采购入库单反审核：{entity.PurEntryNo}";
                    }

                    transactionList.Add(transaction);
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                // 【死锁优化】按 (ProdDetailID, Location_ID) 排序，确保所有事务以相同顺序访问库存资源
                invUpdateList = invUpdateList.OrderBy(i => i.ProdDetailID).ThenBy(i => i.Location_ID).ToList();

                if (invUpdateList.Count > 0)
                {
                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (Counter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，库存更新数据为0，更新失败！");
                    }

                    // 记录库存流水（带死锁重试机制）
                    tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                    await tranController.BatchRecordTransactionsWithRetry(transactionList);
                }

                if (BOM_SDetails.Count > 0)
                {
                    DbHelper<tb_BOM_SDetail> BOM_SDetaildbHelper = _appContext.GetRequiredService<DbHelper<tb_BOM_SDetail>>();
                    var BOM_SDetailCounter = await BOM_SDetaildbHelper.BaseDefaultAddElseUpdateAsync(BOM_SDetails);
                    if (BOM_SDetailCounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，配方明细成本更新数据为0，更新失败！");
                    }
                }

                if (BOMs.Count > 0)
                {
                    DbHelper<tb_BOM_S> BOM_SdbHelper = _appContext.GetRequiredService<DbHelper<tb_BOM_S>>();
                    var BOMCounter = await BOM_SdbHelper.BaseDefaultAddElseUpdateAsync(BOMs);
                    if (BOMCounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，配方主表成本更新数据为0，更新失败！");
                    }
                }

                if (PriceRecords.Count > 0)
                {
                    DbHelper<tb_PriceRecord> PriceRecorddbHelper = _appContext.GetRequiredService<DbHelper<tb_PriceRecord>>();
                    var PriceRecordCounter = await PriceRecorddbHelper.BaseDefaultAddElseUpdateAsync(PriceRecords);
                    if (PriceRecordCounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，采购价格历史记录更新数据为0，更新失败！");
                    }
                }

                if (entity.tb_purorder != null)
                {
                    #region  反审检测写回 退回

                    //处理采购订单
                    entity.tb_purorder = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                         .Includes(a => a.tb_PurEntries, b => b.tb_PurEntryDetails)
                         .Includes(t => t.tb_PurOrderDetails)
                         .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                         .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                         .Where(c => c.PurOrder_ID == entity.PurOrder_ID)
                         .Single();


                    //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
                    List<tb_PurEntryDetail> detailList = new List<tb_PurEntryDetail>();
                    foreach (var item in entity.tb_purorder.tb_PurEntries)
                    {
                        detailList.AddRange(item.tb_PurEntryDetails);
                    }

                    //分两种情况处理。
                    for (int i = 0; i < entity.tb_purorder.tb_PurOrderDetails.Count; i++)
                    {
                        //如果当前订单明细行，不存在于入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较
                        string prodName = entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.Specifications;
                        //明细中有相同的产品或物品。
                        var aa = entity.tb_purorder.tb_PurOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0 && entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID > 0)
                        {
                            #region //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            if (entity.tb_PurEntryDetails.Any(c => c.PurOrder_ChildID == 0))
                            {
                                //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加，反审失败！";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.Debug(msg);
                                }
                                return rs;
                            }
                            #endregion

                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {
                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量。";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.Debug(msg);
                                }
                                return rs;
                            }
                            else
                            {
                                var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                                && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID
                                && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                                //算出交付的数量
                                entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity -= RowQty;
                                entity.tb_purorder.tb_PurOrderDetails[i].UndeliveredQty += RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                        else
                        {
                            //一对一时
                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {

                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量。";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.Debug(msg);
                                }
                                return rs;
                            }
                            else
                            {
                                //当前行累计到交付
                                var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                                && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID).Sum(c => c.Quantity);
                                entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity -= RowQty;
                                entity.tb_purorder.tb_PurOrderDetails[i].UndeliveredQty += RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                    }


                    #endregion
                    entity.tb_purorder.TotalUndeliveredQty = entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.UndeliveredQty);
                    //更新未交数量
                    int OrderCounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder).UpdateColumns(c => c.TotalUndeliveredQty).ExecuteCommandAsync();
                    if (OrderCounter > 0)
                    {

                    }
                    //更新已交数量
                    int updatecounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrderDetail>(entity.tb_purorder.tb_PurOrderDetails).ExecuteCommandAsync();
                    if (updatecounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}明细中的已交数更新出错！");
                    }

                }



                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "被反审核";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列

                int counter = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(
                   it => new
                   {
                       it.DataStatus,
                       it.ApprovalStatus,
                       it.ApprovalResults,
                       it.ApprovalOpinions
                   }
                   ).ExecuteCommandAsync();


                //采购入库单，如果来自于采购订单，则要把入库数量累加到订单中的已交数量
                if (entity.tb_purorder != null && entity.tb_purorder.TotalQty != entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity))
                {
                    entity.tb_purorder.DataStatus = (int)DataStatus.确认;
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                }
                #region 财务反审
                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    #region 反审 反核销预付

                    var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                    //出库时，全部生成应收，账期的。就加上到期日
                    //有付款过的。就去预收中抵扣，不够的金额及状态标识出来
                    //如果收款了，则不能反审,预收的可以
                    var ARAPList = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                    .Includes(c => c.tb_FM_ReceivablePayableDetails)
                                   .Where(c => c.SourceBillId == entity.PurEntryID
                                   && c.TotalLocalPayableAmount > 0 //正向
                                   && c.SourceBizType == (int)BizType.采购入库单).ToListAsync();
                    if (ARAPList != null && ARAPList.Count > 0)
                    {
                        if (ARAPList.Count == 1)
                        {
                            var result = await ctrpayable.AntiApplyManualPaymentAllocation(ARAPList[0], ReceivePaymentType.付款, true, false);
                        }
                        else
                        {
                            //不会为多行。有错误
                            _unitOfWorkManage.RollbackTran();
                            rs.ErrorMsg = $"采购入库单{entity.PurEntryNo}有多张应付款单，数据重复，请检查数据正确性后，再操作。";
                            rs.Succeeded = false;
                            return rs;
                        }
                    }

                    #endregion

                }

                #endregion

                _unitOfWorkManage.CommitTran();

                rs.ReturnObject = entity as T;
                rs.Succeeded = true;



                return rs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rs.ErrorMsg = ex.Message;
                return rs;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            List<tb_PurEntry> list = await _appContext.Db.CopyNew().Queryable<tb_PurEntry>().Where(m => m.PurEntryID == MainID)
                             .Includes(a => a.tb_customervendor)
                                .Includes(a => a.tb_employee)
                          .Includes(a => a.tb_department)
                           .Includes(a => a.tb_PurEntryDetails, c => c.tb_location)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_PurEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                    .Includes(a => a.tb_PurEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                                 .ToListAsync();
            return list as List<T>;
        }

        /// <summary>
        /// ✅ P0修复: 采购入库后直接更新BOM的RealTimeCost(使用采购单价)
        /// </summary>
        /// <param name="prodDetailId">产品详情ID</param>
        /// <param name="purchaseUnitCost">采购不含税单价</param>
        /// <param name="bomController">BOM控制器实例</param>
        private async Task UpdateBOMRealTimeCostAfterPurchaseAsync(long prodDetailId, decimal purchaseUnitCost, tb_BOM_SController<tb_BOM_S> bomController)
        {
            try
            {
                // 1. 查询所有包含该产品的BOM明细
                var bomDetails = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_BOM_SDetail>()
                    .Where(d => d.ProdDetailID == prodDetailId)
                    .ToListAsync();

                if (bomDetails == null || bomDetails.Count == 0)
                {
                    return; // 该产品未被任何BOM引用
                }

                // 2. 直接更新RealTimeCost和SubtotalRealTimeCost
                foreach (var detail in bomDetails)
                {
                    detail.RealTimeCost = purchaseUnitCost;  // ✅ 直接使用采购单价
                    detail.SubtotalRealTimeCost = purchaseUnitCost * detail.UsedQty;
                }

                // 3. 批量更新BOM明细
                await _unitOfWorkManage.GetDbClient()
                    .Updateable(bomDetails)
                    .UpdateColumns(d => new { d.RealTimeCost, d.SubtotalRealTimeCost })
                    .ExecuteCommandAsync();

                _logger.LogInformation($"✅ 采购入库更新BOM实时成本: 产品ID={prodDetailId}, 采购单价={purchaseUnitCost:F4}, 影响{bomDetails.Count}条BOM明细");

                // 4. 递归更新上级BOM的总成本(基于新的RealTimeCost重新计算)
                var affectedBomIds = bomDetails.Select(d => d.BOM_ID).Distinct().ToList();
                foreach (var bomId in affectedBomIds)
                {
                    var parentBOM = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_BOM_S>()
                        .Where(b => b.BOM_ID == bomId)
                        .FirstAsync();

                    if (parentBOM != null)
                    {
                        // 重新计算主表汇总字段
                        var allDetails = await _unitOfWorkManage.GetDbClient()
                            .Queryable<tb_BOM_SDetail>()
                            .Where(d => d.BOM_ID == bomId)
                            .ToListAsync();

                        parentBOM.TotalMaterialCost = allDetails.Sum(d => d.SubtotalUnitCost);
                        parentBOM.SelfProductionAllCosts = parentBOM.TotalMaterialCost 
                            + parentBOM.TotalSelfManuCost 
                            + parentBOM.SelfApportionedCost;
                        parentBOM.OutProductionAllCosts = parentBOM.TotalMaterialCost 
                            + parentBOM.TotalOutManuCost 
                            + parentBOM.OutApportionedCost;

                        await _unitOfWorkManage.GetDbClient()
                            .Updateable(parentBOM)
                            .UpdateColumns(b => new { 
                                b.TotalMaterialCost, 
                                b.SelfProductionAllCosts, 
                                b.OutProductionAllCosts 
                            })
                            .ExecuteCommandAsync();

                        // 5. 递归处理上一级BOM
                        await bomController.UpdateParentBOMsAsync(parentBOM.ProdDetailID, parentBOM.SelfProductionAllCosts);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"❌ 采购入库更新BOM实时成本失败: 产品ID={prodDetailId}, 采购单价={purchaseUnitCost}");
                // 不抛出异常,避免影响采购入库主流程
            }
        }

        /// <summary>
        /// 验证采购订单数量是否超限
        /// </summary>
        private ReturnResults<T> ValidatePurOrderQuantity(tb_PurEntry entity)
        {
            var rs = new ReturnResults<T>();
            
            // 先找到所有入库明细
            List<tb_PurEntryDetail> detailList = new List<tb_PurEntryDetail>();
            foreach (var item in entity.tb_purorder.tb_PurEntries)
            {
                detailList.AddRange(item.tb_PurEntryDetails);
            }

            // 分两种情况处理
            for (int i = 0; i < entity.tb_purorder.tb_PurOrderDetails.Count; i++)
            {
                string prodName = entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.SKU + "-" + 
                                  entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.Specifications;

                // 明细中有相同的产品或物品
                var aa = entity.tb_purorder.tb_PurOrderDetails.Select(c => c.ProdDetailID).ToList()
                    .GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    
                if (aa.Count > 0 && entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID > 0)
                {
                    // 如果存在不是引用的明细,则不允许入库
                    if (entity.tb_PurEntryDetails.Any(c => c.PurOrder_ChildID == 0))
                    {
                        rs.ErrorMsg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加。";
                        rs.Succeeded = false;
                        return rs;
                    }

                    var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                        && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                        && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID)
                        .Where(c => c.IsGift.HasValue && !c.IsGift.Value)
                        .Sum(c => c.Quantity);
                        
                    if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                    {
                        rs.ErrorMsg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行数量\r\n" + 
                                      $"或当前采购订单重复录入采购入库单。";
                        rs.Succeeded = false;
                        return rs;
                    }
                }
                else
                {
                    // 一对一时
                    var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                        && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID)
                        .Where(c => c.IsGift.HasValue && !c.IsGift.Value)
                        .Sum(c => c.Quantity);
                        
                    if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                    {
                        rs.ErrorMsg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行数量\r\n" + 
                                      $"或当前采购订单重复录入了采购入库单。";
                        rs.Succeeded = false;
                        return rs;
                    }
                }
            }

            rs.Succeeded = true;
            return rs;
        }

        /// <summary>
        /// 更新采购订单明细的已入库数量和未入库数量
        /// </summary>
        private void UpdatePurOrderDeliveryQty(tb_PurEntry entity)
        {
            if (entity.tb_purorder == null) return;

            //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
            List<tb_PurEntryDetail> detailList = new List<tb_PurEntryDetail>();
            foreach (var item in entity.tb_purorder.tb_PurEntries)
            {
                detailList.AddRange(item.tb_PurEntryDetails);
            }

            //分两种情况处理。
            for (int i = 0; i < entity.tb_purorder.tb_PurOrderDetails.Count; i++)
            {
                string prodName = entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.SKU + "-" +
                                  entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.Specifications;

                //明细中有相同的产品或物品。
                var aa = entity.tb_purorder.tb_PurOrderDetails.Select(c => c.ProdDetailID).ToList()
                    .GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

                if (aa.Count > 0 && entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID > 0)
                {
                    #region 如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                    if (entity.tb_PurEntryDetails.Any(c => c.PurOrder_ChildID == 0))
                    {
                        throw new Exception($"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加。");
                    }
                    #endregion

                    var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                        && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID
                        && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                        && c.IsGift.HasValue && !c.IsGift.Value)
                        .Sum(c => c.Quantity);

                    if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                    {
                        throw new Exception($"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行数量\r\n" +
                                          $"或当前采购订单重复录入采购入库单。");
                    }
                    else
                    {
                        var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                            && c.IsGift.HasValue && !c.IsGift.Value)
                            .Sum(c => c.Quantity);
                        //算出交付的数量
                        entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity += RowQty;
                        entity.tb_purorder.tb_PurOrderDetails[i].UndeliveredQty -= RowQty;

                        //如果已交数据大于 订单数量 给出警告
                        if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                        {
                            throw new Exception($"入库单：{entity.PurEntryNo}审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，{prodName}的入库总数量不能大于订单数量！");
                        }
                    }
                }
                else
                {
                    //一对一时
                    var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                        && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                        && c.IsGift.HasValue && !c.IsGift.Value)
                        .Sum(c => c.Quantity);

                    if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                    {
                        throw new Exception($"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量，\r\n" +
                                          $"或当前采购订单重复录入了采购入库单。");
                    }
                    else
                    {
                        //当前行累计到交付
                        var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                            && c.IsGift.HasValue && !c.IsGift.Value)
                            .Sum(c => c.Quantity);
                        entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity += RowQty;
                        entity.tb_purorder.tb_PurOrderDetails[i].UndeliveredQty -= RowQty;

                        //如果已交数据大于 订单数量 给出警告
                        if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                        {
                            throw new Exception($"入库单：{entity.PurEntryNo}审核时，【{prodName}】的入库总数量不能大于订单数量！");
                        }
                    }
                }
            }

            //如果已交数据大于 订单数量 给出警告
            if (entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity) > entity.tb_purorder.TotalQty)
            {
                throw new Exception($"采购订单：{entity.tb_purorder.PurOrderNo}中，入库总交付数量不能大于订单数量！");
            }
        }

        /// <summary>
        /// 计算订单未交数量和结案状态
        /// </summary>
        private void CalculateOrderDeliveryStatus(tb_PurEntry entity)
        {
            if (entity.tb_purorder == null) return;

            entity.tb_purorder.TotalUndeliveredQty = entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.UndeliveredQty);
            var orderTotalDeliveredQty = entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity);

            if (entity.tb_purorder.DataStatus == (int)DataStatus.确认
                && (entity.TotalQty == entity.tb_purorder.TotalQty || orderTotalDeliveredQty == entity.tb_purorder.TotalQty))
            {
                entity.tb_purorder.DataStatus = (int)DataStatus.完结;
                entity.tb_purorder.CloseCaseOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + 
                    _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + 
                    "审核入库单:" + entity.PurEntryNo + "结案。";
            }
        }

        /// <summary>
        /// 预加载库存和价格记录(性能优化)
        /// </summary>
        private async Task<(Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal PurQtySum, bool? IsGift, decimal UntaxedUnitPrice, DateTime LatestStorageTime)> InventoryGroups,
                          List<tb_Inventory> InvUpdateList,
                          List<tb_InventoryTransaction> TransactionList,
                          List<tb_PriceRecord> PriceRecords,
                          List<tb_BOM_SDetail> BOMDetails,
                          List<tb_BOM_S> BOMs)>
            PreloadInventoryAndPricesAsync(tb_PurEntry entity)
        {
            // 使用字典按 (ProdDetailID, LocationID) 分组
            var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal PurQtySum, bool? IsGift, decimal UntaxedUnitPrice, DateTime LatestStorageTime)>();

            #region 【死锁优化】预处理阶段（事务外批量预加载库存）
            var prodDetailIds = entity.tb_PurEntryDetails.Select(c => c.ProdDetailID).Distinct().ToList();
            var requiredPairs = entity.tb_PurEntryDetails
                .Select(c => new { c.ProdDetailID, c.Location_ID })
                .Distinct()
                .ToHashSet();

            var inventoryList = await _unitOfWorkManage.GetDbClient()
                .Queryable<tb_Inventory>()
                .Where(i => prodDetailIds.Contains(i.ProdDetailID))
                .ToListAsync();

            inventoryList = inventoryList.Where(i => requiredPairs.Contains(new { i.ProdDetailID, i.Location_ID })).ToList();
            var invDict = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
            #endregion

            // 声明价格记录 Lookup
            ILookup<long, tb_PriceRecord> priceRecordLookup = null;

            #region 【优化】批量预加载采购价格记录，避免循环内查询（事务外）
            if (entity.tb_purorder != null && entity.tb_purorder.Employee_ID > 0 && prodDetailIds.Count > 0)
            {
                var priceRecordList = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_PriceRecord>()
                    .Where(c => c.Employee_ID == entity.tb_purorder.Employee_ID &&
                                prodDetailIds.Contains(c.ProdDetailID))
                    .OrderByDescending(c => c.PurDate)
                    .ToListAsync();

                priceRecordLookup = priceRecordList.ToLookup(p => p.ProdDetailID);
            }
            #endregion

            // 遍历入库明细，聚合数据
            foreach (var child in entity.tb_PurEntryDetails)
            {
                var key = (child.ProdDetailID, child.Location_ID);
                decimal currentEntryQty = child.Quantity;
                DateTime currentStorageTime = DateTime.Now;

                if (!inventoryGroups.TryGetValue(key, out var group))
                {
                    #region 库存表的更新
                    if (!invDict.TryGetValue(key, out var inv) || inv == null)
                    {
                        inv = new tb_Inventory
                        {
                            ProdDetailID = key.ProdDetailID,
                            Location_ID = key.Location_ID,
                            Quantity = 0,
                            Inv_Cost = 0,
                            Notes = "采购入库创建",
                            InitInventory = 0,
                            Sale_Qty = 0,
                            LatestStorageTime = DateTime.Now
                        };
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    else
                    {
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    #endregion

                    group = (
                        Inventory: inv,
                        PurQtySum: currentEntryQty,
                        IsGift: child.IsGift,
                        UntaxedUnitPrice: child.UntaxedUnitPrice,
                        LatestStorageTime: currentStorageTime
                    );
                    inventoryGroups[key] = group;
                }
                else
                {
                    // ✅ P1修复：改进加权平均单价计算，减少精度累积误差
                    group.IsGift = child.IsGift;
                    if (group.IsGift.HasValue && !group.IsGift.Value && group.UntaxedUnitPrice > 0)
                    {
                        decimal totalCost = (currentEntryQty * child.UntaxedUnitPrice) + (group.UntaxedUnitPrice * group.PurQtySum);
                        decimal totalQty = group.PurQtySum + currentEntryQty;
                        
                        if (totalQty > 0)
                        {
                            // 保留4位小数，符合财务精度要求
                            group.UntaxedUnitPrice = Math.Round(totalCost / totalQty, 4, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            group.UntaxedUnitPrice = child.UntaxedUnitPrice;
                        }
                    }
                    else
                    {
                        group.UntaxedUnitPrice = child.UntaxedUnitPrice;
                    }
                    group.PurQtySum += currentEntryQty;
                    group.LatestStorageTime = System.DateTime.Now;
                    inventoryGroups[key] = group;
                }
            }

            // 准备更新列表
            List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
            List<tb_BOM_SDetail> bomDetails = new List<tb_BOM_SDetail>();
            List<tb_BOM_S> boms = new List<tb_BOM_S>();
            List<tb_PriceRecord> priceRecords = new List<tb_PriceRecord>();
            List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();

            // 处理分组数据
            foreach (var group in inventoryGroups)
            {
                var inv = group.Value.Inventory;

                #region 计算成本
                if (group.Value.IsGift.HasValue && !group.Value.IsGift.Value && group.Value.UntaxedUnitPrice > 0)
                {
                    decimal UntaxedShippingCost = 0;
                    UntaxedShippingCost = entity.ShipCost;
                    if (entity.ShipCost > 0 && entity.TotalTaxAmount > 0)
                    {
                        decimal FreightTaxRate = entity.tb_PurEntryDetails.FirstOrDefault(c => c.TaxRate > 0).TaxRate;
                        UntaxedShippingCost = (entity.ShipCost / (1 + FreightTaxRate));
                        UntaxedShippingCost = Math.Round(UntaxedShippingCost, 2);
                    }
                    CommService.CostCalculations.CostCalculation(_appContext, inv, group.Value.PurQtySum.ToInt(), group.Value.UntaxedUnitPrice, UntaxedShippingCost);

                    // 更新BOM实时成本
                    var ctrbom = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
                    await UpdateBOMRealTimeCostAfterPurchaseAsync(group.Key.ProdDetailID, group.Value.UntaxedUnitPrice, ctrbom);
                }
                #endregion

                #region 更新采购价格记录表
                tb_PriceRecord priceRecord = null;
                if (priceRecordLookup != null)
                {
                    var priceRecordsForProduct = priceRecordLookup[group.Key.ProdDetailID];
                    if (priceRecordsForProduct != null && priceRecordsForProduct.Any())
                    {
                        priceRecord = priceRecordsForProduct.First();
                    }
                }
                if (priceRecord == null)
                {
                    priceRecord = new tb_PriceRecord();
                    priceRecord.ProdDetailID = group.Key.ProdDetailID;
                }
                priceRecord.Employee_ID = entity.tb_purorder?.Employee_ID ?? 0;
                if (group.Value.UntaxedUnitPrice != priceRecord.PurPrice)
                {
                    priceRecord.PurPrice = group.Value.UntaxedUnitPrice;
                    priceRecord.PurDate = System.DateTime.Now;
                    priceRecords.Add(priceRecord);
                }
                #endregion

                // 记录变动前的库存数量
                int beforeQty = inv.Quantity;

                // 累加数值字段
                inv.On_the_way_Qty -= group.Value.PurQtySum.ToInt();
                inv.Quantity += group.Value.PurQtySum.ToInt();
                inv.LatestStorageTime = System.DateTime.Now;
                inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                invUpdateList.Add(inv);

                // 创建库存流水记录
                tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                transaction.ProdDetailID = inv.ProdDetailID;
                transaction.Location_ID = inv.Location_ID;
                transaction.BizType = (int)BizType.采购入库单;
                transaction.ReferenceId = entity.PurEntryID;
                transaction.ReferenceNo = entity.PurEntryNo;
                transaction.BeforeQuantity = beforeQty;
                transaction.QuantityChange = group.Value.PurQtySum.ToInt();
                transaction.AfterQuantity = inv.Quantity;
                transaction.UnitCost = inv.Inv_Cost;
                transaction.TransactionTime = DateTime.Now;
                transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                
                View_ProdDetail obj = _cacheManager.GetEntity<View_ProdDetail>(inv.ProdDetailID);
                if (obj != null)
                {
                    transaction.Notes = $"采购入库单审核：{entity.PurEntryNo}，产品：{obj.SKU}-{obj.CNName}";
                }
                else
                {
                    transaction.Notes = $"采购入库单审核：{entity.PurEntryNo}";
                }
                transactionList.Add(transaction);
            }

            // 【死锁优化】按 (ProdDetailID, Location_ID) 排序
            invUpdateList = invUpdateList.OrderBy(i => i.ProdDetailID).ThenBy(i => i.Location_ID).ToList();

            return (inventoryGroups, invUpdateList, transactionList, priceRecords, bomDetails, boms);
        }

        /// <summary>
        /// 财务独立事务处理 - 生成应付账款（含补偿机制）
        /// </summary>
        private async Task<ReturnResults<object>> ProcessFinanceAfterApprovalAsync(tb_PurEntry entity)
        {
            var result = new ReturnResults<object>();
            tb_FM_ReceivablePayable savedPayable = null;
            bool payableSaved = false;

            try
            {
                var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                tb_FM_ReceivablePayable payable = await ctrpayable.BuildReceivablePayable(entity, false);
                ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(payable, false);

                if (rmr.Succeeded)
                {
                    savedPayable = rmr.ReturnObject;
                    payableSaved = true;
                    _logger.LogInformation($"采购入库单{entity.PurEntryNo}：应付账款 {savedPayable?.ARAPNo} 生成成功");

                    // 自动审核（如果配置启用）
                    if (_appContext.FMConfig?.AutoAuditPaymentable == true)
                    {
                        savedPayable.ApprovalOpinions = "自动审核";
                        var autoApproval = await ctrpayable.ApprovalAsync(savedPayable, true);
                        if (!autoApproval.Succeeded)
                        {
                            // 自动审核失败，触发补偿删除
                            await CompensatePayableAsync(savedPayable?.ARAPId, entity.PurEntryNo);
                            result.ErrorMsg = $"自动审核失败：{autoApproval.ErrorMsg}";
                            result.Succeeded = false;
                            _logger.LogWarning($"采购入库单{entity.PurEntryNo}：自动审核失败，已触发补偿机制");
                            return result;
                        }
                        _logger.LogInformation($"采购入库单{entity.PurEntryNo}：应付账款自动审核成功");
                    }

                    result.ReturnObject = savedPayable;
                    result.Succeeded = true;
                    _logger.LogInformation($"采购入库单{entity.PurEntryNo}财务处理成功");
                }
                else
                {
                    result.ErrorMsg = $"应付账款生成失败：{rmr.ErrorMsg}";
                    result.Succeeded = false;
                    _logger.LogWarning($"采购入库单{entity.PurEntryNo}财务处理失败：{rmr.ErrorMsg}");
                }
            }
            catch (Exception ex)
            {
                // 发生异常时，如果已保存应付账款，触发补偿删除
                if (payableSaved)
                {
                    await CompensatePayableAsync(savedPayable?.ARAPId, entity.PurEntryNo);
                }
                result.ErrorMsg = $"财务数据处理异常：{ex.Message}";
                result.Succeeded = false;
                _logger.Error(ex, $"采购入库单{entity.PurEntryNo}财务处理异常，已触发补偿机制");
            }

            return result;
        }

        /// <summary>
        /// 补偿机制：删除已创建的应付账款
        /// </summary>
        private async Task CompensatePayableAsync(long? arapId, string purEntryNo)
        {
            if (!arapId.HasValue)
            {
                _logger.LogWarning($"采购入库单{purEntryNo}：应付账款ID无效，无需补偿");
                return;
            }

            try
            {
                var payable = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_FM_ReceivablePayable>()
                    .Where(c => c.ARAPId == arapId)
                    .FirstAsync();

                if (payable == null)
                {
                    _logger.LogInformation($"采购入库单{purEntryNo}：应付账款 {arapId} 不存在，无需补偿");
                    return;
                }

                // 检查是否已被核销
                if (payable.LocalPaidAmount > 0 || payable.ForeignPaidAmount > 0)
                {
                    _logger.LogError($"采购入库单{purEntryNo}：应付账款 {arapId} 已被核销，无法补偿删除");
                    return;
                }

                // 删除应付账款
                var deletedCount = await _unitOfWorkManage.GetDbClient()
                    .Deleteable<tb_FM_ReceivablePayable>()
                    .Where(c => c.ARAPId == arapId)
                    .ExecuteCommandAsync();

                if (deletedCount > 0)
                {
                    _logger.LogInformation($"采购入库单{purEntryNo}：应付账款 {arapId} 补偿删除成功");
                }
                else
                {
                    _logger.LogWarning($"采购入库单{purEntryNo}：应付账款 {arapId} 补偿删除未找到记录");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"采购入库单{purEntryNo}：应付账款 {arapId} 补偿删除失败");
            }
        }





    }
}



