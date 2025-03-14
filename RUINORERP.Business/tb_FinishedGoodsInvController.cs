
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:40
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
    /// 成品入库单 要进一步完善
    /// </summary>
    public partial class tb_FinishedGoodsInvController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FinishedGoodsInvServices _tb_FinishedGoodsInvServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FinishedGoodsInvController(ILogger<tb_FinishedGoodsInvController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FinishedGoodsInvServices tb_FinishedGoodsInvServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FinishedGoodsInvServices = tb_FinishedGoodsInvServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FinishedGoodsInv info)
        {

           // tb_FinishedGoodsInvValidator validator = new tb_FinishedGoodsInvValidator();
           tb_FinishedGoodsInvValidator validator = _appContext.GetRequiredService<tb_FinishedGoodsInvValidator>();
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
        public async Task<ReturnResults<tb_FinishedGoodsInv>> SaveOrUpdate(tb_FinishedGoodsInv entity)
        {
            ReturnResults<tb_FinishedGoodsInv> rr = new ReturnResults<tb_FinishedGoodsInv>();
            tb_FinishedGoodsInv Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.FG_ID > 0)
                {
                    bool rs = await _tb_FinishedGoodsInvServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FinishedGoodsInvServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(entity);
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
            tb_FinishedGoodsInv entity = model as tb_FinishedGoodsInv;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.FG_ID > 0)
                {
                    bool rs = await _tb_FinishedGoodsInvServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FinishedGoodsInvServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(entity);
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
            List<T> list = await _tb_FinishedGoodsInvServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FinishedGoodsInv entity = item as tb_FinishedGoodsInv;
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
            List<T> list = await _tb_FinishedGoodsInvServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FinishedGoodsInv entity = item as tb_FinishedGoodsInv;
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
            tb_FinishedGoodsInv entity = model as tb_FinishedGoodsInv;
            bool rs = await _tb_FinishedGoodsInvServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FinishedGoodsInv>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FinishedGoodsInv> entitys = models as List<tb_FinishedGoodsInv>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FinishedGoodsInv>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.FG_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FinishedGoodsInv>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FinishedGoodsInvValidator validator = new tb_FinishedGoodsInvValidator();
           tb_FinishedGoodsInvValidator validator = _appContext.GetRequiredService<tb_FinishedGoodsInvValidator>();
            ValidationResult results = validator.Validate(info as tb_FinishedGoodsInv);
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

                tb_FinishedGoodsInv entity = model as tb_FinishedGoodsInv;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.FG_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FinishedGoodsInv>(entity as tb_FinishedGoodsInv)
                        .Include(m => m.tb_FinishedGoodsInvDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FinishedGoodsInv>(entity as tb_FinishedGoodsInv)
                .Include(m => m.tb_FinishedGoodsInvDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.FG_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInv>()
                                .Includes(m => m.tb_FinishedGoodsInvDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FinishedGoodsInv entity = model as tb_FinishedGoodsInv;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FinishedGoodsInv>(m => m.FG_ID== entity.FG_ID)
                                .Include(m => m.tb_FinishedGoodsInvDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FinishedGoodsInv AddReEntity(tb_FinishedGoodsInv entity)
        {
            tb_FinishedGoodsInv AddEntity =  _tb_FinishedGoodsInvServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FinishedGoodsInv> AddReEntityAsync(tb_FinishedGoodsInv entity)
        {
            tb_FinishedGoodsInv AddEntity = await _tb_FinishedGoodsInvServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FinishedGoodsInv entity)
        {
            long id = await _tb_FinishedGoodsInvServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FinishedGoodsInv> infos)
        {
            List<long> ids = await _tb_FinishedGoodsInvServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FinishedGoodsInv entity)
        {
            bool rs = await _tb_FinishedGoodsInvServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FinishedGoodsInv>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FinishedGoodsInv entity)
        {
            bool rs = await _tb_FinishedGoodsInvServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FinishedGoodsInvServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FinishedGoodsInv>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FinishedGoodsInvServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FinishedGoodsInv>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FinishedGoodsInv>> QueryAsync()
        {
            List<tb_FinishedGoodsInv> list = await  _tb_FinishedGoodsInvServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(list);
            return list;
        }
        
        public virtual List<tb_FinishedGoodsInv> Query()
        {
            List<tb_FinishedGoodsInv> list =  _tb_FinishedGoodsInvServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(list);
            return list;
        }
        
        public virtual List<tb_FinishedGoodsInv> Query(string wheresql)
        {
            List<tb_FinishedGoodsInv> list =  _tb_FinishedGoodsInvServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FinishedGoodsInv>> QueryAsync(string wheresql) 
        {
            List<tb_FinishedGoodsInv> list = await _tb_FinishedGoodsInvServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FinishedGoodsInv>> QueryAsync(Expression<Func<tb_FinishedGoodsInv, bool>> exp)
        {
            List<tb_FinishedGoodsInv> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInv>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FinishedGoodsInv>> QueryByNavAsync()
        {
            List<tb_FinishedGoodsInv> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInv>()
                               .Includes(t => t.tb_manufacturingorder )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                                            .Includes(t => t.tb_FinishedGoodsInvDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FinishedGoodsInv>> QueryByNavAsync(Expression<Func<tb_FinishedGoodsInv, bool>> exp)
        {
            List<tb_FinishedGoodsInv> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInv>().Where(exp)
                               .Includes(t => t.tb_manufacturingorder )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                                            .Includes(t => t.tb_FinishedGoodsInvDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FinishedGoodsInv> QueryByNav(Expression<Func<tb_FinishedGoodsInv, bool>> exp)
        {
            List<tb_FinishedGoodsInv> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInv>().Where(exp)
                            .Includes(t => t.tb_manufacturingorder )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                                        .Includes(t => t.tb_FinishedGoodsInvDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FinishedGoodsInv>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInv>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FinishedGoodsInvServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FinishedGoodsInv entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInv>().Where(w => w.FG_ID == (long)id)
                             .Includes(t => t.tb_manufacturingorder )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                                        .Includes(t => t.tb_FinishedGoodsInvDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInv>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



