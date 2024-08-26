
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
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Business.Security;

namespace RUINORERP.Business
{

    public partial class tb_FM_ExpenseClaimController<T> : BaseController<T> where T : class
    {

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
               // entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
             //   entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ExpenseClaim>(entity).ExecuteCommandAsync();
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;
               // _logger.Info(approvalEntity.bizName + "审核事务成功");
                return rmrs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                {
                    _logger.Error( "事务回滚" + ex.Message);
                }
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }

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
                        // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ExpenseClaim>(entity).ExecuteCommandAsync();
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

 

        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_ExpenseClaim> list = await _appContext.Db.CopyNew().Queryable<tb_FM_ExpenseClaim>().Where(m => m.ClaimMainID == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_fm_expenseclaim)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_department)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_projectgroup)
                             .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_fm_account)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_fm_expensetype)
                            .Includes(a => a.tb_FM_ExpenseClaimDetails, b => b.tb_fm_subject)
                                 .ToListAsync();
            return list as List<T>;
        }

 

 
    }

}



