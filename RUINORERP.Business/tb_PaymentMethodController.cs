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
    /// 付款方式 交易方式，后面扩展有关账期 账龄分析的字段
    /// </summary>
    public partial class tb_PaymentMethodController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_PaymentMethodServices _tb_PaymentMethodServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_PaymentMethodController(ILogger<tb_PaymentMethodController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_PaymentMethodServices tb_PaymentMethodServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_PaymentMethodServices = tb_PaymentMethodServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_PaymentMethod info)
        {

           // tb_PaymentMethodValidator validator = new tb_PaymentMethodValidator();
           tb_PaymentMethodValidator validator = _appContext.GetRequiredService<tb_PaymentMethodValidator>();
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
        public async Task<ReturnResults<tb_PaymentMethod>> SaveOrUpdate(tb_PaymentMethod entity)
        {
            ReturnResults<tb_PaymentMethod> rr = new ReturnResults<tb_PaymentMethod>();
            tb_PaymentMethod Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Paytype_ID > 0)
                {
                    bool rs = await _tb_PaymentMethodServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_PaymentMethod>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_PaymentMethodServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_PaymentMethod>(entity);
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
            tb_PaymentMethod entity = model as tb_PaymentMethod;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Paytype_ID > 0)
                {
                    bool rs = await _tb_PaymentMethodServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_PaymentMethod>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_PaymentMethodServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_PaymentMethod>(entity);
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
            List<T> list = await _tb_PaymentMethodServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_PaymentMethod entity = item as tb_PaymentMethod;
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
            List<T> list = await _tb_PaymentMethodServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_PaymentMethod entity = item as tb_PaymentMethod;
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
            tb_PaymentMethod entity = model as tb_PaymentMethod;
            bool rs = await _tb_PaymentMethodServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_PaymentMethod>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_PaymentMethod> entitys = models as List<tb_PaymentMethod>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_PaymentMethod>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_PaymentMethod>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_PaymentMethodValidator validator = new tb_PaymentMethodValidator();
           tb_PaymentMethodValidator validator = _appContext.GetRequiredService<tb_PaymentMethodValidator>();
            ValidationResult results = validator.Validate(info as tb_PaymentMethod);
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

                tb_PaymentMethod entity = model as tb_PaymentMethod;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Paytype_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_PaymentMethod>(entity as tb_PaymentMethod)
                        .Include(m => m.tb_PurReturnEntries)
                    .Include(m => m.tb_FM_PaymentRecords)
                    .Include(m => m.tb_PurEntryRes)
                    .Include(m => m.tb_AS_RepairOrders)
                    .Include(m => m.tb_PurEntries)
                    .Include(m => m.tb_FM_PriceAdjustments)
                    .Include(m => m.tb_CustomerVendors)
                    .Include(m => m.tb_FM_PreReceivedPayments)
                    .Include(m => m.tb_PayMethodAccountMappers)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_PaymentMethod>(entity as tb_PaymentMethod)
                .Include(m => m.tb_PurReturnEntries)
                .Include(m => m.tb_FM_PaymentRecords)
                .Include(m => m.tb_PurEntryRes)
                .Include(m => m.tb_AS_RepairOrders)
                .Include(m => m.tb_PurEntries)
                .Include(m => m.tb_FM_PriceAdjustments)
                .Include(m => m.tb_CustomerVendors)
                .Include(m => m.tb_FM_PreReceivedPayments)
                .Include(m => m.tb_PayMethodAccountMappers)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Paytype_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PaymentMethod>()
                                .Includes(m => m.tb_PurReturnEntries)
                        .Includes(m => m.tb_FM_PaymentRecords)
                        .Includes(m => m.tb_PurEntryRes)
                        .Includes(m => m.tb_AS_RepairOrders)
                        .Includes(m => m.tb_PurEntries)
                        .Includes(m => m.tb_FM_PriceAdjustments)
                        .Includes(m => m.tb_CustomerVendors)
                        .Includes(m => m.tb_FM_PreReceivedPayments)
                        .Includes(m => m.tb_PayMethodAccountMappers)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_PaymentMethod entity = model as tb_PaymentMethod;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_PaymentMethod>(m => m.Paytype_ID== entity.Paytype_ID)
                                .Include(m => m.tb_PurReturnEntries)
                        .Include(m => m.tb_FM_PaymentRecords)
                        .Include(m => m.tb_PurEntryRes)
                        .Include(m => m.tb_AS_RepairOrders)
                        .Include(m => m.tb_PurEntries)
                        .Include(m => m.tb_FM_PriceAdjustments)
                        .Include(m => m.tb_CustomerVendors)
                        .Include(m => m.tb_FM_PreReceivedPayments)
                        .Include(m => m.tb_PayMethodAccountMappers)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_PaymentMethod AddReEntity(tb_PaymentMethod entity)
        {
            tb_PaymentMethod AddEntity =  _tb_PaymentMethodServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_PaymentMethod>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_PaymentMethod> AddReEntityAsync(tb_PaymentMethod entity)
        {
            tb_PaymentMethod AddEntity = await _tb_PaymentMethodServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_PaymentMethod>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_PaymentMethod entity)
        {
            long id = await _tb_PaymentMethodServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_PaymentMethod>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_PaymentMethod> infos)
        {
            List<long> ids = await _tb_PaymentMethodServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_PaymentMethod>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_PaymentMethod entity)
        {
            bool rs = await _tb_PaymentMethodServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_PaymentMethod>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_PaymentMethod entity)
        {
            bool rs = await _tb_PaymentMethodServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_PaymentMethod>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_PaymentMethodServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_PaymentMethod>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_PaymentMethodServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_PaymentMethod>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_PaymentMethod>> QueryAsync()
        {
            List<tb_PaymentMethod> list = await  _tb_PaymentMethodServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_PaymentMethod>(list);
            return list;
        }
        
        public virtual List<tb_PaymentMethod> Query()
        {
            List<tb_PaymentMethod> list =  _tb_PaymentMethodServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_PaymentMethod>(list);
            return list;
        }
        
        public virtual List<tb_PaymentMethod> Query(string wheresql)
        {
            List<tb_PaymentMethod> list =  _tb_PaymentMethodServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_PaymentMethod>(list);
            return list;
        }
        
        public virtual async Task<List<tb_PaymentMethod>> QueryAsync(string wheresql) 
        {
            List<tb_PaymentMethod> list = await _tb_PaymentMethodServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_PaymentMethod>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_PaymentMethod>> QueryAsync(Expression<Func<tb_PaymentMethod, bool>> exp)
        {
            List<tb_PaymentMethod> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PaymentMethod>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_PaymentMethod>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PaymentMethod>> QueryByNavAsync()
        {
            List<tb_PaymentMethod> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PaymentMethod>()
                                            .Includes(t => t.tb_PurReturnEntries )
                                .Includes(t => t.tb_FM_PaymentRecords )
                                .Includes(t => t.tb_PurEntryRes )
                                .Includes(t => t.tb_AS_RepairOrders )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_FM_PriceAdjustments )
                                .Includes(t => t.tb_CustomerVendors )
                                .Includes(t => t.tb_FM_PreReceivedPayments )
                                .Includes(t => t.tb_PayMethodAccountMappers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_PaymentMethod>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PaymentMethod>> QueryByNavAsync(Expression<Func<tb_PaymentMethod, bool>> exp)
        {
            List<tb_PaymentMethod> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PaymentMethod>().Where(exp)
                                            .Includes(t => t.tb_PurReturnEntries )
                                .Includes(t => t.tb_FM_PaymentRecords )
                                .Includes(t => t.tb_PurEntryRes )
                                .Includes(t => t.tb_AS_RepairOrders )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_FM_PriceAdjustments )
                                .Includes(t => t.tb_CustomerVendors )
                                .Includes(t => t.tb_FM_PreReceivedPayments )
                                .Includes(t => t.tb_PayMethodAccountMappers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_PaymentMethod>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_PaymentMethod> QueryByNav(Expression<Func<tb_PaymentMethod, bool>> exp)
        {
            List<tb_PaymentMethod> list = _unitOfWorkManage.GetDbClient().Queryable<tb_PaymentMethod>().Where(exp)
                                        .Includes(t => t.tb_PurReturnEntries )
                            .Includes(t => t.tb_FM_PaymentRecords )
                            .Includes(t => t.tb_PurEntryRes )
                            .Includes(t => t.tb_AS_RepairOrders )
                            .Includes(t => t.tb_PurEntries )
                            .Includes(t => t.tb_FM_PriceAdjustments )
                            .Includes(t => t.tb_CustomerVendors )
                            .Includes(t => t.tb_FM_PreReceivedPayments )
                            .Includes(t => t.tb_PayMethodAccountMappers )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_PaymentMethod>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_PaymentMethod>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PaymentMethod>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_PaymentMethodServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_PaymentMethod entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PaymentMethod>().Where(w => w.Paytype_ID == (long)id)
                         

                                            .Includes(t => t.tb_PurReturnEntries )
                                            .Includes(t => t.tb_FM_PaymentRecords )
                                            .Includes(t => t.tb_PurEntryRes )
                                            .Includes(t => t.tb_AS_RepairOrders )
                                            .Includes(t => t.tb_PurEntries )
                                            .Includes(t => t.tb_FM_PriceAdjustments )
                                            .Includes(t => t.tb_CustomerVendors )
                                            .Includes(t => t.tb_FM_PreReceivedPayments )
                                            .Includes(t => t.tb_PayMethodAccountMappers )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_PaymentMethod>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



