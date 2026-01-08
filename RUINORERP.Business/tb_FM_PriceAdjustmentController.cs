// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:10
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
    /// 价格调整单
    /// </summary>
    public partial class tb_FM_PriceAdjustmentController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_PriceAdjustmentServices _tb_FM_PriceAdjustmentServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_PriceAdjustmentController(ILogger<tb_FM_PriceAdjustmentController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_PriceAdjustmentServices tb_FM_PriceAdjustmentServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_PriceAdjustmentServices = tb_FM_PriceAdjustmentServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_FM_PriceAdjustment info)
        {

           // tb_FM_PriceAdjustmentValidator validator = new tb_FM_PriceAdjustmentValidator();
           tb_FM_PriceAdjustmentValidator validator = _appContext.GetRequiredService<tb_FM_PriceAdjustmentValidator>();
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
        public async Task<ReturnResults<tb_FM_PriceAdjustment>> SaveOrUpdate(tb_FM_PriceAdjustment entity)
        {
            ReturnResults<tb_FM_PriceAdjustment> rr = new ReturnResults<tb_FM_PriceAdjustment>();
            tb_FM_PriceAdjustment Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.AdjustId > 0)
                {
                    bool rs = await _tb_FM_PriceAdjustmentServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_FM_PriceAdjustment>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_PriceAdjustmentServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_FM_PriceAdjustment>(entity);
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
            tb_FM_PriceAdjustment entity = model as tb_FM_PriceAdjustment;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.AdjustId > 0)
                {
                    bool rs = await _tb_FM_PriceAdjustmentServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_FM_PriceAdjustment>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_PriceAdjustmentServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_FM_PriceAdjustment>(entity);
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
            List<T> list = await _tb_FM_PriceAdjustmentServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_PriceAdjustment entity = item as tb_FM_PriceAdjustment;
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
            List<T> list = await _tb_FM_PriceAdjustmentServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_PriceAdjustment entity = item as tb_FM_PriceAdjustment;
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
            tb_FM_PriceAdjustment entity = model as tb_FM_PriceAdjustment;
            bool rs = await _tb_FM_PriceAdjustmentServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_FM_PriceAdjustment>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_PriceAdjustment> entitys = models as List<tb_FM_PriceAdjustment>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PriceAdjustment>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_FM_PriceAdjustment>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_PriceAdjustmentValidator validator = new tb_FM_PriceAdjustmentValidator();
           tb_FM_PriceAdjustmentValidator validator = _appContext.GetRequiredService<tb_FM_PriceAdjustmentValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_PriceAdjustment);
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

                tb_FM_PriceAdjustment entity = model as tb_FM_PriceAdjustment;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.AdjustId > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_PriceAdjustment>(entity as tb_FM_PriceAdjustment)
                        .Include(m => m.tb_FM_PriceAdjustmentDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_PriceAdjustment>(entity as tb_FM_PriceAdjustment)
                .Include(m => m.tb_FM_PriceAdjustmentDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.AdjustId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PriceAdjustment>()
                                .Includes(m => m.tb_FM_PriceAdjustmentDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_PriceAdjustment entity = model as tb_FM_PriceAdjustment;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_PriceAdjustment>(m => m.AdjustId== entity.AdjustId)
                                .Include(m => m.tb_FM_PriceAdjustmentDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_PriceAdjustment AddReEntity(tb_FM_PriceAdjustment entity)
        {
            tb_FM_PriceAdjustment AddEntity =  _tb_FM_PriceAdjustmentServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_FM_PriceAdjustment>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_PriceAdjustment> AddReEntityAsync(tb_FM_PriceAdjustment entity)
        {
            tb_FM_PriceAdjustment AddEntity = await _tb_FM_PriceAdjustmentServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_FM_PriceAdjustment>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_PriceAdjustment entity)
        {
            long id = await _tb_FM_PriceAdjustmentServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_FM_PriceAdjustment>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_PriceAdjustment> infos)
        {
            List<long> ids = await _tb_FM_PriceAdjustmentServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_FM_PriceAdjustment>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_PriceAdjustment entity)
        {
            bool rs = await _tb_FM_PriceAdjustmentServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_FM_PriceAdjustment>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_PriceAdjustment entity)
        {
            bool rs = await _tb_FM_PriceAdjustmentServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_FM_PriceAdjustment>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_PriceAdjustmentServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_FM_PriceAdjustment>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_PriceAdjustmentServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_FM_PriceAdjustment>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_PriceAdjustment>> QueryAsync()
        {
            List<tb_FM_PriceAdjustment> list = await  _tb_FM_PriceAdjustmentServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PriceAdjustment>(list);
            return list;
        }
        
        public virtual List<tb_FM_PriceAdjustment> Query()
        {
            List<tb_FM_PriceAdjustment> list =  _tb_FM_PriceAdjustmentServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PriceAdjustment>(list);
            return list;
        }
        
        public virtual List<tb_FM_PriceAdjustment> Query(string wheresql)
        {
            List<tb_FM_PriceAdjustment> list =  _tb_FM_PriceAdjustmentServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PriceAdjustment>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_PriceAdjustment>> QueryAsync(string wheresql) 
        {
            List<tb_FM_PriceAdjustment> list = await _tb_FM_PriceAdjustmentServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PriceAdjustment>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PriceAdjustment>> QueryAsync(Expression<Func<tb_FM_PriceAdjustment, bool>> exp)
        {
            List<tb_FM_PriceAdjustment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PriceAdjustment>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PriceAdjustment>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PriceAdjustment>> QueryByNavAsync()
        {
            List<tb_FM_PriceAdjustment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PriceAdjustment>()
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                                            .Includes(t => t.tb_FM_PriceAdjustmentDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PriceAdjustment>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PriceAdjustment>> QueryByNavAsync(Expression<Func<tb_FM_PriceAdjustment, bool>> exp)
        {
            List<tb_FM_PriceAdjustment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PriceAdjustment>().Where(exp)
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                                            .Includes(t => t.tb_FM_PriceAdjustmentDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PriceAdjustment>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_PriceAdjustment> QueryByNav(Expression<Func<tb_FM_PriceAdjustment, bool>> exp)
        {
            List<tb_FM_PriceAdjustment> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PriceAdjustment>().Where(exp)
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                                        .Includes(t => t.tb_FM_PriceAdjustmentDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PriceAdjustment>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_PriceAdjustment>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PriceAdjustment>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_PriceAdjustmentServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_PriceAdjustment entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PriceAdjustment>().Where(w => w.AdjustId == (long)id)
                             .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                        

                                            .Includes(t => t.tb_FM_PriceAdjustmentDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_FM_PriceAdjustment>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



