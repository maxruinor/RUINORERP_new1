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
    /// 销售分区表-大中华区
    /// </summary>
    public partial class tb_CRM_RegionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CRM_RegionServices _tb_CRM_RegionServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_CRM_RegionController(ILogger<tb_CRM_RegionController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CRM_RegionServices tb_CRM_RegionServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CRM_RegionServices = tb_CRM_RegionServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_CRM_Region info)
        {

           // tb_CRM_RegionValidator validator = new tb_CRM_RegionValidator();
           tb_CRM_RegionValidator validator = _appContext.GetRequiredService<tb_CRM_RegionValidator>();
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
        public async Task<ReturnResults<tb_CRM_Region>> SaveOrUpdate(tb_CRM_Region entity)
        {
            ReturnResults<tb_CRM_Region> rr = new ReturnResults<tb_CRM_Region>();
            tb_CRM_Region Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Region_ID > 0)
                {
                    bool rs = await _tb_CRM_RegionServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CRM_Region>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CRM_RegionServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_CRM_Region>(entity);
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
            tb_CRM_Region entity = model as tb_CRM_Region;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Region_ID > 0)
                {
                    bool rs = await _tb_CRM_RegionServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CRM_Region>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CRM_RegionServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_CRM_Region>(entity);
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
            List<T> list = await _tb_CRM_RegionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_CRM_Region entity = item as tb_CRM_Region;
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
            List<T> list = await _tb_CRM_RegionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_CRM_Region entity = item as tb_CRM_Region;
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
            tb_CRM_Region entity = model as tb_CRM_Region;
            bool rs = await _tb_CRM_RegionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_CRM_Region>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_CRM_Region> entitys = models as List<tb_CRM_Region>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_CRM_Region>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_CRM_Region>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_CRM_RegionValidator validator = new tb_CRM_RegionValidator();
           tb_CRM_RegionValidator validator = _appContext.GetRequiredService<tb_CRM_RegionValidator>();
            ValidationResult results = validator.Validate(info as tb_CRM_Region);
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

                tb_CRM_Region entity = model as tb_CRM_Region;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Region_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_CRM_Region>(entity as tb_CRM_Region)
                    .Include(m => m.tb_CRM_Customers)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_CRM_Region>(entity as tb_CRM_Region)
                .Include(m => m.tb_CRM_Customers)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Region_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Region>()
                        .Includes(m => m.tb_CRM_Customers)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CRM_Region entity = model as tb_CRM_Region;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CRM_Region>(m => m.Region_ID== entity.Region_ID)
                        .Include(m => m.tb_CRM_Customers)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_CRM_Region AddReEntity(tb_CRM_Region entity)
        {
            tb_CRM_Region AddEntity =  _tb_CRM_RegionServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_CRM_Region>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CRM_Region> AddReEntityAsync(tb_CRM_Region entity)
        {
            tb_CRM_Region AddEntity = await _tb_CRM_RegionServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_CRM_Region>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CRM_Region entity)
        {
            long id = await _tb_CRM_RegionServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_CRM_Region>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CRM_Region> infos)
        {
            List<long> ids = await _tb_CRM_RegionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_CRM_Region>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CRM_Region entity)
        {
            bool rs = await _tb_CRM_RegionServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_CRM_Region>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CRM_Region entity)
        {
            bool rs = await _tb_CRM_RegionServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_CRM_Region>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CRM_RegionServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_CRM_Region>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CRM_RegionServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_CRM_Region>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_CRM_Region>> QueryAsync()
        {
            List<tb_CRM_Region> list = await  _tb_CRM_RegionServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CRM_Region>(list);
            return list;
        }
        
        public virtual List<tb_CRM_Region> Query()
        {
            List<tb_CRM_Region> list =  _tb_CRM_RegionServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_CRM_Region>(list);
            return list;
        }
        
        public virtual List<tb_CRM_Region> Query(string wheresql)
        {
            List<tb_CRM_Region> list =  _tb_CRM_RegionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CRM_Region>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CRM_Region>> QueryAsync(string wheresql) 
        {
            List<tb_CRM_Region> list = await _tb_CRM_RegionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CRM_Region>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_CRM_Region>> QueryAsync(Expression<Func<tb_CRM_Region, bool>> exp)
        {
            List<tb_CRM_Region> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Region>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_CRM_Region>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CRM_Region>> QueryByNavAsync()
        {
            List<tb_CRM_Region> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Region>()
                                .Includes(t => t.tb_CRM_Customers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CRM_Region>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CRM_Region>> QueryByNavAsync(Expression<Func<tb_CRM_Region, bool>> exp)
        {
            List<tb_CRM_Region> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Region>().Where(exp)
                                .Includes(t => t.tb_CRM_Customers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CRM_Region>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CRM_Region> QueryByNav(Expression<Func<tb_CRM_Region, bool>> exp)
        {
            List<tb_CRM_Region> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Region>().Where(exp)
                            .Includes(t => t.tb_CRM_Customers )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CRM_Region>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CRM_Region>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Region>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CRM_RegionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_CRM_Region entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Region>().Where(w => w.Region_ID == (long)id)
                         

                                            .Includes(t => t.tb_CRM_Customers )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_CRM_Region>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



