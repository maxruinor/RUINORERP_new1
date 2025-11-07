// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:01
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
using RUINORERP.Business.Cache;

namespace RUINORERP.Business
{
    /// <summary>
    /// 客户关系配置表
    /// </summary>
    public partial class tb_CRMConfigController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CRMConfigServices _tb_CRMConfigServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_CRMConfigController(ILogger<tb_CRMConfigController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CRMConfigServices tb_CRMConfigServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CRMConfigServices = tb_CRMConfigServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_CRMConfig info)
        {

           // tb_CRMConfigValidator validator = new tb_CRMConfigValidator();
           tb_CRMConfigValidator validator = _appContext.GetRequiredService<tb_CRMConfigValidator>();
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
        public async Task<ReturnResults<tb_CRMConfig>> SaveOrUpdate(tb_CRMConfig entity)
        {
            ReturnResults<tb_CRMConfig> rr = new ReturnResults<tb_CRMConfig>();
            tb_CRMConfig Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.CRMConfigID > 0)
                {
                    bool rs = await _tb_CRMConfigServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CRMConfig>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CRMConfigServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_CRMConfig>(entity);
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
            tb_CRMConfig entity = model as tb_CRMConfig;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.CRMConfigID > 0)
                {
                    bool rs = await _tb_CRMConfigServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CRMConfig>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CRMConfigServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_CRMConfig>(entity);
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
            List<T> list = await _tb_CRMConfigServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_CRMConfig entity = item as tb_CRMConfig;
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
            List<T> list = await _tb_CRMConfigServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_CRMConfig entity = item as tb_CRMConfig;
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
            tb_CRMConfig entity = model as tb_CRMConfig;
            bool rs = await _tb_CRMConfigServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_CRMConfig>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_CRMConfig> entitys = models as List<tb_CRMConfig>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_CRMConfig>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_CRMConfig>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_CRMConfigValidator validator = new tb_CRMConfigValidator();
           tb_CRMConfigValidator validator = _appContext.GetRequiredService<tb_CRMConfigValidator>();
            ValidationResult results = validator.Validate(info as tb_CRMConfig);
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

                tb_CRMConfig entity = model as tb_CRMConfig;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.CRMConfigID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_CRMConfig>(entity as tb_CRMConfig)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_CRMConfig>(entity as tb_CRMConfig)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.CRMConfigID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CRMConfig>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CRMConfig entity = model as tb_CRMConfig;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CRMConfig>(m => m.CRMConfigID== entity.CRMConfigID)
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
        
        
        
        public tb_CRMConfig AddReEntity(tb_CRMConfig entity)
        {
            tb_CRMConfig AddEntity =  _tb_CRMConfigServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_CRMConfig>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CRMConfig> AddReEntityAsync(tb_CRMConfig entity)
        {
            tb_CRMConfig AddEntity = await _tb_CRMConfigServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_CRMConfig>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CRMConfig entity)
        {
            long id = await _tb_CRMConfigServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_CRMConfig>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CRMConfig> infos)
        {
            List<long> ids = await _tb_CRMConfigServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_CRMConfig>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CRMConfig entity)
        {
            bool rs = await _tb_CRMConfigServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_CRMConfig>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CRMConfig entity)
        {
            bool rs = await _tb_CRMConfigServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_CRMConfig>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CRMConfigServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_CRMConfig>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CRMConfigServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_CRMConfig>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_CRMConfig>> QueryAsync()
        {
            List<tb_CRMConfig> list = await  _tb_CRMConfigServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CRMConfig>(list);
            return list;
        }
        
        public virtual List<tb_CRMConfig> Query()
        {
            List<tb_CRMConfig> list =  _tb_CRMConfigServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_CRMConfig>(list);
            return list;
        }
        
        public virtual List<tb_CRMConfig> Query(string wheresql)
        {
            List<tb_CRMConfig> list =  _tb_CRMConfigServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CRMConfig>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CRMConfig>> QueryAsync(string wheresql) 
        {
            List<tb_CRMConfig> list = await _tb_CRMConfigServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CRMConfig>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_CRMConfig>> QueryAsync(Expression<Func<tb_CRMConfig, bool>> exp)
        {
            List<tb_CRMConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRMConfig>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_CRMConfig>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CRMConfig>> QueryByNavAsync()
        {
            List<tb_CRMConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRMConfig>()
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CRMConfig>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CRMConfig>> QueryByNavAsync(Expression<Func<tb_CRMConfig, bool>> exp)
        {
            List<tb_CRMConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRMConfig>().Where(exp)
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CRMConfig>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CRMConfig> QueryByNav(Expression<Func<tb_CRMConfig, bool>> exp)
        {
            List<tb_CRMConfig> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CRMConfig>().Where(exp)
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CRMConfig>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CRMConfig>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CRMConfig>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CRMConfigServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_CRMConfig entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRMConfig>().Where(w => w.CRMConfigID == (long)id)
                         

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_CRMConfig>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



