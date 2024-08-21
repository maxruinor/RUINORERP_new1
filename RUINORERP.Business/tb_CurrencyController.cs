
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2024 10:31:32
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
    /// 币别资料表
    /// </summary>
    public partial class tb_CurrencyController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CurrencyServices _tb_CurrencyServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_CurrencyController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CurrencyServices tb_CurrencyServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CurrencyServices = tb_CurrencyServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_Currency info)
        {
            tb_CurrencyValidator validator = new tb_CurrencyValidator();
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
        public async Task<ReturnResults<tb_Currency>> SaveOrUpdate(tb_Currency entity)
        {
            ReturnResults<tb_Currency> rr = new ReturnResults<tb_Currency>();
            tb_Currency Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Currency_ID > 0)
                {
                    bool rs = await _tb_CurrencyServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Currency>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CurrencyServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Currency>(entity);
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
            tb_Currency entity = model as tb_Currency;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Currency_ID > 0)
                {
                    bool rs = await _tb_CurrencyServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Currency>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CurrencyServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Currency>(entity);
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
            List<T> list = await _tb_CurrencyServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Currency entity = item as tb_Currency;
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
            List<T> list = await _tb_CurrencyServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Currency entity = item as tb_Currency;
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
            tb_Currency entity = model as tb_Currency;
            bool rs = await _tb_CurrencyServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Currency>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Currency> entitys = models as List<tb_Currency>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Currency>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Currency_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Currency>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_CurrencyValidator validator = new tb_CurrencyValidator();
            ValidationResult results = validator.Validate(info as tb_Currency);
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
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_Currency entity = model as tb_Currency;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.Currency_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Currency>(entity as tb_Currency)
                        .Include(m => m.tb_FM_OtherExpenses)
                    .Include(m => m.tb_FM_Accounts)
                    .Include(m => m.tb_FM_PaymentBills)
                    .Include(m => m.tb_FM_PrePaymentBillDetails)
                    .Include(m => m.tb_FM_ExpenseClaims)
                    .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Currency>(entity as tb_Currency)
                .Include(m => m.tb_FM_OtherExpenses)
                .Include(m => m.tb_FM_Accounts)
                .Include(m => m.tb_FM_PaymentBills)
                .Include(m => m.tb_FM_PrePaymentBillDetails)
                .Include(m => m.tb_FM_ExpenseClaims)
                        .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Currency_ID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                //出错后，取消生成的ID等值
                command.Undo();
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                //_logger.Error("BaseSaveOrUpdateWithChild事务回滚");
                // rr.ErrorMsg = "事务回滚=>" + ex.Message;
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Currency>()
                                .Includes(m => m.tb_FM_OtherExpenses)
                        .Includes(m => m.tb_FM_Accounts)
                        .Includes(m => m.tb_FM_PaymentBills)
                        .Includes(m => m.tb_FM_PrePaymentBillDetails)
                        .Includes(m => m.tb_FM_ExpenseClaims)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Currency entity = model as tb_Currency;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Currency>(m => m.Currency_ID== entity.Currency_ID)
                                .Include(m => m.tb_FM_OtherExpenses)
                        .Include(m => m.tb_FM_Accounts)
                        .Include(m => m.tb_FM_PaymentBills)
                        .Include(m => m.tb_FM_PrePaymentBillDetails)
                        .Include(m => m.tb_FM_ExpenseClaims)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Currency AddReEntity(tb_Currency entity)
        {
            tb_Currency AddEntity =  _tb_CurrencyServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Currency> AddReEntityAsync(tb_Currency entity)
        {
            tb_Currency AddEntity = await _tb_CurrencyServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Currency entity)
        {
            long id = await _tb_CurrencyServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Currency>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Currency> infos)
        {
            List<long> ids = await _tb_CurrencyServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Currency>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Currency entity)
        {
            bool rs = await _tb_CurrencyServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Currency>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Currency entity)
        {
            bool rs = await _tb_CurrencyServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Currency>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CurrencyServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Currency>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CurrencyServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Currency>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Currency>> QueryAsync()
        {
            List<tb_Currency> list = await  _tb_CurrencyServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(list);
            return list;
        }
        
        public virtual List<tb_Currency> Query()
        {
            List<tb_Currency> list =  _tb_CurrencyServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(list);
            return list;
        }
        
        public virtual List<tb_Currency> Query(string wheresql)
        {
            List<tb_Currency> list =  _tb_CurrencyServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Currency>> QueryAsync(string wheresql) 
        {
            List<tb_Currency> list = await _tb_CurrencyServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Currency>> QueryAsync(Expression<Func<tb_Currency, bool>> exp)
        {
            List<tb_Currency> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Currency>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Currency>> QueryByNavAsync()
        {
            List<tb_Currency> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Currency>()
                                            .Includes(t => t.tb_FM_OtherExpenses )
                                .Includes(t => t.tb_FM_Accounts )
                                .Includes(t => t.tb_FM_PaymentBills )
                                .Includes(t => t.tb_FM_PrePaymentBillDetails )
                                .Includes(t => t.tb_FM_ExpenseClaims )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Currency>> QueryByNavAsync(Expression<Func<tb_Currency, bool>> exp)
        {
            List<tb_Currency> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Currency>().Where(exp)
                                            .Includes(t => t.tb_FM_OtherExpenses )
                                .Includes(t => t.tb_FM_Accounts )
                                .Includes(t => t.tb_FM_PaymentBills )
                                .Includes(t => t.tb_FM_PrePaymentBillDetails )
                                .Includes(t => t.tb_FM_ExpenseClaims )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Currency> QueryByNav(Expression<Func<tb_Currency, bool>> exp)
        {
            List<tb_Currency> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Currency>().Where(exp)
                                        .Includes(t => t.tb_FM_OtherExpenses )
                            .Includes(t => t.tb_FM_Accounts )
                            .Includes(t => t.tb_FM_PaymentBills )
                            .Includes(t => t.tb_FM_PrePaymentBillDetails )
                            .Includes(t => t.tb_FM_ExpenseClaims )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Currency>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Currency>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CurrencyServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Currency entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Currency>().Where(w => w.Currency_ID == (long)id)
                                         .Includes(t => t.tb_FM_OtherExpenses )
                            .Includes(t => t.tb_FM_Accounts )
                            .Includes(t => t.tb_FM_PaymentBills )
                            .Includes(t => t.tb_FM_PrePaymentBillDetails )
                            .Includes(t => t.tb_FM_ExpenseClaims )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Currency>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



