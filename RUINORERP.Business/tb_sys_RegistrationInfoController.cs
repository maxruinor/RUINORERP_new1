
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:54
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
    /// 系统注册信息
    /// </summary>
    public partial class tb_sys_RegistrationInfoController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_sys_RegistrationInfoServices _tb_sys_RegistrationInfoServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_sys_RegistrationInfoController(ILogger<tb_sys_RegistrationInfoController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_sys_RegistrationInfoServices tb_sys_RegistrationInfoServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_sys_RegistrationInfoServices = tb_sys_RegistrationInfoServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_sys_RegistrationInfo info)
        {

           // tb_sys_RegistrationInfoValidator validator = new tb_sys_RegistrationInfoValidator();
           tb_sys_RegistrationInfoValidator validator = _appContext.GetRequiredService<tb_sys_RegistrationInfoValidator>();
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
        public async Task<ReturnResults<tb_sys_RegistrationInfo>> SaveOrUpdate(tb_sys_RegistrationInfo entity)
        {
            ReturnResults<tb_sys_RegistrationInfo> rr = new ReturnResults<tb_sys_RegistrationInfo>();
            tb_sys_RegistrationInfo Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RegistrationInfoD > 0)
                {
                    bool rs = await _tb_sys_RegistrationInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_sys_RegistrationInfoServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(entity);
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
            tb_sys_RegistrationInfo entity = model as tb_sys_RegistrationInfo;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RegistrationInfoD > 0)
                {
                    bool rs = await _tb_sys_RegistrationInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_sys_RegistrationInfoServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(entity);
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
            List<T> list = await _tb_sys_RegistrationInfoServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_sys_RegistrationInfo entity = item as tb_sys_RegistrationInfo;
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
            List<T> list = await _tb_sys_RegistrationInfoServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_sys_RegistrationInfo entity = item as tb_sys_RegistrationInfo;
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
            tb_sys_RegistrationInfo entity = model as tb_sys_RegistrationInfo;
            bool rs = await _tb_sys_RegistrationInfoServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_sys_RegistrationInfo>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_sys_RegistrationInfo> entitys = models as List<tb_sys_RegistrationInfo>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_sys_RegistrationInfo>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.RegistrationInfoD).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_sys_RegistrationInfo>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_sys_RegistrationInfoValidator validator = new tb_sys_RegistrationInfoValidator();
           tb_sys_RegistrationInfoValidator validator = _appContext.GetRequiredService<tb_sys_RegistrationInfoValidator>();
            ValidationResult results = validator.Validate(info as tb_sys_RegistrationInfo);
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

                tb_sys_RegistrationInfo entity = model as tb_sys_RegistrationInfo;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.RegistrationInfoD > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_sys_RegistrationInfo>(entity as tb_sys_RegistrationInfo)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_sys_RegistrationInfo>(entity as tb_sys_RegistrationInfo)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.RegistrationInfoD;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_sys_RegistrationInfo>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_sys_RegistrationInfo entity = model as tb_sys_RegistrationInfo;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_sys_RegistrationInfo>(m => m.RegistrationInfoD== entity.RegistrationInfoD)
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
        
        
        
        public tb_sys_RegistrationInfo AddReEntity(tb_sys_RegistrationInfo entity)
        {
            tb_sys_RegistrationInfo AddEntity =  _tb_sys_RegistrationInfoServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_sys_RegistrationInfo> AddReEntityAsync(tb_sys_RegistrationInfo entity)
        {
            tb_sys_RegistrationInfo AddEntity = await _tb_sys_RegistrationInfoServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_sys_RegistrationInfo entity)
        {
            long id = await _tb_sys_RegistrationInfoServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_sys_RegistrationInfo> infos)
        {
            List<long> ids = await _tb_sys_RegistrationInfoServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_sys_RegistrationInfo entity)
        {
            bool rs = await _tb_sys_RegistrationInfoServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_sys_RegistrationInfo>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_sys_RegistrationInfo entity)
        {
            bool rs = await _tb_sys_RegistrationInfoServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_sys_RegistrationInfoServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_sys_RegistrationInfo>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_sys_RegistrationInfoServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_sys_RegistrationInfo>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_sys_RegistrationInfo>> QueryAsync()
        {
            List<tb_sys_RegistrationInfo> list = await  _tb_sys_RegistrationInfoServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(list);
            return list;
        }
        
        public virtual List<tb_sys_RegistrationInfo> Query()
        {
            List<tb_sys_RegistrationInfo> list =  _tb_sys_RegistrationInfoServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(list);
            return list;
        }
        
        public virtual List<tb_sys_RegistrationInfo> Query(string wheresql)
        {
            List<tb_sys_RegistrationInfo> list =  _tb_sys_RegistrationInfoServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(list);
            return list;
        }
        
        public virtual async Task<List<tb_sys_RegistrationInfo>> QueryAsync(string wheresql) 
        {
            List<tb_sys_RegistrationInfo> list = await _tb_sys_RegistrationInfoServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_sys_RegistrationInfo>> QueryAsync(Expression<Func<tb_sys_RegistrationInfo, bool>> exp)
        {
            List<tb_sys_RegistrationInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_sys_RegistrationInfo>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_sys_RegistrationInfo>> QueryByNavAsync()
        {
            List<tb_sys_RegistrationInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_sys_RegistrationInfo>()
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_sys_RegistrationInfo>> QueryByNavAsync(Expression<Func<tb_sys_RegistrationInfo, bool>> exp)
        {
            List<tb_sys_RegistrationInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_sys_RegistrationInfo>().Where(exp)
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_sys_RegistrationInfo> QueryByNav(Expression<Func<tb_sys_RegistrationInfo, bool>> exp)
        {
            List<tb_sys_RegistrationInfo> list = _unitOfWorkManage.GetDbClient().Queryable<tb_sys_RegistrationInfo>().Where(exp)
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_sys_RegistrationInfo>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_sys_RegistrationInfo>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_sys_RegistrationInfoServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_sys_RegistrationInfo entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_sys_RegistrationInfo>().Where(w => w.RegistrationInfoD == (long)id)
                                     .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_sys_RegistrationInfo>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



