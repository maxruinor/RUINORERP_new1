
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:17
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
    /// 单位换算表
    /// </summary>
    public partial class tb_Unit_ConversionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_Unit_ConversionServices _tb_Unit_ConversionServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_Unit_ConversionController(ILogger<tb_Unit_ConversionController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_Unit_ConversionServices tb_Unit_ConversionServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_Unit_ConversionServices = tb_Unit_ConversionServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_Unit_Conversion info)
        {

           // tb_Unit_ConversionValidator validator = new tb_Unit_ConversionValidator();
           tb_Unit_ConversionValidator validator = _appContext.GetRequiredService<tb_Unit_ConversionValidator>();
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
        public async Task<ReturnResults<tb_Unit_Conversion>> SaveOrUpdate(tb_Unit_Conversion entity)
        {
            ReturnResults<tb_Unit_Conversion> rr = new ReturnResults<tb_Unit_Conversion>();
            tb_Unit_Conversion Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UnitConversion_ID > 0)
                {
                    bool rs = await _tb_Unit_ConversionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_Unit_ConversionServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(entity);
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
            tb_Unit_Conversion entity = model as tb_Unit_Conversion;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UnitConversion_ID > 0)
                {
                    bool rs = await _tb_Unit_ConversionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_Unit_ConversionServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(entity);
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
            List<T> list = await _tb_Unit_ConversionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Unit_Conversion entity = item as tb_Unit_Conversion;
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
            List<T> list = await _tb_Unit_ConversionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Unit_Conversion entity = item as tb_Unit_Conversion;
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
            tb_Unit_Conversion entity = model as tb_Unit_Conversion;
            bool rs = await _tb_Unit_ConversionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Unit_Conversion>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Unit_Conversion> entitys = models as List<tb_Unit_Conversion>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Unit_Conversion>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.UnitConversion_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Unit_Conversion>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_Unit_ConversionValidator validator = new tb_Unit_ConversionValidator();
           tb_Unit_ConversionValidator validator = _appContext.GetRequiredService<tb_Unit_ConversionValidator>();
            ValidationResult results = validator.Validate(info as tb_Unit_Conversion);
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
                tb_Unit_Conversion entity = model as tb_Unit_Conversion;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.UnitConversion_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Unit_Conversion>(entity as tb_Unit_Conversion)
                        .Include(m => m.tb_BOM_SDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Unit_Conversion>(entity as tb_Unit_Conversion)
                .Include(m => m.tb_BOM_SDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.UnitConversion_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Unit_Conversion>()
                                .Includes(m => m.tb_BOM_SDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Unit_Conversion entity = model as tb_Unit_Conversion;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Unit_Conversion>(m => m.UnitConversion_ID== entity.UnitConversion_ID)
                                .Include(m => m.tb_BOM_SDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Unit_Conversion AddReEntity(tb_Unit_Conversion entity)
        {
            tb_Unit_Conversion AddEntity =  _tb_Unit_ConversionServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Unit_Conversion> AddReEntityAsync(tb_Unit_Conversion entity)
        {
            tb_Unit_Conversion AddEntity = await _tb_Unit_ConversionServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Unit_Conversion entity)
        {
            long id = await _tb_Unit_ConversionServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Unit_Conversion> infos)
        {
            List<long> ids = await _tb_Unit_ConversionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Unit_Conversion entity)
        {
            bool rs = await _tb_Unit_ConversionServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Unit_Conversion>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Unit_Conversion entity)
        {
            bool rs = await _tb_Unit_ConversionServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_Unit_ConversionServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Unit_Conversion>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_Unit_ConversionServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Unit_Conversion>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Unit_Conversion>> QueryAsync()
        {
            List<tb_Unit_Conversion> list = await  _tb_Unit_ConversionServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(list);
            return list;
        }
        
        public virtual List<tb_Unit_Conversion> Query()
        {
            List<tb_Unit_Conversion> list =  _tb_Unit_ConversionServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(list);
            return list;
        }
        
        public virtual List<tb_Unit_Conversion> Query(string wheresql)
        {
            List<tb_Unit_Conversion> list =  _tb_Unit_ConversionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Unit_Conversion>> QueryAsync(string wheresql) 
        {
            List<tb_Unit_Conversion> list = await _tb_Unit_ConversionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Unit_Conversion>> QueryAsync(Expression<Func<tb_Unit_Conversion, bool>> exp)
        {
            List<tb_Unit_Conversion> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Unit_Conversion>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Unit_Conversion>> QueryByNavAsync()
        {
            List<tb_Unit_Conversion> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Unit_Conversion>()
                                            .Includes(t => t.tb_BOM_SDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Unit_Conversion>> QueryByNavAsync(Expression<Func<tb_Unit_Conversion, bool>> exp)
        {
            List<tb_Unit_Conversion> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Unit_Conversion>().Where(exp)
                                            .Includes(t => t.tb_BOM_SDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Unit_Conversion> QueryByNav(Expression<Func<tb_Unit_Conversion, bool>> exp)
        {
            List<tb_Unit_Conversion> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Unit_Conversion>().Where(exp)
                                        .Includes(t => t.tb_BOM_SDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Unit_Conversion>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Unit_Conversion>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_Unit_ConversionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Unit_Conversion entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Unit_Conversion>().Where(w => w.UnitConversion_ID == (long)id)
                                         .Includes(t => t.tb_BOM_SDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Unit_Conversion>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



