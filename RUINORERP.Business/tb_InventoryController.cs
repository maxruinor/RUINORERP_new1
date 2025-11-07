// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:12
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
    /// 库存表
    /// </summary>
    public partial class tb_InventoryController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_InventoryServices _tb_InventoryServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_InventoryController(ILogger<tb_InventoryController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_InventoryServices tb_InventoryServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_InventoryServices = tb_InventoryServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_Inventory info)
        {

           // tb_InventoryValidator validator = new tb_InventoryValidator();
           tb_InventoryValidator validator = _appContext.GetRequiredService<tb_InventoryValidator>();
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
        public async Task<ReturnResults<tb_Inventory>> SaveOrUpdate(tb_Inventory entity)
        {
            ReturnResults<tb_Inventory> rr = new ReturnResults<tb_Inventory>();
            tb_Inventory Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Inventory_ID > 0)
                {
                    bool rs = await _tb_InventoryServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Inventory>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_InventoryServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_Inventory>(entity);
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
            tb_Inventory entity = model as tb_Inventory;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Inventory_ID > 0)
                {
                    bool rs = await _tb_InventoryServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Inventory>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_InventoryServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_Inventory>(entity);
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
            List<T> list = await _tb_InventoryServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Inventory entity = item as tb_Inventory;
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
            List<T> list = await _tb_InventoryServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Inventory entity = item as tb_Inventory;
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
            tb_Inventory entity = model as tb_Inventory;
            bool rs = await _tb_InventoryServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_Inventory>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Inventory> entitys = models as List<tb_Inventory>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Inventory>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_Inventory>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_InventoryValidator validator = new tb_InventoryValidator();
           tb_InventoryValidator validator = _appContext.GetRequiredService<tb_InventoryValidator>();
            ValidationResult results = validator.Validate(info as tb_Inventory);
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

                tb_Inventory entity = model as tb_Inventory;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Inventory_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Inventory>(entity as tb_Inventory)
                        .Include(m => m.tb_Inv_Alert_Attributes)
                    .Include(m => m.tb_OpeningInventories)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Inventory>(entity as tb_Inventory)
                .Include(m => m.tb_Inv_Alert_Attributes)
                .Include(m => m.tb_OpeningInventories)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Inventory_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>()
                                .Includes(m => m.tb_Inv_Alert_Attributes)
                        .Includes(m => m.tb_OpeningInventories)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Inventory entity = model as tb_Inventory;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Inventory>(m => m.Inventory_ID== entity.Inventory_ID)
                                .Include(m => m.tb_Inv_Alert_Attributes)
                        .Include(m => m.tb_OpeningInventories)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Inventory AddReEntity(tb_Inventory entity)
        {
            tb_Inventory AddEntity =  _tb_InventoryServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_Inventory>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Inventory> AddReEntityAsync(tb_Inventory entity)
        {
            tb_Inventory AddEntity = await _tb_InventoryServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_Inventory>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Inventory entity)
        {
            long id = await _tb_InventoryServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_Inventory>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Inventory> infos)
        {
            List<long> ids = await _tb_InventoryServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_Inventory>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Inventory entity)
        {
            bool rs = await _tb_InventoryServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_Inventory>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Inventory entity)
        {
            bool rs = await _tb_InventoryServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_Inventory>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_InventoryServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_Inventory>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_InventoryServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_Inventory>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Inventory>> QueryAsync()
        {
            List<tb_Inventory> list = await  _tb_InventoryServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Inventory>(list);
            return list;
        }
        
        public virtual List<tb_Inventory> Query()
        {
            List<tb_Inventory> list =  _tb_InventoryServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_Inventory>(list);
            return list;
        }
        
        public virtual List<tb_Inventory> Query(string wheresql)
        {
            List<tb_Inventory> list =  _tb_InventoryServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Inventory>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Inventory>> QueryAsync(string wheresql) 
        {
            List<tb_Inventory> list = await _tb_InventoryServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Inventory>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Inventory>> QueryAsync(Expression<Func<tb_Inventory, bool>> exp)
        {
            List<tb_Inventory> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_Inventory>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Inventory>> QueryByNavAsync()
        {
            List<tb_Inventory> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>()
                               .Includes(t => t.tb_storagerack )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_location )
                                            .Includes(t => t.tb_Inv_Alert_Attributes )
                                .Includes(t => t.tb_OpeningInventories )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Inventory>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Inventory>> QueryByNavAsync(Expression<Func<tb_Inventory, bool>> exp)
        {
            List<tb_Inventory> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>().Where(exp)
                               .Includes(t => t.tb_storagerack )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_location )
                                            .Includes(t => t.tb_Inv_Alert_Attributes )
                                .Includes(t => t.tb_OpeningInventories )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Inventory>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Inventory> QueryByNav(Expression<Func<tb_Inventory, bool>> exp)
        {
            List<tb_Inventory> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>().Where(exp)
                            .Includes(t => t.tb_storagerack )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_location )
                                        .Includes(t => t.tb_Inv_Alert_Attributes )
                            .Includes(t => t.tb_OpeningInventories )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Inventory>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Inventory>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_InventoryServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Inventory entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>().Where(w => w.Inventory_ID == (long)id)
                             .Includes(t => t.tb_storagerack )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_location )
                        

                                            .Includes(t => t.tb_Inv_Alert_Attributes )
                                            .Includes(t => t.tb_OpeningInventories )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_Inventory>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



