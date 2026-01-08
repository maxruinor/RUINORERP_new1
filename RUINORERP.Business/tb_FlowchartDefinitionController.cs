// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:03
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
    /// 流程图定义
    /// </summary>
    public partial class tb_FlowchartDefinitionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FlowchartDefinitionServices _tb_FlowchartDefinitionServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_FlowchartDefinitionController(ILogger<tb_FlowchartDefinitionController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FlowchartDefinitionServices tb_FlowchartDefinitionServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FlowchartDefinitionServices = tb_FlowchartDefinitionServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_FlowchartDefinition info)
        {

           // tb_FlowchartDefinitionValidator validator = new tb_FlowchartDefinitionValidator();
           tb_FlowchartDefinitionValidator validator = _appContext.GetRequiredService<tb_FlowchartDefinitionValidator>();
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
        public async Task<ReturnResults<tb_FlowchartDefinition>> SaveOrUpdate(tb_FlowchartDefinition entity)
        {
            ReturnResults<tb_FlowchartDefinition> rr = new ReturnResults<tb_FlowchartDefinition>();
            tb_FlowchartDefinition Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ID > 0)
                {
                    bool rs = await _tb_FlowchartDefinitionServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_FlowchartDefinition>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FlowchartDefinitionServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_FlowchartDefinition>(entity);
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
            tb_FlowchartDefinition entity = model as tb_FlowchartDefinition;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ID > 0)
                {
                    bool rs = await _tb_FlowchartDefinitionServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_FlowchartDefinition>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FlowchartDefinitionServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_FlowchartDefinition>(entity);
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
            List<T> list = await _tb_FlowchartDefinitionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FlowchartDefinition entity = item as tb_FlowchartDefinition;
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
            List<T> list = await _tb_FlowchartDefinitionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FlowchartDefinition entity = item as tb_FlowchartDefinition;
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
            tb_FlowchartDefinition entity = model as tb_FlowchartDefinition;
            bool rs = await _tb_FlowchartDefinitionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_FlowchartDefinition>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FlowchartDefinition> entitys = models as List<tb_FlowchartDefinition>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FlowchartDefinition>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_FlowchartDefinition>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FlowchartDefinitionValidator validator = new tb_FlowchartDefinitionValidator();
           tb_FlowchartDefinitionValidator validator = _appContext.GetRequiredService<tb_FlowchartDefinitionValidator>();
            ValidationResult results = validator.Validate(info as tb_FlowchartDefinition);
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

                tb_FlowchartDefinition entity = model as tb_FlowchartDefinition;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FlowchartDefinition>(entity as tb_FlowchartDefinition)
                        .Include(m => m.tb_FlowchartItems)
                    .Include(m => m.tb_FlowchartLines)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FlowchartDefinition>(entity as tb_FlowchartDefinition)
                .Include(m => m.tb_FlowchartItems)
                .Include(m => m.tb_FlowchartLines)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FlowchartDefinition>()
                                .Includes(m => m.tb_FlowchartItems)
                        .Includes(m => m.tb_FlowchartLines)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FlowchartDefinition entity = model as tb_FlowchartDefinition;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FlowchartDefinition>(m => m.ID== entity.ID)
                                .Include(m => m.tb_FlowchartItems)
                        .Include(m => m.tb_FlowchartLines)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FlowchartDefinition AddReEntity(tb_FlowchartDefinition entity)
        {
            tb_FlowchartDefinition AddEntity =  _tb_FlowchartDefinitionServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_FlowchartDefinition>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FlowchartDefinition> AddReEntityAsync(tb_FlowchartDefinition entity)
        {
            tb_FlowchartDefinition AddEntity = await _tb_FlowchartDefinitionServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_FlowchartDefinition>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FlowchartDefinition entity)
        {
            long id = await _tb_FlowchartDefinitionServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_FlowchartDefinition>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FlowchartDefinition> infos)
        {
            List<long> ids = await _tb_FlowchartDefinitionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_FlowchartDefinition>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FlowchartDefinition entity)
        {
            bool rs = await _tb_FlowchartDefinitionServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_FlowchartDefinition>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FlowchartDefinition entity)
        {
            bool rs = await _tb_FlowchartDefinitionServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_FlowchartDefinition>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FlowchartDefinitionServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_FlowchartDefinition>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FlowchartDefinitionServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_FlowchartDefinition>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FlowchartDefinition>> QueryAsync()
        {
            List<tb_FlowchartDefinition> list = await  _tb_FlowchartDefinitionServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_FlowchartDefinition>(list);
            return list;
        }
        
        public virtual List<tb_FlowchartDefinition> Query()
        {
            List<tb_FlowchartDefinition> list =  _tb_FlowchartDefinitionServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_FlowchartDefinition>(list);
            return list;
        }
        
        public virtual List<tb_FlowchartDefinition> Query(string wheresql)
        {
            List<tb_FlowchartDefinition> list =  _tb_FlowchartDefinitionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_FlowchartDefinition>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FlowchartDefinition>> QueryAsync(string wheresql) 
        {
            List<tb_FlowchartDefinition> list = await _tb_FlowchartDefinitionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_FlowchartDefinition>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FlowchartDefinition>> QueryAsync(Expression<Func<tb_FlowchartDefinition, bool>> exp)
        {
            List<tb_FlowchartDefinition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FlowchartDefinition>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_FlowchartDefinition>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FlowchartDefinition>> QueryByNavAsync()
        {
            List<tb_FlowchartDefinition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FlowchartDefinition>()
                               .Includes(t => t.tb_moduledefinition )
                                            .Includes(t => t.tb_FlowchartItems )
                                .Includes(t => t.tb_FlowchartLines )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_FlowchartDefinition>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FlowchartDefinition>> QueryByNavAsync(Expression<Func<tb_FlowchartDefinition, bool>> exp)
        {
            List<tb_FlowchartDefinition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FlowchartDefinition>().Where(exp)
                               .Includes(t => t.tb_moduledefinition )
                                            .Includes(t => t.tb_FlowchartItems )
                                .Includes(t => t.tb_FlowchartLines )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_FlowchartDefinition>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FlowchartDefinition> QueryByNav(Expression<Func<tb_FlowchartDefinition, bool>> exp)
        {
            List<tb_FlowchartDefinition> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FlowchartDefinition>().Where(exp)
                            .Includes(t => t.tb_moduledefinition )
                                        .Includes(t => t.tb_FlowchartItems )
                            .Includes(t => t.tb_FlowchartLines )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_FlowchartDefinition>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FlowchartDefinition>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FlowchartDefinition>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FlowchartDefinitionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FlowchartDefinition entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FlowchartDefinition>().Where(w => w.ID == (long)id)
                             .Includes(t => t.tb_moduledefinition )
                        

                                            .Includes(t => t.tb_FlowchartItems )
                                            .Includes(t => t.tb_FlowchartLines )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_FlowchartDefinition>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



