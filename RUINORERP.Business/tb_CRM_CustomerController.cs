
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:01
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
    /// 目标客户-公海客户CRM系统中使用，给成交客户作外键引用
    /// </summary>
    public partial class tb_CRM_CustomerController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CRM_CustomerServices _tb_CRM_CustomerServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_CRM_CustomerController(ILogger<tb_CRM_CustomerController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CRM_CustomerServices tb_CRM_CustomerServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CRM_CustomerServices = tb_CRM_CustomerServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_CRM_Customer info)
        {

           // tb_CRM_CustomerValidator validator = new tb_CRM_CustomerValidator();
           tb_CRM_CustomerValidator validator = _appContext.GetRequiredService<tb_CRM_CustomerValidator>();
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
        public async Task<ReturnResults<tb_CRM_Customer>> SaveOrUpdate(tb_CRM_Customer entity)
        {
            ReturnResults<tb_CRM_Customer> rr = new ReturnResults<tb_CRM_Customer>();
            tb_CRM_Customer Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Customer_id > 0)
                {
                    bool rs = await _tb_CRM_CustomerServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CRM_CustomerServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(entity);
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
            tb_CRM_Customer entity = model as tb_CRM_Customer;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Customer_id > 0)
                {
                    bool rs = await _tb_CRM_CustomerServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CRM_CustomerServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(entity);
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
            List<T> list = await _tb_CRM_CustomerServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_CRM_Customer entity = item as tb_CRM_Customer;
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
            List<T> list = await _tb_CRM_CustomerServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_CRM_Customer entity = item as tb_CRM_Customer;
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
            tb_CRM_Customer entity = model as tb_CRM_Customer;
            bool rs = await _tb_CRM_CustomerServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_Customer>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_CRM_Customer> entitys = models as List<tb_CRM_Customer>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_CRM_Customer>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Customer_id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_Customer>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_CRM_CustomerValidator validator = new tb_CRM_CustomerValidator();
           tb_CRM_CustomerValidator validator = _appContext.GetRequiredService<tb_CRM_CustomerValidator>();
            ValidationResult results = validator.Validate(info as tb_CRM_Customer);
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
            try
            {
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_CRM_Customer entity = model as tb_CRM_Customer;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Customer_id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_CRM_Customer>(entity as tb_CRM_Customer)
                        .Include(m => m.tb_CRM_Contacts)
                    .Include(m => m.tb_CRM_FollowUpPlanses)
                    .Include(m => m.tb_CRM_FollowUpRecordses)
                    .Include(m => m.tb_CRM_Collaborators)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_CRM_Customer>(entity as tb_CRM_Customer)
                .Include(m => m.tb_CRM_Contacts)
                .Include(m => m.tb_CRM_FollowUpPlanses)
                .Include(m => m.tb_CRM_FollowUpRecordses)
                .Include(m => m.tb_CRM_Collaborators)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Customer_id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Customer>()
                                .Includes(m => m.tb_CRM_Contacts)
                        .Includes(m => m.tb_CRM_FollowUpPlanses)
                        .Includes(m => m.tb_CRM_FollowUpRecordses)
                        .Includes(m => m.tb_CRM_Collaborators)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CRM_Customer entity = model as tb_CRM_Customer;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CRM_Customer>(m => m.Customer_id== entity.Customer_id)
                                .Include(m => m.tb_CRM_Contacts)
                        .Include(m => m.tb_CRM_FollowUpPlanses)
                        .Include(m => m.tb_CRM_FollowUpRecordses)
                        .Include(m => m.tb_CRM_Collaborators)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_CRM_Customer AddReEntity(tb_CRM_Customer entity)
        {
            tb_CRM_Customer AddEntity =  _tb_CRM_CustomerServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CRM_Customer> AddReEntityAsync(tb_CRM_Customer entity)
        {
            tb_CRM_Customer AddEntity = await _tb_CRM_CustomerServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CRM_Customer entity)
        {
            long id = await _tb_CRM_CustomerServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CRM_Customer> infos)
        {
            List<long> ids = await _tb_CRM_CustomerServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CRM_Customer entity)
        {
            bool rs = await _tb_CRM_CustomerServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_Customer>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CRM_Customer entity)
        {
            bool rs = await _tb_CRM_CustomerServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CRM_CustomerServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_Customer>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CRM_CustomerServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_Customer>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_CRM_Customer>> QueryAsync()
        {
            List<tb_CRM_Customer> list = await  _tb_CRM_CustomerServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(list);
            return list;
        }
        
        public virtual List<tb_CRM_Customer> Query()
        {
            List<tb_CRM_Customer> list =  _tb_CRM_CustomerServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(list);
            return list;
        }
        
        public virtual List<tb_CRM_Customer> Query(string wheresql)
        {
            List<tb_CRM_Customer> list =  _tb_CRM_CustomerServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CRM_Customer>> QueryAsync(string wheresql) 
        {
            List<tb_CRM_Customer> list = await _tb_CRM_CustomerServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_CRM_Customer>> QueryAsync(Expression<Func<tb_CRM_Customer, bool>> exp)
        {
            List<tb_CRM_Customer> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Customer>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CRM_Customer>> QueryByNavAsync()
        {
            List<tb_CRM_Customer> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Customer>()
                               .Includes(t => t.tb_crm_region )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_cities )
                               .Includes(t => t.tb_crm_leads )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_provinces )
                                            .Includes(t => t.tb_CRM_Contacts )
                                .Includes(t => t.tb_CRM_FollowUpPlanses )
                                .Includes(t => t.tb_CRM_FollowUpRecordses )
                                .Includes(t => t.tb_CRM_Collaborators )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CRM_Customer>> QueryByNavAsync(Expression<Func<tb_CRM_Customer, bool>> exp)
        {
            List<tb_CRM_Customer> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Customer>().Where(exp)
                               .Includes(t => t.tb_crm_region )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_cities )
                               .Includes(t => t.tb_crm_leads )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_provinces )
                                            .Includes(t => t.tb_CRM_Contacts )
                                .Includes(t => t.tb_CRM_FollowUpPlanses )
                                .Includes(t => t.tb_CRM_FollowUpRecordses )
                                .Includes(t => t.tb_CRM_Collaborators )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CRM_Customer> QueryByNav(Expression<Func<tb_CRM_Customer, bool>> exp)
        {
            List<tb_CRM_Customer> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Customer>().Where(exp)
                            .Includes(t => t.tb_crm_region )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_cities )
                            .Includes(t => t.tb_crm_leads )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_provinces )
                                        .Includes(t => t.tb_CRM_Contacts )
                            .Includes(t => t.tb_CRM_FollowUpPlanses )
                            .Includes(t => t.tb_CRM_FollowUpRecordses )
                            .Includes(t => t.tb_CRM_Collaborators )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CRM_Customer>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Customer>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CRM_CustomerServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_CRM_Customer entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Customer>().Where(w => w.Customer_id == (long)id)
                             .Includes(t => t.tb_crm_region )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_cities )
                            .Includes(t => t.tb_crm_leads )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_provinces )
                                        .Includes(t => t.tb_CRM_Contacts )
                            .Includes(t => t.tb_CRM_FollowUpPlanses )
                            .Includes(t => t.tb_CRM_FollowUpRecordses )
                            .Includes(t => t.tb_CRM_Collaborators )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Customer>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



