﻿// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/04/2025 18:02:21
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
    /// 对账单
    /// </summary>
    public partial class tb_FM_StatementController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_StatementServices _tb_FM_StatementServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_StatementController(ILogger<tb_FM_StatementController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_StatementServices tb_FM_StatementServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_StatementServices = tb_FM_StatementServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_Statement info)
        {

           // tb_FM_StatementValidator validator = new tb_FM_StatementValidator();
           tb_FM_StatementValidator validator = _appContext.GetRequiredService<tb_FM_StatementValidator>();
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
        public async Task<ReturnResults<tb_FM_Statement>> SaveOrUpdate(tb_FM_Statement entity)
        {
            ReturnResults<tb_FM_Statement> rr = new ReturnResults<tb_FM_Statement>();
            tb_FM_Statement Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.StatementId > 0)
                {
                    bool rs = await _tb_FM_StatementServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_StatementServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(entity);
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
            tb_FM_Statement entity = model as tb_FM_Statement;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.StatementId > 0)
                {
                    bool rs = await _tb_FM_StatementServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_StatementServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(entity);
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
            List<T> list = await _tb_FM_StatementServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_Statement entity = item as tb_FM_Statement;
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
            List<T> list = await _tb_FM_StatementServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_Statement entity = item as tb_FM_Statement;
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
            tb_FM_Statement entity = model as tb_FM_Statement;
            bool rs = await _tb_FM_StatementServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Statement>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_Statement> entitys = models as List<tb_FM_Statement>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_Statement>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.StatementId).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Statement>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_StatementValidator validator = new tb_FM_StatementValidator();
           tb_FM_StatementValidator validator = _appContext.GetRequiredService<tb_FM_StatementValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_Statement);
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

                tb_FM_Statement entity = model as tb_FM_Statement;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.StatementId > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_Statement>(entity as tb_FM_Statement)
                        .Include(m => m.tb_FM_StatementDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_Statement>(entity as tb_FM_Statement)
                .Include(m => m.tb_FM_StatementDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.StatementId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Statement>()
                                .Includes(m => m.tb_FM_StatementDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_Statement entity = model as tb_FM_Statement;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_Statement>(m => m.StatementId== entity.StatementId)
                                .Include(m => m.tb_FM_StatementDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_Statement AddReEntity(tb_FM_Statement entity)
        {
            tb_FM_Statement AddEntity =  _tb_FM_StatementServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_Statement> AddReEntityAsync(tb_FM_Statement entity)
        {
            tb_FM_Statement AddEntity = await _tb_FM_StatementServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_Statement entity)
        {
            long id = await _tb_FM_StatementServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_Statement> infos)
        {
            List<long> ids = await _tb_FM_StatementServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_Statement entity)
        {
            bool rs = await _tb_FM_StatementServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Statement>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_Statement entity)
        {
            bool rs = await _tb_FM_StatementServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_StatementServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Statement>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_StatementServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Statement>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_Statement>> QueryAsync()
        {
            List<tb_FM_Statement> list = await  _tb_FM_StatementServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(list);
            return list;
        }
        
        public virtual List<tb_FM_Statement> Query()
        {
            List<tb_FM_Statement> list =  _tb_FM_StatementServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(list);
            return list;
        }
        
        public virtual List<tb_FM_Statement> Query(string wheresql)
        {
            List<tb_FM_Statement> list =  _tb_FM_StatementServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_Statement>> QueryAsync(string wheresql) 
        {
            List<tb_FM_Statement> list = await _tb_FM_StatementServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_Statement>> QueryAsync(Expression<Func<tb_FM_Statement, bool>> exp)
        {
            List<tb_FM_Statement> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Statement>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_Statement>> QueryByNavAsync()
        {
            List<tb_FM_Statement> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Statement>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_payeeinfo )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_FM_StatementDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_Statement>> QueryByNavAsync(Expression<Func<tb_FM_Statement, bool>> exp)
        {
            List<tb_FM_Statement> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Statement>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_payeeinfo )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_FM_StatementDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_Statement> QueryByNav(Expression<Func<tb_FM_Statement, bool>> exp)
        {
            List<tb_FM_Statement> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Statement>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_payeeinfo )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_FM_StatementDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_Statement>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Statement>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_StatementServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_Statement entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Statement>().Where(w => w.StatementId == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_payeeinfo )
                            .Includes(t => t.tb_employee )
                        

                                            .Includes(t => t.tb_FM_StatementDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_Statement>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



