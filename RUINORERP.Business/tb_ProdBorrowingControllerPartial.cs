
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/01/2023 18:04:38
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
using RUINORERP.Business.Security;


namespace RUINORERP.Business
{
    public partial class tb_ProdBorrowingController<T>
    {

        /// <summary>
        /// 审核借出 注意逻辑是减少库存，并且更新单据本身状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rsms = new ReturnResults<T>();
            tb_ProdBorrowing entity = ObjectEntity as tb_ProdBorrowing;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                foreach (var child in entity.tb_ProdBorrowingDetails)
                {

                    #region 库存表的更新 这里应该是必需有库存的数据，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv != null)
                    {
                        if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - child.Qty) < 0)
                        {
                            rsms.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                            _unitOfWorkManage.RollbackTran();
                            rsms.Succeeded = false;
                            return rsms;
                        }
                        //更新库存
                        inv.Quantity = inv.Quantity - child.Qty;

                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    else
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception($"当前仓库{child.Location_ID}无产品{child.ProdDetailID}的库存数据,请联系管理员");
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
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestOutboundTime = System.DateTime.Now;
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                // entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdBorrowing>(entity).ExecuteCommandAsync();
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();

                // entitys[ii].tb_purorder.CloseCaseOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "审核入库单:" + entitys[ii].PurEntryNo + "结案。"; ;
                rsms.ReturnObject = entity as T;
                rsms.Succeeded = true;
                return rsms;
            }
            catch (Exception ex)
            {
           
                _unitOfWorkManage.RollbackTran();


                _logger.Error(ex, "事务回滚" + ex.Message);
                rsms.ErrorMsg = "事务回滚=>" + ex.Message;
                return rsms;
            }

        }


        /// <summary>
        /// 反审核 库存加回来
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rsms = new ReturnResults<T>();
            tb_ProdBorrowing entity = ObjectEntity as tb_ProdBorrowing;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少


                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    //return false;
                    return rsms;
                }
                foreach (var child in entity.tb_ProdBorrowingDetails)
                {
                    #region 库存表的更新 ，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rsms.ErrorMsg = $"{child.ProdDetailID}当前产品无库存数据，无法借出。请使用【期初盘点】【采购入库】】【生产缴库】的方式进行盘点后，再操作。";
                        rsms.Succeeded = false;
                        return rsms;

                       
                        inv = new tb_Inventory();
                        inv.ProdDetailID = child.ProdDetailID;
                        inv.Location_ID = child.Location_ID;
                        inv.Quantity = 0;
                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "";//后面修改数据库是不需要？
                                       //inv.LatestStorageTime = System.DateTime.Now;
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    //更新在途库存
                    //反审，出库的要加回来，要卖的也要加回来
                    inv.Quantity = inv.Quantity + child.Qty;
                    //最后出库时间要改回来，这里没有处理
                    //inv.LatestStorageTime
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }



                    //==

                    //这部分是否能提出到上一级公共部分？
                    entity.DataStatus = (int)DataStatus.新建;
                    entity.ApprovalResults = false;
                    entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                    BusinessHelper.Instance.ApproverEntity(entity);

                    //后面是不是要做一个审核历史记录表？

                    //只更新指定列
                    // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdBorrowing>(entity).ExecuteCommandAsync();
                }


                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                //  _logger.Info(approvalEntity.bizName + "审核事务成功");
                rsms.ReturnObject = entity as T;
                rsms.Succeeded = true;
                return rsms;
            }
            catch (Exception ex)
            {
              
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                //  _logger.Error(approvalEntity.bizName + "事务回滚");
                return rsms;
            }

        }

        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_ProdBorrowing> list = await _appContext.Db.CopyNew().Queryable<tb_ProdBorrowing>().Where(m => m.BorrowID == ID)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_ProdBorrowingDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }


    }


}



