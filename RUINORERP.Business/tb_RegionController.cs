
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2024 10:31:40
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
    /// 地区表
    /// </summary>
    public partial class tb_RegionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_RegionServices _tb_RegionServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_RegionController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_RegionServices tb_RegionServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_RegionServices = tb_RegionServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_Region info)
        {
            tb_RegionValidator validator = new tb_RegionValidator();
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
        public async Task<ReturnResults<tb_Region>> SaveOrUpdate(tb_Region entity)
        {
            ReturnResults<tb_Region> rr = new ReturnResults<tb_Region>();
            tb_Region Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Region_ID > 0)
                {
                    bool rs = await _tb_RegionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Region>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_RegionServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Region>(entity);
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
            tb_Region entity = model as tb_Region;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Region_ID > 0)
                {
                    bool rs = await _tb_RegionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Region>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_RegionServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Region>(entity);
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
            List<T> list = await _tb_RegionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Region entity = item as tb_Region;
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
            List<T> list = await _tb_RegionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Region entity = item as tb_Region;
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
            tb_Region entity = model as tb_Region;
            bool rs = await _tb_RegionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Region>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Region> entitys = models as List<tb_Region>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Region>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Region_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Region>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_RegionValidator validator = new tb_RegionValidator();
            ValidationResult results = validator.Validate(info as tb_Region);
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
                tb_Region entity = model as tb_Region;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.Region_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Region>(entity as tb_Region)
                        .Include(m => m.tb_Regions)
                    .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Region>(entity as tb_Region)
                .Include(m => m.tb_Regions)
                        .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Region_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Region>()
                                .Includes(m => m.tb_Regions)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Region entity = model as tb_Region;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Region>(m => m.Region_ID== entity.Region_ID)
                                .Include(m => m.tb_Regions)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Region AddReEntity(tb_Region entity)
        {
            tb_Region AddEntity =  _tb_RegionServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Region>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Region> AddReEntityAsync(tb_Region entity)
        {
            tb_Region AddEntity = await _tb_RegionServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Region>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Region entity)
        {
            long id = await _tb_RegionServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Region>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Region> infos)
        {
            List<long> ids = await _tb_RegionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Region>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Region entity)
        {
            bool rs = await _tb_RegionServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Region>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Region entity)
        {
            bool rs = await _tb_RegionServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Region>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_RegionServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Region>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_RegionServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Region>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Region>> QueryAsync()
        {
            List<tb_Region> list = await  _tb_RegionServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Region>(list);
            return list;
        }
        
        public virtual List<tb_Region> Query()
        {
            List<tb_Region> list =  _tb_RegionServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Region>(list);
            return list;
        }
        
        public virtual List<tb_Region> Query(string wheresql)
        {
            List<tb_Region> list =  _tb_RegionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Region>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Region>> QueryAsync(string wheresql) 
        {
            List<tb_Region> list = await _tb_RegionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Region>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Region>> QueryAsync(Expression<Func<tb_Region, bool>> exp)
        {
            List<tb_Region> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Region>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Region>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Region>> QueryByNavAsync()
        {
            List<tb_Region> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Region>()
                               .Includes(t => t.tb_region )
                               .Includes(t => t.tb_customer )
                                            .Includes(t => t.tb_Regions )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Region>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Region>> QueryByNavAsync(Expression<Func<tb_Region, bool>> exp)
        {
            List<tb_Region> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Region>().Where(exp)
                               .Includes(t => t.tb_region )
                               .Includes(t => t.tb_customer )
                                            .Includes(t => t.tb_Regions )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Region>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Region> QueryByNav(Expression<Func<tb_Region, bool>> exp)
        {
            List<tb_Region> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Region>().Where(exp)
                            .Includes(t => t.tb_region )
                            .Includes(t => t.tb_customer )
                                        .Includes(t => t.tb_Regions )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Region>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Region>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Region>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_RegionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Region entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Region>().Where(w => w.Region_ID == (long)id)
                             .Includes(t => t.tb_region )
                            .Includes(t => t.tb_customer )
                                        .Includes(t => t.tb_Regions )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Region>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



