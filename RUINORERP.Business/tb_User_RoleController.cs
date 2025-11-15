// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:26
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
    /// 用户角色关系表
    /// </summary>
    public partial class tb_User_RoleController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_User_RoleServices _tb_User_RoleServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_User_RoleController(ILogger<tb_User_RoleController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_User_RoleServices tb_User_RoleServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_User_RoleServices = tb_User_RoleServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_User_Role info)
        {

           // tb_User_RoleValidator validator = new tb_User_RoleValidator();
           tb_User_RoleValidator validator = _appContext.GetRequiredService<tb_User_RoleValidator>();
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
        public async Task<ReturnResults<tb_User_Role>> SaveOrUpdate(tb_User_Role entity)
        {
            ReturnResults<tb_User_Role> rr = new ReturnResults<tb_User_Role>();
            tb_User_Role Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ID > 0)
                {
                    bool rs = await _tb_User_RoleServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_User_Role>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_User_RoleServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_User_Role>(entity);
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
            tb_User_Role entity = model as tb_User_Role;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ID > 0)
                {
                    bool rs = await _tb_User_RoleServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_User_Role>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_User_RoleServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_User_Role>(entity);
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
            List<T> list = await _tb_User_RoleServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_User_Role entity = item as tb_User_Role;
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
            List<T> list = await _tb_User_RoleServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_User_Role entity = item as tb_User_Role;
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
            tb_User_Role entity = model as tb_User_Role;
            bool rs = await _tb_User_RoleServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_User_Role>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_User_Role> entitys = models as List<tb_User_Role>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_User_Role>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_User_Role>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_User_RoleValidator validator = new tb_User_RoleValidator();
           tb_User_RoleValidator validator = _appContext.GetRequiredService<tb_User_RoleValidator>();
            ValidationResult results = validator.Validate(info as tb_User_Role);
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

                tb_User_Role entity = model as tb_User_Role;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_User_Role>(entity as tb_User_Role)
                        .Include(m => m.tb_UserPersonalizeds)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_User_Role>(entity as tb_User_Role)
                .Include(m => m.tb_UserPersonalizeds)
         
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_User_Role>()
                                .Includes(m => m.tb_UserPersonalizeds)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_User_Role entity = model as tb_User_Role;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_User_Role>(m => m.ID== entity.ID)
                                .Include(m => m.tb_UserPersonalizeds)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_User_Role AddReEntity(tb_User_Role entity)
        {
            tb_User_Role AddEntity =  _tb_User_RoleServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_User_Role>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_User_Role> AddReEntityAsync(tb_User_Role entity)
        {
            tb_User_Role AddEntity = await _tb_User_RoleServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_User_Role>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_User_Role entity)
        {
            long id = await _tb_User_RoleServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_User_Role>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_User_Role> infos)
        {
            List<long> ids = await _tb_User_RoleServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_User_Role>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_User_Role entity)
        {
            bool rs = await _tb_User_RoleServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_User_Role>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_User_Role entity)
        {
            bool rs = await _tb_User_RoleServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_User_Role>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_User_RoleServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_User_Role>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_User_RoleServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_User_Role>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_User_Role>> QueryAsync()
        {
            List<tb_User_Role> list = await  _tb_User_RoleServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_User_Role>(list);
            return list;
        }
        
        public virtual List<tb_User_Role> Query()
        {
            List<tb_User_Role> list =  _tb_User_RoleServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_User_Role>(list);
            return list;
        }
        
        public virtual List<tb_User_Role> Query(string wheresql)
        {
            List<tb_User_Role> list =  _tb_User_RoleServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_User_Role>(list);
            return list;
        }
        
        public virtual async Task<List<tb_User_Role>> QueryAsync(string wheresql) 
        {
            List<tb_User_Role> list = await _tb_User_RoleServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_User_Role>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_User_Role>> QueryAsync(Expression<Func<tb_User_Role, bool>> exp)
        {
            List<tb_User_Role> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_User_Role>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_User_Role>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_User_Role>> QueryByNavAsync()
        {
            List<tb_User_Role> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_User_Role>()
                               .Includes(t => t.tb_userinfo )
                               .Includes(t => t.tb_roleinfo )
                                            .Includes(t => t.tb_UserPersonalizeds )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_User_Role>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_User_Role>> QueryByNavAsync(Expression<Func<tb_User_Role, bool>> exp)
        {
            List<tb_User_Role> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_User_Role>().Where(exp)
                               .Includes(t => t.tb_userinfo )
                               .Includes(t => t.tb_roleinfo )
                                            .Includes(t => t.tb_UserPersonalizeds )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_User_Role>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_User_Role> QueryByNav(Expression<Func<tb_User_Role, bool>> exp)
        {
            List<tb_User_Role> list = _unitOfWorkManage.GetDbClient().Queryable<tb_User_Role>().Where(exp)
                            .Includes(t => t.tb_userinfo )
                            .Includes(t => t.tb_roleinfo )
                                        .Includes(t => t.tb_UserPersonalizeds )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_User_Role>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_User_Role>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_User_Role>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_User_RoleServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_User_Role entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_User_Role>().Where(w => w.ID == (long)id)
                             .Includes(t => t.tb_userinfo )
                            .Includes(t => t.tb_roleinfo )
                        

                                            .Includes(t => t.tb_UserPersonalizeds )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_User_Role>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



