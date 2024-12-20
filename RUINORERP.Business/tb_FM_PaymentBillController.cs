
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:05
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
    /// 付款单 中有两种情况，1）如果有应收款，可以抵扣而少付款，如果有预付款也可以抵扣。
    /// </summary>
    public partial class tb_FM_PaymentBillController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_PaymentBillServices _tb_FM_PaymentBillServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_PaymentBillController(ILogger<tb_FM_PaymentBillController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_PaymentBillServices tb_FM_PaymentBillServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_PaymentBillServices = tb_FM_PaymentBillServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_PaymentBill info)
        {

           // tb_FM_PaymentBillValidator validator = new tb_FM_PaymentBillValidator();
           tb_FM_PaymentBillValidator validator = _appContext.GetRequiredService<tb_FM_PaymentBillValidator>();
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
        public async Task<ReturnResults<tb_FM_PaymentBill>> SaveOrUpdate(tb_FM_PaymentBill entity)
        {
            ReturnResults<tb_FM_PaymentBill> rr = new ReturnResults<tb_FM_PaymentBill>();
            tb_FM_PaymentBill Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Payment_id > 0)
                {
                    bool rs = await _tb_FM_PaymentBillServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_PaymentBillServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(entity);
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
            tb_FM_PaymentBill entity = model as tb_FM_PaymentBill;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Payment_id > 0)
                {
                    bool rs = await _tb_FM_PaymentBillServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_PaymentBillServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(entity);
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
            List<T> list = await _tb_FM_PaymentBillServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_PaymentBill entity = item as tb_FM_PaymentBill;
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
            List<T> list = await _tb_FM_PaymentBillServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_PaymentBill entity = item as tb_FM_PaymentBill;
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
            tb_FM_PaymentBill entity = model as tb_FM_PaymentBill;
            bool rs = await _tb_FM_PaymentBillServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentBill>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_PaymentBill> entitys = models as List<tb_FM_PaymentBill>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PaymentBill>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Payment_id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentBill>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_PaymentBillValidator validator = new tb_FM_PaymentBillValidator();
           tb_FM_PaymentBillValidator validator = _appContext.GetRequiredService<tb_FM_PaymentBillValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_PaymentBill);
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
                tb_FM_PaymentBill entity = model as tb_FM_PaymentBill;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Payment_id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_PaymentBill>(entity as tb_FM_PaymentBill)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_PaymentBill>(entity as tb_FM_PaymentBill)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Payment_id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentBill>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_PaymentBill entity = model as tb_FM_PaymentBill;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_PaymentBill>(m => m.Payment_id== entity.Payment_id)
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_PaymentBill AddReEntity(tb_FM_PaymentBill entity)
        {
            tb_FM_PaymentBill AddEntity =  _tb_FM_PaymentBillServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_PaymentBill> AddReEntityAsync(tb_FM_PaymentBill entity)
        {
            tb_FM_PaymentBill AddEntity = await _tb_FM_PaymentBillServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_PaymentBill entity)
        {
            long id = await _tb_FM_PaymentBillServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_PaymentBill> infos)
        {
            List<long> ids = await _tb_FM_PaymentBillServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_PaymentBill entity)
        {
            bool rs = await _tb_FM_PaymentBillServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentBill>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_PaymentBill entity)
        {
            bool rs = await _tb_FM_PaymentBillServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_PaymentBillServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentBill>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_PaymentBillServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentBill>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_PaymentBill>> QueryAsync()
        {
            List<tb_FM_PaymentBill> list = await  _tb_FM_PaymentBillServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(list);
            return list;
        }
        
        public virtual List<tb_FM_PaymentBill> Query()
        {
            List<tb_FM_PaymentBill> list =  _tb_FM_PaymentBillServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(list);
            return list;
        }
        
        public virtual List<tb_FM_PaymentBill> Query(string wheresql)
        {
            List<tb_FM_PaymentBill> list =  _tb_FM_PaymentBillServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_PaymentBill>> QueryAsync(string wheresql) 
        {
            List<tb_FM_PaymentBill> list = await _tb_FM_PaymentBillServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PaymentBill>> QueryAsync(Expression<Func<tb_FM_PaymentBill, bool>> exp)
        {
            List<tb_FM_PaymentBill> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentBill>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PaymentBill>> QueryByNavAsync()
        {
            List<tb_FM_PaymentBill> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentBill>()
                               .Includes(t => t.tb_fm_prepaymentbill )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PaymentBill>> QueryByNavAsync(Expression<Func<tb_FM_PaymentBill, bool>> exp)
        {
            List<tb_FM_PaymentBill> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentBill>().Where(exp)
                               .Includes(t => t.tb_fm_prepaymentbill )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_PaymentBill> QueryByNav(Expression<Func<tb_FM_PaymentBill, bool>> exp)
        {
            List<tb_FM_PaymentBill> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentBill>().Where(exp)
                            .Includes(t => t.tb_fm_prepaymentbill )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_PaymentBill>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentBill>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_PaymentBillServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_PaymentBill entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentBill>().Where(w => w.Payment_id == (long)id)
                             .Includes(t => t.tb_fm_prepaymentbill )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentBill>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



