// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:15
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
    /// 行级权限规则-角色关联表
    /// </summary>
    public partial class tb_P4RowAuthPolicyByRoleController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_P4RowAuthPolicyByRoleServices _tb_P4RowAuthPolicyByRoleServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_P4RowAuthPolicyByRoleController(ILogger<tb_P4RowAuthPolicyByRoleController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_P4RowAuthPolicyByRoleServices tb_P4RowAuthPolicyByRoleServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_P4RowAuthPolicyByRoleServices = tb_P4RowAuthPolicyByRoleServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_P4RowAuthPolicyByRole info)
        {

           // tb_P4RowAuthPolicyByRoleValidator validator = new tb_P4RowAuthPolicyByRoleValidator();
           tb_P4RowAuthPolicyByRoleValidator validator = _appContext.GetRequiredService<tb_P4RowAuthPolicyByRoleValidator>();
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
        public async Task<ReturnResults<tb_P4RowAuthPolicyByRole>> SaveOrUpdate(tb_P4RowAuthPolicyByRole entity)
        {
            ReturnResults<tb_P4RowAuthPolicyByRole> rr = new ReturnResults<tb_P4RowAuthPolicyByRole>();
            tb_P4RowAuthPolicyByRole Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Policy_Role_RID > 0)
                {
                    bool rs = await _tb_P4RowAuthPolicyByRoleServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_P4RowAuthPolicyByRole>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_P4RowAuthPolicyByRoleServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_P4RowAuthPolicyByRole>(entity);
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
            tb_P4RowAuthPolicyByRole entity = model as tb_P4RowAuthPolicyByRole;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Policy_Role_RID > 0)
                {
                    bool rs = await _tb_P4RowAuthPolicyByRoleServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_P4RowAuthPolicyByRole>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_P4RowAuthPolicyByRoleServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_P4RowAuthPolicyByRole>(entity);
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
            List<T> list = await _tb_P4RowAuthPolicyByRoleServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_P4RowAuthPolicyByRole entity = item as tb_P4RowAuthPolicyByRole;
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
            List<T> list = await _tb_P4RowAuthPolicyByRoleServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_P4RowAuthPolicyByRole entity = item as tb_P4RowAuthPolicyByRole;
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
            tb_P4RowAuthPolicyByRole entity = model as tb_P4RowAuthPolicyByRole;
            bool rs = await _tb_P4RowAuthPolicyByRoleServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_P4RowAuthPolicyByRole>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_P4RowAuthPolicyByRole> entitys = models as List<tb_P4RowAuthPolicyByRole>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_P4RowAuthPolicyByRole>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_P4RowAuthPolicyByRole>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_P4RowAuthPolicyByRoleValidator validator = new tb_P4RowAuthPolicyByRoleValidator();
           tb_P4RowAuthPolicyByRoleValidator validator = _appContext.GetRequiredService<tb_P4RowAuthPolicyByRoleValidator>();
            ValidationResult results = validator.Validate(info as tb_P4RowAuthPolicyByRole);
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

                tb_P4RowAuthPolicyByRole entity = model as tb_P4RowAuthPolicyByRole;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Policy_Role_RID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_P4RowAuthPolicyByRole>(entity as tb_P4RowAuthPolicyByRole)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_P4RowAuthPolicyByRole>(entity as tb_P4RowAuthPolicyByRole)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Policy_Role_RID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByRole>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_P4RowAuthPolicyByRole entity = model as tb_P4RowAuthPolicyByRole;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_P4RowAuthPolicyByRole>(m => m.Policy_Role_RID== entity.Policy_Role_RID)
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
        
        
        
        public tb_P4RowAuthPolicyByRole AddReEntity(tb_P4RowAuthPolicyByRole entity)
        {
            tb_P4RowAuthPolicyByRole AddEntity =  _tb_P4RowAuthPolicyByRoleServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_P4RowAuthPolicyByRole>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_P4RowAuthPolicyByRole> AddReEntityAsync(tb_P4RowAuthPolicyByRole entity)
        {
            tb_P4RowAuthPolicyByRole AddEntity = await _tb_P4RowAuthPolicyByRoleServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_P4RowAuthPolicyByRole>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_P4RowAuthPolicyByRole entity)
        {
            long id = await _tb_P4RowAuthPolicyByRoleServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_P4RowAuthPolicyByRole>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_P4RowAuthPolicyByRole> infos)
        {
            List<long> ids = await _tb_P4RowAuthPolicyByRoleServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_P4RowAuthPolicyByRole>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_P4RowAuthPolicyByRole entity)
        {
            bool rs = await _tb_P4RowAuthPolicyByRoleServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_P4RowAuthPolicyByRole>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_P4RowAuthPolicyByRole entity)
        {
            bool rs = await _tb_P4RowAuthPolicyByRoleServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_P4RowAuthPolicyByRole>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_P4RowAuthPolicyByRoleServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_P4RowAuthPolicyByRole>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_P4RowAuthPolicyByRoleServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_P4RowAuthPolicyByRole>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_P4RowAuthPolicyByRole>> QueryAsync()
        {
            List<tb_P4RowAuthPolicyByRole> list = await  _tb_P4RowAuthPolicyByRoleServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_P4RowAuthPolicyByRole>(list);
            return list;
        }
        
        public virtual List<tb_P4RowAuthPolicyByRole> Query()
        {
            List<tb_P4RowAuthPolicyByRole> list =  _tb_P4RowAuthPolicyByRoleServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_P4RowAuthPolicyByRole>(list);
            return list;
        }
        
        public virtual List<tb_P4RowAuthPolicyByRole> Query(string wheresql)
        {
            List<tb_P4RowAuthPolicyByRole> list =  _tb_P4RowAuthPolicyByRoleServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_P4RowAuthPolicyByRole>(list);
            return list;
        }
        
        public virtual async Task<List<tb_P4RowAuthPolicyByRole>> QueryAsync(string wheresql) 
        {
            List<tb_P4RowAuthPolicyByRole> list = await _tb_P4RowAuthPolicyByRoleServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_P4RowAuthPolicyByRole>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_P4RowAuthPolicyByRole>> QueryAsync(Expression<Func<tb_P4RowAuthPolicyByRole, bool>> exp)
        {
            List<tb_P4RowAuthPolicyByRole> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByRole>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_P4RowAuthPolicyByRole>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_P4RowAuthPolicyByRole>> QueryByNavAsync()
        {
            List<tb_P4RowAuthPolicyByRole> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByRole>()
                               .Includes(t => t.tb_rowauthpolicy )
                               .Includes(t => t.tb_menuinfo )
                               .Includes(t => t.tb_roleinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_P4RowAuthPolicyByRole>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_P4RowAuthPolicyByRole>> QueryByNavAsync(Expression<Func<tb_P4RowAuthPolicyByRole, bool>> exp)
        {
            List<tb_P4RowAuthPolicyByRole> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByRole>().Where(exp)
                               .Includes(t => t.tb_rowauthpolicy )
                               .Includes(t => t.tb_menuinfo )
                               .Includes(t => t.tb_roleinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_P4RowAuthPolicyByRole>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_P4RowAuthPolicyByRole> QueryByNav(Expression<Func<tb_P4RowAuthPolicyByRole, bool>> exp)
        {
            List<tb_P4RowAuthPolicyByRole> list = _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByRole>().Where(exp)
                            .Includes(t => t.tb_rowauthpolicy )
                            .Includes(t => t.tb_menuinfo )
                            .Includes(t => t.tb_roleinfo )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_P4RowAuthPolicyByRole>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_P4RowAuthPolicyByRole>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByRole>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_P4RowAuthPolicyByRoleServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_P4RowAuthPolicyByRole entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByRole>().Where(w => w.Policy_Role_RID == (long)id)
                             .Includes(t => t.tb_rowauthpolicy )
                            .Includes(t => t.tb_menuinfo )
                            .Includes(t => t.tb_roleinfo )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_P4RowAuthPolicyByRole>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



