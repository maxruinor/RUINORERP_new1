
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
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Extensions;

namespace RUINORERP.Business
{
    public partial class tb_PurOrderController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 批量结案  销售出库标记结案，数据状态为8,可以修改付款状态，同时检测销售订单的付款状态，也可以更新销售订单付款状态
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案就是一个财务确认过程
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<bool>> BatchCloseCaseAsync(List<tb_PurOrder> entitys)
        {
            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                #region 结案
                foreach (var entity in entitys)
                {
                    //结案的出库单。先要是审核成功通过的
                    if (entity.DataStatus == (int)DataStatus.确认 && (entity.ApprovalStatus.HasValue && entity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && entity.ApprovalResults.Value))
                    {

                        //更新在途库存
                        //如果采购明细中的入库数量小于订单中数量，则在途数量要减去这个差值,比方说采购入库只入了一半，那么在途库存就要减去这个差值，另一半可能不要了。
                        if (entity.tb_PurOrderDetails.Select(c => c.DeliveredQuantity).Sum() < entity.tb_PurOrderDetails.Select(c => c.Quantity).Sum())
                        {
                            tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                            foreach (var child in entity.tb_PurOrderDetails)
                            {
                                #region 库存表的更新 这里应该是必需有库存的数据，
                                tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                                if (inv == null)
                                {
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
                                inv.On_the_way_Qty -= (child.Quantity - child.DeliveredQuantity);
                                BusinessHelper.Instance.EditEntity(inv);
                                #endregion
                                ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                                if (rr.Succeeded)
                                {

                                }
                            }
                        }


                        entity.DataStatus = (int)DataStatus.完结;
                        BusinessHelper.Instance.EditEntity(entity);
                        //只更新指定列
                        var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(entity).UpdateColumns(it => new
                        {
                            it.DataStatus,
                            it.CloseCaseOpinions,
                            it.Paytype_ID,
                            it.Modified_by,
                            it.Modified_at
                        }).ExecuteCommandAsync();

                    }
                }

                #endregion
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
 
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rs.ErrorMsg = ex.Message;
                rs.Succeeded = false;
                return rs;
            }

        }




        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_PurOrder entity = ObjectEntity as tb_PurOrder;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();


                if (entity == null)
                {
                    return rmrs;
                }


                //如果采购订单明细数据来自于请购单，则明细要回写状态为已采购
                if (entity.RefBillID.HasValue && entity.RefBillID.Value > 0)
                {
                    if (entity.RefBizType == (int)BizType.请购单)
                    {
                        tb_BuyingRequisition buyingRequisition = _appContext.Db.Queryable<tb_BuyingRequisition>()
                            .Includes(c => c.tb_BuyingRequisitionDetails)
                            .Where(c => c.PuRequisition_ID == entity.RefBillID).Single();
                        if (buyingRequisition != null)
                        {

                            foreach (var child in entity.tb_PurOrderDetails)
                            {
                                var buyItem = buyingRequisition.tb_BuyingRequisitionDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID);
                                if (buyItem != null)//为空则是买的东西不在请购单明细中。
                                {
                                    buyItem.Purchased = true;
                                    buyItem.HasChanged = true;
                                }
                            }
                            await _unitOfWorkManage.GetDbClient().Updateable<tb_BuyingRequisitionDetail>(buyingRequisition.tb_BuyingRequisitionDetails).ExecuteCommandAsync();
                        }
                    }
                }
                foreach (var child in entity.tb_PurOrderDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
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
                    inv.On_the_way_Qty = inv.On_the_way_Qty + child.Quantity;
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }

                    //
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //   entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                //   entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(entity).ExecuteCommandAsync();
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                //_logger.Info(approvalEntity.bizName + "审核事务成功");
                rmrs.ReturnObject= entity as T; 
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
          
                _unitOfWorkManage.RollbackTran();
                if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                {
                    _logger.Error("事务回滚" + ex.Message);
                }
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                _logger.Error(ex);
                return rmrs;
            }

        }


        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_PurOrder entity = ObjectEntity as tb_PurOrder;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {

                //判断是否能反审?
                if (entity.tb_PurEntries != null
                    && (entity.tb_PurEntries.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_PurEntries.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {

                    rs.ErrorMsg = "存在已确认或已完结，或已审核的采购入库单，不能反审核  ";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }



                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //如果采购订单明细数据来自于请购单，则明细要回写状态为已采购
                if (entity.RefBillID.HasValue && entity.RefBillID.Value > 0)
                {
                    if (entity.RefBizType == (int)BizType.请购单)
                    {
                        tb_BuyingRequisition buyingRequisition = _appContext.Db.Queryable<tb_BuyingRequisition>()
                            .Includes(c => c.tb_BuyingRequisitionDetails)
                            .Where(c => c.PuRequisition_ID == entity.RefBillID).Single();
                        if (buyingRequisition != null)
                        {

                            foreach (var child in entity.tb_PurOrderDetails)
                            {
                                var buyItem = buyingRequisition.tb_BuyingRequisitionDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID);
                                buyItem.Purchased = false;
                            }

                            await _unitOfWorkManage.GetDbClient().Updateable<tb_BuyingRequisitionDetail>(buyingRequisition.tb_BuyingRequisitionDetails).ExecuteCommandAsync();

                        }
                    }
                }

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                foreach (var child in entity.tb_PurOrderDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
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
                    inv.On_the_way_Qty = inv.On_the_way_Qty - child.Quantity;
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(entity).ExecuteCommandAsync();
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
        
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            //var queryable = _appContext.Db.Queryable<tb_SaleOrderDetail>();
            //var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();
            List<tb_PurOrder> list = await _appContext.Db.CopyNew().Queryable<tb_PurOrder>().Where(m => m.PurOrder_ID == MainID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }




    }

}



