
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 10:37:59
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

namespace RUINORERP.Business
{
    /// <summary>
    /// 销售订单
    /// </summary>
    public partial class tb_SaleOrderController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_SaleOrderServices _tb_SaleOrderServices { get; set; }
        // private readonly ApplicationContext _appContext;

        public tb_SaleOrderController(ILogger<tb_SaleOrderController<T>> logger, IUnitOfWorkManage unitOfWorkManage, tb_SaleOrderServices tb_SaleOrderServices, ApplicationContext appContext = null) : base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _tb_SaleOrderServices = tb_SaleOrderServices;
            _appContext = appContext;
        }


        public ValidationResult Validator(tb_SaleOrder info)
        {

            // tb_SaleOrderValidator validator = new tb_SaleOrderValidator();
            tb_SaleOrderValidator validator = _appContext.GetRequiredService<tb_SaleOrderValidator>();
            ValidationResult results = validator.Validate(info);
            return results;
        }

        #region 扩展方法

        /// <summary>
        /// 某字段是否存在
        /// </summary>
        /// <param name="exp">e => e.ModeuleName == mod.ModeuleName</param>
        /// <returns></returns>
        public override async Task<bool> ExistFieldValue(Expression<Func<T, bool>> exp)
        {
            return await _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp).AnyAsync();
        }


        /// <summary>
        /// 雪花ID模式下的新增和修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ReturnResults<tb_SaleOrder>> SaveOrUpdate(tb_SaleOrder entity)
        {
            ReturnResults<tb_SaleOrder> rr = new ReturnResults<tb_SaleOrder>();
            tb_SaleOrder Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SOrder_ID > 0)
                {
                    bool rs = await _tb_SaleOrderServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_SaleOrderServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(entity);
                }

                rr.ReturnObject = Returnobj;
                rr.Succeeded = true;
                entity.ActionStatus = ActionStatus.无操作;
            }
            catch (Exception ex)
            {
                ////这里需要进一步优化处理？
                throw ex;
            }
            return rr;
        }


        /// <summary>
        /// 雪花ID模式下的新增和修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> BaseSaveOrUpdate(T model)
        {
            ReturnResults<T> rr = new ReturnResults<T>();
            tb_SaleOrder entity = model as tb_SaleOrder;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SOrder_ID > 0)
                {
                    bool rs = await _tb_SaleOrderServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_SaleOrderServices.AddReEntityAsync(entity) as T;
                    MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(entity);
                }

                rr.ReturnObject = Returnobj;
                rr.Succeeded = true;
                entity.ActionStatus = ActionStatus.无操作;
            }
            catch (Exception ex)
            {
                ////这里需要进一步优化处理？
                throw ex;
            }
            return rr;
        }

        public async override Task<List<T>> BaseQueryAsync(string wheresql)
        {
            List<T> list = await _tb_SaleOrderServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_SaleOrder entity = item as tb_SaleOrder;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                MyCacheManager.Instance.UpdateEntityList<List<T>>(list);
            }
            return list;
        }

        public async override Task<List<T>> BaseQueryAsync()
        {
            List<T> list = await _tb_SaleOrderServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_SaleOrder entity = item as tb_SaleOrder;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                MyCacheManager.Instance.UpdateEntityList<List<T>>(list);
            }
            return list;
        }


        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_SaleOrder entity = model as tb_SaleOrder;
            bool rs = await _tb_SaleOrderServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrder>(entity);
            }
            return rs;
        }

        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs = false;
            List<tb_SaleOrder> entitys = models as List<tb_SaleOrder>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_SaleOrder>(entitys).ExecuteCommandAsync();
            if (c > 0)
            {
                rs = true;
                ////生成时暂时只考虑了一个主键的情况
                long[] result = entitys.Select(e => e.SOrder_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrder>(result);
            }
            return rs;
        }

        public override ValidationResult BaseValidator(T info)
        {
            //tb_SaleOrderValidator validator = new tb_SaleOrderValidator();
            tb_SaleOrderValidator validator = _appContext.GetRequiredService<tb_SaleOrderValidator>();
            ValidationResult results = validator.Validate(info as tb_SaleOrder);
            return results;
        }


        public async override Task<List<T>> BaseQueryByAdvancedAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync();
        }

        public async override Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model) where C : class
        {
            bool rs = false;
            RevertCommand command = new RevertCommand();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
            //缓存当前编辑的对象。如果撤销就回原来的值
            T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
            try
            {
                tb_SaleOrder entity = model as tb_SaleOrder;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                    //entity.SOrder_ID = 0;
                    //entity.tb_SaleOrderDetails.ForEach(

                    //    c =>
                    //    {
                    //        c.SOrder_ID = 0;
                    //        c.SaleOrderDetail_ID = 0;
                    //    }
                    //    );
                };

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                if (entity.SOrder_ID > 0)
                {

                    rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_SaleOrder>(entity as tb_SaleOrder)
                   .Include(m => m.tb_SaleOrderDetails)
               .ExecuteCommandAsync();
                }
                else
                {
                    rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_SaleOrder>(entity as tb_SaleOrder)
            .Include(m => m.tb_SaleOrderDetails)
            .ExecuteCommandAsync();
                }

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T;
                entity.PrimaryKeyID = entity.SOrder_ID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                //出错后，取消生成的ID等值
                command.Undo();
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
                _logger.Error(ex);
            }

            return rsms;
        }

        #endregion


        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                                .Includes(m => m.tb_SaleOrderDetails)
                        .Includes(m => m.tb_SaleOuts)
                        .Includes(m => m.tb_ProductionPlans)
                        .Includes(m => m.tb_OrderPackings)
                        .Includes(m => m.tb_PurOrders)
                                        .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync() as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model)
        {
            tb_SaleOrder entity = model as tb_SaleOrder;
            bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_SaleOrder>(m => m.SOrder_ID == entity.SOrder_ID)
                               .Include(m => m.tb_SaleOrderDetails)
                       .Include(m => m.tb_SaleOuts)
                       .Include(m => m.tb_ProductionPlans)
                       .Include(m => m.tb_OrderPackings)
                       .Include(m => m.tb_PurOrders)
                                       .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion



        public tb_SaleOrder AddReEntity(tb_SaleOrder entity)
        {
            tb_SaleOrder AddEntity = _tb_SaleOrderServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }

        public async Task<tb_SaleOrder> AddReEntityAsync(tb_SaleOrder entity)
        {
            tb_SaleOrder AddEntity = await _tb_SaleOrderServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }

        public async Task<long> AddAsync(tb_SaleOrder entity)
        {
            long id = await _tb_SaleOrderServices.Add(entity);
            if (id > 0)
            {
                MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(entity);
            }
            return id;
        }

        public async Task<List<long>> AddAsync(List<tb_SaleOrder> infos)
        {
            List<long> ids = await _tb_SaleOrderServices.Add(infos);
            if (ids.Count > 0)//成功的个数 这里缓存 对不对呢？
            {
                MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(infos);
            }
            return ids;
        }


        public async Task<bool> DeleteAsync(tb_SaleOrder entity)
        {
            bool rs = await _tb_SaleOrderServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrder>(entity);

            }
            return rs;
        }

        public async Task<bool> UpdateAsync(tb_SaleOrder entity)
        {
            bool rs = await _tb_SaleOrderServices.Update(entity);
            if (rs)
            {
                MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_SaleOrderServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrder>(id);
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_SaleOrderServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrder>(ids);
            }
            return rs;
        }

        public virtual async Task<List<tb_SaleOrder>> QueryAsync()
        {
            List<tb_SaleOrder> list = await _tb_SaleOrderServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }

        public virtual List<tb_SaleOrder> Query()
        {
            List<tb_SaleOrder> list = _tb_SaleOrderServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }

        public virtual List<tb_SaleOrder> Query(string wheresql)
        {
            List<tb_SaleOrder> list = _tb_SaleOrderServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }

        public virtual async Task<List<tb_SaleOrder>> QueryAsync(string wheresql)
        {
            List<tb_SaleOrder> list = await _tb_SaleOrderServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }



        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_SaleOrder>> QueryAsync(Expression<Func<tb_SaleOrder, bool>> exp)
        {
            List<tb_SaleOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }



        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual async Task<List<tb_SaleOrder>> QueryByNavAsync()
        {
            List<tb_SaleOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                               .Includes(t => t.tb_currency)
                               .Includes(t => t.tb_fm_account)
                               .Includes(t => t.tb_projectgroup)
                               .Includes(t => t.tb_customervendor)
                               .Includes(t => t.tb_employee)
                               .Includes(t => t.tb_paymentmethod)
                                            .Includes(t => t.tb_SaleOrderDetails)
                                .Includes(t => t.tb_SaleOuts)
                                .Includes(t => t.tb_ProductionPlans)
                                .Includes(t => t.tb_OrderPackings)
                                .Includes(t => t.tb_PurOrders)
                        .ToListAsync();

            foreach (var item in list)
            {
                item.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual async Task<List<tb_SaleOrder>> QueryByNavAsync(Expression<Func<tb_SaleOrder, bool>> exp)
        {
            List<tb_SaleOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>().Where(exp)
                               .Includes(t => t.tb_currency)
                               .Includes(t => t.tb_fm_account)
                               .Includes(t => t.tb_projectgroup)
                               .Includes(t => t.tb_customervendor)
                               .Includes(t => t.tb_employee)
                               .Includes(t => t.tb_paymentmethod)
                                            .Includes(t => t.tb_SaleOrderDetails)
                                .Includes(t => t.tb_SaleOuts)
                                .Includes(t => t.tb_ProductionPlans)
                                .Includes(t => t.tb_OrderPackings)
                                .Includes(t => t.tb_PurOrders)
                        .ToListAsync();

            foreach (var item in list)
            {
                item.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual List<tb_SaleOrder> QueryByNav(Expression<Func<tb_SaleOrder, bool>> exp)
        {
            List<tb_SaleOrder> list = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>().Where(exp)
                            .Includes(t => t.tb_currency)
                            .Includes(t => t.tb_fm_account)
                            .Includes(t => t.tb_projectgroup)
                            .Includes(t => t.tb_customervendor)
                            .Includes(t => t.tb_employee)
                            .Includes(t => t.tb_paymentmethod)
                                        .Includes(t => t.tb_SaleOrderDetails)
                            .Includes(t => t.tb_SaleOuts)
                            .Includes(t => t.tb_ProductionPlans)
                            .Includes(t => t.tb_OrderPackings)
                            .Includes(t => t.tb_PurOrders)
                        .ToList();

            foreach (var item in list)
            {
                item.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }



        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_SaleOrder>> QueryByAdvancedAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>().WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_SaleOrderServices.QueryByIdAsync(id) as T;
            return entity;
        }



        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_SaleOrder entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>().Where(w => w.SOrder_ID == (long)id)
                             .Includes(t => t.tb_currency)
                            .Includes(t => t.tb_fm_account)
                            .Includes(t => t.tb_projectgroup)
                            .Includes(t => t.tb_customervendor)
                            .Includes(t => t.tb_employee)
                            .Includes(t => t.tb_paymentmethod)
                                        .Includes(t => t.tb_SaleOrderDetails)
                            .Includes(t => t.tb_SaleOuts)
                            .Includes(t => t.tb_ProductionPlans)
                            .Includes(t => t.tb_OrderPackings)
                            .Includes(t => t.tb_PurOrders)
                        .FirstAsync();
            if (entity != null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(entity);
            return entity as T;
        }






    }
}



