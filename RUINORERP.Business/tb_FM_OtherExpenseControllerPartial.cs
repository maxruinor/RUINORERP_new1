
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

    public partial class tb_FM_OtherExpenseController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="approvalEntity"></param>
        /// <returns></returns>


        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rrs = new ReturnResults<T>();
            tb_FM_OtherExpense entity = ObjectEntity as tb_FM_OtherExpense;

            try
            {

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();


                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                // entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_OtherExpense>(entity).ExecuteCommandAsync();


                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rrs.ReturnObject = entity as T;
                rrs.Succeeded = true;
                return rrs;
            }
            catch (Exception ex)
            {
         
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "事务回滚" + ex.Message);
                rrs.ErrorMsg = ex.Message;
                rrs.Succeeded = false;
                return rrs;
            }

        }


        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>


        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rrs = new ReturnResults<T>();
            tb_FM_OtherExpense entity = ObjectEntity as tb_FM_OtherExpense;

            try
            {

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "反审核";
                //后面已经修改为
                entity.ApprovalResults = null;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_OtherExpense>(entity).ExecuteCommandAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rrs.Succeeded = true;
                return rrs;
            }
            catch (Exception ex)
            {
              
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rrs.ErrorMsg = ex.Message;
                rrs.Succeeded = false;
                return rrs;
            }

        }



        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_OtherExpense> list = await _appContext.Db.CopyNew().Queryable<tb_FM_OtherExpense>().Where(m => m.ExpenseMainID == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_FM_OtherExpenseDetails, b => b.tb_customervendor)
                            .Includes(a => a.tb_FM_OtherExpenseDetails, b => b.tb_department)
                            .Includes(a => a.tb_FM_OtherExpenseDetails, b => b.tb_projectgroup)
                             .Includes(a => a.tb_FM_OtherExpenseDetails, b => b.tb_fm_account)
                            .Includes(a => a.tb_FM_OtherExpenseDetails, b => b.tb_fm_expensetype)
                            .Includes(a => a.tb_FM_OtherExpenseDetails, b => b.tb_fm_subject)
                              .Includes(a => a.tb_FM_OtherExpenseDetails, b => b.tb_employee)
                                 .ToListAsync();
            return list as List<T>;
        }






    }

}



