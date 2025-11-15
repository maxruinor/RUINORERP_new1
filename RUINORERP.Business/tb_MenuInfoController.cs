// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:13
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
    /// 菜单程序集信息表
    /// </summary>
    public partial class tb_MenuInfoController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_MenuInfoServices _tb_MenuInfoServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_MenuInfoController(ILogger<tb_MenuInfoController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_MenuInfoServices tb_MenuInfoServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_MenuInfoServices = tb_MenuInfoServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_MenuInfo info)
        {

           // tb_MenuInfoValidator validator = new tb_MenuInfoValidator();
           tb_MenuInfoValidator validator = _appContext.GetRequiredService<tb_MenuInfoValidator>();
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
        public async Task<ReturnResults<tb_MenuInfo>> SaveOrUpdate(tb_MenuInfo entity)
        {
            ReturnResults<tb_MenuInfo> rr = new ReturnResults<tb_MenuInfo>();
            tb_MenuInfo Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MenuID > 0)
                {
                    bool rs = await _tb_MenuInfoServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_MenuInfo>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_MenuInfoServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_MenuInfo>(entity);
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
            tb_MenuInfo entity = model as tb_MenuInfo;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MenuID > 0)
                {
                    bool rs = await _tb_MenuInfoServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_MenuInfo>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_MenuInfoServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_MenuInfo>(entity);
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
            List<T> list = await _tb_MenuInfoServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_MenuInfo entity = item as tb_MenuInfo;
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
            List<T> list = await _tb_MenuInfoServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_MenuInfo entity = item as tb_MenuInfo;
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
            tb_MenuInfo entity = model as tb_MenuInfo;
            bool rs = await _tb_MenuInfoServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_MenuInfo>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_MenuInfo> entitys = models as List<tb_MenuInfo>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_MenuInfo>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_MenuInfo>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_MenuInfoValidator validator = new tb_MenuInfoValidator();
           tb_MenuInfoValidator validator = _appContext.GetRequiredService<tb_MenuInfoValidator>();
            ValidationResult results = validator.Validate(info as tb_MenuInfo);
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

                tb_MenuInfo entity = model as tb_MenuInfo;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.MenuID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_MenuInfo>(entity as tb_MenuInfo)
                        .Include(m => m.tb_FieldInfos)
                    .Include(m => m.tb_UIMenuPersonalizations)
                    .Include(m => m.tb_P4Fields)
                    .Include(m => m.tb_ButtonInfos)
                    .Include(m => m.tb_P4RowAuthPolicyByRoles)
                    .Include(m => m.tb_P4RowAuthPolicyByUsers)
                    .Include(m => m.tb_P4Buttons)
                    .Include(m => m.tb_P4Menus)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_MenuInfo>(entity as tb_MenuInfo)
                .Include(m => m.tb_FieldInfos)
                .Include(m => m.tb_UIMenuPersonalizations)
                .Include(m => m.tb_P4Fields)
                .Include(m => m.tb_ButtonInfos)
                .Include(m => m.tb_P4RowAuthPolicyByRoles)
                .Include(m => m.tb_P4RowAuthPolicyByUsers)
                .Include(m => m.tb_P4Buttons)
                .Include(m => m.tb_P4Menus)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.MenuID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MenuInfo>()
                                .Includes(m => m.tb_FieldInfos)
                        .Includes(m => m.tb_UIMenuPersonalizations)
                        .Includes(m => m.tb_P4Fields)
                        .Includes(m => m.tb_ButtonInfos)
                        .Includes(m => m.tb_P4RowAuthPolicyByRoles)
                        .Includes(m => m.tb_P4RowAuthPolicyByUsers)
                        .Includes(m => m.tb_P4Buttons)
                        .Includes(m => m.tb_P4Menus)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_MenuInfo entity = model as tb_MenuInfo;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_MenuInfo>(m => m.MenuID== entity.MenuID)
                                .Include(m => m.tb_FieldInfos)
                        .Include(m => m.tb_UIMenuPersonalizations)
                        .Include(m => m.tb_P4Fields)
                        .Include(m => m.tb_ButtonInfos)
                        .Include(m => m.tb_P4RowAuthPolicyByRoles)
                        .Include(m => m.tb_P4RowAuthPolicyByUsers)
                        .Include(m => m.tb_P4Buttons)
                        .Include(m => m.tb_P4Menus)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_MenuInfo AddReEntity(tb_MenuInfo entity)
        {
            tb_MenuInfo AddEntity =  _tb_MenuInfoServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_MenuInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_MenuInfo> AddReEntityAsync(tb_MenuInfo entity)
        {
            tb_MenuInfo AddEntity = await _tb_MenuInfoServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_MenuInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_MenuInfo entity)
        {
            long id = await _tb_MenuInfoServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_MenuInfo>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_MenuInfo> infos)
        {
            List<long> ids = await _tb_MenuInfoServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_MenuInfo>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_MenuInfo entity)
        {
            bool rs = await _tb_MenuInfoServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_MenuInfo>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_MenuInfo entity)
        {
            bool rs = await _tb_MenuInfoServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_MenuInfo>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_MenuInfoServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_MenuInfo>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_MenuInfoServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_MenuInfo>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_MenuInfo>> QueryAsync()
        {
            List<tb_MenuInfo> list = await  _tb_MenuInfoServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_MenuInfo>(list);
            return list;
        }
        
        public virtual List<tb_MenuInfo> Query()
        {
            List<tb_MenuInfo> list =  _tb_MenuInfoServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_MenuInfo>(list);
            return list;
        }
        
        public virtual List<tb_MenuInfo> Query(string wheresql)
        {
            List<tb_MenuInfo> list =  _tb_MenuInfoServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_MenuInfo>(list);
            return list;
        }
        
        public virtual async Task<List<tb_MenuInfo>> QueryAsync(string wheresql) 
        {
            List<tb_MenuInfo> list = await _tb_MenuInfoServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_MenuInfo>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_MenuInfo>> QueryAsync(Expression<Func<tb_MenuInfo, bool>> exp)
        {
            List<tb_MenuInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MenuInfo>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_MenuInfo>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MenuInfo>> QueryByNavAsync()
        {
            List<tb_MenuInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MenuInfo>()
                               .Includes(t => t.tb_moduledefinition )
                                            .Includes(t => t.tb_FieldInfos )
                                .Includes(t => t.tb_UIMenuPersonalizations )
                                .Includes(t => t.tb_P4Fields )
                                .Includes(t => t.tb_ButtonInfos )
                                .Includes(t => t.tb_P4RowAuthPolicyByRoles )
                                .Includes(t => t.tb_P4RowAuthPolicyByUsers )
                                .Includes(t => t.tb_P4Buttons )
                                .Includes(t => t.tb_P4Menus )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_MenuInfo>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MenuInfo>> QueryByNavAsync(Expression<Func<tb_MenuInfo, bool>> exp)
        {
            List<tb_MenuInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MenuInfo>().Where(exp)
                               .Includes(t => t.tb_moduledefinition )
                                            .Includes(t => t.tb_FieldInfos )
                                .Includes(t => t.tb_UIMenuPersonalizations )
                                .Includes(t => t.tb_P4Fields )
                                .Includes(t => t.tb_ButtonInfos )
                                .Includes(t => t.tb_P4RowAuthPolicyByRoles )
                                .Includes(t => t.tb_P4RowAuthPolicyByUsers )
                                .Includes(t => t.tb_P4Buttons )
                                .Includes(t => t.tb_P4Menus )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_MenuInfo>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_MenuInfo> QueryByNav(Expression<Func<tb_MenuInfo, bool>> exp)
        {
            List<tb_MenuInfo> list = _unitOfWorkManage.GetDbClient().Queryable<tb_MenuInfo>().Where(exp)
                            .Includes(t => t.tb_moduledefinition )
                                        .Includes(t => t.tb_FieldInfos )
                            .Includes(t => t.tb_UIMenuPersonalizations )
                            .Includes(t => t.tb_P4Fields )
                            .Includes(t => t.tb_ButtonInfos )
                            .Includes(t => t.tb_P4RowAuthPolicyByRoles )
                            .Includes(t => t.tb_P4RowAuthPolicyByUsers )
                            .Includes(t => t.tb_P4Buttons )
                            .Includes(t => t.tb_P4Menus )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_MenuInfo>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_MenuInfo>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MenuInfo>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_MenuInfoServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_MenuInfo entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_MenuInfo>().Where(w => w.MenuID == (long)id)
                             .Includes(t => t.tb_moduledefinition )
                        

                                            .Includes(t => t.tb_FieldInfos )
                                            .Includes(t => t.tb_UIMenuPersonalizations )
                                            .Includes(t => t.tb_P4Fields )
                                            .Includes(t => t.tb_ButtonInfos )
                                            .Includes(t => t.tb_P4RowAuthPolicyByRoles )
                                            .Includes(t => t.tb_P4RowAuthPolicyByUsers )
                                            .Includes(t => t.tb_P4Buttons )
                                            .Includes(t => t.tb_P4Menus )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_MenuInfo>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



