
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/08/2025 16:15:00
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
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business.CommService;
using RUINORERP.Global;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.BizMapperService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 售后申请单 -登记，评估，清单，确认。目标是维修翻新
    /// </summary>
    public partial class tb_AS_AfterSaleApplyController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 转换为售后交付单
        /// </summary>
        /// <param name="AfterSaleApply"></param>
        public async Task<tb_AS_AfterSaleDelivery> ToAfterSaleDelivery(tb_AS_AfterSaleApply AfterSaleApply)
        {
            tb_AS_AfterSaleDelivery entity = new tb_AS_AfterSaleDelivery();
            //转单
            if (AfterSaleApply != null)
            {
                entity = mapper.Map<tb_AS_AfterSaleDelivery>(AfterSaleApply);
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
                List<string> tipsMsg = new List<string>();
                List<tb_AS_AfterSaleDeliveryDetail> details = mapper.Map<List<tb_AS_AfterSaleDeliveryDetail>>(AfterSaleApply.tb_AS_AfterSaleApplyDetails);
                List<tb_AS_AfterSaleDeliveryDetail> NewDetails = new List<tb_AS_AfterSaleDeliveryDetail>();

                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && details[i].ASApplyDetailID > 0)
                    {
                        #region 产品ID可能大于1行，共用料号情况
                        tb_AS_AfterSaleApplyDetail item = AfterSaleApply.tb_AS_AfterSaleApplyDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                        && c.Location_ID == details[i].Location_ID
                        && c.ASApplyDetailID == details[i].ASApplyDetailID);
                        details[i].Quantity = item.ConfirmedQuantity - item.DeliveredQty;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }

                        #endregion
                    }
                    else
                    {
                        #region 每行产品ID唯一
                        tb_AS_AfterSaleApplyDetail item = AfterSaleApply.tb_AS_AfterSaleApplyDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                          && c.Location_ID == details[i].Location_ID
                        && c.ASApplyDetailID == details[i].ASApplyDetailID);
                        details[i].Quantity = item.ConfirmedQuantity - item.DeliveredQty;// 已经交付数量去掉
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        //else
                        //{
                        //    tipsMsg.Add($"当前订单的SKU:{item.tb_proddetail.SKU}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                        //}
                        #endregion
                    }

                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"售后申请单:{entity.ASApplyNo}已全部交付，请检查是否正在重复交付！");
                }

                entity.tb_AS_AfterSaleDeliveryDetails = NewDetails;

                //如果这个订单已经有出库单 则第二次运费为0
                if (AfterSaleApply.tb_AS_AfterSaleDeliveries != null && AfterSaleApply.tb_AS_AfterSaleDeliveries.Count > 0)
                {
                    tipsMsg.Add($"当前【售后申请单】已经有交付记录！");
                }
                entity.DeliveryDate = System.DateTime.Now;
                BusinessHelper.Instance.InitEntity(entity);
                IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
                entity.ASDeliveryNo =await bizCodeService.GenerateBizBillNoAsync(BizType.售后交付单);
                entity.tb_as_aftersaleapply = AfterSaleApply;
                entity.TotalDeliveryQty = NewDetails.Sum(c => c.Quantity);

               
                //保存到数据库

            }
            return entity;
        }


        /// <summary>
        /// 维修工单
        /// </summary>
        /// <param name="AfterSaleApply"></param>
        public async Task<tb_AS_RepairOrder> ToRepairOrder(tb_AS_AfterSaleApply AfterSaleApply)
        {
            tb_AS_RepairOrder entity = new tb_AS_RepairOrder();
            //转单
            if (AfterSaleApply != null)
            {
                entity = mapper.Map<tb_AS_RepairOrder>(AfterSaleApply);
                entity.ApprovalOpinions = "";
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
                //entity.RepairStatus = (int)RepairStatus.待维修;
                List<string> tipsMsg = new List<string>();
                List<tb_AS_RepairOrderDetail> details = mapper.Map<List<tb_AS_RepairOrderDetail>>(AfterSaleApply.tb_AS_AfterSaleApplyDetails);
                List<tb_AS_RepairOrderDetail> NewDetails = new List<tb_AS_RepairOrderDetail>();

                for (global::System.Int32 i = 0; i < details.Count; i++)
                {

                    #region 每行产品ID唯一
                    var item = AfterSaleApply.tb_AS_AfterSaleApplyDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                      && c.Location_ID == details[i].Location_ID);
                    //details[i].Quantity = item.ConfirmedQuantity - item.DeliveredQty;// 减掉已经出库的数量
                    details[i].Quantity = item.ConfirmedQuantity;// 确认的数量全部修，但是可以根据实际来手工修改
                                                                 //评估后 材料表中的成本传过来。成本才生效
                                                                 // details[i].SubtotalCostAmount = (details[i].SubtotalCost + details[i].CustomizedCost) * details[i].Quantity;
                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    //else
                    //{
                    //    tipsMsg.Add($"当前【售后申请单】的SKU:{item.tb_proddetail.SKU}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                    //}
                    #endregion


                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"【售后申请单】:{entity.ASApplyNo}已全部进入维修工单，请检查是否正在重复操作！");
                }

                entity.tb_AS_RepairOrderDetails = NewDetails;
                entity.RepairStartDate = System.DateTime.Now;
                BusinessHelper.Instance.InitEntity(entity);

                IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
                entity.RepairOrderNo = await bizCodeService.GenerateBizBillNoAsync(BizType.维修工单);
                entity.tb_as_aftersaleapply = AfterSaleApply;
                entity.TotalQty = NewDetails.Sum(c => c.Quantity);


                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }

        /// <summary>
        /// 将售后商品转到售后维修仓库
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_AfterSaleApply entity = ObjectEntity as tb_AS_AfterSaleApply;
            try
            {

                if ((entity.TotalConfirmedQuantity == 0 || entity.tb_AS_AfterSaleApplyDetails.Sum(c => c.ConfirmedQuantity) == 0))
                {
                    rmrs.ErrorMsg = $"单据总复核数量{entity.TotalConfirmedQuantity}和明细复核数量之和{entity.tb_AS_AfterSaleApplyDetails.Sum(c => c.ConfirmedQuantity)},其中有数据为零，请检查后再试！";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                // 开启事务，保证数据一致性
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invList = new List<tb_Inventory>();

                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal ConfirmedQty)>();

                //更新拟销售量
                foreach (var child in entity.tb_AS_AfterSaleApplyDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentAfterSaleQty = child.ConfirmedQuantity;
                    DateTime currentOutboundTime = DateTime.Now; // 每次出库更新时间
                                                                 // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            //采购和销售都会提前处理。所以这里默认提供一行数据。成本和数量都可能为0
                            inv = new tb_Inventory
                            {
                                ProdDetailID = key.ProdDetailID,
                                Location_ID = key.Location_ID,
                                Quantity = 0, // 初始数量
                                InitInventory = 0,
                                Inv_Cost = 0, // 假设成本价需从其他地方获取，需根据业务补充 。 售后维修仓  不处理成本 所以是不是在仓库设置中设置一个参数是否处理成本
                                Notes = "售后申请单创建",
                                Sale_Qty = 0,
                            };
                            BusinessHelper.Instance.InitEntity(inv);
                        }
                        else
                        {
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            ConfirmedQty: currentAfterSaleQty // 首次累加
                                                              //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.ConfirmedQty += currentAfterSaleQty;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    // 累加数值字段
                    inv.Quantity += group.Value.ConfirmedQty.ToInt();
                    inv.LatestStorageTime = System.DateTime.Now;
                    invList.Add(inv);
                }

                _unitOfWorkManage.BeginTran();
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                if (Counter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存更新数据为0，更新失败！");
                }


                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                entity.ApprovalResults = true;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_AfterSaleApply>(entity).UpdateColumns(it => new
                {
                    it.DataStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();


                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                
                                  
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
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

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_AfterSaleApply entity = ObjectEntity as tb_AS_AfterSaleApply;
            try
            {

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少

                //判断是否能反审? 如果出库是草稿，订单反审 修改后。出库再提交 审核。所以 出库审核要核对订单数据。
                if (entity.tb_AS_AfterSaleDeliveries != null
                    && (entity.tb_AS_AfterSaleDeliveries.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结)
                    && entity.tb_AS_AfterSaleDeliveries.Any(c => c.ApprovalStatus == (int)ApprovalStatus.审核通过)))
                {
                    rmrs.ErrorMsg = "存在已确认或已完结，或已审核的【售后交付单】，不能反审核,请联系管理员，或作退回处理。";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {

                    rmrs.ErrorMsg = "只能反审核已确认,并且有审核结果的订单 ";
                    rmrs.Succeeded = false;
                    return rmrs;
                }
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal ConfirmedQty)>();


                foreach (var child in entity.tb_AS_AfterSaleApplyDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentSaleQty = child.ConfirmedQuantity; // 假设 Sale_Qty 对应明细中的 Quantity
                                                                      // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            //实际不会出现这个情况。因为审核时创建了。
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception("库存数据不存在,反审失败！");
                        }

                        BusinessHelper.Instance.EditEntity(inv);
                        #endregion
                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            ConfirmedQty: currentSaleQty // 首次累加
                        );
                        inventoryGroups[key] = group;
                    }
                    else
                    {
                        // 累加分组的数值字段 反审也是累加。下面才可能是减少
                        group.ConfirmedQty += currentSaleQty;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    //反审 要用减
                    inv.Quantity -= group.Value.ConfirmedQty.ToInt();
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity; // 需确保 Inv_Cost 有值
                    invUpdateList.Add(inv);
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvUpdateCounter == 0)
                {
                    _logger.Debug($"{entity.ASApplyNo}反审核，更新库存结果为0行，请检查数据！");
                }

                //AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                //if (authorizeController.EnableFinancialModule())
                //{

                //}
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalOpinions += "【被反审】";
                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.ApprovalStatus,
                    it.DataStatus,
                    it.ApprovalResults,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();
                if (result > 0)
                {
                    // 注意信息的完整性
                    _unitOfWorkManage.CommitTran();
                    rmrs.ReturnObject = entity as T;
                    rmrs.Succeeded = true;
                }
                else
                {
                    _unitOfWorkManage.RollbackTran();
                    ;
                    rmrs.ErrorMsg = BizMapperService.EntityMappingHelper.GetBizType(typeof(tb_AS_AfterSaleApply)).ToString() + "事务回滚=> 保存出错";
                    rmrs.Succeeded = false;
                }
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                 
                rmrs.ErrorMsg = BizMapperService.EntityMappingHelper.GetBizType(typeof(tb_AS_AfterSaleApply)).ToString() + "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
            }
            return rmrs;
        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            List<tb_AS_AfterSaleApply> list = await _appContext.Db.CopyNew().Queryable<tb_AS_AfterSaleApply>().Where(m => m.ASApplyID == MainID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_projectgroup)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_AS_AfterSaleApplyDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }

    }
}



