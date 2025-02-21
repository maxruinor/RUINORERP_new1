
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/24/2024 21:21:26
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
using RUINORERP.Global;
using System.Windows.Forms;
using RUINORERP.Business.Security;

namespace RUINORERP.Business
{
    /// <summary>
    /// 领料单(包括生产和托工)
    /// </summary>
    /// 
    public partial class tb_MaterialRequisitionController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 批量结案  数据状态为8,可以修改付款状态，同时检测领料单的
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案就是一个财务确认过程
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_MaterialRequisition> entitys = new List<tb_MaterialRequisition>();
            entitys = NeedCloseCaseList as List<tb_MaterialRequisition>;
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


                        //这部分是否能提出到上一级公共部分？
                        entity.DataStatus = (int)DataStatus.完结;
                        //if (string.IsNullOrEmpty(entity.cl))
                        //{
                        //    entity.ApprovalOpinions += "批量完结结案";
                        //}
                        BusinessHelper.Instance.EditEntity(entity);
                        //只更新指定列
                        var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialRequisition>(entity).UpdateColumns(it => new { it.DataStatus, it.Modified_by, it.Modified_at }).ExecuteCommandAsync();

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



        /// <summary>
        /// 审核 库存减少， 回写制令单实发数量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>


        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_MaterialRequisition entity = ObjectEntity as tb_MaterialRequisition;
            ReturnResults<T> rrs = new ReturnResults<T>();


            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();


                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //  entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                // entity.ApprovalResults = approvalEntity.ApprovalResults;

