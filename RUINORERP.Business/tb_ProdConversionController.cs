
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:10
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
    /// 产品转换单 A变成B出库,AB相近。可能只是换说明书或刷机  A  数量  加或减 。B数量增加或减少。
    /// </summary>
    public partial class tb_ProdConversionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProdConversionServices _tb_ProdConversionServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProdConversionController(ILogger<tb_ProdConversionController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProdConversionServices tb_ProdConversionServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProdConversionServices = tb_ProdConversionServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProdConversion info)
        {

           // tb_ProdConversionValidator validator = new tb_ProdConversionValidator();
           tb_ProdConversionValidator validator = _appContext.GetRequiredService<tb_ProdConversionValidator>();
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
        public async Task<ReturnResults<tb_ProdConversion>> SaveOrUpdate(tb_ProdConversion entity)
        {
            ReturnResults<tb_ProdConversion> rr = new ReturnResults<tb_ProdConversion>();
            tb_ProdConversion Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ConversionID > 0)
                {
                    bool rs = await _tb_ProdConversionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProdConversionServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(entity);
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
            tb_ProdConversion entity = model as tb_ProdConversion;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ConversionID > 0)
                {
                    bool rs = await _tb_ProdConversionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProdConversionServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(entity);
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
            List<T> list = await _tb_ProdConversionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProdConversion entity = item as tb_ProdConversion;
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
            List<T> list = await _tb_ProdConversionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProdConversion entity = item as tb_ProdConversion;
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
            tb_ProdConversion entity = model as tb_ProdConversion;
            bool rs = await _tb_ProdConversionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProdConversion>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProdConversion> entitys = models as List<tb_ProdConversion>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProdConversion>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ConversionID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProdConversion>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProdConversionValidator validator = new tb_ProdConversionValidator();
           tb_ProdConversionValidator validator = _appContext.GetRequiredService<tb_ProdConversionValidator>();
            ValidationResult results = validator.Validate(info as tb_ProdConversion);
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
                tb_ProdConversion entity = model as tb_ProdConversion;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ConversionID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProdConversion>(entity as tb_ProdConversion)
                        .Include(m => m.tb_ProdConversionDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProdConversion>(entity as tb_ProdConversion)
                .Include(m => m.tb_ProdConversionDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ConversionID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdConversion>()
                                .Includes(m => m.tb_ProdConversionDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProdConversion entity = model as tb_ProdConversion;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProdConversion>(m => m.ConversionID== entity.ConversionID)
                                .Include(m => m.tb_ProdConversionDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProdConversion AddReEntity(tb_ProdConversion entity)
        {
            tb_ProdConversion AddEntity =  _tb_ProdConversionServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProdConversion> AddReEntityAsync(tb_ProdConversion entity)
        {
            tb_ProdConversion AddEntity = await _tb_ProdConversionServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProdConversion entity)
        {
            long id = await _tb_ProdConversionServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProdConversion> infos)
        {
            List<long> ids = await _tb_ProdConversionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProdConversion entity)
        {
            bool rs = await _tb_ProdConversionServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdConversion>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProdConversion entity)
        {
            bool rs = await _tb_ProdConversionServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProdConversionServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdConversion>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProdConversionServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdConversion>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProdConversion>> QueryAsync()
        {
            List<tb_ProdConversion> list = await  _tb_ProdConversionServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(list);
            return list;
        }
        
        public virtual List<tb_ProdConversion> Query()
        {
            List<tb_ProdConversion> list =  _tb_ProdConversionServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(list);
            return list;
        }
        
        public virtual List<tb_ProdConversion> Query(string wheresql)
        {
            List<tb_ProdConversion> list =  _tb_ProdConversionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProdConversion>> QueryAsync(string wheresql) 
        {
            List<tb_ProdConversion> list = await _tb_ProdConversionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProdConversion>> QueryAsync(Expression<Func<tb_ProdConversion, bool>> exp)
        {
            List<tb_ProdConversion> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdConversion>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdConversion>> QueryByNavAsync()
        {
            List<tb_ProdConversion> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdConversion>()
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_location )
                                            .Includes(t => t.tb_ProdConversionDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdConversion>> QueryByNavAsync(Expression<Func<tb_ProdConversion, bool>> exp)
        {
            List<tb_ProdConversion> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdConversion>().Where(exp)
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_location )
                                            .Includes(t => t.tb_ProdConversionDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProdConversion> QueryByNav(Expression<Func<tb_ProdConversion, bool>> exp)
        {
            List<tb_ProdConversion> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdConversion>().Where(exp)
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_location )
                                        .Includes(t => t.tb_ProdConversionDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProdConversion>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdConversion>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProdConversionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProdConversion entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdConversion>().Where(w => w.ConversionID == (long)id)
                             .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_location )
                                        .Includes(t => t.tb_ProdConversionDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProdConversion>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



