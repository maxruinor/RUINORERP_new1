
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:09
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
    /// 货品基本信息表
    /// </summary>
    public partial class tb_ProdController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProdServices _tb_ProdServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProdController(ILogger<tb_ProdController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProdServices tb_ProdServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProdServices = tb_ProdServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_Prod info)
        {
           // tb_ProdValidator validator = new tb_ProdValidator();
           tb_ProdValidator validator = _appContext.GetRequiredService<tb_ProdValidator>();
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
        public async Task<ReturnResults<tb_Prod>> SaveOrUpdate(tb_Prod entity)
        {
            ReturnResults<tb_Prod> rr = new ReturnResults<tb_Prod>();
            tb_Prod Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProdBaseID > 0)
                {
                    bool rs = await _tb_ProdServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Prod>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProdServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Prod>(entity);
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
            tb_Prod entity = model as tb_Prod;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProdBaseID > 0)
                {
                    bool rs = await _tb_ProdServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Prod>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProdServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Prod>(entity);
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
            List<T> list = await _tb_ProdServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Prod entity = item as tb_Prod;
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
            List<T> list = await _tb_ProdServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Prod entity = item as tb_Prod;
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
            tb_Prod entity = model as tb_Prod;
            bool rs = await _tb_ProdServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Prod>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Prod> entitys = models as List<tb_Prod>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Prod>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ProdBaseID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Prod>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProdValidator validator = new tb_ProdValidator();
           tb_ProdValidator validator = _appContext.GetRequiredService<tb_ProdValidator>();
            ValidationResult results = validator.Validate(info as tb_Prod);
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
                tb_Prod entity = model as tb_Prod;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ProdBaseID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Prod>(entity as tb_Prod)
                        .Include(m => m.tb_ProdDetails)
                    .Include(m => m.tb_Prod_Attr_Relations)
                    .Include(m => m.tb_Packings)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Prod>(entity as tb_Prod)
                .Include(m => m.tb_ProdDetails)
                .Include(m => m.tb_Prod_Attr_Relations)
                .Include(m => m.tb_Packings)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ProdBaseID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Prod>()
                                .Includes(m => m.tb_ProdDetails)
                        .Includes(m => m.tb_Prod_Attr_Relations)
                        .Includes(m => m.tb_Packings)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Prod entity = model as tb_Prod;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Prod>(m => m.ProdBaseID== entity.ProdBaseID)
                                .Include(m => m.tb_ProdDetails)
                        .Include(m => m.tb_Prod_Attr_Relations)
                        .Include(m => m.tb_Packings)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Prod AddReEntity(tb_Prod entity)
        {
            tb_Prod AddEntity =  _tb_ProdServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Prod> AddReEntityAsync(tb_Prod entity)
        {
            tb_Prod AddEntity = await _tb_ProdServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Prod entity)
        {
            long id = await _tb_ProdServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Prod>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Prod> infos)
        {
            List<long> ids = await _tb_ProdServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Prod>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Prod entity)
        {
            bool rs = await _tb_ProdServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Prod>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Prod entity)
        {
            bool rs = await _tb_ProdServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Prod>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProdServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Prod>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProdServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Prod>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Prod>> QueryAsync()
        {
            List<tb_Prod> list = await  _tb_ProdServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(list);
            return list;
        }
        
        public virtual List<tb_Prod> Query()
        {
            List<tb_Prod> list =  _tb_ProdServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(list);
            return list;
        }
        
        public virtual List<tb_Prod> Query(string wheresql)
        {
            List<tb_Prod> list =  _tb_ProdServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Prod>> QueryAsync(string wheresql) 
        {
            List<tb_Prod> list = await _tb_ProdServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Prod>> QueryAsync(Expression<Func<tb_Prod, bool>> exp)
        {
            List<tb_Prod> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Prod>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Prod>> QueryByNavAsync()
        {
            List<tb_Prod> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Prod>()
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_prodcategories )
                               .Includes(t => t.tb_producttype )
                               .Includes(t => t.tb_unit )
                               .Includes(t => t.tb_storagerack )
                                            .Includes(t => t.tb_ProdDetails )
                                .Includes(t => t.tb_Prod_Attr_Relations )
                                .Includes(t => t.tb_Packings )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Prod>> QueryByNavAsync(Expression<Func<tb_Prod, bool>> exp)
        {
            List<tb_Prod> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Prod>().Where(exp)
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_prodcategories )
                               .Includes(t => t.tb_producttype )
                               .Includes(t => t.tb_unit )
                               .Includes(t => t.tb_storagerack )
                                            .Includes(t => t.tb_ProdDetails )
                                .Includes(t => t.tb_Prod_Attr_Relations )
                                .Includes(t => t.tb_Packings )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Prod> QueryByNav(Expression<Func<tb_Prod, bool>> exp)
        {
            List<tb_Prod> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Prod>().Where(exp)
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_prodcategories )
                            .Includes(t => t.tb_producttype )
                            .Includes(t => t.tb_unit )
                            .Includes(t => t.tb_storagerack )
                                        .Includes(t => t.tb_ProdDetails )
                            .Includes(t => t.tb_Prod_Attr_Relations )
                            .Includes(t => t.tb_Packings )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Prod>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Prod>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProdServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Prod entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Prod>().Where(w => w.ProdBaseID == (long)id)
                             .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_prodcategories )
                            .Includes(t => t.tb_producttype )
                            .Includes(t => t.tb_unit )
                            .Includes(t => t.tb_storagerack )
                                        .Includes(t => t.tb_ProdDetails )
                            .Includes(t => t.tb_Prod_Attr_Relations )
                            .Includes(t => t.tb_Packings )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Prod>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



