
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:07
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
    /// 流程步骤 转移条件集合
    /// </summary>
    public partial class tb_NextNodesController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_NextNodesServices _tb_NextNodesServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_NextNodesController(ILogger<tb_NextNodesController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_NextNodesServices tb_NextNodesServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_NextNodesServices = tb_NextNodesServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_NextNodes info)
        {

           // tb_NextNodesValidator validator = new tb_NextNodesValidator();
           tb_NextNodesValidator validator = _appContext.GetRequiredService<tb_NextNodesValidator>();
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
        public async Task<ReturnResults<tb_NextNodes>> SaveOrUpdate(tb_NextNodes entity)
        {
            ReturnResults<tb_NextNodes> rr = new ReturnResults<tb_NextNodes>();
            tb_NextNodes Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.NextNode_ID > 0)
                {
                    bool rs = await _tb_NextNodesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_NextNodesServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(entity);
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
            tb_NextNodes entity = model as tb_NextNodes;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.NextNode_ID > 0)
                {
                    bool rs = await _tb_NextNodesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_NextNodesServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(entity);
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
            List<T> list = await _tb_NextNodesServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_NextNodes entity = item as tb_NextNodes;
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
            List<T> list = await _tb_NextNodesServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_NextNodes entity = item as tb_NextNodes;
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
            tb_NextNodes entity = model as tb_NextNodes;
            bool rs = await _tb_NextNodesServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_NextNodes>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_NextNodes> entitys = models as List<tb_NextNodes>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_NextNodes>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.NextNode_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_NextNodes>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_NextNodesValidator validator = new tb_NextNodesValidator();
           tb_NextNodesValidator validator = _appContext.GetRequiredService<tb_NextNodesValidator>();
            ValidationResult results = validator.Validate(info as tb_NextNodes);
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
                tb_NextNodes entity = model as tb_NextNodes;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.NextNode_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_NextNodes>(entity as tb_NextNodes)
                        .Include(m => m.tb_ProcessSteps)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_NextNodes>(entity as tb_NextNodes)
                .Include(m => m.tb_ProcessSteps)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.NextNode_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_NextNodes>()
                                .Includes(m => m.tb_ProcessSteps)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_NextNodes entity = model as tb_NextNodes;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_NextNodes>(m => m.NextNode_ID== entity.NextNode_ID)
                                .Include(m => m.tb_ProcessSteps)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_NextNodes AddReEntity(tb_NextNodes entity)
        {
            tb_NextNodes AddEntity =  _tb_NextNodesServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_NextNodes> AddReEntityAsync(tb_NextNodes entity)
        {
            tb_NextNodes AddEntity = await _tb_NextNodesServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_NextNodes entity)
        {
            long id = await _tb_NextNodesServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_NextNodes> infos)
        {
            List<long> ids = await _tb_NextNodesServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_NextNodes entity)
        {
            bool rs = await _tb_NextNodesServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_NextNodes>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_NextNodes entity)
        {
            bool rs = await _tb_NextNodesServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_NextNodesServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_NextNodes>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_NextNodesServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_NextNodes>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_NextNodes>> QueryAsync()
        {
            List<tb_NextNodes> list = await  _tb_NextNodesServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(list);
            return list;
        }
        
        public virtual List<tb_NextNodes> Query()
        {
            List<tb_NextNodes> list =  _tb_NextNodesServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(list);
            return list;
        }
        
        public virtual List<tb_NextNodes> Query(string wheresql)
        {
            List<tb_NextNodes> list =  _tb_NextNodesServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(list);
            return list;
        }
        
        public virtual async Task<List<tb_NextNodes>> QueryAsync(string wheresql) 
        {
            List<tb_NextNodes> list = await _tb_NextNodesServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_NextNodes>> QueryAsync(Expression<Func<tb_NextNodes, bool>> exp)
        {
            List<tb_NextNodes> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_NextNodes>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_NextNodes>> QueryByNavAsync()
        {
            List<tb_NextNodes> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_NextNodes>()
                               .Includes(t => t.tb_connodeconditions )
                                            .Includes(t => t.tb_ProcessSteps )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_NextNodes>> QueryByNavAsync(Expression<Func<tb_NextNodes, bool>> exp)
        {
            List<tb_NextNodes> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_NextNodes>().Where(exp)
                               .Includes(t => t.tb_connodeconditions )
                                            .Includes(t => t.tb_ProcessSteps )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_NextNodes> QueryByNav(Expression<Func<tb_NextNodes, bool>> exp)
        {
            List<tb_NextNodes> list = _unitOfWorkManage.GetDbClient().Queryable<tb_NextNodes>().Where(exp)
                            .Includes(t => t.tb_connodeconditions )
                                        .Includes(t => t.tb_ProcessSteps )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_NextNodes>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_NextNodes>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_NextNodesServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_NextNodes entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_NextNodes>().Where(w => w.NextNode_ID == (long)id)
                             .Includes(t => t.tb_connodeconditions )
                                        .Includes(t => t.tb_ProcessSteps )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_NextNodes>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



