
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:23
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

namespace RUINORERP.Business
{
    /// <summary>
    /// 提醒内容
    /// </summary>
    public partial class tb_ReminderAlertController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ReminderAlertServices _tb_ReminderAlertServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ReminderAlertController(ILogger<tb_ReminderAlertController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ReminderAlertServices tb_ReminderAlertServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ReminderAlertServices = tb_ReminderAlertServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ReminderAlert info)
        {

           // tb_ReminderAlertValidator validator = new tb_ReminderAlertValidator();
           tb_ReminderAlertValidator validator = _appContext.GetRequiredService<tb_ReminderAlertValidator>();
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
        public async Task<ReturnResults<tb_ReminderAlert>> SaveOrUpdate(tb_ReminderAlert entity)
        {
            ReturnResults<tb_ReminderAlert> rr = new ReturnResults<tb_ReminderAlert>();
            tb_ReminderAlert Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.AlertId > 0)
                {
                    bool rs = await _tb_ReminderAlertServices.Update(entity);
                    if (rs)
                    {
                        Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlert>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ReminderAlertServices.AddReEntityAsync(entity);
                    Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlert>(entity);
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
        public async override Task<ReturnResults<T>>  BaseSaveOrUpdate(T model)
        {
            ReturnResults<T> rr = new ReturnResults<T>();
            tb_ReminderAlert entity = model as tb_ReminderAlert;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.AlertId > 0)
                {
                    bool rs = await _tb_ReminderAlertServices.Update(entity);
                    if (rs)
                    {
                        Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlert>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ReminderAlertServices.AddReEntityAsync(entity) as T ;
                    Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlert>(entity);
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
            List<T> list = await _tb_ReminderAlertServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ReminderAlert entity = item as tb_ReminderAlert;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                Cache.EntityCacheHelper.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        public async override Task<List<T>> BaseQueryAsync() 
        {
            List<T> list = await _tb_ReminderAlertServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ReminderAlert entity = item as tb_ReminderAlert;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                Cache.EntityCacheHelper.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        
        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_ReminderAlert entity = model as tb_ReminderAlert;
            bool rs = await _tb_ReminderAlertServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlert>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ReminderAlert> entitys = models as List<tb_ReminderAlert>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ReminderAlert>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.AlertId).ToArray();
                Cache.EntityCacheHelper.DeleteEntityList<tb_ReminderAlert>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ReminderAlertValidator validator = new tb_ReminderAlertValidator();
           tb_ReminderAlertValidator validator = _appContext.GetRequiredService<tb_ReminderAlertValidator>();
            ValidationResult results = validator.Validate(info as tb_ReminderAlert);
            return results;
        }
        
        
        public async override Task<List<T>> BaseQueryByAdvancedAsync(bool useLike,object dto) 
        {
            var  querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().WhereCustom(useLike,dto);
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

                tb_ReminderAlert entity = model as tb_ReminderAlert;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.AlertId > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ReminderAlert>(entity as tb_ReminderAlert)
                        .Include(m => m.tb_ReminderAlertHistories)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ReminderAlert>(entity as tb_ReminderAlert)
                .Include(m => m.tb_ReminderAlertHistories)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.AlertId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlert>()
                                .Includes(m => m.tb_ReminderAlertHistories)
                                        .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ReminderAlert entity = model as tb_ReminderAlert;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ReminderAlert>(m => m.AlertId== entity.AlertId)
                                .Include(m => m.tb_ReminderAlertHistories)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                Cache.EntityCacheHelper.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ReminderAlert AddReEntity(tb_ReminderAlert entity)
        {
            tb_ReminderAlert AddEntity =  _tb_ReminderAlertServices.AddReEntity(entity);
            Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlert>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ReminderAlert> AddReEntityAsync(tb_ReminderAlert entity)
        {
            tb_ReminderAlert AddEntity = await _tb_ReminderAlertServices.AddReEntityAsync(entity);
            Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlert>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ReminderAlert entity)
        {
            long id = await _tb_ReminderAlertServices.Add(entity);
            if(id>0)
            {
                 Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlert>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ReminderAlert> infos)
        {
            List<long> ids = await _tb_ReminderAlertServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlert>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ReminderAlert entity)
        {
            bool rs = await _tb_ReminderAlertServices.Delete(entity);
            if (rs)
            {
                Cache.EntityCacheHelper.DeleteEntity<tb_ReminderAlert>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ReminderAlert entity)
        {
            bool rs = await _tb_ReminderAlertServices.Update(entity);
            if (rs)
            {
                 Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlert>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ReminderAlertServices.DeleteById(id);
            if (rs)
            {
                Cache.EntityCacheHelper.DeleteEntity<tb_ReminderAlert>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ReminderAlertServices.DeleteByIds(ids);
            if (rs)
            {
                Cache.EntityCacheHelper.DeleteEntity<tb_ReminderAlert>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ReminderAlert>> QueryAsync()
        {
            List<tb_ReminderAlert> list = await  _tb_ReminderAlertServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlert>(list);
            return list;
        }
        
        public virtual List<tb_ReminderAlert> Query()
        {
            List<tb_ReminderAlert> list =  _tb_ReminderAlertServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
           Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlert>(list);;
            return list;
        }
        
        public virtual List<tb_ReminderAlert> Query(string wheresql)
        {
            List<tb_ReminderAlert> list =  _tb_ReminderAlertServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
           Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlert>(list);;
            return list;
        }
        
        public virtual async Task<List<tb_ReminderAlert>> QueryAsync(string wheresql) 
        {
            List<tb_ReminderAlert> list = await _tb_ReminderAlertServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
           Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlert>(list);;
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ReminderAlert>> QueryAsync(Expression<Func<tb_ReminderAlert, bool>> exp)
        {
            List<tb_ReminderAlert> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlert>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
           Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlert>(list);;
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderAlert>> QueryByNavAsync()
        {
            List<tb_ReminderAlert> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlert>()
                               .Includes(t => t.tb_reminderrule )
                                            .Includes(t => t.tb_ReminderAlertHistories )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
           Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlert>(list);;
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderAlert>> QueryByNavAsync(Expression<Func<tb_ReminderAlert, bool>> exp)
        {
            List<tb_ReminderAlert> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlert>().Where(exp)
                               .Includes(t => t.tb_reminderrule )
                                            .Includes(t => t.tb_ReminderAlertHistories )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
           Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlert>(list);;
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ReminderAlert> QueryByNav(Expression<Func<tb_ReminderAlert, bool>> exp)
        {
            List<tb_ReminderAlert> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlert>().Where(exp)
                            .Includes(t => t.tb_reminderrule )
                                        .Includes(t => t.tb_ReminderAlertHistories )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
           Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlert>(list);;
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ReminderAlert>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlert>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ReminderAlertServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ReminderAlert entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlert>().Where(w => w.AlertId == (long)id)
                             .Includes(t => t.tb_reminderrule )
                                        .Includes(t => t.tb_ReminderAlertHistories )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlert>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



