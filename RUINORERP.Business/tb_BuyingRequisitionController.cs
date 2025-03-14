
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
    /// 请购单，可能来自销售订单,也可以来自其它日常需求也可能来自生产需求也可以直接录数据，是一个纯业务性的数据表
    /// </summary>
    public partial class tb_BuyingRequisitionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_BuyingRequisitionServices _tb_BuyingRequisitionServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_BuyingRequisitionController(ILogger<tb_BuyingRequisitionController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_BuyingRequisitionServices tb_BuyingRequisitionServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_BuyingRequisitionServices = tb_BuyingRequisitionServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_BuyingRequisition info)
        {

           // tb_BuyingRequisitionValidator validator = new tb_BuyingRequisitionValidator();
           tb_BuyingRequisitionValidator validator = _appContext.GetRequiredService<tb_BuyingRequisitionValidator>();
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
        public async Task<ReturnResults<tb_BuyingRequisition>> SaveOrUpdate(tb_BuyingRequisition entity)
        {
            ReturnResults<tb_BuyingRequisition> rr = new ReturnResults<tb_BuyingRequisition>();
            tb_BuyingRequisition Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PuRequisition_ID > 0)
                {
                    bool rs = await _tb_BuyingRequisitionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_BuyingRequisitionServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(entity);
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
            tb_BuyingRequisition entity = model as tb_BuyingRequisition;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PuRequisition_ID > 0)
                {
                    bool rs = await _tb_BuyingRequisitionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_BuyingRequisitionServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(entity);
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
            List<T> list = await _tb_BuyingRequisitionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_BuyingRequisition entity = item as tb_BuyingRequisition;
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
            List<T> list = await _tb_BuyingRequisitionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_BuyingRequisition entity = item as tb_BuyingRequisition;
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
            tb_BuyingRequisition entity = model as tb_BuyingRequisition;
            bool rs = await _tb_BuyingRequisitionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_BuyingRequisition>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_BuyingRequisition> entitys = models as List<tb_BuyingRequisition>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_BuyingRequisition>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PuRequisition_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_BuyingRequisition>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_BuyingRequisitionValidator validator = new tb_BuyingRequisitionValidator();
           tb_BuyingRequisitionValidator validator = _appContext.GetRequiredService<tb_BuyingRequisitionValidator>();
            ValidationResult results = validator.Validate(info as tb_BuyingRequisition);
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

                tb_BuyingRequisition entity = model as tb_BuyingRequisition;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PuRequisition_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_BuyingRequisition>(entity as tb_BuyingRequisition)
                        .Include(m => m.tb_BuyingRequisitionDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_BuyingRequisition>(entity as tb_BuyingRequisition)
                .Include(m => m.tb_BuyingRequisitionDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PuRequisition_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BuyingRequisition>()
                                .Includes(m => m.tb_BuyingRequisitionDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_BuyingRequisition entity = model as tb_BuyingRequisition;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_BuyingRequisition>(m => m.PuRequisition_ID== entity.PuRequisition_ID)
                                .Include(m => m.tb_BuyingRequisitionDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_BuyingRequisition AddReEntity(tb_BuyingRequisition entity)
        {
            tb_BuyingRequisition AddEntity =  _tb_BuyingRequisitionServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_BuyingRequisition> AddReEntityAsync(tb_BuyingRequisition entity)
        {
            tb_BuyingRequisition AddEntity = await _tb_BuyingRequisitionServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_BuyingRequisition entity)
        {
            long id = await _tb_BuyingRequisitionServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_BuyingRequisition> infos)
        {
            List<long> ids = await _tb_BuyingRequisitionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_BuyingRequisition entity)
        {
            bool rs = await _tb_BuyingRequisitionServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BuyingRequisition>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_BuyingRequisition entity)
        {
            bool rs = await _tb_BuyingRequisitionServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_BuyingRequisitionServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BuyingRequisition>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_BuyingRequisitionServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BuyingRequisition>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_BuyingRequisition>> QueryAsync()
        {
            List<tb_BuyingRequisition> list = await  _tb_BuyingRequisitionServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(list);
            return list;
        }
        
        public virtual List<tb_BuyingRequisition> Query()
        {
            List<tb_BuyingRequisition> list =  _tb_BuyingRequisitionServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(list);
            return list;
        }
        
        public virtual List<tb_BuyingRequisition> Query(string wheresql)
        {
            List<tb_BuyingRequisition> list =  _tb_BuyingRequisitionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(list);
            return list;
        }
        
        public virtual async Task<List<tb_BuyingRequisition>> QueryAsync(string wheresql) 
        {
            List<tb_BuyingRequisition> list = await _tb_BuyingRequisitionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_BuyingRequisition>> QueryAsync(Expression<Func<tb_BuyingRequisition, bool>> exp)
        {
            List<tb_BuyingRequisition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BuyingRequisition>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BuyingRequisition>> QueryByNavAsync()
        {
            List<tb_BuyingRequisition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BuyingRequisition>()
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_location )
                                            .Includes(t => t.tb_BuyingRequisitionDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BuyingRequisition>> QueryByNavAsync(Expression<Func<tb_BuyingRequisition, bool>> exp)
        {
            List<tb_BuyingRequisition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BuyingRequisition>().Where(exp)
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_location )
                                            .Includes(t => t.tb_BuyingRequisitionDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_BuyingRequisition> QueryByNav(Expression<Func<tb_BuyingRequisition, bool>> exp)
        {
            List<tb_BuyingRequisition> list = _unitOfWorkManage.GetDbClient().Queryable<tb_BuyingRequisition>().Where(exp)
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_location )
                                        .Includes(t => t.tb_BuyingRequisitionDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_BuyingRequisition>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BuyingRequisition>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_BuyingRequisitionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_BuyingRequisition entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_BuyingRequisition>().Where(w => w.PuRequisition_ID == (long)id)
                             .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_location )
                                        .Includes(t => t.tb_BuyingRequisitionDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_BuyingRequisition>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



