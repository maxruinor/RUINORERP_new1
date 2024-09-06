
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
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINORERP.Global;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 入库单 非生产领料/退料
    /// </summary>
    public partial class tb_ProdSplitController<T> : BaseController<T> where T : class
    {


        /// <summary>
        /// 审核 母件减少，子件增加
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_ProdSplit entity = ObjectEntity as tb_ProdSplit;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();


                //减少母件
                tb_Inventory invMother = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entity.ProdDetailID && i.Location_ID == entity.Location_ID);
                if (invMother != null)
                {
                    if (!_appContext.SysConfig.CheckNegativeInventory && (invMother.Quantity - entity.SplitParentQty) < 0)
                    {

                        // rrs.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                        rs.ErrorMsg = $"产品拆分时，母件库存为：{invMother.Quantity}，拆分量为：{entity.SplitParentQty}\r\n 系统设置不允许负库存， 请检查母件数量与库存相关数据";
                        _unitOfWorkManage.RollbackTran();
                        rs.Succeeded = false;
                        return rs;
                    }
                    //更新库存
                    invMother.Quantity = invMother.Quantity - entity.SplitParentQty;
                    invMother.LatestOutboundTime = DateTime.Now;
                    BusinessHelper.Instance.EditEntity(invMother);
                }
                else
                {
                    throw new Exception("系统对应的仓库中没有母件库存,请检查数据！ ");
                }
                ReturnResults<tb_Inventory> rrm = await ctrinv.SaveOrUpdate(invMother);
                if (rrm.Succeeded)
                {
                    #region 子件增加
                    foreach (var child in entity.tb_ProdSplitDetails)
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        //标记是否有期初
                        bool Opening = false;
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv != null)
                        {
                            //更新库存
                            inv.Quantity = inv.Quantity + child.Qty;
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        else
                        {
                            Opening = true;
                            inv = new tb_Inventory();
                            inv.Quantity = inv.Quantity + child.Qty;
                            inv.InitInventory = (int)inv.Quantity;
                            inv.Location_ID = child.Location_ID;

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
                        ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                        if (rr.Succeeded)
                        {
                            if (Opening)
                            {
                                #region 处理期初
                                //库存都没有。期初也会没有 ,并且期初只会新增，不会修改。
                                tb_OpeningInventory oinv = new tb_OpeningInventory();
                                oinv.Inventory_ID = rr.ReturnObject.Inventory_ID;
                                oinv.Cost_price = rr.ReturnObject.Inv_Cost;
                                oinv.Subtotal_Cost_Price = oinv.Cost_price * oinv.InitQty;
                                oinv.InitInvDate = entity.SplitDate;
                                oinv.RefBillID = entity.SplitID;
                                oinv.RefNO = entity.SplitNo;
                                oinv.Notes = entity.SplitNo + "由母件切割得到";//后面修改数据库是不需要？
                                oinv.InitQty = 0;
                                oinv.InitInvDate = System.DateTime.Now;
                                await ctrOPinv.AddReEntityAsync(oinv);
                                #endregion
                            }
                        }
                    }
                    //这部分是否能提出到上一级公共部分？
                    entity.DataStatus = (int)DataStatus.确认;
                    // entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                    //后面已经修改为
                    //  entity.ApprovalResults = approvalEntity.ApprovalResults;
                    entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                    BusinessHelper.Instance.ApproverEntity(entity);
                    //只更新指定列
                    // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdSplit>(entity).ExecuteCommandAsync();
                    #endregion
                }






                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.Succeeded = true;
                rs.ReturnObject = entity as T;
                return rs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                rs.Succeeded = false;
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                _logger.Error(ex, "事务回滚");
                return rs;
            }

        }



        /// <summary>
        ///其它入库单反审会将数量减少
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_ProdSplit entity = ObjectEntity as tb_ProdSplit;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();


                //增加母件
                tb_Inventory invMother = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entity.ProdDetailID && i.Location_ID == entity.Location_ID);
                if (invMother != null)
                {
                    //更新库存
                    invMother.Quantity = invMother.Quantity + entity.SplitParentQty;
                    invMother.LatestOutboundTime = DateTime.Now;
                    BusinessHelper.Instance.EditEntity(invMother);
                }
                else
                {
                    throw new Exception("系统对应的仓库中没有母件库存,请检查数据！ ");
                }
                ReturnResults<tb_Inventory> rrm = await ctrinv.SaveOrUpdate(invMother);
                if (rrm.Succeeded)
                {
                    foreach (var child in entity.tb_ProdSplitDetails)
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
                        // inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                        inv.LatestStorageTime = System.DateTime.Now;
                        #endregion
                        ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                        if (rr.Succeeded)
                        {

                        }
                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = null;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;

                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdSplit>(entity).ExecuteCommandAsync();
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);



                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                rs.Succeeded = false;
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }

        }



        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            // var queryable = _appContext.Db.Queryable<tb_SaleOutDetail>();
            // var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();

            List<tb_ProdSplit> list = await _appContext.Db.CopyNew().Queryable<tb_ProdSplit>().Where(m => m.SplitID == ID)
                             .Includes(a => a.tb_bom_s)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_ProdSplitDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }



    }
}



