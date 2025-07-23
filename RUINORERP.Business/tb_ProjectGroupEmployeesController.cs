
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 14:14:54
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
    /// 项目及成员关系表
    /// </summary>
    public partial class tb_ProjectGroupEmployeesController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProjectGroupEmployeesServices _tb_ProjectGroupEmployeesServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProjectGroupEmployeesController(ILogger<tb_ProjectGroupEmployeesController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProjectGroupEmployeesServices tb_ProjectGroupEmployeesServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProjectGroupEmployeesServices = tb_ProjectGroupEmployeesServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProjectGroupEmployees info)
        {

           // tb_ProjectGroupEmployeesValidator validator = new tb_ProjectGroupEmployeesValidator();
           tb_ProjectGroupEmployeesValidator validator = _appContext.GetRequiredService<tb_ProjectGroupEmployeesValidator>();
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
        public async Task<ReturnResults<tb_ProjectGroupEmployees>> SaveOrUpdate(tb_ProjectGroupEmployees entity)
        {
            ReturnResults<tb_ProjectGroupEmployees> rr = new ReturnResults<tb_ProjectGroupEmployees>();
            tb_ProjectGroupEmployees Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProjectGroupEmpID > 0)
                {
                    bool rs = await _tb_ProjectGroupEmployeesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProjectGroupEmployeesServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(entity);
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
            tb_ProjectGroupEmployees entity = model as tb_ProjectGroupEmployees;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProjectGroupEmpID > 0)
                {
                    bool rs = await _tb_ProjectGroupEmployeesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProjectGroupEmployeesServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(entity);
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
            List<T> list = await _tb_ProjectGroupEmployeesServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProjectGroupEmployees entity = item as tb_ProjectGroupEmployees;
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
            List<T> list = await _tb_ProjectGroupEmployeesServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProjectGroupEmployees entity = item as tb_ProjectGroupEmployees;
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
            tb_ProjectGroupEmployees entity = model as tb_ProjectGroupEmployees;
            bool rs = await _tb_ProjectGroupEmployeesServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroupEmployees>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProjectGroupEmployees> entitys = models as List<tb_ProjectGroupEmployees>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProjectGroupEmployees>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ProjectGroupEmpID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroupEmployees>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProjectGroupEmployeesValidator validator = new tb_ProjectGroupEmployeesValidator();
           tb_ProjectGroupEmployeesValidator validator = _appContext.GetRequiredService<tb_ProjectGroupEmployeesValidator>();
            ValidationResult results = validator.Validate(info as tb_ProjectGroupEmployees);
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

                tb_ProjectGroupEmployees entity = model as tb_ProjectGroupEmployees;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ProjectGroupEmpID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_ProjectGroupEmployees>(entity as tb_ProjectGroupEmployees)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_ProjectGroupEmployees>(entity as tb_ProjectGroupEmployees)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ProjectGroupEmpID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupEmployees>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProjectGroupEmployees entity = model as tb_ProjectGroupEmployees;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProjectGroupEmployees>(m => m.ProjectGroupEmpID== entity.ProjectGroupEmpID)
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
        
        
        
        public tb_ProjectGroupEmployees AddReEntity(tb_ProjectGroupEmployees entity)
        {
            tb_ProjectGroupEmployees AddEntity =  _tb_ProjectGroupEmployeesServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProjectGroupEmployees> AddReEntityAsync(tb_ProjectGroupEmployees entity)
        {
            tb_ProjectGroupEmployees AddEntity = await _tb_ProjectGroupEmployeesServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProjectGroupEmployees entity)
        {
            long id = await _tb_ProjectGroupEmployeesServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProjectGroupEmployees> infos)
        {
            List<long> ids = await _tb_ProjectGroupEmployeesServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProjectGroupEmployees entity)
        {
            bool rs = await _tb_ProjectGroupEmployeesServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroupEmployees>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProjectGroupEmployees entity)
        {
            bool rs = await _tb_ProjectGroupEmployeesServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProjectGroupEmployeesServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroupEmployees>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProjectGroupEmployeesServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroupEmployees>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProjectGroupEmployees>> QueryAsync()
        {
            List<tb_ProjectGroupEmployees> list = await  _tb_ProjectGroupEmployeesServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(list);
            return list;
        }
        
        public virtual List<tb_ProjectGroupEmployees> Query()
        {
            List<tb_ProjectGroupEmployees> list =  _tb_ProjectGroupEmployeesServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(list);
            return list;
        }
        
        public virtual List<tb_ProjectGroupEmployees> Query(string wheresql)
        {
            List<tb_ProjectGroupEmployees> list =  _tb_ProjectGroupEmployeesServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProjectGroupEmployees>> QueryAsync(string wheresql) 
        {
            List<tb_ProjectGroupEmployees> list = await _tb_ProjectGroupEmployeesServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProjectGroupEmployees>> QueryAsync(Expression<Func<tb_ProjectGroupEmployees, bool>> exp)
        {
            List<tb_ProjectGroupEmployees> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupEmployees>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProjectGroupEmployees>> QueryByNavAsync()
        {
            List<tb_ProjectGroupEmployees> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupEmployees>()
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProjectGroupEmployees>> QueryByNavAsync(Expression<Func<tb_ProjectGroupEmployees, bool>> exp)
        {
            List<tb_ProjectGroupEmployees> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupEmployees>().Where(exp)
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProjectGroupEmployees> QueryByNav(Expression<Func<tb_ProjectGroupEmployees, bool>> exp)
        {
            List<tb_ProjectGroupEmployees> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupEmployees>().Where(exp)
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProjectGroupEmployees>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupEmployees>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProjectGroupEmployeesServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProjectGroupEmployees entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupEmployees>().Where(w => w.ProjectGroupEmpID == (long)id)
                             .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupEmployees>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



