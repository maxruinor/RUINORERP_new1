
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Business
{
    public partial class tb_PurEntryReController<T>
    {

        /// <summary>
        /// 采购入库退回，会影响到原始采购单，UI上如果勾选影响原始采购单，则会影响到原始采购单的状态和数量，如果不勾选，则只退货。不要退了。。退货款。
        /// 采购入库退货 是否需要更新回写采购订单明细中的退回数量呢？
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_PurEntryRe entity = ObjectEntity as tb_PurEntryRe;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                if (entity == null)
                {
                    return rs;
                }
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var ReDetail in entity.tb_PurEntryReDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == ReDetail.ProdDetailID && i.Location_ID == ReDetail.Location_ID);
                    if (inv != null)
                    {
                        if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - ReDetail.Quantity) < 0)
                        {
                            // rrs.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                            rs.ErrorMsg = $"库存为：{inv.Quantity}，采购退回数量为：{ReDetail.Quantity}\r\n 系统设置不允许负库存， 请检查退回数量与库存相关数据";
                            _unitOfWorkManage.RollbackTran();
                            rs.Succeeded = false;
                            return rs;
                        }

                        //更新库存
                        inv.Quantity = inv.Quantity - ReDetail.Quantity;
                        if (entity.ProcessWay == (int)PurReProcessWay.需要返回)
                        {
                            inv.On_the_way_Qty = inv.On_the_way_Qty + ReDetail.Quantity;
                        }

                        BusinessHelper.Instance.EditEntity(inv);

                    }
                    else
                    {
                        _unitOfWorkManage.RollbackTran();
                        //这里都是采购入库退货了。必须是入过库有数据的
                        throw new Exception($"当前仓库{ReDetail.Location_ID}无产品{ReDetail.ProdDetailID}的库存数据,请联系管理员");
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
                    // inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;
                    #endregion

                    invUpdateList.Add(inv);
                }
                int InvUpdateCounter = await _unitOfWorkManage.GetDbClient().Updateable(invUpdateList).ExecuteCommandAsync();
                if (InvUpdateCounter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("采购退货审核时，库存更新失败！");
                }
                //入库退回 写回入库单明细,审核用加，
                if (entity.tb_purentry != null)
                {
                    if (entity.tb_purentry.tb_PurEntryDetails == null)
                    {
                        entity.tb_purentry.tb_PurEntryDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>()
                            //.Includes(a=>a.tb_purentry,b=>b.tb_purorder)
                            .Where(c => c.PurEntryID == entity.tb_purentry.PurEntryID).ToListAsync();
                    }

                    //循环入库明细。更新入库明细中的对应的退回数量
                    for (int i = 0; i < entity.tb_purentry.tb_PurEntryDetails.Count; i++)
                    {
                        var EntryDetail = entity.tb_purentry.tb_PurEntryDetails[i];
                        tb_PurEntryReDetail ReturnDetail = entity.tb_PurEntryReDetails
                            .Where(c => c.ProdDetailID == EntryDetail.ProdDetailID && c.Location_ID == EntryDetail.Location_ID).FirstOrDefault();
                        if (ReturnDetail == null)
                        {
                            continue;
                        }
                        EntryDetail.ReturnedQty += ReturnDetail.Quantity;
                        //如果已退数量大于订单数量 给出警告实际操作中 使用其他方式出库
                        if (EntryDetail.ReturnedQty > entity.tb_purentry.TotalQty)
                        {
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception("退回数量不能大于对应入库数量！");
                        }
                    }
                    //更新已退回数量
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purentry.tb_PurEntryDetails)
                        .UpdateColumns(t => t.ReturnedQty)
                        .ExecuteCommandAsync();
                }


                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    if (entity.ProcessWay == (int)PurReProcessWay.厂商退款)
                    {
                        //处理财务数据 
                        #region 采购入库退货 财务处理 不管什么情况都是生成红字应付【金额为负】生成红冲负向应付款单
                        //如果是有出库情况，则反冲。如果是没有出库情况。则生成付款单
                        //退货单审核后生成红字应收单（负金额）
                        var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                        ReturnMainSubResults<tb_FM_ReceivablePayable> results = await ctrpayable.CreateReceivablePayable(entity);
                        if (results.Succeeded)
                        {
                            //下面冲销逻辑应该放到付款的审核时处理
                            /*
                            tb_FM_ReceivablePayable returnpayable = results.ReturnObject;
                            //如果这个供应商要退款给我们，则去找这个供应商名下是否还有应付款。找到所有的。倒算自动红冲 余额。抵消
                            var PositivePayableList = await ctrpayable.BaseQueryByWhereAsync(c => c.CustomerVendor_ID == returnpayable.CustomerVendor_ID
                            && (c.ARAPStatus == (long)ARAPStatus.已生效 || c.ARAPStatus == (long)ARAPStatus.部分支付)
                            &&
                            (c.LocalBalanceAmount > 0 && c.ForeignBalanceAmount > 0)
                            );
                            PositivePayableList = PositivePayableList.OrderBy(c => c.Created_at.Value).ToList();

                            for (int i = 0; i < PositivePayableList.Count; i++)
                            {
                                var OriginalPayable = PositivePayableList[i];
                                #region 如果原始应付没有核销收款，则直接生成 红字应付核销

                                //判断 如果出库的应收金额和未核销余额一样。说明 客户还没有支付任何款，则可以直接全额红冲
                                OriginalPayable.LocalBalanceAmount -= entity.TotalAmount;
                                //-500 加上退款500 应该为0
                                returnpayable.LocalBalanceAmount += entity.TotalAmount;

                                OriginalPayable.ForeignBalanceAmount -= entity.ForeignTotalAmount;
                                //-500 加上退款500 应该为0
                                returnpayable.ForeignBalanceAmount += entity.ForeignTotalAmount;
                                OriginalPayable.ARAPStatus = (long)ARAPStatus.已冲销;
                                returnpayable.ARAPStatus = (long)ARAPStatus.已冲销;
                                //生成核销记录证明正负抵消应收应付
                                //生成一笔核销记录  应收红冲
                                var settlementController = _appContext.GetRequiredService<tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement>>();
                                await settlementController.GenerateSettlement(OriginalPayable, returnpayable);

                                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(OriginalPayable).ExecuteCommandAsync();
                                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(returnpayable).ExecuteCommandAsync();

                                #endregion
                            }

                            if (Math.Abs(returnpayable.LocalBalanceAmount) > 0 || Math.Abs(returnpayable.ForeignBalanceAmount) > 0)
                            {
                                var paymentController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                                #region 通过负数的应收，生成退款单

                                //找到支付记录
                                //通过负数的应收，生成退款单
                                await paymentController.CreatePaymentRecord(new List<tb_FM_ReceivablePayable> { returnpayable }, false);
                                //退款单生成成功等待 财务审核
                                #endregion

                            }
                            else
                            {
                                entity.PayStatus = (int)PayStatus.全部付款;
                            }
                            */
                            //财务审核应收红单后 看如何核销
                            /*
                              生成退款单	- 手动录入：选择退款方式（现金/原支付渠道）	生成 退款单（RefundMaster），金额为退货金额，关联原收款单	退款单状态：草稿 → 财务审核 → 已审核
                                                    5. 更新资金账户	- 现金退款：直接减少现金账户余额
                                                    - 原渠道退款：生成红字收款单并关联银行流水	更新 PaymentDetail 或生成红字收款单（IsRed=1）	现金账户余额减少
                                                    红字收款单状态为 已审核

                             */


                        }
                        await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(t => t.ApprovalResults).ExecuteCommandAsync();
                        #endregion
                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;

                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryRe>(entity).ExecuteCommandAsync();
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
            tb_PurEntryRe entity = ObjectEntity as tb_PurEntryRe;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            try
            {
                //判断是否能反审? 意思是。我这个入库单错了。但是你都当入库成功进行了后面的操作了，现在要反审，那肯定不行。所以，要判断，
                if (entity.tb_PurReturnEntries != null
                    && (entity.tb_PurReturnEntries.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_PurReturnEntries.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {
                    rs.ErrorMsg = "存在已确认或已完结，或已审核的采购退回入库单，不能反审采购退回单。 ";
                    return rs;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();


                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();

                foreach (var child in entity.tb_PurEntryReDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //实际 期初已经有数据了，则要

                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv != null)
                    {

                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    else
                    {
                        //不应该为空
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception(child.ProdDetailID + "期初库存不应该为空.");
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestStorageTime = System.DateTime.Now;

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
                    CommService.CostCalculations.AntiCostCalculation(_appContext, inv, child.Quantity, child.UnitPrice);
                    inv.Quantity = inv.Quantity + child.Quantity;
                    if (entity.ProcessWay == (int)PurReProcessWay.需要返回)
                    {
                        inv.On_the_way_Qty = inv.On_the_way_Qty - child.Quantity;
                    }

                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestOutboundTime = System.DateTime.Now;
                    #endregion
                    invUpdateList.Add(inv);
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (Counter.ToInt() == 0)
                {
                    _logger.LogInformation($"{entity.PurEntryReNo}更新库存结果为0行，请检查数据！");
                }


                //入库退回 写回入库单明细，
                if (entity.tb_purentry != null)
                {
                    if (entity.tb_purentry.tb_PurEntryDetails == null)
                    {
                        entity.tb_purentry.tb_PurEntryDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>().Where(c => c.PurEntryID == entity.tb_purentry.PurEntryID).ToListAsync();
                    }
                    //不用foreach
                    for (int i = 0; i < entity.tb_purentry.tb_PurEntryDetails.Count; i++)
                    {
                        tb_PurEntryDetail entryDetail = entity.tb_purentry.tb_PurEntryDetails[i];
                        tb_PurEntryReDetail ReturnDetail = entity.tb_PurEntryReDetails
                            .Where(c => c.ProdDetailID == entryDetail.ProdDetailID && c.Location_ID == entryDetail.Location_ID).FirstOrDefault();
                        if (ReturnDetail == null)
                        {
                            continue;
                        }
                        entryDetail.ReturnedQty -= ReturnDetail.Quantity;
                        //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                        if (entryDetail.ReturnedQty < 0)
                        {
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception("已退回数量不能小于0！");
                        }
                    }
                    //更新已退回数量
                    int ReturnQtyUpdaterCounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purentry.tb_PurEntryDetails).ExecuteCommandAsync();
                    if (ReturnQtyUpdaterCounter > 0)
                    {

                    }
                }
                //===============
                //也写回采购订单明细
                //退回流程不算入采购订单的已交数量
                //退回售后流程是一个独立的。与入库明细已交挂够。观察售后回来情况
                //采购订单。入库时回写已交数量 是观察订单情况。两个要分开处理



                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    if (entity.ProcessWay == (int)PurReProcessWay.厂商退款)
                    {
                        //处理财务数据 退货退货
                        #region 
                        var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                        //反向应付 如果没核销则直接删除 一个ID只会一次应收付。收款可以分开多次。
                        //被冲销了，无法反审了。实在要反向。就业务上反向
                        tb_FM_ReceivablePayable returnpayable = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                               .Where(c => c.CustomerVendor_ID == entity.CustomerVendor_ID
                                                && c.SourceBizType == (int)BizType. 采购退货单 && c.SourceBillId == entity.PurEntryRe_ID
                                                ).SingleAsync();
                        if (returnpayable != null)
                        {
                            if (returnpayable.ARAPStatus == (int)ARAPStatus.草稿
                                || returnpayable.ARAPStatus == (int)ARAPStatus.待审核 || returnpayable.ARAPStatus == (int)ARAPStatus.已生效)
                            {
                                //删除
                                //bool deleters = await ctrpayable.BaseDeleteByNavAsync(returnpayable);
                                bool deleters = await ctrpayable.BaseDeleteAsync(returnpayable);
                            }
                            else
                            {
                                _unitOfWorkManage.RollbackTran();
                                rs.ErrorMsg = $"采购退货单：{entity.PurEntryReNo}中,对应的应收红冲数据已经生效。无法反审核！";
                                rs.Succeeded = false;
                                return rs;
                            }
                        }

                        #endregion
                    }
                }


                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "被反审核";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryRe>(entity).UpdateColumns(it => new
                {
                    it.DataStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
                    it.ApprovalResults,
                    it.ApprovalStatus
                }).ExecuteCommandAsync();
                // await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryRe>(entity).ExecuteCommandAsync();

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


        /// <summary>
        /// 批量结案  销售出库标记结案，数据状态为8,可以修改付款状态，同时检测销售订单的付款状态，也可以更新销售订单付款状态
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案就是一个财务确认过程。 如：返厂后不用退回的。扣货款的。则可以直接结案。
        /// 结案时，如果引用了入库单，则要更新入库和他对应的采购单为结案状态。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_PurEntryRe> entitys = new List<tb_PurEntryRe>();
            entitys = NeedCloseCaseList as List<tb_PurEntryRe>;
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
                        if (entity.PurEntryID.HasValue)
                        {
                            if (entity.tb_purentry == null)
                            {
                                entity.tb_purentry = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                                    .Includes(a => a.tb_purorder)
                                    .Where(c => c.PurEntryID == entity.PurEntryID.Value)
                                    .SingleAsync();
                            }
                            if (entity.tb_purentry.DataStatus == (int)DataStatus.确认)
                            {
                                entity.tb_purentry.DataStatus = (int)DataStatus.完结;
                            }

                            BusinessHelper.Instance.EditEntity(entity);
                            if (entity.tb_purentry.tb_purorder == null && entity.tb_purentry.PurOrder_ID.HasValue)
                            {
                                entity.tb_purentry.tb_purorder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                                   .Where(c => c.PurOrder_ID == entity.tb_purentry.PurOrder_ID.Value)
                                   .SingleAsync();
                            }
                            if (entity.tb_purentry.tb_purorder.DataStatus == (int)DataStatus.确认)
                            {
                                entity.tb_purentry.tb_purorder.DataStatus = (int)DataStatus.完结;
                            }
                            entity.tb_purentry.tb_purorder.CloseCaseOpinions = "由采购退货单关联式结案";
                            BusinessHelper.Instance.EditEntity(entity);
                            var affectedPORows = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(entity).UpdateColumns(it => new
                            {
                                it.DataStatus,
                                it.CloseCaseOpinions,
                                it.Modified_by,
                                it.Modified_at
                            }).ExecuteCommandAsync();

                            var affectedPERows = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntry>(entity).UpdateColumns(it => new
                            {
                                it.DataStatus,
                                it.Modified_by,
                                it.Modified_at
                            }).ExecuteCommandAsync();

                        }

                        entity.DataStatus = (int)DataStatus.完结;
                        BusinessHelper.Instance.EditEntity(entity);
                        //只更新指定列
                        var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryRe>(entity).UpdateColumns(it => new
                        {
                            it.DataStatus,
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




        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            //var queryable = _appContext.Db.Queryable<tb_SaleOrderDetail>();
            //var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();
            List<tb_PurEntryRe> list = await _appContext.Db.CopyNew().Queryable<tb_PurEntryRe>().Where(m => m.PurEntryRe_ID == MainID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_PurEntryReDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }


    }
}



