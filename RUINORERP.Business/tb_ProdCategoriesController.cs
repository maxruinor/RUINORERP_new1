
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
    /// 产品类别表 与行业相关的产品分类
    /// </summary>
    public partial class tb_ProdCategoriesController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProdCategoriesServices _tb_ProdCategoriesServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProdCategoriesController(ILogger<tb_ProdCategoriesController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProdCategoriesServices tb_ProdCategoriesServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProdCategoriesServices = tb_ProdCategoriesServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProdCategories info)
        {

           // tb_ProdCategoriesValidator validator = new tb_ProdCategoriesValidator();
           tb_ProdCategoriesValidator validator = _appContext.GetRequiredService<tb_ProdCategoriesValidator>();
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
        public async Task<ReturnResults<tb_ProdCategories>> SaveOrUpdate(tb_ProdCategories entity)
        {
            ReturnResults<tb_ProdCategories> rr = new ReturnResults<tb_ProdCategories>();
            tb_ProdCategories Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Category_ID > 0)
                {
                    bool rs = await _tb_ProdCategoriesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProdCategoriesServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(entity);
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
            tb_ProdCategories entity = model as tb_ProdCategories;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Category_ID > 0)
                {
                    bool rs = await _tb_ProdCategoriesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProdCategoriesServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(entity);
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
            List<T> list = await _tb_ProdCategoriesServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProdCategories entity = item as tb_ProdCategories;
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
            List<T> list = await _tb_ProdCategoriesServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProdCategories entity = item as tb_ProdCategories;
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
            tb_ProdCategories entity = model as tb_ProdCategories;
            bool rs = await _tb_ProdCategoriesServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProdCategories>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProdCategories> entitys = models as List<tb_ProdCategories>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProdCategories>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Category_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProdCategories>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProdCategoriesValidator validator = new tb_ProdCategoriesValidator();
           tb_ProdCategoriesValidator validator = _appContext.GetRequiredService<tb_ProdCategoriesValidator>();
            ValidationResult results = validator.Validate(info as tb_ProdCategories);
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

                tb_ProdCategories entity = model as tb_ProdCategories;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Category_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProdCategories>(entity as tb_ProdCategories)
                        .Include(m => m.tb_Prods)
                    .Include(m => m.tb_ProdCategorieses_parents)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProdCategories>(entity as tb_ProdCategories)
                .Include(m => m.tb_Prods)
                .Include(m => m.tb_ProdCategorieses_parents)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Category_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdCategories>()
                                .Includes(m => m.tb_Prods)
                        .Includes(m => m.tb_ProdCategorieses_parents)
                                        .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProdCategories entity = model as tb_ProdCategories;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProdCategories>(m => m.Category_ID== entity.Category_ID)
                                .Include(m => m.tb_Prods)
                        .Include(m => m.tb_ProdCategorieses_parents)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProdCategories AddReEntity(tb_ProdCategories entity)
        {
            tb_ProdCategories AddEntity =  _tb_ProdCategoriesServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProdCategories> AddReEntityAsync(tb_ProdCategories entity)
        {
            tb_ProdCategories AddEntity = await _tb_ProdCategoriesServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProdCategories entity)
        {
            long id = await _tb_ProdCategoriesServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProdCategories> infos)
        {
            List<long> ids = await _tb_ProdCategoriesServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProdCategories entity)
        {
            bool rs = await _tb_ProdCategoriesServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdCategories>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProdCategories entity)
        {
            bool rs = await _tb_ProdCategoriesServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProdCategoriesServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdCategories>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProdCategoriesServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdCategories>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProdCategories>> QueryAsync()
        {
            List<tb_ProdCategories> list = await  _tb_ProdCategoriesServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(list);
            return list;
        }
        
        public virtual List<tb_ProdCategories> Query()
        {
            List<tb_ProdCategories> list =  _tb_ProdCategoriesServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(list);
            return list;
        }
        
        public virtual List<tb_ProdCategories> Query(string wheresql)
        {
            List<tb_ProdCategories> list =  _tb_ProdCategoriesServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProdCategories>> QueryAsync(string wheresql) 
        {
            List<tb_ProdCategories> list = await _tb_ProdCategoriesServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProdCategories>> QueryAsync(Expression<Func<tb_ProdCategories, bool>> exp)
        {
            List<tb_ProdCategories> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdCategories>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdCategories>> QueryByNavAsync()
        {
            List<tb_ProdCategories> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdCategories>()
                               .Includes(t => t.tb_ProdCategorieses_parents)
                                            .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_ProdCategorieses_parents)
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdCategories>> QueryByNavAsync(Expression<Func<tb_ProdCategories, bool>> exp)
        {
            List<tb_ProdCategories> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdCategories>().Where(exp)
                               .Includes(t => t.tb_ProdCategorieses_parents)
                                            .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_ProdCategorieses_parents)
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProdCategories> QueryByNav(Expression<Func<tb_ProdCategories, bool>> exp)
        {
            List<tb_ProdCategories> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdCategories>().Where(exp)
                            .Includes(t => t.tb_ProdCategorieses_parents)
                                        .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_ProdCategorieses_parents)
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProdCategories>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdCategories>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProdCategoriesServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProdCategories entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdCategories>().Where(w => w.Category_ID == (long)id)
                             .Includes(t => t.tb_ProdCategorieses_parents)
                                        .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_ProdCategorieses_parents)
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProdCategories>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



