
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/05/2024 23:44:22
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
    /// UI查询条件设置
    /// </summary>
    public partial class tb_UIQueryConditionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_UIQueryConditionServices _tb_UIQueryConditionServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_UIQueryConditionController(ILogger<tb_UIQueryConditionController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_UIQueryConditionServices tb_UIQueryConditionServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_UIQueryConditionServices = tb_UIQueryConditionServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_UIQueryCondition info)
        {
            tb_UIQueryConditionValidator validator = new tb_UIQueryConditionValidator();
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
        public async Task<ReturnResults<tb_UIQueryCondition>> SaveOrUpdate(tb_UIQueryCondition entity)
        {
            ReturnResults<tb_UIQueryCondition> rr = new ReturnResults<tb_UIQueryCondition>();
            tb_UIQueryCondition Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UIQCID > 0)
                {
                    bool rs = await _tb_UIQueryConditionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_UIQueryConditionServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(entity);
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
            tb_UIQueryCondition entity = model as tb_UIQueryCondition;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UIQCID > 0)
                {
                    bool rs = await _tb_UIQueryConditionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_UIQueryConditionServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(entity);
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
            List<T> list = await _tb_UIQueryConditionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_UIQueryCondition entity = item as tb_UIQueryCondition;
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
            List<T> list = await _tb_UIQueryConditionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_UIQueryCondition entity = item as tb_UIQueryCondition;
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
            tb_UIQueryCondition entity = model as tb_UIQueryCondition;
            bool rs = await _tb_UIQueryConditionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_UIQueryCondition>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_UIQueryCondition> entitys = models as List<tb_UIQueryCondition>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_UIQueryCondition>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.UIQCID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_UIQueryCondition>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_UIQueryConditionValidator validator = new tb_UIQueryConditionValidator();
            ValidationResult results = validator.Validate(info as tb_UIQueryCondition);
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
                tb_UIQueryCondition entity = model as tb_UIQueryCondition;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.UIQCID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_UIQueryCondition>(entity as tb_UIQueryCondition)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_UIQueryCondition>(entity as tb_UIQueryCondition)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.UIQCID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIQueryCondition>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_UIQueryCondition entity = model as tb_UIQueryCondition;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_UIQueryCondition>(m => m.UIQCID== entity.UIQCID)
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
        
        
        
        public tb_UIQueryCondition AddReEntity(tb_UIQueryCondition entity)
        {
            tb_UIQueryCondition AddEntity =  _tb_UIQueryConditionServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_UIQueryCondition> AddReEntityAsync(tb_UIQueryCondition entity)
        {
            tb_UIQueryCondition AddEntity = await _tb_UIQueryConditionServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_UIQueryCondition entity)
        {
            long id = await _tb_UIQueryConditionServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_UIQueryCondition> infos)
        {
            List<long> ids = await _tb_UIQueryConditionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_UIQueryCondition entity)
        {
            bool rs = await _tb_UIQueryConditionServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIQueryCondition>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_UIQueryCondition entity)
        {
            bool rs = await _tb_UIQueryConditionServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_UIQueryConditionServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIQueryCondition>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_UIQueryConditionServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIQueryCondition>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_UIQueryCondition>> QueryAsync()
        {
            List<tb_UIQueryCondition> list = await  _tb_UIQueryConditionServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(list);
            return list;
        }
        
        public virtual List<tb_UIQueryCondition> Query()
        {
            List<tb_UIQueryCondition> list =  _tb_UIQueryConditionServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(list);
            return list;
        }
        
        public virtual List<tb_UIQueryCondition> Query(string wheresql)
        {
            List<tb_UIQueryCondition> list =  _tb_UIQueryConditionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(list);
            return list;
        }
        
        public virtual async Task<List<tb_UIQueryCondition>> QueryAsync(string wheresql) 
        {
            List<tb_UIQueryCondition> list = await _tb_UIQueryConditionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_UIQueryCondition>> QueryAsync(Expression<Func<tb_UIQueryCondition, bool>> exp)
        {
            List<tb_UIQueryCondition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIQueryCondition>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIQueryCondition>> QueryByNavAsync()
        {
            List<tb_UIQueryCondition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIQueryCondition>()
                               .Includes(t => t.tb_uimenupersonalization )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIQueryCondition>> QueryByNavAsync(Expression<Func<tb_UIQueryCondition, bool>> exp)
        {
            List<tb_UIQueryCondition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIQueryCondition>().Where(exp)
                               .Includes(t => t.tb_uimenupersonalization )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_UIQueryCondition> QueryByNav(Expression<Func<tb_UIQueryCondition, bool>> exp)
        {
            List<tb_UIQueryCondition> list = _unitOfWorkManage.GetDbClient().Queryable<tb_UIQueryCondition>().Where(exp)
                            .Includes(t => t.tb_uimenupersonalization )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_UIQueryCondition>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIQueryCondition>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_UIQueryConditionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_UIQueryCondition entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIQueryCondition>().Where(w => w.UIQCID == (long)id)
                             .Includes(t => t.tb_uimenupersonalization )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_UIQueryCondition>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



