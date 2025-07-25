﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:26:55
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
    /// 收款信息，供应商报销人的收款账号
    /// </summary>
    public partial class tb_FM_PayeeInfoController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_PayeeInfoServices _tb_FM_PayeeInfoServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_PayeeInfoController(ILogger<tb_FM_PayeeInfoController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_PayeeInfoServices tb_FM_PayeeInfoServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_PayeeInfoServices = tb_FM_PayeeInfoServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_PayeeInfo info)
        {

           // tb_FM_PayeeInfoValidator validator = new tb_FM_PayeeInfoValidator();
           tb_FM_PayeeInfoValidator validator = _appContext.GetRequiredService<tb_FM_PayeeInfoValidator>();
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
        public async Task<ReturnResults<tb_FM_PayeeInfo>> SaveOrUpdate(tb_FM_PayeeInfo entity)
        {
            ReturnResults<tb_FM_PayeeInfo> rr = new ReturnResults<tb_FM_PayeeInfo>();
            tb_FM_PayeeInfo Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PayeeInfoID > 0)
                {
                    bool rs = await _tb_FM_PayeeInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_PayeeInfoServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(entity);
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
            tb_FM_PayeeInfo entity = model as tb_FM_PayeeInfo;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PayeeInfoID > 0)
                {
                    bool rs = await _tb_FM_PayeeInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_PayeeInfoServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(entity);
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
            List<T> list = await _tb_FM_PayeeInfoServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_PayeeInfo entity = item as tb_FM_PayeeInfo;
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
            List<T> list = await _tb_FM_PayeeInfoServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_PayeeInfo entity = item as tb_FM_PayeeInfo;
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
            tb_FM_PayeeInfo entity = model as tb_FM_PayeeInfo;
            bool rs = await _tb_FM_PayeeInfoServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PayeeInfo>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_PayeeInfo> entitys = models as List<tb_FM_PayeeInfo>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PayeeInfo>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PayeeInfoID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PayeeInfo>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_PayeeInfoValidator validator = new tb_FM_PayeeInfoValidator();
           tb_FM_PayeeInfoValidator validator = _appContext.GetRequiredService<tb_FM_PayeeInfoValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_PayeeInfo);
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

                tb_FM_PayeeInfo entity = model as tb_FM_PayeeInfo;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PayeeInfoID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_PayeeInfo>(entity as tb_FM_PayeeInfo)
                        .Include(m => m.tb_FM_ReceivablePayables)
                    .Include(m => m.tb_FM_PaymentRecords)
                    .Include(m => m.tb_FM_PaymentApplications)
                    .Include(m => m.tb_FM_ExpenseClaims)
                    .Include(m => m.tb_FM_PreReceivedPayments)
                    .Include(m => m.tb_FM_Statements)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_PayeeInfo>(entity as tb_FM_PayeeInfo)
                .Include(m => m.tb_FM_ReceivablePayables)
                .Include(m => m.tb_FM_PaymentRecords)
                .Include(m => m.tb_FM_PaymentApplications)
                .Include(m => m.tb_FM_ExpenseClaims)
                .Include(m => m.tb_FM_PreReceivedPayments)
                .Include(m => m.tb_FM_Statements)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PayeeInfoID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PayeeInfo>()
                                .Includes(m => m.tb_FM_ReceivablePayables)
                        .Includes(m => m.tb_FM_PaymentRecords)
                        .Includes(m => m.tb_FM_PaymentApplications)
                        .Includes(m => m.tb_FM_ExpenseClaims)
                        .Includes(m => m.tb_FM_PreReceivedPayments)
                        .Includes(m => m.tb_FM_Statements)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_PayeeInfo entity = model as tb_FM_PayeeInfo;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_PayeeInfo>(m => m.PayeeInfoID== entity.PayeeInfoID)
                                .Include(m => m.tb_FM_ReceivablePayables)
                        .Include(m => m.tb_FM_PaymentRecords)
                        .Include(m => m.tb_FM_PaymentApplications)
                        .Include(m => m.tb_FM_ExpenseClaims)
                        .Include(m => m.tb_FM_PreReceivedPayments)
                        .Include(m => m.tb_FM_Statements)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_PayeeInfo AddReEntity(tb_FM_PayeeInfo entity)
        {
            tb_FM_PayeeInfo AddEntity =  _tb_FM_PayeeInfoServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_PayeeInfo> AddReEntityAsync(tb_FM_PayeeInfo entity)
        {
            tb_FM_PayeeInfo AddEntity = await _tb_FM_PayeeInfoServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_PayeeInfo entity)
        {
            long id = await _tb_FM_PayeeInfoServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_PayeeInfo> infos)
        {
            List<long> ids = await _tb_FM_PayeeInfoServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_PayeeInfo entity)
        {
            bool rs = await _tb_FM_PayeeInfoServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PayeeInfo>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_PayeeInfo entity)
        {
            bool rs = await _tb_FM_PayeeInfoServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_PayeeInfoServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PayeeInfo>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_PayeeInfoServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PayeeInfo>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_PayeeInfo>> QueryAsync()
        {
            List<tb_FM_PayeeInfo> list = await  _tb_FM_PayeeInfoServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(list);
            return list;
        }
        
        public virtual List<tb_FM_PayeeInfo> Query()
        {
            List<tb_FM_PayeeInfo> list =  _tb_FM_PayeeInfoServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(list);
            return list;
        }
        
        public virtual List<tb_FM_PayeeInfo> Query(string wheresql)
        {
            List<tb_FM_PayeeInfo> list =  _tb_FM_PayeeInfoServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_PayeeInfo>> QueryAsync(string wheresql) 
        {
            List<tb_FM_PayeeInfo> list = await _tb_FM_PayeeInfoServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PayeeInfo>> QueryAsync(Expression<Func<tb_FM_PayeeInfo, bool>> exp)
        {
            List<tb_FM_PayeeInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PayeeInfo>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PayeeInfo>> QueryByNavAsync()
        {
            List<tb_FM_PayeeInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PayeeInfo>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_FM_ReceivablePayables )
                                .Includes(t => t.tb_FM_PaymentRecords )
                                .Includes(t => t.tb_FM_PaymentApplications )
                                .Includes(t => t.tb_FM_ExpenseClaims )
                                .Includes(t => t.tb_FM_PreReceivedPayments )
                                .Includes(t => t.tb_FM_Statements )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PayeeInfo>> QueryByNavAsync(Expression<Func<tb_FM_PayeeInfo, bool>> exp)
        {
            List<tb_FM_PayeeInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PayeeInfo>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_FM_ReceivablePayables )
                                .Includes(t => t.tb_FM_PaymentRecords )
                                .Includes(t => t.tb_FM_PaymentApplications )
                                .Includes(t => t.tb_FM_ExpenseClaims )
                                .Includes(t => t.tb_FM_PreReceivedPayments )
                                .Includes(t => t.tb_FM_Statements )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_PayeeInfo> QueryByNav(Expression<Func<tb_FM_PayeeInfo, bool>> exp)
        {
            List<tb_FM_PayeeInfo> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PayeeInfo>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_FM_ReceivablePayables )
                            .Includes(t => t.tb_FM_PaymentRecords )
                            .Includes(t => t.tb_FM_PaymentApplications )
                            .Includes(t => t.tb_FM_ExpenseClaims )
                            .Includes(t => t.tb_FM_PreReceivedPayments )
                            .Includes(t => t.tb_FM_Statements )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_PayeeInfo>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PayeeInfo>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_PayeeInfoServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_PayeeInfo entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PayeeInfo>().Where(w => w.PayeeInfoID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_FM_ReceivablePayables )
                            .Includes(t => t.tb_FM_PaymentRecords )
                            .Includes(t => t.tb_FM_PaymentApplications )
                            .Includes(t => t.tb_FM_ExpenseClaims )
                            .Includes(t => t.tb_FM_PreReceivedPayments )
                            .Includes(t => t.tb_FM_Statements )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_PayeeInfo>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



