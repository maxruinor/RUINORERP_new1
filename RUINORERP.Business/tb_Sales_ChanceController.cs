
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:33
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
    /// 销售机会
    /// </summary>
    public partial class tb_Sales_ChanceController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_Sales_ChanceServices _tb_Sales_ChanceServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_Sales_ChanceController(ILogger<tb_Sales_ChanceController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_Sales_ChanceServices tb_Sales_ChanceServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_Sales_ChanceServices = tb_Sales_ChanceServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_Sales_Chance info)
        {
            tb_Sales_ChanceValidator validator = new tb_Sales_ChanceValidator();
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
        public async Task<ReturnResults<tb_Sales_Chance>> SaveOrUpdate(tb_Sales_Chance entity)
        {
            ReturnResults<tb_Sales_Chance> rr = new ReturnResults<tb_Sales_Chance>();
            tb_Sales_Chance Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.opportunity_id > 0)
                {
                    bool rs = await _tb_Sales_ChanceServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_Sales_ChanceServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(entity);
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
            tb_Sales_Chance entity = model as tb_Sales_Chance;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.opportunity_id > 0)
                {
                    bool rs = await _tb_Sales_ChanceServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_Sales_ChanceServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(entity);
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
            List<T> list = await _tb_Sales_ChanceServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Sales_Chance entity = item as tb_Sales_Chance;
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
            List<T> list = await _tb_Sales_ChanceServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Sales_Chance entity = item as tb_Sales_Chance;
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
            tb_Sales_Chance entity = model as tb_Sales_Chance;
            bool rs = await _tb_Sales_ChanceServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Sales_Chance>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Sales_Chance> entitys = models as List<tb_Sales_Chance>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Sales_Chance>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.opportunity_id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Sales_Chance>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_Sales_ChanceValidator validator = new tb_Sales_ChanceValidator();
            ValidationResult results = validator.Validate(info as tb_Sales_Chance);
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
                tb_Sales_Chance entity = model as tb_Sales_Chance;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.opportunity_id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Sales_Chance>(entity as tb_Sales_Chance)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Sales_Chance>(entity as tb_Sales_Chance)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.opportunity_id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Sales_Chance>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Sales_Chance entity = model as tb_Sales_Chance;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Sales_Chance>(m => m.opportunity_id== entity.opportunity_id)
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
        
        
        
        public tb_Sales_Chance AddReEntity(tb_Sales_Chance entity)
        {
            tb_Sales_Chance AddEntity =  _tb_Sales_ChanceServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Sales_Chance> AddReEntityAsync(tb_Sales_Chance entity)
        {
            tb_Sales_Chance AddEntity = await _tb_Sales_ChanceServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Sales_Chance entity)
        {
            long id = await _tb_Sales_ChanceServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Sales_Chance> infos)
        {
            List<long> ids = await _tb_Sales_ChanceServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Sales_Chance entity)
        {
            bool rs = await _tb_Sales_ChanceServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Sales_Chance>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Sales_Chance entity)
        {
            bool rs = await _tb_Sales_ChanceServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_Sales_ChanceServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Sales_Chance>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_Sales_ChanceServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Sales_Chance>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Sales_Chance>> QueryAsync()
        {
            List<tb_Sales_Chance> list = await  _tb_Sales_ChanceServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(list);
            return list;
        }
        
        public virtual List<tb_Sales_Chance> Query()
        {
            List<tb_Sales_Chance> list =  _tb_Sales_ChanceServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(list);
            return list;
        }
        
        public virtual List<tb_Sales_Chance> Query(string wheresql)
        {
            List<tb_Sales_Chance> list =  _tb_Sales_ChanceServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Sales_Chance>> QueryAsync(string wheresql) 
        {
            List<tb_Sales_Chance> list = await _tb_Sales_ChanceServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Sales_Chance>> QueryAsync(Expression<Func<tb_Sales_Chance, bool>> exp)
        {
            List<tb_Sales_Chance> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Sales_Chance>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Sales_Chance>> QueryByNavAsync()
        {
            List<tb_Sales_Chance> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Sales_Chance>()
                               .Includes(t => t.tb_customer )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Sales_Chance>> QueryByNavAsync(Expression<Func<tb_Sales_Chance, bool>> exp)
        {
            List<tb_Sales_Chance> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Sales_Chance>().Where(exp)
                               .Includes(t => t.tb_customer )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Sales_Chance> QueryByNav(Expression<Func<tb_Sales_Chance, bool>> exp)
        {
            List<tb_Sales_Chance> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Sales_Chance>().Where(exp)
                            .Includes(t => t.tb_customer )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Sales_Chance>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Sales_Chance>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_Sales_ChanceServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Sales_Chance entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Sales_Chance>().Where(w => w.opportunity_id == (long)id)
                             .Includes(t => t.tb_customer )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Sales_Chance>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



