
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:15:47
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
    /// 线索机会-询盘
    /// </summary>
    public partial class tb_CRM_LeadsController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CRM_LeadsServices _tb_CRM_LeadsServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_CRM_LeadsController(ILogger<tb_CRM_LeadsController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CRM_LeadsServices tb_CRM_LeadsServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CRM_LeadsServices = tb_CRM_LeadsServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_CRM_Leads info)
        {
            tb_CRM_LeadsValidator validator = new tb_CRM_LeadsValidator();
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
        public async Task<ReturnResults<tb_CRM_Leads>> SaveOrUpdate(tb_CRM_Leads entity)
        {
            ReturnResults<tb_CRM_Leads> rr = new ReturnResults<tb_CRM_Leads>();
            tb_CRM_Leads Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.LeadID > 0)
                {
                    bool rs = await _tb_CRM_LeadsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CRM_LeadsServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(entity);
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
            tb_CRM_Leads entity = model as tb_CRM_Leads;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.LeadID > 0)
                {
                    bool rs = await _tb_CRM_LeadsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CRM_LeadsServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(entity);
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
            List<T> list = await _tb_CRM_LeadsServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_CRM_Leads entity = item as tb_CRM_Leads;
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
            List<T> list = await _tb_CRM_LeadsServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_CRM_Leads entity = item as tb_CRM_Leads;
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
            tb_CRM_Leads entity = model as tb_CRM_Leads;
            bool rs = await _tb_CRM_LeadsServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_Leads>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_CRM_Leads> entitys = models as List<tb_CRM_Leads>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_CRM_Leads>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.LeadID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_Leads>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_CRM_LeadsValidator validator = new tb_CRM_LeadsValidator();
            ValidationResult results = validator.Validate(info as tb_CRM_Leads);
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
                tb_CRM_Leads entity = model as tb_CRM_Leads;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.LeadID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_CRM_Leads>(entity as tb_CRM_Leads)
                        .Include(m => m.tb_CRM_FollowUpRecordses)
                    .Include(m => m.tb_CRM_Customers)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_CRM_Leads>(entity as tb_CRM_Leads)
                .Include(m => m.tb_CRM_FollowUpRecordses)
                .Include(m => m.tb_CRM_Customers)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.LeadID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Leads>()
                                .Includes(m => m.tb_CRM_FollowUpRecordses)
                        .Includes(m => m.tb_CRM_Customers)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CRM_Leads entity = model as tb_CRM_Leads;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CRM_Leads>(m => m.LeadID== entity.LeadID)
                                .Include(m => m.tb_CRM_FollowUpRecordses)
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
        
        
        
        public tb_CRM_Leads AddReEntity(tb_CRM_Leads entity)
        {
            tb_CRM_Leads AddEntity =  _tb_CRM_LeadsServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CRM_Leads> AddReEntityAsync(tb_CRM_Leads entity)
        {
            tb_CRM_Leads AddEntity = await _tb_CRM_LeadsServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CRM_Leads entity)
        {
            long id = await _tb_CRM_LeadsServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CRM_Leads> infos)
        {
            List<long> ids = await _tb_CRM_LeadsServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CRM_Leads entity)
        {
            bool rs = await _tb_CRM_LeadsServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_Leads>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CRM_Leads entity)
        {
            bool rs = await _tb_CRM_LeadsServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CRM_LeadsServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_Leads>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CRM_LeadsServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CRM_Leads>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_CRM_Leads>> QueryAsync()
        {
            List<tb_CRM_Leads> list = await  _tb_CRM_LeadsServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(list);
            return list;
        }
        
        public virtual List<tb_CRM_Leads> Query()
        {
            List<tb_CRM_Leads> list =  _tb_CRM_LeadsServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(list);
            return list;
        }
        
        public virtual List<tb_CRM_Leads> Query(string wheresql)
        {
            List<tb_CRM_Leads> list =  _tb_CRM_LeadsServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CRM_Leads>> QueryAsync(string wheresql) 
        {
            List<tb_CRM_Leads> list = await _tb_CRM_LeadsServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_CRM_Leads>> QueryAsync(Expression<Func<tb_CRM_Leads, bool>> exp)
        {
            List<tb_CRM_Leads> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Leads>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CRM_Leads>> QueryByNavAsync()
        {
            List<tb_CRM_Leads> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Leads>()
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_CRM_FollowUpRecordses )
                                .Includes(t => t.tb_CRM_Customers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CRM_Leads>> QueryByNavAsync(Expression<Func<tb_CRM_Leads, bool>> exp)
        {
            List<tb_CRM_Leads> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Leads>().Where(exp)
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_CRM_FollowUpRecordses )
                                .Includes(t => t.tb_CRM_Customers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CRM_Leads> QueryByNav(Expression<Func<tb_CRM_Leads, bool>> exp)
        {
            List<tb_CRM_Leads> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Leads>().Where(exp)
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_CRM_FollowUpRecordses )
                            .Includes(t => t.tb_CRM_Customers )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CRM_Leads>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Leads>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CRM_LeadsServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_CRM_Leads entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_Leads>().Where(w => w.LeadID == (long)id)
                             .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_CRM_FollowUpRecordses )
                            .Includes(t => t.tb_CRM_Customers )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_CRM_Leads>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



