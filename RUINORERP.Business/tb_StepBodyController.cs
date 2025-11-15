// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:23
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
    /// 步骤定义
    /// </summary>
    public partial class tb_StepBodyController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_StepBodyServices _tb_StepBodyServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_StepBodyController(ILogger<tb_StepBodyController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_StepBodyServices tb_StepBodyServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_StepBodyServices = tb_StepBodyServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_StepBody info)
        {

           // tb_StepBodyValidator validator = new tb_StepBodyValidator();
           tb_StepBodyValidator validator = _appContext.GetRequiredService<tb_StepBodyValidator>();
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
        public async Task<ReturnResults<tb_StepBody>> SaveOrUpdate(tb_StepBody entity)
        {
            ReturnResults<tb_StepBody> rr = new ReturnResults<tb_StepBody>();
            tb_StepBody Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.StepBodyld > 0)
                {
                    bool rs = await _tb_StepBodyServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_StepBody>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_StepBodyServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_StepBody>(entity);
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
            tb_StepBody entity = model as tb_StepBody;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.StepBodyld > 0)
                {
                    bool rs = await _tb_StepBodyServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_StepBody>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_StepBodyServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_StepBody>(entity);
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
            List<T> list = await _tb_StepBodyServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_StepBody entity = item as tb_StepBody;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                _eventDrivenCacheManager.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        public async override Task<List<T>> BaseQueryAsync() 
        {
            List<T> list = await _tb_StepBodyServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_StepBody entity = item as tb_StepBody;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                _eventDrivenCacheManager.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        
        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_StepBody entity = model as tb_StepBody;
            bool rs = await _tb_StepBodyServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_StepBody>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_StepBody> entitys = models as List<tb_StepBody>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_StepBody>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_StepBody>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_StepBodyValidator validator = new tb_StepBodyValidator();
           tb_StepBodyValidator validator = _appContext.GetRequiredService<tb_StepBodyValidator>();
            ValidationResult results = validator.Validate(info as tb_StepBody);
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

                tb_StepBody entity = model as tb_StepBody;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.StepBodyld > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_StepBody>(entity as tb_StepBody)
                        .Include(m => m.tb_ProcessSteps)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_StepBody>(entity as tb_StepBody)
                .Include(m => m.tb_ProcessSteps)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.StepBodyld;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StepBody>()
                                .Includes(m => m.tb_ProcessSteps)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_StepBody entity = model as tb_StepBody;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_StepBody>(m => m.StepBodyld== entity.StepBodyld)
                                .Include(m => m.tb_ProcessSteps)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_StepBody AddReEntity(tb_StepBody entity)
        {
            tb_StepBody AddEntity =  _tb_StepBodyServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_StepBody>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_StepBody> AddReEntityAsync(tb_StepBody entity)
        {
            tb_StepBody AddEntity = await _tb_StepBodyServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_StepBody>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_StepBody entity)
        {
            long id = await _tb_StepBodyServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_StepBody>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_StepBody> infos)
        {
            List<long> ids = await _tb_StepBodyServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_StepBody>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_StepBody entity)
        {
            bool rs = await _tb_StepBodyServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_StepBody>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_StepBody entity)
        {
            bool rs = await _tb_StepBodyServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_StepBody>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_StepBodyServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_StepBody>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_StepBodyServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_StepBody>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_StepBody>> QueryAsync()
        {
            List<tb_StepBody> list = await  _tb_StepBodyServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBody>(list);
            return list;
        }
        
        public virtual List<tb_StepBody> Query()
        {
            List<tb_StepBody> list =  _tb_StepBodyServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBody>(list);
            return list;
        }
        
        public virtual List<tb_StepBody> Query(string wheresql)
        {
            List<tb_StepBody> list =  _tb_StepBodyServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBody>(list);
            return list;
        }
        
        public virtual async Task<List<tb_StepBody>> QueryAsync(string wheresql) 
        {
            List<tb_StepBody> list = await _tb_StepBodyServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBody>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_StepBody>> QueryAsync(Expression<Func<tb_StepBody, bool>> exp)
        {
            List<tb_StepBody> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StepBody>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBody>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StepBody>> QueryByNavAsync()
        {
            List<tb_StepBody> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StepBody>()
                               .Includes(t => t.tb_stepbodypara )
                                            .Includes(t => t.tb_ProcessSteps )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBody>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StepBody>> QueryByNavAsync(Expression<Func<tb_StepBody, bool>> exp)
        {
            List<tb_StepBody> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StepBody>().Where(exp)
                               .Includes(t => t.tb_stepbodypara )
                                            .Includes(t => t.tb_ProcessSteps )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBody>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_StepBody> QueryByNav(Expression<Func<tb_StepBody, bool>> exp)
        {
            List<tb_StepBody> list = _unitOfWorkManage.GetDbClient().Queryable<tb_StepBody>().Where(exp)
                            .Includes(t => t.tb_stepbodypara )
                                        .Includes(t => t.tb_ProcessSteps )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBody>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_StepBody>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StepBody>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_StepBodyServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_StepBody entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_StepBody>().Where(w => w.StepBodyld == (long)id)
                             .Includes(t => t.tb_stepbodypara )
                        

                                            .Includes(t => t.tb_ProcessSteps )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_StepBody>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



