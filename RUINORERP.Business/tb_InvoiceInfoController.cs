
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:06
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
    /// 开票资料
    /// </summary>
    public partial class tb_InvoiceInfoController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_InvoiceInfoServices _tb_InvoiceInfoServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_InvoiceInfoController(ILogger<tb_InvoiceInfoController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_InvoiceInfoServices tb_InvoiceInfoServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_InvoiceInfoServices = tb_InvoiceInfoServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_InvoiceInfo info)
        {

           // tb_InvoiceInfoValidator validator = new tb_InvoiceInfoValidator();
           tb_InvoiceInfoValidator validator = _appContext.GetRequiredService<tb_InvoiceInfoValidator>();
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
        public async Task<ReturnResults<tb_InvoiceInfo>> SaveOrUpdate(tb_InvoiceInfo entity)
        {
            ReturnResults<tb_InvoiceInfo> rr = new ReturnResults<tb_InvoiceInfo>();
            tb_InvoiceInfo Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.InvoiceInfo_ID > 0)
                {
                    bool rs = await _tb_InvoiceInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_InvoiceInfoServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(entity);
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
            tb_InvoiceInfo entity = model as tb_InvoiceInfo;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.InvoiceInfo_ID > 0)
                {
                    bool rs = await _tb_InvoiceInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_InvoiceInfoServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(entity);
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
            List<T> list = await _tb_InvoiceInfoServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_InvoiceInfo entity = item as tb_InvoiceInfo;
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
            List<T> list = await _tb_InvoiceInfoServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_InvoiceInfo entity = item as tb_InvoiceInfo;
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
            tb_InvoiceInfo entity = model as tb_InvoiceInfo;
            bool rs = await _tb_InvoiceInfoServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_InvoiceInfo>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_InvoiceInfo> entitys = models as List<tb_InvoiceInfo>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_InvoiceInfo>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.InvoiceInfo_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_InvoiceInfo>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_InvoiceInfoValidator validator = new tb_InvoiceInfoValidator();
           tb_InvoiceInfoValidator validator = _appContext.GetRequiredService<tb_InvoiceInfoValidator>();
            ValidationResult results = validator.Validate(info as tb_InvoiceInfo);
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
                tb_InvoiceInfo entity = model as tb_InvoiceInfo;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.InvoiceInfo_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_InvoiceInfo>(entity as tb_InvoiceInfo)
                        .Include(m => m.tb_Contracts)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_InvoiceInfo>(entity as tb_InvoiceInfo)
                .Include(m => m.tb_Contracts)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.InvoiceInfo_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_InvoiceInfo>()
                                .Includes(m => m.tb_Contracts)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_InvoiceInfo entity = model as tb_InvoiceInfo;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_InvoiceInfo>(m => m.InvoiceInfo_ID== entity.InvoiceInfo_ID)
                                .Include(m => m.tb_Contracts)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_InvoiceInfo AddReEntity(tb_InvoiceInfo entity)
        {
            tb_InvoiceInfo AddEntity =  _tb_InvoiceInfoServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_InvoiceInfo> AddReEntityAsync(tb_InvoiceInfo entity)
        {
            tb_InvoiceInfo AddEntity = await _tb_InvoiceInfoServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_InvoiceInfo entity)
        {
            long id = await _tb_InvoiceInfoServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_InvoiceInfo> infos)
        {
            List<long> ids = await _tb_InvoiceInfoServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_InvoiceInfo entity)
        {
            bool rs = await _tb_InvoiceInfoServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_InvoiceInfo>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_InvoiceInfo entity)
        {
            bool rs = await _tb_InvoiceInfoServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_InvoiceInfoServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_InvoiceInfo>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_InvoiceInfoServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_InvoiceInfo>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_InvoiceInfo>> QueryAsync()
        {
            List<tb_InvoiceInfo> list = await  _tb_InvoiceInfoServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(list);
            return list;
        }
        
        public virtual List<tb_InvoiceInfo> Query()
        {
            List<tb_InvoiceInfo> list =  _tb_InvoiceInfoServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(list);
            return list;
        }
        
        public virtual List<tb_InvoiceInfo> Query(string wheresql)
        {
            List<tb_InvoiceInfo> list =  _tb_InvoiceInfoServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(list);
            return list;
        }
        
        public virtual async Task<List<tb_InvoiceInfo>> QueryAsync(string wheresql) 
        {
            List<tb_InvoiceInfo> list = await _tb_InvoiceInfoServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_InvoiceInfo>> QueryAsync(Expression<Func<tb_InvoiceInfo, bool>> exp)
        {
            List<tb_InvoiceInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_InvoiceInfo>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_InvoiceInfo>> QueryByNavAsync()
        {
            List<tb_InvoiceInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_InvoiceInfo>()
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_Contracts )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_InvoiceInfo>> QueryByNavAsync(Expression<Func<tb_InvoiceInfo, bool>> exp)
        {
            List<tb_InvoiceInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_InvoiceInfo>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_Contracts )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_InvoiceInfo> QueryByNav(Expression<Func<tb_InvoiceInfo, bool>> exp)
        {
            List<tb_InvoiceInfo> list = _unitOfWorkManage.GetDbClient().Queryable<tb_InvoiceInfo>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_Contracts )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_InvoiceInfo>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_InvoiceInfo>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_InvoiceInfoServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_InvoiceInfo entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_InvoiceInfo>().Where(w => w.InvoiceInfo_ID == (long)id)
                             .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_Contracts )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_InvoiceInfo>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



