
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:48
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
    /// 产品属性表
    /// </summary>
    public partial class tb_ProdPropertyController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProdPropertyServices _tb_ProdPropertyServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProdPropertyController(ILogger<tb_ProdPropertyController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProdPropertyServices tb_ProdPropertyServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProdPropertyServices = tb_ProdPropertyServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProdProperty info)
        {

           // tb_ProdPropertyValidator validator = new tb_ProdPropertyValidator();
           tb_ProdPropertyValidator validator = _appContext.GetRequiredService<tb_ProdPropertyValidator>();
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
        public async Task<ReturnResults<tb_ProdProperty>> SaveOrUpdate(tb_ProdProperty entity)
        {
            ReturnResults<tb_ProdProperty> rr = new ReturnResults<tb_ProdProperty>();
            tb_ProdProperty Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Property_ID > 0)
                {
                    bool rs = await _tb_ProdPropertyServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProdPropertyServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(entity);
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
            tb_ProdProperty entity = model as tb_ProdProperty;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Property_ID > 0)
                {
                    bool rs = await _tb_ProdPropertyServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProdPropertyServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(entity);
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
            List<T> list = await _tb_ProdPropertyServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProdProperty entity = item as tb_ProdProperty;
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
            List<T> list = await _tb_ProdPropertyServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProdProperty entity = item as tb_ProdProperty;
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
            tb_ProdProperty entity = model as tb_ProdProperty;
            bool rs = await _tb_ProdPropertyServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProdProperty>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProdProperty> entitys = models as List<tb_ProdProperty>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProdProperty>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Property_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProdProperty>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProdPropertyValidator validator = new tb_ProdPropertyValidator();
           tb_ProdPropertyValidator validator = _appContext.GetRequiredService<tb_ProdPropertyValidator>();
            ValidationResult results = validator.Validate(info as tb_ProdProperty);
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

                tb_ProdProperty entity = model as tb_ProdProperty;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Property_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProdProperty>(entity as tb_ProdProperty)
                        .Include(m => m.tb_ProdPropertyValues)
                    .Include(m => m.tb_Prod_Attr_Relations)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProdProperty>(entity as tb_ProdProperty)
                .Include(m => m.tb_ProdPropertyValues)
                .Include(m => m.tb_Prod_Attr_Relations)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Property_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdProperty>()
                                .Includes(m => m.tb_ProdPropertyValues)
                        .Includes(m => m.tb_Prod_Attr_Relations)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProdProperty entity = model as tb_ProdProperty;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProdProperty>(m => m.Property_ID== entity.Property_ID)
                                .Include(m => m.tb_ProdPropertyValues)
                        .Include(m => m.tb_Prod_Attr_Relations)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProdProperty AddReEntity(tb_ProdProperty entity)
        {
            tb_ProdProperty AddEntity =  _tb_ProdPropertyServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProdProperty> AddReEntityAsync(tb_ProdProperty entity)
        {
            tb_ProdProperty AddEntity = await _tb_ProdPropertyServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProdProperty entity)
        {
            long id = await _tb_ProdPropertyServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProdProperty> infos)
        {
            List<long> ids = await _tb_ProdPropertyServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProdProperty entity)
        {
            bool rs = await _tb_ProdPropertyServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdProperty>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProdProperty entity)
        {
            bool rs = await _tb_ProdPropertyServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProdPropertyServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdProperty>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProdPropertyServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdProperty>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProdProperty>> QueryAsync()
        {
            List<tb_ProdProperty> list = await  _tb_ProdPropertyServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(list);
            return list;
        }
        
        public virtual List<tb_ProdProperty> Query()
        {
            List<tb_ProdProperty> list =  _tb_ProdPropertyServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(list);
            return list;
        }
        
        public virtual List<tb_ProdProperty> Query(string wheresql)
        {
            List<tb_ProdProperty> list =  _tb_ProdPropertyServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProdProperty>> QueryAsync(string wheresql) 
        {
            List<tb_ProdProperty> list = await _tb_ProdPropertyServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProdProperty>> QueryAsync(Expression<Func<tb_ProdProperty, bool>> exp)
        {
            List<tb_ProdProperty> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdProperty>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdProperty>> QueryByNavAsync()
        {
            List<tb_ProdProperty> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdProperty>()
                                            .Includes(t => t.tb_ProdPropertyValues )
                                .Includes(t => t.tb_Prod_Attr_Relations )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdProperty>> QueryByNavAsync(Expression<Func<tb_ProdProperty, bool>> exp)
        {
            List<tb_ProdProperty> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdProperty>().Where(exp)
                                            .Includes(t => t.tb_ProdPropertyValues )
                                .Includes(t => t.tb_Prod_Attr_Relations )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProdProperty> QueryByNav(Expression<Func<tb_ProdProperty, bool>> exp)
        {
            List<tb_ProdProperty> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdProperty>().Where(exp)
                                        .Includes(t => t.tb_ProdPropertyValues )
                            .Includes(t => t.tb_Prod_Attr_Relations )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProdProperty>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdProperty>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProdPropertyServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProdProperty entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdProperty>().Where(w => w.Property_ID == (long)id)
                                         .Includes(t => t.tb_ProdPropertyValues )
                            .Includes(t => t.tb_Prod_Attr_Relations )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProdProperty>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



