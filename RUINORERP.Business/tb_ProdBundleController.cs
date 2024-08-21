
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/07/2024 19:06:31
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
    /// 产品套装表
    /// </summary>
    public partial class tb_ProdBundleController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProdBundleServices _tb_ProdBundleServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProdBundleController(ILogger<tb_ProdBundleController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProdBundleServices tb_ProdBundleServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProdBundleServices = tb_ProdBundleServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_ProdBundle info)
        {
            tb_ProdBundleValidator validator = new tb_ProdBundleValidator();
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
        public async Task<ReturnResults<tb_ProdBundle>> SaveOrUpdate(tb_ProdBundle entity)
        {
            ReturnResults<tb_ProdBundle> rr = new ReturnResults<tb_ProdBundle>();
            tb_ProdBundle Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BundleID > 0)
                {
                    bool rs = await _tb_ProdBundleServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProdBundleServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(entity);
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
            tb_ProdBundle entity = model as tb_ProdBundle;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BundleID > 0)
                {
                    bool rs = await _tb_ProdBundleServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProdBundleServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(entity);
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
            List<T> list = await _tb_ProdBundleServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProdBundle entity = item as tb_ProdBundle;
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
            List<T> list = await _tb_ProdBundleServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProdBundle entity = item as tb_ProdBundle;
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
            tb_ProdBundle entity = model as tb_ProdBundle;
            bool rs = await _tb_ProdBundleServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBundle>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProdBundle> entitys = models as List<tb_ProdBundle>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProdBundle>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.BundleID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBundle>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_ProdBundleValidator validator = new tb_ProdBundleValidator();
            ValidationResult results = validator.Validate(info as tb_ProdBundle);
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
                tb_ProdBundle entity = model as tb_ProdBundle;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.BundleID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProdBundle>(entity as tb_ProdBundle)
                        .Include(m => m.tb_Packings)
                    .Include(m => m.tb_ProdBundleDetails)
                    .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProdBundle>(entity as tb_ProdBundle)
                .Include(m => m.tb_Packings)
                .Include(m => m.tb_ProdBundleDetails)
                        .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.BundleID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBundle>()
                                .Includes(m => m.tb_Packings)
                        .Includes(m => m.tb_ProdBundleDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProdBundle entity = model as tb_ProdBundle;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProdBundle>(m => m.BundleID== entity.BundleID)
                                .Include(m => m.tb_Packings)
                        .Include(m => m.tb_ProdBundleDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProdBundle AddReEntity(tb_ProdBundle entity)
        {
            tb_ProdBundle AddEntity =  _tb_ProdBundleServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProdBundle> AddReEntityAsync(tb_ProdBundle entity)
        {
            tb_ProdBundle AddEntity = await _tb_ProdBundleServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProdBundle entity)
        {
            long id = await _tb_ProdBundleServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProdBundle> infos)
        {
            List<long> ids = await _tb_ProdBundleServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProdBundle entity)
        {
            bool rs = await _tb_ProdBundleServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBundle>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProdBundle entity)
        {
            bool rs = await _tb_ProdBundleServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProdBundleServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBundle>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProdBundleServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBundle>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProdBundle>> QueryAsync()
        {
            List<tb_ProdBundle> list = await  _tb_ProdBundleServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(list);
            return list;
        }
        
        public virtual List<tb_ProdBundle> Query()
        {
            List<tb_ProdBundle> list =  _tb_ProdBundleServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(list);
            return list;
        }
        
        public virtual List<tb_ProdBundle> Query(string wheresql)
        {
            List<tb_ProdBundle> list =  _tb_ProdBundleServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProdBundle>> QueryAsync(string wheresql) 
        {
            List<tb_ProdBundle> list = await _tb_ProdBundleServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProdBundle>> QueryAsync(Expression<Func<tb_ProdBundle, bool>> exp)
        {
            List<tb_ProdBundle> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBundle>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdBundle>> QueryByNavAsync()
        {
            List<tb_ProdBundle> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBundle>()
                               .Includes(t => t.tb_unit )
                                            .Includes(t => t.tb_Packings )
                                .Includes(t => t.tb_ProdBundleDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdBundle>> QueryByNavAsync(Expression<Func<tb_ProdBundle, bool>> exp)
        {
            List<tb_ProdBundle> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBundle>().Where(exp)
                               .Includes(t => t.tb_unit )
                                            .Includes(t => t.tb_Packings )
                                .Includes(t => t.tb_ProdBundleDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProdBundle> QueryByNav(Expression<Func<tb_ProdBundle, bool>> exp)
        {
            List<tb_ProdBundle> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBundle>().Where(exp)
                            .Includes(t => t.tb_unit )
                                        .Includes(t => t.tb_Packings )
                            .Includes(t => t.tb_ProdBundleDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProdBundle>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBundle>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProdBundleServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProdBundle entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBundle>().Where(w => w.BundleID == (long)id)
                             .Includes(t => t.tb_unit )
                                        .Includes(t => t.tb_Packings )
                            .Includes(t => t.tb_ProdBundleDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProdBundle>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



