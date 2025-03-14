
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:49
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
    /// 可视化排程
    /// </summary>
    public partial class tb_ProduceViewScheduleController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProduceViewScheduleServices _tb_ProduceViewScheduleServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProduceViewScheduleController(ILogger<tb_ProduceViewScheduleController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProduceViewScheduleServices tb_ProduceViewScheduleServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProduceViewScheduleServices = tb_ProduceViewScheduleServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProduceViewSchedule info)
        {

           // tb_ProduceViewScheduleValidator validator = new tb_ProduceViewScheduleValidator();
           tb_ProduceViewScheduleValidator validator = _appContext.GetRequiredService<tb_ProduceViewScheduleValidator>();
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
        public async Task<ReturnResults<tb_ProduceViewSchedule>> SaveOrUpdate(tb_ProduceViewSchedule entity)
        {
            ReturnResults<tb_ProduceViewSchedule> rr = new ReturnResults<tb_ProduceViewSchedule>();
            tb_ProduceViewSchedule Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.id > 0)
                {
                    bool rs = await _tb_ProduceViewScheduleServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProduceViewScheduleServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(entity);
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
            tb_ProduceViewSchedule entity = model as tb_ProduceViewSchedule;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.id > 0)
                {
                    bool rs = await _tb_ProduceViewScheduleServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProduceViewScheduleServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(entity);
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
            List<T> list = await _tb_ProduceViewScheduleServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProduceViewSchedule entity = item as tb_ProduceViewSchedule;
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
            List<T> list = await _tb_ProduceViewScheduleServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProduceViewSchedule entity = item as tb_ProduceViewSchedule;
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
            tb_ProduceViewSchedule entity = model as tb_ProduceViewSchedule;
            bool rs = await _tb_ProduceViewScheduleServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProduceViewSchedule>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProduceViewSchedule> entitys = models as List<tb_ProduceViewSchedule>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProduceViewSchedule>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProduceViewSchedule>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProduceViewScheduleValidator validator = new tb_ProduceViewScheduleValidator();
           tb_ProduceViewScheduleValidator validator = _appContext.GetRequiredService<tb_ProduceViewScheduleValidator>();
            ValidationResult results = validator.Validate(info as tb_ProduceViewSchedule);
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
                             //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
            try
            {

                tb_ProduceViewSchedule entity = model as tb_ProduceViewSchedule;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.id > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_ProduceViewSchedule>(entity as tb_ProduceViewSchedule)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_ProduceViewSchedule>(entity as tb_ProduceViewSchedule)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceViewSchedule>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProduceViewSchedule entity = model as tb_ProduceViewSchedule;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProduceViewSchedule>(m => m.id== entity.id)
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
        
        
        
        public tb_ProduceViewSchedule AddReEntity(tb_ProduceViewSchedule entity)
        {
            tb_ProduceViewSchedule AddEntity =  _tb_ProduceViewScheduleServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProduceViewSchedule> AddReEntityAsync(tb_ProduceViewSchedule entity)
        {
            tb_ProduceViewSchedule AddEntity = await _tb_ProduceViewScheduleServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProduceViewSchedule entity)
        {
            long id = await _tb_ProduceViewScheduleServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProduceViewSchedule> infos)
        {
            List<long> ids = await _tb_ProduceViewScheduleServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProduceViewSchedule entity)
        {
            bool rs = await _tb_ProduceViewScheduleServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProduceViewSchedule>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProduceViewSchedule entity)
        {
            bool rs = await _tb_ProduceViewScheduleServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProduceViewScheduleServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProduceViewSchedule>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProduceViewScheduleServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProduceViewSchedule>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProduceViewSchedule>> QueryAsync()
        {
            List<tb_ProduceViewSchedule> list = await  _tb_ProduceViewScheduleServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(list);
            return list;
        }
        
        public virtual List<tb_ProduceViewSchedule> Query()
        {
            List<tb_ProduceViewSchedule> list =  _tb_ProduceViewScheduleServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(list);
            return list;
        }
        
        public virtual List<tb_ProduceViewSchedule> Query(string wheresql)
        {
            List<tb_ProduceViewSchedule> list =  _tb_ProduceViewScheduleServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProduceViewSchedule>> QueryAsync(string wheresql) 
        {
            List<tb_ProduceViewSchedule> list = await _tb_ProduceViewScheduleServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProduceViewSchedule>> QueryAsync(Expression<Func<tb_ProduceViewSchedule, bool>> exp)
        {
            List<tb_ProduceViewSchedule> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceViewSchedule>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProduceViewSchedule>> QueryByNavAsync()
        {
            List<tb_ProduceViewSchedule> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceViewSchedule>()
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProduceViewSchedule>> QueryByNavAsync(Expression<Func<tb_ProduceViewSchedule, bool>> exp)
        {
            List<tb_ProduceViewSchedule> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceViewSchedule>().Where(exp)
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProduceViewSchedule> QueryByNav(Expression<Func<tb_ProduceViewSchedule, bool>> exp)
        {
            List<tb_ProduceViewSchedule> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceViewSchedule>().Where(exp)
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProduceViewSchedule>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceViewSchedule>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProduceViewScheduleServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProduceViewSchedule entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceViewSchedule>().Where(w => w.id == (long)id)
                                     .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProduceViewSchedule>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



