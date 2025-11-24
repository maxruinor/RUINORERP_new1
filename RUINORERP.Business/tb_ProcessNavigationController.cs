// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/24/2025 17:01:19
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
    /// 
    /// </summary>
    public partial class tb_ProcessNavigationController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProcessNavigationServices _tb_ProcessNavigationServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_ProcessNavigationController(ILogger<tb_ProcessNavigationController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProcessNavigationServices tb_ProcessNavigationServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProcessNavigationServices = tb_ProcessNavigationServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_ProcessNavigation info)
        {

           // tb_ProcessNavigationValidator validator = new tb_ProcessNavigationValidator();
           tb_ProcessNavigationValidator validator = _appContext.GetRequiredService<tb_ProcessNavigationValidator>();
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
        public async Task<ReturnResults<tb_ProcessNavigation>> SaveOrUpdate(tb_ProcessNavigation entity)
        {
            ReturnResults<tb_ProcessNavigation> rr = new ReturnResults<tb_ProcessNavigation>();
            tb_ProcessNavigation Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProcessNavID > 0)
                {
                    bool rs = await _tb_ProcessNavigationServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ProcessNavigation>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProcessNavigationServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_ProcessNavigation>(entity);
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
            tb_ProcessNavigation entity = model as tb_ProcessNavigation;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProcessNavID > 0)
                {
                    bool rs = await _tb_ProcessNavigationServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ProcessNavigation>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProcessNavigationServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_ProcessNavigation>(entity);
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
            List<T> list = await _tb_ProcessNavigationServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProcessNavigation entity = item as tb_ProcessNavigation;
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
            List<T> list = await _tb_ProcessNavigationServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProcessNavigation entity = item as tb_ProcessNavigation;
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
            tb_ProcessNavigation entity = model as tb_ProcessNavigation;
            bool rs = await _tb_ProcessNavigationServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_ProcessNavigation>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProcessNavigation> entitys = models as List<tb_ProcessNavigation>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProcessNavigation>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_ProcessNavigation>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProcessNavigationValidator validator = new tb_ProcessNavigationValidator();
           tb_ProcessNavigationValidator validator = _appContext.GetRequiredService<tb_ProcessNavigationValidator>();
            ValidationResult results = validator.Validate(info as tb_ProcessNavigation);
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

                tb_ProcessNavigation entity = model as tb_ProcessNavigation;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ProcessNavID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProcessNavigation>(entity as tb_ProcessNavigation)
                        .Include(m => m.tb_ProcessNavigations)
                    .Include(m => m.tb_ProcessNavigationNodes)
                    .Include(m => m.tb_ProcessNavigationNodes)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProcessNavigation>(entity as tb_ProcessNavigation)
                .Include(m => m.tb_ProcessNavigations)
                .Include(m => m.tb_ProcessNavigationNodes)
                .Include(m => m.tb_ProcessNavigationNodes)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ProcessNavID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                                .Includes(m => m.tb_moduledefinition)
                            .Includes(m => m.tb_processnavigation)
                                            .Includes(m => m.tb_ProcessNavigations)
                        .Includes(m => m.tb_ProcessNavigationNodes)
                        .Includes(m => m.tb_ProcessNavigationNodesByChildNavigation)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProcessNavigation entity = model as tb_ProcessNavigation;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProcessNavigation>(m => m.ProcessNavID== entity.ProcessNavID)
                                .Include(m => m.tb_ProcessNavigations)
                        .Include(m => m.tb_ProcessNavigationNodes)
                        .Include(m => m.tb_ProcessNavigationNodesByChildNavigation)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProcessNavigation AddReEntity(tb_ProcessNavigation entity)
        {
            tb_ProcessNavigation AddEntity =  _tb_ProcessNavigationServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_ProcessNavigation>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProcessNavigation> AddReEntityAsync(tb_ProcessNavigation entity)
        {
            tb_ProcessNavigation AddEntity = await _tb_ProcessNavigationServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_ProcessNavigation>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProcessNavigation entity)
        {
            long id = await _tb_ProcessNavigationServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_ProcessNavigation>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProcessNavigation> infos)
        {
            List<long> ids = await _tb_ProcessNavigationServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_ProcessNavigation>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProcessNavigation entity)
        {
            bool rs = await _tb_ProcessNavigationServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_ProcessNavigation>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProcessNavigation entity)
        {
            bool rs = await _tb_ProcessNavigationServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_ProcessNavigation>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProcessNavigationServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_ProcessNavigation>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProcessNavigationServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_ProcessNavigation>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProcessNavigation>> QueryAsync()
        {
            List<tb_ProcessNavigation> list = await  _tb_ProcessNavigationServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessNavigation>(list);
            return list;
        }
        
        public virtual List<tb_ProcessNavigation> Query()
        {
            List<tb_ProcessNavigation> list =  _tb_ProcessNavigationServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessNavigation>(list);
            return list;
        }
        
        public virtual List<tb_ProcessNavigation> Query(string wheresql)
        {
            List<tb_ProcessNavigation> list =  _tb_ProcessNavigationServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessNavigation>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProcessNavigation>> QueryAsync(string wheresql) 
        {
            List<tb_ProcessNavigation> list = await _tb_ProcessNavigationServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessNavigation>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProcessNavigation>> QueryAsync(Expression<Func<tb_ProcessNavigation, bool>> exp)
        {
            List<tb_ProcessNavigation> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessNavigation>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProcessNavigation>> QueryByNavAsync()
        {
            List<tb_ProcessNavigation> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                               .Includes(t => t.tb_moduledefinition )
                               .Includes(t => t.tb_processnavigation )
                                            .Includes(t => t.tb_ProcessNavigations )
                                .Includes(t => t.tb_ProcessNavigationNodes )
                                .Includes(t => t.tb_ProcessNavigationNodes )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessNavigation>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProcessNavigation>> QueryByNavAsync(Expression<Func<tb_ProcessNavigation, bool>> exp)
        {
            List<tb_ProcessNavigation> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>().Where(exp)
                               .Includes(t => t.tb_moduledefinition )
                               .Includes(t => t.tb_processnavigation )
                                            .Includes(t => t.tb_ProcessNavigations )
                                .Includes(t => t.tb_ProcessNavigationNodes )
                                .Includes(t => t.tb_ProcessNavigationNodesByChildNavigation )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessNavigation>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProcessNavigation> QueryByNav(Expression<Func<tb_ProcessNavigation, bool>> exp)
        {
            List<tb_ProcessNavigation> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>().Where(exp)
                            .Includes(t => t.tb_moduledefinition )
                            .Includes(t => t.tb_processnavigation )
                                        .Includes(t => t.tb_ProcessNavigations )
                            .Includes(t => t.tb_ProcessNavigationNodes )
                            .Includes(t => t.tb_ProcessNavigationNodesByChildNavigation )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessNavigation>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProcessNavigation>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProcessNavigationServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProcessNavigation entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>().Where(w => w.ProcessNavID == (long)id)
                             .Includes(t => t.tb_moduledefinition )
                            .Includes(t => t.tb_processnavigation )
                        

                                            .Includes(t => t.tb_ProcessNavigations )
                                            .Includes(t => t.tb_ProcessNavigationNodes )
                                            .Includes(t => t.tb_ProcessNavigationNodesByChildNavigation )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_ProcessNavigation>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



