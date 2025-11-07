
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
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using RUINORERP.Business.CommService;
using System.Collections;

namespace RUINORERP.Business
{
    public partial class tb_PurReturnEntryController<T>
    {


        /// <summary>
        /// 返回批量审核的结果
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_PurReturnEntry entity = ObjectEntity as tb_PurReturnEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();


                //处理采购退货单
                entity.tb_purentryre = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryRe>()
                     .Includes(a => a.tb_PurReturnEntries, b => b.tb_PurReturnEntryDetails)
                     .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                     .Includes(a => a.tb_PurEntryReDetails, b => b.tb_proddetail, c => c.tb_prod)
                     .Where(c => c.PurEntryRe_ID == entity.PurEntryRe_ID)
                     .Single();

                if (entity.tb_purentryre == null)
                {
                    rs.ErrorMsg = $"没有找到对应的采购退货单!请检查数据后重试！";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }
                //如果采购入库的供应商和这里采购退货的供应商不相同，要提示
                if (entity.CustomerVendor_ID != entity.tb_purentryre.CustomerVendor_ID)
                {
                    rs.Succeeded = false;
                    rs.ErrorMsg = $"采购退回后的入库单的供应商和采购入库退货时的供应商不同!请检查数据后重试！";
                    return rs;
                }
                //如果入库明细中的产品。不存在于采购退货单中。审核失败。
                foreach (var child in entity.tb_PurReturnEntryDetails)
                {
                    if (!entity.is_force_offset && !entity.tb_purentryre.tb_PurEntryReDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID))
                    {
                        rs.Succeeded = false;
                        _unitOfWorkManage.RollbackTran();
                        rs.ErrorMsg = $"入库明细中有产品不属于采购退货单!请检查数据后重试！";
                        return rs;
                    }
                }

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();

