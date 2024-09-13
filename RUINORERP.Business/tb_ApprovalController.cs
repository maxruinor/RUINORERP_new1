
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:22
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
    /// 审核配置表 对于所有单据审核，并且提供明细，每个明细通过则主表通过主表中对应一个业务单据的主ID https://www.likecs.com/show-747870.html 
    /// </summary>
    public partial class tb_ApprovalController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ApprovalServices _tb_ApprovalServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ApprovalController(ILogger<tb_ApprovalController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ApprovalServices tb_ApprovalServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ApprovalServices = tb_ApprovalServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_Approval info)
        {
            tb_ApprovalValidator validator = new tb_ApprovalValidator();
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
        public async Task<ReturnResults<tb_Approval>> SaveOrUpdate(tb_Approval entity)
        {
            ReturnResults<tb_Approval> rr = new ReturnResults<tb_Approval>();
            tb_Approval Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ApprovalID > 0)
                {
                    bool rs = await _tb_ApprovalServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Approval>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ApprovalServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Approval>(entity);
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
            tb_Approval entity = model as tb_Approval;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ApprovalID > 0)
                {
                    bool rs = await _tb_ApprovalServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Approval>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ApprovalServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Approval>(entity);
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
            List<T> list = await _tb_ApprovalServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Approval entity = item as tb_Approval;
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
            List<T> list = await _tb_ApprovalServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Approval entity = item as tb_Approval;
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
            tb_Approval entity = model as tb_Approval;
            bool rs = await _tb_ApprovalServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Approval>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Approval> entitys = models as List<tb_Approval>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Approval>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ApprovalID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Approval>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_ApprovalValidator validator = new tb_ApprovalValidator();
            ValidationResult results = validator.Validate(info as tb_Approval);
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
                tb_Approval entity = model as tb_Approval;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ApprovalID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Approval>(entity as tb_Approval)
                        .Include(m => m.tb_ApprovalProcessDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Approval>(entity as tb_Approval)
                .Include(m => m.tb_ApprovalProcessDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ApprovalID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Approval>()
                                .Includes(m => m.tb_ApprovalProcessDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Approval entity = model as tb_Approval;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Approval>(m => m.ApprovalID== entity.ApprovalID)
                                .Include(m => m.tb_ApprovalProcessDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Approval AddReEntity(tb_Approval entity)
        {
            tb_Approval AddEntity =  _tb_ApprovalServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Approval> AddReEntityAsync(tb_Approval entity)
        {
            tb_Approval AddEntity = await _tb_ApprovalServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Approval entity)
        {
            long id = await _tb_ApprovalServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Approval>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Approval> infos)
        {
            List<long> ids = await _tb_ApprovalServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Approval>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Approval entity)
        {
            bool rs = await _tb_ApprovalServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Approval>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Approval entity)
        {
            bool rs = await _tb_ApprovalServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Approval>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ApprovalServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Approval>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ApprovalServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Approval>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Approval>> QueryAsync()
        {
            List<tb_Approval> list = await  _tb_ApprovalServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(list);
            return list;
        }
        
        public virtual List<tb_Approval> Query()
        {
            List<tb_Approval> list =  _tb_ApprovalServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(list);
            return list;
        }
        
        public virtual List<tb_Approval> Query(string wheresql)
        {
            List<tb_Approval> list =  _tb_ApprovalServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Approval>> QueryAsync(string wheresql) 
        {
            List<tb_Approval> list = await _tb_ApprovalServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Approval>> QueryAsync(Expression<Func<tb_Approval, bool>> exp)
        {
            List<tb_Approval> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Approval>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Approval>> QueryByNavAsync()
        {
            List<tb_Approval> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Approval>()
                                            .Includes(t => t.tb_ApprovalProcessDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Approval>> QueryByNavAsync(Expression<Func<tb_Approval, bool>> exp)
        {
            List<tb_Approval> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Approval>().Where(exp)
                                            .Includes(t => t.tb_ApprovalProcessDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Approval> QueryByNav(Expression<Func<tb_Approval, bool>> exp)
        {
            List<tb_Approval> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Approval>().Where(exp)
                                        .Includes(t => t.tb_ApprovalProcessDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Approval>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Approval>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ApprovalServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Approval entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Approval>().Where(w => w.ApprovalID == (long)id)
                                         .Includes(t => t.tb_ApprovalProcessDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Approval>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



