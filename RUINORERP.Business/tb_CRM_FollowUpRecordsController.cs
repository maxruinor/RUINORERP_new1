﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 21:23:58
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
    /// 跟进记录表
    /// </summary>
    public partial class tb_CRM_FollowUpRecordsController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CRM_FollowUpRecordsServices _tb_CRM_FollowUpRecordsServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_CRM_FollowUpRecordsController(ILogger<tb_CRM_FollowUpRecordsController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CRM_FollowUpRecordsServices tb_CRM_FollowUpRecordsServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CRM_FollowUpRecordsServices = tb_CRM_FollowUpRecordsServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_CRM_FollowUpRecords info)
        {
            tb_CRM_FollowUpRecordsValidator validator = new tb_CRM_FollowUpRecordsValidator();
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
        public async Task<ReturnResults<tb_CRM_FollowUpRecords>> SaveOrUpdate(tb_CRM_FollowUpRecords entity)
        {
            ReturnResults<tb_CRM_FollowUpRecords> rr = new ReturnResults<tb_CRM_FollowUpRecords>();
            tb_CRM_FollowUpRecords Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RecordID > 0)
                {
                    bool rs = await _tb_CRM_FollowUpRecordsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CRM_FollowUpRecordsServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(entity);
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
            tb_CRM_FollowUpRecords entity = model as tb_CRM_FollowUpRecords;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RecordID > 0)
                {
                    bool rs = await _tb_CRM_FollowUpRecordsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CRM_FollowUpRecordsServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(entity);
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
            List<T> list = await _tb_CRM_FollowUpRecordsServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_CRM_FollowUpRecords entity = item as tb_CRM_FollowUpRecords;
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
            List<T> list = await _tb_CRM_FollowUpRecordsServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_CRM_FollowUpRecords entity = item as tb_CRM_FollowUpRecords;
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
            tb_CRM_FollowUpRecords entity = model as tb_CRM_FollowUpRecords;
            bool rs = await _tb_CRM_FollowUpRecordsServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_FollowUpRecords>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_CRM_FollowUpRecords> entitys = models as List<tb_CRM_FollowUpRecords>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_CRM_FollowUpRecords>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.RecordID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_FollowUpRecords>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_CRM_FollowUpRecordsValidator validator = new tb_CRM_FollowUpRecordsValidator();
            ValidationResult results = validator.Validate(info as tb_CRM_FollowUpRecords);
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
                tb_CRM_FollowUpRecords entity = model as tb_CRM_FollowUpRecords;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.RecordID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_CRM_FollowUpRecords>(entity as tb_CRM_FollowUpRecords)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_CRM_FollowUpRecords>(entity as tb_CRM_FollowUpRecords)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.RecordID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_FollowUpRecords>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CRM_FollowUpRecords entity = model as tb_CRM_FollowUpRecords;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CRM_FollowUpRecords>(m => m.RecordID== entity.RecordID)
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
        
        
        
        public tb_CRM_FollowUpRecords AddReEntity(tb_CRM_FollowUpRecords entity)
        {
            tb_CRM_FollowUpRecords AddEntity =  _tb_CRM_FollowUpRecordsServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CRM_FollowUpRecords> AddReEntityAsync(tb_CRM_FollowUpRecords entity)
        {
            tb_CRM_FollowUpRecords AddEntity = await _tb_CRM_FollowUpRecordsServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CRM_FollowUpRecords entity)
        {
            long id = await _tb_CRM_FollowUpRecordsServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CRM_FollowUpRecords> infos)
        {
            List<long> ids = await _tb_CRM_FollowUpRecordsServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CRM_FollowUpRecords entity)
        {
            bool rs = await _tb_CRM_FollowUpRecordsServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_FollowUpRecords>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CRM_FollowUpRecords entity)
        {
            bool rs = await _tb_CRM_FollowUpRecordsServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CRM_FollowUpRecordsServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_FollowUpRecords>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CRM_FollowUpRecordsServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_FollowUpRecords>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_CRM_FollowUpRecords>> QueryAsync()
        {
            List<tb_CRM_FollowUpRecords> list = await  _tb_CRM_FollowUpRecordsServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(list);
            return list;
        }
        
        public virtual List<tb_CRM_FollowUpRecords> Query()
        {
            List<tb_CRM_FollowUpRecords> list =  _tb_CRM_FollowUpRecordsServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(list);
            return list;
        }
        
        public virtual List<tb_CRM_FollowUpRecords> Query(string wheresql)
        {
            List<tb_CRM_FollowUpRecords> list =  _tb_CRM_FollowUpRecordsServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CRM_FollowUpRecords>> QueryAsync(string wheresql) 
        {
            List<tb_CRM_FollowUpRecords> list = await _tb_CRM_FollowUpRecordsServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_CRM_FollowUpRecords>> QueryAsync(Expression<Func<tb_CRM_FollowUpRecords, bool>> exp)
        {
            List<tb_CRM_FollowUpRecords> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_FollowUpRecords>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CRM_FollowUpRecords>> QueryByNavAsync()
        {
            List<tb_CRM_FollowUpRecords> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_FollowUpRecords>()
                               .Includes(t => t.tb_crm_followupplans )
                               .Includes(t => t.tb_crm_followupplans )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_crm_customer )
                               .Includes(t => t.tb_crm_leads )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CRM_FollowUpRecords>> QueryByNavAsync(Expression<Func<tb_CRM_FollowUpRecords, bool>> exp)
        {
            List<tb_CRM_FollowUpRecords> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_FollowUpRecords>().Where(exp)
                               .Includes(t => t.tb_crm_followupplans )
                               .Includes(t => t.tb_crm_followupplans )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_crm_customer )
                               .Includes(t => t.tb_crm_leads )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CRM_FollowUpRecords> QueryByNav(Expression<Func<tb_CRM_FollowUpRecords, bool>> exp)
        {
            List<tb_CRM_FollowUpRecords> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_FollowUpRecords>().Where(exp)
                            .Includes(t => t.tb_crm_followupplans )
                            .Includes(t => t.tb_crm_followupplans )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_crm_customer )
                            .Includes(t => t.tb_crm_leads )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CRM_FollowUpRecords>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_FollowUpRecords>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CRM_FollowUpRecordsServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_CRM_FollowUpRecords entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_FollowUpRecords>().Where(w => w.RecordID == (long)id)
                             .Includes(t => t.tb_crm_followupplans )
                            .Includes(t => t.tb_crm_followupplans )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_crm_customer )
                            .Includes(t => t.tb_crm_leads )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_CRM_FollowUpRecords>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}


