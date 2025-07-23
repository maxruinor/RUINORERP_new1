
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
    /// 卡通箱规格表
    /// </summary>
    public partial class tb_CartoonBoxController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CartoonBoxServices _tb_CartoonBoxServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_CartoonBoxController(ILogger<tb_CartoonBoxController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CartoonBoxServices tb_CartoonBoxServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CartoonBoxServices = tb_CartoonBoxServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_CartoonBox info)
        {

           // tb_CartoonBoxValidator validator = new tb_CartoonBoxValidator();
           tb_CartoonBoxValidator validator = _appContext.GetRequiredService<tb_CartoonBoxValidator>();
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
        public async Task<ReturnResults<tb_CartoonBox>> SaveOrUpdate(tb_CartoonBox entity)
        {
            ReturnResults<tb_CartoonBox> rr = new ReturnResults<tb_CartoonBox>();
            tb_CartoonBox Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.CartonID > 0)
                {
                    bool rs = await _tb_CartoonBoxServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CartoonBoxServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(entity);
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
            tb_CartoonBox entity = model as tb_CartoonBox;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.CartonID > 0)
                {
                    bool rs = await _tb_CartoonBoxServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CartoonBoxServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(entity);
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
            List<T> list = await _tb_CartoonBoxServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_CartoonBox entity = item as tb_CartoonBox;
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
            List<T> list = await _tb_CartoonBoxServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_CartoonBox entity = item as tb_CartoonBox;
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
            tb_CartoonBox entity = model as tb_CartoonBox;
            bool rs = await _tb_CartoonBoxServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_CartoonBox>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_CartoonBox> entitys = models as List<tb_CartoonBox>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_CartoonBox>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.CartonID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_CartoonBox>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_CartoonBoxValidator validator = new tb_CartoonBoxValidator();
           tb_CartoonBoxValidator validator = _appContext.GetRequiredService<tb_CartoonBoxValidator>();
            ValidationResult results = validator.Validate(info as tb_CartoonBox);
            return results;
        }
        
        
        public async override Task<List<T>> BaseQueryByAdvancedAsync(bool useLike,object dto) 
        {
            var  querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().WhereCustom(useLike,dto);
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

                tb_CartoonBox entity = model as tb_CartoonBox;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.CartonID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_CartoonBox>(entity as tb_CartoonBox)
                        .Include(m => m.tb_BoxRuleses)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_CartoonBox>(entity as tb_CartoonBox)
                .Include(m => m.tb_BoxRuleses)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.CartonID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CartoonBox>()
                                .Includes(m => m.tb_BoxRuleses)
                                        .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CartoonBox entity = model as tb_CartoonBox;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CartoonBox>(m => m.CartonID== entity.CartonID)
                                .Include(m => m.tb_BoxRuleses)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_CartoonBox AddReEntity(tb_CartoonBox entity)
        {
            tb_CartoonBox AddEntity =  _tb_CartoonBoxServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CartoonBox> AddReEntityAsync(tb_CartoonBox entity)
        {
            tb_CartoonBox AddEntity = await _tb_CartoonBoxServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CartoonBox entity)
        {
            long id = await _tb_CartoonBoxServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CartoonBox> infos)
        {
            List<long> ids = await _tb_CartoonBoxServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CartoonBox entity)
        {
            bool rs = await _tb_CartoonBoxServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CartoonBox>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CartoonBox entity)
        {
            bool rs = await _tb_CartoonBoxServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CartoonBoxServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CartoonBox>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CartoonBoxServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_CartoonBox>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_CartoonBox>> QueryAsync()
        {
            List<tb_CartoonBox> list = await  _tb_CartoonBoxServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(list);
            return list;
        }
        
        public virtual List<tb_CartoonBox> Query()
        {
            List<tb_CartoonBox> list =  _tb_CartoonBoxServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(list);
            return list;
        }
        
        public virtual List<tb_CartoonBox> Query(string wheresql)
        {
            List<tb_CartoonBox> list =  _tb_CartoonBoxServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CartoonBox>> QueryAsync(string wheresql) 
        {
            List<tb_CartoonBox> list = await _tb_CartoonBoxServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_CartoonBox>> QueryAsync(Expression<Func<tb_CartoonBox, bool>> exp)
        {
            List<tb_CartoonBox> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CartoonBox>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CartoonBox>> QueryByNavAsync()
        {
            List<tb_CartoonBox> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CartoonBox>()
                                            .Includes(t => t.tb_BoxRuleses )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CartoonBox>> QueryByNavAsync(Expression<Func<tb_CartoonBox, bool>> exp)
        {
            List<tb_CartoonBox> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CartoonBox>().Where(exp)
                                            .Includes(t => t.tb_BoxRuleses )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CartoonBox> QueryByNav(Expression<Func<tb_CartoonBox, bool>> exp)
        {
            List<tb_CartoonBox> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CartoonBox>().Where(exp)
                                        .Includes(t => t.tb_BoxRuleses )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CartoonBox>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CartoonBox>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CartoonBoxServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_CartoonBox entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_CartoonBox>().Where(w => w.CartonID == (long)id)
                                         .Includes(t => t.tb_BoxRuleses )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_CartoonBox>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



