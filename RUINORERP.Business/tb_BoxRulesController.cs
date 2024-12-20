
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/19/2024 15:52:33
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
    /// 箱规表
    /// </summary>
    public partial class tb_BoxRulesController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_BoxRulesServices _tb_BoxRulesServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_BoxRulesController(ILogger<tb_BoxRulesController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_BoxRulesServices tb_BoxRulesServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_BoxRulesServices = tb_BoxRulesServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_BoxRules info)
        {

           // tb_BoxRulesValidator validator = new tb_BoxRulesValidator();
           tb_BoxRulesValidator validator = _appContext.GetRequiredService<tb_BoxRulesValidator>();
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
        public async Task<ReturnResults<tb_BoxRules>> SaveOrUpdate(tb_BoxRules entity)
        {
            ReturnResults<tb_BoxRules> rr = new ReturnResults<tb_BoxRules>();
            tb_BoxRules Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BoxRules_ID > 0)
                {
                    bool rs = await _tb_BoxRulesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_BoxRulesServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(entity);
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
            tb_BoxRules entity = model as tb_BoxRules;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BoxRules_ID > 0)
                {
                    bool rs = await _tb_BoxRulesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_BoxRulesServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(entity);
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
            List<T> list = await _tb_BoxRulesServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_BoxRules entity = item as tb_BoxRules;
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
            List<T> list = await _tb_BoxRulesServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_BoxRules entity = item as tb_BoxRules;
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
            tb_BoxRules entity = model as tb_BoxRules;
            bool rs = await _tb_BoxRulesServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_BoxRules>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_BoxRules> entitys = models as List<tb_BoxRules>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_BoxRules>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.BoxRules_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_BoxRules>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_BoxRulesValidator validator = new tb_BoxRulesValidator();
           tb_BoxRulesValidator validator = _appContext.GetRequiredService<tb_BoxRulesValidator>();
            ValidationResult results = validator.Validate(info as tb_BoxRules);
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
                tb_BoxRules entity = model as tb_BoxRules;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.BoxRules_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_BoxRules>(entity as tb_BoxRules)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_BoxRules>(entity as tb_BoxRules)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.BoxRules_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BoxRules>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_BoxRules entity = model as tb_BoxRules;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_BoxRules>(m => m.BoxRules_ID== entity.BoxRules_ID)
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
        
        
        
        public tb_BoxRules AddReEntity(tb_BoxRules entity)
        {
            tb_BoxRules AddEntity =  _tb_BoxRulesServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_BoxRules> AddReEntityAsync(tb_BoxRules entity)
        {
            tb_BoxRules AddEntity = await _tb_BoxRulesServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_BoxRules entity)
        {
            long id = await _tb_BoxRulesServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_BoxRules> infos)
        {
            List<long> ids = await _tb_BoxRulesServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_BoxRules entity)
        {
            bool rs = await _tb_BoxRulesServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BoxRules>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_BoxRules entity)
        {
            bool rs = await _tb_BoxRulesServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_BoxRulesServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BoxRules>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_BoxRulesServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BoxRules>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_BoxRules>> QueryAsync()
        {
            List<tb_BoxRules> list = await  _tb_BoxRulesServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(list);
            return list;
        }
        
        public virtual List<tb_BoxRules> Query()
        {
            List<tb_BoxRules> list =  _tb_BoxRulesServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(list);
            return list;
        }
        
        public virtual List<tb_BoxRules> Query(string wheresql)
        {
            List<tb_BoxRules> list =  _tb_BoxRulesServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(list);
            return list;
        }
        
        public virtual async Task<List<tb_BoxRules>> QueryAsync(string wheresql) 
        {
            List<tb_BoxRules> list = await _tb_BoxRulesServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_BoxRules>> QueryAsync(Expression<Func<tb_BoxRules, bool>> exp)
        {
            List<tb_BoxRules> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BoxRules>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BoxRules>> QueryByNavAsync()
        {
            List<tb_BoxRules> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BoxRules>()
                               .Includes(t => t.tb_cartoonbox )
                               .Includes(t => t.tb_packing )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BoxRules>> QueryByNavAsync(Expression<Func<tb_BoxRules, bool>> exp)
        {
            List<tb_BoxRules> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BoxRules>().Where(exp)
                               .Includes(t => t.tb_cartoonbox )
                               .Includes(t => t.tb_packing )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_BoxRules> QueryByNav(Expression<Func<tb_BoxRules, bool>> exp)
        {
            List<tb_BoxRules> list = _unitOfWorkManage.GetDbClient().Queryable<tb_BoxRules>().Where(exp)
                            .Includes(t => t.tb_cartoonbox )
                            .Includes(t => t.tb_packing )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_BoxRules>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BoxRules>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_BoxRulesServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_BoxRules entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_BoxRules>().Where(w => w.BoxRules_ID == (long)id)
                             .Includes(t => t.tb_cartoonbox )
                            .Includes(t => t.tb_packing )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_BoxRules>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



