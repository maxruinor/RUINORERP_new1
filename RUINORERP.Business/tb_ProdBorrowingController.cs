
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:47
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
    /// 产品借出单
    /// </summary>
    public partial class tb_ProdBorrowingController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProdBorrowingServices _tb_ProdBorrowingServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProdBorrowingController(ILogger<tb_ProdBorrowingController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProdBorrowingServices tb_ProdBorrowingServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProdBorrowingServices = tb_ProdBorrowingServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProdBorrowing info)
        {

           // tb_ProdBorrowingValidator validator = new tb_ProdBorrowingValidator();
           tb_ProdBorrowingValidator validator = _appContext.GetRequiredService<tb_ProdBorrowingValidator>();
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
        public async Task<ReturnResults<tb_ProdBorrowing>> SaveOrUpdate(tb_ProdBorrowing entity)
        {
            ReturnResults<tb_ProdBorrowing> rr = new ReturnResults<tb_ProdBorrowing>();
            tb_ProdBorrowing Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BorrowID > 0)
                {
                    bool rs = await _tb_ProdBorrowingServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProdBorrowingServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(entity);
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
            tb_ProdBorrowing entity = model as tb_ProdBorrowing;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BorrowID > 0)
                {
                    bool rs = await _tb_ProdBorrowingServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProdBorrowingServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(entity);
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
            List<T> list = await _tb_ProdBorrowingServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProdBorrowing entity = item as tb_ProdBorrowing;
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
            List<T> list = await _tb_ProdBorrowingServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProdBorrowing entity = item as tb_ProdBorrowing;
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
            tb_ProdBorrowing entity = model as tb_ProdBorrowing;
            bool rs = await _tb_ProdBorrowingServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBorrowing>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProdBorrowing> entitys = models as List<tb_ProdBorrowing>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProdBorrowing>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.BorrowID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBorrowing>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProdBorrowingValidator validator = new tb_ProdBorrowingValidator();
           tb_ProdBorrowingValidator validator = _appContext.GetRequiredService<tb_ProdBorrowingValidator>();
            ValidationResult results = validator.Validate(info as tb_ProdBorrowing);
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

                tb_ProdBorrowing entity = model as tb_ProdBorrowing;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.BorrowID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProdBorrowing>(entity as tb_ProdBorrowing)
                        .Include(m => m.tb_ProdReturnings)
                    .Include(m => m.tb_ProdBorrowingDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProdBorrowing>(entity as tb_ProdBorrowing)
                .Include(m => m.tb_ProdReturnings)
                .Include(m => m.tb_ProdBorrowingDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.BorrowID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowing>()
                                .Includes(m => m.tb_ProdReturnings)
                        .Includes(m => m.tb_ProdBorrowingDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProdBorrowing entity = model as tb_ProdBorrowing;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProdBorrowing>(m => m.BorrowID== entity.BorrowID)
                                .Include(m => m.tb_ProdReturnings)
                        .Include(m => m.tb_ProdBorrowingDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProdBorrowing AddReEntity(tb_ProdBorrowing entity)
        {
            tb_ProdBorrowing AddEntity =  _tb_ProdBorrowingServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProdBorrowing> AddReEntityAsync(tb_ProdBorrowing entity)
        {
            tb_ProdBorrowing AddEntity = await _tb_ProdBorrowingServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProdBorrowing entity)
        {
            long id = await _tb_ProdBorrowingServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProdBorrowing> infos)
        {
            List<long> ids = await _tb_ProdBorrowingServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProdBorrowing entity)
        {
            bool rs = await _tb_ProdBorrowingServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBorrowing>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProdBorrowing entity)
        {
            bool rs = await _tb_ProdBorrowingServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProdBorrowingServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBorrowing>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProdBorrowingServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBorrowing>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProdBorrowing>> QueryAsync()
        {
            List<tb_ProdBorrowing> list = await  _tb_ProdBorrowingServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(list);
            return list;
        }
        
        public virtual List<tb_ProdBorrowing> Query()
        {
            List<tb_ProdBorrowing> list =  _tb_ProdBorrowingServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(list);
            return list;
        }
        
        public virtual List<tb_ProdBorrowing> Query(string wheresql)
        {
            List<tb_ProdBorrowing> list =  _tb_ProdBorrowingServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProdBorrowing>> QueryAsync(string wheresql) 
        {
            List<tb_ProdBorrowing> list = await _tb_ProdBorrowingServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProdBorrowing>> QueryAsync(Expression<Func<tb_ProdBorrowing, bool>> exp)
        {
            List<tb_ProdBorrowing> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowing>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdBorrowing>> QueryByNavAsync()
        {
            List<tb_ProdBorrowing> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowing>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_ProdReturnings )
                                .Includes(t => t.tb_ProdBorrowingDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdBorrowing>> QueryByNavAsync(Expression<Func<tb_ProdBorrowing, bool>> exp)
        {
            List<tb_ProdBorrowing> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowing>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_ProdReturnings )
                                .Includes(t => t.tb_ProdBorrowingDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProdBorrowing> QueryByNav(Expression<Func<tb_ProdBorrowing, bool>> exp)
        {
            List<tb_ProdBorrowing> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowing>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_ProdReturnings )
                            .Includes(t => t.tb_ProdBorrowingDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProdBorrowing>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowing>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProdBorrowingServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProdBorrowing entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowing>().Where(w => w.BorrowID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_ProdReturnings )
                            .Includes(t => t.tb_ProdBorrowingDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowing>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



