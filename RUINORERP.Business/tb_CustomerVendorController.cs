// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/07/2025 11:04:21
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
    /// 客户厂商表 开票资料这种与财务有关另外开表
    /// </summary>
    public partial class tb_CustomerVendorController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CustomerVendorServices _tb_CustomerVendorServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_CustomerVendorController(ILogger<tb_CustomerVendorController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CustomerVendorServices tb_CustomerVendorServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CustomerVendorServices = tb_CustomerVendorServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_CustomerVendor info)
        {

           // tb_CustomerVendorValidator validator = new tb_CustomerVendorValidator();
           tb_CustomerVendorValidator validator = _appContext.GetRequiredService<tb_CustomerVendorValidator>();
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
        public async Task<ReturnResults<tb_CustomerVendor>> SaveOrUpdate(tb_CustomerVendor entity)
        {
            ReturnResults<tb_CustomerVendor> rr = new ReturnResults<tb_CustomerVendor>();
            tb_CustomerVendor Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.CustomerVendor_ID > 0)
                {
                    bool rs = await _tb_CustomerVendorServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendor>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CustomerVendorServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendor>(entity);
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
            tb_CustomerVendor entity = model as tb_CustomerVendor;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.CustomerVendor_ID > 0)
                {
                    bool rs = await _tb_CustomerVendorServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendor>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CustomerVendorServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendor>(entity);
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
            List<T> list = await _tb_CustomerVendorServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_CustomerVendor entity = item as tb_CustomerVendor;
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
            List<T> list = await _tb_CustomerVendorServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_CustomerVendor entity = item as tb_CustomerVendor;
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
            tb_CustomerVendor entity = model as tb_CustomerVendor;
            bool rs = await _tb_CustomerVendorServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendor>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_CustomerVendor> entitys = models as List<tb_CustomerVendor>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_CustomerVendor>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_CustomerVendor>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_CustomerVendorValidator validator = new tb_CustomerVendorValidator();
           tb_CustomerVendorValidator validator = _appContext.GetRequiredService<tb_CustomerVendorValidator>();
            ValidationResult results = validator.Validate(info as tb_CustomerVendor);
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

                tb_CustomerVendor entity = model as tb_CustomerVendor;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.CustomerVendor_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_CustomerVendor>(entity as tb_CustomerVendor)
                        .Include(m => m.tb_FM_Invoices)
                    .Include(m => m.tb_AS_AfterSaleDeliveries)
                    .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_FM_OtherExpenseDetails)
                    .Include(m => m.tb_AS_AfterSaleApplies)
                    .Include(m => m.tb_FM_Statements)
                    .Include(m => m.tb_PurOrders)
                    .Include(m => m.tb_Prods)
                    .Include(m => m.tb_InvoiceInfos)
                    .Include(m => m.tb_PurReturnEntries)
                    .Include(m => m.tb_MaterialReturns)
                    .Include(m => m.tb_ProdBorrowings)
                    .Include(m => m.tb_AS_RepairInStocks)
                    .Include(m => m.tb_PurOrderRes)
                    .Include(m => m.tb_ProdReturnings)
                    .Include(m => m.tb_SaleOutRes)
                    .Include(m => m.tb_CustomerVendorFileses)
                    .Include(m => m.tb_PurGoodsRecommendDetails)
                    .Include(m => m.tb_FM_PaymentRecords)
                    .Include(m => m.tb_FM_PaymentApplications)
                    .Include(m => m.tb_PurEntryRes)
                    .Include(m => m.tb_StockOuts)
                    .Include(m => m.tb_SO_Contracts)
                    .Include(m => m.tb_SaleOuts)
                    .Include(m => m.tb_MRP_ReworkEntries)
                    .Include(m => m.tb_AS_RepairOrders)
                    .Include(m => m.tb_FM_PayeeInfos)
                    .Include(m => m.tb_MRP_ReworkReturns)
                    .Include(m => m.tb_PurEntries)
                    .Include(m => m.tb_PO_Contracts)
                    .Include(m => m.tb_FM_PriceAdjustments)
                    .Include(m => m.tb_StockIns)
                    .Include(m => m.tb_FM_PreReceivedPayments)
                    .Include(m => m.tb_FinishedGoodsInvs)
                    .Include(m => m.tb_BillingInformations)
                    .Include(m => m.tb_FM_ReceivablePayables)
                    .Include(m => m.tb_SaleOrders)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_CustomerVendor>(entity as tb_CustomerVendor)
                .Include(m => m.tb_FM_Invoices)
                .Include(m => m.tb_AS_AfterSaleDeliveries)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_FM_OtherExpenseDetails)
                .Include(m => m.tb_AS_AfterSaleApplies)
                .Include(m => m.tb_FM_Statements)
                .Include(m => m.tb_PurOrders)
                .Include(m => m.tb_Prods)
                .Include(m => m.tb_InvoiceInfos)
                .Include(m => m.tb_PurReturnEntries)
                .Include(m => m.tb_MaterialReturns)
                .Include(m => m.tb_ProdBorrowings)
                .Include(m => m.tb_AS_RepairInStocks)
                .Include(m => m.tb_PurOrderRes)
                .Include(m => m.tb_ProdReturnings)
                .Include(m => m.tb_SaleOutRes)
                .Include(m => m.tb_CustomerVendorFileses)
                .Include(m => m.tb_PurGoodsRecommendDetails)
                .Include(m => m.tb_FM_PaymentRecords)
                .Include(m => m.tb_FM_PaymentApplications)
                .Include(m => m.tb_PurEntryRes)
                .Include(m => m.tb_StockOuts)
                .Include(m => m.tb_SO_Contracts)
                .Include(m => m.tb_SaleOuts)
                .Include(m => m.tb_MRP_ReworkEntries)
                .Include(m => m.tb_AS_RepairOrders)
                .Include(m => m.tb_FM_PayeeInfos)
                .Include(m => m.tb_MRP_ReworkReturns)
                .Include(m => m.tb_PurEntries)
                .Include(m => m.tb_PO_Contracts)
                .Include(m => m.tb_FM_PriceAdjustments)
                .Include(m => m.tb_StockIns)
                .Include(m => m.tb_FM_PreReceivedPayments)
                .Include(m => m.tb_FinishedGoodsInvs)
                .Include(m => m.tb_BillingInformations)
                .Include(m => m.tb_FM_ReceivablePayables)
                .Include(m => m.tb_SaleOrders)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.CustomerVendor_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>()
                                .Includes(m => m.tb_crm_customer)
                            .Includes(m => m.tb_employee)
                            .Includes(m => m.tb_customervendortype)
                            .Includes(m => m.tb_paymentmethod)
                                            .Includes(m => m.tb_FM_Invoices)
                        .Includes(m => m.tb_AS_AfterSaleDeliveries)
                        .Includes(m => m.tb_ManufacturingOrders)
                        .Includes(m => m.tb_ManufacturingOrdersByCustomerVendor)
                        .Includes(m => m.tb_FM_OtherExpenseDetails)
                        .Includes(m => m.tb_AS_AfterSaleApplies)
                        .Includes(m => m.tb_FM_Statements)
                        .Includes(m => m.tb_PurOrders)
                        .Includes(m => m.tb_Prods)
                        .Includes(m => m.tb_InvoiceInfos)
                        .Includes(m => m.tb_PurReturnEntries)
                        .Includes(m => m.tb_MaterialReturns)
                        .Includes(m => m.tb_ProdBorrowings)
                        .Includes(m => m.tb_AS_RepairInStocks)
                        .Includes(m => m.tb_PurOrderRes)
                        .Includes(m => m.tb_ProdReturnings)
                        .Includes(m => m.tb_SaleOutRes)
                        .Includes(m => m.tb_CustomerVendorFileses)
                        .Includes(m => m.tb_PurGoodsRecommendDetails)
                        .Includes(m => m.tb_FM_PaymentRecords)
                        .Includes(m => m.tb_FM_PaymentApplications)
                        .Includes(m => m.tb_PurEntryRes)
                        .Includes(m => m.tb_StockOuts)
                        .Includes(m => m.tb_SO_Contracts)
                        .Includes(m => m.tb_SaleOuts)
                        .Includes(m => m.tb_MRP_ReworkEntries)
                        .Includes(m => m.tb_AS_RepairOrders)
                        .Includes(m => m.tb_FM_PayeeInfos)
                        .Includes(m => m.tb_MRP_ReworkReturns)
                        .Includes(m => m.tb_PurEntries)
                        .Includes(m => m.tb_PO_Contracts)
                        .Includes(m => m.tb_FM_PriceAdjustments)
                        .Includes(m => m.tb_StockIns)
                        .Includes(m => m.tb_FM_PreReceivedPayments)
                        .Includes(m => m.tb_FinishedGoodsInvs)
                        .Includes(m => m.tb_BillingInformations)
                        .Includes(m => m.tb_FM_ReceivablePayables)
                        .Includes(m => m.tb_SaleOrders)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CustomerVendor entity = model as tb_CustomerVendor;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CustomerVendor>(m => m.CustomerVendor_ID== entity.CustomerVendor_ID)
                                .Include(m => m.tb_FM_Invoices)
                        .Include(m => m.tb_AS_AfterSaleDeliveries)
                        .Include(m => m.tb_ManufacturingOrders)
                        .Include(m => m.tb_ManufacturingOrdersByCustomerVendor)
                        .Include(m => m.tb_FM_OtherExpenseDetails)
                        .Include(m => m.tb_AS_AfterSaleApplies)
                        .Include(m => m.tb_FM_Statements)
                        .Include(m => m.tb_PurOrders)
                        .Include(m => m.tb_Prods)
                        .Include(m => m.tb_InvoiceInfos)
                        .Include(m => m.tb_PurReturnEntries)
                        .Include(m => m.tb_MaterialReturns)
                        .Include(m => m.tb_ProdBorrowings)
                        .Include(m => m.tb_AS_RepairInStocks)
                        .Include(m => m.tb_PurOrderRes)
                        .Include(m => m.tb_ProdReturnings)
                        .Include(m => m.tb_SaleOutRes)
                        .Include(m => m.tb_CustomerVendorFileses)
                        .Include(m => m.tb_PurGoodsRecommendDetails)
                        .Include(m => m.tb_FM_PaymentRecords)
                        .Include(m => m.tb_FM_PaymentApplications)
                        .Include(m => m.tb_PurEntryRes)
                        .Include(m => m.tb_StockOuts)
                        .Include(m => m.tb_SO_Contracts)
                        .Include(m => m.tb_SaleOuts)
                        .Include(m => m.tb_MRP_ReworkEntries)
                        .Include(m => m.tb_AS_RepairOrders)
                        .Include(m => m.tb_FM_PayeeInfos)
                        .Include(m => m.tb_MRP_ReworkReturns)
                        .Include(m => m.tb_PurEntries)
                        .Include(m => m.tb_PO_Contracts)
                        .Include(m => m.tb_FM_PriceAdjustments)
                        .Include(m => m.tb_StockIns)
                        .Include(m => m.tb_FM_PreReceivedPayments)
                        .Include(m => m.tb_FinishedGoodsInvs)
                        .Include(m => m.tb_BillingInformations)
                        .Include(m => m.tb_FM_ReceivablePayables)
                        .Include(m => m.tb_SaleOrders)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_CustomerVendor AddReEntity(tb_CustomerVendor entity)
        {
            tb_CustomerVendor AddEntity =  _tb_CustomerVendorServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendor>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CustomerVendor> AddReEntityAsync(tb_CustomerVendor entity)
        {
            tb_CustomerVendor AddEntity = await _tb_CustomerVendorServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendor>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CustomerVendor entity)
        {
            long id = await _tb_CustomerVendorServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendor>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CustomerVendor> infos)
        {
            List<long> ids = await _tb_CustomerVendorServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendor>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CustomerVendor entity)
        {
            bool rs = await _tb_CustomerVendorServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendor>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CustomerVendor entity)
        {
            bool rs = await _tb_CustomerVendorServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendor>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CustomerVendorServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendor>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CustomerVendorServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_CustomerVendor>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_CustomerVendor>> QueryAsync()
        {
            List<tb_CustomerVendor> list = await  _tb_CustomerVendorServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        public virtual List<tb_CustomerVendor> Query()
        {
            List<tb_CustomerVendor> list =  _tb_CustomerVendorServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        public virtual List<tb_CustomerVendor> Query(string wheresql)
        {
            List<tb_CustomerVendor> list =  _tb_CustomerVendorServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CustomerVendor>> QueryAsync(string wheresql) 
        {
            List<tb_CustomerVendor> list = await _tb_CustomerVendorServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_CustomerVendor>> QueryAsync(Expression<Func<tb_CustomerVendor, bool>> exp)
        {
            List<tb_CustomerVendor> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CustomerVendor>> QueryByNavAsync()
        {
            List<tb_CustomerVendor> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>()
                               .Includes(t => t.tb_crm_customer )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendortype )
                               .Includes(t => t.tb_paymentmethod )
                                            .Includes(t => t.tb_FM_Invoices )
                                .Includes(t => t.tb_AS_AfterSaleDeliveries )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_AS_AfterSaleApplies )
                                .Includes(t => t.tb_FM_Statements )
                                .Includes(t => t.tb_PurOrders )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_InvoiceInfos )
                                .Includes(t => t.tb_PurReturnEntries )
                                .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_ProdBorrowings )
                                .Includes(t => t.tb_AS_RepairInStocks )
                                .Includes(t => t.tb_PurOrderRes )
                                .Includes(t => t.tb_ProdReturnings )
                                .Includes(t => t.tb_SaleOutRes )
                                .Includes(t => t.tb_CustomerVendorFileses )
                                .Includes(t => t.tb_PurGoodsRecommendDetails )
                                .Includes(t => t.tb_FM_PaymentRecords )
                                .Includes(t => t.tb_FM_PaymentApplications )
                                .Includes(t => t.tb_PurEntryRes )
                                .Includes(t => t.tb_StockOuts )
                                .Includes(t => t.tb_SO_Contracts )
                                .Includes(t => t.tb_SaleOuts )
                                .Includes(t => t.tb_MRP_ReworkEntries )
                                .Includes(t => t.tb_AS_RepairOrders )
                                .Includes(t => t.tb_FM_PayeeInfos )
                                .Includes(t => t.tb_MRP_ReworkReturns )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_PO_Contracts )
                                .Includes(t => t.tb_FM_PriceAdjustments )
                                .Includes(t => t.tb_StockIns )
                                .Includes(t => t.tb_FM_PreReceivedPayments )
                                .Includes(t => t.tb_FinishedGoodsInvs )
                                .Includes(t => t.tb_BillingInformations )
                                .Includes(t => t.tb_FM_ReceivablePayables )
                                .Includes(t => t.tb_SaleOrders )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CustomerVendor>> QueryByNavAsync(Expression<Func<tb_CustomerVendor, bool>> exp)
        {
            List<tb_CustomerVendor> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>().Where(exp)
                               .Includes(t => t.tb_crm_customer )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendortype )
                               .Includes(t => t.tb_paymentmethod )
                                            .Includes(t => t.tb_FM_Invoices )
                                .Includes(t => t.tb_AS_AfterSaleDeliveries )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ManufacturingOrdersByCustomerVendor )
                                .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_AS_AfterSaleApplies )
                                .Includes(t => t.tb_FM_Statements )
                                .Includes(t => t.tb_PurOrders )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_InvoiceInfos )
                                .Includes(t => t.tb_PurReturnEntries )
                                .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_ProdBorrowings )
                                .Includes(t => t.tb_AS_RepairInStocks )
                                .Includes(t => t.tb_PurOrderRes )
                                .Includes(t => t.tb_ProdReturnings )
                                .Includes(t => t.tb_SaleOutRes )
                                .Includes(t => t.tb_CustomerVendorFileses )
                                .Includes(t => t.tb_PurGoodsRecommendDetails )
                                .Includes(t => t.tb_FM_PaymentRecords )
                                .Includes(t => t.tb_FM_PaymentApplications )
                                .Includes(t => t.tb_PurEntryRes )
                                .Includes(t => t.tb_StockOuts )
                                .Includes(t => t.tb_SO_Contracts )
                                .Includes(t => t.tb_SaleOuts )
                                .Includes(t => t.tb_MRP_ReworkEntries )
                                .Includes(t => t.tb_AS_RepairOrders )
                                .Includes(t => t.tb_FM_PayeeInfos )
                                .Includes(t => t.tb_MRP_ReworkReturns )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_PO_Contracts )
                                .Includes(t => t.tb_FM_PriceAdjustments )
                                .Includes(t => t.tb_StockIns )
                                .Includes(t => t.tb_FM_PreReceivedPayments )
                                .Includes(t => t.tb_FinishedGoodsInvs )
                                .Includes(t => t.tb_BillingInformations )
                                .Includes(t => t.tb_FM_ReceivablePayables )
                                .Includes(t => t.tb_SaleOrders )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CustomerVendor> QueryByNav(Expression<Func<tb_CustomerVendor, bool>> exp)
        {
            List<tb_CustomerVendor> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>().Where(exp)
                            .Includes(t => t.tb_crm_customer )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendortype )
                            .Includes(t => t.tb_paymentmethod )
                                        .Includes(t => t.tb_FM_Invoices )
                            .Includes(t => t.tb_AS_AfterSaleDeliveries )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ManufacturingOrdersByCustomerVendor )
                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_AS_AfterSaleApplies )
                            .Includes(t => t.tb_FM_Statements )
                            .Includes(t => t.tb_PurOrders )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_InvoiceInfos )
                            .Includes(t => t.tb_PurReturnEntries )
                            .Includes(t => t.tb_MaterialReturns )
                            .Includes(t => t.tb_ProdBorrowings )
                            .Includes(t => t.tb_AS_RepairInStocks )
                            .Includes(t => t.tb_PurOrderRes )
                            .Includes(t => t.tb_ProdReturnings )
                            .Includes(t => t.tb_SaleOutRes )
                            .Includes(t => t.tb_CustomerVendorFileses )
                            .Includes(t => t.tb_PurGoodsRecommendDetails )
                            .Includes(t => t.tb_FM_PaymentRecords )
                            .Includes(t => t.tb_FM_PaymentApplications )
                            .Includes(t => t.tb_PurEntryRes )
                            .Includes(t => t.tb_StockOuts )
                            .Includes(t => t.tb_SO_Contracts )
                            .Includes(t => t.tb_SaleOuts )
                            .Includes(t => t.tb_MRP_ReworkEntries )
                            .Includes(t => t.tb_AS_RepairOrders )
                            .Includes(t => t.tb_FM_PayeeInfos )
                            .Includes(t => t.tb_MRP_ReworkReturns )
                            .Includes(t => t.tb_PurEntries )
                            .Includes(t => t.tb_PO_Contracts )
                            .Includes(t => t.tb_FM_PriceAdjustments )
                            .Includes(t => t.tb_StockIns )
                            .Includes(t => t.tb_FM_PreReceivedPayments )
                            .Includes(t => t.tb_FinishedGoodsInvs )
                            .Includes(t => t.tb_BillingInformations )
                            .Includes(t => t.tb_FM_ReceivablePayables )
                            .Includes(t => t.tb_SaleOrders )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CustomerVendor>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CustomerVendorServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_CustomerVendor entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>().Where(w => w.CustomerVendor_ID == (long)id)
                             .Includes(t => t.tb_crm_customer )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendortype )
                            .Includes(t => t.tb_paymentmethod )
                        

                                            .Includes(t => t.tb_FM_Invoices )
                                            .Includes(t => t.tb_AS_AfterSaleDeliveries )
                                            .Includes(t => t.tb_ManufacturingOrders )
                                            .Includes(t => t.tb_ManufacturingOrdersByCustomerVendor )
                                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                                            .Includes(t => t.tb_AS_AfterSaleApplies )
                                            .Includes(t => t.tb_FM_Statements )
                                            .Includes(t => t.tb_PurOrders )
                                            .Includes(t => t.tb_Prods )
                                            .Includes(t => t.tb_InvoiceInfos )
                                            .Includes(t => t.tb_PurReturnEntries )
                                            .Includes(t => t.tb_MaterialReturns )
                                            .Includes(t => t.tb_ProdBorrowings )
                                            .Includes(t => t.tb_AS_RepairInStocks )
                                            .Includes(t => t.tb_PurOrderRes )
                                            .Includes(t => t.tb_ProdReturnings )
                                            .Includes(t => t.tb_SaleOutRes )
                                            .Includes(t => t.tb_CustomerVendorFileses )
                                            .Includes(t => t.tb_PurGoodsRecommendDetails )
                                            .Includes(t => t.tb_FM_PaymentRecords )
                                            .Includes(t => t.tb_FM_PaymentApplications )
                                            .Includes(t => t.tb_PurEntryRes )
                                            .Includes(t => t.tb_StockOuts )
                                            .Includes(t => t.tb_SO_Contracts )
                                            .Includes(t => t.tb_SaleOuts )
                                            .Includes(t => t.tb_MRP_ReworkEntries )
                                            .Includes(t => t.tb_AS_RepairOrders )
                                            .Includes(t => t.tb_FM_PayeeInfos )
                                            .Includes(t => t.tb_MRP_ReworkReturns )
                                            .Includes(t => t.tb_PurEntries )
                                            .Includes(t => t.tb_PO_Contracts )
                                            .Includes(t => t.tb_FM_PriceAdjustments )
                                            .Includes(t => t.tb_StockIns )
                                            .Includes(t => t.tb_FM_PreReceivedPayments )
                                            .Includes(t => t.tb_FinishedGoodsInvs )
                                            .Includes(t => t.tb_BillingInformations )
                                            .Includes(t => t.tb_FM_ReceivablePayables )
                                            .Includes(t => t.tb_SaleOrders )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendor>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



