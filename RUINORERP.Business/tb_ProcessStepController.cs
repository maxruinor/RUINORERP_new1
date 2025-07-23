
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:47
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
    /// 流程步骤
    /// </summary>
    public partial class tb_ProcessStepController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProcessStepServices _tb_ProcessStepServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProcessStepController(ILogger<tb_ProcessStepController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProcessStepServices tb_ProcessStepServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProcessStepServices = tb_ProcessStepServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProcessStep info)
        {

           // tb_ProcessStepValidator validator = new tb_ProcessStepValidator();
           tb_ProcessStepValidator validator = _appContext.GetRequiredService<tb_ProcessStepValidator>();
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
        public async Task<ReturnResults<tb_ProcessStep>> SaveOrUpdate(tb_ProcessStep entity)
        {
            ReturnResults<tb_ProcessStep> rr = new ReturnResults<tb_ProcessStep>();
            tb_ProcessStep Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Step_Id > 0)
                {
                    bool rs = await _tb_ProcessStepServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProcessStepServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(entity);
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
            tb_ProcessStep entity = model as tb_ProcessStep;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Step_Id > 0)
                {
                    bool rs = await _tb_ProcessStepServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProcessStepServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(entity);
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
            List<T> list = await _tb_ProcessStepServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProcessStep entity = item as tb_ProcessStep;
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
            List<T> list = await _tb_ProcessStepServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProcessStep entity = item as tb_ProcessStep;
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
            tb_ProcessStep entity = model as tb_ProcessStep;
            bool rs = await _tb_ProcessStepServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProcessStep>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProcessStep> entitys = models as List<tb_ProcessStep>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProcessStep>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Step_Id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProcessStep>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProcessStepValidator validator = new tb_ProcessStepValidator();
           tb_ProcessStepValidator validator = _appContext.GetRequiredService<tb_ProcessStepValidator>();
            ValidationResult results = validator.Validate(info as tb_ProcessStep);
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

                tb_ProcessStep entity = model as tb_ProcessStep;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Step_Id > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProcessStep>(entity as tb_ProcessStep)
                        .Include(m => m.tb_ProcessDefinitions)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProcessStep>(entity as tb_ProcessStep)
                .Include(m => m.tb_ProcessDefinitions)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Step_Id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessStep>()
                                .Includes(m => m.tb_ProcessDefinitions)
                                        .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProcessStep entity = model as tb_ProcessStep;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProcessStep>(m => m.Step_Id== entity.Step_Id)
                                .Include(m => m.tb_ProcessDefinitions)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProcessStep AddReEntity(tb_ProcessStep entity)
        {
            tb_ProcessStep AddEntity =  _tb_ProcessStepServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProcessStep> AddReEntityAsync(tb_ProcessStep entity)
        {
            tb_ProcessStep AddEntity = await _tb_ProcessStepServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProcessStep entity)
        {
            long id = await _tb_ProcessStepServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProcessStep> infos)
        {
            List<long> ids = await _tb_ProcessStepServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProcessStep entity)
        {
            bool rs = await _tb_ProcessStepServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProcessStep>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProcessStep entity)
        {
            bool rs = await _tb_ProcessStepServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProcessStepServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProcessStep>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProcessStepServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProcessStep>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProcessStep>> QueryAsync()
        {
            List<tb_ProcessStep> list = await  _tb_ProcessStepServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(list);
            return list;
        }
        
        public virtual List<tb_ProcessStep> Query()
        {
            List<tb_ProcessStep> list =  _tb_ProcessStepServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(list);
            return list;
        }
        
        public virtual List<tb_ProcessStep> Query(string wheresql)
        {
            List<tb_ProcessStep> list =  _tb_ProcessStepServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProcessStep>> QueryAsync(string wheresql) 
        {
            List<tb_ProcessStep> list = await _tb_ProcessStepServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProcessStep>> QueryAsync(Expression<Func<tb_ProcessStep, bool>> exp)
        {
            List<tb_ProcessStep> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessStep>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProcessStep>> QueryByNavAsync()
        {
            List<tb_ProcessStep> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessStep>()
                               .Includes(t => t.tb_nextnodes )
                               .Includes(t => t.tb_position )
                               .Includes(t => t.tb_stepbody )
                                            .Includes(t => t.tb_ProcessDefinitions )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProcessStep>> QueryByNavAsync(Expression<Func<tb_ProcessStep, bool>> exp)
        {
            List<tb_ProcessStep> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessStep>().Where(exp)
                               .Includes(t => t.tb_nextnodes )
                               .Includes(t => t.tb_position )
                               .Includes(t => t.tb_stepbody )
                                            .Includes(t => t.tb_ProcessDefinitions )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProcessStep> QueryByNav(Expression<Func<tb_ProcessStep, bool>> exp)
        {
            List<tb_ProcessStep> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessStep>().Where(exp)
                            .Includes(t => t.tb_nextnodes )
                            .Includes(t => t.tb_position )
                            .Includes(t => t.tb_stepbody )
                                        .Includes(t => t.tb_ProcessDefinitions )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProcessStep>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessStep>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProcessStepServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProcessStep entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessStep>().Where(w => w.Step_Id == (long)id)
                             .Includes(t => t.tb_nextnodes )
                            .Includes(t => t.tb_position )
                            .Includes(t => t.tb_stepbody )
                                        .Includes(t => t.tb_ProcessDefinitions )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProcessStep>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



