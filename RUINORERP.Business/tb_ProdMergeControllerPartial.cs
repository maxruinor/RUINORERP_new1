
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
using RUINORERP.Business.CommService;
using System.Collections;
using RUINORERP.Business.BizMapperService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 入库单 非生产领料/退料
    /// </summary>
    public partial class tb_ProdMergeController<T> : BaseController<T> where T : class
    {


        /// <summary>
        /// 组合单审核  母件增加，子件减少
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>


        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_ProdMerge entity = ObjectEntity as tb_ProdMerge;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                //增加母件
                tb_Inventory invMother = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entity.ProdDetailID && i.Location_ID == entity.Location_ID);
                if (invMother == null)
                {
                    _unitOfWorkManage.RollbackTran();
                    rs.ErrorMsg = $"母件首次增加库存必须通过【采购入库】，【期初盘点】或【缴库记录】产生过库存记录。";
                    rs.Succeeded = false;
                    return rs;

                }
                else
                {
                    //如果母件 成本为零 则将子件的加总赋值。实际少了加工费这些。缴款才是合理的入库成本变更的方式。这里暂时应急
                    if (invMother.Inv_Cost == 0)
                    {
                        invMother.Inv_Cost = entity.tb_ProdMergeDetails.Sum(c => c.UnitCost);
                    }
                    //更新库存
                    invMother.Quantity = invMother.Quantity + entity.MergeTargetQty;
                    invMother.LatestStorageTime = DateTime.Now;
                    BusinessHelper.Instance.EditEntity(invMother);
                }
                int InvInsertCounter = await _unitOfWorkManage.GetDbClient().Updateable(invMother).ExecuteCommandAsync();
                if (InvInsertCounter > 0)
                {
                    #region 子件减少
                    List<tb_Inventory> invUpdateList = new List<tb_Inventory>();

                    foreach (var child in entity.tb_ProdMergeDetails)
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，

                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv != null)
                        {
                            if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - child.Qty) < 0)
                            {
                                if (child.tb_proddetail != null)
                                {
                                    rs.ErrorMsg = $"{child.tb_proddetail.SKU}库存为：{inv.Quantity}，组合消耗量为：{child.Qty}\r\n 系统设置不允许负库存， 请检查消耗数量与库存相关数据";
                                }
                                else
                                {
                                    rs.ErrorMsg = $"库存为：{inv.Quantity}，组合消耗量为：{child.Qty}\r\n 系统设置不允许负库存， 请检查消耗数量与库存相关数据";
                                }
                                _unitOfWorkManage.RollbackTran();
                                rs.Succeeded = false;
                                return rs;
                            }
                            //更新库存
                            inv.Quantity = inv.Quantity - child.Qty;
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        else
                        {
                            if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - child.Qty) < 0)
                            {
                                rs.ErrorMsg = $"当前子件{child.tb_proddetail.SKU},在对应仓库中没有库存数据。请检查数据。组合消耗量为：{child.Qty}\r\n 系统设置不允许负库存， 请检查消耗数量与库存相关数据";
                                _unitOfWorkManage.RollbackTran();
                                rs.Succeeded = false;
                                return rs;
                            }

                            inv = new tb_Inventory();
                            inv.Quantity = inv.Quantity - child.Qty;
                            inv.InitInventory = (int)inv.Quantity;
                            inv.Location_ID = child.Location_ID;
                            inv.ProdDetailID = child.ProdDetailID;
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
                        //计算成本切割时没有指定，则保持之前的。如果后面优化要子件可以指定成本时，这里也可以参与计算
                        //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                        // 、、inv.CostFIFO = child.Cost;
                        //、 inv.CostMonthlyWA = child.Cost;
                        // inv.CostMovingWA = child.Cost;
                        inv.LatestStorageTime = System.DateTime.Now;

                        #endregion
                        invUpdateList.Add(inv);

                    }


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
                    if (Counter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("子件库存更新失败！");
                    }

                    //这部分是否能提出到上一级公共部分？
                    entity.DataStatus = (int)DataStatus.确认;
                    // entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                    //后面已经修改为
                    ///  entity.ApprovalResults = approvalEntity.ApprovalResults;
                    entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                    BusinessHelper.Instance.ApproverEntity(entity);
                    var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                             .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                             .ExecuteCommandHasChangeAsync();
                    #endregion
                }



                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
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
        ///组合单反审  母件减少，子件增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_ProdMerge entity = ObjectEntity as tb_ProdMerge;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //母件减少
                tb_Inventory invMother = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entity.ProdDetailID && i.Location_ID == entity.Location_ID);
                if (invMother != null)
                {
                    //更新库存
                    invMother.Quantity = invMother.Quantity - entity.MergeTargetQty;
                    invMother.LatestOutboundTime = DateTime.Now;
                    BusinessHelper.Instance.EditEntity(invMother);
                }
                else
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("系统对应的仓库中没有母件库存,请检查数据！ ");
                }
                int InvInsertCounter = await _unitOfWorkManage.GetDbClient().Updateable(invMother).ExecuteCommandAsync();
                if (InvInsertCounter > 0)
                {
                    //子件增加
                    List<tb_Inventory> invUpdateList = new List<tb_Inventory>();

                    foreach (var child in entity.tb_ProdMergeDetails)
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
                        // inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                        inv.LatestStorageTime = System.DateTime.Now;
                        #endregion
                        invUpdateList.Add(inv);
                    }
                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (Counter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("子件库存更新失败！");
                    }
                }

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
                rs.Succeeded = false;
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }
        }

        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_ProdMerge> list = await _appContext.Db.CopyNew().Queryable<tb_ProdMerge>().Where(m => m.MergeID == ID)
                             .Includes(a => a.tb_bom_s)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_ProdMergeDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }



    }
}



