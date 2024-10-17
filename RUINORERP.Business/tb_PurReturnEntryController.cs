
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/16/2024 20:05:36
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
    /// 采购退货入库单
    /// </summary>
    public partial class tb_PurReturnEntryController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_PurReturnEntryServices _tb_PurReturnEntryServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_PurReturnEntryController(ILogger<tb_PurReturnEntryController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_PurReturnEntryServices tb_PurReturnEntryServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_PurReturnEntryServices = tb_PurReturnEntryServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_PurReturnEntry info)
        {
            tb_PurReturnEntryValidator validator = new tb_PurReturnEntryValidator();
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
        public async Task<ReturnResults<tb_PurReturnEntry>> SaveOrUpdate(tb_PurReturnEntry entity)
        {
            ReturnResults<tb_PurReturnEntry> rr = new ReturnResults<tb_PurReturnEntry>();
            tb_PurReturnEntry Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurReEntry_ID > 0)
                {
                    bool rs = await _tb_PurReturnEntryServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_PurReturnEntryServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(entity);
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
            tb_PurReturnEntry entity = model as tb_PurReturnEntry;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurReEntry_ID > 0)
                {
                    bool rs = await _tb_PurReturnEntryServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_PurReturnEntryServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(entity);
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
            List<T> list = await _tb_PurReturnEntryServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_PurReturnEntry entity = item as tb_PurReturnEntry;
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
            List<T> list = await _tb_PurReturnEntryServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_PurReturnEntry entity = item as tb_PurReturnEntry;
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
            tb_PurReturnEntry entity = model as tb_PurReturnEntry;
            bool rs = await _tb_PurReturnEntryServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_PurReturnEntry>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_PurReturnEntry> entitys = models as List<tb_PurReturnEntry>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_PurReturnEntry>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PurReEntry_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_PurReturnEntry>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_PurReturnEntryValidator validator = new tb_PurReturnEntryValidator();
            ValidationResult results = validator.Validate(info as tb_PurReturnEntry);
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
                tb_PurReturnEntry entity = model as tb_PurReturnEntry;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PurReEntry_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_PurReturnEntry>(entity as tb_PurReturnEntry)
                        .Include(m => m.tb_PurReturnEntryDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_PurReturnEntry>(entity as tb_PurReturnEntry)
                .Include(m => m.tb_PurReturnEntryDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PurReEntry_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurReturnEntry>()
                                .Includes(m => m.tb_PurReturnEntryDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_PurReturnEntry entity = model as tb_PurReturnEntry;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_PurReturnEntry>(m => m.PurReEntry_ID== entity.PurReEntry_ID)
                                .Include(m => m.tb_PurReturnEntryDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_PurReturnEntry AddReEntity(tb_PurReturnEntry entity)
        {
            tb_PurReturnEntry AddEntity =  _tb_PurReturnEntryServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_PurReturnEntry> AddReEntityAsync(tb_PurReturnEntry entity)
        {
            tb_PurReturnEntry AddEntity = await _tb_PurReturnEntryServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_PurReturnEntry entity)
        {
            long id = await _tb_PurReturnEntryServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_PurReturnEntry> infos)
        {
            List<long> ids = await _tb_PurReturnEntryServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_PurReturnEntry entity)
        {
            bool rs = await _tb_PurReturnEntryServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurReturnEntry>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_PurReturnEntry entity)
        {
            bool rs = await _tb_PurReturnEntryServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_PurReturnEntryServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurReturnEntry>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_PurReturnEntryServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurReturnEntry>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_PurReturnEntry>> QueryAsync()
        {
            List<tb_PurReturnEntry> list = await  _tb_PurReturnEntryServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(list);
            return list;
        }
        
        public virtual List<tb_PurReturnEntry> Query()
        {
            List<tb_PurReturnEntry> list =  _tb_PurReturnEntryServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(list);
            return list;
        }
        
        public virtual List<tb_PurReturnEntry> Query(string wheresql)
        {
            List<tb_PurReturnEntry> list =  _tb_PurReturnEntryServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(list);
            return list;
        }
        
        public virtual async Task<List<tb_PurReturnEntry>> QueryAsync(string wheresql) 
        {
            List<tb_PurReturnEntry> list = await _tb_PurReturnEntryServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_PurReturnEntry>> QueryAsync(Expression<Func<tb_PurReturnEntry, bool>> exp)
        {
            List<tb_PurReturnEntry> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurReturnEntry>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurReturnEntry>> QueryByNavAsync()
        {
            List<tb_PurReturnEntry> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurReturnEntry>()
                               .Includes(t => t.tb_purentryre )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_PurReturnEntryDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurReturnEntry>> QueryByNavAsync(Expression<Func<tb_PurReturnEntry, bool>> exp)
        {
            List<tb_PurReturnEntry> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurReturnEntry>().Where(exp)
                               .Includes(t => t.tb_purentryre )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_PurReturnEntryDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_PurReturnEntry> QueryByNav(Expression<Func<tb_PurReturnEntry, bool>> exp)
        {
            List<tb_PurReturnEntry> list = _unitOfWorkManage.GetDbClient().Queryable<tb_PurReturnEntry>().Where(exp)
                            .Includes(t => t.tb_purentryre )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_PurReturnEntryDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_PurReturnEntry>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurReturnEntry>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_PurReturnEntryServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_PurReturnEntry entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurReturnEntry>().Where(w => w.PurReEntry_ID == (long)id)
                             .Includes(t => t.tb_purentryre )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_PurReturnEntryDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_PurReturnEntry>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



