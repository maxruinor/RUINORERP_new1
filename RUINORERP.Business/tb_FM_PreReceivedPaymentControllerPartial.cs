
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
using RUINORERP.Global.EnumExt;
using AutoMapper;

namespace RUINORERP.Business
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PreReceivedPaymentController<T> : BaseController<T> where T : class
    {
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PreReceivedPayment entity = ObjectEntity as tb_FM_PreReceivedPayment;


            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                entity.FMPaymentStatus = (int)FMPaymentStatus.草稿;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).ExecuteCommandAsync();
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;

                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "事务回滚" + ex.Message);
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }

        }
        
        /// <summary>
        /// 审核通过时 自动生成付款单
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PreReceivedPayment entity = ObjectEntity as tb_FM_PreReceivedPayment;


            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //预收付款单 审核时 自动生成 收付款记录
                IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
                tb_FM_PaymentRecord paymentRecord = new tb_FM_PaymentRecord();
                paymentRecord = mapper.Map<tb_FM_PaymentRecord>(entity);

                //自动提交
                paymentRecord.FMPaymentStatus = (int)FMPaymentStatus.提交;
                BusinessHelper.Instance.InitEntity(paymentRecord);
                long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecord>(paymentRecord).ExecuteReturnSnowflakeIdAsync();
                if (id > 0)
                {

                }
                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                {

                }

                entity.FMPaymentStatus = (int)FMPaymentStatus.已审核;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).ExecuteCommandAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;
                return rmrs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "事务回滚" + ex.Message);
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }

        }

        public async Task<bool> BaseLogicDeleteAsync(tb_FM_PreReceivedPayment ObjectEntity)
        {
            //  ReturnResults<tb_FM_PreReceivedPaymentController> rrs = new Business.ReturnResults<tb_FM_PreReceivedPaymentController>();
            int count = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PreReceivedPayment>(ObjectEntity).IsLogic().ExecuteCommandAsync();
            if (count > 0)
            {
                //rrs.Succeeded = true;
                return true;
                ////生成时暂时只考虑了一个主键的情况
                // MyCacheManager.Instance.DeleteEntityList<tb_FM_PreReceivedPaymentController>(entity);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async virtual Task<bool> BatchApproval(List<tb_FM_PreReceivedPayment> entitys, ApprovalEntity approvalEntity)
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
                        entity.FMPaymentStatus = (int)FMPaymentStatus.已审核;
                        entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
                        //后面已经修改为
                        entity.ApprovalResults = approvalEntity.ApprovalResults;
                        entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                        BusinessHelper.Instance.ApproverEntity(entity);
                        //只更新指定列
                        // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).ExecuteCommandAsync();
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
            List<tb_FM_PreReceivedPayment> entitys = new List<tb_FM_PreReceivedPayment>();
            entitys = NeedCloseCaseList as List<tb_FM_PreReceivedPayment>;

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
                    if (entitys[m].FMPaymentStatus != (int)FMPaymentStatus.已冲销 || !entitys[m].ApprovalResults.HasValue)
                    {
                        //return false;
                        continue;
                    }
                    //这部分是否能提出到上一级公共部分？
                    entitys[m].FMPaymentStatus = (int)FMPaymentStatus.已冲销;
                    BusinessHelper.Instance.EditEntity(entitys[m]);
                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient()
                        .Updateable<tb_FM_PreReceivedPayment>(entitys[m])
                        .UpdateColumns(it => new
                        {
                            it.FMPaymentStatus,
                            it.ApprovalStatus,
                            it.ApprovalResults,
                            it.ApprovalOpinions,
                            it.PaymentImagePath,
                            it.Modified_by,
                            it.Modified_at,
                            it.Remark
                        }).ExecuteCommandAsync();
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
            List<tb_FM_PreReceivedPayment> list = await _appContext.Db.CopyNew().Queryable<tb_FM_PreReceivedPayment>()
                .Where(m => m.PreRPID == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_paymentmethod)
                            .Includes(a => a.tb_projectgroup)
                            .Includes(a => a.tb_department)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_fm_account)
                            .ToListAsync();
            return list as List<T>;
        }




    }

}



