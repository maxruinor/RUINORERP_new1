// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/07/2025 00:04:31
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
    /// 销售出库退回单
    /// </summary>
    public partial class tb_SaleOutReController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_SaleOutReServices _tb_SaleOutReServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_SaleOutReController(ILogger<tb_SaleOutReController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_SaleOutReServices tb_SaleOutReServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_SaleOutReServices = tb_SaleOutReServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_SaleOutRe info)
        {

           // tb_SaleOutReValidator validator = new tb_SaleOutReValidator();
           tb_SaleOutReValidator validator = _appContext.GetRequiredService<tb_SaleOutReValidator>();
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
        public async Task<ReturnResults<tb_SaleOutRe>> SaveOrUpdate(tb_SaleOutRe entity)
        {
            ReturnResults<tb_SaleOutRe> rr = new ReturnResults<tb_SaleOutRe>();
            tb_SaleOutRe Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SaleOutRe_ID > 0)
                {
                    bool rs = await _tb_SaleOutReServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_SaleOutRe>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_SaleOutReServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_SaleOutRe>(entity);
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
            tb_SaleOutRe entity = model as tb_SaleOutRe;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SaleOutRe_ID > 0)
                {
                    bool rs = await _tb_SaleOutReServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_SaleOutRe>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_SaleOutReServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_SaleOutRe>(entity);
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
            List<T> list = await _tb_SaleOutReServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_SaleOutRe entity = item as tb_SaleOutRe;
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
            List<T> list = await _tb_SaleOutReServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_SaleOutRe entity = item as tb_SaleOutRe;
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
            tb_SaleOutRe entity = model as tb_SaleOutRe;
            bool rs = await _tb_SaleOutReServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_SaleOutRe>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_SaleOutRe> entitys = models as List<tb_SaleOutRe>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_SaleOutRe>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_SaleOutRe>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_SaleOutReValidator validator = new tb_SaleOutReValidator();
           tb_SaleOutReValidator validator = _appContext.GetRequiredService<tb_SaleOutReValidator>();
            ValidationResult results = validator.Validate(info as tb_SaleOutRe);
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

                tb_SaleOutRe entity = model as tb_SaleOutRe;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.SaleOutRe_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_SaleOutRe>(entity as tb_SaleOutRe)
                        .Include(m => m.tb_SaleOutReRefurbishedMaterialsDetails)
                    .Include(m => m.tb_SaleOutReDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_SaleOutRe>(entity as tb_SaleOutRe)
                .Include(m => m.tb_SaleOutReRefurbishedMaterialsDetails)
                .Include(m => m.tb_SaleOutReDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.SaleOutRe_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutRe>()
                                .Includes(m => m.tb_paymentmethod)
                            .Includes(m => m.tb_projectgroup)
                            .Includes(m => m.tb_saleout)
                            .Includes(m => m.tb_employee)
                            .Includes(m => m.tb_customervendor)
                                            .Includes(m => m.tb_SaleOutReRefurbishedMaterialsDetails)
                        .Includes(m => m.tb_SaleOutReDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_SaleOutRe entity = model as tb_SaleOutRe;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_SaleOutRe>(m => m.SaleOutRe_ID== entity.SaleOutRe_ID)
                                .Include(m => m.tb_SaleOutReRefurbishedMaterialsDetails)
                        .Include(m => m.tb_SaleOutReDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_SaleOutRe AddReEntity(tb_SaleOutRe entity)
        {
            tb_SaleOutRe AddEntity =  _tb_SaleOutReServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_SaleOutRe>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_SaleOutRe> AddReEntityAsync(tb_SaleOutRe entity)
        {
            tb_SaleOutRe AddEntity = await _tb_SaleOutReServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_SaleOutRe>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_SaleOutRe entity)
        {
            long id = await _tb_SaleOutReServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_SaleOutRe>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_SaleOutRe> infos)
        {
            List<long> ids = await _tb_SaleOutReServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutRe>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_SaleOutRe entity)
        {
            bool rs = await _tb_SaleOutReServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_SaleOutRe>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_SaleOutRe entity)
        {
            bool rs = await _tb_SaleOutReServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_SaleOutRe>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_SaleOutReServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_SaleOutRe>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_SaleOutReServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_SaleOutRe>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_SaleOutRe>> QueryAsync()
        {
            List<tb_SaleOutRe> list = await  _tb_SaleOutReServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutRe>(list);
            return list;
        }
        
        public virtual List<tb_SaleOutRe> Query()
        {
            List<tb_SaleOutRe> list =  _tb_SaleOutReServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutRe>(list);
            return list;
        }
        
        public virtual List<tb_SaleOutRe> Query(string wheresql)
        {
            List<tb_SaleOutRe> list =  _tb_SaleOutReServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutRe>(list);
            return list;
        }
        
        public virtual async Task<List<tb_SaleOutRe>> QueryAsync(string wheresql) 
        {
            List<tb_SaleOutRe> list = await _tb_SaleOutReServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutRe>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_SaleOutRe>> QueryAsync(Expression<Func<tb_SaleOutRe, bool>> exp)
        {
            List<tb_SaleOutRe> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutRe>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutRe>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_SaleOutRe>> QueryByNavAsync()
        {
            List<tb_SaleOutRe> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutRe>()
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_saleout )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_SaleOutReRefurbishedMaterialsDetails )
                                .Includes(t => t.tb_SaleOutReDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutRe>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_SaleOutRe>> QueryByNavAsync(Expression<Func<tb_SaleOutRe, bool>> exp)
        {
            List<tb_SaleOutRe> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutRe>().Where(exp)
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_saleout )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_SaleOutReRefurbishedMaterialsDetails )
                                .Includes(t => t.tb_SaleOutReDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutRe>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_SaleOutRe> QueryByNav(Expression<Func<tb_SaleOutRe, bool>> exp)
        {
            List<tb_SaleOutRe> list = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutRe>().Where(exp)
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_saleout )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_SaleOutReRefurbishedMaterialsDetails )
                            .Includes(t => t.tb_SaleOutReDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutRe>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_SaleOutRe>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutRe>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_SaleOutReServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_SaleOutRe entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutRe>().Where(w => w.SaleOutRe_ID == (long)id)
                             .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_saleout )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendor )
                        

                                            .Includes(t => t.tb_SaleOutReRefurbishedMaterialsDetails )
                                            .Includes(t => t.tb_SaleOutReDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_SaleOutRe>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



