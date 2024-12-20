
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:02
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
       // private readonly ApplicationContext _appContext;
       
        public tb_CustomerVendorController(ILogger<tb_CustomerVendorController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CustomerVendorServices tb_CustomerVendorServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CustomerVendorServices = tb_CustomerVendorServices;
            _appContext = appContext;
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
        public override bool ExistFieldValue(Expression<Func<T, bool>> exp)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp).Any();
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
                        MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CustomerVendorServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(entity);
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
                        MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CustomerVendorServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(entity);
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
                MyCacheManager.Instance.UpdateEntityList<List<T>>(list);
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
                MyCacheManager.Instance.UpdateEntityList<List<T>>(list);
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
                MyCacheManager.Instance.DeleteEntityList<tb_CustomerVendor>(entity);
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
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.CustomerVendor_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_CustomerVendor>(result);
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
            var  querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }
        
        public async override Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model) where C : class
        {
            bool rs = false;
            Command command = new Command();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
            try
            {
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
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
                        .Include(m => m.tb_PurOrders)
                    .Include(m => m.tb_FM_OtherExpenseDetails)
                    .Include(m => m.tb_MaterialRequisitions)
                    .Include(m => m.tb_Returns)
                    .Include(m => m.tb_PurEntries)
                    .Include(m => m.tb_SaleOuts)
                    .Include(m => m.tb_Prods)
                    .Include(m => m.tb_InvoiceInfos)
                    .Include(m => m.tb_MaterialReturns)
                    .Include(m => m.tb_FinishedGoodsInvs)
                    .Include(m => m.tb_ProdReturnings)
                    .Include(m => m.tb_CustomerVendorFileses)
                    .Include(m => m.tb_PurReturnEntries)
                    .Include(m => m.tb_StockOuts)
                    .Include(m => m.tb_SaleOrders)
                    .Include(m => m.tb_ProdBorrowings)
                    .Include(m => m.tb_SaleOutRes)
                    .Include(m => m.tb_FM_Initial_PayAndReceivables)
                    .Include(m => m.tb_FM_PayeeInfos)
                    .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_FM_PaymentBills)
                    .Include(m => m.tb_StockIns)
                    .Include(m => m.tb_PurEntryRes)
                    .Include(m => m.tb_FM_PrePaymentBillDetails)
                    .Include(m => m.tb_PurOrderRes)
                    .Include(m => m.tb_PurGoodsRecommendDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_CustomerVendor>(entity as tb_CustomerVendor)
                .Include(m => m.tb_PurOrders)
                .Include(m => m.tb_FM_OtherExpenseDetails)
                .Include(m => m.tb_MaterialRequisitions)
                .Include(m => m.tb_Returns)
                .Include(m => m.tb_PurEntries)
                .Include(m => m.tb_SaleOuts)
                .Include(m => m.tb_Prods)
                .Include(m => m.tb_InvoiceInfos)
                .Include(m => m.tb_MaterialReturns)
                .Include(m => m.tb_FinishedGoodsInvs)
                .Include(m => m.tb_ProdReturnings)
                .Include(m => m.tb_CustomerVendorFileses)
                .Include(m => m.tb_PurReturnEntries)
                .Include(m => m.tb_StockOuts)
                .Include(m => m.tb_SaleOrders)
                .Include(m => m.tb_ProdBorrowings)
                .Include(m => m.tb_SaleOutRes)
                .Include(m => m.tb_FM_Initial_PayAndReceivables)
                .Include(m => m.tb_FM_PayeeInfos)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_FM_PaymentBills)
                .Include(m => m.tb_StockIns)
                .Include(m => m.tb_PurEntryRes)
                .Include(m => m.tb_FM_PrePaymentBillDetails)
                .Include(m => m.tb_PurOrderRes)
                .Include(m => m.tb_PurGoodsRecommendDetails)
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>()
                                .Includes(m => m.tb_PurOrders)
                        .Includes(m => m.tb_FM_OtherExpenseDetails)
                        .Includes(m => m.tb_MaterialRequisitions)
                        .Includes(m => m.tb_Returns)
                        .Includes(m => m.tb_PurEntries)
                        .Includes(m => m.tb_SaleOuts)
                        .Includes(m => m.tb_Prods)
                        .Includes(m => m.tb_InvoiceInfos)
                        .Includes(m => m.tb_MaterialReturns)
                        .Includes(m => m.tb_FinishedGoodsInvs)
                        .Includes(m => m.tb_ProdReturnings)
                        .Includes(m => m.tb_CustomerVendorFileses)
                        .Includes(m => m.tb_PurReturnEntries)
                        .Includes(m => m.tb_StockOuts)
                        .Includes(m => m.tb_SaleOrders)
                        .Includes(m => m.tb_ProdBorrowings)
                        .Includes(m => m.tb_SaleOutRes)
                        .Includes(m => m.tb_FM_Initial_PayAndReceivables)
                        .Includes(m => m.tb_FM_PayeeInfos)
                        .Includes(m => m.tb_ManufacturingOrders)
                        .Includes(m => m.tb_ManufacturingOrders)
                        .Includes(m => m.tb_FM_PaymentBills)
                        .Includes(m => m.tb_StockIns)
                        .Includes(m => m.tb_PurEntryRes)
                        .Includes(m => m.tb_FM_PrePaymentBillDetails)
                        .Includes(m => m.tb_PurOrderRes)
                        .Includes(m => m.tb_PurGoodsRecommendDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CustomerVendor entity = model as tb_CustomerVendor;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CustomerVendor>(m => m.CustomerVendor_ID== entity.CustomerVendor_ID)
                                .Include(m => m.tb_PurOrders)
                        .Include(m => m.tb_FM_OtherExpenseDetails)
                        .Include(m => m.tb_MaterialRequisitions)
                        .Include(m => m.tb_Returns)
                        .Include(m => m.tb_PurEntries)
                        .Include(m => m.tb_SaleOuts)
                        .Include(m => m.tb_Prods)
                        .Include(m => m.tb_InvoiceInfos)
                        .Include(m => m.tb_MaterialReturns)
                        .Include(m => m.tb_FinishedGoodsInvs)
                        .Include(m => m.tb_ProdReturnings)
                        .Include(m => m.tb_CustomerVendorFileses)
                        .Include(m => m.tb_PurReturnEntries)
                        .Include(m => m.tb_StockOuts)
                        .Include(m => m.tb_SaleOrders)
                        .Include(m => m.tb_ProdBorrowings)
                        .Include(m => m.tb_SaleOutRes)
                        .Include(m => m.tb_FM_Initial_PayAndReceivables)
                        .Include(m => m.tb_FM_PayeeInfos)
                        .Include(m => m.tb_ManufacturingOrders)
                        .Include(m => m.tb_ManufacturingOrders)
                        .Include(m => m.tb_FM_PaymentBills)
                        .Include(m => m.tb_StockIns)
                        .Include(m => m.tb_PurEntryRes)
                        .Include(m => m.tb_FM_PrePaymentBillDetails)
                        .Include(m => m.tb_PurOrderRes)
                        .Include(m => m.tb_PurGoodsRecommendDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_CustomerVendor AddReEntity(tb_CustomerVendor entity)
        {
            tb_CustomerVendor AddEntity =  _tb_CustomerVendorServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CustomerVendor> AddReEntityAsync(tb_CustomerVendor entity)
        {
            tb_CustomerVendor AddEntity = await _tb_CustomerVendorServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CustomerVendor entity)
        {
            long id = await _tb_CustomerVendorServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CustomerVendor> infos)
        {
            List<long> ids = await _tb_CustomerVendorServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CustomerVendor entity)
        {
            bool rs = await _tb_CustomerVendorServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CustomerVendor>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CustomerVendor entity)
        {
            bool rs = await _tb_CustomerVendorServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CustomerVendorServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CustomerVendor>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CustomerVendorServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CustomerVendor>(ids);
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
            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        public virtual List<tb_CustomerVendor> Query()
        {
            List<tb_CustomerVendor> list =  _tb_CustomerVendorServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        public virtual List<tb_CustomerVendor> Query(string wheresql)
        {
            List<tb_CustomerVendor> list =  _tb_CustomerVendorServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CustomerVendor>> QueryAsync(string wheresql) 
        {
            List<tb_CustomerVendor> list = await _tb_CustomerVendorServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(list);
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
            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CustomerVendor>> QueryByNavAsync()
        {
            List<tb_CustomerVendor> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>()
                               .Includes(t => t.tb_bankaccount )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendortype )
                               .Includes(t => t.tb_paymentmethod )
                                            .Includes(t => t.tb_PurOrders )
                                .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_MaterialRequisitions )
                                .Includes(t => t.tb_Returns )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_SaleOuts )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_InvoiceInfos )
                                .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_FinishedGoodsInvs )
                                .Includes(t => t.tb_ProdReturnings )
                                .Includes(t => t.tb_CustomerVendorFileses )
                                .Includes(t => t.tb_PurReturnEntries )
                                .Includes(t => t.tb_StockOuts )
                                .Includes(t => t.tb_SaleOrders )
                                .Includes(t => t.tb_ProdBorrowings )
                                .Includes(t => t.tb_SaleOutRes )
                                .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                                .Includes(t => t.tb_FM_PayeeInfos )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_FM_PaymentBills )
                                .Includes(t => t.tb_StockIns )
                                .Includes(t => t.tb_PurEntryRes )
                                .Includes(t => t.tb_FM_PrePaymentBillDetails )
                                .Includes(t => t.tb_PurOrderRes )
                                .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CustomerVendor>> QueryByNavAsync(Expression<Func<tb_CustomerVendor, bool>> exp)
        {
            List<tb_CustomerVendor> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>().Where(exp)
                               .Includes(t => t.tb_bankaccount )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendortype )
                               .Includes(t => t.tb_paymentmethod )
                                            .Includes(t => t.tb_PurOrders )
                                .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_MaterialRequisitions )
                                .Includes(t => t.tb_Returns )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_SaleOuts )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_InvoiceInfos )
                                .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_FinishedGoodsInvs )
                                .Includes(t => t.tb_ProdReturnings )
                                .Includes(t => t.tb_CustomerVendorFileses )
                                .Includes(t => t.tb_PurReturnEntries )
                                .Includes(t => t.tb_StockOuts )
                                .Includes(t => t.tb_SaleOrders )
                                .Includes(t => t.tb_ProdBorrowings )
                                .Includes(t => t.tb_SaleOutRes )
                                .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                                .Includes(t => t.tb_FM_PayeeInfos )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_FM_PaymentBills )
                                .Includes(t => t.tb_StockIns )
                                .Includes(t => t.tb_PurEntryRes )
                                .Includes(t => t.tb_FM_PrePaymentBillDetails )
                                .Includes(t => t.tb_PurOrderRes )
                                .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CustomerVendor> QueryByNav(Expression<Func<tb_CustomerVendor, bool>> exp)
        {
            List<tb_CustomerVendor> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>().Where(exp)
                            .Includes(t => t.tb_bankaccount )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendortype )
                            .Includes(t => t.tb_paymentmethod )
                                        .Includes(t => t.tb_PurOrders )
                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_MaterialRequisitions )
                            .Includes(t => t.tb_Returns )
                            .Includes(t => t.tb_PurEntries )
                            .Includes(t => t.tb_SaleOuts )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_InvoiceInfos )
                            .Includes(t => t.tb_MaterialReturns )
                            .Includes(t => t.tb_FinishedGoodsInvs )
                            .Includes(t => t.tb_ProdReturnings )
                            .Includes(t => t.tb_CustomerVendorFileses )
                            .Includes(t => t.tb_PurReturnEntries )
                            .Includes(t => t.tb_StockOuts )
                            .Includes(t => t.tb_SaleOrders )
                            .Includes(t => t.tb_ProdBorrowings )
                            .Includes(t => t.tb_SaleOutRes )
                            .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                            .Includes(t => t.tb_FM_PayeeInfos )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_FM_PaymentBills )
                            .Includes(t => t.tb_StockIns )
                            .Includes(t => t.tb_PurEntryRes )
                            .Includes(t => t.tb_FM_PrePaymentBillDetails )
                            .Includes(t => t.tb_PurOrderRes )
                            .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CustomerVendor>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendor>().Where(useLike,dto);
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
                             .Includes(t => t.tb_bankaccount )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendortype )
                            .Includes(t => t.tb_paymentmethod )
                                        .Includes(t => t.tb_PurOrders )
                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_MaterialRequisitions )
                            .Includes(t => t.tb_Returns )
                            .Includes(t => t.tb_PurEntries )
                            .Includes(t => t.tb_SaleOuts )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_InvoiceInfos )
                            .Includes(t => t.tb_MaterialReturns )
                            .Includes(t => t.tb_FinishedGoodsInvs )
                            .Includes(t => t.tb_ProdReturnings )
                            .Includes(t => t.tb_CustomerVendorFileses )
                            .Includes(t => t.tb_PurReturnEntries )
                            .Includes(t => t.tb_StockOuts )
                            .Includes(t => t.tb_SaleOrders )
                            .Includes(t => t.tb_ProdBorrowings )
                            .Includes(t => t.tb_SaleOutRes )
                            .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                            .Includes(t => t.tb_FM_PayeeInfos )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_FM_PaymentBills )
                            .Includes(t => t.tb_StockIns )
                            .Includes(t => t.tb_PurEntryRes )
                            .Includes(t => t.tb_FM_PrePaymentBillDetails )
                            .Includes(t => t.tb_PurOrderRes )
                            .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_CustomerVendor>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



