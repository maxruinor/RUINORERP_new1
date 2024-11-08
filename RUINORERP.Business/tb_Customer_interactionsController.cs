﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/13/2023 17:38:13
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
    /// 客户交互表，CRM系统中使用      
    /// </summary>
    public partial class tb_Customer_interactionsController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_Customer_interactionsServices _tb_Customer_interactionsServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_Customer_interactionsController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_Customer_interactionsServices tb_Customer_interactionsServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_Customer_interactionsServices = tb_Customer_interactionsServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_Customer_interactions info)
        {
            tb_Customer_interactionsValidator validator = new tb_Customer_interactionsValidator();
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
        public async Task<ReturnResults<tb_Customer_interactions>> SaveOrUpdate(tb_Customer_interactions entity)
        {
            ReturnResults<tb_Customer_interactions> rr = new ReturnResults<tb_Customer_interactions>();
            tb_Customer_interactions Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.interaction_id > 0)
                {
                    bool rs = await _tb_Customer_interactionsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_Customer_interactionsServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(entity);
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
            tb_Customer_interactions entity = model as tb_Customer_interactions;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.interaction_id > 0)
                {
                    bool rs = await _tb_Customer_interactionsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_Customer_interactionsServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(entity);
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
            List<T> list = await _tb_Customer_interactionsServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Customer_interactions entity = item as tb_Customer_interactions;
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
            List<T> list = await _tb_Customer_interactionsServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Customer_interactions entity = item as tb_Customer_interactions;
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
            tb_Customer_interactions entity = model as tb_Customer_interactions;
            bool rs = await _tb_Customer_interactionsServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Customer_interactions>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Customer_interactions> entitys = models as List<tb_Customer_interactions>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Customer_interactions>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.interaction_id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Customer_interactions>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_Customer_interactionsValidator validator = new tb_Customer_interactionsValidator();
            ValidationResult results = validator.Validate(info as tb_Customer_interactions);
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
                tb_Customer_interactions entity = model as tb_Customer_interactions;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.interaction_id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Customer_interactions>(entity as tb_Customer_interactions)
            //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Customer_interactions>(entity as tb_Customer_interactions)
        //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.interaction_id;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                //出错后，取消生成的ID等值
                command.Undo();
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                _logger.Error("BaseSaveOrUpdateWithChild事务回滚");
                rsms.ErrorMsg = "事务回滚=>" + ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interactions>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Customer_interactions entity = model as tb_Customer_interactions;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Customer_interactions>(m => m.interaction_id== entity.interaction_id)
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
        
        
        
        public tb_Customer_interactions AddReEntity(tb_Customer_interactions entity)
        {
            tb_Customer_interactions AddEntity =  _tb_Customer_interactionsServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Customer_interactions> AddReEntityAsync(tb_Customer_interactions entity)
        {
            tb_Customer_interactions AddEntity = await _tb_Customer_interactionsServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Customer_interactions entity)
        {
            long id = await _tb_Customer_interactionsServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Customer_interactions> infos)
        {
            List<long> ids = await _tb_Customer_interactionsServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Customer_interactions entity)
        {
            bool rs = await _tb_Customer_interactionsServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Customer_interactions>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Customer_interactions entity)
        {
            bool rs = await _tb_Customer_interactionsServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_Customer_interactionsServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Customer_interactions>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_Customer_interactionsServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Customer_interactions>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Customer_interactions>> QueryAsync()
        {
            List<tb_Customer_interactions> list = await  _tb_Customer_interactionsServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(list);
            return list;
        }
        
        public virtual List<tb_Customer_interactions> Query()
        {
            List<tb_Customer_interactions> list =  _tb_Customer_interactionsServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(list);
            return list;
        }
        
        public virtual List<tb_Customer_interactions> Query(string wheresql)
        {
            List<tb_Customer_interactions> list =  _tb_Customer_interactionsServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Customer_interactions>> QueryAsync(string wheresql) 
        {
            List<tb_Customer_interactions> list = await _tb_Customer_interactionsServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Customer_interactions>> QueryAsync(Expression<Func<tb_Customer_interactions, bool>> exp)
        {
            List<tb_Customer_interactions> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interactions>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Customer_interactions>> QueryByNavAsync()
        {
            List<tb_Customer_interactions> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interactions>()
                               .Includes(t => t.tb_Employee )
                               .Includes(t => t.tb_Customer )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Customer_interactions>> QueryByNavAsync(Expression<Func<tb_Customer_interactions, bool>> exp)
        {
            List<tb_Customer_interactions> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interactions>().Where(exp)
                               .Includes(t => t.tb_Employee )
                               .Includes(t => t.tb_Customer )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Customer_interactions> QueryByNav(Expression<Func<tb_Customer_interactions, bool>> exp)
        {
            List<tb_Customer_interactions> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interactions>().Where(exp)
                            .Includes(t => t.tb_Employee )
                            .Includes(t => t.tb_Customer )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Customer_interactions>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interactions>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_Customer_interactionsServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Customer_interactions entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interactions>().Where(w => w.interaction_id == (long)id)
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interactions>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



