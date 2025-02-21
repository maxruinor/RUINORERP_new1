
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/24/2024 21:21:28
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
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Security;
using RUINORERP.Global;
using System.Windows.Forms;

namespace RUINORERP.Business
{
    /// <summary>
    /// 退料单(包括生产和托工） 在生产过程中或结束后，我们会根据加工任务（制令单）进行生产退料。这时就需要使用生产退料这个单据进行退料。生产退料单会影响到制令单的直接材料成本，它会冲减该制令单所发生的原料成本
    /// </summary>
    public partial class tb_MaterialReturnController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 批量结案  数据状态为8,可以修改付款状态，同时检测退料单的
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案就是一个财务确认过程
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_MaterialReturn> entitys = new List<tb_MaterialReturn>();
            entitys = NeedCloseCaseList as List<tb_MaterialReturn>;

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
                        ////只修改订单的付款状态
                        //if (entity.tb_MaterialReturnDetails.TotalQty == entity.tb_SaleOutDetails.Sum(c => c.Quantity))
                        //{
                        //    entity.tb_saleorder.PayStatus = entity.PayStatus.Value;
                        //}
                        //await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity.tb_saleorder).ExecuteCommandAsync();

                        //这部分是否能提出到上一级公共部分？
                        entity.DataStatus = (int)DataStatus.完结;
                        BusinessHelper.Instance.EditEntity(entity);
                        //只更新指定列
                        var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialReturn>(entity).UpdateColumns(it => new { it.DataStatus, it.Modified_by, it.Modified_at }).ExecuteCommandAsync();
                        //.Where(d => d.ProdBaseID == info.ProdBaseID).ExecuteCommandAsync();
                        // return affectedRows > 0;
                        //var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                        //await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity).ExecuteCommandAsync();
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
                ;
                _unitOfWorkManage.RollbackTran();
                rs.ErrorMsg = ex.Message;
                rs.Succeeded = false;
                return rs;
            }

        }



        /// <summary>
        /// 审核 库存变化（增加）  超发可能要退，强制结案会退,
        /// 同时 对应的领料单，实发数也要减少,退回数量增加
        /// 制令单对应的材料也要减少
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_MaterialReturn entity = ObjectEntity as tb_MaterialReturn;
            ReturnResults<T> rrs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                //entity.ApprovalResults = approvalEntity.ApprovalResults;

                #region 审核 通过时

                if (entity.tb_materialrequisition == null)
                {
                    entity.tb_materialrequisition = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisition>().Where(c => c.MR_ID == entity.MR_ID)
                                     .Includes(a => a.tb_MaterialRequisitionDetails, b => b.tb_proddetail, c => c.tb_prod)
                                     .Includes(a => a.tb_manufacturingorder, b => b.tb_proddetail, c => c.tb_prod)
                                     .Includes(a => a.tb_manufacturingorder, b => b.tb_ManufacturingOrderDetails)
                                     .SingleAsync();
                }
                else
                {
                    if (entity.tb_materialrequisition.tb_manufacturingorder == null)
                    {
                        entity.tb_materialrequisition.tb_manufacturingorder = await _unitOfWorkManage.GetDbClient()
                                    .Queryable<tb_ManufacturingOrder>()
                                    .Where(c => c.MOID == entity.tb_materialrequisition.MOID)
                                     .Includes(b => b.tb_proddetail, c => c.tb_prod)
                                     .Includes(b => b.tb_ManufacturingOrderDetails)
                                     .SingleAsync();
                    }
                }
                //先获取到相关发料主子数据
                if (entity.tb_materialrequisition != null)
                {
                    if (entity.tb_materialrequisition.tb_MaterialRequisitionDetails == null)
                    {
                        entity.tb_materialrequisition.tb_MaterialRequisitionDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisitionDetail>().Where(w => w.MR_ID == entity.MR_ID)
                         .Includes(b => b.tb_proddetail, c => c.tb_prod)
                         .ToListAsync();
                    }
                }

                //先获取到相关发料主子数据
                if (entity.tb_materialrequisition.tb_manufacturingorder != null)
                {
                    if (entity.tb_materialrequisition.tb_manufacturingorder.tb_ManufacturingOrderDetails == null)
                    {
                        entity.tb_materialrequisition.tb_manufacturingorder.tb_ManufacturingOrderDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrderDetail>().Where(w => w.MOID == entity.tb_materialrequisition.tb_manufacturingorder.MOID)
                         .Includes(b => b.tb_proddetail, c => c.tb_prod)
                         .ToListAsync();
                    }
                }


                if (entity.ApprovalResults.Value)
                {
                    //因为要计算未发数量，所以要更新库存要在最后一步
                    foreach (var child in entity.tb_MaterialReturnDetails)
                    {
                        var Detail = entity.tb_materialrequisition.tb_MaterialRequisitionDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID);
                        var prodInfo = Detail.tb_proddetail.tb_prod.CNName + Detail.tb_proddetail.tb_prod.Specifications;
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv != null)
                        {
                            //更新库存
                            inv.Quantity = inv.Quantity + child.Quantity;
                            inv.NotOutQty += child.Quantity;
                            inv.LatestStorageTime = System.DateTime.Now;
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        else
                        {
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception($"当前仓库无产品{prodInfo}的库存数据,请联系管理员");
                        }
                        // CommService.CostCalculations.CostCalculation(_appContext, inv, child.TransactionPrice);
                        //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                        inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                        inv.LatestOutboundTime = System.DateTime.Now;
                        #endregion

                        ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                        if (rr.Succeeded)
                        {
                            var tb_MaterialRequisitionDetail = entity.tb_materialrequisition.tb_MaterialRequisitionDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID && child.Location_ID == child.Location_ID);

                            //如果退料数量大于领料数量就不对了
                            if (tb_MaterialRequisitionDetail.ActualSentQty < child.Quantity)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"{prodInfo}的退回数量不能大于实发数量,请检查后再试");
                            }
                            tb_MaterialRequisitionDetail.ReturnQty += child.Quantity;

                            tb_ManufacturingOrderDetail manufacturingOrderDetail = entity.tb_materialrequisition.tb_manufacturingorder.tb_ManufacturingOrderDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID && child.Location_ID == child.Location_ID);
                            manufacturingOrderDetail.ActualSentQty -= child.Quantity;

                            // tb_MaterialRequisitionDetail.ActualSentQty -= child.Quantity; 实发不变。应该是变到 制令单的实发，因为还可能要退了再补进去。做领料单时引用制令单的数量
                        }
                    }
                }

                #endregion
                await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrderDetail>(entity.tb_materialrequisition.tb_manufacturingorder.tb_ManufacturingOrderDetails).ExecuteCommandAsync();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialRequisitionDetail>(entity.tb_materialrequisition.tb_MaterialRequisitionDetails).ExecuteCommandAsync();
                entity.tb_materialrequisition.TotalReQty = entity.tb_materialrequisition.tb_MaterialRequisitionDetails.Sum(s => s.ReturnQty);

                //entity.tb_materialrequisition.TotalSendQty = entity.tb_materialrequisition.tb_MaterialRequisitionDetails.Sum(s => s.ActualSentQty);
                await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_materialrequisition).UpdateColumns(t => new { t.TotalReQty }).ExecuteCommandAsync();

                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                int last = await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialReturn>(entity).ExecuteCommandAsync();
                if (last > 0)
                {
                    _logger.LogInformation("审核退料单成功" + entity.BillNo);
                }
                else
                {
                    _logger.LogInformation("审核退料单失败" + entity.BillNo);
                    _unitOfWorkManage.RollbackTran();
                    rrs.Succeeded = false;
                    return rrs;
                }
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rrs.ReturnObject = entity as T;
                rrs.Succeeded = true;
                return rrs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                rrs.ErrorMsg = "事务回滚=>" + ex.Message;
                _logger.Error(ex, "事务回滚");
                return rrs;
            }

        }

        /// <summary>
        /// 反审核 
        /// 这里要判断，如果针对这个返工退货，已经有或部分入库了。这时不可以反审。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_MaterialReturn entity = ObjectEntity as tb_MaterialReturn;
            ReturnResults<T> rs = new ReturnResults<T>();
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
                    rs.ErrorMsg = "有结案的单据，已经跳过反审";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }
                if (entity.tb_materialrequisition == null)
                {
                    entity.tb_materialrequisition = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisition>().Where(c => c.MR_ID == entity.MR_ID)
                                     .Includes(a => a.tb_MaterialRequisitionDetails, b => b.tb_proddetail, c => c.tb_prod)
                                     .Includes(a => a.tb_manufacturingorder, b => b.tb_proddetail, c => c.tb_prod)
                                     .Includes(a => a.tb_manufacturingorder, b => b.tb_ManufacturingOrderDetails)
                                     .SingleAsync();
                }
                else
                {
                    if (entity.tb_materialrequisition.tb_manufacturingorder == null)
                    {
                        entity.tb_materialrequisition.tb_manufacturingorder = await _unitOfWorkManage.GetDbClient()
                                    .Queryable<tb_ManufacturingOrder>()
                                    .Where(c => c.MOID == entity.tb_materialrequisition.MOID)
                                     .Includes(b => b.tb_proddetail, c => c.tb_prod)
                                     .Includes(b => b.tb_ManufacturingOrderDetails)
                                     .SingleAsync();
                    }
                }

                //先获取到相关发料主子数据
                if (entity.tb_materialrequisition != null)
                {
                    if (entity.tb_materialrequisition.tb_MaterialRequisitionDetails == null)
                    {
                        entity.tb_materialrequisition.tb_MaterialRequisitionDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisitionDetail>().Where(w => w.MR_ID == entity.MR_ID)
                         .Includes(b => b.tb_proddetail, c => c.tb_prod)
                         .ToListAsync();
                    }
                }


                foreach (var child in entity.tb_MaterialReturnDetails)
                {
                    //var Detail = entity.tb_materialrequisition.tb_MaterialRequisitionDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID);
                    //var prodInfo = Detail.tb_proddetail.tb_prod.CNName + Detail.tb_proddetail.tb_prod.Specifications;

                    #region 库存表的更新 ，
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
                    if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - child.Quantity) < 0)
                    {
                        rs.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                        _unitOfWorkManage.RollbackTran();
                        rs.Succeeded = false;
                        return rs;
                    }
                    //更新在途库存
                    //反审，出库的要加回来，要卖的也要加回来
                    inv.Quantity = inv.Quantity - child.Quantity;
                    inv.NotOutQty -= child.Quantity;
                    inv.LatestOutboundTime = System.DateTime.Now;
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        //更新领料单明细中退回数量
                        var tb_MaterialRequisitionDetail = entity.tb_materialrequisition.tb_MaterialRequisitionDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID && child.Location_ID == child.Location_ID);

                        tb_MaterialRequisitionDetail.ReturnQty -= child.Quantity;

                        tb_ManufacturingOrderDetail manufacturingOrderDetail = entity.tb_materialrequisition.tb_manufacturingorder.tb_ManufacturingOrderDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID && child.Location_ID == child.Location_ID);
                        manufacturingOrderDetail.ActualSentQty += child.Quantity;

                        // tb_MaterialRequisitionDetail.ActualSentQty += child.Quantity;
                    }
                }

                await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrderDetail>(entity.tb_materialrequisition.tb_manufacturingorder.tb_ManufacturingOrderDetails).ExecuteCommandAsync();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialRequisitionDetail>(entity.tb_materialrequisition.tb_MaterialRequisitionDetails).ExecuteCommandAsync();
                entity.tb_materialrequisition.TotalReQty = entity.tb_materialrequisition.tb_MaterialRequisitionDetails.Sum(s => s.ReturnQty);
                //entity.tb_materialrequisition.TotalSendQty = entity.tb_materialrequisition.tb_MaterialRequisitionDetails.Sum(s => s.ActualSentQty);
                await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_materialrequisition).UpdateColumns(t => new { t.TotalReQty }).ExecuteCommandAsync();

                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalOpinions = $"由{_appContext.CurUserInfo.UserInfo.UserName}反审核";
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？

                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialReturn>(entity).ExecuteCommandAsync();


                // 注意信息的完整性
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
                rs.Succeeded = false;
                return rs;
            }

        }





        /// <summary>
        /// 某字段是否存在
        /// </summary>
        /// <param name="exp">e => e.ModeuleName == mod.ModeuleName</param>
        /// <returns></returns>
        public T ExistFieldValueWithReturn(Expression<Func<T, bool>> exp)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>()
                .Where(exp)
                .First();
        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            List<tb_MaterialReturn> list = await _appContext.Db.CopyNew().Queryable<tb_MaterialReturn>().Where(m => m.MRE_ID == MainID)
                             .Includes(a => a.tb_MaterialReturnDetails)
                             .Includes(a => a.tb_employee)
                             .Includes(a => a.tb_customervendor)
                             .Includes(a => a.tb_department)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_MaterialReturnDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }









    }
}



