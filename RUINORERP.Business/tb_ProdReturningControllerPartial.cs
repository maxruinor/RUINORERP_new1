
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
using RUINORERP.Business.CommService;
using System.Collections;
using static StackExchange.Redis.Role;
using System.Text;
using RUINORERP.Business.BizMapperService;
using System.Threading;
using RUINORERP.Business.EntityLoadService;

namespace RUINORERP.Business
{
    /// <summary>
    ///  
    /// </summary>
    public partial class tb_ProdReturningController<T> : BaseController<T> where T : class
    {


        /// <summary>
        /// 审核归还单，库存增加，借出单也要处理 ，
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_ProdReturning entity = ObjectEntity as tb_ProdReturning;
            ReturnResults<T> rs = new ReturnResults<T>();

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                // entity.ApprovalResults = approvalEntity.ApprovalResults;

                #region 审核 通过时
                if (entity.ApprovalResults.Value)
                {

                    entity.tb_prodborrowing = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowing>()
                        .Includes(a => a.tb_ProdReturnings, b => b.tb_ProdReturningDetails)
                        .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                      .Includes(a => a.tb_ProdBorrowingDetails, b => b.tb_proddetail, c => c.tb_prod)
                        .Where(c => c.BorrowID == entity.BorrowID).Single();


                    //如果归还的明细中的产品。不存在于借出单中。审核失败。
                    foreach (var child in entity.tb_ProdReturningDetails)
                    {
                        if (!entity.tb_prodborrowing.tb_ProdBorrowingDetails.Any(c => c.ProdDetailID == child.ProdDetailID))
                        {
                            rs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rs.ErrorMsg = $"归还明细中有产品不属于对应的借出单!请检查数据后重试！";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.Debug(rs.ErrorMsg);
                            }

                            return rs;
                        }
                    }


                    #region 回写借出的状态及明细数据
                    //先找到所有眼还明细,再找按借出明细去循环比较。如果眼还总数量大于借出订单数量，则不允许审核。
                    List<tb_ProdReturningDetail> detailList = new List<tb_ProdReturningDetail>();
                    foreach (var item in entity.tb_prodborrowing.tb_ProdReturnings)
                    {
                        detailList.AddRange(item.tb_ProdReturningDetails);
                    }

                    for (int i = 0; i < entity.tb_prodborrowing.tb_ProdBorrowingDetails.Count; i++)
                    {
                        //如果当前归还明细行，不存在于借出明细。直接跳过
                        string prodName = entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].tb_proddetail.tb_prod.Specifications;

