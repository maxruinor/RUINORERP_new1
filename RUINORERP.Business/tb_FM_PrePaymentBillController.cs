
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:08
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
    /// 预收预付单
    /// </summary>
    public partial class tb_FM_PrePaymentBillController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_PrePaymentBillServices _tb_FM_PrePaymentBillServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_PrePaymentBillController(ILogger<tb_FM_PrePaymentBillController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_PrePaymentBillServices tb_FM_PrePaymentBillServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_PrePaymentBillServices = tb_FM_PrePaymentBillServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_PrePaymentBill info)
        {

           // tb_FM_PrePaymentBillValidator validator = new tb_FM_PrePaymentBillValidator();
           tb_FM_PrePaymentBillValidator validator = _appContext.GetRequiredService<tb_FM_PrePaymentBillValidator>();
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
        public async Task<ReturnResults<tb_FM_PrePaymentBill>> SaveOrUpdate(tb_FM_PrePaymentBill entity)
        {
            ReturnResults<tb_FM_PrePaymentBill> rr = new ReturnResults<tb_FM_PrePaymentBill>();
            tb_FM_PrePaymentBill Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PrePaymentBill_id > 0)
                {
                    bool rs = await _tb_FM_PrePaymentBillServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_PrePaymentBillServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(entity);
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
            tb_FM_PrePaymentBill entity = model as tb_FM_PrePaymentBill;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PrePaymentBill_id > 0)
                {
                    bool rs = await _tb_FM_PrePaymentBillServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_PrePaymentBillServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(entity);
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
            List<T> list = await _tb_FM_PrePaymentBillServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_PrePaymentBill entity = item as tb_FM_PrePaymentBill;
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
            List<T> list = await _tb_FM_PrePaymentBillServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_PrePaymentBill entity = item as tb_FM_PrePaymentBill;
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
            tb_FM_PrePaymentBill entity = model as tb_FM_PrePaymentBill;
            bool rs = await _tb_FM_PrePaymentBillServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PrePaymentBill>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_PrePaymentBill> entitys = models as List<tb_FM_PrePaymentBill>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PrePaymentBill>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PrePaymentBill_id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PrePaymentBill>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_PrePaymentBillValidator validator = new tb_FM_PrePaymentBillValidator();
           tb_FM_PrePaymentBillValidator validator = _appContext.GetRequiredService<tb_FM_PrePaymentBillValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_PrePaymentBill);
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
                tb_FM_PrePaymentBill entity = model as tb_FM_PrePaymentBill;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PrePaymentBill_id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_PrePaymentBill>(entity as tb_FM_PrePaymentBill)
                        .Include(m => m.tb_FM_PaymentBills)
                    .Include(m => m.tb_FM_PrePaymentBillDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_PrePaymentBill>(entity as tb_FM_PrePaymentBill)
                .Include(m => m.tb_FM_PaymentBills)
                .Include(m => m.tb_FM_PrePaymentBillDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PrePaymentBill_id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePaymentBill>()
                                .Includes(m => m.tb_FM_PaymentBills)
                        .Includes(m => m.tb_FM_PrePaymentBillDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_PrePaymentBill entity = model as tb_FM_PrePaymentBill;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_PrePaymentBill>(m => m.PrePaymentBill_id== entity.PrePaymentBill_id)
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
        
        
        
        public tb_FM_PrePaymentBill AddReEntity(tb_FM_PrePaymentBill entity)
        {
            tb_FM_PrePaymentBill AddEntity =  _tb_FM_PrePaymentBillServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_PrePaymentBill> AddReEntityAsync(tb_FM_PrePaymentBill entity)
        {
            tb_FM_PrePaymentBill AddEntity = await _tb_FM_PrePaymentBillServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_PrePaymentBill entity)
        {
            long id = await _tb_FM_PrePaymentBillServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_PrePaymentBill> infos)
        {
            List<long> ids = await _tb_FM_PrePaymentBillServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_PrePaymentBill entity)
        {
            bool rs = await _tb_FM_PrePaymentBillServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PrePaymentBill>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_PrePaymentBill entity)
        {
            bool rs = await _tb_FM_PrePaymentBillServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_PrePaymentBillServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PrePaymentBill>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_PrePaymentBillServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PrePaymentBill>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_PrePaymentBill>> QueryAsync()
        {
            List<tb_FM_PrePaymentBill> list = await  _tb_FM_PrePaymentBillServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(list);
            return list;
        }
        
        public virtual List<tb_FM_PrePaymentBill> Query()
        {
            List<tb_FM_PrePaymentBill> list =  _tb_FM_PrePaymentBillServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(list);
            return list;
        }
        
        public virtual List<tb_FM_PrePaymentBill> Query(string wheresql)
        {
            List<tb_FM_PrePaymentBill> list =  _tb_FM_PrePaymentBillServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_PrePaymentBill>> QueryAsync(string wheresql) 
        {
            List<tb_FM_PrePaymentBill> list = await _tb_FM_PrePaymentBillServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PrePaymentBill>> QueryAsync(Expression<Func<tb_FM_PrePaymentBill, bool>> exp)
        {
            List<tb_FM_PrePaymentBill> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePaymentBill>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PrePaymentBill>> QueryByNavAsync()
        {
            List<tb_FM_PrePaymentBill> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePaymentBill>()
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_FM_PaymentBills )
                                .Includes(t => t.tb_FM_PrePaymentBillDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PrePaymentBill>> QueryByNavAsync(Expression<Func<tb_FM_PrePaymentBill, bool>> exp)
        {
            List<tb_FM_PrePaymentBill> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePaymentBill>().Where(exp)
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_FM_PaymentBills )
                                .Includes(t => t.tb_FM_PrePaymentBillDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_PrePaymentBill> QueryByNav(Expression<Func<tb_FM_PrePaymentBill, bool>> exp)
        {
            List<tb_FM_PrePaymentBill> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePaymentBill>().Where(exp)
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_FM_PaymentBills )
                            .Includes(t => t.tb_FM_PrePaymentBillDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_PrePaymentBill>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePaymentBill>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_PrePaymentBillServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_PrePaymentBill entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePaymentBill>().Where(w => w.PrePaymentBill_id == (long)id)
                             .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_FM_PaymentBills )
                            .Includes(t => t.tb_FM_PrePaymentBillDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePaymentBill>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



