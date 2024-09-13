
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:40
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
    /// 用户个性化设置表
    /// </summary>
    public partial class tb_UserPersonalizationController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_UserPersonalizationServices _tb_UserPersonalizationServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_UserPersonalizationController(ILogger<tb_UserPersonalizationController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_UserPersonalizationServices tb_UserPersonalizationServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_UserPersonalizationServices = tb_UserPersonalizationServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_UserPersonalization info)
        {
            tb_UserPersonalizationValidator validator = new tb_UserPersonalizationValidator();
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
        public async Task<ReturnResults<tb_UserPersonalization>> SaveOrUpdate(tb_UserPersonalization entity)
        {
            ReturnResults<tb_UserPersonalization> rr = new ReturnResults<tb_UserPersonalization>();
            tb_UserPersonalization Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ID > 0)
                {
                    bool rs = await _tb_UserPersonalizationServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_UserPersonalizationServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(entity);
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
            tb_UserPersonalization entity = model as tb_UserPersonalization;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ID > 0)
                {
                    bool rs = await _tb_UserPersonalizationServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_UserPersonalizationServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(entity);
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
            List<T> list = await _tb_UserPersonalizationServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_UserPersonalization entity = item as tb_UserPersonalization;
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
            List<T> list = await _tb_UserPersonalizationServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_UserPersonalization entity = item as tb_UserPersonalization;
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
            tb_UserPersonalization entity = model as tb_UserPersonalization;
            bool rs = await _tb_UserPersonalizationServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_UserPersonalization>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_UserPersonalization> entitys = models as List<tb_UserPersonalization>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_UserPersonalization>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_UserPersonalization>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_UserPersonalizationValidator validator = new tb_UserPersonalizationValidator();
            ValidationResult results = validator.Validate(info as tb_UserPersonalization);
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
                tb_UserPersonalization entity = model as tb_UserPersonalization;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_UserPersonalization>(entity as tb_UserPersonalization)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_UserPersonalization>(entity as tb_UserPersonalization)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalization>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_UserPersonalization entity = model as tb_UserPersonalization;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_UserPersonalization>(m => m.ID== entity.ID)
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
        
        
        
        public tb_UserPersonalization AddReEntity(tb_UserPersonalization entity)
        {
            tb_UserPersonalization AddEntity =  _tb_UserPersonalizationServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_UserPersonalization> AddReEntityAsync(tb_UserPersonalization entity)
        {
            tb_UserPersonalization AddEntity = await _tb_UserPersonalizationServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_UserPersonalization entity)
        {
            long id = await _tb_UserPersonalizationServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_UserPersonalization> infos)
        {
            List<long> ids = await _tb_UserPersonalizationServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_UserPersonalization entity)
        {
            bool rs = await _tb_UserPersonalizationServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UserPersonalization>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_UserPersonalization entity)
        {
            bool rs = await _tb_UserPersonalizationServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_UserPersonalizationServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UserPersonalization>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_UserPersonalizationServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UserPersonalization>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_UserPersonalization>> QueryAsync()
        {
            List<tb_UserPersonalization> list = await  _tb_UserPersonalizationServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(list);
            return list;
        }
        
        public virtual List<tb_UserPersonalization> Query()
        {
            List<tb_UserPersonalization> list =  _tb_UserPersonalizationServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(list);
            return list;
        }
        
        public virtual List<tb_UserPersonalization> Query(string wheresql)
        {
            List<tb_UserPersonalization> list =  _tb_UserPersonalizationServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(list);
            return list;
        }
        
        public virtual async Task<List<tb_UserPersonalization>> QueryAsync(string wheresql) 
        {
            List<tb_UserPersonalization> list = await _tb_UserPersonalizationServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_UserPersonalization>> QueryAsync(Expression<Func<tb_UserPersonalization, bool>> exp)
        {
            List<tb_UserPersonalization> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalization>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UserPersonalization>> QueryByNavAsync()
        {
            List<tb_UserPersonalization> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalization>()
                               .Includes(t => t.tb_userinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UserPersonalization>> QueryByNavAsync(Expression<Func<tb_UserPersonalization, bool>> exp)
        {
            List<tb_UserPersonalization> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalization>().Where(exp)
                               .Includes(t => t.tb_userinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_UserPersonalization> QueryByNav(Expression<Func<tb_UserPersonalization, bool>> exp)
        {
            List<tb_UserPersonalization> list = _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalization>().Where(exp)
                            .Includes(t => t.tb_userinfo )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_UserPersonalization>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalization>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_UserPersonalizationServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_UserPersonalization entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalization>().Where(w => w.ID == (long)id)
                             .Includes(t => t.tb_userinfo )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalization>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



