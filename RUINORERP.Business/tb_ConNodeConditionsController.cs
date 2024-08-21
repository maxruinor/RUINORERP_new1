
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2024 10:31:32
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
    /// 流程步骤 为转移条件集合，Field为条件左参数，Operator为操作操作符如果值类型为String则表达式只能为==或者!=，Value为表达式值
    /// </summary>
    public partial class tb_ConNodeConditionsController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ConNodeConditionsServices _tb_ConNodeConditionsServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ConNodeConditionsController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ConNodeConditionsServices tb_ConNodeConditionsServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ConNodeConditionsServices = tb_ConNodeConditionsServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_ConNodeConditions info)
        {
            tb_ConNodeConditionsValidator validator = new tb_ConNodeConditionsValidator();
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
        public async Task<ReturnResults<tb_ConNodeConditions>> SaveOrUpdate(tb_ConNodeConditions entity)
        {
            ReturnResults<tb_ConNodeConditions> rr = new ReturnResults<tb_ConNodeConditions>();
            tb_ConNodeConditions Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ConNodeConditions_Id > 0)
                {
                    bool rs = await _tb_ConNodeConditionsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ConNodeConditionsServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(entity);
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
            tb_ConNodeConditions entity = model as tb_ConNodeConditions;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ConNodeConditions_Id > 0)
                {
                    bool rs = await _tb_ConNodeConditionsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ConNodeConditionsServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(entity);
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
            List<T> list = await _tb_ConNodeConditionsServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ConNodeConditions entity = item as tb_ConNodeConditions;
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
            List<T> list = await _tb_ConNodeConditionsServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ConNodeConditions entity = item as tb_ConNodeConditions;
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
            tb_ConNodeConditions entity = model as tb_ConNodeConditions;
            bool rs = await _tb_ConNodeConditionsServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ConNodeConditions>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ConNodeConditions> entitys = models as List<tb_ConNodeConditions>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ConNodeConditions>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ConNodeConditions_Id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ConNodeConditions>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_ConNodeConditionsValidator validator = new tb_ConNodeConditionsValidator();
            ValidationResult results = validator.Validate(info as tb_ConNodeConditions);
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
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_ConNodeConditions entity = model as tb_ConNodeConditions;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.ConNodeConditions_Id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ConNodeConditions>(entity as tb_ConNodeConditions)
                        .Include(m => m.tb_NextNodeses)
                    .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ConNodeConditions>(entity as tb_ConNodeConditions)
                .Include(m => m.tb_NextNodeses)
                        .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ConNodeConditions_Id;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                //出错后，取消生成的ID等值
                command.Undo();
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                //_logger.Error("BaseSaveOrUpdateWithChild事务回滚");
                // rr.ErrorMsg = "事务回滚=>" + ex.Message;
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ConNodeConditions>()
                                .Includes(m => m.tb_NextNodeses)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ConNodeConditions entity = model as tb_ConNodeConditions;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ConNodeConditions>(m => m.ConNodeConditions_Id== entity.ConNodeConditions_Id)
                                .Include(m => m.tb_NextNodeses)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ConNodeConditions AddReEntity(tb_ConNodeConditions entity)
        {
            tb_ConNodeConditions AddEntity =  _tb_ConNodeConditionsServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ConNodeConditions> AddReEntityAsync(tb_ConNodeConditions entity)
        {
            tb_ConNodeConditions AddEntity = await _tb_ConNodeConditionsServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ConNodeConditions entity)
        {
            long id = await _tb_ConNodeConditionsServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ConNodeConditions> infos)
        {
            List<long> ids = await _tb_ConNodeConditionsServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ConNodeConditions entity)
        {
            bool rs = await _tb_ConNodeConditionsServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ConNodeConditions>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ConNodeConditions entity)
        {
            bool rs = await _tb_ConNodeConditionsServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ConNodeConditionsServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ConNodeConditions>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ConNodeConditionsServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ConNodeConditions>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ConNodeConditions>> QueryAsync()
        {
            List<tb_ConNodeConditions> list = await  _tb_ConNodeConditionsServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(list);
            return list;
        }
        
        public virtual List<tb_ConNodeConditions> Query()
        {
            List<tb_ConNodeConditions> list =  _tb_ConNodeConditionsServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(list);
            return list;
        }
        
        public virtual List<tb_ConNodeConditions> Query(string wheresql)
        {
            List<tb_ConNodeConditions> list =  _tb_ConNodeConditionsServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ConNodeConditions>> QueryAsync(string wheresql) 
        {
            List<tb_ConNodeConditions> list = await _tb_ConNodeConditionsServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ConNodeConditions>> QueryAsync(Expression<Func<tb_ConNodeConditions, bool>> exp)
        {
            List<tb_ConNodeConditions> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ConNodeConditions>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ConNodeConditions>> QueryByNavAsync()
        {
            List<tb_ConNodeConditions> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ConNodeConditions>()
                                            .Includes(t => t.tb_NextNodeses )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ConNodeConditions>> QueryByNavAsync(Expression<Func<tb_ConNodeConditions, bool>> exp)
        {
            List<tb_ConNodeConditions> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ConNodeConditions>().Where(exp)
                                            .Includes(t => t.tb_NextNodeses )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ConNodeConditions> QueryByNav(Expression<Func<tb_ConNodeConditions, bool>> exp)
        {
            List<tb_ConNodeConditions> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ConNodeConditions>().Where(exp)
                                        .Includes(t => t.tb_NextNodeses )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ConNodeConditions>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ConNodeConditions>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ConNodeConditionsServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ConNodeConditions entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ConNodeConditions>().Where(w => w.ConNodeConditions_Id == (long)id)
                                         .Includes(t => t.tb_NextNodeses )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ConNodeConditions>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



