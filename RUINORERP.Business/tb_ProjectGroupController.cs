// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:16
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
    /// 项目组信息 用于业务分组小团队
    /// </summary>
    public partial class tb_ProjectGroupController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProjectGroupServices _tb_ProjectGroupServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProjectGroupController(ILogger<tb_ProjectGroupController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProjectGroupServices tb_ProjectGroupServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProjectGroupServices = tb_ProjectGroupServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProjectGroup info)
        {

           // tb_ProjectGroupValidator validator = new tb_ProjectGroupValidator();
           tb_ProjectGroupValidator validator = _appContext.GetRequiredService<tb_ProjectGroupValidator>();
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
        public async Task<ReturnResults<tb_ProjectGroup>> SaveOrUpdate(tb_ProjectGroup entity)
        {
            ReturnResults<tb_ProjectGroup> rr = new ReturnResults<tb_ProjectGroup>();
            tb_ProjectGroup Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProjectGroup_ID > 0)
                {
                    bool rs = await _tb_ProjectGroupServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProjectGroupServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(entity);
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
            tb_ProjectGroup entity = model as tb_ProjectGroup;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProjectGroup_ID > 0)
                {
                    bool rs = await _tb_ProjectGroupServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProjectGroupServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(entity);
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
            List<T> list = await _tb_ProjectGroupServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProjectGroup entity = item as tb_ProjectGroup;
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
            List<T> list = await _tb_ProjectGroupServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProjectGroup entity = item as tb_ProjectGroup;
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
            tb_ProjectGroup entity = model as tb_ProjectGroup;
            bool rs = await _tb_ProjectGroupServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroup>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProjectGroup> entitys = models as List<tb_ProjectGroup>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProjectGroup>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ProjectGroup_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroup>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProjectGroupValidator validator = new tb_ProjectGroupValidator();
           tb_ProjectGroupValidator validator = _appContext.GetRequiredService<tb_ProjectGroupValidator>();
            ValidationResult results = validator.Validate(info as tb_ProjectGroup);
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

                tb_ProjectGroup entity = model as tb_ProjectGroup;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ProjectGroup_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProjectGroup>(entity as tb_ProjectGroup)
                        .Include(m => m.tb_ProjectGroupEmployeeses)
                    .Include(m => m.tb_AS_AfterSaleDeliveries)
                    .Include(m => m.tb_FM_OtherExpenseDetails)
                    .Include(m => m.tb_AS_AfterSaleApplies)
                    .Include(m => m.tb_FM_ReceivablePayables)
                    .Include(m => m.tb_AS_RepairInStocks)
                    .Include(m => m.tb_FM_ProfitLosses)
                    .Include(m => m.tb_FM_PaymentRecordDetails)
                    .Include(m => m.tb_ProductionPlans)
                    .Include(m => m.tb_FM_ExpenseClaimDetails)
                    .Include(m => m.tb_AS_RepairOrders)
                    .Include(m => m.tb_ProjectGroupAccountMappers)
                    .Include(m => m.tb_SaleOutRes)
                    .Include(m => m.tb_PurEntries)
                    .Include(m => m.tb_FM_PriceAdjustments)
                    .Include(m => m.tb_EOP_WaterStorages)
                    .Include(m => m.tb_FM_PreReceivedPayments)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProjectGroup>(entity as tb_ProjectGroup)
                .Include(m => m.tb_ProjectGroupEmployeeses)
                .Include(m => m.tb_AS_AfterSaleDeliveries)
                .Include(m => m.tb_FM_OtherExpenseDetails)
                .Include(m => m.tb_AS_AfterSaleApplies)
                .Include(m => m.tb_FM_ReceivablePayables)
                .Include(m => m.tb_AS_RepairInStocks)
                .Include(m => m.tb_FM_ProfitLosses)
                .Include(m => m.tb_FM_PaymentRecordDetails)
                .Include(m => m.tb_ProductionPlans)
                .Include(m => m.tb_FM_ExpenseClaimDetails)
                .Include(m => m.tb_AS_RepairOrders)
                .Include(m => m.tb_ProjectGroupAccountMappers)
                .Include(m => m.tb_SaleOutRes)
                .Include(m => m.tb_PurEntries)
                .Include(m => m.tb_FM_PriceAdjustments)
                .Include(m => m.tb_EOP_WaterStorages)
                .Include(m => m.tb_FM_PreReceivedPayments)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ProjectGroup_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroup>()
                                .Includes(m => m.tb_ProjectGroupEmployeeses)
                        .Includes(m => m.tb_AS_AfterSaleDeliveries)
                        .Includes(m => m.tb_FM_OtherExpenseDetails)
                        .Includes(m => m.tb_AS_AfterSaleApplies)
                        .Includes(m => m.tb_FM_ReceivablePayables)
                        .Includes(m => m.tb_AS_RepairInStocks)
                        .Includes(m => m.tb_FM_ProfitLosses)
                        .Includes(m => m.tb_FM_PaymentRecordDetails)
                        .Includes(m => m.tb_ProductionPlans)
                        .Includes(m => m.tb_FM_ExpenseClaimDetails)
                        .Includes(m => m.tb_AS_RepairOrders)
                        .Includes(m => m.tb_ProjectGroupAccountMappers)
                        .Includes(m => m.tb_SaleOutRes)
                        .Includes(m => m.tb_PurEntries)
                        .Includes(m => m.tb_FM_PriceAdjustments)
                        .Includes(m => m.tb_EOP_WaterStorages)
                        .Includes(m => m.tb_FM_PreReceivedPayments)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProjectGroup entity = model as tb_ProjectGroup;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProjectGroup>(m => m.ProjectGroup_ID== entity.ProjectGroup_ID)
                                .Include(m => m.tb_ProjectGroupEmployeeses)
                        .Include(m => m.tb_AS_AfterSaleDeliveries)
                        .Include(m => m.tb_FM_OtherExpenseDetails)
                        .Include(m => m.tb_AS_AfterSaleApplies)
                        .Include(m => m.tb_FM_ReceivablePayables)
                        .Include(m => m.tb_AS_RepairInStocks)
                        .Include(m => m.tb_FM_ProfitLosses)
                        .Include(m => m.tb_FM_PaymentRecordDetails)
                        .Include(m => m.tb_ProductionPlans)
                        .Include(m => m.tb_FM_ExpenseClaimDetails)
                        .Include(m => m.tb_AS_RepairOrders)
                        .Include(m => m.tb_ProjectGroupAccountMappers)
                        .Include(m => m.tb_SaleOutRes)
                        .Include(m => m.tb_PurEntries)
                        .Include(m => m.tb_FM_PriceAdjustments)
                        .Include(m => m.tb_EOP_WaterStorages)
                        .Include(m => m.tb_FM_PreReceivedPayments)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProjectGroup AddReEntity(tb_ProjectGroup entity)
        {
            tb_ProjectGroup AddEntity =  _tb_ProjectGroupServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProjectGroup> AddReEntityAsync(tb_ProjectGroup entity)
        {
            tb_ProjectGroup AddEntity = await _tb_ProjectGroupServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProjectGroup entity)
        {
            long id = await _tb_ProjectGroupServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProjectGroup> infos)
        {
            List<long> ids = await _tb_ProjectGroupServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProjectGroup entity)
        {
            bool rs = await _tb_ProjectGroupServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroup>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProjectGroup entity)
        {
            bool rs = await _tb_ProjectGroupServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProjectGroupServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroup>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProjectGroupServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroup>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProjectGroup>> QueryAsync()
        {
            List<tb_ProjectGroup> list = await  _tb_ProjectGroupServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(list);
            return list;
        }
        
        public virtual List<tb_ProjectGroup> Query()
        {
            List<tb_ProjectGroup> list =  _tb_ProjectGroupServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(list);
            return list;
        }
        
        public virtual List<tb_ProjectGroup> Query(string wheresql)
        {
            List<tb_ProjectGroup> list =  _tb_ProjectGroupServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProjectGroup>> QueryAsync(string wheresql) 
        {
            List<tb_ProjectGroup> list = await _tb_ProjectGroupServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProjectGroup>> QueryAsync(Expression<Func<tb_ProjectGroup, bool>> exp)
        {
            List<tb_ProjectGroup> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroup>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProjectGroup>> QueryByNavAsync()
        {
            List<tb_ProjectGroup> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroup>()
                               .Includes(t => t.tb_department )
                                            .Includes(t => t.tb_ProjectGroupEmployeeses )
                                .Includes(t => t.tb_AS_AfterSaleDeliveries )
                                .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_AS_AfterSaleApplies )
                                .Includes(t => t.tb_FM_ReceivablePayables )
                                .Includes(t => t.tb_AS_RepairInStocks )
                                .Includes(t => t.tb_FM_ProfitLosses )
                                .Includes(t => t.tb_FM_PaymentRecordDetails )
                                .Includes(t => t.tb_ProductionPlans )
                                .Includes(t => t.tb_FM_ExpenseClaimDetails )
                                .Includes(t => t.tb_AS_RepairOrders )
                                .Includes(t => t.tb_ProjectGroupAccountMappers )
                                .Includes(t => t.tb_SaleOutRes )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_FM_PriceAdjustments )
                                .Includes(t => t.tb_EOP_WaterStorages )
                                .Includes(t => t.tb_FM_PreReceivedPayments )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProjectGroup>> QueryByNavAsync(Expression<Func<tb_ProjectGroup, bool>> exp)
        {
            List<tb_ProjectGroup> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroup>().Where(exp)
                               .Includes(t => t.tb_department )
                                            .Includes(t => t.tb_ProjectGroupEmployeeses )
                                .Includes(t => t.tb_AS_AfterSaleDeliveries )
                                .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_AS_AfterSaleApplies )
                                .Includes(t => t.tb_FM_ReceivablePayables )
                                .Includes(t => t.tb_AS_RepairInStocks )
                                .Includes(t => t.tb_FM_ProfitLosses )
                                .Includes(t => t.tb_FM_PaymentRecordDetails )
                                .Includes(t => t.tb_ProductionPlans )
                                .Includes(t => t.tb_FM_ExpenseClaimDetails )
                                .Includes(t => t.tb_AS_RepairOrders )
                                .Includes(t => t.tb_ProjectGroupAccountMappers )
                                .Includes(t => t.tb_SaleOutRes )
                                .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_FM_PriceAdjustments )
                                .Includes(t => t.tb_EOP_WaterStorages )
                                .Includes(t => t.tb_FM_PreReceivedPayments )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProjectGroup> QueryByNav(Expression<Func<tb_ProjectGroup, bool>> exp)
        {
            List<tb_ProjectGroup> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroup>().Where(exp)
                            .Includes(t => t.tb_department )
                                        .Includes(t => t.tb_ProjectGroupEmployeeses )
                            .Includes(t => t.tb_AS_AfterSaleDeliveries )
                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_AS_AfterSaleApplies )
                            .Includes(t => t.tb_FM_ReceivablePayables )
                            .Includes(t => t.tb_AS_RepairInStocks )
                            .Includes(t => t.tb_FM_ProfitLosses )
                            .Includes(t => t.tb_FM_PaymentRecordDetails )
                            .Includes(t => t.tb_ProductionPlans )
                            .Includes(t => t.tb_FM_ExpenseClaimDetails )
                            .Includes(t => t.tb_AS_RepairOrders )
                            .Includes(t => t.tb_ProjectGroupAccountMappers )
                            .Includes(t => t.tb_SaleOutRes )
                            .Includes(t => t.tb_PurEntries )
                            .Includes(t => t.tb_FM_PriceAdjustments )
                            .Includes(t => t.tb_EOP_WaterStorages )
                            .Includes(t => t.tb_FM_PreReceivedPayments )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProjectGroup>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroup>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProjectGroupServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProjectGroup entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroup>().Where(w => w.ProjectGroup_ID == (long)id)
                             .Includes(t => t.tb_department )
                        

                                            .Includes(t => t.tb_ProjectGroupEmployeeses )
                                            .Includes(t => t.tb_AS_AfterSaleDeliveries )
                                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                                            .Includes(t => t.tb_AS_AfterSaleApplies )
                                            .Includes(t => t.tb_FM_ReceivablePayables )
                                            .Includes(t => t.tb_AS_RepairInStocks )
                                            .Includes(t => t.tb_FM_ProfitLosses )
                                            .Includes(t => t.tb_FM_PaymentRecordDetails )
                                            .Includes(t => t.tb_ProductionPlans )
                                            .Includes(t => t.tb_FM_ExpenseClaimDetails )
                                            .Includes(t => t.tb_AS_RepairOrders )
                                            .Includes(t => t.tb_ProjectGroupAccountMappers )
                                            .Includes(t => t.tb_SaleOutRes )
                                            .Includes(t => t.tb_PurEntries )
                                            .Includes(t => t.tb_FM_PriceAdjustments )
                                            .Includes(t => t.tb_EOP_WaterStorages )
                                            .Includes(t => t.tb_FM_PreReceivedPayments )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroup>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



