
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/18/2025 10:33:39
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
using RUINORERP.Business.CommService;
using RUINORERP.Global;

namespace RUINORERP.Business
{
    /// <summary>
    /// 蓄水登记表
    /// </summary>
    public partial class tb_EOP_WaterStorageController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// BOM审核，改变本身状态，修改对应母件的详情中的BOMID
        /// BOM明细成本不能为0，如果是新物料，后面会采购入库时覆盖这个物料成本变为最新成本
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_EOP_WaterStorage entity = ObjectEntity as tb_EOP_WaterStorage;

            try
            {

                tb_EOP_WaterStorageController<tb_EOP_WaterStorage> ctrinv = _appContext.GetRequiredService<tb_EOP_WaterStorageController<tb_EOP_WaterStorage>>();
                if (entity == null)
                {
                    return rmrs;
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalResults = true;
                //后面已经修改为
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                    .ExecuteCommandHasChangeAsync();

                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {

                return rmrs;
            }

        }


        /// <summary>
        /// 反审核  并且要修改写到产品表中的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rs = new ReturnResults<T>();
            tb_EOP_WaterStorage entity = ObjectEntity as tb_EOP_WaterStorage;

            try
            {

                entity.DataStatus = (int)DataStatus.新建;
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions,
                                        it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                    .ExecuteCommandHasChangeAsync();

                // 注意信息的完整性
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {

                rs.Succeeded = false;
                return rs;
            }

        }



        /// <summary>
        /// 某字段是否存在
        /// </summary>
        /// <param name="exp">e => e.ModeuleName == mod.ModeuleName</param>
        /// <returns></returns>
        public T ExistFieldValueWithReturn(Expression<Func<T, bool>> exp)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>()
                .Where(exp)
                .First();
        }



    }
}



