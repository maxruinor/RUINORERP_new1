
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:37
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
    /// 标准物料表BOM明细-要适当冗余
    /// </summary>
    public partial class tb_BOM_SDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_BOM_SDetailServices _tb_BOM_SDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_BOM_SDetailController(ILogger<tb_BOM_SDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_BOM_SDetailServices tb_BOM_SDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_BOM_SDetailServices = tb_BOM_SDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_BOM_SDetail info)
        {

           // tb_BOM_SDetailValidator validator = new tb_BOM_SDetailValidator();
           tb_BOM_SDetailValidator validator = _appContext.GetRequiredService<tb_BOM_SDetailValidator>();
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
        public async Task<ReturnResults<tb_BOM_SDetail>> SaveOrUpdate(tb_BOM_SDetail entity)
        {
            ReturnResults<tb_BOM_SDetail> rr = new ReturnResults<tb_BOM_SDetail>();
            tb_BOM_SDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SubID > 0)
                {
                    bool rs = await _tb_BOM_SDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_BOM_SDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(entity);
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
            tb_BOM_SDetail entity = model as tb_BOM_SDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SubID > 0)
                {
                    bool rs = await _tb_BOM_SDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_BOM_SDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(entity);
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
            List<T> list = await _tb_BOM_SDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_BOM_SDetail entity = item as tb_BOM_SDetail;
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
            List<T> list = await _tb_BOM_SDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_BOM_SDetail entity = item as tb_BOM_SDetail;
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
            tb_BOM_SDetail entity = model as tb_BOM_SDetail;
            bool rs = await _tb_BOM_SDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_BOM_SDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_BOM_SDetail> entitys = models as List<tb_BOM_SDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_BOM_SDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.SubID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_BOM_SDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_BOM_SDetailValidator validator = new tb_BOM_SDetailValidator();
           tb_BOM_SDetailValidator validator = _appContext.GetRequiredService<tb_BOM_SDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_BOM_SDetail);
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

                tb_BOM_SDetail entity = model as tb_BOM_SDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.SubID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_BOM_SDetail>(entity as tb_BOM_SDetail)
                        .Include(m => m.tb_BOM_SDetailSubstituteMaterials)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_BOM_SDetail>(entity as tb_BOM_SDetail)
                .Include(m => m.tb_BOM_SDetailSubstituteMaterials)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.SubID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>()
                                .Includes(m => m.tb_BOM_SDetailSubstituteMaterials)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_BOM_SDetail entity = model as tb_BOM_SDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_BOM_SDetail>(m => m.SubID== entity.SubID)
                                .Include(m => m.tb_BOM_SDetailSubstituteMaterials)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_BOM_SDetail AddReEntity(tb_BOM_SDetail entity)
        {
            tb_BOM_SDetail AddEntity =  _tb_BOM_SDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_BOM_SDetail> AddReEntityAsync(tb_BOM_SDetail entity)
        {
            tb_BOM_SDetail AddEntity = await _tb_BOM_SDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_BOM_SDetail entity)
        {
            long id = await _tb_BOM_SDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_BOM_SDetail> infos)
        {
            List<long> ids = await _tb_BOM_SDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_BOM_SDetail entity)
        {
            bool rs = await _tb_BOM_SDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BOM_SDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_BOM_SDetail entity)
        {
            bool rs = await _tb_BOM_SDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_BOM_SDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BOM_SDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_BOM_SDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BOM_SDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_BOM_SDetail>> QueryAsync()
        {
            List<tb_BOM_SDetail> list = await  _tb_BOM_SDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(list);
            return list;
        }
        
        public virtual List<tb_BOM_SDetail> Query()
        {
            List<tb_BOM_SDetail> list =  _tb_BOM_SDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(list);
            return list;
        }
        
        public virtual List<tb_BOM_SDetail> Query(string wheresql)
        {
            List<tb_BOM_SDetail> list =  _tb_BOM_SDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_BOM_SDetail>> QueryAsync(string wheresql) 
        {
            List<tb_BOM_SDetail> list = await _tb_BOM_SDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_BOM_SDetail>> QueryAsync(Expression<Func<tb_BOM_SDetail, bool>> exp)
        {
            List<tb_BOM_SDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BOM_SDetail>> QueryByNavAsync()
        {
            List<tb_BOM_SDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>()
                               .Includes(t => t.tb_unit )
                               .Includes(t => t.tb_unit_conversion )
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_proddetail )
                                            .Includes(t => t.tb_BOM_SDetailSubstituteMaterials )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BOM_SDetail>> QueryByNavAsync(Expression<Func<tb_BOM_SDetail, bool>> exp)
        {
            List<tb_BOM_SDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>().Where(exp)
                               .Includes(t => t.tb_unit )
                               .Includes(t => t.tb_unit_conversion )
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_proddetail )
                                            .Includes(t => t.tb_BOM_SDetailSubstituteMaterials )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_BOM_SDetail> QueryByNav(Expression<Func<tb_BOM_SDetail, bool>> exp)
        {
            List<tb_BOM_SDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>().Where(exp)
                            .Includes(t => t.tb_unit )
                            .Includes(t => t.tb_unit_conversion )
                            .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_proddetail )
                                        .Includes(t => t.tb_BOM_SDetailSubstituteMaterials )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_BOM_SDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_BOM_SDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_BOM_SDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>().Where(w => w.SubID == (long)id)
                             .Includes(t => t.tb_unit )
                            .Includes(t => t.tb_unit_conversion )
                            .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_proddetail )
                                        .Includes(t => t.tb_BOM_SDetailSubstituteMaterials )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_BOM_SDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