                #region 审核 通过时
                if (entity.ApprovalResults.Value)
                {
                    //要更新制令单的已发货物料数量
                    entity.tb_manufacturingorder = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                        .Includes(t => t.tb_ManufacturingOrderDetails)
                        .Includes(t => t.tb_MaterialRequisitions, b => b.tb_MaterialRequisitionDetails)
                    .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                      .Includes(a => a.tb_ManufacturingOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                        .Where(c => c.MOID == entity.MOID).Single();


                    //如果领料明细中的产品。不存在于制令单明细中。审核失败。意思是领料的东西必须是制令单中的明细数据。如果不是 可以用其他 出库
                    foreach (var child in entity.tb_MaterialRequisitionDetails)
                    {
                        if (!entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID))
                        {
                            rrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rrs.ErrorMsg = $"领料明细中，有不属于当前制令单的明细!请检查数据后重试！";
                            _logger.LogInformation(rrs.ErrorMsg);
                            return rrs;
                        }
                    }


                    #region
                    //制令单明细中有相同的产品或物品。
                    var SameItem = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

                    for (int i = 0; i < entity.tb_MaterialRequisitionDetails.Count; i++)
                    {
                        tb_ManufacturingOrderDetail moitem = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.FirstOrDefault(c => c.ProdDetailID == entity.tb_MaterialRequisitionDetails[i].ProdDetailID && c.Location_ID == entity.tb_MaterialRequisitionDetails[i].Location_ID);

                        string prodName = string.Empty;
                        prodName = moitem.tb_proddetail.tb_prod.CNName + moitem.tb_proddetail.tb_prod.Specifications;
                        tb_ManufacturingOrderDetail mochild = new tb_ManufacturingOrderDetail();

                        if (SameItem.Count > 0)
                        {
                            #region 如果存在不是引用的制令单的明细,则不允许领料出库。这样不支持手动添加的情况 ，
                            //意思是一个东西多行。不是引用来的。无法减对应的行数，反审不用管。
                            if (entity.tb_MaterialRequisitionDetails.Any(c => c.ManufacturingOrderDetailRowID == 0))
                            {
                                string msg = $"制令单:{entity.tb_manufacturingorder.MONO}的【{prodName}】在明细中拥有多行记录，必须使用引用的方式添加，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                rrs.ErrorMsg = msg;
                                _logger.LogInformation(msg);
                                return rrs;
                            }
                            #endregion
                            mochild = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.FirstOrDefault(c => c.ProdDetailID == entity.tb_MaterialRequisitionDetails[i].ProdDetailID
                           && c.Location_ID == entity.tb_MaterialRequisitionDetails[i].Location_ID
                           && c.MOCID == entity.tb_MaterialRequisitionDetails[i].ManufacturingOrderDetailRowID
                           );
                        }
                        else
                        {
                            mochild = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.FirstOrDefault(c => c.ProdDetailID == entity.tb_MaterialRequisitionDetails[i].ProdDetailID
                         && c.Location_ID == entity.tb_MaterialRequisitionDetails[i].Location_ID
                         );

                        }

                        //更新当前发料明细行数据对应的制令单明细。以行号和产品ID为标准
                        //先找到这个MO名下所有发料的和以行号和产品ID为标准
                        int TotalQty = entity.tb_MaterialRequisitionDetails.Where(c => c.ProdDetailID == entity.tb_MaterialRequisitionDetails[i].ProdDetailID && c.Location_ID == entity.tb_MaterialRequisitionDetails[i].Location_ID).Sum(c => c.ActualSentQty);
                        if ((mochild.ActualSentQty + TotalQty) > mochild.ShouldSendQty)//超发
                        {
                            /***/

                            if (!entity.ReApply)
                            {
                                string msg = $"非补料时，制令单:{entity.tb_manufacturingorder.MONO}的【{prodName}】的领料数量不能大于制令单对应行的应发数量，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                rrs.ErrorMsg = msg;
                                _logger.LogInformation(msg);
                                return rrs;
                            }
                            else
                            {
                                mochild.OverSentQty += (mochild.ActualSentQty + TotalQty) - mochild.ShouldSendQty;
                            }

                            /***/
                        }
                        mochild.ActualSentQty += TotalQty;

                    }
                    #endregion



                    //更新已交数量,制令单的
                    int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrderDetail>(entity.tb_manufacturingorder.tb_ManufacturingOrderDetails).ExecuteCommandAsync();
                    if (poCounter > 0)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Debug(entity.MaterialRequisitionNO + "==>" + entity.tb_manufacturingorder.MONO + $"对应的制令更新成功===重点代码 看已交数量是否正确");
                        }
                    }

                    #endregion

                    //因为要计算未发数量，所以要更新库存要在最后一步
                    foreach (var child in entity.tb_MaterialRequisitionDetails)
                    {


                        #region 库存表的更新 这里应该是必需有库存的数据，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv != null)
                        {
                            if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - child.ActualSentQty) < 0)
                            {
                                rrs.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                                _unitOfWorkManage.RollbackTran();
                                rrs.Succeeded = false;
                                return rrs;
                            }
                            //更新库存
                            inv.Quantity = inv.Quantity - child.ActualSentQty;
                            //不可能为空
                            tb_ManufacturingOrderDetail MoDeltail = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.Where(c => c.ProdDetailID == inv.ProdDetailID && c.Location_ID == inv.Location_ID).FirstOrDefault();
                            if (MoDeltail != null)
                            {
                                //所有对应的领料明细减少去制令单中的应该发的差。
                                //是不是应该统计审核过的？
                                decimal totalActualSentQty = entity.tb_manufacturingorder.tb_MaterialRequisitions
                                    .Where(c => c.ApprovalStatus.HasValue && c.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && (c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结))
                                    .Where(c => c.ApprovalResults.HasValue && c.ApprovalResults.Value == true)
                                    .Sum(c => c.tb_MaterialRequisitionDetails.Where(c => c.ProdDetailID == inv.ProdDetailID && c.Location_ID == inv.Location_ID).Sum(d => d.ActualSentQty));

                                //所有实发的数量不能大于应发的数量，除非是补料
                                if (!entity.ReApply && totalActualSentQty > MoDeltail.ShouldSendQty)
                                {
                                    string prodName = child.tb_proddetail.tb_prod.CNName + child.tb_proddetail.tb_prod.Specifications;
                                    string msg = $"非补料时，制令单:{entity.tb_manufacturingorder.MONO}的【{prodName}】的领料数量不能大于制令单对应行的应发数量，审核失败！";
                                    MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    _unitOfWorkManage.RollbackTran();
                                    rrs.ErrorMsg = msg;
                                    _logger.LogInformation(msg);
                                    return rrs;

                                }
                                inv.NotOutQty -= child.ActualSentQty.ToInt();
                            }


                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        else
                        {
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception($"当前仓库{child.Location_ID}无产品{child.ProdDetailID}的库存数据,请联系管理员");
                        }
                        // CommService.CostCalculations.CostCalculation(_appContext, inv, child.TransactionPrice);
                        //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                        inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                        inv.LatestOutboundTime = System.DateTime.Now;
                        #endregion

                        ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                        if (rr.Succeeded)
                        {

                        }
                        else
                        {
                            return rrs;
                        }

                    }

                    //TODO: 制令单  怎么样才能结案
                    //领料单，如果来自于制令单，则要把领料出库数量累加到制令单中的已交数量 并且如果数量够则自动结案
                    //if (entity.tb_manufacturingorder != null && entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.Sum(c => c.TotalDeliveredQty) == entity.tb_manufacturingorder.tb_SaleOrderDetails.Sum(c => c.Quantity))
                    //{
                    //    entity.tb_manufacturingorder.DataStatus = (int)DataStatus.完结;
                    //    entity.tb_manufacturingorder.CloseCaseOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "审核销售库单时:" + entity.SaleOutNo + "结案。"; ;
                    //    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_manufacturingorder).UpdateColumns(t => new { t.DataStatus, t.CloseCaseOpinions }).ExecuteCommandAsync();
                    //}

                }



                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                int last = await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialRequisition>(entity).ExecuteCommandAsync();
                if (last > 0)
                {
                    _logger.LogInformation("审核领料单成功" + entity.MaterialRequisitionNO);
                }
                else
                {
                    _logger.LogInformation("审核领料单失败" + entity.MaterialRequisitionNO);
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
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_MaterialRequisition entity = ObjectEntity as tb_MaterialRequisition;
            ReturnResults<T> rs = new ReturnResults<T>();


            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少


                //判断是否能反审?
                if (entity.tb_manufacturingorder != null && entity.tb_manufacturingorder.tb_FinishedGoodsInvs != null
                    && (entity.tb_manufacturingorder.tb_FinishedGoodsInvs.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_manufacturingorder.tb_FinishedGoodsInvs.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {

                    rs.ErrorMsg = "对应的制令单下存在已确认或已完结，或已审核的缴库单，不能反审核  ";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }
                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    //return false;
                    rs.ErrorMsg = "有结案的单据，已经跳过反审";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalOpinions = $"由{_appContext.CurUserInfo.UserInfo.UserName}反审核";
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;

                foreach (var child in entity.tb_MaterialRequisitionDetails)
                {
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
                    //更新在途库存
                    //反审，出库的要加回来，要卖的也要加回来
                    inv.Quantity = inv.Quantity + child.ActualSentQty;
                    //不可能为空
                    //tb_ManufacturingOrderDetail MoDeltail = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.Where(c => c.ProdDetailID == inv.ProdDetailID).FirstOrDefault();
                    //if (MoDeltail != null)
                    //{
                    //所有对应的领料明细减少去制令单中的应该发的差。
                    //是不是应该统计审核过的？
                    //decimal totalActualSentQty = entity.tb_manufacturingorder.tb_MaterialRequisitionses
                    //    .Where(c => c.ApprovalResults.HasValue && c.ApprovalResults.Value == true)
                    //    .Where(c => c.ApprovalStatus.HasValue && c.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && (c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结))
                    //    .Sum(c => c.tb_MaterialRequisitionDetails.Where(c => c.ProdDetailID == inv.ProdDetailID).Sum(d => d.ActualSentQty));
                    //反审只是处理当前领料单的数量， 应发减去实发

                    inv.NotOutQty += child.ActualSentQty;

                    // }
                    //最后出库时间要改回来，这里没有处理
                    //inv.LatestStorageTime
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }
                }

                //更新制作单明细的已发数量
                if (entity.tb_manufacturingorder != null)
                {

                    #region  反审检测写回    主要是修改制令单的实发数量，审核时是统计审核过生效的总和。反审只要减掉当前领料的数量即可？

                    //要更新制令单的已发货物料数量
                    entity.tb_manufacturingorder = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                        .Includes(t => t.tb_ManufacturingOrderDetails)
                        .Includes(t => t.tb_MaterialRequisitions, b => b.tb_MaterialRequisitionDetails)
                    .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                      .Includes(a => a.tb_ManufacturingOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                        .Where(c => c.MOID == entity.MOID).Single();

                    //分两种情况处理。既然是处理领料单明细。就只要循环领料明细再对应到制令单明细的数据变更即可。
                    #region
                    //制令单明细中有相同的产品或物品。
                    var SameItem = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

                    for (int i = 0; i < entity.tb_MaterialRequisitionDetails.Count; i++)
                    {
                        tb_ManufacturingOrderDetail moitem = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.FirstOrDefault(c => c.ProdDetailID == entity.tb_MaterialRequisitionDetails[i].ProdDetailID && c.Location_ID == entity.tb_MaterialRequisitionDetails[i].Location_ID
                       );

                        string prodName = string.Empty;
                        prodName = moitem.tb_proddetail.tb_prod.CNName + moitem.tb_proddetail.tb_prod.Specifications;
                        tb_ManufacturingOrderDetail mochild = new tb_ManufacturingOrderDetail();

                        if (SameItem.Count > 0)
                        {

                            mochild = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.FirstOrDefault(c => c.ProdDetailID == entity.tb_MaterialRequisitionDetails[i].ProdDetailID
                           && c.Location_ID == entity.tb_MaterialRequisitionDetails[i].Location_ID
                           && c.MOCID == entity.tb_MaterialRequisitionDetails[i].ManufacturingOrderDetailRowID
                           );
                        }
                        else
                        {
                            mochild = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.FirstOrDefault(c => c.ProdDetailID == entity.tb_MaterialRequisitionDetails[i].ProdDetailID
                         && c.Location_ID == entity.tb_MaterialRequisitionDetails[i].Location_ID
                         );

                        }

                        //更新当前发料明细行数据对应的制令单明细。以行号和产品ID为标准
                        //先找到这个MO名下所有发料的和以行号和产品ID为标准

                        int TotalQty = entity.tb_MaterialRequisitionDetails.Where(c => c.ProdDetailID == entity.tb_MaterialRequisitionDetails[i].ProdDetailID && c.Location_ID == entity.tb_MaterialRequisitionDetails[i].Location_ID).Sum(c => c.ActualSentQty);
                        //发料数量与制令单实发数量相减不能小于0
                        int Difference = mochild.ActualSentQty.ToInt() - TotalQty;

                        decimal diff = mochild.ActualSentQty - mochild.ShouldSendQty;
                        if (Difference < 0)
                        {
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception($"领料单：{entity.MaterialRequisitionNO}反审核，对应的制令单：{entity.tb_manufacturingorder.MONO}，{prodName}的实发明细不能为负数！");
                        }
                        else
                        {
                            mochild.ActualSentQty -= TotalQty;
                        }

                        if (mochild.ActualSentQty == 0)
                        {
                            mochild.OverSentQty = 0;
                        }
                        else
                        {
                            mochild.OverSentQty -= diff;
                        }
                    }

                    #endregion
                    #endregion
                    //更新已发料
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrderDetail>(entity.tb_manufacturingorder.tb_ManufacturingOrderDetails).ExecuteCommandAsync();

                }

                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？

                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialRequisition>(entity).ExecuteCommandAsync();


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
            List<tb_MaterialRequisition> list = await _appContext.Db.CopyNew().Queryable<tb_MaterialRequisition>().Where(m => m.MR_ID == MainID)
                             .Includes(a => a.tb_MaterialRequisitionDetails)
                             .Includes(a => a.tb_manufacturingorder)
                             .Includes(a => a.tb_department)
                             .Includes(a => a.tb_customervendor)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_MaterialRequisitionDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }



    }





}




