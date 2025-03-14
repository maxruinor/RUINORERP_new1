
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
    /// 产品组合单
    /// </summary>
    public partial class tb_ProdMergeController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProdMergeServices _tb_ProdMergeServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProdMergeController(ILogger<tb_ProdMergeController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProdMergeServices tb_ProdMergeServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProdMergeServices = tb_ProdMergeServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProdMerge info)
        {

           // tb_ProdMergeValidator validator = new tb_ProdMergeValidator();
           tb_ProdMergeValidator validator = _appContext.GetRequiredService<tb_ProdMergeValidator>();
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
        public async Task<ReturnResults<tb_ProdMerge>> SaveOrUpdate(tb_ProdMerge entity)
        {
            ReturnResults<tb_ProdMerge> rr = new ReturnResults<tb_ProdMerge>();
            tb_ProdMerge Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MergeID > 0)
                {
                    bool rs = await _tb_ProdMergeServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProdMergeServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(entity);
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
            tb_ProdMerge entity = model as tb_ProdMerge;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MergeID > 0)
                {
                    bool rs = await _tb_ProdMergeServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProdMergeServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(entity);
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
            List<T> list = await _tb_ProdMergeServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProdMerge entity = item as tb_ProdMerge;
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
            List<T> list = await _tb_ProdMergeServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProdMerge entity = item as tb_ProdMerge;
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
            tb_ProdMerge entity = model as tb_ProdMerge;
            bool rs = await _tb_ProdMergeServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProdMerge>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProdMerge> entitys = models as List<tb_ProdMerge>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProdMerge>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.MergeID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProdMerge>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProdMergeValidator validator = new tb_ProdMergeValidator();
           tb_ProdMergeValidator validator = _appContext.GetRequiredService<tb_ProdMergeValidator>();
            ValidationResult results = validator.Validate(info as tb_ProdMerge);
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

                tb_ProdMerge entity = model as tb_ProdMerge;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.MergeID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProdMerge>(entity as tb_ProdMerge)
                        .Include(m => m.tb_ProdMergeDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProdMerge>(entity as tb_ProdMerge)
                .Include(m => m.tb_ProdMergeDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.MergeID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdMerge>()
                                .Includes(m => m.tb_ProdMergeDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProdMerge entity = model as tb_ProdMerge;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProdMerge>(m => m.MergeID== entity.MergeID)
                                .Include(m => m.tb_ProdMergeDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProdMerge AddReEntity(tb_ProdMerge entity)
        {
            tb_ProdMerge AddEntity =  _tb_ProdMergeServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProdMerge> AddReEntityAsync(tb_ProdMerge entity)
        {
            tb_ProdMerge AddEntity = await _tb_ProdMergeServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProdMerge entity)
        {
            long id = await _tb_ProdMergeServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProdMerge> infos)
        {
            List<long> ids = await _tb_ProdMergeServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProdMerge entity)
        {
            bool rs = await _tb_ProdMergeServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdMerge>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProdMerge entity)
        {
            bool rs = await _tb_ProdMergeServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProdMergeServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdMerge>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProdMergeServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdMerge>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProdMerge>> QueryAsync()
        {
            List<tb_ProdMerge> list = await  _tb_ProdMergeServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(list);
            return list;
        }
        
        public virtual List<tb_ProdMerge> Query()
        {
            List<tb_ProdMerge> list =  _tb_ProdMergeServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(list);
            return list;
        }
        
        public virtual List<tb_ProdMerge> Query(string wheresql)
        {
            List<tb_ProdMerge> list =  _tb_ProdMergeServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProdMerge>> QueryAsync(string wheresql) 
        {
            List<tb_ProdMerge> list = await _tb_ProdMergeServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProdMerge>> QueryAsync(Expression<Func<tb_ProdMerge, bool>> exp)
        {
            List<tb_ProdMerge> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdMerge>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdMerge>> QueryByNavAsync()
        {
            List<tb_ProdMerge> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdMerge>()
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                                            .Includes(t => t.tb_ProdMergeDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdMerge>> QueryByNavAsync(Expression<Func<tb_ProdMerge, bool>> exp)
        {
            List<tb_ProdMerge> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdMerge>().Where(exp)
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                                            .Includes(t => t.tb_ProdMergeDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProdMerge> QueryByNav(Expression<Func<tb_ProdMerge, bool>> exp)
        {
            List<tb_ProdMerge> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdMerge>().Where(exp)
                            .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                                        .Includes(t => t.tb_ProdMergeDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProdMerge>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdMerge>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProdMergeServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProdMerge entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdMerge>().Where(w => w.MergeID == (long)id)
                             .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                                        .Includes(t => t.tb_ProdMergeDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProdMerge>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



