// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/07/2025 10:19:26
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
    /// 币别换算表
    /// </summary>
    public partial class tb_CurrencyExchangeRateController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CurrencyExchangeRateServices _tb_CurrencyExchangeRateServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_CurrencyExchangeRateController(ILogger<tb_CurrencyExchangeRateController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CurrencyExchangeRateServices tb_CurrencyExchangeRateServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CurrencyExchangeRateServices = tb_CurrencyExchangeRateServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_CurrencyExchangeRate info)
        {

           // tb_CurrencyExchangeRateValidator validator = new tb_CurrencyExchangeRateValidator();
           tb_CurrencyExchangeRateValidator validator = _appContext.GetRequiredService<tb_CurrencyExchangeRateValidator>();
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
        public async Task<ReturnResults<tb_CurrencyExchangeRate>> SaveOrUpdate(tb_CurrencyExchangeRate entity)
        {
            ReturnResults<tb_CurrencyExchangeRate> rr = new ReturnResults<tb_CurrencyExchangeRate>();
            tb_CurrencyExchangeRate Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ExchangeRateID > 0)
                {
                    bool rs = await _tb_CurrencyExchangeRateServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CurrencyExchangeRate>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CurrencyExchangeRateServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_CurrencyExchangeRate>(entity);
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
            tb_CurrencyExchangeRate entity = model as tb_CurrencyExchangeRate;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ExchangeRateID > 0)
                {
                    bool rs = await _tb_CurrencyExchangeRateServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CurrencyExchangeRate>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CurrencyExchangeRateServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_CurrencyExchangeRate>(entity);
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
            List<T> list = await _tb_CurrencyExchangeRateServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_CurrencyExchangeRate entity = item as tb_CurrencyExchangeRate;
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
            List<T> list = await _tb_CurrencyExchangeRateServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_CurrencyExchangeRate entity = item as tb_CurrencyExchangeRate;
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
            tb_CurrencyExchangeRate entity = model as tb_CurrencyExchangeRate;
            bool rs = await _tb_CurrencyExchangeRateServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_CurrencyExchangeRate>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_CurrencyExchangeRate> entitys = models as List<tb_CurrencyExchangeRate>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_CurrencyExchangeRate>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_CurrencyExchangeRate>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_CurrencyExchangeRateValidator validator = new tb_CurrencyExchangeRateValidator();
           tb_CurrencyExchangeRateValidator validator = _appContext.GetRequiredService<tb_CurrencyExchangeRateValidator>();
            ValidationResult results = validator.Validate(info as tb_CurrencyExchangeRate);
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

                tb_CurrencyExchangeRate entity = model as tb_CurrencyExchangeRate;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ExchangeRateID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_CurrencyExchangeRate>(entity as tb_CurrencyExchangeRate)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_CurrencyExchangeRate>(entity as tb_CurrencyExchangeRate)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ExchangeRateID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CurrencyExchangeRate>()
                                .Includes(m => m.tb_currency)
                            .Includes(m => m.tb_currencyByTargetCurrency)
                                            //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CurrencyExchangeRate entity = model as tb_CurrencyExchangeRate;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CurrencyExchangeRate>(m => m.ExchangeRateID== entity.ExchangeRateID)
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
        
        
        
        public tb_CurrencyExchangeRate AddReEntity(tb_CurrencyExchangeRate entity)
        {
            tb_CurrencyExchangeRate AddEntity =  _tb_CurrencyExchangeRateServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_CurrencyExchangeRate>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CurrencyExchangeRate> AddReEntityAsync(tb_CurrencyExchangeRate entity)
        {
            tb_CurrencyExchangeRate AddEntity = await _tb_CurrencyExchangeRateServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_CurrencyExchangeRate>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CurrencyExchangeRate entity)
        {
            long id = await _tb_CurrencyExchangeRateServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_CurrencyExchangeRate>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CurrencyExchangeRate> infos)
        {
            List<long> ids = await _tb_CurrencyExchangeRateServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_CurrencyExchangeRate>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CurrencyExchangeRate entity)
        {
            bool rs = await _tb_CurrencyExchangeRateServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_CurrencyExchangeRate>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CurrencyExchangeRate entity)
        {
            bool rs = await _tb_CurrencyExchangeRateServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_CurrencyExchangeRate>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CurrencyExchangeRateServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_CurrencyExchangeRate>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CurrencyExchangeRateServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_CurrencyExchangeRate>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_CurrencyExchangeRate>> QueryAsync()
        {
            List<tb_CurrencyExchangeRate> list = await  _tb_CurrencyExchangeRateServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CurrencyExchangeRate>(list);
            return list;
        }
        
        public virtual List<tb_CurrencyExchangeRate> Query()
        {
            List<tb_CurrencyExchangeRate> list =  _tb_CurrencyExchangeRateServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_CurrencyExchangeRate>(list);
            return list;
        }
        
        public virtual List<tb_CurrencyExchangeRate> Query(string wheresql)
        {
            List<tb_CurrencyExchangeRate> list =  _tb_CurrencyExchangeRateServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CurrencyExchangeRate>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CurrencyExchangeRate>> QueryAsync(string wheresql) 
        {
            List<tb_CurrencyExchangeRate> list = await _tb_CurrencyExchangeRateServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CurrencyExchangeRate>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_CurrencyExchangeRate>> QueryAsync(Expression<Func<tb_CurrencyExchangeRate, bool>> exp)
        {
            List<tb_CurrencyExchangeRate> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CurrencyExchangeRate>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_CurrencyExchangeRate>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CurrencyExchangeRate>> QueryByNavAsync()
        {
            List<tb_CurrencyExchangeRate> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CurrencyExchangeRate>()
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_currencyByTargetCurrency )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CurrencyExchangeRate>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CurrencyExchangeRate>> QueryByNavAsync(Expression<Func<tb_CurrencyExchangeRate, bool>> exp)
        {
            List<tb_CurrencyExchangeRate> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CurrencyExchangeRate>().Where(exp)
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_currencyByTargetCurrency )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CurrencyExchangeRate>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CurrencyExchangeRate> QueryByNav(Expression<Func<tb_CurrencyExchangeRate, bool>> exp)
        {
            List<tb_CurrencyExchangeRate> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CurrencyExchangeRate>().Where(exp)
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_currencyByTargetCurrency )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CurrencyExchangeRate>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CurrencyExchangeRate>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CurrencyExchangeRate>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CurrencyExchangeRateServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_CurrencyExchangeRate entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_CurrencyExchangeRate>().Where(w => w.ExchangeRateID == (long)id)
                             .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_currencyByTargetCurrency )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_CurrencyExchangeRate>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



