
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:37
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
    /// 城市表
    /// </summary>
    public partial class tb_CitiesController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CitiesServices _tb_CitiesServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_CitiesController(ILogger<tb_CitiesController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CitiesServices tb_CitiesServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CitiesServices = tb_CitiesServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_Cities info)
        {

           // tb_CitiesValidator validator = new tb_CitiesValidator();
           tb_CitiesValidator validator = _appContext.GetRequiredService<tb_CitiesValidator>();
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
        public async Task<ReturnResults<tb_Cities>> SaveOrUpdate(tb_Cities entity)
        {
            ReturnResults<tb_Cities> rr = new ReturnResults<tb_Cities>();
            tb_Cities Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.CityID > 0)
                {
                    bool rs = await _tb_CitiesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Cities>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CitiesServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Cities>(entity);
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
            tb_Cities entity = model as tb_Cities;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.CityID > 0)
                {
                    bool rs = await _tb_CitiesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Cities>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CitiesServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Cities>(entity);
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
            List<T> list = await _tb_CitiesServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Cities entity = item as tb_Cities;
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
            List<T> list = await _tb_CitiesServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Cities entity = item as tb_Cities;
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
            tb_Cities entity = model as tb_Cities;
            bool rs = await _tb_CitiesServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Cities>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Cities> entitys = models as List<tb_Cities>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Cities>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.CityID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Cities>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_CitiesValidator validator = new tb_CitiesValidator();
           tb_CitiesValidator validator = _appContext.GetRequiredService<tb_CitiesValidator>();
            ValidationResult results = validator.Validate(info as tb_Cities);
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

                tb_Cities entity = model as tb_Cities;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.CityID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Cities>(entity as tb_Cities)
                        .Include(m => m.tb_CRM_Customers)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Cities>(entity as tb_Cities)
                .Include(m => m.tb_CRM_Customers)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.CityID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Cities>()
                                .Includes(m => m.tb_CRM_Customers)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Cities entity = model as tb_Cities;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Cities>(m => m.CityID== entity.CityID)
                                .Include(m => m.tb_CRM_Customers)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Cities AddReEntity(tb_Cities entity)
        {
            tb_Cities AddEntity =  _tb_CitiesServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Cities> AddReEntityAsync(tb_Cities entity)
        {
            tb_Cities AddEntity = await _tb_CitiesServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Cities entity)
        {
            long id = await _tb_CitiesServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Cities>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Cities> infos)
        {
            List<long> ids = await _tb_CitiesServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Cities>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Cities entity)
        {
            bool rs = await _tb_CitiesServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Cities>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Cities entity)
        {
            bool rs = await _tb_CitiesServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Cities>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CitiesServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Cities>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CitiesServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Cities>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Cities>> QueryAsync()
        {
            List<tb_Cities> list = await  _tb_CitiesServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(list);
            return list;
        }
        
        public virtual List<tb_Cities> Query()
        {
            List<tb_Cities> list =  _tb_CitiesServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(list);
            return list;
        }
        
        public virtual List<tb_Cities> Query(string wheresql)
        {
            List<tb_Cities> list =  _tb_CitiesServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Cities>> QueryAsync(string wheresql) 
        {
            List<tb_Cities> list = await _tb_CitiesServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Cities>> QueryAsync(Expression<Func<tb_Cities, bool>> exp)
        {
            List<tb_Cities> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Cities>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Cities>> QueryByNavAsync()
        {
            List<tb_Cities> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Cities>()
                               .Includes(t => t.tb_provinces )
                                            .Includes(t => t.tb_CRM_Customers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Cities>> QueryByNavAsync(Expression<Func<tb_Cities, bool>> exp)
        {
            List<tb_Cities> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Cities>().Where(exp)
                               .Includes(t => t.tb_provinces )
                                            .Includes(t => t.tb_CRM_Customers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Cities> QueryByNav(Expression<Func<tb_Cities, bool>> exp)
        {
            List<tb_Cities> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Cities>().Where(exp)
                            .Includes(t => t.tb_provinces )
                                        .Includes(t => t.tb_CRM_Customers )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Cities>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Cities>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CitiesServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Cities entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Cities>().Where(w => w.CityID == (long)id)
                             .Includes(t => t.tb_provinces )
                                        .Includes(t => t.tb_CRM_Customers )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Cities>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



