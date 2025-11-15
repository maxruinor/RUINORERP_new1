
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/11/2024 00:33:16
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
using RUINORERP.Global;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.BizMapperService;

namespace RUINORERP.Business
{

    public partial class tb_FM_ExpenseClaimController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 费用报销反审
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_ExpenseClaim entity = ObjectEntity as tb_FM_ExpenseClaim;


            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
 

 
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                    .ExecuteCommandHasChangeAsync();

                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    //注意，反审 将对应的预付生成的收款单，只有收款单没有审核前，可以删除
                    //不能直接删除上级。要让对应的人员自己删除。不然不清楚。逻辑也不对。只能通过判断
                    var PaymentRecordlist = await _appContext.Db.Queryable<tb_FM_PaymentRecord>()
                            .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == entity.ClaimMainID))
                              .ToListAsync();
                    if (PaymentRecordlist != null && PaymentRecordlist.Count > 0)
                    {
                        //判断是否能反审? 如果出库是草稿，订单反审 修改后。出库再提交 审核。所以 出库审核要核对订单数据。
                        if ((PaymentRecordlist.Any(c => c.PaymentStatus == (int)PaymentStatus.已支付)
                            && PaymentRecordlist.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                        {
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = "存在【已支付】的付款单，不能反审,请联系上级财务，或作退回处理。";
                            rmrs.Succeeded = false;
                            return rmrs;
                        }
                        else
                        {
                            foreach (var item in PaymentRecordlist)
                            {
                                //删除对应生成的收款单
                                await _appContext.Db.DeleteNav<tb_FM_PaymentRecord>(item)
                                    .Include(c => c.tb_FM_PaymentRecordDetails)
                                    .ExecuteCommandAsync();
                            }

                        }

                    }
                }



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
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_ExpenseClaim entity = ObjectEntity as tb_FM_ExpenseClaim;


            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, 
                                        it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                    .ExecuteCommandHasChangeAsync();

                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    //简化流程，不算债权关系，
                    //更新财务模块 
                    var paymentController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                    tb_FM_PaymentRecord paymentRecord =await paymentController.BuildPaymentRecord(entity);
                    await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord, false);
                    //等待审核
                }

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

        public async Task<bool> BaseLogicDeleteAsync(tb_FM_ExpenseClaim ObjectEntity)
        {
          //  ReturnResults<tb_FM_ExpenseClaim> rrs = new Business.ReturnResults<tb_FM_ExpenseClaim>();
            int count = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_ExpenseClaim>(ObjectEntity).IsLogic().ExecuteCommandAsync();
            if (count > 0)
            {
                //rrs.Succeeded = true;
                return true;
                ////生成时暂时只考虑了一个主键的情况
                // Cache.EntityCacheHelper.DeleteEntityList<tb_FM_ExpenseClaim>(entity);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async virtual Task<bool> BatchApproval(List<tb_FM_ExpenseClaim> entitys, ApprovalEntity approvalEntity)
        {
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                if (!approvalEntity.ApprovalResults)
                {
                    if (entitys == null)
                    {
                        return false;
                    }
                }
                else
                {
                    foreach (var entity in entitys)
                    {
                        //这部分是否能提出到上一级公共部分？
                        entity.DataStatus = (int)DataStatus.确认;
                        entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
                        //后面已经修改为
                        entity.ApprovalResults = approvalEntity.ApprovalResults;
                        entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                        BusinessHelper.Instance.ApproverEntity(entity);
                        //只更新指定列
                        var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                            .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, 
                                                it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                            .ExecuteCommandHasChangeAsync();
                    }
                }
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();

                //_logger.Info(approvalEntity.bizName + "审核事务成功");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                _logger.Error(approvalEntity.bizName + "事务回滚");
                return false;
            }

        }
        /// <summary>
        /// 批量结案  销售订单标记结案，数据状态为8, 
        /// 如果还没有出库。但是结案的订单时。修正拟出库数量。
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案 是仓库和业务确定这个订单不再执行的一个确认过程。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_FM_ExpenseClaim> entitys = new List<tb_FM_ExpenseClaim>();
            entitys = NeedCloseCaseList as List<tb_FM_ExpenseClaim>;

            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                #region 结案
                //更新拟销售量  减少
                for (int m = 0; m < entitys.Count; m++)
                {
                    //判断 能结案的 是确认审核过的。
                    if (entitys[m].DataStatus != (int)DataStatus.确认 || !entitys[m].ApprovalResults.HasValue)
                    {
                        //return false;
                        continue;
                    }
                    //这部分是否能提出到上一级公共部分？
                    entitys[m].DataStatus = (int)DataStatus.完结;
                    BusinessHelper.Instance.EditEntity(entitys[m]);
                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ExpenseClaim>(entitys[m]).UpdateColumns(it => new { it.DataStatus, it.CloseCaseOpinions, it.CloseCaseImagePath, it.Modified_by, it.Modified_at, it.Notes }).ExecuteCommandAsync();
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

        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_ExpenseClaim> list = await _appContext.Db.CopyNew().Queryable<tb_FM_ExpenseClaim>().Where(m => m.ClaimMainID == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_fm_payeeinfo)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_fm_expenseclaim)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_department)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_projectgroup)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_fm_account)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_fm_expensetype)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_fm_subject)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_fm_subject)
                            .ToListAsync();
            return list as List<T>;
        }




    }

}



