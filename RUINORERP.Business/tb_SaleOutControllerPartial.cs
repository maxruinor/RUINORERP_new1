
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
using System.Windows.Forms;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt.CRM;
using System.Runtime.InteropServices.ComTypes;
using RUINORERP.Business.CommService;
using AutoMapper;
using RUINORERP.Global.EnumExt;
using Castle.Core.Resource;
using SharpYaml.Tokens;


namespace RUINORERP.Business
{

    public partial class tb_SaleOutController<T>
    {
        public async Task<ReturnResults<tb_SaleOutRe>> RefundProcessAsync(tb_SaleOut saleout)
        {
            ReturnResults<tb_SaleOutRe> rs = new ReturnResults<tb_SaleOutRe>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_SaleOutRe entity = new tb_SaleOutRe();
                //转单
                if (saleout != null)
                {
                    entity = mapper.Map<tb_SaleOutRe>(saleout);
                    entity.ApprovalOpinions = "平台退款时预转单";
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
                    //退货时 默认不写付款情况，实际是有些平台的。会提前退？线下是先退再付款
                    if (saleout.TotalAmount == entity.TotalAmount)
                    {
                        entity.PayStatus = (int)PayStatus.全部付款;
                    }
                    else
                    {
                        entity.PayStatus = (int)PayStatus.部分付款;
                    }
                    entity.Paytype_ID = null;
                    entity.RefundStatus = (int)RefundStatus.已退款等待退货;
                    List<string> tipsMsg = new List<string>();
                    List<tb_SaleOutReDetail> details = mapper.Map<List<tb_SaleOutReDetail>>(saleout.tb_SaleOutDetails);
                    List<tb_SaleOutReDetail> NewDetails = new List<tb_SaleOutReDetail>();

                    for (global::System.Int32 i = 0; i < details.Count; i++)
                    {
                        var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0 && details[i].SaleOutDetail_ID > 0)
                        {
                            #region 产品ID可能大于1行，共用料号情况
                            tb_SaleOutDetail item = saleout.tb_SaleOutDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                            && c.Location_ID == details[i].Location_ID
                            && c.SaleOutDetail_ID == details[i].SaleOutDetail_ID);
                            details[i].Cost = item.Cost;
                            details[i].CustomizedCost = item.CustomizedCost;
                            //这时有一种情况就是订单时没有成本。没有产品。出库前有类似采购入库确定的成本
                            if (details[i].Cost == 0)
                            {
                                View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                                if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                                {
                                    if (obj.Inv_Cost != null)
                                    {
                                        details[i].Cost = obj.Inv_Cost.Value;
                                    }
                                }
                            }
                            details[i].Quantity = item.Quantity - item.TotalReturnedQty;// 已经出数量去掉
                            details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                            details[i].SubtotalCostAmount = (details[i].Cost + details[i].CustomizedCost) * details[i].Quantity;
                            if (details[i].Quantity > 0)
                            {
                                NewDetails.Add(details[i]);
                            }
                            else
                            {
                                tipsMsg.Add($"销售出库单{saleout.SaleOutNo}，{item.tb_proddetail.tb_prod.CNName + item.tb_proddetail.tb_prod.Specifications}已退回数为{item.TotalReturnedQty}，可退库数为{details[i].Quantity}，当前行数据忽略！");
                            }

                            #endregion
                        }
                        else
                        {
                            #region 每行产品ID唯一
                            tb_SaleOutDetail item = saleout.tb_SaleOutDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                              && c.Location_ID == details[i].Location_ID
                            && c.SaleOutDetail_ID == details[i].SaleOutDetail_ID);
                            details[i].Cost = item.Cost;
                            details[i].CustomizedCost = item.CustomizedCost;
                            //这时有一种情况就是订单时没有成本。没有产品。出库前有类似采购入库确定的成本
                            if (details[i].Cost == 0)
                            {
                                View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                                if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                                {
                                    if (obj.Inv_Cost == null)
                                    {
                                        obj.Inv_Cost = 0;
                                    }
                                    details[i].Cost = obj.Inv_Cost.Value;
                                }
                            }
                            details[i].Quantity = details[i].Quantity - item.TotalReturnedQty;// 减掉已经出库的数量
                            details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                            details[i].SubtotalCostAmount = (details[i].Cost + details[i].CustomizedCost) * details[i].Quantity;

                            if (details[i].Quantity > 0)
                            {
                                NewDetails.Add(details[i]);
                            }
                            else
                            {
                                tipsMsg.Add($"当前订单的SKU:{item.tb_proddetail.SKU}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                            }
                            #endregion
                        }
                    }

                    if (NewDetails.Count == 0)
                    {
                        tipsMsg.Add($"出库单:{entity.SaleOut_NO}已全部退库，请检查是否正在重复退库！");
                    }

                    entity.tb_SaleOutReDetails = NewDetails;

                    //如果这个订单已经有出库单 则第二次运费为0
                    if (saleout.tb_SaleOutRes != null && saleout.tb_SaleOutRes.Count > 0)
                    {
                        if (saleout.FreightIncome > 0)
                        {
                            tipsMsg.Add($"当前出库单已经有退库记录，运费收入退回已经计入前面退库单，当前退库运费收入退回为零！");
                            entity.FreightIncome = 0;
                        }
                        else
                        {
                            tipsMsg.Add($"当前出库单已经有退库记录！");
                        }
                    }

                    entity.ReturnDate = System.DateTime.Now;

                    BusinessHelper.Instance.InitEntity(entity);
                    if (entity.SaleOut_MainID.HasValue && entity.SaleOut_MainID > 0)
                    {
                        entity.CustomerVendor_ID = saleout.CustomerVendor_ID;
                        entity.SaleOut_NO = saleout.SaleOutNo;
                        entity.IsFromPlatform = saleout.IsFromPlatform;
                    }
                    entity.ReturnNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售退回单);
                    entity.tb_saleout = saleout;
                    entity.TotalQty = NewDetails.Sum(c => c.Quantity);

