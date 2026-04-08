
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
using RUINORERP.Business.CommService;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.EntityLoadService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 入库单 非生产领料/退料
    /// </summary>
    public partial class tb_StockInController<T> : BaseController<T> where T : class
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rs = new ReturnResults<T>();
            tb_StockIn entity = ObjectEntity as tb_StockIn;
            try
            {
                #region 【死锁优化】第一步：预处理阶段（事务外批量预加载库存）
                var requiredKeys = entity.tb_StockInDetails
                    .Select(c => new { c.ProdDetailID, c.Location_ID })
                    .Distinct()
                    .ToList();

                var inventoryList = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_Inventory>()
                    .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                    .ToListAsync();

                var invDict = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                #endregion

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_StockInDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //标记是否有期初

                    // ✅ 从预加载字典获取（死锁优化）
                    if (!invDict.TryGetValue((child.ProdDetailID, child.Location_ID), out var inv) || inv == null)
                    {
                        inv = new tb_Inventory();
                        inv.Quantity = child.Qty;
                        inv.InitInventory = 0;
                        inv.Location_ID = child.Location_ID;
                        inv.ProdDetailID = child.ProdDetailID;
                        inv.Notes = "其他入库初始化";
                        BusinessHelper.Instance.InitEntity(inv);
                        invDict[(child.ProdDetailID, child.Location_ID)] = inv;
                    }
                    else
                    {
                        //更新库存
                        inv.Quantity = inv.Quantity + child.Qty;
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    
                    inv.Rack_ID = child.Rack_ID;
                    inv.LatestStorageTime = System.DateTime.Now;

                    #endregion
                    invUpdateList.Add(inv);
                }

                List<tb_Inventory> InsertList = invUpdateList.Where(c => c.Inventory_ID == 0).ToList();
                if (invUpdateList.Count > 0)
                {
                    // 使用LINQ查询
                    var CheckNewInvList = InsertList.Where(c => c.Inventory_ID == 0)
                        .GroupBy(i => new { i.ProdDetailID, i.Location_ID })
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key.ProdDetailID)
                        .ToList();

                    if (CheckNewInvList.Count > 0)
                    {
                        //新增库存中有重复的商品，操作失败。请联系管理员。
                        rs.ErrorMsg = "新增库存中有重复的商品，操作失败。";
                        rs.Succeeded = false;
                        _logger.LogError(rs.ErrorMsg + "详细信息：" + string.Join(",", CheckNewInvList));
                        return rs;
                    }
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                // 创建库存流水记录列表
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                
                foreach (var inv in invUpdateList)
                {
                    // 找到对应的明细，获取数量变化
                    var child = entity.tb_StockInDetails.FirstOrDefault(c => c.ProdDetailID == inv.ProdDetailID && c.Location_ID == inv.Location_ID);
                    if (child != null)
                    {
                        // 创建库存流水记录
                        tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                        transaction.ProdDetailID = inv.ProdDetailID;
                        transaction.Location_ID = inv.Location_ID;
                        transaction.BizType = (int)BizType.其他入库单;
                        transaction.ReferenceId = entity.MainID;
                        transaction.ReferenceNo = entity.BillNo;
                        transaction.BeforeQuantity = inv.Quantity - child.Qty; // 变动前的库存数量
                        transaction.QuantityChange = child.Qty; // 库存入库增加库存
                        transaction.AfterQuantity = inv.Quantity;
                        transaction.UnitCost = inv.Inv_Cost;
                        transaction.TransactionTime = DateTime.Now;
                        transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                        transaction.Notes = $"库存入库单审核：{entity.BillNo}，产品：{inv.tb_proddetail?.tb_prod?.CNName}";

                        transactionList.Add(transaction);
                    }
                }

                DbHelper<tb_Inventory> InvdbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await InvdbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (Counter == 0)
                {
                    _logger.Debug($"{entity.BillNo}审核时，更新库存结果为0行，请检查数据！");
                }

                // 记录库存流水
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactionsWithRetry(transactionList);

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                // entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                BusinessHelper.Instance.ApproverEntity(entity);
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                              .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                              .ExecuteCommandHasChangeAsync();
 
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rs.Succeeded = false;
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }

        }



        /// <summary>
        ///其他入库单反审会将数量减少
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rs = new ReturnResults<T>();
            tb_StockIn entity = ObjectEntity as tb_StockIn;
            try
            {
                #region 【死锁优化】第一步：预处理阶段（事务外批量预加载库存）
                var requiredKeys = entity.tb_StockInDetails
                    .Select(c => new { c.ProdDetailID, c.Location_ID })
                    .Distinct()
                    .ToList();

                var inventoryList = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_Inventory>()
                    .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                    .ToListAsync();

                var invDict = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                #endregion

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                if (entity == null)
                {
                    return rs;
                }
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_StockInDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //标记是否有期初
                    // ✅ 从预加载字典获取（死锁优化）
                    if (!invDict.TryGetValue((child.ProdDetailID, child.Location_ID), out var inv) || inv == null)
                    {
                        inv = new tb_Inventory();
                        inv.Quantity = 0;
                        inv.ProdDetailID = child.ProdDetailID;
                        inv.Location_ID = child.Location_ID;
                        BusinessHelper.Instance.InitEntity(inv);
                        invDict[(child.ProdDetailID, child.Location_ID)] = inv;
                    }
                    else
                    {
                        //更新库存
                        inv.Quantity = inv.Quantity - child.Qty;
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Rack_ID = child.Rack_ID;
                    inv.LatestStorageTime = System.DateTime.Now;
                    #endregion
                    invUpdateList.Add(inv);
                }
                
                // 创建反向库存流水记录列表
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                
                foreach (var inv in invUpdateList)
                {
                    // 找到对应的明细，获取数量变化
                    var child = entity.tb_StockInDetails.FirstOrDefault(c => c.ProdDetailID == inv.ProdDetailID && c.Location_ID == inv.Location_ID);
                    if (child != null)
                    {
                        // 创建反向库存流水记录
                        tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                        transaction.ProdDetailID = inv.ProdDetailID;
                        transaction.Location_ID = inv.Location_ID;
                        transaction.BizType = (int)BizType.其他入库单;
                        transaction.ReferenceId = entity.MainID;
                        transaction.ReferenceNo = entity.BillNo;
                        transaction.BeforeQuantity = inv.Quantity + child.Qty; // 变动前的库存数量
                        transaction.QuantityChange = -child.Qty; // 反审核减少库存
                        transaction.AfterQuantity = inv.Quantity;
                        transaction.UnitCost = inv.Inv_Cost;
                        transaction.TransactionTime = DateTime.Now;
                        transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                        transaction.Notes = $"库存入库单反审核：{entity.BillNo}，产品：{inv.tb_proddetail?.tb_prod?.CNName}";

                        transactionList.Add(transaction);
                    }
                }

                DbHelper<tb_Inventory> InvdbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await InvdbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (Counter == 0)
                {
                    _logger.Debug($"{entity.BillNo}反审核时，更新库存结果为0行，请检查数据！");
                }

                // 记录库存流水（带死锁重试机制）
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactionsWithRetry(transactionList);

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = null;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;

                BusinessHelper.Instance.ApproverEntity(entity);
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                               .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                               .ExecuteCommandHasChangeAsync();


                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rs.Succeeded = false;
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }

        }



        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_StockIn> list = await _appContext.Db.CopyNew().Queryable<tb_StockIn>().Where(m => m.MainID == ID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_outinstocktype)//要加上。区别打印出来
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_StockInDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }



    }
}