                        //一对一时，找到所有的出库明细数量总和
                        var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].ProdDetailID
                        && c.Location_ID == entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].Location_ID
                        ).Sum(c => c.Qty);
                        if (inQty > entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].Qty)
                        {

                            string msg = $"归还单:{entity.tb_prodborrowing.BorrowNo}的【{prodName}】的归还数量不能大于借出中对应行的数量，\r\n\" " +
                                $"或存在当前借出单重复录入了归还单。";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.Debug(msg);
                            return rs;
                        }
                        else
                        {
                            //当前行累计到交付，只能是当前行所以重新找到当前出库单明细的的数量
                            var RowQty = entity.tb_ProdReturningDetails.Where(c => c.ProdDetailID == entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].ProdDetailID && c.Location_ID == entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].Location_ID).Sum(c => c.Qty);
                            entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].ReQty += RowQty; //可以分多次还，所以累加

                            //如果已交数据大于 归还数量 给出警告实际操作中 
                            if (entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].ReQty > entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].Qty)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"归还单：{entity.ReturnNo}审核时，【{prodName}】的归还总数不能大于借出时数量！");
                            }
                        }

                    }

                    //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                    if (entity.tb_prodborrowing.tb_ProdBorrowingDetails.Sum(c => c.ReQty) > entity.tb_prodborrowing.TotalQty)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception($"归还单：{entity.ReturnNo}审核时，归还总数不能大于借出时数量！");
                    }

                    //更新已交数量
                    int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdBorrowingDetail>(entity.tb_prodborrowing.tb_ProdBorrowingDetails).ExecuteCommandAsync();

                    #endregion


                    //归还的等于借出的，数量够则自动结案
                    if (entity.tb_prodborrowing != null && entity.tb_prodborrowing.DataStatus == (int)DataStatus.确认 &&
                        entity.tb_prodborrowing.tb_ProdBorrowingDetails.Sum(c => c.ReQty) == entity.tb_prodborrowing.tb_ProdBorrowingDetails
                        .Sum(c => c.Qty))
                    {
                        entity.tb_prodborrowing.DataStatus = (int)DataStatus.完结;
                        entity.tb_prodborrowing.CloseCaseOpinions = "【归还单审核时，借出单系统自动结案】-" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name;
                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_prodborrowing).UpdateColumns(t => new { t.DataStatus, t.CloseCaseOpinions }).ExecuteCommandAsync();
                    }

                }
                #endregion

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                // 创建库存流水记录列表
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                
                foreach (var child in entity.tb_ProdReturningDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //标记是否有期初

                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv != null)
                    {
                        //更新库存
                        inv.Quantity = inv.Quantity + child.Qty;
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    else
                    {
                        inv = new tb_Inventory();
                        inv.Quantity = inv.Quantity + child.Qty;
                        inv.InitInventory = (int)inv.Quantity;
                        inv.Location_ID = child.Location_ID;
                        inv.ProdDetailID = child.ProdDetailID;
                        inv.Notes = "";//后面修改数据库是不需要？
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    /*
                  直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                 平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                 先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                 后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                 数据来源可以是多种多样的，例如：
                 采购价格：从供应商处购买产品或物品时的价格。
                 生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                 市场价格：参考市场上类似产品或物品的价格。
                  */
                    //inv.Inv_Cost = child.Cost;//这里需要计算，根据系统设置中的算法计算。
                    //inv.CostFIFO = child.Cost;
                    //inv.CostMonthlyWA = child.Cost;
                    //inv.CostMovingWA = child.Cost;
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;

                    #endregion
                    invUpdateList.Add(inv);
                    
                    // 实时获取当前库存成本
                    decimal realtimeCost = inv.Inv_Cost;
                    
                    // 更新归还明细的成本为实时成本
                    child.Cost = realtimeCost;
                    child.SubtotalCostAmount = realtimeCost * child.Qty;
                    
                    // 创建库存流水记录
                    tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                    transaction.ProdDetailID = inv.ProdDetailID;
                    transaction.Location_ID = inv.Location_ID;
                    transaction.BizType = (int)BizType.归还单;
                    transaction.ReferenceId = entity.ReturnID;
                    transaction.QuantityChange = child.Qty; // 产品归还增加库存
                    transaction.AfterQuantity = inv.Quantity;
                    transaction.UnitCost = realtimeCost; // 使用实时成本
                    transaction.TransactionTime = DateTime.Now;
                    transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    transaction.Notes = $"产品归还单审核：{entity.ReturnNo}，产品：{inv.tb_proddetail?.tb_prod?.CNName}";

                    transactionList.Add(transaction);
                }
                
                // 记录库存流水
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactions(transactionList);

                // 使用LINQ查询
                var CheckNewInvList = invUpdateList.Where(c => c.Inventory_ID == 0)
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

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (Counter.ToInt() == 0)
                {
                    _logger.Debug($"{entity.ReturnNo}更新库存结果为0行，请检查数据！");
                }

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
            tb_ProdReturning entity = ObjectEntity as tb_ProdReturning;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();



                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = null;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;

                BusinessHelper.Instance.ApproverEntity(entity);

                #region 审核 通过时

                entity.tb_prodborrowing = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowing>()
                    .Includes(a => a.tb_ProdReturnings, b => b.tb_ProdReturningDetails)
                    .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                  .Includes(a => a.tb_ProdBorrowingDetails, b => b.tb_proddetail, c => c.tb_prod)
                    .Where(c => c.BorrowID == entity.BorrowID).Single();

                #region 回写借出的状态及明细数据
                //先找到所有眼还明细,再找按借出明细去循环比较。如果眼还总数量大于借出订单数量，则不允许审核。
                List<tb_ProdReturningDetail> detailList = new List<tb_ProdReturningDetail>();
                foreach (var item in entity.tb_prodborrowing.tb_ProdReturnings)
                {
                    detailList.AddRange(item.tb_ProdReturningDetails);
                }

                for (int i = 0; i < entity.tb_prodborrowing.tb_ProdBorrowingDetails.Count; i++)
                {
                    //如果当前归还明细行，不存在于借出明细。直接跳过
                    string prodName = entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].tb_proddetail.tb_prod.CNName +
                              entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].tb_proddetail.tb_prod.Specifications;

                    //一对一时，找到所有的出库明细数量总和
                    var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].ProdDetailID
                    && c.Location_ID == entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].Location_ID).Sum(c => c.Qty);
                    if (inQty < 0)
                    {
                        string msg = $"归还单:{entity.tb_prodborrowing.BorrowNo}的【{prodName}】的归还数量不能于小于零，\r\n\" " +
                            "反审失败！";
                        rs.ErrorMsg = msg;
                        _unitOfWorkManage.RollbackTran();
                        _logger.Debug(msg);
                        return rs;
                    }
                    else
                    {
                        //当前行累计到交付，只能是当前行所以重新找到当前出库单明细的的数量
                        var RowQty = entity.tb_ProdReturningDetails.Where(c => c.ProdDetailID == entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].ProdDetailID
                        && c.Location_ID == entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].Location_ID).Sum(c => c.Qty);
                        entity.tb_prodborrowing.tb_ProdBorrowingDetails[i].ReQty -= RowQty; //可以分多次还，所以累加
                    }

                }

                //更新已交数量
                int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdBorrowingDetail>
                    (entity.tb_prodborrowing.tb_ProdBorrowingDetails).ExecuteCommandAsync();

                #endregion

                //归还的等于借出的，数量够则自动结案
                if (entity.tb_prodborrowing != null && entity.tb_prodborrowing.tb_ProdBorrowingDetails.Sum(c => c.ReQty) != entity.tb_prodborrowing.tb_ProdBorrowingDetails.Sum(c => c.Qty))
                {
                    entity.tb_prodborrowing.DataStatus = (int)DataStatus.确认;
                    entity.tb_prodborrowing.CloseCaseOpinions = "【系统自动反结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "反审核归还单时:" + entity.ReturnNo + "反结案。"; ;
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_prodborrowing).UpdateColumns(t => new { t.DataStatus, t.CloseCaseOpinions }).ExecuteCommandAsync();
                }


                #endregion

                // 创建反向库存流水记录列表
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                
                foreach (var child in entity.tb_ProdReturningDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //标记是否有期初
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv != null)
                    {
                        //更新库存
                        inv.Quantity = inv.Quantity - child.Qty;
                        BusinessHelper.Instance.EditEntity(inv);
                    }

                    /*
                  直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                 平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                 先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                 后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                 数据来源可以是多种多样的，例如：
                 采购价格：从供应商处购买产品或物品时的价格。
                 生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                 市场价格：参考市场上类似产品或物品的价格。
                  */
                    //inv.Inv_Cost = child.Cost;//这里需要计算，根据系统设置中的算法计算。
                    //inv.CostFIFO = child.Cost;
                    //inv.CostMonthlyWA = child.Cost;
                    //inv.CostMovingWA = child.Cost;
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        // 实时获取当前库存成本
                        decimal realtimeCost = inv.Inv_Cost;
                        
                        // 创建反向库存流水记录
                        tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                        transaction.ProdDetailID = inv.ProdDetailID;
                        transaction.Location_ID = inv.Location_ID;
                        transaction.BizType = (int)BizType.归还单;
                        transaction.ReferenceId = entity.ReturnID;
                        transaction.QuantityChange = -child.Qty; // 反审核减少库存
                        transaction.AfterQuantity = inv.Quantity;
                        transaction.UnitCost = realtimeCost; // 使用实时成本
                        transaction.TransactionTime = DateTime.Now;
                        transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                        transaction.Notes = $"产品归还单反审核：{entity.ReturnNo}，产品：{inv.tb_proddetail?.tb_prod?.CNName}";

                        transactionList.Add(transaction);
                    }
                }
                
                // 记录反向库存流水
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactions(transactionList);

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
                rs.Succeeded = false;
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }

        }



        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            // var queryable = _appContext.Db.Queryable<tb_SaleOutDetail>();
            // var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();

            List<tb_ProdReturning> list = await _appContext.Db.CopyNew().Queryable<tb_ProdReturning>().Where(m => m.ReturnID == ID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_ProdReturningDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }

        /// <summary>
        /// 转换为销售退库单
        /// </summary>
        /// <param name="prodBorrowing"></param>
        public  async Task<tb_ProdReturning> BuildProdReturningFromBorrow(tb_ProdBorrowing prodBorrowing)
        {
            tb_ProdReturning entity = new tb_ProdReturning();
            //转单
            if (prodBorrowing != null)
            {
                entity = mapper.Map<tb_ProdReturning>(prodBorrowing);
                entity.ApprovalOpinions = "快捷转单";
                entity.ApprovalResults = null;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.BorrowID = prodBorrowing.BorrowID;
                entity.BorrowNO = prodBorrowing.BorrowNo;

                List<string> tipsMsg = new List<string>();
                List<tb_ProdReturningDetail> details = mapper.Map<List<tb_ProdReturningDetail>>(prodBorrowing.tb_ProdBorrowingDetails);
                List<tb_ProdReturningDetail> NewDetails = new List<tb_ProdReturningDetail>();

                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    #region 每行产品ID唯一

                    tb_ProdBorrowingDetail item = prodBorrowing.tb_ProdBorrowingDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    details[i].Qty = item.Qty - item.ReQty;// 已经交数量去掉
                    details[i].SubtotalPirceAmount = details[i].Price * details[i].Qty;
                    details[i].SubtotalCostAmount = details[i].Cost * details[i].Qty;
                    if (details[i].Qty > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"借出单{prodBorrowing.BorrowNo}，{item.tb_proddetail.tb_prod.CNName}已归还数为{item.ReQty}，可归还数为{details[i].Qty}，当前行数据忽略！");
                    }
                    #endregion
                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"订单:{prodBorrowing.BorrowNo}已全部归还，请检查是否正在重复归还！");
                }

                StringBuilder msg = new StringBuilder();
                foreach (var item in tipsMsg)
                {
                    msg.Append(item).Append("\r\n");
                }
                if (tipsMsg.Count > 0)
                {
                    MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                entity.tb_ProdReturningDetails = NewDetails;
                entity.TotalAmount = NewDetails.Sum(c => c.SubtotalPirceAmount);
                entity.TotalCost = NewDetails.Sum(c => c.SubtotalCostAmount);
                entity.TotalQty = NewDetails.Sum(c => c.Qty);
                entity.ReturnDate = System.DateTime.Now;
                entity.ActionStatus = ActionStatus.新增;
                BusinessHelper.Instance.InitEntity(entity);
                entity.CustomerVendor_ID = prodBorrowing.CustomerVendor_ID;
                IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
                entity.ReturnNo = await bizCodeService.GenerateBizBillNoAsync(BizType.归还单, CancellationToken.None);
                entity.tb_prodborrowing = prodBorrowing;

            }
            return entity;
        }

    }
}



