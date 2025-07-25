﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/04/2025 18:27:20
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
using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.Global;
using RUINORERP.Model.CommonModel;
using System.Windows.Forms;

namespace RUINORERP.Business
{
    /// <summary>
    /// 返工入库
    /// </summary>
    public partial class tb_MRP_ReworkEntryController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 返回批量审核的结果
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_MRP_ReworkEntry entity = ObjectEntity as tb_MRP_ReworkEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            try
            {
                //采购入库总数量和明细求和检查
                if (entity.TotalQty.Equals(entity.tb_MRP_ReworkEntryDetails.Sum(c => c.Quantity)) == false)
                {
                    rs.ErrorMsg = $"返工入库数量与明细之和不相等!请检查数据后重试！";
                    rs.Succeeded = false;
                    return rs;
                }
                //都返工了。正常是不用判断初始库存数据了
                
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();

                entity.tb_mrp_reworkreturn = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkReturn>()
                     .Includes(a => a.tb_MRP_ReworkEntries, b => b.tb_MRP_ReworkEntryDetails)
                     .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                     .Includes(a => a.tb_MRP_ReworkReturnDetails, b => b.tb_proddetail, c => c.tb_prod)
                     .Where(c => c.ReworkReturnID == entity.ReworkReturnID)
                     .Single();

                if (entity.tb_mrp_reworkreturn == null)
                {
                    rs.ErrorMsg = $"没有找到对应的返工退库单!请检查数据后重试！";
                    rs.Succeeded = false;
                    return rs;
                }

                //如果入库明细中的产品。不存在于订单中。审核失败。
                foreach (var child in entity.tb_MRP_ReworkEntryDetails)
                {
                    if (!entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID))
                    {
                        rs.Succeeded = false;
                        rs.ErrorMsg = $"返工入库明细中有产品不属于当前退库单!请检查数据后重试！";
                        return rs;
                    }
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
                List<tb_MRP_ReworkEntryDetail> detailList = new List<tb_MRP_ReworkEntryDetail>();
                foreach (var item in entity.tb_mrp_reworkreturn.tb_MRP_ReworkEntries)
                {
                    detailList.AddRange(item.tb_MRP_ReworkEntryDetails);
                }

                //判断更新引用的退库单数据
                for (int i = 0; i < entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails.Count; i++)
                {
                    //如果当前订单明细行，不存在于入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较
                    string prodName = entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].tb_proddetail.tb_prod.CNName +
                              entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].tb_proddetail.tb_prod.Specifications;

