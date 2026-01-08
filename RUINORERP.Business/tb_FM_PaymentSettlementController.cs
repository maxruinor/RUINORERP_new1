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
    /// 记录收款 与应收的匹配，核销表 核销记录用于跟踪资金与债权债务的冲抵关系，确保财务数据可追溯。正常的收款，支付不需要保存核销记录
    /// </summary>
    public partial class tb_FM_PaymentSettlementController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_PaymentSettlementServices _tb_FM_PaymentSettlementServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_PaymentSettlementController(ILogger<tb_FM_PaymentSettlementController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_PaymentSettlementServices tb_FM_PaymentSettlementServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_PaymentSettlementServices = tb_FM_PaymentSettlementServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_FM_PaymentSettlement info)
        {

           // tb_FM_PaymentSettlementValidator validator = new tb_FM_PaymentSettlementValidator();
           tb_FM_PaymentSettlementValidator validator = _appContext.GetRequiredService<tb_FM_PaymentSettlementValidator>();
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
        public async Task<ReturnResults<tb_FM_PaymentSettlement>> SaveOrUpdate(tb_FM_PaymentSettlement entity)
        {
            ReturnResults<tb_FM_PaymentSettlement> rr = new ReturnResults<tb_FM_PaymentSettlement>();
            tb_FM_PaymentSettlement Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SettlementId > 0)
                {
                    bool rs = await _tb_FM_PaymentSettlementServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_FM_PaymentSettlement>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_PaymentSettlementServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_FM_PaymentSettlement>(entity);
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
            tb_FM_PaymentSettlement entity = model as tb_FM_PaymentSettlement;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SettlementId > 0)
                {
                    bool rs = await _tb_FM_PaymentSettlementServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_FM_PaymentSettlement>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_PaymentSettlementServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_FM_PaymentSettlement>(entity);
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
            List<T> list = await _tb_FM_PaymentSettlementServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_PaymentSettlement entity = item as tb_FM_PaymentSettlement;
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
            List<T> list = await _tb_FM_PaymentSettlementServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_PaymentSettlement entity = item as tb_FM_PaymentSettlement;
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
            tb_FM_PaymentSettlement entity = model as tb_FM_PaymentSettlement;
            bool rs = await _tb_FM_PaymentSettlementServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_FM_PaymentSettlement>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_PaymentSettlement> entitys = models as List<tb_FM_PaymentSettlement>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PaymentSettlement>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_FM_PaymentSettlement>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_PaymentSettlementValidator validator = new tb_FM_PaymentSettlementValidator();
           tb_FM_PaymentSettlementValidator validator = _appContext.GetRequiredService<tb_FM_PaymentSettlementValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_PaymentSettlement);
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

                tb_FM_PaymentSettlement entity = model as tb_FM_PaymentSettlement;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.SettlementId > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_PaymentSettlement>(entity as tb_FM_PaymentSettlement)
                        .Include(m => m.tb_FM_PaymentSettlements)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_PaymentSettlement>(entity as tb_FM_PaymentSettlement)
                .Include(m => m.tb_FM_PaymentSettlements)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.SettlementId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentSettlement>()
                                .Includes(m => m.tb_FM_PaymentSettlements)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_PaymentSettlement entity = model as tb_FM_PaymentSettlement;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_PaymentSettlement>(m => m.SettlementId== entity.SettlementId)
                                .Include(m => m.tb_FM_PaymentSettlements)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_PaymentSettlement AddReEntity(tb_FM_PaymentSettlement entity)
        {
            tb_FM_PaymentSettlement AddEntity =  _tb_FM_PaymentSettlementServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_FM_PaymentSettlement>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_PaymentSettlement> AddReEntityAsync(tb_FM_PaymentSettlement entity)
        {
            tb_FM_PaymentSettlement AddEntity = await _tb_FM_PaymentSettlementServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_FM_PaymentSettlement>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_PaymentSettlement entity)
        {
            long id = await _tb_FM_PaymentSettlementServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_FM_PaymentSettlement>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_PaymentSettlement> infos)
        {
            List<long> ids = await _tb_FM_PaymentSettlementServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_FM_PaymentSettlement>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_PaymentSettlement entity)
        {
            bool rs = await _tb_FM_PaymentSettlementServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_FM_PaymentSettlement>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_PaymentSettlement entity)
        {
            bool rs = await _tb_FM_PaymentSettlementServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_FM_PaymentSettlement>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_PaymentSettlementServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_FM_PaymentSettlement>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_PaymentSettlementServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_FM_PaymentSettlement>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_PaymentSettlement>> QueryAsync()
        {
            List<tb_FM_PaymentSettlement> list = await  _tb_FM_PaymentSettlementServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PaymentSettlement>(list);
            return list;
        }
        
        public virtual List<tb_FM_PaymentSettlement> Query()
        {
            List<tb_FM_PaymentSettlement> list =  _tb_FM_PaymentSettlementServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PaymentSettlement>(list);
            return list;
        }
        
        public virtual List<tb_FM_PaymentSettlement> Query(string wheresql)
        {
            List<tb_FM_PaymentSettlement> list =  _tb_FM_PaymentSettlementServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PaymentSettlement>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_PaymentSettlement>> QueryAsync(string wheresql) 
        {
            List<tb_FM_PaymentSettlement> list = await _tb_FM_PaymentSettlementServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PaymentSettlement>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PaymentSettlement>> QueryAsync(Expression<Func<tb_FM_PaymentSettlement, bool>> exp)
        {
            List<tb_FM_PaymentSettlement> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentSettlement>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PaymentSettlement>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PaymentSettlement>> QueryByNavAsync()
        {
            List<tb_FM_PaymentSettlement> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentSettlement>()
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_paymentsettlement )
                                            .Includes(t => t.tb_FM_PaymentSettlements )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PaymentSettlement>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PaymentSettlement>> QueryByNavAsync(Expression<Func<tb_FM_PaymentSettlement, bool>> exp)
        {
            List<tb_FM_PaymentSettlement> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentSettlement>().Where(exp)
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_paymentsettlement )
                                            .Includes(t => t.tb_FM_PaymentSettlements )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PaymentSettlement>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_PaymentSettlement> QueryByNav(Expression<Func<tb_FM_PaymentSettlement, bool>> exp)
        {
            List<tb_FM_PaymentSettlement> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentSettlement>().Where(exp)
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_paymentsettlement )
                                        .Includes(t => t.tb_FM_PaymentSettlements )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_FM_PaymentSettlement>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_PaymentSettlement>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentSettlement>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_PaymentSettlementServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_PaymentSettlement entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentSettlement>().Where(w => w.SettlementId == (long)id)
                             .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_paymentsettlement )
                        

                                            .Includes(t => t.tb_FM_PaymentSettlements )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_FM_PaymentSettlement>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



