
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/16/2025 10:31:33
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
    /// 维修物料明细表
    /// </summary>
    public partial class tb_AS_RepairOrderMaterialDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_AS_RepairOrderMaterialDetailServices _tb_AS_RepairOrderMaterialDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_AS_RepairOrderMaterialDetailController(ILogger<tb_AS_RepairOrderMaterialDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_AS_RepairOrderMaterialDetailServices tb_AS_RepairOrderMaterialDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_AS_RepairOrderMaterialDetailServices = tb_AS_RepairOrderMaterialDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_AS_RepairOrderMaterialDetail info)
        {

           // tb_AS_RepairOrderMaterialDetailValidator validator = new tb_AS_RepairOrderMaterialDetailValidator();
           tb_AS_RepairOrderMaterialDetailValidator validator = _appContext.GetRequiredService<tb_AS_RepairOrderMaterialDetailValidator>();
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
        public async Task<ReturnResults<tb_AS_RepairOrderMaterialDetail>> SaveOrUpdate(tb_AS_RepairOrderMaterialDetail entity)
        {
            ReturnResults<tb_AS_RepairOrderMaterialDetail> rr = new ReturnResults<tb_AS_RepairOrderMaterialDetail>();
            tb_AS_RepairOrderMaterialDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RepairMaterialDetailID > 0)
                {
                    bool rs = await _tb_AS_RepairOrderMaterialDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_AS_RepairOrderMaterialDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(entity);
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
            tb_AS_RepairOrderMaterialDetail entity = model as tb_AS_RepairOrderMaterialDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RepairMaterialDetailID > 0)
                {
                    bool rs = await _tb_AS_RepairOrderMaterialDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_AS_RepairOrderMaterialDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(entity);
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
            List<T> list = await _tb_AS_RepairOrderMaterialDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_AS_RepairOrderMaterialDetail entity = item as tb_AS_RepairOrderMaterialDetail;
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
            List<T> list = await _tb_AS_RepairOrderMaterialDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_AS_RepairOrderMaterialDetail entity = item as tb_AS_RepairOrderMaterialDetail;
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
            tb_AS_RepairOrderMaterialDetail entity = model as tb_AS_RepairOrderMaterialDetail;
            bool rs = await _tb_AS_RepairOrderMaterialDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_AS_RepairOrderMaterialDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_AS_RepairOrderMaterialDetail> entitys = models as List<tb_AS_RepairOrderMaterialDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_AS_RepairOrderMaterialDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.RepairMaterialDetailID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_AS_RepairOrderMaterialDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_AS_RepairOrderMaterialDetailValidator validator = new tb_AS_RepairOrderMaterialDetailValidator();
           tb_AS_RepairOrderMaterialDetailValidator validator = _appContext.GetRequiredService<tb_AS_RepairOrderMaterialDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_AS_RepairOrderMaterialDetail);
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

                tb_AS_RepairOrderMaterialDetail entity = model as tb_AS_RepairOrderMaterialDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.RepairMaterialDetailID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_RepairOrderMaterialDetail>(entity as tb_AS_RepairOrderMaterialDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_AS_RepairOrderMaterialDetail>(entity as tb_AS_RepairOrderMaterialDetail)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.RepairMaterialDetailID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderMaterialDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_AS_RepairOrderMaterialDetail entity = model as tb_AS_RepairOrderMaterialDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_AS_RepairOrderMaterialDetail>(m => m.RepairMaterialDetailID== entity.RepairMaterialDetailID)
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_AS_RepairOrderMaterialDetail AddReEntity(tb_AS_RepairOrderMaterialDetail entity)
        {
            tb_AS_RepairOrderMaterialDetail AddEntity =  _tb_AS_RepairOrderMaterialDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_AS_RepairOrderMaterialDetail> AddReEntityAsync(tb_AS_RepairOrderMaterialDetail entity)
        {
            tb_AS_RepairOrderMaterialDetail AddEntity = await _tb_AS_RepairOrderMaterialDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_AS_RepairOrderMaterialDetail entity)
        {
            long id = await _tb_AS_RepairOrderMaterialDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_AS_RepairOrderMaterialDetail> infos)
        {
            List<long> ids = await _tb_AS_RepairOrderMaterialDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_AS_RepairOrderMaterialDetail entity)
        {
            bool rs = await _tb_AS_RepairOrderMaterialDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AS_RepairOrderMaterialDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_AS_RepairOrderMaterialDetail entity)
        {
            bool rs = await _tb_AS_RepairOrderMaterialDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_AS_RepairOrderMaterialDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AS_RepairOrderMaterialDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_AS_RepairOrderMaterialDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AS_RepairOrderMaterialDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_AS_RepairOrderMaterialDetail>> QueryAsync()
        {
            List<tb_AS_RepairOrderMaterialDetail> list = await  _tb_AS_RepairOrderMaterialDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(list);
            return list;
        }
        
        public virtual List<tb_AS_RepairOrderMaterialDetail> Query()
        {
            List<tb_AS_RepairOrderMaterialDetail> list =  _tb_AS_RepairOrderMaterialDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(list);
            return list;
        }
        
        public virtual List<tb_AS_RepairOrderMaterialDetail> Query(string wheresql)
        {
            List<tb_AS_RepairOrderMaterialDetail> list =  _tb_AS_RepairOrderMaterialDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_AS_RepairOrderMaterialDetail>> QueryAsync(string wheresql) 
        {
            List<tb_AS_RepairOrderMaterialDetail> list = await _tb_AS_RepairOrderMaterialDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_AS_RepairOrderMaterialDetail>> QueryAsync(Expression<Func<tb_AS_RepairOrderMaterialDetail, bool>> exp)
        {
            List<tb_AS_RepairOrderMaterialDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderMaterialDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AS_RepairOrderMaterialDetail>> QueryByNavAsync()
        {
            List<tb_AS_RepairOrderMaterialDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderMaterialDetail>()
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_as_repairorder )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AS_RepairOrderMaterialDetail>> QueryByNavAsync(Expression<Func<tb_AS_RepairOrderMaterialDetail, bool>> exp)
        {
            List<tb_AS_RepairOrderMaterialDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderMaterialDetail>().Where(exp)
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_as_repairorder )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_AS_RepairOrderMaterialDetail> QueryByNav(Expression<Func<tb_AS_RepairOrderMaterialDetail, bool>> exp)
        {
            List<tb_AS_RepairOrderMaterialDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderMaterialDetail>().Where(exp)
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_as_repairorder )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_AS_RepairOrderMaterialDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderMaterialDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_AS_RepairOrderMaterialDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_AS_RepairOrderMaterialDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderMaterialDetail>().Where(w => w.RepairMaterialDetailID == (long)id)
                             .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_as_repairorder )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrderMaterialDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



