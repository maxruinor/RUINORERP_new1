
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 19:02:37
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
    /// 入库单 非生产领料/退料
    /// </summary>
    public partial class tb_StockInController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_StockInServices _tb_StockInServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_StockInController(ILogger<tb_StockInController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_StockInServices tb_StockInServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_StockInServices = tb_StockInServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_StockIn info)
        {
            tb_StockInValidator validator = new tb_StockInValidator();
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
        public async Task<ReturnResults<tb_StockIn>> SaveOrUpdate(tb_StockIn entity)
        {
            ReturnResults<tb_StockIn> rr = new ReturnResults<tb_StockIn>();
            tb_StockIn Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MainID > 0)
                {
                    bool rs = await _tb_StockInServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_StockInServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(entity);
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
            tb_StockIn entity = model as tb_StockIn;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MainID > 0)
                {
                    bool rs = await _tb_StockInServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_StockInServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(entity);
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
            List<T> list = await _tb_StockInServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_StockIn entity = item as tb_StockIn;
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
            List<T> list = await _tb_StockInServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_StockIn entity = item as tb_StockIn;
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
            tb_StockIn entity = model as tb_StockIn;
            bool rs = await _tb_StockInServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_StockIn>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_StockIn> entitys = models as List<tb_StockIn>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_StockIn>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.MainID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_StockIn>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_StockInValidator validator = new tb_StockInValidator();
            ValidationResult results = validator.Validate(info as tb_StockIn);
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
                tb_StockIn entity = model as tb_StockIn;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.MainID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_StockIn>(entity as tb_StockIn)
                        .Include(m => m.tb_StockInDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_StockIn>(entity as tb_StockIn)
                .Include(m => m.tb_StockInDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.MainID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StockIn>()
                                .Includes(m => m.tb_StockInDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_StockIn entity = model as tb_StockIn;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_StockIn>(m => m.MainID== entity.MainID)
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
        
        
        
        public tb_StockIn AddReEntity(tb_StockIn entity)
        {
            tb_StockIn AddEntity =  _tb_StockInServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_StockIn> AddReEntityAsync(tb_StockIn entity)
        {
            tb_StockIn AddEntity = await _tb_StockInServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_StockIn entity)
        {
            long id = await _tb_StockInServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_StockIn> infos)
        {
            List<long> ids = await _tb_StockInServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_StockIn entity)
        {
            bool rs = await _tb_StockInServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockIn>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_StockIn entity)
        {
            bool rs = await _tb_StockInServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_StockInServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockIn>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_StockInServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockIn>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_StockIn>> QueryAsync()
        {
            List<tb_StockIn> list = await  _tb_StockInServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(list);
            return list;
        }
        
        public virtual List<tb_StockIn> Query()
        {
            List<tb_StockIn> list =  _tb_StockInServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(list);
            return list;
        }
        
        public virtual List<tb_StockIn> Query(string wheresql)
        {
            List<tb_StockIn> list =  _tb_StockInServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(list);
            return list;
        }
        
        public virtual async Task<List<tb_StockIn>> QueryAsync(string wheresql) 
        {
            List<tb_StockIn> list = await _tb_StockInServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_StockIn>> QueryAsync(Expression<Func<tb_StockIn, bool>> exp)
        {
            List<tb_StockIn> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockIn>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StockIn>> QueryByNavAsync()
        {
            List<tb_StockIn> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockIn>()
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_StockInDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StockIn>> QueryByNavAsync(Expression<Func<tb_StockIn, bool>> exp)
        {
            List<tb_StockIn> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockIn>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_StockInDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_StockIn> QueryByNav(Expression<Func<tb_StockIn, bool>> exp)
        {
            List<tb_StockIn> list = _unitOfWorkManage.GetDbClient().Queryable<tb_StockIn>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_StockInDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_StockIn>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StockIn>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_StockInServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_StockIn entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockIn>().Where(w => w.MainID == (long)id)
                             .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_StockInDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_StockIn>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



