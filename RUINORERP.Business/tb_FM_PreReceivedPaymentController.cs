﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/25/2025 18:51:44
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
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PreReceivedPaymentController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_PreReceivedPaymentServices _tb_FM_PreReceivedPaymentServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_PreReceivedPaymentController(ILogger<tb_FM_PreReceivedPaymentController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_PreReceivedPaymentServices tb_FM_PreReceivedPaymentServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_PreReceivedPaymentServices = tb_FM_PreReceivedPaymentServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_PreReceivedPayment info)
        {

           // tb_FM_PreReceivedPaymentValidator validator = new tb_FM_PreReceivedPaymentValidator();
           tb_FM_PreReceivedPaymentValidator validator = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentValidator>();
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
        public async Task<ReturnResults<tb_FM_PreReceivedPayment>> SaveOrUpdate(tb_FM_PreReceivedPayment entity)
        {
            ReturnResults<tb_FM_PreReceivedPayment> rr = new ReturnResults<tb_FM_PreReceivedPayment>();
            tb_FM_PreReceivedPayment Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PreRPID > 0)
                {
                    bool rs = await _tb_FM_PreReceivedPaymentServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_PreReceivedPaymentServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(entity);
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
            tb_FM_PreReceivedPayment entity = model as tb_FM_PreReceivedPayment;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PreRPID > 0)
                {
                    bool rs = await _tb_FM_PreReceivedPaymentServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_PreReceivedPaymentServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(entity);
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
            List<T> list = await _tb_FM_PreReceivedPaymentServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_PreReceivedPayment entity = item as tb_FM_PreReceivedPayment;
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
            List<T> list = await _tb_FM_PreReceivedPaymentServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_PreReceivedPayment entity = item as tb_FM_PreReceivedPayment;
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
            tb_FM_PreReceivedPayment entity = model as tb_FM_PreReceivedPayment;
            bool rs = await _tb_FM_PreReceivedPaymentServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PreReceivedPayment>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_PreReceivedPayment> entitys = models as List<tb_FM_PreReceivedPayment>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PreReceivedPayment>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PreRPID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PreReceivedPayment>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_PreReceivedPaymentValidator validator = new tb_FM_PreReceivedPaymentValidator();
           tb_FM_PreReceivedPaymentValidator validator = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_PreReceivedPayment);
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

                tb_FM_PreReceivedPayment entity = model as tb_FM_PreReceivedPayment;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PreRPID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity as tb_FM_PreReceivedPayment)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PreReceivedPayment>(entity as tb_FM_PreReceivedPayment)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PreRPID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_PreReceivedPayment entity = model as tb_FM_PreReceivedPayment;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_PreReceivedPayment>(m => m.PreRPID== entity.PreRPID)
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
        
        
        
        public tb_FM_PreReceivedPayment AddReEntity(tb_FM_PreReceivedPayment entity)
        {
            tb_FM_PreReceivedPayment AddEntity =  _tb_FM_PreReceivedPaymentServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_PreReceivedPayment> AddReEntityAsync(tb_FM_PreReceivedPayment entity)
        {
            tb_FM_PreReceivedPayment AddEntity = await _tb_FM_PreReceivedPaymentServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_PreReceivedPayment entity)
        {
            long id = await _tb_FM_PreReceivedPaymentServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_PreReceivedPayment> infos)
        {
            List<long> ids = await _tb_FM_PreReceivedPaymentServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_PreReceivedPayment entity)
        {
            bool rs = await _tb_FM_PreReceivedPaymentServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PreReceivedPayment>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_PreReceivedPayment entity)
        {
            bool rs = await _tb_FM_PreReceivedPaymentServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_PreReceivedPaymentServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PreReceivedPayment>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_PreReceivedPaymentServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PreReceivedPayment>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_PreReceivedPayment>> QueryAsync()
        {
            List<tb_FM_PreReceivedPayment> list = await  _tb_FM_PreReceivedPaymentServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(list);
            return list;
        }
        
        public virtual List<tb_FM_PreReceivedPayment> Query()
        {
            List<tb_FM_PreReceivedPayment> list =  _tb_FM_PreReceivedPaymentServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(list);
            return list;
        }
        
        public virtual List<tb_FM_PreReceivedPayment> Query(string wheresql)
        {
            List<tb_FM_PreReceivedPayment> list =  _tb_FM_PreReceivedPaymentServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_PreReceivedPayment>> QueryAsync(string wheresql) 
        {
            List<tb_FM_PreReceivedPayment> list = await _tb_FM_PreReceivedPaymentServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PreReceivedPayment>> QueryAsync(Expression<Func<tb_FM_PreReceivedPayment, bool>> exp)
        {
            List<tb_FM_PreReceivedPayment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PreReceivedPayment>> QueryByNavAsync()
        {
            List<tb_FM_PreReceivedPayment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_payeeinfo )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PreReceivedPayment>> QueryByNavAsync(Expression<Func<tb_FM_PreReceivedPayment, bool>> exp)
        {
            List<tb_FM_PreReceivedPayment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>().Where(exp)
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_payeeinfo )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_PreReceivedPayment> QueryByNav(Expression<Func<tb_FM_PreReceivedPayment, bool>> exp)
        {
            List<tb_FM_PreReceivedPayment> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>().Where(exp)
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_payeeinfo )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_PreReceivedPayment>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_PreReceivedPaymentServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_PreReceivedPayment entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>().Where(w => w.PreRPID == (long)id)
                             .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_payeeinfo )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_PreReceivedPayment>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



