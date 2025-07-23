
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:53
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
    /// 调拨单-两个仓库之间的库存转移
    /// </summary>
    public partial class tb_StockTransferController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_StockTransferServices _tb_StockTransferServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_StockTransferController(ILogger<tb_StockTransferController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_StockTransferServices tb_StockTransferServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_StockTransferServices = tb_StockTransferServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_StockTransfer info)
        {

           // tb_StockTransferValidator validator = new tb_StockTransferValidator();
           tb_StockTransferValidator validator = _appContext.GetRequiredService<tb_StockTransferValidator>();
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
        public async Task<ReturnResults<tb_StockTransfer>> SaveOrUpdate(tb_StockTransfer entity)
        {
            ReturnResults<tb_StockTransfer> rr = new ReturnResults<tb_StockTransfer>();
            tb_StockTransfer Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.StockTransferID > 0)
                {
                    bool rs = await _tb_StockTransferServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_StockTransferServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(entity);
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
            tb_StockTransfer entity = model as tb_StockTransfer;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.StockTransferID > 0)
                {
                    bool rs = await _tb_StockTransferServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_StockTransferServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(entity);
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
            List<T> list = await _tb_StockTransferServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_StockTransfer entity = item as tb_StockTransfer;
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
            List<T> list = await _tb_StockTransferServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_StockTransfer entity = item as tb_StockTransfer;
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
            tb_StockTransfer entity = model as tb_StockTransfer;
            bool rs = await _tb_StockTransferServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_StockTransfer>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_StockTransfer> entitys = models as List<tb_StockTransfer>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_StockTransfer>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.StockTransferID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_StockTransfer>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_StockTransferValidator validator = new tb_StockTransferValidator();
           tb_StockTransferValidator validator = _appContext.GetRequiredService<tb_StockTransferValidator>();
            ValidationResult results = validator.Validate(info as tb_StockTransfer);
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

                tb_StockTransfer entity = model as tb_StockTransfer;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.StockTransferID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_StockTransfer>(entity as tb_StockTransfer)
                        .Include(m => m.tb_StockTransferDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_StockTransfer>(entity as tb_StockTransfer)
                .Include(m => m.tb_StockTransferDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.StockTransferID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransfer>()
                                .Includes(m => m.tb_StockTransferDetails)
                                        .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_StockTransfer entity = model as tb_StockTransfer;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_StockTransfer>(m => m.StockTransferID== entity.StockTransferID)
                                .Include(m => m.tb_StockTransferDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_StockTransfer AddReEntity(tb_StockTransfer entity)
        {
            tb_StockTransfer AddEntity =  _tb_StockTransferServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_StockTransfer> AddReEntityAsync(tb_StockTransfer entity)
        {
            tb_StockTransfer AddEntity = await _tb_StockTransferServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_StockTransfer entity)
        {
            long id = await _tb_StockTransferServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_StockTransfer> infos)
        {
            List<long> ids = await _tb_StockTransferServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_StockTransfer entity)
        {
            bool rs = await _tb_StockTransferServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockTransfer>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_StockTransfer entity)
        {
            bool rs = await _tb_StockTransferServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_StockTransferServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockTransfer>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_StockTransferServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockTransfer>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_StockTransfer>> QueryAsync()
        {
            List<tb_StockTransfer> list = await  _tb_StockTransferServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(list);
            return list;
        }
        
        public virtual List<tb_StockTransfer> Query()
        {
            List<tb_StockTransfer> list =  _tb_StockTransferServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(list);
            return list;
        }
        
        public virtual List<tb_StockTransfer> Query(string wheresql)
        {
            List<tb_StockTransfer> list =  _tb_StockTransferServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(list);
            return list;
        }
        
        public virtual async Task<List<tb_StockTransfer>> QueryAsync(string wheresql) 
        {
            List<tb_StockTransfer> list = await _tb_StockTransferServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_StockTransfer>> QueryAsync(Expression<Func<tb_StockTransfer, bool>> exp)
        {
            List<tb_StockTransfer> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransfer>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StockTransfer>> QueryByNavAsync()
        {
            List<tb_StockTransfer> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransfer>()
                               .Includes(t => t.tb_employee )
                              .Includes(t => t.tb_location_from)
                               .Includes(t => t.tb_location_to)
                                            .Includes(t => t.tb_StockTransferDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StockTransfer>> QueryByNavAsync(Expression<Func<tb_StockTransfer, bool>> exp)
        {
            List<tb_StockTransfer> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransfer>().Where(exp)
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_location_from )
                               .Includes(t => t.tb_location_to )
                                            .Includes(t => t.tb_StockTransferDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_StockTransfer> QueryByNav(Expression<Func<tb_StockTransfer, bool>> exp)
        {
            List<tb_StockTransfer> list = _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransfer>().Where(exp)
                            .Includes(t => t.tb_employee )
                              .Includes(t => t.tb_location_from)
                               .Includes(t => t.tb_location_to)
                                        .Includes(t => t.tb_StockTransferDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_StockTransfer>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransfer>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_StockTransferServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_StockTransfer entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransfer>().Where(w => w.StockTransferID == (long)id)
                             .Includes(t => t.tb_employee )
                              .Includes(t => t.tb_location_from)
                               .Includes(t => t.tb_location_to)
                                        .Includes(t => t.tb_StockTransferDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_StockTransfer>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



