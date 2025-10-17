
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
using RUINORERP.Extensions.Middlewares;
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
    public partial class tb_AS_RepairOrderController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 将售后商品转到维后仓修售库
        /// 减少维修仓库的库存
        /// 进入等待维修的状态
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_RepairOrder entity = ObjectEntity as tb_AS_RepairOrder;
            try
            {

                if ((entity.TotalQty == 0 || entity.tb_AS_RepairOrderDetails.Sum(c => c.Quantity) == 0))
                {
                    rmrs.ErrorMsg = $"单据总数量{entity.TotalQty}和明细数量之和{entity.tb_AS_RepairOrderDetails.Sum(c => c.Quantity)},其中有数据为零，请检查后再试！";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //// 后面可以优化  需求单 请求这种。
                //if (entity.tb_as_aftersaleapply == null && entity.ASApplyID > 0)
                //{

                //}

                if (entity.ASApplyID.HasValue && entity.ASApplyID.Value > 0)
                {
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_AfterSaleApply>().SetColumns(it => it.ASProcessStatus == (int)ASProcessStatus.评估报价中).Where(it => it.ASApplyID == entity.ASApplyID).ExecuteCommandHasChangeAsync();
                }

                entity.RepairStatus = (int)RepairStatus.待维修;
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_RepairOrder>(entity).UpdateColumns(it => new
                {
                    it.RepairStatus,
                    it.DataStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();


                //生成应收款
                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    #region 生成应收款单


                    // 获取付款方式信息
                    if (_appContext.PaymentMethodOfPeriod == null)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rmrs.Succeeded = false;
                        rmrs.ErrorMsg = $"请先配置付款方式信息！";
                        if (_appContext.SysConfig.ShowDebugInfo)
                        {
                            _logger.Debug(rmrs.ErrorMsg);
                        }
                        return rmrs;
                    }

                    //如果是账期必须是未付款
                    if (entity.Paytype_ID == _appContext.PaymentMethodOfPeriod.Paytype_ID)
                    {
                        if (entity.PayStatus != (int)PayStatus.未付款)
                        {
                            rmrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"付款方式为账期的工单必须是未付款。";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.Debug(rmrs.ErrorMsg);
                            }
                            return rmrs;
                        }
                    }

                    if (entity.PayStatus == (int)PayStatus.未付款)
                    {
                        if (entity.Paytype_ID != _appContext.PaymentMethodOfPeriod.Paytype_ID)
                        {
                            rmrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"未付款工单的付款方式必须是账期。";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.Debug(rmrs.ErrorMsg);
                            }
                            return rmrs;
                        }
                    }

                    //未付款的账期时，才生成应收款，向客户收取维修费用
                    if (entity.Paytype_ID == _appContext.PaymentMethodOfPeriod.Paytype_ID)
                    {
                        //正常来说。不能重复生成。即使退款也只会有一个对应订单的预收款单。 一个预收款单可以对应正负两个收款单。
                        // 生成预收款单前 检测
                        var ctrpay = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                        var ReceivablePayable = await ctrpay.BuildReceivablePayable(entity);
                        var rmpay = await ctrpay.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(ReceivablePayable, false);
                        if (!rmpay.Succeeded)
                        {
                            // 处理预收款单生成失败的情况
                            rmrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"应收款单生成失败：{rmpay.ErrorMsg ?? "未知错误"}";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.Debug(rmrs.ErrorMsg);
                            }
                        }
                    }

                    #endregion
                }

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
        /// 将售后商品转到维后仓修售库
        /// 减少维修仓库的库存
        /// 进入等待维修的状态
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async Task<ReturnResults<T>> RepairProcessAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_RepairOrder entity = ObjectEntity as tb_AS_RepairOrder;
            try
            {

                if ((entity.TotalQty == 0 || entity.tb_AS_RepairOrderDetails.Sum(c => c.Quantity) == 0))
                {
                    rmrs.ErrorMsg = $"单据总数量{entity.TotalQty}和明细数量之和{entity.tb_AS_RepairOrderDetails.Sum(c => c.Quantity)},其中有数据为零，请检查后再试！";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                // 开启事务，保证数据一致性
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invList = new List<tb_Inventory>();

                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal RepairQty)>();

                //更新  维修仓的数量  减少  材料 是通过领料来减少的。不是在这里处理的。
                foreach (var child in entity.tb_AS_RepairOrderDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentRepairQty = child.Quantity;
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
                                Notes = "维修工单创建",
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
                            RepairQty: currentRepairQty // 首次累加
                                                        //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.RepairQty += currentRepairQty;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    inv.Quantity -= group.Value.RepairQty.ToInt();
                    inv.LatestOutboundTime = System.DateTime.Now;
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

                entity.RepairStatus = (int)RepairStatus.维修中;
                if (entity.ASApplyID.HasValue && entity.ASApplyID.Value > 0)
                {
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_AfterSaleApply>().SetColumns(it => it.ASProcessStatus == (int)ASProcessStatus.维修中).Where(it => it.ASApplyID == entity.ASApplyID).ExecuteCommandAsync();
                }

                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_RepairOrder>(entity).UpdateColumns(it => new
                {
                    it.RepairStatus,
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
                _logger.Error(ex, "维修工单处理时，事务回滚" + ex.Message);
                rmrs.ErrorMsg = "维修工单处理时 事务回滚=>" + ex.Message;
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
        /// 反审核 加库存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_RepairOrder entity = ObjectEntity as tb_AS_RepairOrder;
            try
            {

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少

                //判断是否能反审? 如果出库是草稿，订单反审 修改后。出库再提交 审核。所以 出库审核要核对订单数据。
                if (entity.tb_AS_RepairInStocks != null
                    && (entity.tb_AS_RepairInStocks.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结)
                    && entity.tb_AS_RepairInStocks.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {
                    rmrs.ErrorMsg = "存在已确认或已完结，或已审核的【维修入库单】，不能反审核,请联系管理员，或作退回处理。";
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

                /*
                // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal ConfirmedQty)>();


                foreach (var child in entity.tb_AS_RepairOrderDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentSaleQty = child.Quantity; // 假设 Sale_Qty 对应明细中的 Quantity
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
                    inv.Quantity += group.Value.ConfirmedQty.ToInt();
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity; // 需确保 Inv_Cost 有值
                    invUpdateList.Add(inv);
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvUpdateCounter == 0)
                {
                    _logger.Debug($"{entity.ASApplyNo}反审核，更新库存结果为0行，请检查数据！");
                }
                */

                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    //生成的应收款单 如果没有审核，及支付时是可以反审核删除的。

                    #region 反审 应收款单



                    var ReceivablePayables = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayable>()
                        .Where(p => p.SourceBillId == entity.RepairOrderID && p.SourceBizType == (int)BizType.维修工单 && p.ReceivePaymentType == (int)ReceivePaymentType.收款)
                        .ToListAsync();
                    if (ReceivablePayables != null && ReceivablePayables.Count > 0)
                    {
                        var Paymentable = ReceivablePayables[0];
                        //一个单。只会有一个应收款单
                        if (Paymentable != null)
                        {
                            if (Paymentable.ARAPStatus <= (int)ARAPStatus.待支付)
                            {
                                await _unitOfWorkManage.GetDbClient().DeleteNav(Paymentable).Include(c => c.tb_FM_ReceivablePayableDetails).ExecuteCommandAsync();
                            }
                            else
                            {
                                //客户已经支付了维修款，只能先生成红字负数的应收
                                #region  检测对应的收款单记录，如果没有支付也可以直接删除

                                var PaymentList = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                      .Includes(a => a.tb_FM_PaymentRecordDetails)
                                     .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == Paymentable.ARAPId)).ToListAsync();
                                if (PaymentList != null && PaymentList.Count > 0)
                                {
                                    if (PaymentList.Count > 1 && PaymentList.Sum(c => c.TotalLocalAmount) == 0 && PaymentList.Any(c => c.IsReversed))
                                    {
                                        //退款冲销过
                                        _unitOfWorkManage.RollbackTran();
                                        rmrs.ErrorMsg = $" 维修工单{Paymentable.SourceBillNo}的应收款单{Paymentable.ARAPNo}状态为【已冲销】，不能反审,只能【取消】作废。";
                                        rmrs.Succeeded = false;
                                        return rmrs;
                                    }
                                    else
                                    {
                                        tb_FM_PaymentRecord Payment = PaymentList[0];
                                        if (Payment.PaymentStatus == (int)PaymentStatus.草稿 || Payment.PaymentStatus == (int)PaymentStatus.待审核)
                                        {
                                            var PaymentCounter = await _unitOfWorkManage.GetDbClient().DeleteNav(Payment)
                                                .Include(c => c.tb_FM_PaymentRecordDetails)
                                                .ExecuteCommandAsync();
                                            if (PaymentCounter)
                                            {
                                                await _unitOfWorkManage.GetDbClient().DeleteNav(Paymentable).Include(c => c.tb_FM_ReceivablePayableDetails).ExecuteCommandAsync();
                                            }
                                        }
                                        else
                                        {
                                            _unitOfWorkManage.RollbackTran();
                                            rmrs.ErrorMsg = $"对应的收款单{Payment.PaymentNo}状态为【{(PaymentStatus)Payment.PaymentStatus}】，不能反审\r\n" +
                                                $"只能进行冲销处理";
                                            rmrs.Succeeded = false;
                                            return rmrs;
                                        }
                                    }

                                }
                                //else
                                //{
                                //    //预收单审核了。应该有收款单。正常不会到这步
                                //    await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                                //}
                                #endregion
                            }



                        }
                    }


                    #endregion

                }

                if (entity.ASApplyID.HasValue && entity.ASApplyID.Value > 0)
                {
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_AfterSaleApply>().SetColumns(it => it.ASProcessStatus == (int)ASProcessStatus.登记).Where(it => it.ASApplyID == entity.ASApplyID).ExecuteCommandHasChangeAsync();
                }

                entity.RepairStatus = (int)RepairStatus.评估报价;
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.RepairStatus = null;
                entity.ApprovalResults = false;
                entity.RepairStatus = null;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalOpinions += "【被反审】";
                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_RepairOrder>(entity).UpdateColumns(it => new
                {
                    it.RepairStatus,
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
                    BizTypeMapper mapper = new BizTypeMapper();
                    rmrs.ErrorMsg = mapper.GetBizType(typeof(tb_AS_RepairOrder)).ToString() + "事务回滚=> 保存出错";
                    rmrs.Succeeded = false;
                }
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                BizTypeMapper mapper = new BizTypeMapper();
                rmrs.ErrorMsg = mapper.GetBizType(typeof(tb_AS_RepairOrder)).ToString() + "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
            }
            return rmrs;
        }


    }
}