                    //一对一时 查出入库单对应返工退货名下的所有其它入库的明细的和
                    var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].ProdDetailID
                    && c.Location_ID == entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].Location_ID).Sum(c => c.Quantity);
                    if (inQty > entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].Quantity)
                    {
                        string msg = $"返工退库:{entity.tb_mrp_reworkreturn.ReworkReturnNo}的【{prodName}】的入库数量不能大于返工退库单中对应行的数量\r\n" + $"或存在重复录入了返工入库单。";
                        rs.ErrorMsg = msg;
                        _unitOfWorkManage.RollbackTran();
                        _logger.LogInformation(msg);
                        return rs;
                    }
                    else
                    {
                        //当前行累计到交付
                        var RowQty = entity.tb_MRP_ReworkEntryDetails.Where(c => c.ProdDetailID == entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].ProdDetailID && c.Location_ID == entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].Location_ID).Sum(c => c.Quantity);
                        entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].DeliveredQuantity += RowQty;
                        //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                        if (entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].DeliveredQuantity > entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].Quantity)
                        {
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception($"返工入库：{entity.ReworkEntryNo}审核时，对应的退库单：{entity.tb_mrp_reworkreturn.ReworkReturnNo}，入库总数量不能大于退库数量！");
                        }
                    }

                    //更新已交数量
                    int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_MRP_ReworkReturnDetail>(entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails).ExecuteCommandAsync();
                    if (poCounter > 0)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Debug(entity.ReworkEntryNo + "==>" + entity.ReworkEntryNo + $"对应 的返工退库单更新成功===重点代码 看已交数量是否正确");
                        }
                    }
                }

                foreach (tb_MRP_ReworkEntryDetail child in entity.tb_MRP_ReworkEntryDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {

                        _unitOfWorkManage.RollbackTran();
                        throw new Exception($"返工入库：{entity.ReworkEntryNo}审核时，对应的入库明细没有对应的库存初始数据！");
                    }
                    else
                    {

                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestStorageTime = System.DateTime.Now;
                    //采购订单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty - child.Quantity;

                    //直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                    //平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                    //先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。适用范围：适用于存货的实物流转比较符合先进先出的假设，比如食品、药品等有保质期限制的商品，先购进的存货会先发出销售。

                    //数据来源可以是多种多样的，例如：
                    //采购价格：从供应商处购买产品或物品时的价格。
                    //生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                    //市场价格：参考市场上类似产品或物品的价格。
                    inv.Quantity = inv.Quantity + child.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;
                    #endregion
                    //这样是不是有事务？
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Info(child.ProdDetailID + "==>" + child.property + "库存更新成功");
                        }
                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //  entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                //  entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                             .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                             .ExecuteCommandHasChangeAsync();

                 

                //采购入库单，如果来自于采购订单，则要把入库数量累加到订单中的已交数量 TODO 销售也会有这种情况
                if (entity.tb_mrp_reworkreturn != null && entity.tb_mrp_reworkreturn.DataStatus == (int)DataStatus.确认 &&
                    (entity.TotalQty == entity.tb_mrp_reworkreturn.TotalQty ||
                    entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails.Sum(c => c.DeliveredQuantity) == entity.tb_mrp_reworkreturn.TotalQty)
                    && entity.ApprovalStatus == (int)ApprovalStatus.已审核
                    )
                {
                    entity.tb_mrp_reworkreturn.DataStatus = (int)DataStatus.完结;
                    entity.tb_mrp_reworkreturn.CloseCaseOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "审核退工入库单:" + entity.ReworkEntryNo + "结案。"; ;
                    int poendcounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_mrp_reworkreturn)
                        .UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                    if (poendcounter > 0)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Info(entity.tb_mrp_reworkreturn.ReworkReturnNo + "==>" + "结案状态更新成功");
                        }
                    }
                }




                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "事务回滚" + ex.Message);
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
            tb_MRP_ReworkEntry entity = ObjectEntity as tb_MRP_ReworkEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();

                foreach (var child in entity.tb_MRP_ReworkEntryDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，

                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception($"返工入库：{entity.ReworkEntryNo}审核时，对应的入库明细没有对应的库存初始数据！");
                    }
                    else
                    {

                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestStorageTime = System.DateTime.Now;

                    //采购订单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty + child.Quantity;

                    inv.Quantity = inv.Quantity - child.Quantity;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestOutboundTime = System.DateTime.Now;

                    #endregion
                    //这个是不是有事务功能？
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }
                }

                if (entity.tb_mrp_reworkreturn != null)
                {
                    #region  反审检测写回 退回

                    //处理采购订单
                    entity.tb_mrp_reworkreturn = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkReturn>()
                       .Includes(a => a.tb_MRP_ReworkEntries, b => b.tb_MRP_ReworkEntryDetails)
                       .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                       .Includes(a => a.tb_MRP_ReworkReturnDetails, b => b.tb_proddetail, c => c.tb_prod)
                       .Where(c => c.ReworkReturnID == entity.ReworkReturnID)
                       .Single();


                    //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
                    List<tb_MRP_ReworkEntryDetail> detailList = new List<tb_MRP_ReworkEntryDetail>();
                    foreach (var item in entity.tb_mrp_reworkreturn.tb_MRP_ReworkEntries)
                    {
                        detailList.AddRange(item.tb_MRP_ReworkEntryDetails);
                    }

                    //分两种情况处理。
                    for (int i = 0; i < entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails.Count; i++)
                    {
                        //如果当前订单明细行，不存在于入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较
                        string prodName = entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].tb_proddetail.tb_prod.Specifications;

                        //一对一时
                        var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].ProdDetailID
                        && c.Location_ID == entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].Location_ID).Sum(c => c.Quantity);
                        if (inQty > entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].Quantity)
                        {

                            string msg = $"返工退库:{entity.tb_mrp_reworkreturn.ReworkReturnNo}的【{prodName}】的返工入库数量不能大于对应退库时的数量。";
                            rs.ErrorMsg = msg;
                            _unitOfWorkManage.RollbackTran();
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.LogInformation(msg);
                            }
                            return rs;
                        }
                        else
                        {
                            //当前行累计到交付
                            var RowQty = entity.tb_MRP_ReworkEntryDetails.Where(c => c.ProdDetailID == entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].Location_ID).Sum(c => c.Quantity);
                            entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].DeliveredQuantity -= RowQty;
                            //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails[i].DeliveredQuantity < 0)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"返工入库：{entity.ReworkEntryNo}反审核时，对应的退库：{entity.tb_mrp_reworkreturn.ReworkReturnNo}，{prodName}的明细不能为负数！");
                            }
                        }


                        #endregion

                        //更新已交数量
                        int updatecounter = await _unitOfWorkManage.GetDbClient()
                            .Updateable<tb_MRP_ReworkReturnDetail>(entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails).ExecuteCommandAsync();
                        if (updatecounter == 0)
                        {

                        }

                    }
                    //这部分是否能提出到上一级公共部分？
                }
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "被反审核";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                await _unitOfWorkManage.GetDbClient().Updateable<tb_MRP_ReworkEntry>(entity).ExecuteCommandAsync();

                //采购入库单，如果来自于采购订单，则要把入库数量累加到订单中的已交数量
                if (entity.tb_mrp_reworkreturn != null && entity.tb_mrp_reworkreturn.TotalQty != entity.tb_mrp_reworkreturn.tb_MRP_ReworkReturnDetails.Sum(c => c.DeliveredQuantity))
                {
                    entity.tb_mrp_reworkreturn.DataStatus = (int)DataStatus.确认;
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_mrp_reworkreturn).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                }

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
            List<tb_MRP_ReworkEntry> list = await _appContext.Db.CopyNew().Queryable<tb_MRP_ReworkEntry>().Where(m => m.ReworkEntryID == MainID)
                             .Includes(a => a.tb_customervendor)
                                .Includes(a => a.tb_employee)
                          .Includes(a => a.tb_department)
                             .Includes(a => a.tb_mrp_reworkreturn, c => c.tb_MRP_ReworkReturnDetails)
                           .Includes(a => a.tb_MRP_ReworkEntryDetails, c => c.tb_location)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_MRP_ReworkEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                    .Includes(a => a.tb_MRP_ReworkEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                                 .ToListAsync();
            return list as List<T>;
        }






    }
}



