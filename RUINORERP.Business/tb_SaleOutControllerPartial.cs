
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
using RUINORERP.Model.Base;
using RUINORERP.Business.Helper;
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
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.EntityLoadService;
using RUINORERP.Business.Helpers;


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
                    entity.ApprovalOpinions = "同意平台退款时预转单";
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
                    entity.SaleOut_MainID = saleout.SaleOut_MainID;
                    entity.SaleOut_NO = saleout.SaleOutNo;
                    List<string> tipsMsg = new List<string>();
                    List<tb_SaleOutReDetail> details = mapper.Map<List<tb_SaleOutReDetail>>(saleout.tb_SaleOutDetails);
                    List<tb_SaleOutReDetail> NewDetails = new List<tb_SaleOutReDetail>();

                    // 优化：预先计算重复的ProdDetailID，避免在循环内重复查询
                    var duplicateProdDetailIds = details
                        .Select(c => c.ProdDetailID)
                        .ToList()
                        .GroupBy(x => x)
                        .Where(x => x.Count() > 1)
                        .Select(x => x.Key)
                        .ToHashSet();

                    for (global::System.Int32 i = 0; i < details.Count; i++)
                    {
                        if (duplicateProdDetailIds.Contains(details[i].ProdDetailID) && details[i].SaleOutDetail_ID > 0)
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
                                View_ProdDetail obj = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
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
                                View_ProdDetail obj = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
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
                    IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
                    entity.ReturnNo = await bizCodeService.GenerateBizBillNoAsync(BizType.销售退回单);
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
                else
                {
                    throw new Exception($"创建销售退回单失败：{SaveRs.ErrorMsg}");
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
                    if (entity.DataStatus == (int)DataStatus.确认 && (entity.ApprovalStatus.HasValue && entity.ApprovalStatus.Value == (int)ApprovalStatus.审核通过 && entity.ApprovalResults.Value))
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
        /// 如果非账期则同时生成收款单.
        /// 
        /// 1. 现金销售（全额收款）	- 销售出库时生成应收单（即使现金）
        //- 生成收款单并审核 → 自动核销应收单	- 应收应付表：生成应收单（状态=已审核）
        //- 收付款表：插入收款单（状态=已审核）
        //- 核销表：自动生成核销记录
        ///2. 账期销售（分期收款）	- 销售出库生成应收单
        //- 分次生成收款单 → 手动核销	- 每次核销更新 应收应付表.RemainAmount
        //- 最后一次核销标记 IsFullySettled = 1

        // </summary>
        // <param name="entity">销售出库单实体</param>
        // <returns>审核结果</returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rrs = new ReturnResults<T>();
            tb_SaleOut entity = ObjectEntity as tb_SaleOut;
            FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
            
            try
            {
                // ========== 第一阶段: 预处理验证(无事务) ==========
                
                // 1.1 基础验证 - 检查重复审核和审核结果
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

                var existingEntity = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                    .Where(c => c.SaleOut_MainID == entity.SaleOut_MainID)
                    .Select(c => new { c.DataStatus, c.ApprovalStatus, c.ApprovalResults })
                    .FirstAsync();

                if (existingEntity != null && 
                    existingEntity.DataStatus == (int)DataStatus.确认 && 
                    existingEntity.ApprovalStatus == (int)ApprovalStatus.审核通过)
                {
                    rrs.ErrorMsg = "销售出库单已经审核通过，不能重复审核！";
                    rrs.Succeeded = false;
                    return rrs;
                }

                // 1.2 加载依赖数据
                if (entity.tb_SaleOutDetails == null)
                {
                    entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                        .Includes(c => c.tb_SaleOutDetails)
                        .Where(d => d.SaleOut_MainID == entity.SaleOut_MainID)
                        .FirstAsync();
                }

                // 1.3 基础业务规则验证
                if (entity.TotalQty.Equals(entity.tb_SaleOutDetails.Sum(c => c.Quantity)) == false)
                {
                    rrs.ErrorMsg = $"销售出库数量与明细之和不相等!请检查数据后重试！";
                    rrs.Succeeded = false;
                    return rrs;
                }

                // 1.4 加载并验证销售订单
                entity.tb_saleorder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                    .Includes(a => a.tb_paymentmethod)
                    .Includes(a => a.tb_projectgroup, b => b.tb_department)
                    .Includes(a => a.tb_SaleOuts, b => b.tb_SaleOutDetails)
                    .Includes(a => a.tb_customervendor, b => b.tb_crm_customer, c => c.tb_crm_leads)
                    .AsNavQueryable()
                    .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                    .Where(c => c.SOrder_ID == entity.SOrder_ID)
                    .SingleAsync();

                // 验证客户一致性
                if (entity.CustomerVendor_ID != entity.tb_saleorder.CustomerVendor_ID)
                {
                    rrs.ErrorMsg = $"销售出库单的客户和销售订单的客户不同!请检查数据后重试！";
                    rrs.Succeeded = false;
                    return rrs;
                }

                // 验证订单状态
                bool isOrderConfirmed = entity.tb_saleorder.DataStatus == (int)DataStatus.确认;
                bool isApproved = entity.tb_saleorder.ApprovalResults.HasValue && entity.tb_saleorder.ApprovalResults.Value;
                
                if (!isOrderConfirmed || !isApproved)
                {
                    rrs.ErrorMsg = $" 销售订单{entity.tb_saleorder.SOrderNo}状态为【{((DataStatus)entity.tb_saleorder.DataStatus).ToString()}】\r\n请确认状态为【确认】已审核，并且审核结果为已通过!\r\n" +
                        $"请检查订单状态数据是否正确，或当前为相同订单重复出库！";
                    rrs.Succeeded = false;
                    return rrs;
                }

                // 验证付款状态(如果启用)
                if (_appContext.FMConfig.EnableSalesOrderPaymentStatusValidation)
                {
                    var paymentValidationResult = await ValidateSalesOrderPaymentStatusAsync(entity.tb_saleorder);
                    if (!paymentValidationResult.IsValid)
                    {
                        rrs.ErrorMsg = $"订单未满足付款条件，无法审核。\r\n订单：{entity.tb_saleorder.SOrderNo}\r\n付款状态：{paymentValidationResult.PaymentStatus}\r\n原因：{paymentValidationResult.Reason}";
                        rrs.Succeeded = false;
                        return rrs;
                    }
                }

                // 验证产品归属(如果不是换货出库)
                if (!entity.ReplaceOut)
                {
                    foreach (var child in entity.tb_SaleOutDetails)
                    {
                        if (!entity.tb_saleorder.tb_SaleOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID))
                        {
                            rrs.ErrorMsg = $"出库明细中有产品不属于当前销售订单!请检查数据后重试！";
                            rrs.Succeeded = false;
                            return rrs;
                        }
                    }
                }

                // 1.5 预加载库存和价格记录(性能优化)
                var preloadResult = await PreloadInventoryAndPricesForSaleOutAsync(entity);
                var inventoryGroups = preloadResult.InventoryGroups;
                var invUpdateList = preloadResult.InvUpdateList;
                var transactionList = preloadResult.TransactionList;
                var priceUpdateList = preloadResult.PriceUpdateList;
                var updateCostList = preloadResult.UpdateCostList;

                // 1.6 处理运费逻辑
                ProcessFreightIncome(entity);

                // 1.7 准备CRM更新数据
                var crmUpdateData = PrepareCrmUpdateData(entity);

                // ========== 第二阶段: 事务内执行核心业务 ==========
                _unitOfWorkManage.BeginTran();
                
                try
                {
                    // 2.1 更新库存(带死锁重试)
                    DbHelper<tb_Inventory> invDbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var invCounter = await invDbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (invCounter == 0 && invUpdateList.Count > 0)
                    {
                        _logger.Debug($"{entity.SaleOutNo}审核时，更新库存结果为0行，请检查数据！");
                    }

                    // 2.2 记录库存流水(带死锁重试)
                    tb_InventoryTransactionController<tb_InventoryTransaction> tranController = 
                        _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                    await tranController.BatchRecordTransactionsWithRetry(transactionList);

                    // 2.3 更新出库明细成本(如果有变化)
                    if (updateCostList.Count > 0)
                    {
                        DbHelper<tb_SaleOutDetail> detailDbHelper = _appContext.GetRequiredService<DbHelper<tb_SaleOutDetail>>();
                        var costCounter = await detailDbHelper.BaseDefaultAddElseUpdateAsync(updateCostList);
                        if (costCounter == 0)
                        {
                            _logger.Debug($"{entity.SaleOutNo}销售出库时成本价格更新失败");
                        }
                    }

                    // 2.4 更新价格记录
                    if (priceUpdateList.Count > 0)
                    {
                        DbHelper<tb_PriceRecord> priceDbHelper = _appContext.GetRequiredService<DbHelper<tb_PriceRecord>>();
                        var priceCounter = await priceDbHelper.BaseDefaultAddElseUpdateAsync(priceUpdateList);
                        if (priceCounter == 0)
                        {
                            _logger.Debug($"{entity.SaleOutNo}销售出库时价格记录更新失败");
                        }
                    }

                    // 2.5 更新订单明细交付数量(非换货场景)
                    bool needAutoClose = false;
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
                        // 优化：预先计算重复的ProdDetailID，避免在循环内重复查询
                        var duplicateOrderProdDetailIds = entity.tb_saleorder.tb_SaleOrderDetails
                            .Select(c => c.ProdDetailID)
                            .ToList()
                            .GroupBy(x => x)
                            .Where(x => x.Count() > 1)
                            .Select(x => x.Key)
                            .ToHashSet();

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

                            var prodDetail = entity.tb_saleorder.tb_SaleOrderDetails[i].tb_proddetail;
                            var prod = prodDetail?.tb_prod;
                            string prodName = prod != null
                                ? (prod.CNName ?? "") + (prod.Specifications ?? "")
                                : $"[产品ID:{entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID}]";
                            //明细中有相同的产品或物品。
                            //2024-4-29 思路更新:如果订单中有相同的产品的多行情况。出库明细冗余了订单明细的行号ID，就容易分清具体行的数据
                            if (duplicateOrderProdDetailIds.Contains(entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID) && entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID > 0)
                            {
                                #region 如果存在不是引用的明细,则不允许出库。这样不支持手动添加的情况。
                                if (entity.tb_saleorder.tb_SaleOrderDetails.Any(c => c.SaleOrderDetail_ID == 0))
                                {
                                    string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加。";
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception(msg);
                                }
                                #endregion

                                var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                                && c.SaleOrderDetail_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID
                                  && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                                ).Sum(c => c.Quantity);

                                if (inQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】的出库数量不能大于订单中对应行的数量，\r\n" +
                                        $"或存在当前销售订单重复录入了销售出库单。";
                                    throw new Exception(msg);
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
                                        throw new Exception(msg);
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
                                    string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】的出库数量不能大于订单中对应行的数量，\r\n" +
                                        $"或存在当前销售订单重复录入了销售出库单，审核失败！";
                                    throw new Exception(msg);
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
                                        throw new Exception(msg);
                                    }
                                }
                            }
                        }

                        //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                        if (entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) > entity.tb_saleorder.TotalQty)
                        {
                            _unitOfWorkManage.RollbackTran();
                            string msg = $"销售订单：{entity.tb_saleorder.SOrderNo}中，出库总交付数量不能大于订单数量！";
                            throw new Exception(msg);
                        }

                        //更新已交数量
                        int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrderDetail>(entity.tb_saleorder.tb_SaleOrderDetails).ExecuteCommandAsync();
                        if (poCounter > 0)
                        {
                            if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                            {
                                _logger.Debug($"{entity.SaleOutNo} ==> {entity.tb_saleorder.SOrderNo} 订单明细更新成功");
                            }
                        }

                        // 判断是否需要自动结案
                        if (entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) == entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity)
                            && entity.tb_saleorder.DataStatus == (int)DataStatus.确认)
                        {
                            needAutoClose = true;
                        }

                        #endregion
                    }

                    // 2.6 更新订单状态(如果需要自动结案)
                    if (needAutoClose)
                    {
                        entity.tb_saleorder.DataStatus = (int)DataStatus.完结;
                        string employeeName = _appContext.CurUserInfo?.UserInfo?.tb_employee?.Employee_Name ?? "系统";
                        entity.tb_saleorder.CloseCaseOpinions = $"【系统自动结案】{DateTime.Now:yyyy-MM-dd HH:mm:ss}{employeeName}审核销售库单时:{entity.SaleOutNo}结案。";
                        entity.tb_saleorder.TotalCost = entity.tb_SaleOutDetails.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity);

                        var orderUpdateResult = await _unitOfWorkManage.GetDbClient()
                            .Updateable(entity.tb_saleorder)
                            .UpdateColumns(t => new { t.DataStatus, t.CloseCaseOpinions, t.TotalCost })
                            .ExecuteCommandAsync();

                        if (orderUpdateResult <= 0)
                        {
                            throw new Exception("订单状态更新失败，审核终止");
                        }
                    }

                    // 2.7 更新出库单状态
                    entity.DataStatus = (int)DataStatus.确认;
                    entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                    entity.ApprovalResults = true;
                    entity.TotalCost = entity.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount) + entity.FreightCost;
                    BusinessHelper.Instance.ApproverEntity(entity);

                    await _unitOfWorkManage.GetDbClient()
                        .Updateable<tb_SaleOut>(entity)
                        .UpdateColumns(it => new {
                            it.ApprovalStatus, it.DataStatus, it.ApprovalResults,
                            it.Approver_at, it.Approver_by, it.ApprovalOpinions,
                            it.TotalCost, it.FreightIncome
                        })
                        .ExecuteCommandAsync();

                    // 提交主事务(快速提交,~50ms)
                    _unitOfWorkManage.CommitTran();
                    _logger.LogInformation($"销售出库单{entity.SaleOutNo}审核：主事务提交成功");

                    if (entity.tb_saleorder != null && entity.tb_saleorder.DataStatus == (int)DataStatus.完结)
                    {
                        _logger.LogInformation($"销售出库单{entity.SaleOutNo}审核：订单{entity.tb_saleorder.SOrderNo}已标记为完结状态");
                    }

                    // ========== 第三阶段: 后置处理(独立事务) ==========
                    
                    // 3.1 更新CRM数据(非关键路径)
                    if (crmUpdateData.NeedUpdate)
                    {
                        try
                        {
                            var crmCustomer = crmUpdateData.Customer;
                            
                            // 如果是新客户或潜在客户，则转换为首单客户
                            if (crmCustomer.CustomerStatus == (int)CustomerStatus.潜在客户 ||
                                crmCustomer.CustomerStatus == (int)CustomerStatus.新增客户)
                            {
                                crmCustomer.CustomerStatus = (int)CustomerStatus.首单客户;
                                crmCustomer.FirstPurchaseDate = entity.OutDate;
                            }

                            // 更新采购金额
                            if (crmCustomer.PurchaseCount == null)
                            {
                                crmCustomer.PurchaseCount = 0;
                            }

                            if (crmCustomer.TotalPurchaseAmount == null)
                            {
                                crmCustomer.TotalPurchaseAmount = 0;
                            }
                            crmCustomer.TotalPurchaseAmount += entity.tb_SaleOutDetails.Sum(c => c.Quantity * c.TransactionPrice);
                            
                            if (crmCustomer.FirstPurchaseDate.HasValue)
                            {
                                if (crmCustomer.LastPurchaseDate.HasValue)
                                {
                                    TimeSpan duration = entity.OutDate - crmCustomer.LastPurchaseDate.Value;
                                    int days = duration.Days;
                                    crmCustomer.DaysSinceLastPurchase = days;
                                }
                                else
                                {
                                    crmCustomer.DaysSinceLastPurchase = 0;
                                }
                            }
                            else
                            {
                                crmCustomer.FirstPurchaseDate = entity.OutDate;
                            }
                            crmCustomer.LastPurchaseDate = entity.OutDate;
                            crmCustomer.Converted = true;
                            
                            // 如果订单已结案，增加购买次数
                            if (entity.tb_saleorder != null && 
                                entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) == entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity))
                            {
                                crmCustomer.PurchaseCount = crmCustomer.PurchaseCount + 1;
                                if (crmCustomer.PurchaseCount > 1)
                                {
                                    crmCustomer.CustomerStatus = (int)CustomerStatus.活跃客户;
                                }
                            }

                            await _unitOfWorkManage.GetDbClient()
                                .Updateable(crmCustomer)
                                .UpdateColumns(t => new { 
                                    t.CustomerStatus, t.PurchaseCount, t.TotalPurchaseAmount, 
                                    t.LastPurchaseDate, t.DaysSinceLastPurchase, t.FirstPurchaseDate,
                                    t.Converted
                                })
                                .ExecuteCommandAsync();

                            // 如果这个客户是由线索转过来的，则线索转化成功
                            if (crmUpdateData.Leads != null)
                            {
                                crmUpdateData.Leads.LeadsStatus = (int)LeadsStatus.已转化;
                                await _unitOfWorkManage.GetDbClient()
                                    .Updateable(crmUpdateData.Leads)
                                    .UpdateColumns(t => new { t.LeadsStatus })
                                    .ExecuteCommandAsync();
                            }
                            
                            _logger.LogInformation($"销售出库单{entity.SaleOutNo}审核：CRM数据更新成功");
                        }
                        catch (Exception crmEx)
                        {
                            _logger.LogWarning(crmEx, $"销售出库单{entity.SaleOutNo}审核：CRM数据更新失败，不影响主流程 - {crmEx.Message}");
                        }
                    }
                    
                    // 3.2 财务处理(独立事务)
                    AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                    if (authorizeController.EnableFinancialModule())
                    {
                        var financeResult = await ProcessFinanceAfterSaleOutApprovalAsync(entity, fMAuditLog);
                        if (!financeResult.Succeeded)
                        {
                            _logger.LogWarning($"销售出库单{entity.SaleOutNo}审核：主事务成功，但财务处理失败 - {financeResult.ErrorMsg}");
                        }
                    }

                    rrs.ReturnObject = entity as T;
                    rrs.Succeeded = true;
                    return rrs;
                }
                catch (Exception ex)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, $"销售出库单{entity?.SaleOutNo}审核异常: {ex.Message}");
                rrs.Succeeded = false;
                rrs.ErrorMsg = ex.Message;
                return rrs;
            }
        }

        /// <summary>
        /// 销售出库审核后的财务独立事务处理
        /// 在主事务提交后执行，失败不影响出库审核结果（含补偿机制）
        /// </summary>
        private async Task<ReturnResults<bool>> ProcessFinanceAfterSaleOutApprovalAsync(tb_SaleOut entity, FMAuditLogHelper fMAuditLog)
        {
            ReturnResults<bool> result = new ReturnResults<bool>();
            List<(long ARAPID, string ARAPNo, bool IsCommission)> savedPayables = new List<(long, string, bool)>();
            bool hasError = false;

            try
            {
                var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                // 生成应收款单
                if (entity.TotalAmount > 0 || entity.ForeignTotalAmount > 0)
                {
                    try
                    {
                        // 重新加载完整实体（因为主事务已提交，需要新的数据库上下文）
                        tb_SaleOut financeEntity = await _unitOfWorkManage.GetDbClient()
                            .Queryable<tb_SaleOut>()
                            .Includes(a => a.tb_SaleOutDetails)
                            .Includes(a => a.tb_saleorder, b => b.tb_paymentmethod)
                            .Where(s => s.SaleOut_MainID == entity.SaleOut_MainID)
                            .FirstAsync();

                        if (financeEntity != null)
                        {
                            tb_FM_ReceivablePayable payable = await ctrpayable.BuildReceivablePayable(financeEntity);
                            ReturnMainSubResults<tb_FM_ReceivablePayable> rmr =
                                await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(payable, false);

                            if (rmr.Succeeded)
                            {
                                var savedARAP = rmr.ReturnObject;
                                savedPayables.Add((savedARAP.ARAPId, savedARAP.ARAPNo, false));
                                fMAuditLog.CreateAuditLog<tb_FM_ReceivablePayable>($"销售出库单{entity.SaleOutNo}审核：生成应收款单，单号：{savedARAP.ARAPNo}", savedARAP);

                                //如果是平台单，则自动审核
                                if (payable.IsFromPlatform)
                                {
                                    payable.ApprovalOpinions = "自动审核";
                                    ReturnResults<tb_FM_ReceivablePayable> autoApproval = await ctrpayable.ApprovalAsync(payable, true);
                                    if (!autoApproval.Succeeded)
                                    {
                                        // 平台单自动审核失败，触发补偿
                                        await CompensateReceivablePayableAsync(savedARAP.ARAPId, entity.SaleOutNo, false);
                                        autoApproval.Succeeded = false;
                                        autoApproval.ErrorMsg = $"自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                        hasError = true;
                                        _logger.LogWarning($"销售出库单{entity.SaleOutNo}：平台单自动审核失败，已触发补偿机制");
                                    }
                                    else
                                    {
                                        fMAuditLog.CreateAuditLog<tb_FM_ReceivablePayable>("自动审核成功", autoApproval.ReturnObject as tb_FM_ReceivablePayable);
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogError(
                                    $"销售出库单{entity.SaleOutNo}审核：应收款单生成失败 - {rmr.ErrorMsg}\n" +
                                    $"⚠️ 请财务部门手动补录应收单据");
                                hasError = true;
                            }
                        }
                        else
                        {
                            _logger.LogError(
                                $"销售出库单{entity.SaleOutNo}审核：无法重新加载出库单实体，无法生成应收款单\n" +
                                $"⚠️ 请财务部门手动补录应收单据");
                            hasError = true;
                        }
                    }
                    catch (Exception arEx)
                    {
                        _logger.LogError(arEx,
                            $"销售出库单{entity.SaleOutNo}审核：生成应收款单时发生异常 - {arEx.Message}\n" +
                            $"⚠️ 请财务部门手动补录应收单据");
                        hasError = true;
                    }
                }

                // 生成佣金应付款单
                if (entity.TotalCommissionAmount > 0)
                {
                    try
                    {
                        tb_SaleOut commissionEntity = await _unitOfWorkManage.GetDbClient()
                            .Queryable<tb_SaleOut>()
                            .Includes(a => a.tb_SaleOutDetails)
                            .Includes(a => a.tb_saleorder, b => b.tb_paymentmethod)
                            .Where(s => s.SaleOut_MainID == entity.SaleOut_MainID)
                            .FirstAsync();

                        if (commissionEntity != null)
                        {
                            tb_FM_ReceivablePayable payableCommission =
                                await ctrpayable.BuildReceivablePayable(commissionEntity, true);
                            ReturnMainSubResults<tb_FM_ReceivablePayable> rmrCommission =
                                await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(payableCommission, false);

                            if (rmrCommission.Succeeded)
                            {
                                var savedCommission = rmrCommission.ReturnObject;
                                savedPayables.Add((savedCommission.ARAPId, savedCommission.ARAPNo, true));
                                fMAuditLog.CreateAuditLog<tb_FM_ReceivablePayable>($"销售出库单{entity.SaleOutNo}审核：佣金应付款单生成成功，单号：{savedCommission.ARAPNo}", savedCommission);
                            }
                            else
                            {
                                _logger.LogError(
                                    $"销售出库单{entity.SaleOutNo}审核：佣金应付款单生成失败 - {rmrCommission.ErrorMsg}\n" +
                                    $"⚠️ 请财务部门手动补录应付单据");
                                hasError = true;
                            }
                        }
                    }
                    catch (Exception commEx)
                    {
                        _logger.LogError(commEx,
                            $"销售出库单{entity.SaleOutNo}审核：生成佣金应付款单时发生异常 - {commEx.Message}\n" +
                            $"⚠️ 请财务部门手动补录应付单据");
                        hasError = true;
                    }
                }

                _logger.LogInformation(
                    $"销售出库单{entity.SaleOutNo}审核：财务独立事务处理完成");

                // 如果有错误且有已保存的财务单据，触发补偿机制
                if (hasError && savedPayables.Count > 0)
                {
                    foreach (var saved in savedPayables)
                    {
                        await CompensateReceivablePayableAsync(saved.ARAPID, entity.SaleOutNo, saved.IsCommission);
                    }
                }

                result.Succeeded = !hasError;
                return result;
            }
            catch (Exception financeEx)
            {
                // 财务处理失败不影响出库审核结果，但需要提供明确的用户反馈
                // 触发补偿机制
                if (savedPayables.Count > 0)
                {
                    foreach (var saved in savedPayables)
                    {
                        await CompensateReceivablePayableAsync(saved.ARAPID, entity.SaleOutNo, saved.IsCommission);
                    }
                }

                _logger.LogError(financeEx,
                    $"销售出库单{entity.SaleOutNo}审核：财务独立事务处理失败（出库审核已成功）- {financeEx.Message}\n" +
                    $"⚠️ 请财务部门检查并手动补录相关单据");

                result.Succeeded = false;
                result.ErrorMsg = $"财务单据生成失败：{financeEx.Message}\n" +
                                  $"请财务部门手动补录相关单据，单据号：{entity.SaleOutNo}";
                return result;
            }
        }

        /// <summary>
        /// 补偿机制：删除已创建的应收/应付账款
        /// </summary>
        private async Task CompensateReceivablePayableAsync(long arapId, string saleOutNo, bool isCommission)
        {
            try
            {
                var payable = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_FM_ReceivablePayable>()
                    .Where(c => c.ARAPId == arapId)
                    .FirstAsync();

                if (payable == null)
                {
                    _logger.LogInformation($"销售出库单{saleOutNo}：{(isCommission ? "佣金应付款" : "应收款")} {arapId} 不存在，无需补偿");
                    return;
                }

                // 检查是否已被核销/支付
                if (payable.LocalPaidAmount > 0 || payable.ForeignPaidAmount > 0)
                {
                    _logger.LogError($"销售出库单{saleOutNo}：{(isCommission ? "佣金应付款" : "应收款")} {arapId} 已被核销，无法补偿删除");
                    return;
                }

                // 删除应收/应付账款
                var deletedCount = await _unitOfWorkManage.GetDbClient()
                    .Deleteable<tb_FM_ReceivablePayable>()
                    .Where(c => c.ARAPId == arapId)
                    .ExecuteCommandAsync();

                if (deletedCount > 0)
                {
                    _logger.LogInformation($"销售出库单{saleOutNo}：{(isCommission ? "佣金应付款" : "应收款")} {arapId} 补偿删除成功");
                }
                else
                {
                    _logger.LogWarning($"销售出库单{saleOutNo}：{(isCommission ? "佣金应付款" : "应收款")} {arapId} 补偿删除未找到记录");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"销售出库单{saleOutNo}：{(isCommission ? "佣金应付款" : "应收款")} {arapId} 补偿删除失败");
            }
        }

        /// <summary>
        /// 判断销售出库单是否满足自动审核条件
        /// 条件：配置开关启用 + 财务模块启用 + 销售订单为全额预收款类型 + 存在未核销预收款
        /// </summary>
        /// <param name="saleOut">销售出库单</param>
        /// <returns>是否满足自动审核条件</returns>
        private async Task<(bool IsSuccess, string Reason)> ShouldAutoAuditSalesOut(tb_SaleOut saleOut)
        {
            // 检查财务模块是否启用
            AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
            if (!authorizeController.EnableFinancialModule())
            {
                return (false, "财务模块未启用");
            }
            // 检查配置开关是否启用
            if (!_appContext.FMConfig.EnableAutoAuditSalesOutboundForFullPrepaymentOrders)
            {
                return (false, "未启用全额预收款订单自动审核配置");
            }
            if (saleOut.PayStatus != (int)PayStatus.全额预付)
            {
                return (false, "付款状态不是全额预付");
            }

            // 检查是否有关联的销售订单
            if (!saleOut.SOrder_ID.HasValue)
            {
                return (false, "未关联销售订单");
            }

            // 加载销售订单数据
            var saleOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                .Where(c => c.SOrder_ID == saleOut.SOrder_ID.Value)
                .FirstAsync();

            if (saleOrder == null)
            {
                return (false, "关联的销售订单不存在");
            }

            // 判断是否为全额预收款订单
            // 通过检查 Paytype_ID 是否为全额预收款类型(假设ID=1表示全额预收款)
            // 实际应根据业务配置确定具体的全额预收款类型ID
            if (!saleOrder.Paytype_ID.HasValue)
            {
                return (false, "销售订单未设置付款类型");
            }

            //如果为账期付款类型则不处理
            if (_appContext.PaymentMethodOfPeriod.Paytype_ID == saleOrder.Paytype_ID.Value)
            {
                return (false, "账期付款类型订单不支持自动审核");
            }


            // 加载付款类型信息
            var paymentMethod = await _unitOfWorkManage.GetDbClient().Queryable<tb_PaymentMethod>()
                .Where(c => c.Paytype_ID == saleOrder.Paytype_ID.Value)
                .FirstAsync();

            if (paymentMethod == null)
            {
                return (false, "付款类型不存在");
            }


            // 检查是否存在未核销的预收款
            var prePayments = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                .Where(x => (x.PrePaymentStatus == (int)PrePaymentStatus.待核销 || x.PrePaymentStatus == (int)PrePaymentStatus.处理中)
                && x.CustomerVendor_ID == saleOut.CustomerVendor_ID
                && x.IsAvailable == true
                && x.SourceBizType == (int)BizType.销售订单
                && x.SourceBillId == saleOut.SOrder_ID.Value
                && x.ReceivePaymentType == (int)ReceivePaymentType.收款)
                .ToListAsync();

            if (prePayments == null || prePayments.Count == 0)
            {
                return (false, "不存在未核销的预收款");
            }

            // 计算预收款总额
            decimal totalPrePayment = prePayments.Sum(x => x.LocalBalanceAmount);

            // 检查预收款是否足够支付出库金额
            if (totalPrePayment < saleOut.TotalAmount - 0.01m) // 允许0.01的误差
            {
                return (false, $"预收款金额不足，需要{saleOut.TotalAmount}，可用{totalPrePayment}");
            }

            return (true, "满足自动审核条件");
        }

        /// <summary>
        /// 销售出库单保存后自动审核
        /// 当满足全额预收款条件时,自动执行审核流程
        /// </summary>
        /// <param name="saleOut">销售出库单</param>
        /// <returns>审核结果</returns>
        public async Task<ReturnResults<tb_SaleOut>> AutoAuditSalesOutAsync(tb_SaleOut saleOut)
        {
            ReturnResults<tb_SaleOut> result = new ReturnResults<tb_SaleOut>();

            try
            {
                // 检查是否满足自动审核条件
                var autoAuditResult = await ShouldAutoAuditSalesOut(saleOut);

                if (!autoAuditResult.IsSuccess)
                {
                    result.Succeeded = false;
                    result.ErrorMsg = $"不满足自动审核条件,跳过自动审核: {autoAuditResult.Reason}";
                    return result;
                }

                // 设置审核信息
                saleOut.ApprovalResults = true;
                saleOut.ApprovalOpinions = "系统自动审核(全额预收款订单)";
                BusinessHelper.Instance.ApproverEntity(saleOut);

                // 执行审核流程
                var approvalResult = await ApprovalAsync(saleOut as T);
                if (approvalResult.Succeeded)
                {
                    result.Succeeded = true;
                    result.ReturnObject = saleOut;
                }
                else
                {
                    _logger.LogWarning($"销售出库单【{saleOut.SaleOutNo}】自动审核失败:{approvalResult.ErrorMsg}");
                    result.Succeeded = false;
                    result.ErrorMsg = $"自动审核失败:{approvalResult.ErrorMsg}";
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"销售出库单【{saleOut.SaleOutNo}】自动审核异常");
                result.Succeeded = false;
                result.ErrorMsg = $"自动审核异常:{ex.Message}";
                return result;
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
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少

                //判断是否能反审?
                if (entity.tb_SaleOutRes != null
                    && (entity.tb_SaleOutRes.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_SaleOutRes.Any(c => c.ApprovalStatus == (int)ApprovalStatus.审核通过)))
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
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();

                // 【死锁优化】按 (ProdDetailID, LocationID) 排序，确保所有事务以相同顺序访问资源
                var sortedInventoryGroups = inventoryGroups
                    .OrderBy(g => g.Key.ProdDetailID)
                    .ThenBy(g => g.Key.LocationID)
                    .ToList();

                foreach (var group in sortedInventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    // 记录原始库存数量，用于计算变化量
                    int originalQty = inv.Quantity;
                    decimal originalCost = inv.Inv_Cost;

                    // 累加数值字段
                    inv.Sale_Qty += group.Value.OutQtySum.ToInt();
                    inv.Quantity += group.Value.OutQtySum.ToInt();
                    // 计算衍生字段（如总成本）
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity; // 需确保 Inv_Cost 有值
                    invUpdateList.Add(inv);

                    // 创建库存流水记录（反审核，库存增加）
                    tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                    transaction.ProdDetailID = inv.ProdDetailID;
                    transaction.Location_ID = inv.Location_ID;
                    transaction.BizType = (int)BizType.销售出库单;
                    transaction.ReferenceId = entity.SaleOut_MainID;
                    transaction.ReferenceNo = entity.SaleOutNo;
                    transaction.BeforeQuantity = originalQty; // 变动前的库存数量
                    transaction.QuantityChange = group.Value.OutQtySum.ToInt(); // 反审核增加库存
                    transaction.AfterQuantity = inv.Quantity;
                    transaction.UnitCost = inv.Inv_Cost;
                    transaction.TransactionTime = DateTime.Now;
                    transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    View_ProdDetail obj = _cacheManager.GetEntity<View_ProdDetail>(inv.ProdDetailID);
                    if (obj != null)
                    {
                        transaction.Notes = $"销售出库单反审核：{entity.SaleOutNo}，产品：{obj.SKU}-{obj.CNName}";
                    }
                    else
                    {
                        transaction.Notes = $"销售出库单反审核：{entity.SaleOutNo}";
                    }
                    transactionList.Add(transaction);
                }


                DbHelper<tb_Inventory> InvdbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await InvdbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (Counter == 0)
                {
                    _logger.Debug($"{entity.SaleOutNo}反审核时，更新库存结果为0行，请检查数据！");
                }

                // 记录库存流水（带死锁重试机制）
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactionsWithRetry(transactionList);

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

                    // 优化：预先计算重复的ProdDetailID，避免在循环内重复查询
                    var duplicateOrderProdDetailIds2 = entity.tb_saleorder.tb_SaleOrderDetails
                        .Select(c => c.ProdDetailID)
                        .ToList()
                        .GroupBy(x => x)
                        .Where(x => x.Count() > 1)
                        .Select(x => x.Key)
                        .ToHashSet();

                    //分两种情况处理。
                    for (int i = 0; i < entity.tb_saleorder.tb_SaleOrderDetails.Count; i++)
                    {
                        //如果当前订单明细行，不存在于出库明细行。直接跳过。这种就是多行多品被删除时。不需要比较

                    var prodDetail2 = entity.tb_saleorder.tb_SaleOrderDetails[i].tb_proddetail;
                    var prod2 = prodDetail2?.tb_prod;
                    string prodName = prod2 != null
                        ? (prod2.CNName ?? "") + (prod2.Specifications ?? "")
                        : $"[产品ID:{entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID}]";
                        //明细中有相同的产品或物品。
                        if (duplicateOrderProdDetailIds2.Contains(entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID) && entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID > 0)
                        {
                            #region 如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            if (entity.tb_SaleOutDetails.Any(c => c.SaleOrderDetail_ID == 0))
                            {
                                //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                                string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加，反审失败！";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                _logger.Debug(msg);
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
                                _logger.Debug(msg);
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
                                _logger.Debug(msg);
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
                                    throw new Exception($"销售出库单：{entity.SaleOutNo}反审核时，对应的订单：{entity.tb_saleorder.SOrderNo}，{prodName}的明细不能为负数！");
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
                            var result = await ctrpayable.AntiApplyManualPaymentAllocation(receivableList[0], ReceivePaymentType.收款, true, false);
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
                            var result = await ctrpayable.AntiApplyManualPaymentAllocation(PayableList[0], ReceivePaymentType.付款, true, true);
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
        public async Task<tb_SaleOutRe> SaleOutToSaleOutRe(tb_SaleOut saleout)
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

                // 优化：预先计算重复的ProdDetailID，避免在循环内重复查询
                var duplicateProdDetailIds3 = details
                    .Select(c => c.ProdDetailID)
                    .ToList()
                    .GroupBy(x => x)
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .ToHashSet();

                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    if (duplicateProdDetailIds3.Contains(details[i].ProdDetailID) && details[i].SaleOutDetail_ID > 0)
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
                            View_ProdDetail obj = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
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
                            var prodInfo = _cacheManager.GetEntity<View_ProdInfo>(details[i].ProdDetailID);
                            if (prodInfo != null)
                            {
                                tipsMsg.Add($"销售出库单{saleout.SaleOutNo}，{prodInfo.CNName + prodInfo.Specifications}已退回数为{item.TotalReturnedQty}，可退库数为{details[i].Quantity}，当前行数据忽略！");
                            }
                            else
                            {
                                tipsMsg.Add($"销售出库单{saleout.SaleOutNo}，{item.property}已退回数为{item.TotalReturnedQty}，可退库数为{details[i].Quantity}，当前行数据忽略！");
                            }
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
                            View_ProdDetail obj = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
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

                IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
                ILogger logger = _appContext.GetRequiredService<ILogger<tb_SaleOutController<T>>>();
                entity.ReturnNo = await BizCodeHelper.GenerateBizBillNoWithRetryAsync(
                    bizCodeService, BizType.销售退回单, maxRetries: 3, initialDelayMs: 500, logger: logger);
                entity.tb_saleout = saleout;
                entity.TotalQty = NewDetails.Sum(c => c.Quantity);


                entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                entity.TotalAmount = entity.TotalAmount + entity.FreightIncome;

                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }

        /// <summary>
        /// 验证销售订单付款状态是否符合出库审核条件
        /// </summary>
        /// <param name="saleOrder">销售订单实体</param>
        /// <returns>验证结果</returns>
        private async Task<(bool IsValid, string PaymentStatus, string Reason)> ValidateSalesOrderPaymentStatusAsync(tb_SaleOrder saleOrder)
        {
            try
            {
                if (saleOrder == null)
                {
                    return (false, "未知", "销售订单数据为空");
                }

                // 获取付款状态的枚举值
                var unPaidStatus = (int)PayStatus.未付款;
                var partialPrepaidStatus = (int)PayStatus.部分预付;
                var partialPaidStatus = (int)PayStatus.部分付款;

                // 检查付款状态是否符合要求
                if (saleOrder.PayStatus == unPaidStatus)
                {
                    return (false, "未付款", "订单处于未付款状态，不允许进行销售出库审核");
                }

                if (saleOrder.PayStatus == partialPrepaidStatus)
                {
                    return (false, "部分预付", "订单处于部分预付状态，不允许进行销售出库审核");
                }

                if (saleOrder.PayStatus == partialPaidStatus)
                {
                    return (false, "部分付款", "订单处于部分付款状态，不允许进行销售出库审核");
                }

                // 符合审核条件（全额预付或全额付款）
                var paymentStatusText = ((PayStatus)saleOrder.PayStatus).ToString();
                _logger.LogDebug($"订单{saleOrder.SOrderNo}付款状态验证通过：{paymentStatusText}");
                return (true, paymentStatusText, "");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"验证销售订单付款状态时发生异常，订单号：{saleOrder?.SOrderNo}");
                return (false, "验证失败", $"付款状态验证异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 处理运费逻辑 - 多次出库时，只有第一次出库包含运费
        /// </summary>
        /// <param name="entity">销售出库单实体</param>
        private void ProcessFreightIncome(tb_SaleOut entity)
        {
            // 运费检测：如果一个订单有运费，多次出库时，运费也默认加到了每次的出库单。
            // 多次出库第一次加上运费，第二次起都是为 0。代码要在财务前面处理，因为应收明细中会将运费为一行添加。
            // 关键修复：需要判断当前出库单是否为该订单的第一次出库
            if (entity.tb_saleorder != null && entity.tb_saleorder.tb_SaleOuts != null)
            {
                // 获取所有已审核的出库单（排除当前正在审核的这张）
                var auditedSaleOuts = entity.tb_saleorder.tb_SaleOuts
                    .Where(o => o.SaleOut_MainID != entity.SaleOut_MainID
                             && o.DataStatus >= (int)DataStatus.确认
                             && o.ApprovalStatus == (int)ApprovalStatus.审核通过)
                    .OrderBy(o => o.Created_at)
                    .ToList();

                // 如果已经有审核过的出库单，则当前出库单的运费收入应该为 0
                if (auditedSaleOuts.Count > 0)
                {
                    entity.FreightIncome = 0;
                    // 同时调整总金额，扣除运费收入
                    entity.TotalAmount = entity.TotalAmount - entity.FreightIncome;
                    _logger.LogDebug($"销售出库单{entity.SaleOutNo}不是第一次出库，运费收入已调整为 0，调整后总金额：{entity.TotalAmount}");
                }
                else
                {
                    _logger.LogDebug($"销售出库单{entity.SaleOutNo}是第一次出库，保留运费收入：{entity.FreightIncome}");
                }
            }
        }

        /// <summary>
        /// 准备CRM更新数据
        /// </summary>
        /// <param name="entity">销售出库单实体</param>
        /// <returns>CRM更新数据</returns>
        private (bool NeedUpdate, tb_CRM_Customer Customer, tb_CRM_Leads Leads) PrepareCrmUpdateData(tb_SaleOut entity)
        {
            bool needUpdate = false;
            tb_CRM_Customer customer = null;
            tb_CRM_Leads leads = null;

            if (entity.tb_saleorder?.tb_customervendor?.tb_crm_customer != null)
            {
                needUpdate = true;
                customer = entity.tb_saleorder.tb_customervendor.tb_crm_customer;
                
                if (entity.tb_saleorder.tb_customervendor.tb_crm_customer.tb_crm_leads != null)
                {
                    leads = entity.tb_saleorder.tb_customervendor.tb_crm_customer.tb_crm_leads;
                }
            }

            return (needUpdate, customer, leads);
        }

        /// <summary>
        /// 预加载库存和价格记录(性能优化)
        /// </summary>
        /// <param name="entity">销售出库单实体</param>
        /// <returns>预加载结果</returns>
        private async Task<(Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal OutQtySum, DateTime LatestOutboundTime)> InventoryGroups,
            List<tb_Inventory> InvUpdateList,
            List<tb_InventoryTransaction> TransactionList,
            List<tb_PriceRecord> PriceUpdateList,
            List<tb_SaleOutDetail> UpdateCostList)> PreloadInventoryAndPricesForSaleOutAsync(tb_SaleOut entity)
        {
            // 提取所有需要的 (ProdDetailID, LocationID) 键
            var requiredInventoryKeys = entity.tb_SaleOutDetails
                .Select(c => new { c.ProdDetailID, c.Location_ID })
                .Distinct()
                .ToList();

            // 批量查询所有需要的库存记录（1 次查询替代 N 次）
            var inventoryList = await _unitOfWorkManage.GetDbClient()
                .Queryable<tb_Inventory>()
                .Includes(a => a.tb_proddetail)
                .Where(i => requiredInventoryKeys.Any(k =>
                    k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                .ToListAsync();

            // 转换为字典，O(1) 时间复杂度查找
            var inventoryDict = inventoryList.ToDictionary(
                i => (i.ProdDetailID, i.Location_ID)
            );

            // 批量预加载价格记录（1 次查询替代 N 次）
            var prodDetailIds = entity.tb_SaleOutDetails.Select(c => c.ProdDetailID).Distinct().ToList();
            var priceRecordList = await _unitOfWorkManage.GetDbClient()
                .Queryable<tb_PriceRecord>()
                .Where(c => c.Employee_ID == entity.tb_saleorder.Employee_ID &&
                            prodDetailIds.Contains(c.ProdDetailID))
                .OrderByDescending(c => c.SaleDate)
                .ToListAsync();

            var priceRecordLookup = priceRecordList.ToLookup(p => p.ProdDetailID);

            // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
            var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal OutQtySum, DateTime LatestOutboundTime)>();
            List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
            List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
            List<tb_PriceRecord> priceUpdateList = new List<tb_PriceRecord>();
            List<tb_SaleOutDetail> updateCostList = new List<tb_SaleOutDetail>();

            // 遍历销售订单明细，聚合数据
            foreach (var child in entity.tb_SaleOutDetails)
            {
                var key = (child.ProdDetailID, child.Location_ID);
                decimal currentSaleQty = child.Quantity;
                DateTime currentOutboundTime = DateTime.Now;

                if (!inventoryGroups.TryGetValue(key, out var group))
                {
                    // 从预加载的字典中获取库存
                    tb_Inventory inv = null;
                    if (!inventoryDict.TryGetValue(key, out inv))
                    {
                        throw new Exception($"当前仓库{child.Location_ID}无产品{child.ProdDetailID}的库存数据，请联系管理员");
                    }
                    if (inv != null)
                    {
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    else
                    {
                        throw new Exception($"当前仓库{child.Location_ID}无产品{child.ProdDetailID}的库存数据,请联系管理员");
                    }

                    // 更新销售价格历史记录
                    tb_PriceRecord priceRecord = null;
                    var priceRecordsForProduct = priceRecordLookup[child.ProdDetailID];
                    if (priceRecordsForProduct == null || !priceRecordsForProduct.Any())
                    {
                        priceRecord = new tb_PriceRecord();
                        priceRecord.ProdDetailID = child.ProdDetailID;
                    }
                    else
                    {
                        priceRecord = priceRecordsForProduct.First();
                    }
                    priceRecord.Employee_ID = entity.tb_saleorder.Employee_ID;
                    if (priceRecord.SalePrice != child.TransactionPrice)
                    {
                        priceRecord.SalePrice = child.TransactionPrice;
                        priceRecord.SaleDate = System.DateTime.Now;
                    }
                    priceUpdateList.Add(priceRecord);

                    // 实时检测库存成本，以库存成本为基准
                    if (!child.Cost.Equals(inv.Inv_Cost))
                    {
                        child.Cost = inv.Inv_Cost;
                        child.SubtotalCostAmount = (child.Cost + child.CustomizedCost) * child.Quantity;
                        updateCostList.Add(child);
                    }

                    // 初始化分组数据
                    group = (
                        Inventory: inv,
                        OutQtySum: currentSaleQty,
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
                        string LocationName = group.Inventory.tb_location?.Name;
                        if (group.Inventory.tb_location == null)
                        {
                            var loc = _cacheManager.GetEntity<tb_Location>(group.Inventory.Location_ID);
                            if (loc != null)
                            {
                                LocationName = loc.Name;
                            }
                        }
                        throw new Exception($"SKU:{group.Inventory.tb_proddetail.SKU} {LocationName}库存为：{group.Inventory.Quantity}，拟销售量为：{group.OutQtySum}\r\n 系统设置不允许负库存， 请检查出库数量与库存相关数据");
                    }

                    group.LatestOutboundTime = System.DateTime.Now;
                    inventoryGroups[key] = group;
                }
            }

            // 处理分组数据，更新库存记录的各字段
            // 【死锁优化】按 (ProdDetailID, LocationID) 排序，确保所有事务以相同顺序访问资源
            var sortedInventoryGroups = inventoryGroups
                .OrderBy(g => g.Key.ProdDetailID)
                .ThenBy(g => g.Key.LocationID)
                .ToList();

            foreach (var group in sortedInventoryGroups)
            {
                var inv = group.Value.Inventory;
                int originalQty = inv.Quantity;
                decimal originalCost = inv.Inv_Cost;

                // 检查负库存
                if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - group.Value.OutQtySum.ToInt()) < 0)
                {
                    throw new Exception($"sku:{inv.tb_proddetail.SKU}库存为：{inv.Quantity}，拟销售量为：{group.Value.OutQtySum}\r\n 系统设置不允许负库存， 请检查出库数量与库存相关数据");
                }

                // 累加数值字段
                inv.Sale_Qty -= group.Value.OutQtySum.ToInt();
                inv.Quantity -= group.Value.OutQtySum.ToInt();
                inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                invUpdateList.Add(inv);

                // 创建库存流水记录
                tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                transaction.ProdDetailID = inv.ProdDetailID;
                transaction.Location_ID = inv.Location_ID;
                transaction.BizType = (int)BizType.销售出库单;
                transaction.ReferenceId = entity.SaleOut_MainID;
                transaction.ReferenceNo = entity.SaleOutNo;
                transaction.BeforeQuantity = originalQty;
                transaction.QuantityChange = -group.Value.OutQtySum.ToInt();
                transaction.AfterQuantity = inv.Quantity;
                transaction.UnitCost = inv.Inv_Cost;
                transaction.TransactionTime = DateTime.Now;
                transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                View_ProdDetail obj = _cacheManager.GetEntity<View_ProdDetail>(inv.ProdDetailID);
                if (obj != null)
                {
                    transaction.Notes = $"销售出库单审核：{entity.SaleOutNo}，产品：{obj.SKU}-{obj.CNName}";
                }
                else
                {
                    transaction.Notes = $"销售出库单审核：{entity.SaleOutNo}";
                }
                transactionList.Add(transaction);
            }

            return (inventoryGroups, invUpdateList, transactionList, priceUpdateList, updateCostList);
        }





    }

}



 