                    entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                    entity.TotalAmount = entity.TotalAmount + entity.FreightIncome;
                    BusinessHelper.Instance.InitEntity(entity);

                }


                var reControl = _appContext.GetRequiredService<tb_SaleOutReController<tb_SaleOutRe>>();
                var SaveRs = await reControl.BaseSaveOrUpdateWithChild<tb_SaleOutRe>(entity); //保存退库单
                if (SaveRs.Succeeded)
                {
                    rs.ReturnObject = SaveRs.ReturnObject;
                    saleout.RefundStatus = (int)RefundStatus.已退款等待退货;
                    var last = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(saleout).UpdateColumns(it => new
                    {
                        it.RefundStatus
                    }).ExecuteCommandAsync();
                }

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
        /// 批量结案  销售出库标记结案，数据状态为8,可以修改付款状态，同时检测销售订单的付款状态，也可以更新销售订单付款状态
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案就是一个财务确认过程
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_SaleOut> entitys = new List<tb_SaleOut>();
            entitys = NeedCloseCaseList as List<tb_SaleOut>;
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
                        //只修改订单的付款状态
                        if (entity.tb_saleorder.TotalQty == entity.tb_SaleOutDetails.Sum(c => c.Quantity))
                        {
                            entity.tb_saleorder.PayStatus = entity.PayStatus.Value;
                        }
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity.tb_saleorder).ExecuteCommandAsync();

                        //这部分是否能提出到上一级公共部分？
                        entity.DataStatus = (int)DataStatus.完结; //强制结案
                        BusinessHelper.Instance.EditEntity(entity);
                        //只更新指定列
                        var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity)
                            .UpdateColumns(it => new { it.DataStatus, it.PayStatus, it.Paytype_ID, it.Modified_by, it.Modified_at }).ExecuteCommandAsync();

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
        /// 注意明细中  支持了 相同产品多行录入。所以要加总分组计算
        /// 审核其他出库单 注意逻辑是减少库存，并且更新单据本身状态
        /// 如果非账期则同时生成收款单
        /// 
        /// 1. 现金销售（全额收款）	- 销售出库时生成应收单（即使现金）
        //- 生成收款单并审核 → 自动核销应收单	- 应收应付表：生成应收单（状态=已审核）
        //- 收付款表：插入收款单（状态=已审核）
        //- 核销表：自动生成核销记录
        //2. 账期销售（分期收款）	- 销售出库生成应收单
        //- 分次生成收款单 → 手动核销	- 每次核销更新 应收应付表.RemainAmount
        //- 最后一次核销标记 IsFullySettled = 1

        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rrs = new ReturnResults<T>();
            tb_SaleOut entity = ObjectEntity as tb_SaleOut;
            try
            {
                if (entity == null)
                {
                    return rrs;
                }
                if (!entity.ApprovalResults.HasValue || !entity.ApprovalResults.Value)
                {
                    rrs.ErrorMsg = $"无审核数据!请刷新重试！";
                    rrs.Succeeded = false;
                    return rrs;
                }
                //当第一次审核失败后，Undo值是没有把tb_SaleOutDetails保存回去。导致 为null  暂时先查一下。后面优化尝试复制，恢复
                if (entity.tb_SaleOutDetails == null)
                {
                    entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                        .Includes(c => c.tb_SaleOutDetails)
                        .Where(d => d.SaleOut_MainID == entity.SaleOut_MainID).FirstAsync();
                }


                //采购入库总数量和明细求和检查
                if (entity.TotalQty.Equals(entity.tb_SaleOutDetails.Sum(c => c.Quantity)) == false)
                {
                    rrs.ErrorMsg = $"销售出库数量与明细之和不相等!请检查数据后重试！";
                    rrs.Succeeded = false;
                    return rrs;
                }
                var ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                //这里设置一个集合 用于保存特殊情况，后面统一更新
                List<tb_SaleOutDetail> UpdateSaleOutCostlist = new List<tb_SaleOutDetail>();

                #region 审核 通过时
                if (entity.ApprovalResults.HasValue && entity.ApprovalResults.Value)
                {
                    entity.tb_saleorder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                        .Includes(a => a.tb_paymentmethod)
                        .Includes(a => a.tb_projectgroup, b => b.tb_department)
                        .Includes(a => a.tb_SaleOuts, b => b.tb_SaleOutDetails)
                        .Includes(a => a.tb_customervendor, b => b.tb_crm_customer, c => c.tb_crm_leads)
                        .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                      .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                        .Where(c => c.SOrder_ID == entity.SOrder_ID).SingleAsync();


                    //如果采购订单的供应商和这里入库的供应商不相同，要提示
                    if (entity.CustomerVendor_ID != entity.tb_saleorder.CustomerVendor_ID)
                    {
                        rrs.Succeeded = false;
                        rrs.ErrorMsg = $"销售出库单的客户和销售订单的客户不同!请检查数据后重试！";
                        return rrs;
                    }


                    // 检查销售订单状态是否为已确认
                    bool isOrderConfirmed = entity.tb_saleorder.DataStatus == (int)DataStatus.确认;
                    bool isApproved = entity.tb_saleorder.ApprovalResults.HasValue &&
                                        entity.tb_saleorder.ApprovalResults.Value;
                    // 销售订单非确认状态时阻止出库审核
                    if (!isOrderConfirmed || !isApproved)
                    {
                        rrs.Succeeded = false;
                        rrs.ErrorMsg = $" 销售订单{entity.tb_saleorder.SOrderNo}状态为【{((DataStatus)entity.tb_saleorder.DataStatus).ToString()}】\r\n请确认状态为【确认】已审核，并且审核结果为已通过!\r\n" +
                            $"请检查订单状态数据是否正确，或当前为相同订单重复出库！";
                        if (_appContext.SysConfig.ShowDebugInfo)
                        {
                            _logger.LogInformation(rrs.ErrorMsg);
                        }

                        return rrs;
                    }


                    if (!entity.ReplaceOut)
                    {
                        //如果明细中的产品。不存在于订单中。审核失败。
                        foreach (var child in entity.tb_SaleOutDetails)
                        {
                            if (!entity.tb_saleorder.tb_SaleOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID))
                            {
                                rrs.Succeeded = false;

                                rrs.ErrorMsg = $"出库明细中有产品不属于当前销售订单!请检查数据后重试！";
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(rrs.ErrorMsg);
                                }
                                return rrs;
                            }
                        }
                    }
                    // 开启事务，保证数据一致性
                    _unitOfWorkManage.BeginTran();

                    // 如果成本为零时则会实时检测库存成本，以库存成本为基准。这种情况解决未知成本，提前销售的情况。 相于于订单时成本不清楚写的0。销售出库时才有成本。

                    // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
                    var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal OutQtySum, DateTime LatestOutboundTime)>();


                    List<tb_PriceRecord> priceUpdateList = new List<tb_PriceRecord>();

                    // 遍历销售订单明细，聚合数据
                    foreach (var child in entity.tb_SaleOutDetails)
                    {
                        //以产品和库位为联合主键，合并重复的产品。将数量累加
                        var key = (child.ProdDetailID, child.Location_ID);
                        decimal currentSaleQty = child.Quantity; // 假设 Sale_Qty 对应明细中的 Quantity
                        DateTime currentOutboundTime = DateTime.Now; // 每次出库更新时间

                        // 若字典中不存在该产品，初始化记录
                        if (!inventoryGroups.TryGetValue(key, out var group))
                        {
                            #region 库存表的更新 这里应该是必需有库存的数据，
                            //tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);

                            tb_Inventory inv = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>()
                                .Includes(a => a.tb_proddetail)
                                .Where(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID).FirstAsync();
                            if (inv != null)
                            {
                                BusinessHelper.Instance.EditEntity(inv);
                            }
                            else
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"当前仓库{child.Location_ID}无产品{child.ProdDetailID}的库存数据,请联系管理员");
                            }
                            // CommService.CostCalculations.CostCalculation(_appContext, inv, child.TransactionPrice);
                            //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                            //inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                            //inv.LatestOutboundTime = System.DateTime.Now;
                            #endregion

                            #region 更新销售价格历史记录
                            //注意这里的人应该是业务员的ID。销售订单的录入人，不是审核人。也不是出库单的人。

                            tb_PriceRecord priceRecord = await _unitOfWorkManage.GetDbClient().Queryable<tb_PriceRecord>()
                            .Where(c => c.Employee_ID == entity.tb_saleorder.Employee_ID && c.ProdDetailID == child.ProdDetailID
                            ).FirstAsync();
                            //如果存在则更新，否则插入
                            if (priceRecord == null)
                            {
                                priceRecord = new tb_PriceRecord();
                                priceRecord.ProdDetailID = child.ProdDetailID;
                            }
                            priceRecord.Employee_ID = entity.tb_saleorder.Employee_ID;
                            if (priceRecord.SalePrice != child.TransactionPrice)
                            {
                                priceRecord.SalePrice = child.TransactionPrice;
                                priceRecord.SaleDate = System.DateTime.Now;
                            }
                            priceUpdateList.Add(priceRecord);
                            #endregion

                            #region 实时检测库存成本，以库存成本为基准。
                            //!!!!!!!!! 更新新于2025-06-12  以仓库实时成本为准。因为 订单时，后面可能有这个产品多次入库修改成本了。并不是当时订单时的成本
                            if (!child.Cost.Equals(inv.Inv_Cost))
                            {
                                child.Cost = inv.Inv_Cost;
                                child.SubtotalCostAmount = (child.Cost + child.CustomizedCost) * child.Quantity;
                                UpdateSaleOutCostlist.Add(child);
                            }
                            #endregion

                            // 初始化分组数据
                            group = (
                                Inventory: inv,
                                OutQtySum: currentSaleQty, // 首次累加
                                LatestOutboundTime: currentOutboundTime
                            );
                            inventoryGroups[key] = group;
                        }
                        else
                        {
                            // 累加已有分组的数值字段
                            group.OutQtySum += currentSaleQty;
                            if (!_appContext.SysConfig.CheckNegativeInventory && (group.Inventory.Quantity - group.OutQtySum) < 0)
                            {
                                // rrs.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                                rrs.ErrorMsg = $"sku:{group.Inventory.tb_proddetail.SKU}库存为：{group.Inventory.Quantity}，拟销售量为：{group.OutQtySum}\r\n 系统设置不允许负库存， 请检查出库数量与库存相关数据";
                                _unitOfWorkManage.RollbackTran();
                                rrs.Succeeded = false;
                                return rrs;
                            }

                            // 取最新出库时间（若当前时间更新，则覆盖）
                            group.LatestOutboundTime = System.DateTime.Now;
                            inventoryGroups[key] = group; // 更新分组数据
                        }
                    }

                    // 处理分组数据，更新库存记录的各字段
                    List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                    foreach (var group in inventoryGroups)
                    {

                        var inv = group.Value.Inventory;
                        // 累加数值字段
                        inv.Sale_Qty -= group.Value.OutQtySum.ToInt();

                        if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - group.Value.OutQtySum.ToInt()) < 0)
                        {
                            // rrs.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                            rrs.ErrorMsg = $"sku:{inv.tb_proddetail.SKU}库存为：{inv.Quantity}，拟销售量为：{group.Value.OutQtySum}\r\n 系统设置不允许负库存， 请检查出库数量与库存相关数据";
                            _unitOfWorkManage.RollbackTran();
                            rrs.Succeeded = false;
                            return rrs;
                        }

                        inv.Quantity -= group.Value.OutQtySum.ToInt();

                        // 计算衍生字段（如总成本）
                        inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity; // 需确保 Inv_Cost 有值
                        invUpdateList.Add(inv);
                    }

                    DbHelper<tb_Inventory> InvdbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var InvCounter = await InvdbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (InvCounter == 0)
                    {
                        _logger.LogInformation($"{entity.SaleOutNo}审核时，更新库存结果为0行，请检查数据！");
                    }



                    DbHelper<tb_SaleOutDetail> dbHelper = _appContext.GetRequiredService<DbHelper<tb_SaleOutDetail>>();

                    if (UpdateSaleOutCostlist.Count > 0)
                    {

                        var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(UpdateSaleOutCostlist);
                        if (Counter == 0)
                        {
                            _logger.LogInformation($"{entity.SaleOutNo}销售出库时成本价格更新失败，更新结果为0行，请检查数据！");
                        }
                    }

                    DbHelper<tb_PriceRecord> dbHelperPrice = _appContext.GetRequiredService<DbHelper<tb_PriceRecord>>();
                    var PriceCounter = await dbHelperPrice.BaseDefaultAddElseUpdateAsync(priceUpdateList);
                    if (PriceCounter == 0)
                    {
                        _logger.LogInformation($"{entity.SaleOutNo}销售出库时价格记录更新失败，更新结果为0行，请检查数据！");
                    }

                    if (!entity.ReplaceOut)
                    {
                        #region 回写订单状态及明细数据

                        //先找到所有出库明细,再找按订单明细去循环比较。如果出库总数量大于订单数量，则不允许出库。
                        List<tb_SaleOutDetail> detailList = new List<tb_SaleOutDetail>();
                        foreach (var item in entity.tb_saleorder.tb_SaleOuts)
                        {
                            detailList.AddRange(item.tb_SaleOutDetails);
                        }

                        //分两种情况处理。
                        for (int i = 0; i < entity.tb_saleorder.tb_SaleOrderDetails.Count; i++)
                        {
                            //如果当前订单明细行，不存在于出库明细行。直接跳过。这种就是多行多品被删除时。不需要比较
                            decimal saleOutDetailCost = 0;
                            var saleOutDetail = detailList
                                .Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                            ).FirstOrDefault();
                            if (saleOutDetail != null)
                            {
                                saleOutDetailCost = saleOutDetail.Cost;
                            }

                            string prodName = entity.tb_saleorder.tb_SaleOrderDetails[i].tb_proddetail.tb_prod.CNName +
                                      entity.tb_saleorder.tb_SaleOrderDetails[i].tb_proddetail.tb_prod.Specifications;
                            //明细中有相同的产品或物品。
                            //2024-4-29 思路更新:如果订单中有相同的产品的多行情况。出库明细冗余了订单明细的行号ID，就容易分清具体行的数据
                            var aa = entity.tb_saleorder.tb_SaleOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                            if (aa.Count > 0 && entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID > 0)
                            {
                                #region 如果存在不是引用的明细,则不允许出库。这样不支持手动添加的情况。
                                if (entity.tb_saleorder.tb_SaleOrderDetails.Any(c => c.SaleOrderDetail_ID == 0))
                                {
                                    string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加。";
                                    rrs.ErrorMsg = msg;
                                    _unitOfWorkManage.RollbackTran();
                                    _logger.LogInformation(msg);
                                    return rrs;
                                }
                                #endregion

                                var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                                && c.SaleOrderDetail_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID
                                  && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                                ).Sum(c => c.Quantity);

                                if (inQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】的出库数量不能大于订单中对应行的数量，\r\n\" " +
                                        $"或存在当前销售订单重复录入了销售出库单。";
                                    rrs.ErrorMsg = msg;
                                    _logger.LogInformation(msg);
                                    return rrs;
                                }
                                else
                                {
                                    var RowQty = entity.tb_SaleOutDetails
                                        .Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                                        && c.SaleOrderDetail_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID
                                        && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                                        ).Sum(c => c.Quantity);
                                    //算出交付的数量
                                    entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty += RowQty;

                                    //如果是业务员在没有成本数据（新品口头上问了采购）就录入了订单。后面入库后销售出库时成本是正确时。要更新回去
                                    if (entity.tb_saleorder.tb_SaleOrderDetails[i].Cost == 0 && saleOutDetailCost > 0)
                                    {
                                        entity.tb_saleorder.tb_SaleOrderDetails[i].Cost = saleOutDetailCost;
                                        entity.tb_saleorder.tb_SaleOrderDetails[i].SubtotalCostAmount = (saleOutDetailCost + entity.tb_saleorder.tb_SaleOrderDetails[i].CustomizedCost) * entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity;
                                    }

                                    //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                    if (entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                                    {
                                        _unitOfWorkManage.RollbackTran();
                                        string msg = $"销售出库单：{entity.SaleOutNo}审核时，对应的订单：{entity.tb_saleorder.SOrderNo}，入库总数量不能大于订单数量！";
                                        return rrs;

                                    }
                                }
                            }
                            else
                            {
                                //一对一时，找到所有的出库明细数量总和
                                var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                                && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                                if (inQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】的出库数量不能大于订单中对应行的数量，\r\n\" " +
                                        $"或存在当前销售订单重复录入了销售出库单，审核失败！";
                                    rrs.ErrorMsg = msg;
                                    _logger.LogInformation(msg);
                                    return rrs;
                                }
                                else
                                {
                                    //当前行累计到交付，只能是当前行所以重新找到当前出库单明细的的数量
                                    var RowQty = entity.tb_SaleOutDetails.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                                    && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                                    ).Sum(c => c.Quantity);
                                    entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty += RowQty;

                                    //如果是业务员在没有成本数据（新品口头上问了采购）就录入了订单。后面入库后销售出库时成本是正确时。要更新回去
                                    if (entity.tb_saleorder.tb_SaleOrderDetails[i].Cost == 0 && saleOutDetailCost > 0)
                                    {
                                        entity.tb_saleorder.tb_SaleOrderDetails[i].Cost = saleOutDetailCost;

                                        entity.tb_saleorder.tb_SaleOrderDetails[i].SubtotalCostAmount = (saleOutDetailCost + entity.tb_saleorder.tb_SaleOrderDetails[i].CustomizedCost)
                                            * entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity;
                                    }

                                    //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                    if (entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                                    {
                                        _unitOfWorkManage.RollbackTran();
                                        string msg = $"销售出库单：{entity.SaleOutNo}审核时，【{prodName}】的出库总数量不能大于订单数量！";
                                        return rrs;

                                    }
                                }
                            }
                        }

                        //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                        if (entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) > entity.tb_saleorder.TotalQty)
                        {
                            _unitOfWorkManage.RollbackTran();
                            string msg = $"销售订单：{entity.tb_saleorder.SOrderNo}中，出库总交付数量不能大于订单数量！";
                            return rrs;
                        }

                        //更新已交数量
                        int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrderDetail>(entity.tb_saleorder.tb_SaleOrderDetails).ExecuteCommandAsync();
                        if (poCounter > 0)
                        {
                            if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                            {
                                // _logger.Debug(entity.SaleOutNo + "==>" + entity.tb_saleorder.SOrderNo + $"对应的订单更新成功===重点代码 看已交数量是否正确");
                            }
                        }

                        #endregion
                    }

                    //如果出库金额为0，但是订单中不是为0，则不允许出库,如果明细中注明了。都是赠品是可以的。
                    if (entity.TotalAmount == 0 && entity.tb_saleorder.TotalAmount != 0 && !entity.tb_SaleOutDetails.Any(c => c.Gift))
                    {
                        _unitOfWorkManage.RollbackTran();
                        string msg = $"出库单:{entity.tb_saleorder.SOrderNo}的总金额为0， 但是订单中不为0，请检查数据!";
                        rrs.ErrorMsg = msg;
                        _logger.LogInformation(msg);
                        return rrs;
                    }

                    #region CRM

                    //如果是新客户或潜在客户。则转换为首单客户
                    if (entity.tb_saleorder.tb_customervendor.tb_crm_customer != null)
                    {
                        var crm_customer = entity.tb_saleorder.tb_customervendor.tb_crm_customer;
                        if (crm_customer.CustomerStatus == (int)CustomerStatus.潜在客户 ||
                            crm_customer.CustomerStatus == (int)CustomerStatus.新增客户)
                        {
                            crm_customer.CustomerStatus = (int)CustomerStatus.首单客户;
                            crm_customer.FirstPurchaseDate = entity.OutDate;
                        }

                        //更新采购金额  结案时才算次数。
                        if (crm_customer.PurchaseCount == null)
                        {
                            crm_customer.PurchaseCount = 0;
                        }

                        if (crm_customer.TotalPurchaseAmount == null)
                        {
                            crm_customer.TotalPurchaseAmount = 0;
                        }
                        crm_customer.TotalPurchaseAmount += entity.tb_SaleOutDetails.Sum(c => c.Quantity * c.TransactionPrice);
                        crm_customer.LastPurchaseDate = entity.OutDate;
                        if (crm_customer.FirstPurchaseDate.HasValue)
                        {
                            TimeSpan duration = crm_customer.LastPurchaseDate.Value - crm_customer.FirstPurchaseDate.Value;
                            int days = duration.Days;
                            crm_customer.DaysSinceLastPurchase = days; //这个可以反审时倒算出来。
                        }
                        else
                        {
                            crm_customer.FirstPurchaseDate = entity.OutDate;
                        }

                        //这个是结案时才算次数
                        if (entity.tb_saleorder != null && entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) == entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity))
                        {
                            crm_customer.PurchaseCount = crm_customer.PurchaseCount + 1;
                            if (crm_customer.PurchaseCount > 1)
                            {
                                crm_customer.CustomerStatus = (int)CustomerStatus.活跃客户;
                            }
                        }


                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleorder.tb_customervendor.tb_crm_customer).UpdateColumns(t => new { t.CustomerStatus, t.PurchaseCount, t.TotalPurchaseAmount, t.LastPurchaseDate, t.DaysSinceLastPurchase, t.FirstPurchaseDate }).ExecuteCommandAsync();

                        //如果这个客户是由线索转过来的则线索转化成功
                        if (entity.tb_saleorder.tb_customervendor.tb_crm_customer.tb_crm_leads != null)
                        {
                            entity.tb_saleorder.tb_customervendor.tb_crm_customer.tb_crm_leads.LeadsStatus = (int)LeadsStatus.已转化;
                            await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleorder.tb_customervendor.tb_crm_customer.tb_crm_leads).UpdateColumns(t => new { t.LeadsStatus }).ExecuteCommandAsync();
                        }
                    }

                    #endregion

                    //销售出库单，如果来自于销售订单，则要把出库数量累加到订单中的已交数量 并且如果数量够则自动结案
                    if (entity.tb_saleorder != null && entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) == entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity)
                        && entity.tb_saleorder.DataStatus == (int)DataStatus.确认)
                    {
                        entity.tb_saleorder.DataStatus = (int)DataStatus.完结;
                        entity.tb_saleorder.CloseCaseOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "审核销售库单时:" + entity.SaleOutNo + "结案。"; ;
                        entity.tb_saleorder.TotalCost = entity.tb_SaleOutDetails.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity);
                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleorder).UpdateColumns(t => new { t.DataStatus, t.CloseCaseOpinions, entity.tb_saleorder.TotalCost }).ExecuteCommandAsync();
                    }

                    #endregion

                    //运费检测  如果一个订单有运费。多次出库时。运费也默认加到了每次的出库单。
                    //多次出库第一次加上运费。 第二次起都是为0. 代码要在财务前面处理。因为应收明细中会将运费为一行添加。
                    if (entity.tb_saleorder.tb_SaleOuts.Count > 1)
                    {
                        entity.FreightIncome = 0;
                    }


                    //出库 产生应收前，先判断预收的情况。先核销一下部分。生成付款单成已经付款状态。同时写核销记录。同时反写预收的余额。
                    //！！！！！！！！！！！！！！！！！！！！！！！！！！！！注意要实现 参考收款单审核中 反冲时回写预收old的代码
                    //出库时，如果有预收款，如果收的金额 与 到账金额不一致时。是不是停止 出库？
                    //所有统一先应收再收款 by 2025-05-18 仅仅是订金时先处理了一些东西而已。
                    AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                    if (authorizeController.EnableFinancialModule())
                    {
                        #region 生成应收 
                        var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                        tb_FM_ReceivablePayable Payable = await ctrpayable.BuildReceivablePayable(entity);
                        var ctrpay = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                        ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpay.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(Payable, false);
                        if (rmr.Succeeded)
                        {
                            //已经是等审核。 
                            rrs.ReturnObjectAsOtherEntity = rmr.ReturnObject;
                        }

                        #endregion

                        #region 佣金生成应付
                        if (entity.TotalCommissionAmount > 0)
                        {
                            tb_FM_ReceivablePayable PayableCommission = await ctrpayable.BuildReceivablePayable(entity, true);
                            ReturnMainSubResults<tb_FM_ReceivablePayable> rmrCommission = await ctrpay.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(PayableCommission, false);
                            if (rmrCommission.Succeeded)
                            {
                                //已经是等审核。 审核时会核销预收付款
                            }
                        }

                        #endregion
                    }

                    entity.DataStatus = (int)DataStatus.确认;
                    entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                    entity.ApprovalResults = true;
                    BusinessHelper.Instance.ApproverEntity(entity);
                    //只更新指定列
                }
                else
                {
                    entity.DataStatus = (int)DataStatus.新建;
                    entity.ApprovalStatus = (int)ApprovalStatus.驳回;
                    entity.ApprovalResults = false;
                }

                entity.TotalCost = entity.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount) + entity.FreightCost;

                var last = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity).UpdateColumns(it => new
                {
                    it.ApprovalStatus,
                    it.DataStatus,
                    it.ApprovalResults,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
                    it.TotalCost,
                    it.FreightIncome
                }).ExecuteCommandAsync();



                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rrs.ReturnObject = entity as T;
                rrs.Succeeded = true;
                return rrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                rrs.Succeeded = false;
                rrs.ErrorMsg = "事务回滚=>" + ex.Message;
                _logger.Error(ex, "事务回滚" + ex.Message);
                return rrs;
            }

        }



        // 示例：封装状态更新方法
        private void UpdatePaymentStatus(tb_FM_ReceivablePayable payable)
        {
            if (payable.ForeignBalanceAmount == 0 || payable.LocalBalanceAmount == 0)
            {
                payable.ARAPStatus = (int)ARAPStatus.全部支付;
            }
            else
            {
                payable.ARAPStatus = (int)ARAPStatus.部分支付;
            }
        }



        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rs = new ReturnResults<T>();
            tb_SaleOut entity = ObjectEntity as tb_SaleOut;

            try
            {


                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少

                //判断是否能反审?
                if (entity.tb_SaleOutRes != null
                    && (entity.tb_SaleOutRes.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_SaleOutRes.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {

                    rs.ErrorMsg = "存在已确认或已完结，或已审核的销售退回单，不能反审核  ";
                    rs.Succeeded = false;
                    return rs;
                }

                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    //return false;
                    rs.ErrorMsg = "有结案的单据，已经跳过反审";
                    rs.Succeeded = false;
                    return rs;
                }

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal OutQtySum, DateTime LatestOutboundTime)>();

                // 遍历销售订单明细，聚合数据
                foreach (var child in entity.tb_SaleOutDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentSaleQty = child.Quantity; // 假设 Sale_Qty 对应明细中的 Quantity
                    DateTime currentOutboundTime = DateTime.Now; // 每次出库更新时间

                    // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            rs.ErrorMsg = $"{child.ProdDetailID}当前产品无库存数据，无法出库。请使用【期初盘点】【采购入库】】【生产缴库】的方式进行盘点后，再操作。";
                            rs.Succeeded = false;
                            return rs;

                        }
                        //更新在途库存
                        //反审，出库的要加回来，要卖的也要加回来
                        //inv.Quantity = inv.Quantity + child.Quantity;
                        //inv.Sale_Qty = inv.Sale_Qty + child.Quantity;
                        //最后出库时间要改回来，这里没有处理
                        //inv.LatestStorageTime
                        BusinessHelper.Instance.EditEntity(inv);
                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            OutQtySum: currentSaleQty, // 首次累加
                            LatestOutboundTime: currentOutboundTime
                        );
                        inventoryGroups[key] = group;

                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.OutQtySum += currentSaleQty;
                        // 取最新出库时间（若当前时间更新，则覆盖）
                        group.LatestOutboundTime = System.DateTime.Now;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }
                // 处理分组数据，更新库存记录的各字段
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    // 累加数值字段
                    inv.Sale_Qty += group.Value.OutQtySum.ToInt();
                    inv.Quantity += group.Value.OutQtySum.ToInt();
                    // 计算衍生字段（如总成本）
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity; // 需确保 Inv_Cost 有值
                    invUpdateList.Add(inv);
                }


                DbHelper<tb_Inventory> InvdbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                _unitOfWorkManage.RollbackTran();
                var Counter = await InvdbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (Counter == 0)
                {
                    _logger.LogInformation($"{entity.SaleOutNo}反审核时，更新库存结果为0行，请检查数据！");
                }

                if (!entity.ReplaceOut)
                {
                    #region  反审检测写回 退回
                    //处理销售订单
                    entity.tb_saleorder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                     .Includes(a => a.tb_SaleOuts, b => b.tb_SaleOutDetails)
                     .Includes(a => a.tb_customervendor, b => b.tb_crm_customer, c => c.tb_crm_leads)
                     .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                     .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                     .Where(c => c.SOrder_ID == entity.SOrder_ID)
                     .SingleAsync();

                    //先找到所有出库明细,再找按订单明细去循环比较。如果出库总数量大于订单数量，则不允许出库。
                    List<tb_SaleOutDetail> detailList = new List<tb_SaleOutDetail>();
                    foreach (var item in entity.tb_saleorder.tb_SaleOuts)
                    {
                        detailList.AddRange(item.tb_SaleOutDetails);
                    }

                    //分两种情况处理。
                    for (int i = 0; i < entity.tb_saleorder.tb_SaleOrderDetails.Count; i++)
                    {
                        //如果当前订单明细行，不存在于出库明细行。直接跳过。这种就是多行多品被删除时。不需要比较

                        string prodName = entity.tb_saleorder.tb_SaleOrderDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_saleorder.tb_SaleOrderDetails[i].tb_proddetail.tb_prod.Specifications;
                        //明细中有相同的产品或物品。
                        var aa = entity.tb_saleorder.tb_SaleOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0 && entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID > 0)
                        {
                            #region 如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            if (entity.tb_SaleOutDetails.Any(c => c.SaleOrderDetail_ID == 0))
                            {
                                //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                                string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加，反审失败！";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                _logger.LogInformation(msg);
                                return rs;
                            }
                            #endregion

                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                            && c.SaleOrderDetail_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID
                             && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            if (inQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                            {
                                string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】的出库数量不能大于订单中对应行的数量。";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                _logger.LogInformation(msg);
                                return rs;
                            }
                            else
                            {
                                var RowQty = entity.tb_SaleOutDetails.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                                && c.SaleOrderDetail_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID
                                  && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                                //算出交付的数量
                                entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty -= RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"销售出库单：{entity.SaleOutNo}反审核时，对应的订单：{entity.tb_saleorder.SOrderNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                        else
                        {
                            //一对一时
                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            if (inQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                            {

                                string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】的出库数量不能大于订单中对应行的数量。";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                _logger.LogInformation(msg);
                                return rs;
                            }
                            else
                            {
                                //当前行累计到交付
                                var RowQty = entity.tb_SaleOutDetails
                                    .Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                                    && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                                    ).Sum(c => c.Quantity);
                                entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty -= RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.SaleOutNo}反审核时，对应的订单：{entity.tb_saleorder.SOrderNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                    }


                    //更新已交数量
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrderDetail>(entity.tb_saleorder.tb_SaleOrderDetails).UpdateColumns(t => new { t.TotalDeliveredQty }).ExecuteCommandAsync();
                    //销售出库单，如果来自于销售订单，则要把出库数量累加到订单中的已交数量 并且如果数量够则自动结案
                    if (entity.tb_saleorder != null && entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) != entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity))
                    {
                        entity.tb_saleorder.DataStatus = (int)DataStatus.确认;
                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleorder).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                    }

                    #endregion
                }

                //如果是新客户或潜在客户。则转换为首单客户
                //降级 退回
                if (entity.tb_saleorder.tb_customervendor.tb_crm_customer != null)
                {
                    var crm_customer = entity.tb_saleorder.tb_customervendor.tb_crm_customer;
                    if (crm_customer.CustomerStatus == (int)CustomerStatus.首单客户)
                    {
                        crm_customer.CustomerStatus = (int)CustomerStatus.潜在客户;
                        crm_customer.LastPurchaseDate = entity.OutDate;
                    }

                    //更新采购金额
                    if (crm_customer.TotalPurchaseAmount == null)
                    {
                        crm_customer.TotalPurchaseAmount = 0;
                    }
                    if (crm_customer.PurchaseCount == null)
                    {
                        crm_customer.PurchaseCount = 0;
                    }

                    crm_customer.TotalPurchaseAmount -= entity.tb_SaleOutDetails.Sum(c => c.Quantity * c.TransactionPrice);
                    if (crm_customer.DaysSinceLastPurchase.HasValue)
                    {
                        crm_customer.LastPurchaseDate = crm_customer.LastPurchaseDate.Value.AddDays(-crm_customer.DaysSinceLastPurchase.Value); //todo ??// 这个如何退回？
                    }
                    else
                    {
                        crm_customer.LastPurchaseDate = null; //没有采购过
                    }

                    //撤回完结时次数也减少1
                    if (entity.tb_saleorder != null && entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) != entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity))
                    {
                        crm_customer.PurchaseCount = crm_customer.PurchaseCount - 1;
                    }
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleorder.tb_customervendor.tb_crm_customer).UpdateColumns(t => new { t.CustomerStatus, t.PurchaseCount, t.TotalPurchaseAmount, t.LastPurchaseDate }).ExecuteCommandAsync();
                }

                //销售出库的反审 
                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {

                    //如果是应收已经有收款记录，则生成反向收款，否则直接删除应收
                    #region

                    var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                    //出库时，全部生成应收，账期的。就加上到期日
                    //有付款过的。就去预收中抵扣，不够的金额及状态标识出来
                    //如果收款了，则不能反审,预收的可以
                    var ARAPList = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                    .Includes(c => c.tb_FM_ReceivablePayableDetails)
                                   .Where(c => c.SourceBillId == entity.SaleOut_MainID
                                   && c.TotalLocalPayableAmount > 0 //正向
                                   && c.SourceBizType == (int)BizType.销售出库单).ToListAsync();
                    if (ARAPList != null && ARAPList.Count > 0)
                    {
                        //处理收款时
                        var receivableList = new List<tb_FM_ReceivablePayable>();
                        receivableList = ARAPList.Where(c => c.ReceivePaymentType == (int)ReceivePaymentType.收款).ToList();
                        if (receivableList.Count == 1)
                        {
                            var result = await ctrpayable.AntiApplyManualPaymentAllocation(receivableList[0], ReceivePaymentType.收款, false);
                        }
                        else if (receivableList.Count > 1)
                        {
                            //不会为多行。有错误
                            _unitOfWorkManage.RollbackTran();
                            rs.ErrorMsg = $"销售出库单{entity.SaleOutNo}有多张应收款单，数据重复，请检查数据正确性后，再操作。";
                            rs.Succeeded = false;
                            return rs;
                        }


                        //处理佣金（付款）
                        var PayableList = new List<tb_FM_ReceivablePayable>();
                        PayableList = ARAPList.Where(c => c.ReceivePaymentType == (int)ReceivePaymentType.付款).ToList();
                        if (PayableList.Count == 1)
                        {
                            var result = await ctrpayable.AntiApplyManualPaymentAllocation(PayableList[0], ReceivePaymentType.付款, true);
                        }
                        else if (PayableList.Count > 1)
                        {
                            //不会为多行。有错误
                            _unitOfWorkManage.RollbackTran();
                            rs.ErrorMsg = $"销售出库单{entity.SaleOutNo}有多张佣金的应付款单，数据重复，请检查数据正确性后，再操作。";
                            rs.Succeeded = false;
                            return rs;
                        }

                    }

                    #endregion
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？
                //只更新指定列
                var last = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity).UpdateColumns(it => new
                {
                    it.ApprovalStatus,
                    it.DataStatus,
                    it.ApprovalResults,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
                }).ExecuteCommandAsync();

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


        public async override Task<List<T>> GetPrintDataSource(long SaleOut_MainID)
        {
            // var queryable = _appContext.Db.Queryable<tb_SaleOutDetail>();
            // var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();

            List<tb_SaleOut> list = await _appContext.Db.CopyNew().Queryable<tb_SaleOut>().Where(m => m.SaleOut_MainID == SaleOut_MainID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_SaleOutDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }


        /// <summary>
        /// 转换为销售退库单
        /// </summary>
        /// <param name="saleout"></param>
        public tb_SaleOutRe SaleOutToSaleOutRe(tb_SaleOut saleout)
        {
            tb_SaleOutRe entity = new tb_SaleOutRe();
            //转单
            if (saleout != null)
            {
                entity = mapper.Map<tb_SaleOutRe>(saleout);
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
                //退货时 默认不写付款情况，实际是有些平台的。会提前退？线下是先退再付款
                entity.PayStatus = null;
                entity.Paytype_ID = null;

                entity.RefundStatus = (int)RefundStatus.未退款等待退货;
                List<string> tipsMsg = new List<string>();
                List<tb_SaleOutReDetail> details = mapper.Map<List<tb_SaleOutReDetail>>(saleout.tb_SaleOutDetails);
                List<tb_SaleOutReDetail> NewDetails = new List<tb_SaleOutReDetail>();

                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && details[i].SaleOutDetail_ID > 0)
                    {
                        #region 产品ID可能大于1行，共用料号情况
                        tb_SaleOutDetail item = saleout.tb_SaleOutDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                        && c.Location_ID == details[i].Location_ID
                        && c.SaleOutDetail_ID == details[i].SaleOutDetail_ID);
                        details[i].Cost = item.Cost;
                        details[i].CustomizedCost = item.CustomizedCost;
                        //这时有一种情况就是订单时没有成本。没有产品。出库前有类似采购入库确定的成本
                        if (details[i].Cost == 0)
                        {
                            View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                            if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                            {
                                if (obj.Inv_Cost != null)
                                {
                                    details[i].Cost = obj.Inv_Cost.Value;
                                }
                            }
                        }
                        details[i].Quantity = item.Quantity - item.TotalReturnedQty;// 已经出数量去掉
                        details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                        details[i].SubtotalCostAmount = (details[i].Cost + details[i].CustomizedCost) * details[i].Quantity;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            tipsMsg.Add($"销售出库单{saleout.SaleOutNo}，{item.tb_proddetail.tb_prod.CNName + item.tb_proddetail.tb_prod.Specifications}已退回数为{item.TotalReturnedQty}，可退库数为{details[i].Quantity}，当前行数据忽略！");
                        }

                        #endregion
                    }
                    else
                    {
                        #region 每行产品ID唯一
                        tb_SaleOutDetail item = saleout.tb_SaleOutDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                          && c.Location_ID == details[i].Location_ID
                        && c.SaleOutDetail_ID == details[i].SaleOutDetail_ID);
                        details[i].Cost = item.Cost;
                        details[i].CustomizedCost = item.CustomizedCost;
                        //这时有一种情况就是订单时没有成本。没有产品。出库前有类似采购入库确定的成本
                        if (details[i].Cost == 0)
                        {
                            View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                            if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                            {
                                if (obj.Inv_Cost == null)
                                {
                                    obj.Inv_Cost = 0;
                                }
                                details[i].Cost = obj.Inv_Cost.Value;
                            }
                        }
                        details[i].Quantity = details[i].Quantity - item.TotalReturnedQty;// 减掉已经出库的数量
                        details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                        details[i].SubtotalCostAmount = (details[i].Cost + details[i].CustomizedCost) * details[i].Quantity;

                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            tipsMsg.Add($"当前订单的SKU:{item.tb_proddetail.SKU}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                        }
                        #endregion
                    }

                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"出库单:{entity.SaleOut_NO}已全部退库，请检查是否正在重复退库！");
                }

                entity.tb_SaleOutReDetails = NewDetails;

                //如果这个订单已经有出库单 则第二次运费为0
                if (saleout.tb_SaleOutRes != null && saleout.tb_SaleOutRes.Count > 0)
                {
                    if (saleout.FreightIncome > 0)
                    {
                        tipsMsg.Add($"当前出库单已经有退库记录，运费收入退回已经计入前面退库单，当前退库运费收入退回为零！");
                        entity.FreightIncome = 0;
                    }
                    else
                    {
                        tipsMsg.Add($"当前出库单已经有退库记录！");
                    }

                    //订金 及订金外币 要如何退? TODO
                }

                entity.ReturnDate = System.DateTime.Now;


                BusinessHelper.Instance.InitEntity(entity);

                if (entity.SaleOut_MainID.HasValue && entity.SaleOut_MainID > 0)
                {
                    entity.CustomerVendor_ID = saleout.CustomerVendor_ID;
                    entity.SaleOut_NO = saleout.SaleOutNo;
                    entity.IsFromPlatform = saleout.IsFromPlatform;
                }
                entity.ReturnNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售退回单);

                entity.tb_saleout = saleout;
                entity.TotalQty = NewDetails.Sum(c => c.Quantity);


                entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                entity.TotalAmount = entity.TotalAmount + entity.FreightIncome;

                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }



    }

}



