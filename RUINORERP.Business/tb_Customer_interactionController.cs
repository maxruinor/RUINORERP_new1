
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
    /// 客户交互表，CRM系统中使用      
    /// </summary>
    public partial class tb_Customer_interactionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_Customer_interactionServices _tb_Customer_interactionServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_Customer_interactionController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_Customer_interactionServices tb_Customer_interactionServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_Customer_interactionServices = tb_Customer_interactionServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_Customer_interaction info)
        {
            tb_Customer_interactionValidator validator = new tb_Customer_interactionValidator();
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
        public async Task<ReturnResults<tb_Customer_interaction>> SaveOrUpdate(tb_Customer_interaction entity)
        {
            ReturnResults<tb_Customer_interaction> rr = new ReturnResults<tb_Customer_interaction>();
            tb_Customer_interaction Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.interaction_id > 0)
                {
                    bool rs = await _tb_Customer_interactionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_Customer_interactionServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(entity);
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
            tb_Customer_interaction entity = model as tb_Customer_interaction;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.interaction_id > 0)
                {
                    bool rs = await _tb_Customer_interactionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_Customer_interactionServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(entity);
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
            List<T> list = await _tb_Customer_interactionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Customer_interaction entity = item as tb_Customer_interaction;
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
            List<T> list = await _tb_Customer_interactionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Customer_interaction entity = item as tb_Customer_interaction;
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
            tb_Customer_interaction entity = model as tb_Customer_interaction;
            bool rs = await _tb_Customer_interactionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Customer_interaction>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Customer_interaction> entitys = models as List<tb_Customer_interaction>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Customer_interaction>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.interaction_id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Customer_interaction>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_Customer_interactionValidator validator = new tb_Customer_interactionValidator();
            ValidationResult results = validator.Validate(info as tb_Customer_interaction);
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
                tb_Customer_interaction entity = model as tb_Customer_interaction;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.interaction_id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Customer_interaction>(entity as tb_Customer_interaction)
            //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Customer_interaction>(entity as tb_Customer_interaction)
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interaction>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Customer_interaction entity = model as tb_Customer_interaction;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Customer_interaction>(m => m.interaction_id== entity.interaction_id)
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
        
        
        
        public tb_Customer_interaction AddReEntity(tb_Customer_interaction entity)
        {
            tb_Customer_interaction AddEntity =  _tb_Customer_interactionServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Customer_interaction> AddReEntityAsync(tb_Customer_interaction entity)
        {
            tb_Customer_interaction AddEntity = await _tb_Customer_interactionServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Customer_interaction entity)
        {
            long id = await _tb_Customer_interactionServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Customer_interaction> infos)
        {
            List<long> ids = await _tb_Customer_interactionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Customer_interaction entity)
        {
            bool rs = await _tb_Customer_interactionServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Customer_interaction>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Customer_interaction entity)
        {
            bool rs = await _tb_Customer_interactionServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_Customer_interactionServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Customer_interaction>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_Customer_interactionServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Customer_interaction>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Customer_interaction>> QueryAsync()
        {
            List<tb_Customer_interaction> list = await  _tb_Customer_interactionServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(list);
            return list;
        }
        
        public virtual List<tb_Customer_interaction> Query()
        {
            List<tb_Customer_interaction> list =  _tb_Customer_interactionServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(list);
            return list;
        }
        
        public virtual List<tb_Customer_interaction> Query(string wheresql)
        {
            List<tb_Customer_interaction> list =  _tb_Customer_interactionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Customer_interaction>> QueryAsync(string wheresql) 
        {
            List<tb_Customer_interaction> list = await _tb_Customer_interactionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Customer_interaction>> QueryAsync(Expression<Func<tb_Customer_interaction, bool>> exp)
        {
            List<tb_Customer_interaction> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interaction>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Customer_interaction>> QueryByNavAsync()
        {
            List<tb_Customer_interaction> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interaction>()
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customer )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Customer_interaction>> QueryByNavAsync(Expression<Func<tb_Customer_interaction, bool>> exp)
        {
            List<tb_Customer_interaction> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interaction>().Where(exp)
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customer )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Customer_interaction> QueryByNav(Expression<Func<tb_Customer_interaction, bool>> exp)
        {
            List<tb_Customer_interaction> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interaction>().Where(exp)
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customer )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Customer_interaction>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interaction>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_Customer_interactionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Customer_interaction entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Customer_interaction>().Where(w => w.interaction_id == (long)id)
                             .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customer )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Customer_interaction>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