                foreach (tb_PurReturnEntryDetail child in entity.tb_PurReturnEntryDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，

                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rs.ErrorMsg = $"{child.ProdDetailID}当前产品无库存数据，无法进行采购退货入库。请使用【期初盘点】【采购入库】】【生产缴库】的方式进行盘点后，再操作。";
                        rs.Succeeded = false;
                        return rs;
                    }
                    else
                    {
                        inv.Quantity = inv.Quantity + child.Quantity;
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.LatestStorageTime = System.DateTime.Now;

                    //采购退货单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty - child.Quantity;

                    //采购退货入库暂时不影响成本
                    decimal UntaxedUnitPrice = child.UnitPrice / (1 + child.TaxRate);
                    UntaxedUnitPrice = Math.Round(UntaxedUnitPrice, 3);
                    if (child.IsGift.HasValue && !child.IsGift.Value && UntaxedUnitPrice > 0)
                    {
                        CommService.CostCalculations.CostCalculation(_appContext, inv, child.Quantity.ToInt(), UntaxedUnitPrice, 0);

                        var ctrbom = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
                        // 递归更新所有上级BOM的成本
                        await ctrbom.UpdateParentBOMsAsync(child.ProdDetailID, inv.Inv_Cost);
                    }

                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;
                    #endregion
                    invUpdateList.Add(inv);

                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (Counter == 0)
                {
                    _logger.Debug($"{entity.PurReEntryNo}更新库存结果为0行，请检查数据！");
                }



                //因为可以多次分批入库，所以需要判断当前入库数量是否大于退货数量
                //先找到所有采购退货入库明细,再找按采购退款明细去循环比较。如果入库总数量大于退货数量，则不允许入库。
                List<tb_PurReturnEntryDetail> detailList = new List<tb_PurReturnEntryDetail>();
                foreach (var item in entity.tb_purentryre.tb_PurReturnEntries)
                {
                    detailList.AddRange(item.tb_PurReturnEntryDetails);
                }

                //分两种情况处理。
                for (int i = 0; i < entity.tb_purentryre.tb_PurEntryReDetails.Count; i++)
                {
                    //如果当前采购退款明细行，不存在于采购退货入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较
                    string prodName = entity.tb_purentryre.tb_PurEntryReDetails[i].tb_proddetail.tb_prod.CNName +
                              entity.tb_purentryre.tb_PurEntryReDetails[i].tb_proddetail.tb_prod.Specifications;
                    //明细中有相同的产品或物品。
                    var aa = entity.tb_purentryre.tb_PurEntryReDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID > 0)
                    {
                        #region 如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                        if (entity.tb_PurReturnEntryDetails.Any(c => c.PurReEntry_CID == 0))
                        {
                            //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            string msg = $"采购退货单:{entity.tb_purentryre.PurEntryReNo}的【{prodName}】在明细中拥有多行记录，必须使用引用的方式添加。";
                            rs.ErrorMsg = msg;
                            _unitOfWorkManage.RollbackTran();
                            _logger.Debug(msg);
                            return rs;
                        }
                        #endregion

                        var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                        && c.PurEntryRe_CID == entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID).Sum(c => c.Quantity);
                        if (inQty > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity && !entity.is_force_offset)
                        {
                            string msg = $"【{prodName}】的采购退货入库数量不能大于退货单中对应行的数量\r\n" + $"或存在针对当前采购退货单重复录入了采购退货入库单。";
                            rs.ErrorMsg = msg;
                            _unitOfWorkManage.RollbackTran();
                            _logger.Debug(msg);
                            return rs;
                        }
                        else
                        {
                            var RowQty = entity.tb_PurReturnEntryDetails
                                .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                                && c.PurEntryRe_CID == entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID
                                 && c.Location_ID == entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                            //算出交付的数量
                            entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity += RowQty;
                            //如果已交数据大于 退货单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity && !entity.is_force_offset)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"采购退货入库：{entity.PurReEntryNo}审核时，对应的采购退货单：{entity.tb_purentryre.PurEntryReNo}中，入库总数量不能大于退货数量！");
                            }
                        }
                    }
                    else
                    {
                        //一对一时
                        var inQty = detailList
                            .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                         && c.Location_ID == entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                        ).Sum(c => c.Quantity);
                        if (inQty > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity && !entity.is_force_offset)
                        {
                            string msg = $"采购退回入库单:{entity.PurReEntryNo}的【{prodName}】的入库数量不能大于【采购退货单】中对应行的数量\r\n" + $"或存在针对当前采购退货单重复录入了采购入库单。";
                            rs.ErrorMsg = msg;
                            _unitOfWorkManage.RollbackTran();
                            _logger.Debug(msg);
                            return rs;
                        }
                        else
                        {
                            //当前行累计到交付
                            var RowQty = entity.tb_PurReturnEntryDetails
                                .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                                && c.Location_ID == entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                            entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity += RowQty;
                            //如果已交数据大于 退货单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity && !entity.is_force_offset)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"采购退回入库单：{entity.PurReEntryNo}审核时，对应的退货单：{entity.tb_purentryre.PurEntryReNo}，入库总数量不能大于退货单数量！");
                            }
                        }
                    }
                }

                //更新已交数量
                int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryReDetail>(entity.tb_purentryre.tb_PurEntryReDetails)
                    .UpdateColumns(it => new { it.DeliveredQuantity }).ExecuteCommandAsync();
                if (poCounter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Debug(entity.PurReEntryNo + "==>" + entity.PurEntryReNo + $"对应 的退货单更新成功===重点代码 看已交数量是否正确");
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

                //采购入库单，如果来自于采购退货单，则要把入库数量累加到退货单中的已交数量 TODO 销售也会有这种情况
                if (entity.tb_purentryre != null && entity.tb_purentryre.DataStatus == (int)DataStatus.确认 && (entity.TotalQty == entity.tb_purentryre.TotalQty || entity.tb_purentryre.tb_PurEntryReDetails.Sum(c => c.DeliveredQuantity) == entity.tb_purentryre.TotalQty))
                {
                    entity.tb_purentryre.DataStatus = (int)DataStatus.完结;
                    // entity.tb_purentryre.ApprovalOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "审核入库单:" + entity.PurReEntryNo + "结案。"; ;
                    int poendcounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purentryre)
                        .UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                    if (poendcounter > 0)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Info(entity.tb_purentryre.PurEntryReNo + "==>" + "结案状态更新成功");
                        }
                    }
                }

                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    try
                    {
                        #region 生成应付 ,这里的应付不从预付中抵扣了，只会去用这个蓝字应付去冲销退货时生成的红字应付
                        var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                        tb_FM_ReceivablePayable Payable =await ctrpayable.BuildReceivablePayable(entity);
                        ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(Payable, false);
                        if (rmr.Succeeded)
                        {
                            //已经是等审核。 审核时会核销预收付款
                            rs.ReturnObjectAsOtherEntity = rmr.ReturnObject;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("采购退回入库时，财务数据处理失败，审核失败！");
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
                _logger.Error(ex, RUINORERP.Business.BizMapperService.EntityDataExtractor.ExtractDataContent(entity));
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
            tb_PurReturnEntry entity = ObjectEntity as tb_PurReturnEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            try
            {

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_PurReturnEntryDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //实际 期初已经有数据了，则要
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        inv = new tb_Inventory();
                        inv.Quantity = inv.Quantity - child.Quantity;
                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "";//后面修改数据库是不需要？
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    else
                    {
                        inv.Quantity = inv.Quantity - child.Quantity;
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestStorageTime = System.DateTime.Now;

                    //采购退货单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty + child.Quantity;

                    //这个业务暂时不处理成本

                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestOutboundTime = System.DateTime.Now;

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
                var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvUpdateCounter == 0)
                {
                    _logger.Debug($"{entity.PurReEntryNo}更新库存结果为0行，请检查数据！");
                }



                if (entity.tb_purentryre != null)
                {
                    #region  反审检测写回 退回

                    //处理采购退货单
                    entity.tb_purentryre = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryRe>()
                         .Includes(a => a.tb_PurReturnEntries, b => b.tb_PurReturnEntryDetails)
                         .Includes(t => t.tb_PurEntryReDetails)
                         .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                         .Includes(a => a.tb_PurEntryReDetails, b => b.tb_proddetail, c => c.tb_prod)
                         .Where(c => c.PurEntryRe_ID == entity.PurEntryRe_ID)
                         .Single();


                    //先找到所有入库明细,再找按退货单明细去循环比较。如果入库总数量大于退货单数量，则不允许入库。
                    List<tb_PurReturnEntryDetail> detailList = new List<tb_PurReturnEntryDetail>();
                    foreach (var item in entity.tb_purentryre.tb_PurReturnEntries)
                    {
                        detailList.AddRange(item.tb_PurReturnEntryDetails);
                    }

                    //分两种情况处理。
                    for (int i = 0; i < entity.tb_purentryre.tb_PurEntryReDetails.Count; i++)
                    {
                        //如果当前退货单明细行，不存在于入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较


                        string prodName = entity.tb_purentryre.tb_PurEntryReDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_purentryre.tb_PurEntryReDetails[i].tb_proddetail.tb_prod.Specifications;
                        //明细中有相同的产品或物品。
                        var aa = entity.tb_purentryre.tb_PurEntryReDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0 && entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID > 0)
                        {
                            #region //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            if (entity.tb_PurReturnEntryDetails.Any(c => c.PurEntryRe_CID == 0))
                            {
                                //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                                string msg = $"采购退货单:{entity.tb_purentryre.PurEntryReNo}的【{prodName}】在退货单明细中拥有多行记录，必须使用引用的方式添加，反审失败！";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.Debug(msg);
                                }
                                return rs;
                            }
                            #endregion

                            var inQty = detailList
                                .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                                             && c.Location_ID == entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                                && c.PurEntryRe_CID == entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity)
                            {
                                string msg = $"采购退货单:{entity.tb_purentryre.PurEntryReNo}的【{prodName}】的入库数量不能大于退货单中对应行的数量。";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.Debug(msg);
                                }
                                return rs;
                            }
                            else
                            {
                                var RowQty = entity.tb_PurReturnEntryDetails
                                    .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                                    && c.PurEntryRe_CID == entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID
                                    && c.Location_ID == entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID

                                    ).Sum(c => c.Quantity);
                                //算出交付的数量
                                entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity -= RowQty;
                                //如果已交数据大于 退货单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.PurReEntryNo}反审核时，对应的退货单：{entity.tb_purentryre.PurEntryReNo}，{prodName}的明细不能为负数！"); throw new Exception($"入库单：{entity.PurReEntryNo}审核时，对应的退货单：{entity.tb_purentryre.PurEntryReNo}，入库总数量不能大于退货单数量！");
                                }
                            }
                        }
                        else
                        {
                            //一对一时
                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity)
                            {

                                string msg = $"采购退货单:{entity.tb_purentryre.PurEntryReNo}的【{prodName}】的入库数量不能大于退货单中对应行的数量。";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.Debug(msg);
                                }
                                return rs;
                            }
                            else
                            {
                                //当前行累计到交付
                                var RowQty = entity.tb_PurReturnEntryDetails
                                    .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                                    && c.Location_ID == entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                                    ).Sum(c => c.Quantity);
                                entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity -= RowQty;
                                //如果已交数据大于 退货单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.PurReEntryNo}反审核时，对应的退货单：{entity.tb_purentryre.PurEntryReNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                    }


                    #endregion

                    //更新已交数量
                    int updatecounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryReDetail>(entity.tb_purentryre.tb_PurEntryReDetails).ExecuteCommandAsync();
                    if (updatecounter == 0)
                    {

                    }

                }
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "被反审核";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                            .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                            .ExecuteCommandHasChangeAsync();

                //采购入库单，如果来自于采购退货单，则要把入库数量累加到退货单中的已交数量
                if (entity.tb_purentryre != null && entity.tb_purentryre.TotalQty != entity.tb_purentryre.tb_PurEntryReDetails.Sum(c => c.DeliveredQuantity))
                {
                    entity.tb_purentryre.DataStatus = (int)DataStatus.确认;
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purentryre).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
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
            List<tb_PurReturnEntry> list = await _appContext.Db.CopyNew().Queryable<tb_PurReturnEntry>().Where(m => m.PurReEntry_ID == MainID)
                             .Includes(a => a.tb_customervendor)
                                .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_purentryre)
                          .Includes(a => a.tb_department)
                           .Includes(a => a.tb_PurReturnEntryDetails, c => c.tb_location)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_PurReturnEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                    .Includes(a => a.tb_PurReturnEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                                 .ToListAsync();
            return list as List<T>;
        }





    }
}



