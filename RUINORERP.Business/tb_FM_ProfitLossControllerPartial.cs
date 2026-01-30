// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:06
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
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using RUINORERP.Business.CommService;
using System.Threading;
using RUINORERP.Business.EntityLoadService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 损益费用单
    /// </summary>
    public partial class tb_FM_ProfitLossController<T> : BaseController<T> where T : class
    {

        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_ProfitLoss entity = ObjectEntity as tb_FM_ProfitLoss;

            try
            {

                // 获取当前状态
                var statusProperty = typeof(DataStatus).Name;
                var currentStatus = (DataStatus)Enum.ToObject(
                    typeof(DataStatus),
                    entity.GetPropertyValue(statusProperty)
                );

                var ValidateValue = StateManager.ValidateBusinessStatusTransitionAsync(currentStatus, DataStatus.新建 as Enum);
                if (!ValidateValue.IsSuccess)
                {
                    rmrs.ErrorMsg = $"状态为【{currentStatus.ToString()}】的{((ProfitLossDirection)entity.ProfitLossDirection).ToString()}确认单不可以反审";
                    return rmrs;
                }


                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.DataStatus,
                    it.ApprovalStatus,
                    it.ApprovalResults,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;

                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }

        }

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            return await ApprovalAsync(ObjectEntity, false);
        }

        /// <summary>
        /// 这个审核可以由业务来审。后面还会有财务来定是否真实收付，这财务审核收款单前，还是可以反审的
        /// 审核通过时生成关联的收款/付款草稿单 财务核对是否到账。
        /// 核销记录：预收款抵扣应收款（正向核销）
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity, bool IsAutoApprove = false)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_ProfitLoss entity = ObjectEntity as tb_FM_ProfitLoss;
            try
            {
                if (entity == null)
                {
                    rmrs.ErrorMsg = "无效的单据";
                    return rmrs;
                }
                // 获取当前状态
                var statusProperty = typeof(DataStatus).Name;
                var currentStatus = (DataStatus)Enum.ToObject(
                    typeof(DataStatus),
                    entity.GetPropertyValue(statusProperty)
                );



                //得到当前实体对应的业务类型
                //var _mapper = _appContext.GetRequiredService<EnhancedBizTypeMapper>();
                //var bizType = _BizMapperService.EntityMappingHelper.GetBizType(typeof(T), entity);
                //应收款中不能存在相同的来源的 正数金额的出库单的应收数据
                //一个出库不能多次应收。一个出库一个应收（负数除外）。一个应收可以多次收款来抵扣
                if (entity.SourceBizType.HasValue && entity.SourceBillId.HasValue)
                {
                    //审核时 要检测明细中对应的相同业务类型下不能有相同来源单号。除非有正负总金额为0对冲情况。或是两行数据?
                    var existingRecords = await _appContext.Db.Queryable<tb_FM_ProfitLoss>()
                        .Includes(c => c.tb_FM_ProfitLossDetails)
                        .Where(c => c.DataStatus >= (int)DataStatus.确认)
                        .Where(c => c.SourceBizType == entity.SourceBizType && c.SourceBillId == entity.SourceBillId)
                        .Select(c => new { c.ProfitLossNo, c.Created_at })
                        .ToListAsync();

                    //检测是否存在相同业务类型和来源单号的记录
                    if (existingRecords.Any())
                    {
                        //收集重复的单号信息
                        var duplicateBillNumbers = string.Join(", ", existingRecords.Select(r => r.ProfitLossNo));

                        rmrs.ErrorMsg = $"检测到重复：业务类型{(BizType)entity.SourceBizType}下的来源单号{entity.SourceBillNo}已存在于以下损溢单中：{duplicateBillNumbers}，不允许重复生成损溢单费用！审核失败。";
                        rmrs.Succeeded = false;
                        rmrs.ReturnObject = entity as T;
                        return rmrs;
                    }
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                entity.ApprovalResults = true;

                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.DataStatus,
                    it.ApprovalStatus,
                    it.ApprovalResults,
                    it.ApprovalOpinions,
                }).ExecuteCommandAsync();
                if (result <= 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    rmrs.ErrorMsg = "更新结果为零，请确保数据完整。请检查当前单据数据是否存在。";
                    return rmrs;
                }

                //
                //结案来源单？
                if (entity.SourceBizType == (int)BizType.盘点单)
                {
                    int borrowUpdateResult = await _appContext.Db.Updateable<tb_Stocktake>()
                                   .SetColumns(it => new tb_Stocktake() { DataStatus = (int)DataStatus.完结, Notes = it.Notes + "盘点确认费用结案" })
                                   .Where(it => it.MainID == entity.SourceBillId)
                                   .ExecuteCommandAsync();
                }

                if (entity.SourceBizType == (int)BizType.借出单)
                {
                    int borrowUpdateResult = await _appContext.Db.Updateable<tb_ProdBorrowing>()
                                   .SetColumns(it => new tb_ProdBorrowing() { DataStatus = (int)DataStatus.完结, CloseCaseOpinions = "借出确认费用结案" })
                                   .Where(it => it.BorrowID == entity.SourceBillId)
                                   .ExecuteCommandAsync();
                }

                //if (entity.SourceBizType == (int)BizType.应付款单 || entity.SourceBizType == (int)BizType.应收款单)
                //{
                //    int borrowUpdateResult = await _appContext.Db.Updateable<tb_FM_ReceivablePayable>()
                //                   .SetColumns(it => new tb_FM_ReceivablePayable() { ARAPStatus = (int)ARAPStatus.坏账, Remark = it.Remark + ((BizType)entity.SourceBizType.Value).ToString() + "坏账确认费用结案" })
                //                   .Where(it => it.ARAPId == entity.SourceBillId)
                //                   .ExecuteCommandAsync();
                //}



                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }
        }

        /// <summary>
        /// 应收应付转费用单还要完善TODO 要根据收付款方式和金额的正负数来确定最后的损失方向
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<tb_FM_ProfitLoss> BuildProfitLoss(tb_FM_ReceivablePayable entity)
        {
            #region 创建损溢单费用单

            tb_FM_ProfitLoss profitLoss = new tb_FM_ProfitLoss();
            profitLoss = mapper.Map<tb_FM_ProfitLoss>(entity);
            profitLoss.ApprovalResults = null;
            profitLoss.ApprovalStatus = (int)ApprovalStatus.未审核;
            profitLoss.Approver_at = null;
            profitLoss.Approver_by = null;
            profitLoss.PrintStatus = 0;
            profitLoss.ActionStatus = ActionStatus.新增;
            profitLoss.ApprovalOpinions = "";
            profitLoss.Modified_at = null;
            profitLoss.Modified_by = null;

            profitLoss.SourceBillNo = entity.ARAPNo;
            profitLoss.SourceBillId = entity.ARAPId;

            tb_FM_ProfitLossDetail profitLossDetail = new tb_FM_ProfitLossDetail();
            #region 明细   一笔预收付款单只有一条明细


            profitLossDetail.Quantity = 1;
            profitLossDetail.UnitPrice = Math.Abs(entity.LocalBalanceAmount);
            profitLossDetail.TaxSubtotalAmont = entity.TaxTotalAmount;
            profitLossDetail.SubtotalAmont = profitLossDetail.UnitPrice.Value * profitLossDetail.Quantity.Value;
            profitLossDetail.UntaxedSubtotalAmont = profitLossDetail.SubtotalAmont - profitLossDetail.TaxSubtotalAmont;
            profitLossDetail.ExpenseDescription = "应" + ((ReceivePaymentType)entity.ReceivePaymentType).ToString() + "单";


            #endregion


            //收不到，就是损失
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                profitLoss.SourceBizType = (int)BizType.应收款单;
                if (entity.LocalBalanceAmount > 0)
                {
                    profitLoss.ProfitLossDirection = (int)ProfitLossDirection.损失;
                    // 确保异步调用正确执行
                    profitLoss.ProfitLossNo = await Task.Run(async () =>
                        await bizCodeService.GenerateBizBillNoAsync(BizType.损失确认单, CancellationToken.None));
                    profitLossDetail.IncomeExpenseDirection = (int)IncomeExpenseDirection.支出;
                }
                else
                {
                    //如果收款是负数， 则是退款（不退就算收入）
                    profitLoss.ProfitLossDirection = (int)ProfitLossDirection.溢余;
                    // 确保异步调用正确执行
                    profitLoss.ProfitLossNo = await Task.Run(async () =>
                        await bizCodeService.GenerateBizBillNoAsync(BizType.溢余确认单, CancellationToken.None));
                    profitLossDetail.ExpenseDescription += "的退款";
                    profitLossDetail.IncomeExpenseDirection = (int)IncomeExpenseDirection.收入;
                }
            }
            else
            {
                profitLoss.ProfitLossDirection = (int)ProfitLossDirection.溢余;
                profitLoss.SourceBizType = (int)BizType.应付款单;
                if (entity.LocalBalanceAmount > 0)
                {
                    profitLoss.ProfitLossDirection = (int)ProfitLossDirection.溢余;
                    // 确保异步调用正确执行
                    profitLoss.ProfitLossNo = await Task.Run(async () =>
                        await bizCodeService.GenerateBizBillNoAsync(BizType.溢余确认单, CancellationToken.None));
                    profitLossDetail.IncomeExpenseDirection = (int)IncomeExpenseDirection.收入;
                }
                else
                {
                    //如果付款是负数， 则是供应商退款（不退就算损失）
                    profitLoss.ProfitLossDirection = (int)ProfitLossDirection.损失;
                    // 确保异步调用正确执行
                    profitLoss.ProfitLossNo = await Task.Run(async () =>
                        await bizCodeService.GenerateBizBillNoAsync(BizType.损失确认单, CancellationToken.None));
                    profitLossDetail.ExpenseDescription += "的退款";
                    profitLossDetail.IncomeExpenseDirection = (int)IncomeExpenseDirection.支出;
                }
            }
            switch (entity.SourceBizType)
            {
                case (int)BizType.销售出库单:

                    break;
                case (int)BizType.销售退回单:
                case (int)BizType.采购入库单:
                case (int)BizType.采购退货单:

                    break;
                default:

                    break;
            }

            profitLoss.PostDate = DateTime.Now;
            if (entity.tb_projectgroup != null && entity.tb_projectgroup.DepartmentID.HasValue)
            {
                profitLoss.DepartmentID = entity.tb_projectgroup.DepartmentID;
            }
            profitLoss.ProjectGroup_ID = entity.ProjectGroup_ID;
            profitLoss.DepartmentID = entity.DepartmentID;


            List<tb_FM_ProfitLossDetail> details = mapper.Map<List<tb_FM_ProfitLossDetail>>(entity.tb_FM_ReceivablePayableDetails);
            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                ////这个写法，如果原来明细中  相同产品ID 多行录入。就会出错。混乱。
                ///如果来源和目录字段值一致。mapper应该完成映射。目前为了保险，在目标明细实体中添加一个 原始的主键ID
                var olditem = entity.tb_FM_ReceivablePayableDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID
                && c.SourceItemRowID == details[i].SourceItemRowID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].SubtotalAmont = olditem.LocalPayableAmount;
                }
                details[i].ProfitLossType = (int)ProfitLossType.坏账准备;
            }

            profitLoss.tb_FM_ProfitLossDetails = details;
            profitLoss.TotalAmount = profitLoss.tb_FM_ProfitLossDetails.Sum(c => c.SubtotalAmont);
            profitLoss.TaxTotalAmount = profitLoss.tb_FM_ProfitLossDetails.Sum(c => c.TaxSubtotalAmont);
            profitLoss.UntaxedTotalAmont = profitLoss.TotalAmount - profitLoss.TaxTotalAmount;
            if (profitLoss.TaxTotalAmount > 0)
            {
                profitLoss.IsIncludeTax = true;
            }

            profitLoss.DataStatus = (int)DataStatus.草稿;
            profitLoss.ActionStatus = ActionStatus.新增;
            Business.BusinessHelper.Instance.InitEntity(profitLoss);

            #endregion
            return profitLoss;
        }


        /// <summary>
        /// 盘点单转为费用单：通常盘点明细会有正负所以会同时转为两张单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_ProfitLoss>> BuildProfitLoss(tb_Stocktake entity)
        {
            //最后根据总的情况来判断单据类型
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();

            List<tb_FM_ProfitLoss> losts = new List<tb_FM_ProfitLoss>();
            if (entity.Adjust_Type == (int)Adjust_Type.全部 || entity.Adjust_Type == (int)Adjust_Type.增加)
            {
                #region 创建损溢单费用单

                tb_FM_ProfitLoss profitLoss = new tb_FM_ProfitLoss();
                profitLoss = mapper.Map<tb_FM_ProfitLoss>(entity);
                profitLoss.ApprovalResults = null;
                profitLoss.ApprovalStatus = (int)ApprovalStatus.未审核;
                profitLoss.Approver_at = null;
                profitLoss.Approver_by = null;
                profitLoss.PrintStatus = 0;
                profitLoss.ActionStatus = ActionStatus.新增;
                profitLoss.ApprovalOpinions = "";
                profitLoss.Modified_at = null;
                profitLoss.Modified_by = null;

                profitLoss.SourceBillNo = entity.CheckNo;
                profitLoss.SourceBillId = entity.MainID;
                profitLoss.SourceBizType = (int)BizType.盘点单;
                profitLoss.PostDate = DateTime.Now;
                //如果是全部则有正有负，如果是减少则损失（负），如果是增加则正
                List<tb_FM_ProfitLossDetail> details = mapper.Map<List<tb_FM_ProfitLossDetail>>(entity.tb_StocktakeDetails.Where(c => c.DiffQty > 0));
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    ////这个写法，如果原来明细中  相同产品ID 多行录入。就会出错。混乱。
                    ///如果来源和目录字段值一致。mapper应该完成映射。目前为了保险，在目标明细实体中添加一个 原始的主键ID
                    var olditem = entity.tb_StocktakeDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID
                    && c.SubID == details[i].SourceItemRowID).FirstOrDefault();
                    if (olditem != null)
                    {
                        details[i].SubtotalAmont = Math.Abs(olditem.DiffSubtotalAmount);
                    }
                    if (details[i].SubtotalAmont > 0)
                    {
                        details[i].ProfitLossType = (int)ProfitLossType.存货盘盈收益;
                        details[i].IncomeExpenseDirection = (int)IncomeExpenseDirection.收入;
                    }
                    details[i].ActionStatus = ActionStatus.新增;
                }

          
                profitLoss.TotalAmount = profitLoss.tb_FM_ProfitLossDetails.Sum(c => c.SubtotalAmont);
                profitLoss.TaxTotalAmount = profitLoss.tb_FM_ProfitLossDetails.Sum(c => c.TaxSubtotalAmont);
                profitLoss.UntaxedTotalAmont = profitLoss.TotalAmount - profitLoss.TaxTotalAmount;
                if (profitLoss.TaxTotalAmount > 0)
                {
                    profitLoss.IsIncludeTax = true;
                }

                profitLoss.ProfitLossDirection = (int)ProfitLossDirection.溢余;
                profitLoss.ProfitLossNo = await bizCodeService.GenerateBizBillNoAsync(BizType.溢余确认单, CancellationToken.None);

                profitLoss.DataStatus = (int)DataStatus.草稿;
                Business.BusinessHelper.Instance.InitEntity(profitLoss);

                #endregion
                losts.Add(profitLoss);
            }

            if (entity.Adjust_Type == (int)Adjust_Type.全部 || entity.Adjust_Type == (int)Adjust_Type.减少)
            {
                #region 创建损溢单费用单

                tb_FM_ProfitLoss profitLoss = new tb_FM_ProfitLoss();
                profitLoss = mapper.Map<tb_FM_ProfitLoss>(entity);
                profitLoss.ApprovalResults = null;
                profitLoss.ApprovalStatus = (int)ApprovalStatus.未审核;
                profitLoss.Approver_at = null;
                profitLoss.Approver_by = null;
                profitLoss.PrintStatus = 0;
                profitLoss.ActionStatus = ActionStatus.新增;
                profitLoss.ApprovalOpinions = "";
                profitLoss.Modified_at = null;
                profitLoss.Modified_by = null;

                profitLoss.SourceBillNo = entity.CheckNo;
                profitLoss.SourceBillId = entity.MainID;
                profitLoss.SourceBizType = (int)BizType.盘点单;
                profitLoss.PostDate = DateTime.Now;

                List<tb_FM_ProfitLossDetail> details = mapper.Map<List<tb_FM_ProfitLossDetail>>(entity.tb_StocktakeDetails.Where(c => c.DiffQty < 0));
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    ////这个写法，如果原来明细中  相同产品ID 多行录入。就会出错。混乱。
                    ///如果来源和目录字段值一致。mapper应该完成映射。目前为了保险，在目标明细实体中添加一个 原始的主键ID
                    var olditem = entity.tb_StocktakeDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID
                    && c.SubID == details[i].SourceItemRowID).FirstOrDefault();
                    if (olditem != null)
                    {
                        details[i].SubtotalAmont = olditem.DiffSubtotalAmount;
                    }

                    details[i].ProfitLossType = (int)ProfitLossType.存货盘亏损失;
                    details[i].IncomeExpenseDirection = (int)IncomeExpenseDirection.支出;

                    //有方向字段标识
                    //费用金额取正数
                    details[i].SubtotalAmont = Math.Abs(details[i].SubtotalAmont);
                    details[i].ActionStatus = ActionStatus.新增;
                }

                profitLoss.tb_FM_ProfitLossDetails = details;

                profitLoss.TotalAmount = profitLoss.tb_FM_ProfitLossDetails.Sum(c => c.SubtotalAmont);
                profitLoss.TaxTotalAmount = profitLoss.tb_FM_ProfitLossDetails.Sum(c => c.TaxSubtotalAmont);
                profitLoss.UntaxedTotalAmont = profitLoss.TotalAmount - profitLoss.TaxTotalAmount;


                if (profitLoss.TaxTotalAmount > 0)
                {
                    profitLoss.IsIncludeTax = true;
                }

                profitLoss.ProfitLossDirection = (int)ProfitLossDirection.损失;
                profitLoss.ProfitLossNo = await bizCodeService.GenerateBizBillNoAsync(BizType.损失确认单, CancellationToken.None);

                profitLoss.DataStatus = (int)DataStatus.草稿;
                Business.BusinessHelper.Instance.InitEntity(profitLoss);

                #endregion
                losts.Add(profitLoss);
            }
            return losts;
        }

        /// <summary>
        /// 借出单转费用单，按成本算损失金额
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<tb_FM_ProfitLoss> BuildProfitLoss(tb_ProdBorrowing entity)
        {
            #region 创建损溢单费用单

            tb_FM_ProfitLoss profitLoss = new tb_FM_ProfitLoss();
            profitLoss = mapper.Map<tb_FM_ProfitLoss>(entity);
            profitLoss.ApprovalResults = null;
            profitLoss.ApprovalStatus = (int)ApprovalStatus.未审核;
            profitLoss.Approver_at = null;
            profitLoss.Approver_by = null;
            profitLoss.PrintStatus = 0;
            profitLoss.ActionStatus = ActionStatus.新增;
            profitLoss.ApprovalOpinions = "";
            profitLoss.Modified_at = null;
            profitLoss.Modified_by = null;

            profitLoss.SourceBillNo = entity.BorrowNo;
            profitLoss.SourceBillId = entity.BorrowID;

            //收不到，就是损失
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            profitLoss.SourceBizType = (int)BizType.借出单;

            profitLoss.ProfitLossDirection = (int)ProfitLossDirection.损失;
            profitLoss.ProfitLossNo = await bizCodeService.GenerateBizBillNoAsync(BizType.损失确认单, CancellationToken.None);

            profitLoss.PostDate = DateTime.Now;
            profitLoss.DepartmentID = entity.DepartmentID;
            profitLoss.ProjectGroup_ID = entity.ProjectGroup_ID;

            List<tb_FM_ProfitLossDetail> details = mapper.Map<List<tb_FM_ProfitLossDetail>>(entity.tb_ProdBorrowingDetails);
            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                ////这个写法，如果原来明细中  相同产品ID 多行录入。就会出错。混乱。
                ///如果来源和目录字段值一致。mapper应该完成映射。目前为了保险，在目标明细实体中添加一个 原始的主键ID
                var olditem = entity.tb_ProdBorrowingDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID
                && c.BorrowDetaill_ID == details[i].SourceItemRowID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].SubtotalAmont = olditem.SubtotalCostAmount;
                }
                details[i].IncomeExpenseDirection = (int)IncomeExpenseDirection.支出;
                details[i].ActionStatus = ActionStatus.新增;
                details[i].ProfitLossType = (int)ProfitLossType.样品赠送支出;
            }

            profitLoss.tb_FM_ProfitLossDetails = details;
            profitLoss.TotalAmount = profitLoss.tb_FM_ProfitLossDetails.Sum(c => c.SubtotalAmont);
            profitLoss.TaxTotalAmount = profitLoss.tb_FM_ProfitLossDetails.Sum(c => c.TaxSubtotalAmont);
            profitLoss.UntaxedTotalAmont = profitLoss.TotalAmount - profitLoss.TaxTotalAmount;
            if (profitLoss.TaxTotalAmount > 0)
            {
                profitLoss.IsIncludeTax = true;
            }

            profitLoss.DataStatus = (int)DataStatus.草稿;
            Business.BusinessHelper.Instance.InitEntity(profitLoss);

            #endregion
            return profitLoss;
        }
    }
}



