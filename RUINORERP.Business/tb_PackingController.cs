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
    /// 包装规格表
    /// </summary>
    public partial class tb_PackingController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_PackingServices _tb_PackingServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_PackingController(ILogger<tb_PackingController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_PackingServices tb_PackingServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_PackingServices = tb_PackingServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_Packing info)
        {

           // tb_PackingValidator validator = new tb_PackingValidator();
           tb_PackingValidator validator = _appContext.GetRequiredService<tb_PackingValidator>();
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
        public async Task<ReturnResults<tb_Packing>> SaveOrUpdate(tb_Packing entity)
        {
            ReturnResults<tb_Packing> rr = new ReturnResults<tb_Packing>();
            tb_Packing Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Pack_ID > 0)
                {
                    bool rs = await _tb_PackingServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Packing>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_PackingServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_Packing>(entity);
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
            tb_Packing entity = model as tb_Packing;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Pack_ID > 0)
                {
                    bool rs = await _tb_PackingServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Packing>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_PackingServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_Packing>(entity);
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
            List<T> list = await _tb_PackingServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Packing entity = item as tb_Packing;
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
            List<T> list = await _tb_PackingServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Packing entity = item as tb_Packing;
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
            tb_Packing entity = model as tb_Packing;
            bool rs = await _tb_PackingServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_Packing>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Packing> entitys = models as List<tb_Packing>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Packing>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_Packing>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_PackingValidator validator = new tb_PackingValidator();
           tb_PackingValidator validator = _appContext.GetRequiredService<tb_PackingValidator>();
            ValidationResult results = validator.Validate(info as tb_Packing);
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

                tb_Packing entity = model as tb_Packing;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Pack_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Packing>(entity as tb_Packing)
                        .Include(m => m.tb_BoxRuleses)
                    .Include(m => m.tb_PackingDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Packing>(entity as tb_Packing)
                .Include(m => m.tb_BoxRuleses)
                .Include(m => m.tb_PackingDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Pack_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Packing>()
                                .Includes(m => m.tb_BoxRuleses)
                        .Includes(m => m.tb_PackingDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Packing entity = model as tb_Packing;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Packing>(m => m.Pack_ID== entity.Pack_ID)
                                .Include(m => m.tb_BoxRuleses)
                        .Include(m => m.tb_PackingDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Packing AddReEntity(tb_Packing entity)
        {
            tb_Packing AddEntity =  _tb_PackingServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_Packing>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Packing> AddReEntityAsync(tb_Packing entity)
        {
            tb_Packing AddEntity = await _tb_PackingServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_Packing>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Packing entity)
        {
            long id = await _tb_PackingServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_Packing>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Packing> infos)
        {
            List<long> ids = await _tb_PackingServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_Packing>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Packing entity)
        {
            bool rs = await _tb_PackingServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_Packing>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Packing entity)
        {
            bool rs = await _tb_PackingServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_Packing>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_PackingServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_Packing>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_PackingServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_Packing>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Packing>> QueryAsync()
        {
            List<tb_Packing> list = await  _tb_PackingServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Packing>(list);
            return list;
        }
        
        public virtual List<tb_Packing> Query()
        {
            List<tb_Packing> list =  _tb_PackingServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_Packing>(list);
            return list;
        }
        
        public virtual List<tb_Packing> Query(string wheresql)
        {
            List<tb_Packing> list =  _tb_PackingServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Packing>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Packing>> QueryAsync(string wheresql) 
        {
            List<tb_Packing> list = await _tb_PackingServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Packing>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Packing>> QueryAsync(Expression<Func<tb_Packing, bool>> exp)
        {
            List<tb_Packing> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Packing>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_Packing>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Packing>> QueryByNavAsync()
        {
            List<tb_Packing> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Packing>()
                               .Includes(t => t.tb_prodbundle )
                               .Includes(t => t.tb_unit )
                               .Includes(t => t.tb_prod )
                               .Includes(t => t.tb_proddetail )
                                            .Includes(t => t.tb_BoxRuleses )
                                .Includes(t => t.tb_PackingDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Packing>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Packing>> QueryByNavAsync(Expression<Func<tb_Packing, bool>> exp)
        {
            List<tb_Packing> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Packing>().Where(exp)
                               .Includes(t => t.tb_prodbundle )
                               .Includes(t => t.tb_unit )
                               .Includes(t => t.tb_prod )
                               .Includes(t => t.tb_proddetail )
                                            .Includes(t => t.tb_BoxRuleses )
                                .Includes(t => t.tb_PackingDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Packing>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Packing> QueryByNav(Expression<Func<tb_Packing, bool>> exp)
        {
            List<tb_Packing> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Packing>().Where(exp)
                            .Includes(t => t.tb_prodbundle )
                            .Includes(t => t.tb_unit )
                            .Includes(t => t.tb_prod )
                            .Includes(t => t.tb_proddetail )
                                        .Includes(t => t.tb_BoxRuleses )
                            .Includes(t => t.tb_PackingDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Packing>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Packing>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Packing>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_PackingServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Packing entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Packing>().Where(w => w.Pack_ID == (long)id)
                             .Includes(t => t.tb_prodbundle )
                            .Includes(t => t.tb_unit )
                            .Includes(t => t.tb_prod )
                            .Includes(t => t.tb_proddetail )
                        

                                            .Includes(t => t.tb_BoxRuleses )
                                            .Includes(t => t.tb_PackingDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_Packing>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



