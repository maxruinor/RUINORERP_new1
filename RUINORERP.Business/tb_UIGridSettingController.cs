// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:25
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
    /// UI表格设置
    /// </summary>
    public partial class tb_UIGridSettingController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_UIGridSettingServices _tb_UIGridSettingServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_UIGridSettingController(ILogger<tb_UIGridSettingController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_UIGridSettingServices tb_UIGridSettingServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_UIGridSettingServices = tb_UIGridSettingServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_UIGridSetting info)
        {

           // tb_UIGridSettingValidator validator = new tb_UIGridSettingValidator();
           tb_UIGridSettingValidator validator = _appContext.GetRequiredService<tb_UIGridSettingValidator>();
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
        public async Task<ReturnResults<tb_UIGridSetting>> SaveOrUpdate(tb_UIGridSetting entity)
        {
            ReturnResults<tb_UIGridSetting> rr = new ReturnResults<tb_UIGridSetting>();
            tb_UIGridSetting Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UIGID > 0)
                {
                    bool rs = await _tb_UIGridSettingServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_UIGridSetting>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_UIGridSettingServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_UIGridSetting>(entity);
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
            tb_UIGridSetting entity = model as tb_UIGridSetting;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UIGID > 0)
                {
                    bool rs = await _tb_UIGridSettingServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_UIGridSetting>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_UIGridSettingServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_UIGridSetting>(entity);
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
            List<T> list = await _tb_UIGridSettingServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_UIGridSetting entity = item as tb_UIGridSetting;
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
            List<T> list = await _tb_UIGridSettingServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_UIGridSetting entity = item as tb_UIGridSetting;
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
            tb_UIGridSetting entity = model as tb_UIGridSetting;
            bool rs = await _tb_UIGridSettingServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_UIGridSetting>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_UIGridSetting> entitys = models as List<tb_UIGridSetting>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_UIGridSetting>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_UIGridSetting>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_UIGridSettingValidator validator = new tb_UIGridSettingValidator();
           tb_UIGridSettingValidator validator = _appContext.GetRequiredService<tb_UIGridSettingValidator>();
            ValidationResult results = validator.Validate(info as tb_UIGridSetting);
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

                tb_UIGridSetting entity = model as tb_UIGridSetting;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.UIGID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_UIGridSetting>(entity as tb_UIGridSetting)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_UIGridSetting>(entity as tb_UIGridSetting)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.UIGID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridSetting>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_UIGridSetting entity = model as tb_UIGridSetting;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_UIGridSetting>(m => m.UIGID== entity.UIGID)
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
        
        
        
        public tb_UIGridSetting AddReEntity(tb_UIGridSetting entity)
        {
            tb_UIGridSetting AddEntity =  _tb_UIGridSettingServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_UIGridSetting>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_UIGridSetting> AddReEntityAsync(tb_UIGridSetting entity)
        {
            tb_UIGridSetting AddEntity = await _tb_UIGridSettingServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_UIGridSetting>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_UIGridSetting entity)
        {
            long id = await _tb_UIGridSettingServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_UIGridSetting>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_UIGridSetting> infos)
        {
            List<long> ids = await _tb_UIGridSettingServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_UIGridSetting>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_UIGridSetting entity)
        {
            bool rs = await _tb_UIGridSettingServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_UIGridSetting>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_UIGridSetting entity)
        {
            bool rs = await _tb_UIGridSettingServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_UIGridSetting>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_UIGridSettingServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_UIGridSetting>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_UIGridSettingServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_UIGridSetting>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_UIGridSetting>> QueryAsync()
        {
            List<tb_UIGridSetting> list = await  _tb_UIGridSettingServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_UIGridSetting>(list);
            return list;
        }
        
        public virtual List<tb_UIGridSetting> Query()
        {
            List<tb_UIGridSetting> list =  _tb_UIGridSettingServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_UIGridSetting>(list);
            return list;
        }
        
        public virtual List<tb_UIGridSetting> Query(string wheresql)
        {
            List<tb_UIGridSetting> list =  _tb_UIGridSettingServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_UIGridSetting>(list);
            return list;
        }
        
        public virtual async Task<List<tb_UIGridSetting>> QueryAsync(string wheresql) 
        {
            List<tb_UIGridSetting> list = await _tb_UIGridSettingServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_UIGridSetting>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_UIGridSetting>> QueryAsync(Expression<Func<tb_UIGridSetting, bool>> exp)
        {
            List<tb_UIGridSetting> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridSetting>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_UIGridSetting>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIGridSetting>> QueryByNavAsync()
        {
            List<tb_UIGridSetting> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridSetting>()
                               .Includes(t => t.tb_uimenupersonalization )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_UIGridSetting>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIGridSetting>> QueryByNavAsync(Expression<Func<tb_UIGridSetting, bool>> exp)
        {
            List<tb_UIGridSetting> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridSetting>().Where(exp)
                               .Includes(t => t.tb_uimenupersonalization )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_UIGridSetting>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_UIGridSetting> QueryByNav(Expression<Func<tb_UIGridSetting, bool>> exp)
        {
            List<tb_UIGridSetting> list = _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridSetting>().Where(exp)
                            .Includes(t => t.tb_uimenupersonalization )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_UIGridSetting>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_UIGridSetting>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridSetting>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_UIGridSettingServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_UIGridSetting entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridSetting>().Where(w => w.UIGID == (long)id)
                             .Includes(t => t.tb_uimenupersonalization )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_UIGridSetting>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



