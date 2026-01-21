// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/21/2026 18:12:14
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
    /// 文件信息元数据表
    /// </summary>
    public partial class tb_FS_FileStorageInfoController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FS_FileStorageInfoServices _tb_FS_FileStorageInfoServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_FS_FileStorageInfoController(ILogger<tb_FS_FileStorageInfoController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FS_FileStorageInfoServices tb_FS_FileStorageInfoServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FS_FileStorageInfoServices = tb_FS_FileStorageInfoServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_FS_FileStorageInfo info)
        {

           // tb_FS_FileStorageInfoValidator validator = new tb_FS_FileStorageInfoValidator();
           tb_FS_FileStorageInfoValidator validator = _appContext.GetRequiredService<tb_FS_FileStorageInfoValidator>();
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
        public async Task<ReturnResults<tb_FS_FileStorageInfo>> SaveOrUpdate(tb_FS_FileStorageInfo entity)
        {
            ReturnResults<tb_FS_FileStorageInfo> rr = new ReturnResults<tb_FS_FileStorageInfo>();
            tb_FS_FileStorageInfo Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.FileId > 0)
                {
                    bool rs = await _tb_FS_FileStorageInfoServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_FS_FileStorageInfo>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FS_FileStorageInfoServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_FS_FileStorageInfo>(entity);
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
            tb_FS_FileStorageInfo entity = model as tb_FS_FileStorageInfo;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.FileId > 0)
                {
                    bool rs = await _tb_FS_FileStorageInfoServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_FS_FileStorageInfo>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FS_FileStorageInfoServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_FS_FileStorageInfo>(entity);
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
            List<T> list = await _tb_FS_FileStorageInfoServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FS_FileStorageInfo entity = item as tb_FS_FileStorageInfo;
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
            List<T> list = await _tb_FS_FileStorageInfoServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FS_FileStorageInfo entity = item as tb_FS_FileStorageInfo;
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
            tb_FS_FileStorageInfo entity = model as tb_FS_FileStorageInfo;
            bool rs = await _tb_FS_FileStorageInfoServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_FS_FileStorageInfo>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FS_FileStorageInfo> entitys = models as List<tb_FS_FileStorageInfo>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FS_FileStorageInfo>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_FS_FileStorageInfo>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FS_FileStorageInfoValidator validator = new tb_FS_FileStorageInfoValidator();
           tb_FS_FileStorageInfoValidator validator = _appContext.GetRequiredService<tb_FS_FileStorageInfoValidator>();
            ValidationResult results = validator.Validate(info as tb_FS_FileStorageInfo);
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

                tb_FS_FileStorageInfo entity = model as tb_FS_FileStorageInfo;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.FileId > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FS_FileStorageInfo>(entity as tb_FS_FileStorageInfo)
                        .Include(m => m.tb_FS_BusinessRelations)
                    .Include(m => m.tb_FS_FileStorageVersions)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FS_FileStorageInfo>(entity as tb_FS_FileStorageInfo)
                .Include(m => m.tb_FS_BusinessRelations)
                .Include(m => m.tb_FS_FileStorageVersions)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.FileId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageInfo>()
                                                .Includes(m => m.tb_FS_BusinessRelations)
                        .Includes(m => m.tb_FS_FileStorageVersions)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FS_FileStorageInfo entity = model as tb_FS_FileStorageInfo;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FS_FileStorageInfo>(m => m.FileId== entity.FileId)
                                .Include(m => m.tb_FS_BusinessRelations)
                        .Include(m => m.tb_FS_FileStorageVersions)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FS_FileStorageInfo AddReEntity(tb_FS_FileStorageInfo entity)
        {
            tb_FS_FileStorageInfo AddEntity =  _tb_FS_FileStorageInfoServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_FS_FileStorageInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FS_FileStorageInfo> AddReEntityAsync(tb_FS_FileStorageInfo entity)
        {
            tb_FS_FileStorageInfo AddEntity = await _tb_FS_FileStorageInfoServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_FS_FileStorageInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FS_FileStorageInfo entity)
        {
            long id = await _tb_FS_FileStorageInfoServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_FS_FileStorageInfo>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FS_FileStorageInfo> infos)
        {
            List<long> ids = await _tb_FS_FileStorageInfoServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_FS_FileStorageInfo>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FS_FileStorageInfo entity)
        {
            bool rs = await _tb_FS_FileStorageInfoServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_FS_FileStorageInfo>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FS_FileStorageInfo entity)
        {
            bool rs = await _tb_FS_FileStorageInfoServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_FS_FileStorageInfo>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FS_FileStorageInfoServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_FS_FileStorageInfo>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FS_FileStorageInfoServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_FS_FileStorageInfo>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FS_FileStorageInfo>> QueryAsync()
        {
            List<tb_FS_FileStorageInfo> list = await  _tb_FS_FileStorageInfoServices.QueryAsync();
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_FS_FileStorageInfo>(list);
            return list;
        }
        
        public virtual List<tb_FS_FileStorageInfo> Query()
        {
            List<tb_FS_FileStorageInfo> list =  _tb_FS_FileStorageInfoServices.Query();
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_FS_FileStorageInfo>(list);
            return list;
        }
        
        public virtual List<tb_FS_FileStorageInfo> Query(string wheresql)
        {
            List<tb_FS_FileStorageInfo> list =  _tb_FS_FileStorageInfoServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_FS_FileStorageInfo>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FS_FileStorageInfo>> QueryAsync(string wheresql) 
        {
            List<tb_FS_FileStorageInfo> list = await _tb_FS_FileStorageInfoServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_FS_FileStorageInfo>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FS_FileStorageInfo>> QueryAsync(Expression<Func<tb_FS_FileStorageInfo, bool>> exp)
        {
            List<tb_FS_FileStorageInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageInfo>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_FS_FileStorageInfo>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FS_FileStorageInfo>> QueryByNavAsync()
        {
            List<tb_FS_FileStorageInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageInfo>()
                                            .Includes(t => t.tb_FS_BusinessRelations )
                                .Includes(t => t.tb_FS_FileStorageVersions )
                        .ToListAsync();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_FS_FileStorageInfo>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FS_FileStorageInfo>> QueryByNavAsync(Expression<Func<tb_FS_FileStorageInfo, bool>> exp)
        {
            List<tb_FS_FileStorageInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageInfo>().Where(exp)
                                            .Includes(t => t.tb_FS_BusinessRelations )
                                .Includes(t => t.tb_FS_FileStorageVersions )
                        .ToListAsync();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_FS_FileStorageInfo>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FS_FileStorageInfo> QueryByNav(Expression<Func<tb_FS_FileStorageInfo, bool>> exp)
        {
            List<tb_FS_FileStorageInfo> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageInfo>().Where(exp)
                                        .Includes(t => t.tb_FS_BusinessRelations )
                            .Includes(t => t.tb_FS_FileStorageVersions )
                        .ToList();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_FS_FileStorageInfo>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FS_FileStorageInfo>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageInfo>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FS_FileStorageInfoServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FS_FileStorageInfo entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageInfo>().Where(w => w.FileId == (long)id)
                         

                                            .Includes(t => t.tb_FS_BusinessRelations )
                                            .Includes(t => t.tb_FS_FileStorageVersions )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_FS_FileStorageInfo>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



