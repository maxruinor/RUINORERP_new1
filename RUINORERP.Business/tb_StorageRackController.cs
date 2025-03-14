
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
    /// 货架信息表
    /// </summary>
    public partial class tb_StorageRackController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_StorageRackServices _tb_StorageRackServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_StorageRackController(ILogger<tb_StorageRackController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_StorageRackServices tb_StorageRackServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_StorageRackServices = tb_StorageRackServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_StorageRack info)
        {

           // tb_StorageRackValidator validator = new tb_StorageRackValidator();
           tb_StorageRackValidator validator = _appContext.GetRequiredService<tb_StorageRackValidator>();
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
        public async Task<ReturnResults<tb_StorageRack>> SaveOrUpdate(tb_StorageRack entity)
        {
            ReturnResults<tb_StorageRack> rr = new ReturnResults<tb_StorageRack>();
            tb_StorageRack Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Rack_ID > 0)
                {
                    bool rs = await _tb_StorageRackServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_StorageRackServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(entity);
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
            tb_StorageRack entity = model as tb_StorageRack;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Rack_ID > 0)
                {
                    bool rs = await _tb_StorageRackServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_StorageRackServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(entity);
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
            List<T> list = await _tb_StorageRackServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_StorageRack entity = item as tb_StorageRack;
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
            List<T> list = await _tb_StorageRackServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_StorageRack entity = item as tb_StorageRack;
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
            tb_StorageRack entity = model as tb_StorageRack;
            bool rs = await _tb_StorageRackServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_StorageRack>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_StorageRack> entitys = models as List<tb_StorageRack>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_StorageRack>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Rack_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_StorageRack>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_StorageRackValidator validator = new tb_StorageRackValidator();
           tb_StorageRackValidator validator = _appContext.GetRequiredService<tb_StorageRackValidator>();
            ValidationResult results = validator.Validate(info as tb_StorageRack);
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

                tb_StorageRack entity = model as tb_StorageRack;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Rack_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_StorageRack>(entity as tb_StorageRack)
                        .Include(m => m.tb_StockOutDetails)
                    .Include(m => m.tb_PurReturnEntryDetails)
                    .Include(m => m.tb_FinishedGoodsInvDetails)
                    .Include(m => m.tb_Prods)
                    .Include(m => m.tb_SaleOutDetails)
                    .Include(m => m.tb_StocktakeDetails)
                    .Include(m => m.tb_PurEntryDetails)
                    .Include(m => m.tb_SaleOutReDetails)
                    .Include(m => m.tb_MRP_ReworkEntryDetails)
                    .Include(m => m.tb_PurEntryReDetails)
                    .Include(m => m.tb_Inventories)
                    .Include(m => m.tb_StockInDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_StorageRack>(entity as tb_StorageRack)
                .Include(m => m.tb_StockOutDetails)
                .Include(m => m.tb_PurReturnEntryDetails)
                .Include(m => m.tb_FinishedGoodsInvDetails)
                .Include(m => m.tb_Prods)
                .Include(m => m.tb_SaleOutDetails)
                .Include(m => m.tb_StocktakeDetails)
                .Include(m => m.tb_PurEntryDetails)
                .Include(m => m.tb_SaleOutReDetails)
                .Include(m => m.tb_MRP_ReworkEntryDetails)
                .Include(m => m.tb_PurEntryReDetails)
                .Include(m => m.tb_Inventories)
                .Include(m => m.tb_StockInDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Rack_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StorageRack>()
                                .Includes(m => m.tb_StockOutDetails)
                        .Includes(m => m.tb_PurReturnEntryDetails)
                        .Includes(m => m.tb_FinishedGoodsInvDetails)
                        .Includes(m => m.tb_Prods)
                        .Includes(m => m.tb_SaleOutDetails)
                        .Includes(m => m.tb_StocktakeDetails)
                        .Includes(m => m.tb_PurEntryDetails)
                        .Includes(m => m.tb_SaleOutReDetails)
                        .Includes(m => m.tb_MRP_ReworkEntryDetails)
                        .Includes(m => m.tb_PurEntryReDetails)
                        .Includes(m => m.tb_Inventories)
                        .Includes(m => m.tb_StockInDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_StorageRack entity = model as tb_StorageRack;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_StorageRack>(m => m.Rack_ID== entity.Rack_ID)
                                .Include(m => m.tb_StockOutDetails)
                        .Include(m => m.tb_PurReturnEntryDetails)
                        .Include(m => m.tb_FinishedGoodsInvDetails)
                        .Include(m => m.tb_Prods)
                        .Include(m => m.tb_SaleOutDetails)
                        .Include(m => m.tb_StocktakeDetails)
                        .Include(m => m.tb_PurEntryDetails)
                        .Include(m => m.tb_SaleOutReDetails)
                        .Include(m => m.tb_MRP_ReworkEntryDetails)
                        .Include(m => m.tb_PurEntryReDetails)
                        .Include(m => m.tb_Inventories)
                        .Include(m => m.tb_StockInDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_StorageRack AddReEntity(tb_StorageRack entity)
        {
            tb_StorageRack AddEntity =  _tb_StorageRackServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_StorageRack> AddReEntityAsync(tb_StorageRack entity)
        {
            tb_StorageRack AddEntity = await _tb_StorageRackServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_StorageRack entity)
        {
            long id = await _tb_StorageRackServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_StorageRack> infos)
        {
            List<long> ids = await _tb_StorageRackServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_StorageRack entity)
        {
            bool rs = await _tb_StorageRackServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StorageRack>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_StorageRack entity)
        {
            bool rs = await _tb_StorageRackServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_StorageRackServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StorageRack>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_StorageRackServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StorageRack>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_StorageRack>> QueryAsync()
        {
            List<tb_StorageRack> list = await  _tb_StorageRackServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(list);
            return list;
        }
        
        public virtual List<tb_StorageRack> Query()
        {
            List<tb_StorageRack> list =  _tb_StorageRackServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(list);
            return list;
        }
        
        public virtual List<tb_StorageRack> Query(string wheresql)
        {
            List<tb_StorageRack> list =  _tb_StorageRackServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(list);
            return list;
        }
        
        public virtual async Task<List<tb_StorageRack>> QueryAsync(string wheresql) 
        {
            List<tb_StorageRack> list = await _tb_StorageRackServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_StorageRack>> QueryAsync(Expression<Func<tb_StorageRack, bool>> exp)
        {
            List<tb_StorageRack> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StorageRack>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StorageRack>> QueryByNavAsync()
        {
            List<tb_StorageRack> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StorageRack>()
                               .Includes(t => t.tb_location )
                                            .Includes(t => t.tb_StockOutDetails )
                                .Includes(t => t.tb_PurReturnEntryDetails )
                                .Includes(t => t.tb_FinishedGoodsInvDetails )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_SaleOutDetails )
                                .Includes(t => t.tb_StocktakeDetails )
                                .Includes(t => t.tb_PurEntryDetails )
                                .Includes(t => t.tb_SaleOutReDetails )
                                .Includes(t => t.tb_MRP_ReworkEntryDetails )
                                .Includes(t => t.tb_PurEntryReDetails )
                                .Includes(t => t.tb_Inventories )
                                .Includes(t => t.tb_StockInDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StorageRack>> QueryByNavAsync(Expression<Func<tb_StorageRack, bool>> exp)
        {
            List<tb_StorageRack> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StorageRack>().Where(exp)
                               .Includes(t => t.tb_location )
                                            .Includes(t => t.tb_StockOutDetails )
                                .Includes(t => t.tb_PurReturnEntryDetails )
                                .Includes(t => t.tb_FinishedGoodsInvDetails )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_SaleOutDetails )
                                .Includes(t => t.tb_StocktakeDetails )
                                .Includes(t => t.tb_PurEntryDetails )
                                .Includes(t => t.tb_SaleOutReDetails )
                                .Includes(t => t.tb_MRP_ReworkEntryDetails )
                                .Includes(t => t.tb_PurEntryReDetails )
                                .Includes(t => t.tb_Inventories )
                                .Includes(t => t.tb_StockInDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_StorageRack> QueryByNav(Expression<Func<tb_StorageRack, bool>> exp)
        {
            List<tb_StorageRack> list = _unitOfWorkManage.GetDbClient().Queryable<tb_StorageRack>().Where(exp)
                            .Includes(t => t.tb_location )
                                        .Includes(t => t.tb_StockOutDetails )
                            .Includes(t => t.tb_PurReturnEntryDetails )
                            .Includes(t => t.tb_FinishedGoodsInvDetails )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_SaleOutDetails )
                            .Includes(t => t.tb_StocktakeDetails )
                            .Includes(t => t.tb_PurEntryDetails )
                            .Includes(t => t.tb_SaleOutReDetails )
                            .Includes(t => t.tb_MRP_ReworkEntryDetails )
                            .Includes(t => t.tb_PurEntryReDetails )
                            .Includes(t => t.tb_Inventories )
                            .Includes(t => t.tb_StockInDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_StorageRack>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StorageRack>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_StorageRackServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_StorageRack entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_StorageRack>().Where(w => w.Rack_ID == (long)id)
                             .Includes(t => t.tb_location )
                                        .Includes(t => t.tb_StockOutDetails )
                            .Includes(t => t.tb_PurReturnEntryDetails )
                            .Includes(t => t.tb_FinishedGoodsInvDetails )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_SaleOutDetails )
                            .Includes(t => t.tb_StocktakeDetails )
                            .Includes(t => t.tb_PurEntryDetails )
                            .Includes(t => t.tb_SaleOutReDetails )
                            .Includes(t => t.tb_MRP_ReworkEntryDetails )
                            .Includes(t => t.tb_PurEntryReDetails )
                            .Includes(t => t.tb_Inventories )
                            .Includes(t => t.tb_StockInDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_StorageRack>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



