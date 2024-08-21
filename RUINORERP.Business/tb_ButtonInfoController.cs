
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2024 10:31:31
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
    /// 字段信息表
    /// </summary>
    public partial class tb_ButtonInfoController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ButtonInfoServices _tb_ButtonInfoServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ButtonInfoController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ButtonInfoServices tb_ButtonInfoServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ButtonInfoServices = tb_ButtonInfoServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_ButtonInfo info)
        {
            tb_ButtonInfoValidator validator = new tb_ButtonInfoValidator();
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
        public async Task<ReturnResults<tb_ButtonInfo>> SaveOrUpdate(tb_ButtonInfo entity)
        {
            ReturnResults<tb_ButtonInfo> rr = new ReturnResults<tb_ButtonInfo>();
            tb_ButtonInfo Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ButtonInfo_ID > 0)
                {
                    bool rs = await _tb_ButtonInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ButtonInfoServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(entity);
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
            tb_ButtonInfo entity = model as tb_ButtonInfo;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ButtonInfo_ID > 0)
                {
                    bool rs = await _tb_ButtonInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ButtonInfoServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(entity);
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
            List<T> list = await _tb_ButtonInfoServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ButtonInfo entity = item as tb_ButtonInfo;
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
            List<T> list = await _tb_ButtonInfoServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ButtonInfo entity = item as tb_ButtonInfo;
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
            tb_ButtonInfo entity = model as tb_ButtonInfo;
            bool rs = await _tb_ButtonInfoServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ButtonInfo>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ButtonInfo> entitys = models as List<tb_ButtonInfo>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ButtonInfo>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ButtonInfo_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ButtonInfo>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_ButtonInfoValidator validator = new tb_ButtonInfoValidator();
            ValidationResult results = validator.Validate(info as tb_ButtonInfo);
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
                tb_ButtonInfo entity = model as tb_ButtonInfo;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.ButtonInfo_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ButtonInfo>(entity as tb_ButtonInfo)
                        .Include(m => m.tb_P4Buttons)
                    .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ButtonInfo>(entity as tb_ButtonInfo)
                .Include(m => m.tb_P4Buttons)
                        .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ButtonInfo_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ButtonInfo>()
                                .Includes(m => m.tb_P4Buttons)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ButtonInfo entity = model as tb_ButtonInfo;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ButtonInfo>(m => m.ButtonInfo_ID== entity.ButtonInfo_ID)
                                .Include(m => m.tb_P4Buttons)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ButtonInfo AddReEntity(tb_ButtonInfo entity)
        {
            tb_ButtonInfo AddEntity =  _tb_ButtonInfoServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ButtonInfo> AddReEntityAsync(tb_ButtonInfo entity)
        {
            tb_ButtonInfo AddEntity = await _tb_ButtonInfoServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ButtonInfo entity)
        {
            long id = await _tb_ButtonInfoServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ButtonInfo> infos)
        {
            List<long> ids = await _tb_ButtonInfoServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ButtonInfo entity)
        {
            bool rs = await _tb_ButtonInfoServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ButtonInfo>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ButtonInfo entity)
        {
            bool rs = await _tb_ButtonInfoServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ButtonInfoServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ButtonInfo>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ButtonInfoServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ButtonInfo>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ButtonInfo>> QueryAsync()
        {
            List<tb_ButtonInfo> list = await  _tb_ButtonInfoServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(list);
            return list;
        }
        
        public virtual List<tb_ButtonInfo> Query()
        {
            List<tb_ButtonInfo> list =  _tb_ButtonInfoServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(list);
            return list;
        }
        
        public virtual List<tb_ButtonInfo> Query(string wheresql)
        {
            List<tb_ButtonInfo> list =  _tb_ButtonInfoServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ButtonInfo>> QueryAsync(string wheresql) 
        {
            List<tb_ButtonInfo> list = await _tb_ButtonInfoServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ButtonInfo>> QueryAsync(Expression<Func<tb_ButtonInfo, bool>> exp)
        {
            List<tb_ButtonInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ButtonInfo>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ButtonInfo>> QueryByNavAsync()
        {
            List<tb_ButtonInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ButtonInfo>()
                               .Includes(t => t.tb_menuinfo )
                                            .Includes(t => t.tb_P4Buttons )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ButtonInfo>> QueryByNavAsync(Expression<Func<tb_ButtonInfo, bool>> exp)
        {
            List<tb_ButtonInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ButtonInfo>().Where(exp)
                               .Includes(t => t.tb_menuinfo )
                                            .Includes(t => t.tb_P4Buttons )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ButtonInfo> QueryByNav(Expression<Func<tb_ButtonInfo, bool>> exp)
        {
            List<tb_ButtonInfo> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ButtonInfo>().Where(exp)
                            .Includes(t => t.tb_menuinfo )
                                        .Includes(t => t.tb_P4Buttons )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ButtonInfo>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ButtonInfo>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ButtonInfoServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ButtonInfo entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ButtonInfo>().Where(w => w.ButtonInfo_ID == (long)id)
                             .Includes(t => t.tb_menuinfo )
                                        .Includes(t => t.tb_P4Buttons )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ButtonInfo>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



