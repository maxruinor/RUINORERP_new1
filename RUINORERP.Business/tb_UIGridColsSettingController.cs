
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/30/2024 00:18:28
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
    /// UI表格列设置
    /// </summary>
    public partial class tb_UIGridColsSettingController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_UIGridColsSettingServices _tb_UIGridColsSettingServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_UIGridColsSettingController(ILogger<tb_UIGridColsSettingController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_UIGridColsSettingServices tb_UIGridColsSettingServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_UIGridColsSettingServices = tb_UIGridColsSettingServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_UIGridColsSetting info)
        {
            tb_UIGridColsSettingValidator validator = new tb_UIGridColsSettingValidator();
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
        public async Task<ReturnResults<tb_UIGridColsSetting>> SaveOrUpdate(tb_UIGridColsSetting entity)
        {
            ReturnResults<tb_UIGridColsSetting> rr = new ReturnResults<tb_UIGridColsSetting>();
            tb_UIGridColsSetting Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UIGCID > 0)
                {
                    bool rs = await _tb_UIGridColsSettingServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_UIGridColsSettingServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(entity);
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
            tb_UIGridColsSetting entity = model as tb_UIGridColsSetting;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UIGCID > 0)
                {
                    bool rs = await _tb_UIGridColsSettingServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_UIGridColsSettingServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(entity);
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
            List<T> list = await _tb_UIGridColsSettingServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_UIGridColsSetting entity = item as tb_UIGridColsSetting;
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
            List<T> list = await _tb_UIGridColsSettingServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_UIGridColsSetting entity = item as tb_UIGridColsSetting;
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
            tb_UIGridColsSetting entity = model as tb_UIGridColsSetting;
            bool rs = await _tb_UIGridColsSettingServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_UIGridColsSetting>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_UIGridColsSetting> entitys = models as List<tb_UIGridColsSetting>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_UIGridColsSetting>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.UIGCID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_UIGridColsSetting>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_UIGridColsSettingValidator validator = new tb_UIGridColsSettingValidator();
            ValidationResult results = validator.Validate(info as tb_UIGridColsSetting);
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
                tb_UIGridColsSetting entity = model as tb_UIGridColsSetting;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.UIGCID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_UIGridColsSetting>(entity as tb_UIGridColsSetting)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_UIGridColsSetting>(entity as tb_UIGridColsSetting)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.UIGCID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridColsSetting>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_UIGridColsSetting entity = model as tb_UIGridColsSetting;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_UIGridColsSetting>(m => m.UIGCID== entity.UIGCID)
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
        
        
        
        public tb_UIGridColsSetting AddReEntity(tb_UIGridColsSetting entity)
        {
            tb_UIGridColsSetting AddEntity =  _tb_UIGridColsSettingServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_UIGridColsSetting> AddReEntityAsync(tb_UIGridColsSetting entity)
        {
            tb_UIGridColsSetting AddEntity = await _tb_UIGridColsSettingServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_UIGridColsSetting entity)
        {
            long id = await _tb_UIGridColsSettingServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_UIGridColsSetting> infos)
        {
            List<long> ids = await _tb_UIGridColsSettingServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_UIGridColsSetting entity)
        {
            bool rs = await _tb_UIGridColsSettingServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIGridColsSetting>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_UIGridColsSetting entity)
        {
            bool rs = await _tb_UIGridColsSettingServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_UIGridColsSettingServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIGridColsSetting>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_UIGridColsSettingServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIGridColsSetting>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_UIGridColsSetting>> QueryAsync()
        {
            List<tb_UIGridColsSetting> list = await  _tb_UIGridColsSettingServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(list);
            return list;
        }
        
        public virtual List<tb_UIGridColsSetting> Query()
        {
            List<tb_UIGridColsSetting> list =  _tb_UIGridColsSettingServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(list);
            return list;
        }
        
        public virtual List<tb_UIGridColsSetting> Query(string wheresql)
        {
            List<tb_UIGridColsSetting> list =  _tb_UIGridColsSettingServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(list);
            return list;
        }
        
        public virtual async Task<List<tb_UIGridColsSetting>> QueryAsync(string wheresql) 
        {
            List<tb_UIGridColsSetting> list = await _tb_UIGridColsSettingServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_UIGridColsSetting>> QueryAsync(Expression<Func<tb_UIGridColsSetting, bool>> exp)
        {
            List<tb_UIGridColsSetting> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridColsSetting>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIGridColsSetting>> QueryByNavAsync()
        {
            List<tb_UIGridColsSetting> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridColsSetting>()
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIGridColsSetting>> QueryByNavAsync(Expression<Func<tb_UIGridColsSetting, bool>> exp)
        {
            List<tb_UIGridColsSetting> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridColsSetting>().Where(exp)
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_UIGridColsSetting> QueryByNav(Expression<Func<tb_UIGridColsSetting, bool>> exp)
        {
            List<tb_UIGridColsSetting> list = _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridColsSetting>().Where(exp)
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_UIGridColsSetting>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridColsSetting>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_UIGridColsSettingServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_UIGridColsSetting entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIGridColsSetting>().Where(w => w.UIGCID == (long)id)
                                     .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_UIGridColsSetting>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



