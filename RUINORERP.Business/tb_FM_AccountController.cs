
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:04
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
    /// 账户管理，财务系统中使用
    /// </summary>
    public partial class tb_FM_AccountController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_AccountServices _tb_FM_AccountServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_AccountController(ILogger<tb_FM_AccountController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_AccountServices tb_FM_AccountServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_AccountServices = tb_FM_AccountServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_Account info)
        {

           // tb_FM_AccountValidator validator = new tb_FM_AccountValidator();
           tb_FM_AccountValidator validator = _appContext.GetRequiredService<tb_FM_AccountValidator>();
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
        public async Task<ReturnResults<tb_FM_Account>> SaveOrUpdate(tb_FM_Account entity)
        {
            ReturnResults<tb_FM_Account> rr = new ReturnResults<tb_FM_Account>();
            tb_FM_Account Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Account_id > 0)
                {
                    bool rs = await _tb_FM_AccountServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_AccountServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(entity);
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
            tb_FM_Account entity = model as tb_FM_Account;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Account_id > 0)
                {
                    bool rs = await _tb_FM_AccountServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_AccountServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(entity);
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
            List<T> list = await _tb_FM_AccountServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_Account entity = item as tb_FM_Account;
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
            List<T> list = await _tb_FM_AccountServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_Account entity = item as tb_FM_Account;
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
            tb_FM_Account entity = model as tb_FM_Account;
            bool rs = await _tb_FM_AccountServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Account>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_Account> entitys = models as List<tb_FM_Account>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_Account>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Account_id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Account>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_AccountValidator validator = new tb_FM_AccountValidator();
           tb_FM_AccountValidator validator = _appContext.GetRequiredService<tb_FM_AccountValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_Account);
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
                tb_FM_Account entity = model as tb_FM_Account;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Account_id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_Account>(entity as tb_FM_Account)
                        .Include(m => m.tb_FM_OtherExpenseDetails)
                    .Include(m => m.tb_FM_ExpenseClaimDetails)
                    .Include(m => m.tb_FM_Initial_PayAndReceivables)
                    .Include(m => m.tb_FM_PaymentBills)
                    .Include(m => m.tb_FM_PrePaymentBillDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_Account>(entity as tb_FM_Account)
                .Include(m => m.tb_FM_OtherExpenseDetails)
                .Include(m => m.tb_FM_ExpenseClaimDetails)
                .Include(m => m.tb_FM_Initial_PayAndReceivables)
                .Include(m => m.tb_FM_PaymentBills)
                .Include(m => m.tb_FM_PrePaymentBillDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Account_id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>()
                                .Includes(m => m.tb_FM_OtherExpenseDetails)
                        .Includes(m => m.tb_FM_ExpenseClaimDetails)
                        .Includes(m => m.tb_FM_Initial_PayAndReceivables)
                        .Includes(m => m.tb_FM_PaymentBills)
                        .Includes(m => m.tb_FM_PrePaymentBillDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_Account entity = model as tb_FM_Account;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_Account>(m => m.Account_id== entity.Account_id)
                                .Include(m => m.tb_FM_OtherExpenseDetails)
                        .Include(m => m.tb_FM_ExpenseClaimDetails)
                        .Include(m => m.tb_FM_Initial_PayAndReceivables)
                        .Include(m => m.tb_FM_PaymentBills)
                        .Include(m => m.tb_FM_PrePaymentBillDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_Account AddReEntity(tb_FM_Account entity)
        {
            tb_FM_Account AddEntity =  _tb_FM_AccountServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_Account> AddReEntityAsync(tb_FM_Account entity)
        {
            tb_FM_Account AddEntity = await _tb_FM_AccountServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_Account entity)
        {
            long id = await _tb_FM_AccountServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_Account> infos)
        {
            List<long> ids = await _tb_FM_AccountServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_Account entity)
        {
            bool rs = await _tb_FM_AccountServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Account>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_Account entity)
        {
            bool rs = await _tb_FM_AccountServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_AccountServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Account>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_AccountServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Account>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_Account>> QueryAsync()
        {
            List<tb_FM_Account> list = await  _tb_FM_AccountServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(list);
            return list;
        }
        
        public virtual List<tb_FM_Account> Query()
        {
            List<tb_FM_Account> list =  _tb_FM_AccountServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(list);
            return list;
        }
        
        public virtual List<tb_FM_Account> Query(string wheresql)
        {
            List<tb_FM_Account> list =  _tb_FM_AccountServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_Account>> QueryAsync(string wheresql) 
        {
            List<tb_FM_Account> list = await _tb_FM_AccountServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_Account>> QueryAsync(Expression<Func<tb_FM_Account, bool>> exp)
        {
            List<tb_FM_Account> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_Account>> QueryByNavAsync()
        {
            List<tb_FM_Account> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>()
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_fm_subject )
                                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_FM_ExpenseClaimDetails )
                                .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                                .Includes(t => t.tb_FM_PaymentBills )
                                .Includes(t => t.tb_FM_PrePaymentBillDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_Account>> QueryByNavAsync(Expression<Func<tb_FM_Account, bool>> exp)
        {
            List<tb_FM_Account> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>().Where(exp)
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_fm_subject )
                                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_FM_ExpenseClaimDetails )
                                .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                                .Includes(t => t.tb_FM_PaymentBills )
                                .Includes(t => t.tb_FM_PrePaymentBillDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_Account> QueryByNav(Expression<Func<tb_FM_Account, bool>> exp)
        {
            List<tb_FM_Account> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>().Where(exp)
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_fm_subject )
                                        .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_FM_ExpenseClaimDetails )
                            .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                            .Includes(t => t.tb_FM_PaymentBills )
                            .Includes(t => t.tb_FM_PrePaymentBillDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_Account>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_AccountServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_Account entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>().Where(w => w.Account_id == (long)id)
                             .Includes(t => t.tb_department )
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_fm_subject )
                                        .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_FM_ExpenseClaimDetails )
                            .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                            .Includes(t => t.tb_FM_PaymentBills )
                            .Includes(t => t.tb_FM_PrePaymentBillDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_Account>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



