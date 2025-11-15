// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:16
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
    /// 流程定义 http://www.phpheidong.com/blog/article/68471/a3129f742e5e396e3d1e/
    /// </summary>
    public partial class tb_ProcessDefinitionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProcessDefinitionServices _tb_ProcessDefinitionServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_ProcessDefinitionController(ILogger<tb_ProcessDefinitionController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProcessDefinitionServices tb_ProcessDefinitionServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProcessDefinitionServices = tb_ProcessDefinitionServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_ProcessDefinition info)
        {

           // tb_ProcessDefinitionValidator validator = new tb_ProcessDefinitionValidator();
           tb_ProcessDefinitionValidator validator = _appContext.GetRequiredService<tb_ProcessDefinitionValidator>();
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
        public async Task<ReturnResults<tb_ProcessDefinition>> SaveOrUpdate(tb_ProcessDefinition entity)
        {
            ReturnResults<tb_ProcessDefinition> rr = new ReturnResults<tb_ProcessDefinition>();
            tb_ProcessDefinition Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProcessDefinition_Id > 0)
                {
                    bool rs = await _tb_ProcessDefinitionServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ProcessDefinition>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProcessDefinitionServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_ProcessDefinition>(entity);
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
            tb_ProcessDefinition entity = model as tb_ProcessDefinition;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProcessDefinition_Id > 0)
                {
                    bool rs = await _tb_ProcessDefinitionServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ProcessDefinition>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProcessDefinitionServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_ProcessDefinition>(entity);
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
            List<T> list = await _tb_ProcessDefinitionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProcessDefinition entity = item as tb_ProcessDefinition;
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
            List<T> list = await _tb_ProcessDefinitionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProcessDefinition entity = item as tb_ProcessDefinition;
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
            tb_ProcessDefinition entity = model as tb_ProcessDefinition;
            bool rs = await _tb_ProcessDefinitionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_ProcessDefinition>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProcessDefinition> entitys = models as List<tb_ProcessDefinition>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProcessDefinition>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_ProcessDefinition>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProcessDefinitionValidator validator = new tb_ProcessDefinitionValidator();
           tb_ProcessDefinitionValidator validator = _appContext.GetRequiredService<tb_ProcessDefinitionValidator>();
            ValidationResult results = validator.Validate(info as tb_ProcessDefinition);
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

                tb_ProcessDefinition entity = model as tb_ProcessDefinition;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ProcessDefinition_Id > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_ProcessDefinition>(entity as tb_ProcessDefinition)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_ProcessDefinition>(entity as tb_ProcessDefinition)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ProcessDefinition_Id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessDefinition>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProcessDefinition entity = model as tb_ProcessDefinition;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProcessDefinition>(m => m.ProcessDefinition_Id== entity.ProcessDefinition_Id)
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProcessDefinition AddReEntity(tb_ProcessDefinition entity)
        {
            tb_ProcessDefinition AddEntity =  _tb_ProcessDefinitionServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_ProcessDefinition>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProcessDefinition> AddReEntityAsync(tb_ProcessDefinition entity)
        {
            tb_ProcessDefinition AddEntity = await _tb_ProcessDefinitionServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_ProcessDefinition>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProcessDefinition entity)
        {
            long id = await _tb_ProcessDefinitionServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_ProcessDefinition>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProcessDefinition> infos)
        {
            List<long> ids = await _tb_ProcessDefinitionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_ProcessDefinition>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProcessDefinition entity)
        {
            bool rs = await _tb_ProcessDefinitionServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_ProcessDefinition>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProcessDefinition entity)
        {
            bool rs = await _tb_ProcessDefinitionServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_ProcessDefinition>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProcessDefinitionServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_ProcessDefinition>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProcessDefinitionServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_ProcessDefinition>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProcessDefinition>> QueryAsync()
        {
            List<tb_ProcessDefinition> list = await  _tb_ProcessDefinitionServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessDefinition>(list);
            return list;
        }
        
        public virtual List<tb_ProcessDefinition> Query()
        {
            List<tb_ProcessDefinition> list =  _tb_ProcessDefinitionServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessDefinition>(list);
            return list;
        }
        
        public virtual List<tb_ProcessDefinition> Query(string wheresql)
        {
            List<tb_ProcessDefinition> list =  _tb_ProcessDefinitionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessDefinition>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProcessDefinition>> QueryAsync(string wheresql) 
        {
            List<tb_ProcessDefinition> list = await _tb_ProcessDefinitionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessDefinition>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProcessDefinition>> QueryAsync(Expression<Func<tb_ProcessDefinition, bool>> exp)
        {
            List<tb_ProcessDefinition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessDefinition>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessDefinition>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProcessDefinition>> QueryByNavAsync()
        {
            List<tb_ProcessDefinition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessDefinition>()
                               .Includes(t => t.tb_processstep )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessDefinition>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProcessDefinition>> QueryByNavAsync(Expression<Func<tb_ProcessDefinition, bool>> exp)
        {
            List<tb_ProcessDefinition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessDefinition>().Where(exp)
                               .Includes(t => t.tb_processstep )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessDefinition>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProcessDefinition> QueryByNav(Expression<Func<tb_ProcessDefinition, bool>> exp)
        {
            List<tb_ProcessDefinition> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessDefinition>().Where(exp)
                            .Includes(t => t.tb_processstep )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ProcessDefinition>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProcessDefinition>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessDefinition>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProcessDefinitionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProcessDefinition entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessDefinition>().Where(w => w.ProcessDefinition_Id == (long)id)
                             .Includes(t => t.tb_processstep )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_ProcessDefinition>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



