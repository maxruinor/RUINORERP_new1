
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:05:52
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

namespace RUINORERP.Business
{
    /// <summary>
    /// 员工表
    /// </summary>
    public partial class tb_EmployeeController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_EmployeeServices _tb_EmployeeServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_EmployeeController(ILogger<tb_EmployeeController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_EmployeeServices tb_EmployeeServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_EmployeeServices = tb_EmployeeServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_Employee info)
        {

           // tb_EmployeeValidator validator = new tb_EmployeeValidator();
           tb_EmployeeValidator validator = _appContext.GetRequiredService<tb_EmployeeValidator>();
            ValidationResult results = validator.Validate(info);
            return results;
        }
        
        #region 扩展方法
        
        /// <summary>
        /// 某字段是否存在
        /// </summary>
        /// <param name="exp">e => e.ModeuleName == mod.ModeuleName</param>
        /// <returns></returns>
        public override bool ExistFieldValue(Expression<Func<T, bool>> exp)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp).Any();
        }
      
        
        /// <summary>
        /// 雪花ID模式下的新增和修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ReturnResults<tb_Employee>> SaveOrUpdate(tb_Employee entity)
        {
            ReturnResults<tb_Employee> rr = new ReturnResults<tb_Employee>();
            tb_Employee Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Employee_ID > 0)
                {
                    bool rs = await _tb_EmployeeServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Employee>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_EmployeeServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Employee>(entity);
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
            tb_Employee entity = model as tb_Employee;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Employee_ID > 0)
                {
                    bool rs = await _tb_EmployeeServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Employee>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_EmployeeServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Employee>(entity);
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
            List<T> list = await _tb_EmployeeServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Employee entity = item as tb_Employee;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                MyCacheManager.Instance.UpdateEntityList<List<T>>(list);
             }
            return list;
        }
        
        public async override Task<List<T>> BaseQueryAsync() 
        {
            List<T> list = await _tb_EmployeeServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Employee entity = item as tb_Employee;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                MyCacheManager.Instance.UpdateEntityList<List<T>>(list);
             }
            return list;
        }
        
        
        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_Employee entity = model as tb_Employee;
            bool rs = await _tb_EmployeeServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Employee>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Employee> entitys = models as List<tb_Employee>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Employee>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Employee_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Employee>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_EmployeeValidator validator = new tb_EmployeeValidator();
           tb_EmployeeValidator validator = _appContext.GetRequiredService<tb_EmployeeValidator>();
            ValidationResult results = validator.Validate(info as tb_Employee);
            return results;
        }
        
        
        public async override Task<List<T>> BaseQueryByAdvancedAsync(bool useLike,object dto) 
        {
            var  querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }
        
        public async override Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model) where C : class
        {
            bool rs = false;
            RevertCommand command = new RevertCommand();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
            try
            {
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_Employee entity = model as tb_Employee;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Employee_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Employee>(entity as tb_Employee)
                        .Include(m => m.tb_PurOrders)
                    .Include(m => m.tb_Stocktakes)
                    .Include(m => m.tb_Locations)
                    .Include(m => m.tb_FM_OtherExpenseDetails)
                    .Include(m => m.tb_AuditLogses)
                    .Include(m => m.tb_MaterialRequisitions)
                    .Include(m => m.tb_PurEntries)
                    .Include(m => m.tb_SaleOuts)
                    .Include(m => m.tb_Prods)
                    .Include(m => m.tb_MaterialReturns)
                    .Include(m => m.tb_PriceRecords)
                    .Include(m => m.tb_FinishedGoodsInvs)
                    .Include(m => m.tb_ProdReturnings)
                    .Include(m => m.tb_StockTransfers)
                    .Include(m => m.tb_PurReturnEntries)
                    .Include(m => m.tb_CRM_FollowUpPlanses)
                    .Include(m => m.tb_ProdSplits)
                    .Include(m => m.tb_StockOuts)
                    .Include(m => m.tb_ProductionPlans)
                    .Include(m => m.tb_BuyingRequisitions)
                    .Include(m => m.tb_SaleOrders)
                    .Include(m => m.tb_ProdBorrowings)
                    .Include(m => m.tb_SaleOutRes)
                    .Include(m => m.tb_FM_Initial_PayAndReceivables)
                    .Include(m => m.tb_ProdConversions)
                    .Include(m => m.tb_FM_ExpenseClaims)
                    .Include(m => m.tb_FM_PayeeInfos)
                    .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_ProdMerges)
                    .Include(m => m.tb_CRM_FollowUpRecordses)
                    .Include(m => m.tb_FM_PaymentBills)
                    .Include(m => m.tb_CRM_Customers)
                    .Include(m => m.tb_FM_PrePaymentBills)
                    .Include(m => m.tb_ProductionDemands)
                    .Include(m => m.tb_StockIns)
                    .Include(m => m.tb_PurEntryRes)
                    .Include(m => m.tb_CRM_Collaborators)
                    .Include(m => m.tb_FM_OtherExpenses)
                    .Include(m => m.tb_CustomerVendors)
                    .Include(m => m.tb_UserInfos)
                    .Include(m => m.tb_CRM_Leadses)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Employee>(entity as tb_Employee)
                .Include(m => m.tb_PurOrders)
                .Include(m => m.tb_Stocktakes)
                .Include(m => m.tb_Locations)
                .Include(m => m.tb_FM_OtherExpenseDetails)
                .Include(m => m.tb_AuditLogses)
                .Include(m => m.tb_MaterialRequisitions)
                .Include(m => m.tb_PurEntries)
                .Include(m => m.tb_SaleOuts)
                .Include(m => m.tb_Prods)
                .Include(m => m.tb_MaterialReturns)
                .Include(m => m.tb_PriceRecords)
                .Include(m => m.tb_FinishedGoodsInvs)
                .Include(m => m.tb_ProdReturnings)
                .Include(m => m.tb_StockTransfers)
                .Include(m => m.tb_PurReturnEntries)
                .Include(m => m.tb_CRM_FollowUpPlanses)
                .Include(m => m.tb_ProdSplits)
                .Include(m => m.tb_StockOuts)
                .Include(m => m.tb_ProductionPlans)
                .Include(m => m.tb_BuyingRequisitions)
                .Include(m => m.tb_SaleOrders)
                .Include(m => m.tb_ProdBorrowings)
                .Include(m => m.tb_SaleOutRes)
                .Include(m => m.tb_FM_Initial_PayAndReceivables)
                .Include(m => m.tb_ProdConversions)
                .Include(m => m.tb_FM_ExpenseClaims)
                .Include(m => m.tb_FM_PayeeInfos)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_ProdMerges)
                .Include(m => m.tb_CRM_FollowUpRecordses)
                .Include(m => m.tb_FM_PaymentBills)
                .Include(m => m.tb_CRM_Customers)
                .Include(m => m.tb_FM_PrePaymentBills)
                .Include(m => m.tb_ProductionDemands)
                .Include(m => m.tb_StockIns)
                .Include(m => m.tb_PurEntryRes)
                .Include(m => m.tb_CRM_Collaborators)
                .Include(m => m.tb_FM_OtherExpenses)
                .Include(m => m.tb_CustomerVendors)
                .Include(m => m.tb_UserInfos)
                .Include(m => m.tb_CRM_Leadses)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Employee_ID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                //出错后，取消生成的ID等值
                command.Undo();
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Employee>()
                                .Includes(m => m.tb_PurOrders)
                        .Includes(m => m.tb_Stocktakes)
                        .Includes(m => m.tb_Locations)
                        .Includes(m => m.tb_FM_OtherExpenseDetails)
                        .Includes(m => m.tb_AuditLogses)
                        .Includes(m => m.tb_MaterialRequisitions)
                        .Includes(m => m.tb_PurEntries)
                        .Includes(m => m.tb_SaleOuts)
                        .Includes(m => m.tb_Prods)
                        .Includes(m => m.tb_MaterialReturns)
                        .Includes(m => m.tb_PriceRecords)
                        .Includes(m => m.tb_FinishedGoodsInvs)
                        .Includes(m => m.tb_ProdReturnings)
                        .Includes(m => m.tb_StockTransfers)
                        .Includes(m => m.tb_PurReturnEntries)
                        .Includes(m => m.tb_CRM_FollowUpPlanses)
                        .Includes(m => m.tb_ProdSplits)
                        .Includes(m => m.tb_StockOuts)
                        .Includes(m => m.tb_ProductionPlans)
                        .Includes(m => m.tb_BuyingRequisitions)
                        .Includes(m => m.tb_SaleOrders)
                        .Includes(m => m.tb_ProdBorrowings)
                        .Includes(m => m.tb_SaleOutRes)
                        .Includes(m => m.tb_FM_Initial_PayAndReceivables)
                        .Includes(m => m.tb_ProdConversions)
                        .Includes(m => m.tb_FM_ExpenseClaims)
                        .Includes(m => m.tb_FM_PayeeInfos)
                        .Includes(m => m.tb_ManufacturingOrders)
                        .Includes(m => m.tb_ProdMerges)
                        .Includes(m => m.tb_CRM_FollowUpRecordses)
                        .Includes(m => m.tb_FM_PaymentBills)
                        .Includes(m => m.tb_CRM_Customers)
                        .Includes(m => m.tb_FM_PrePaymentBills)
                        .Includes(m => m.tb_ProductionDemands)
                        .Includes(m => m.tb_StockIns)
                        .Includes(m => m.tb_PurEntryRes)
                        .Includes(m => m.tb_CRM_Collaborators)
                        .Includes(m => m.tb_FM_OtherExpenses)
                        .Includes(m => m.tb_CustomerVendors)
                        .Includes(m => m.tb_UserInfos)
                        .Includes(m => m.tb_CRM_Leadses)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Employee entity = model as tb_Employee;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Employee>(m => m.Employee_ID== entity.Employee_ID)
                                .Include(m => m.tb_PurOrders)
                        .Include(m => m.tb_Stocktakes)
                        .Include(m => m.tb_Locations)
                        .Include(m => m.tb_FM_OtherExpenseDetails)
                        .Include(m => m.tb_AuditLogses)
                        .Include(m => m.tb_MaterialRequisitions)
                        .Include(m => m.tb_PurEntries)
                        .Include(m => m.tb_SaleOuts)
                        .Include(m => m.tb_Prods)
                        .Include(m => m.tb_MaterialReturns)
                        .Include(m => m.tb_PriceRecords)
                        .Include(m => m.tb_FinishedGoodsInvs)
                        .Include(m => m.tb_ProdReturnings)
                        .Include(m => m.tb_StockTransfers)
                        .Include(m => m.tb_PurReturnEntries)
                        .Include(m => m.tb_CRM_FollowUpPlanses)
                        .Include(m => m.tb_ProdSplits)
                        .Include(m => m.tb_StockOuts)
                        .Include(m => m.tb_ProductionPlans)
                        .Include(m => m.tb_BuyingRequisitions)
                        .Include(m => m.tb_SaleOrders)
                        .Include(m => m.tb_ProdBorrowings)
                        .Include(m => m.tb_SaleOutRes)
                        .Include(m => m.tb_FM_Initial_PayAndReceivables)
                        .Include(m => m.tb_ProdConversions)
                        .Include(m => m.tb_FM_ExpenseClaims)
                        .Include(m => m.tb_FM_PayeeInfos)
                        .Include(m => m.tb_ManufacturingOrders)
                        .Include(m => m.tb_ProdMerges)
                        .Include(m => m.tb_CRM_FollowUpRecordses)
                        .Include(m => m.tb_FM_PaymentBills)
                        .Include(m => m.tb_CRM_Customers)
                        .Include(m => m.tb_FM_PrePaymentBills)
                        .Include(m => m.tb_ProductionDemands)
                        .Include(m => m.tb_StockIns)
                        .Include(m => m.tb_PurEntryRes)
                        .Include(m => m.tb_CRM_Collaborators)
                        .Include(m => m.tb_FM_OtherExpenses)
                        .Include(m => m.tb_CustomerVendors)
                        .Include(m => m.tb_UserInfos)
                        .Include(m => m.tb_CRM_Leadses)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Employee AddReEntity(tb_Employee entity)
        {
            tb_Employee AddEntity =  _tb_EmployeeServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Employee> AddReEntityAsync(tb_Employee entity)
        {
            tb_Employee AddEntity = await _tb_EmployeeServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Employee entity)
        {
            long id = await _tb_EmployeeServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Employee>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Employee> infos)
        {
            List<long> ids = await _tb_EmployeeServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Employee>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Employee entity)
        {
            bool rs = await _tb_EmployeeServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Employee>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Employee entity)
        {
            bool rs = await _tb_EmployeeServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Employee>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_EmployeeServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Employee>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_EmployeeServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Employee>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Employee>> QueryAsync()
        {
            List<tb_Employee> list = await  _tb_EmployeeServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(list);
            return list;
        }
        
        public virtual List<tb_Employee> Query()
        {
            List<tb_Employee> list =  _tb_EmployeeServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(list);
            return list;
        }
        
        public virtual List<tb_Employee> Query(string wheresql)
        {
            List<tb_Employee> list =  _tb_EmployeeServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Employee>> QueryAsync(string wheresql) 
        {
            List<tb_Employee> list = await _tb_EmployeeServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Employee>> QueryAsync(Expression<Func<tb_Employee, bool>> exp)
        {
            List<tb_Employee> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Employee>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Employee>> QueryByNavAsync()
        {
            List<tb_Employee> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Employee>()
                               .Includes(t => t.tb_bankaccount )
                               .Includes(t => t.tb_department )
                                            .Includes(t => t.tb_PurOrders )
                                .Includes(t => t.tb_Stocktakes )
                                .Includes(t => t.tb_Locations )
                                .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_AuditLogses )
                                .Includes(t => t.tb_MaterialRequisitions )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_SaleOuts )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_PriceRecords )
                                .Includes(t => t.tb_FinishedGoodsInvs )
                                .Includes(t => t.tb_ProdReturnings )
                                .Includes(t => t.tb_StockTransfers )
                                .Includes(t => t.tb_PurReturnEntries )
                                .Includes(t => t.tb_CRM_FollowUpPlanses )
                                .Includes(t => t.tb_ProdSplits )
                                .Includes(t => t.tb_StockOuts )
                                .Includes(t => t.tb_ProductionPlans )
                                .Includes(t => t.tb_BuyingRequisitions )
                                .Includes(t => t.tb_SaleOrders )
                                .Includes(t => t.tb_ProdBorrowings )
                                .Includes(t => t.tb_SaleOutRes )
                                .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                                .Includes(t => t.tb_ProdConversions )
                                .Includes(t => t.tb_FM_ExpenseClaims )
                                .Includes(t => t.tb_FM_PayeeInfos )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ProdMerges )
                                .Includes(t => t.tb_CRM_FollowUpRecordses )
                                .Includes(t => t.tb_FM_PaymentBills )
                                .Includes(t => t.tb_CRM_Customers )
                                .Includes(t => t.tb_FM_PrePaymentBills )
                                .Includes(t => t.tb_ProductionDemands )
                                .Includes(t => t.tb_StockIns )
                                .Includes(t => t.tb_PurEntryRes )
                                .Includes(t => t.tb_CRM_Collaborators )
                                .Includes(t => t.tb_FM_OtherExpenses )
                                .Includes(t => t.tb_CustomerVendors )
                                .Includes(t => t.tb_UserInfos )
                                .Includes(t => t.tb_CRM_Leadses )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Employee>> QueryByNavAsync(Expression<Func<tb_Employee, bool>> exp)
        {
            List<tb_Employee> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Employee>().Where(exp)
                               .Includes(t => t.tb_bankaccount )
                               .Includes(t => t.tb_department )
                                            .Includes(t => t.tb_PurOrders )
                                .Includes(t => t.tb_Stocktakes )
                                .Includes(t => t.tb_Locations )
                                .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_AuditLogses )
                                .Includes(t => t.tb_MaterialRequisitions )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_SaleOuts )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_PriceRecords )
                                .Includes(t => t.tb_FinishedGoodsInvs )
                                .Includes(t => t.tb_ProdReturnings )
                                .Includes(t => t.tb_StockTransfers )
                                .Includes(t => t.tb_PurReturnEntries )
                                .Includes(t => t.tb_CRM_FollowUpPlanses )
                                .Includes(t => t.tb_ProdSplits )
                                .Includes(t => t.tb_StockOuts )
                                .Includes(t => t.tb_ProductionPlans )
                                .Includes(t => t.tb_BuyingRequisitions )
                                .Includes(t => t.tb_SaleOrders )
                                .Includes(t => t.tb_ProdBorrowings )
                                .Includes(t => t.tb_SaleOutRes )
                                .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                                .Includes(t => t.tb_ProdConversions )
                                .Includes(t => t.tb_FM_ExpenseClaims )
                                .Includes(t => t.tb_FM_PayeeInfos )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ProdMerges )
                                .Includes(t => t.tb_CRM_FollowUpRecordses )
                                .Includes(t => t.tb_FM_PaymentBills )
                                .Includes(t => t.tb_CRM_Customers )
                                .Includes(t => t.tb_FM_PrePaymentBills )
                                .Includes(t => t.tb_ProductionDemands )
                                .Includes(t => t.tb_StockIns )
                                .Includes(t => t.tb_PurEntryRes )
                                .Includes(t => t.tb_CRM_Collaborators )
                                .Includes(t => t.tb_FM_OtherExpenses )
                                .Includes(t => t.tb_CustomerVendors )
                                .Includes(t => t.tb_UserInfos )
                                .Includes(t => t.tb_CRM_Leadses )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Employee> QueryByNav(Expression<Func<tb_Employee, bool>> exp)
        {
            List<tb_Employee> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Employee>().Where(exp)
                            .Includes(t => t.tb_bankaccount )
                            .Includes(t => t.tb_department )
                                        .Includes(t => t.tb_PurOrders )
                            .Includes(t => t.tb_Stocktakes )
                            .Includes(t => t.tb_Locations )
                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_AuditLogses )
                            .Includes(t => t.tb_MaterialRequisitions )
                            .Includes(t => t.tb_PurEntries )
                            .Includes(t => t.tb_SaleOuts )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_MaterialReturns )
                            .Includes(t => t.tb_PriceRecords )
                            .Includes(t => t.tb_FinishedGoodsInvs )
                            .Includes(t => t.tb_ProdReturnings )
                            .Includes(t => t.tb_StockTransfers )
                            .Includes(t => t.tb_PurReturnEntries )
                            .Includes(t => t.tb_CRM_FollowUpPlanses )
                            .Includes(t => t.tb_ProdSplits )
                            .Includes(t => t.tb_StockOuts )
                            .Includes(t => t.tb_ProductionPlans )
                            .Includes(t => t.tb_BuyingRequisitions )
                            .Includes(t => t.tb_SaleOrders )
                            .Includes(t => t.tb_ProdBorrowings )
                            .Includes(t => t.tb_SaleOutRes )
                            .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                            .Includes(t => t.tb_ProdConversions )
                            .Includes(t => t.tb_FM_ExpenseClaims )
                            .Includes(t => t.tb_FM_PayeeInfos )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ProdMerges )
                            .Includes(t => t.tb_CRM_FollowUpRecordses )
                            .Includes(t => t.tb_FM_PaymentBills )
                            .Includes(t => t.tb_CRM_Customers )
                            .Includes(t => t.tb_FM_PrePaymentBills )
                            .Includes(t => t.tb_ProductionDemands )
                            .Includes(t => t.tb_StockIns )
                            .Includes(t => t.tb_PurEntryRes )
                            .Includes(t => t.tb_CRM_Collaborators )
                            .Includes(t => t.tb_FM_OtherExpenses )
                            .Includes(t => t.tb_CustomerVendors )
                            .Includes(t => t.tb_UserInfos )
                            .Includes(t => t.tb_CRM_Leadses )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Employee>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Employee>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_EmployeeServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Employee entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Employee>().Where(w => w.Employee_ID == (long)id)
                             .Includes(t => t.tb_bankaccount )
                            .Includes(t => t.tb_department )
                                        .Includes(t => t.tb_PurOrders )
                            .Includes(t => t.tb_Stocktakes )
                            .Includes(t => t.tb_Locations )
                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_AuditLogses )
                            .Includes(t => t.tb_MaterialRequisitions )
                            .Includes(t => t.tb_PurEntries )
                            .Includes(t => t.tb_SaleOuts )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_MaterialReturns )
                            .Includes(t => t.tb_PriceRecords )
                            .Includes(t => t.tb_FinishedGoodsInvs )
                            .Includes(t => t.tb_ProdReturnings )
                            .Includes(t => t.tb_StockTransfers )
                            .Includes(t => t.tb_PurReturnEntries )
                            .Includes(t => t.tb_CRM_FollowUpPlanses )
                            .Includes(t => t.tb_ProdSplits )
                            .Includes(t => t.tb_StockOuts )
                            .Includes(t => t.tb_ProductionPlans )
                            .Includes(t => t.tb_BuyingRequisitions )
                            .Includes(t => t.tb_SaleOrders )
                            .Includes(t => t.tb_ProdBorrowings )
                            .Includes(t => t.tb_SaleOutRes )
                            .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                            .Includes(t => t.tb_ProdConversions )
                            .Includes(t => t.tb_FM_ExpenseClaims )
                            .Includes(t => t.tb_FM_PayeeInfos )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ProdMerges )
                            .Includes(t => t.tb_CRM_FollowUpRecordses )
                            .Includes(t => t.tb_FM_PaymentBills )
                            .Includes(t => t.tb_CRM_Customers )
                            .Includes(t => t.tb_FM_PrePaymentBills )
                            .Includes(t => t.tb_ProductionDemands )
                            .Includes(t => t.tb_StockIns )
                            .Includes(t => t.tb_PurEntryRes )
                            .Includes(t => t.tb_CRM_Collaborators )
                            .Includes(t => t.tb_FM_OtherExpenses )
                            .Includes(t => t.tb_CustomerVendors )
                            .Includes(t => t.tb_UserInfos )
                            .Includes(t => t.tb_CRM_Leadses )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Employee>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



