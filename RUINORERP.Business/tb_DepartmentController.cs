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
    /// 部门表是否分层
    /// </summary>
    public partial class tb_DepartmentController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_DepartmentServices _tb_DepartmentServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_DepartmentController(ILogger<tb_DepartmentController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_DepartmentServices tb_DepartmentServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_DepartmentServices = tb_DepartmentServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_Department info)
        {

           // tb_DepartmentValidator validator = new tb_DepartmentValidator();
           tb_DepartmentValidator validator = _appContext.GetRequiredService<tb_DepartmentValidator>();
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
        public async Task<ReturnResults<tb_Department>> SaveOrUpdate(tb_Department entity)
        {
            ReturnResults<tb_Department> rr = new ReturnResults<tb_Department>();
            tb_Department Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.DepartmentID > 0)
                {
                    bool rs = await _tb_DepartmentServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Department>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_DepartmentServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_Department>(entity);
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
            tb_Department entity = model as tb_Department;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.DepartmentID > 0)
                {
                    bool rs = await _tb_DepartmentServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Department>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_DepartmentServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_Department>(entity);
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
            List<T> list = await _tb_DepartmentServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Department entity = item as tb_Department;
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
            List<T> list = await _tb_DepartmentServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Department entity = item as tb_Department;
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
            tb_Department entity = model as tb_Department;
            bool rs = await _tb_DepartmentServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_Department>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Department> entitys = models as List<tb_Department>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Department>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_Department>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_DepartmentValidator validator = new tb_DepartmentValidator();
           tb_DepartmentValidator validator = _appContext.GetRequiredService<tb_DepartmentValidator>();
            ValidationResult results = validator.Validate(info as tb_Department);
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

                tb_Department entity = model as tb_Department;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.DepartmentID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Department>(entity as tb_Department)
                        .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_BOM_Ss)
                    .Include(m => m.tb_FM_PaymentRecordDetails)
                    .Include(m => m.tb_FM_OtherExpenseDetails)
                    .Include(m => m.tb_Prods)
                    .Include(m => m.tb_ProjectGroups)
                    .Include(m => m.tb_PurReturnEntries)
                    .Include(m => m.tb_MaterialReturns)
                    .Include(m => m.tb_ProdBorrowings)
                    .Include(m => m.tb_BuyingRequisitions)
                    .Include(m => m.tb_FM_Accounts)
                    .Include(m => m.tb_FM_PaymentApplications)
                    .Include(m => m.tb_PurEntryRes)
                    .Include(m => m.tb_ProductionPlans)
                    .Include(m => m.tb_Employees)
                    .Include(m => m.tb_FM_ExpenseClaimDetails)
                    .Include(m => m.tb_MRP_ReworkEntries)
                    .Include(m => m.tb_MRP_ReworkReturns)
                    .Include(m => m.tb_FM_ProfitLosses)
                    .Include(m => m.tb_PurEntries)
                    .Include(m => m.tb_CRM_Customers)
                    .Include(m => m.tb_FM_PriceAdjustments)
                    .Include(m => m.tb_FM_PreReceivedPayments)
                    .Include(m => m.tb_FinishedGoodsInvs)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Department>(entity as tb_Department)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_BOM_Ss)
                .Include(m => m.tb_FM_PaymentRecordDetails)
                .Include(m => m.tb_FM_OtherExpenseDetails)
                .Include(m => m.tb_Prods)
                .Include(m => m.tb_ProjectGroups)
                .Include(m => m.tb_PurReturnEntries)
                .Include(m => m.tb_MaterialReturns)
                .Include(m => m.tb_ProdBorrowings)
                .Include(m => m.tb_BuyingRequisitions)
                .Include(m => m.tb_FM_Accounts)
                .Include(m => m.tb_FM_PaymentApplications)
                .Include(m => m.tb_PurEntryRes)
                .Include(m => m.tb_ProductionPlans)
                .Include(m => m.tb_Employees)
                .Include(m => m.tb_FM_ExpenseClaimDetails)
                .Include(m => m.tb_MRP_ReworkEntries)
                .Include(m => m.tb_MRP_ReworkReturns)
                .Include(m => m.tb_FM_ProfitLosses)
                .Include(m => m.tb_PurEntries)
                .Include(m => m.tb_CRM_Customers)
                .Include(m => m.tb_FM_PriceAdjustments)
                .Include(m => m.tb_FM_PreReceivedPayments)
                .Include(m => m.tb_FinishedGoodsInvs)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.DepartmentID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Department>()
                                .Includes(m => m.tb_ManufacturingOrders)
                        .Includes(m => m.tb_BOM_Ss)
                        .Includes(m => m.tb_FM_PaymentRecordDetails)
                        .Includes(m => m.tb_FM_OtherExpenseDetails)
                        .Includes(m => m.tb_Prods)
                        .Includes(m => m.tb_ProjectGroups)
                        .Includes(m => m.tb_PurReturnEntries)
                        .Includes(m => m.tb_MaterialReturns)
                        .Includes(m => m.tb_ProdBorrowings)
                        .Includes(m => m.tb_BuyingRequisitions)
                        .Includes(m => m.tb_FM_Accounts)
                        .Includes(m => m.tb_FM_PaymentApplications)
                        .Includes(m => m.tb_PurEntryRes)
                        .Includes(m => m.tb_ProductionPlans)
                        .Includes(m => m.tb_Employees)
                        .Includes(m => m.tb_FM_ExpenseClaimDetails)
                        .Includes(m => m.tb_MRP_ReworkEntries)
                        .Includes(m => m.tb_MRP_ReworkReturns)
                        .Includes(m => m.tb_FM_ProfitLosses)
                        .Includes(m => m.tb_PurEntries)
                        .Includes(m => m.tb_CRM_Customers)
                        .Includes(m => m.tb_FM_PriceAdjustments)
                        .Includes(m => m.tb_FM_PreReceivedPayments)
                        .Includes(m => m.tb_FinishedGoodsInvs)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Department entity = model as tb_Department;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Department>(m => m.DepartmentID== entity.DepartmentID)
                                .Include(m => m.tb_ManufacturingOrders)
                        .Include(m => m.tb_BOM_Ss)
                        .Include(m => m.tb_FM_PaymentRecordDetails)
                        .Include(m => m.tb_FM_OtherExpenseDetails)
                        .Include(m => m.tb_Prods)
                        .Include(m => m.tb_ProjectGroups)
                        .Include(m => m.tb_PurReturnEntries)
                        .Include(m => m.tb_MaterialReturns)
                        .Include(m => m.tb_ProdBorrowings)
                        .Include(m => m.tb_BuyingRequisitions)
                        .Include(m => m.tb_FM_Accounts)
                        .Include(m => m.tb_FM_PaymentApplications)
                        .Include(m => m.tb_PurEntryRes)
                        .Include(m => m.tb_ProductionPlans)
                        .Include(m => m.tb_Employees)
                        .Include(m => m.tb_FM_ExpenseClaimDetails)
                        .Include(m => m.tb_MRP_ReworkEntries)
                        .Include(m => m.tb_MRP_ReworkReturns)
                        .Include(m => m.tb_FM_ProfitLosses)
                        .Include(m => m.tb_PurEntries)
                        .Include(m => m.tb_CRM_Customers)
                        .Include(m => m.tb_FM_PriceAdjustments)
                        .Include(m => m.tb_FM_PreReceivedPayments)
                        .Include(m => m.tb_FinishedGoodsInvs)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Department AddReEntity(tb_Department entity)
        {
            tb_Department AddEntity =  _tb_DepartmentServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_Department>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Department> AddReEntityAsync(tb_Department entity)
        {
            tb_Department AddEntity = await _tb_DepartmentServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_Department>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Department entity)
        {
            long id = await _tb_DepartmentServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_Department>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Department> infos)
        {
            List<long> ids = await _tb_DepartmentServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_Department>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Department entity)
        {
            bool rs = await _tb_DepartmentServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_Department>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Department entity)
        {
            bool rs = await _tb_DepartmentServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_Department>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_DepartmentServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_Department>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_DepartmentServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_Department>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Department>> QueryAsync()
        {
            List<tb_Department> list = await  _tb_DepartmentServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Department>(list);
            return list;
        }
        
        public virtual List<tb_Department> Query()
        {
            List<tb_Department> list =  _tb_DepartmentServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_Department>(list);
            return list;
        }
        
        public virtual List<tb_Department> Query(string wheresql)
        {
            List<tb_Department> list =  _tb_DepartmentServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Department>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Department>> QueryAsync(string wheresql) 
        {
            List<tb_Department> list = await _tb_DepartmentServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Department>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Department>> QueryAsync(Expression<Func<tb_Department, bool>> exp)
        {
            List<tb_Department> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Department>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_Department>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Department>> QueryByNavAsync()
        {
            List<tb_Department> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Department>()
                               .Includes(t => t.tb_company )
                                            .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_BOM_Ss )
                                .Includes(t => t.tb_FM_PaymentRecordDetails )
                                .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_ProjectGroups )
                                .Includes(t => t.tb_PurReturnEntries )
                                .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_ProdBorrowings )
                                .Includes(t => t.tb_BuyingRequisitions )
                                .Includes(t => t.tb_FM_Accounts )
                                .Includes(t => t.tb_FM_PaymentApplications )
                                .Includes(t => t.tb_PurEntryRes )
                                .Includes(t => t.tb_ProductionPlans )
                                .Includes(t => t.tb_Employees )
                                .Includes(t => t.tb_FM_ExpenseClaimDetails )
                                .Includes(t => t.tb_MRP_ReworkEntries )
                                .Includes(t => t.tb_MRP_ReworkReturns )
                                .Includes(t => t.tb_FM_ProfitLosses )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_CRM_Customers )
                                .Includes(t => t.tb_FM_PriceAdjustments )
                                .Includes(t => t.tb_FM_PreReceivedPayments )
                                .Includes(t => t.tb_FinishedGoodsInvs )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Department>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Department>> QueryByNavAsync(Expression<Func<tb_Department, bool>> exp)
        {
            List<tb_Department> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Department>().Where(exp)
                               .Includes(t => t.tb_company )
                                            .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_BOM_Ss )
                                .Includes(t => t.tb_FM_PaymentRecordDetails )
                                .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_ProjectGroups )
                                .Includes(t => t.tb_PurReturnEntries )
                                .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_ProdBorrowings )
                                .Includes(t => t.tb_BuyingRequisitions )
                                .Includes(t => t.tb_FM_Accounts )
                                .Includes(t => t.tb_FM_PaymentApplications )
                                .Includes(t => t.tb_PurEntryRes )
                                .Includes(t => t.tb_ProductionPlans )
                                .Includes(t => t.tb_Employees )
                                .Includes(t => t.tb_FM_ExpenseClaimDetails )
                                .Includes(t => t.tb_MRP_ReworkEntries )
                                .Includes(t => t.tb_MRP_ReworkReturns )
                                .Includes(t => t.tb_FM_ProfitLosses )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_CRM_Customers )
                                .Includes(t => t.tb_FM_PriceAdjustments )
                                .Includes(t => t.tb_FM_PreReceivedPayments )
                                .Includes(t => t.tb_FinishedGoodsInvs )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Department>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Department> QueryByNav(Expression<Func<tb_Department, bool>> exp)
        {
            List<tb_Department> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Department>().Where(exp)
                            .Includes(t => t.tb_company )
                                        .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_BOM_Ss )
                            .Includes(t => t.tb_FM_PaymentRecordDetails )
                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_ProjectGroups )
                            .Includes(t => t.tb_PurReturnEntries )
                            .Includes(t => t.tb_MaterialReturns )
                            .Includes(t => t.tb_ProdBorrowings )
                            .Includes(t => t.tb_BuyingRequisitions )
                            .Includes(t => t.tb_FM_Accounts )
                            .Includes(t => t.tb_FM_PaymentApplications )
                            .Includes(t => t.tb_PurEntryRes )
                            .Includes(t => t.tb_ProductionPlans )
                            .Includes(t => t.tb_Employees )
                            .Includes(t => t.tb_FM_ExpenseClaimDetails )
                            .Includes(t => t.tb_MRP_ReworkEntries )
                            .Includes(t => t.tb_MRP_ReworkReturns )
                            .Includes(t => t.tb_FM_ProfitLosses )
                            .Includes(t => t.tb_PurEntries )
                            .Includes(t => t.tb_CRM_Customers )
                            .Includes(t => t.tb_FM_PriceAdjustments )
                            .Includes(t => t.tb_FM_PreReceivedPayments )
                            .Includes(t => t.tb_FinishedGoodsInvs )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Department>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Department>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Department>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_DepartmentServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Department entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Department>().Where(w => w.DepartmentID == (long)id)
                             .Includes(t => t.tb_company )
                        

                                            .Includes(t => t.tb_ManufacturingOrders )
                                            .Includes(t => t.tb_BOM_Ss )
                                            .Includes(t => t.tb_FM_PaymentRecordDetails )
                                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                                            .Includes(t => t.tb_Prods )
                                            .Includes(t => t.tb_ProjectGroups )
                                            .Includes(t => t.tb_PurReturnEntries )
                                            .Includes(t => t.tb_MaterialReturns )
                                            .Includes(t => t.tb_ProdBorrowings )
                                            .Includes(t => t.tb_BuyingRequisitions )
                                            .Includes(t => t.tb_FM_Accounts )
                                            .Includes(t => t.tb_FM_PaymentApplications )
                                            .Includes(t => t.tb_PurEntryRes )
                                            .Includes(t => t.tb_ProductionPlans )
                                            .Includes(t => t.tb_Employees )
                                            .Includes(t => t.tb_FM_ExpenseClaimDetails )
                                            .Includes(t => t.tb_MRP_ReworkEntries )
                                            .Includes(t => t.tb_MRP_ReworkReturns )
                                            .Includes(t => t.tb_FM_ProfitLosses )
                                            .Includes(t => t.tb_PurEntries )
                                            .Includes(t => t.tb_CRM_Customers )
                                            .Includes(t => t.tb_FM_PriceAdjustments )
                                            .Includes(t => t.tb_FM_PreReceivedPayments )
                                            .Includes(t => t.tb_FinishedGoodsInvs )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_Department>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



