﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:46
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
    /// 省份表
    /// </summary>
    public partial class tb_ProvincesController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProvincesServices _tb_ProvincesServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProvincesController(ILogger<tb_ProvincesController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProvincesServices tb_ProvincesServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProvincesServices = tb_ProvincesServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_Provinces info)
        {
            tb_ProvincesValidator validator = new tb_ProvincesValidator();
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
        public async Task<ReturnResults<tb_Provinces>> SaveOrUpdate(tb_Provinces entity)
        {
            ReturnResults<tb_Provinces> rr = new ReturnResults<tb_Provinces>();
            tb_Provinces Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProvinceID > 0)
                {
                    bool rs = await _tb_ProvincesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProvincesServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(entity);
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
            tb_Provinces entity = model as tb_Provinces;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProvinceID > 0)
                {
                    bool rs = await _tb_ProvincesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProvincesServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(entity);
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
            List<T> list = await _tb_ProvincesServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Provinces entity = item as tb_Provinces;
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
            List<T> list = await _tb_ProvincesServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Provinces entity = item as tb_Provinces;
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
            tb_Provinces entity = model as tb_Provinces;
            bool rs = await _tb_ProvincesServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Provinces>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Provinces> entitys = models as List<tb_Provinces>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Provinces>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ProvinceID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Provinces>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_ProvincesValidator validator = new tb_ProvincesValidator();
            ValidationResult results = validator.Validate(info as tb_Provinces);
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
                tb_Provinces entity = model as tb_Provinces;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ProvinceID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Provinces>(entity as tb_Provinces)
                        .Include(m => m.tb_Citieses)
                    .Include(m => m.tb_CRM_Customers)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Provinces>(entity as tb_Provinces)
                .Include(m => m.tb_Citieses)
                .Include(m => m.tb_CRM_Customers)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ProvinceID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Provinces>()
                                .Includes(m => m.tb_Citieses)
                        .Includes(m => m.tb_CRM_Customers)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Provinces entity = model as tb_Provinces;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Provinces>(m => m.ProvinceID== entity.ProvinceID)
                                .Include(m => m.tb_Citieses)
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
        
        
        
        public tb_Provinces AddReEntity(tb_Provinces entity)
        {
            tb_Provinces AddEntity =  _tb_ProvincesServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Provinces> AddReEntityAsync(tb_Provinces entity)
        {
            tb_Provinces AddEntity = await _tb_ProvincesServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Provinces entity)
        {
            long id = await _tb_ProvincesServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Provinces> infos)
        {
            List<long> ids = await _tb_ProvincesServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Provinces entity)
        {
            bool rs = await _tb_ProvincesServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Provinces>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Provinces entity)
        {
            bool rs = await _tb_ProvincesServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProvincesServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Provinces>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProvincesServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Provinces>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Provinces>> QueryAsync()
        {
            List<tb_Provinces> list = await  _tb_ProvincesServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(list);
            return list;
        }
        
        public virtual List<tb_Provinces> Query()
        {
            List<tb_Provinces> list =  _tb_ProvincesServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(list);
            return list;
        }
        
        public virtual List<tb_Provinces> Query(string wheresql)
        {
            List<tb_Provinces> list =  _tb_ProvincesServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Provinces>> QueryAsync(string wheresql) 
        {
            List<tb_Provinces> list = await _tb_ProvincesServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Provinces>> QueryAsync(Expression<Func<tb_Provinces, bool>> exp)
        {
            List<tb_Provinces> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Provinces>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Provinces>> QueryByNavAsync()
        {
            List<tb_Provinces> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Provinces>()
                                            .Includes(t => t.tb_Citieses )
                                .Includes(t => t.tb_CRM_Customers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Provinces>> QueryByNavAsync(Expression<Func<tb_Provinces, bool>> exp)
        {
            List<tb_Provinces> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Provinces>().Where(exp)
                                            .Includes(t => t.tb_Citieses )
                                .Includes(t => t.tb_CRM_Customers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Provinces> QueryByNav(Expression<Func<tb_Provinces, bool>> exp)
        {
            List<tb_Provinces> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Provinces>().Where(exp)
                                        .Includes(t => t.tb_Citieses )
                            .Includes(t => t.tb_CRM_Customers )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Provinces>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Provinces>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProvincesServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Provinces entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Provinces>().Where(w => w.ProvinceID == (long)id)
                                         .Includes(t => t.tb_Citieses )
                            .Includes(t => t.tb_CRM_Customers )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Provinces>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}


