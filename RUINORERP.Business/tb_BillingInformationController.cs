// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:42:59
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
using RUINORERP.Business.Cache;

namespace RUINORERP.Business
{
    /// <summary>
    /// 开票资料表
    /// </summary>
    public partial class tb_BillingInformationController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_BillingInformationServices _tb_BillingInformationServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager;
        // private readonly ApplicationContext _appContext;

        public tb_BillingInformationController(ILogger<tb_BillingInformationController<T>> logger, IUnitOfWorkManage unitOfWorkManage, tb_BillingInformationServices tb_BillingInformationServices, EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null) : base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _tb_BillingInformationServices = tb_BillingInformationServices;
            _appContext = appContext;
            _eventDrivenCacheManager = eventDrivenCacheManager;
        }


        public ValidationResult Validator(tb_BillingInformation info)
        {

            // tb_BillingInformationValidator validator = new tb_BillingInformationValidator();
            tb_BillingInformationValidator validator = _appContext.GetRequiredService<tb_BillingInformationValidator>();
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
        public async Task<ReturnResults<tb_BillingInformation>> SaveOrUpdate(tb_BillingInformation entity)
        {
            ReturnResults<tb_BillingInformation> rr = new ReturnResults<tb_BillingInformation>();
            tb_BillingInformation Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BillingInfo_ID > 0)
                {
                    bool rs = await _tb_BillingInformationServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_BillingInformation>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_BillingInformationServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_BillingInformation>(entity);
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
            tb_BillingInformation entity = model as tb_BillingInformation;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BillingInfo_ID > 0)
                {
                    bool rs = await _tb_BillingInformationServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_BillingInformation>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_BillingInformationServices.AddReEntityAsync(entity) as T;
                    _eventDrivenCacheManager.UpdateEntity<tb_BillingInformation>(entity);
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
            List<T> list = await _tb_BillingInformationServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_BillingInformation entity = item as tb_BillingInformation;
                entity.AcceptChanges();
            }
            if (list != null)
            {
                _eventDrivenCacheManager.UpdateEntityList<T>(list);
            }
            return list;
        }

        public async override Task<List<T>> BaseQueryAsync()
        {
            List<T> list = await _tb_BillingInformationServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_BillingInformation entity = item as tb_BillingInformation;
                entity.AcceptChanges();
            }
            if (list != null)
            {
                _eventDrivenCacheManager.UpdateEntityList<T>(list);
            }
            return list;
        }


        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_BillingInformation entity = model as tb_BillingInformation;
            bool rs = await _tb_BillingInformationServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_BillingInformation>(entity.PrimaryKeyID);
            }
            return rs;
        }

        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs = false;
            List<tb_BillingInformation> entitys = models as List<tb_BillingInformation>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_BillingInformation>(entitys).ExecuteCommandAsync();
            if (c > 0)
            {
                rs = true;
                _eventDrivenCacheManager.DeleteEntityList<tb_BillingInformation>(entitys);
            }
            return rs;
        }

        public override ValidationResult BaseValidator(T info)
        {
            //tb_BillingInformationValidator validator = new tb_BillingInformationValidator();
            tb_BillingInformationValidator validator = _appContext.GetRequiredService<tb_BillingInformationValidator>();
            ValidationResult results = validator.Validate(info as tb_BillingInformation);
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

                tb_BillingInformation entity = model as tb_BillingInformation;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                if (entity.BillingInfo_ID > 0)
                {

                    rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_BillingInformation>(entity as tb_BillingInformation)
                        .Include(c => c.tb_customervendor)
           .ExecuteCommandAsync();
                }
                else
                {
                    rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_BillingInformation>(entity as tb_BillingInformation)
     .Include(c => c.tb_customervendor)
            .ExecuteCommandAsync();


                }

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T;
                entity.PrimaryKeyID = entity.BillingInfo_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BillingInformation>()
                                .Includes(m => m.tb_customervendor)
                                        .WhereCustom(useLike, dto); ;
            return await querySqlQueryable.ToListAsync() as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model)
        {
            tb_BillingInformation entity = model as tb_BillingInformation;
            bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_BillingInformation>(m => m.BillingInfo_ID == entity.BillingInfo_ID)
                       .Include(m => m.tb_customervendor)
                                       .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion



        public tb_BillingInformation AddReEntity(tb_BillingInformation entity)
        {
            tb_BillingInformation AddEntity = _tb_BillingInformationServices.AddReEntity(entity);

            _eventDrivenCacheManager.UpdateEntity<tb_BillingInformation>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }

        public async Task<tb_BillingInformation> AddReEntityAsync(tb_BillingInformation entity)
        {
            tb_BillingInformation AddEntity = await _tb_BillingInformationServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_BillingInformation>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }

        public async Task<long> AddAsync(tb_BillingInformation entity)
        {
            long id = await _tb_BillingInformationServices.Add(entity);
            if (id > 0)
            {
                _eventDrivenCacheManager.UpdateEntity<tb_BillingInformation>(entity);
            }
            return id;
        }

        public async Task<List<long>> AddAsync(List<tb_BillingInformation> infos)
        {
            List<long> ids = await _tb_BillingInformationServices.Add(infos);
            if (ids.Count > 0)//成功的个数 这里缓存 对不对呢？
            {
                _eventDrivenCacheManager.UpdateEntityList<tb_BillingInformation>(infos);
            }
            return ids;
        }


        public async Task<bool> DeleteAsync(tb_BillingInformation entity)
        {
            bool rs = await _tb_BillingInformationServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_BillingInformation>(entity);

            }
            return rs;
        }

        public async Task<bool> UpdateAsync(tb_BillingInformation entity)
        {
            bool rs = await _tb_BillingInformationServices.Update(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_BillingInformation>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_BillingInformationServices.DeleteById(id);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_BillingInformation>(id);
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_BillingInformationServices.DeleteByIds(ids);
            if (rs)
            {

                _eventDrivenCacheManager.DeleteEntities<tb_BillingInformation>(ids.Cast<object>().ToArray());
            }
            return rs;
        }

        public virtual async Task<List<tb_BillingInformation>> QueryAsync()
        {
            List<tb_BillingInformation> list = await _tb_BillingInformationServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }

            _eventDrivenCacheManager.UpdateEntityList<tb_BillingInformation>(list);
            return list;
        }

        public virtual List<tb_BillingInformation> Query()
        {
            List<tb_BillingInformation> list = _tb_BillingInformationServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }

            _eventDrivenCacheManager.UpdateEntityList<tb_BillingInformation>(list);
            return list;
        }

        public virtual List<tb_BillingInformation> Query(string wheresql)
        {
            List<tb_BillingInformation> list = _tb_BillingInformationServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }

            _eventDrivenCacheManager.UpdateEntityList<tb_BillingInformation>(list);
            return list;
        }

        public virtual async Task<List<tb_BillingInformation>> QueryAsync(string wheresql)
        {
            List<tb_BillingInformation> list = await _tb_BillingInformationServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }

            _eventDrivenCacheManager.UpdateEntityList<tb_BillingInformation>(list);
            return list;
        }



        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_BillingInformation>> QueryAsync(Expression<Func<tb_BillingInformation, bool>> exp)
        {
            List<tb_BillingInformation> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BillingInformation>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }

            _eventDrivenCacheManager.UpdateEntityList<tb_BillingInformation>(list);
            return list;
        }



        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual async Task<List<tb_BillingInformation>> QueryByNavAsync()
        {
            List<tb_BillingInformation> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BillingInformation>()
                               .Includes(t => t.tb_customervendor)
                        .ToListAsync();

            foreach (var item in list)
            {
                item.AcceptChanges();
            }


            _eventDrivenCacheManager.UpdateEntityList<tb_BillingInformation>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual async Task<List<tb_BillingInformation>> QueryByNavAsync(Expression<Func<tb_BillingInformation, bool>> exp)
        {
            List<tb_BillingInformation> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BillingInformation>().Where(exp)
                               .Includes(t => t.tb_customervendor)
                        .ToListAsync();

            foreach (var item in list)
            {
                item.AcceptChanges();
            }


            _eventDrivenCacheManager.UpdateEntityList<tb_BillingInformation>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual List<tb_BillingInformation> QueryByNav(Expression<Func<tb_BillingInformation, bool>> exp)
        {
            List<tb_BillingInformation> list = _unitOfWorkManage.GetDbClient().Queryable<tb_BillingInformation>().Where(exp)
                            .Includes(t => t.tb_customervendor)
                        .ToList();

            foreach (var item in list)
            {
                item.AcceptChanges();
            }


            _eventDrivenCacheManager.UpdateEntityList<tb_BillingInformation>(list);
            return list;
        }



        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_BillingInformation>> QueryByAdvancedAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BillingInformation>().WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_BillingInformationServices.QueryByIdAsync(id) as T;
            return entity;
        }



        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_BillingInformation entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_BillingInformation>().Where(w => w.BillingInfo_ID == (long)id)
                             .Includes(t => t.tb_customervendor)
                                .FirstAsync();
            if (entity != null)
            {
                entity.AcceptChanges();
            }


            _eventDrivenCacheManager.UpdateEntity<tb_BillingInformation>(entity);
            return entity as T;
        }






    }
}



