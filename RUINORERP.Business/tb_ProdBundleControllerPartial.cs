
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/31/2024 20:15:39
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
using RUINORERP.Business.Security;
using RUINORERP.Global;

namespace RUINORERP.Business
{
    /// <summary>
    /// 产品套装表
    /// </summary>
    public partial class tb_ProdBundleController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 组合单审核 状态变化而已
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<bool>> AdjustingAsync(List<tb_ProdBundle> entitys, ApprovalEntity approvalEntity)
        {
            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                if (!approvalEntity.ApprovalResults)
                {
                    if (entitys == null)
                    {
                        return rs;
                    }

                }
                else
                {
                    foreach (var entity in entitys)
                    {


                        #region 

                        //这部分是否能提出到上一级公共部分？
                        entity.DataStatus = (int)DataStatus.确认;
                        entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                        //后面已经修改为
                        entity.ApprovalResults = approvalEntity.ApprovalResults;
                        entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                        BusinessHelper.Instance.ApproverEntity(entity);
                        //只更新指定列
                        // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdBundle>(entity).ExecuteCommandAsync();
                        #endregion

                    }

                }

                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                rs.Succeeded = false;
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                {
                    _logger.Error(approvalEntity.ToString() + "事务回滚" + ex.Message);
                }
                return rs;
            }

        }



        /// <summary>
        ///组合单反审  母件减少，子件增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<bool>> AntiApprovalAsync(List<tb_ProdBundle> entitys)
        {
            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                foreach (var entity in entitys)
                {
                    if (entity == null)
                    {
                        continue;
                    }

                    //这部分是否能提出到上一级公共部分？
                    entity.DataStatus = (int)DataStatus.新建;
                    entity.ApprovalResults = null;
                    entity.ApprovalStatus = (int)ApprovalStatus.未审核;

                    BusinessHelper.Instance.ApproverEntity(entity);
                    //只更新指定列
                    // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdBundle>(entity).ExecuteCommandAsync();
                    //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                }

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                rs.Succeeded = false;
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }

        }



        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            // var queryable = _appContext.Db.Queryable<tb_SaleOutDetail>();
            // var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();

            List<tb_ProdBundle> list = await _appContext.Db.CopyNew().Queryable<tb_ProdBundle>().Where(m => m.BundleID == ID)
                             .Includes(a => a.tb_Packings)
                            .Includes(a => a.tb_ProdBundleDetails)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_ProdBundleDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }





    }
}



