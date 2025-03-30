
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/30/2025 15:54:04
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
    /// UI表单输入数据字段设置
    /// </summary>
    public partial class tb_UIDataFieldSettingController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_UIDataFieldSettingServices _tb_UIDataFieldSettingServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_UIDataFieldSettingController(ILogger<tb_UIDataFieldSettingController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_UIDataFieldSettingServices tb_UIDataFieldSettingServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_UIDataFieldSettingServices = tb_UIDataFieldSettingServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_UIDataFieldSetting info)
        {

           // tb_UIDataFieldSettingValidator validator = new tb_UIDataFieldSettingValidator();
           tb_UIDataFieldSettingValidator validator = _appContext.GetRequiredService<tb_UIDataFieldSettingValidator>();
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
        public async Task<ReturnResults<tb_UIDataFieldSetting>> SaveOrUpdate(tb_UIDataFieldSetting entity)
        {
            ReturnResults<tb_UIDataFieldSetting> rr = new ReturnResults<tb_UIDataFieldSetting>();
            tb_UIDataFieldSetting Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UIDFCID > 0)
                {
                    bool rs = await _tb_UIDataFieldSettingServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_UIDataFieldSettingServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(entity);
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
            tb_UIDataFieldSetting entity = model as tb_UIDataFieldSetting;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UIDFCID > 0)
                {
                    bool rs = await _tb_UIDataFieldSettingServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_UIDataFieldSettingServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(entity);
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
            List<T> list = await _tb_UIDataFieldSettingServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_UIDataFieldSetting entity = item as tb_UIDataFieldSetting;
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
            List<T> list = await _tb_UIDataFieldSettingServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_UIDataFieldSetting entity = item as tb_UIDataFieldSetting;
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
            tb_UIDataFieldSetting entity = model as tb_UIDataFieldSetting;
            bool rs = await _tb_UIDataFieldSettingServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_UIDataFieldSetting>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_UIDataFieldSetting> entitys = models as List<tb_UIDataFieldSetting>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_UIDataFieldSetting>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.UIDFCID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_UIDataFieldSetting>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_UIDataFieldSettingValidator validator = new tb_UIDataFieldSettingValidator();
           tb_UIDataFieldSettingValidator validator = _appContext.GetRequiredService<tb_UIDataFieldSettingValidator>();
            ValidationResult results = validator.Validate(info as tb_UIDataFieldSetting);
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

                tb_UIDataFieldSetting entity = model as tb_UIDataFieldSetting;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.UIDFCID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_UIDataFieldSetting>(entity as tb_UIDataFieldSetting)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_UIDataFieldSetting>(entity as tb_UIDataFieldSetting)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.UIDFCID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIDataFieldSetting>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_UIDataFieldSetting entity = model as tb_UIDataFieldSetting;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_UIDataFieldSetting>(m => m.UIDFCID== entity.UIDFCID)
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
        
        
        
        public tb_UIDataFieldSetting AddReEntity(tb_UIDataFieldSetting entity)
        {
            tb_UIDataFieldSetting AddEntity =  _tb_UIDataFieldSettingServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_UIDataFieldSetting> AddReEntityAsync(tb_UIDataFieldSetting entity)
        {
            tb_UIDataFieldSetting AddEntity = await _tb_UIDataFieldSettingServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_UIDataFieldSetting entity)
        {
            long id = await _tb_UIDataFieldSettingServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_UIDataFieldSetting> infos)
        {
            List<long> ids = await _tb_UIDataFieldSettingServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_UIDataFieldSetting entity)
        {
            bool rs = await _tb_UIDataFieldSettingServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIDataFieldSetting>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_UIDataFieldSetting entity)
        {
            bool rs = await _tb_UIDataFieldSettingServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_UIDataFieldSettingServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIDataFieldSetting>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_UIDataFieldSettingServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIDataFieldSetting>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_UIDataFieldSetting>> QueryAsync()
        {
            List<tb_UIDataFieldSetting> list = await  _tb_UIDataFieldSettingServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(list);
            return list;
        }
        
        public virtual List<tb_UIDataFieldSetting> Query()
        {
            List<tb_UIDataFieldSetting> list =  _tb_UIDataFieldSettingServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(list);
            return list;
        }
        
        public virtual List<tb_UIDataFieldSetting> Query(string wheresql)
        {
            List<tb_UIDataFieldSetting> list =  _tb_UIDataFieldSettingServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(list);
            return list;
        }
        
        public virtual async Task<List<tb_UIDataFieldSetting>> QueryAsync(string wheresql) 
        {
            List<tb_UIDataFieldSetting> list = await _tb_UIDataFieldSettingServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_UIDataFieldSetting>> QueryAsync(Expression<Func<tb_UIDataFieldSetting, bool>> exp)
        {
            List<tb_UIDataFieldSetting> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIDataFieldSetting>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIDataFieldSetting>> QueryByNavAsync()
        {
            List<tb_UIDataFieldSetting> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIDataFieldSetting>()
                               .Includes(t => t.tb_uimenupersonalization )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIDataFieldSetting>> QueryByNavAsync(Expression<Func<tb_UIDataFieldSetting, bool>> exp)
        {
            List<tb_UIDataFieldSetting> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIDataFieldSetting>().Where(exp)
                               .Includes(t => t.tb_uimenupersonalization )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_UIDataFieldSetting> QueryByNav(Expression<Func<tb_UIDataFieldSetting, bool>> exp)
        {
            List<tb_UIDataFieldSetting> list = _unitOfWorkManage.GetDbClient().Queryable<tb_UIDataFieldSetting>().Where(exp)
                            .Includes(t => t.tb_uimenupersonalization )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_UIDataFieldSetting>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIDataFieldSetting>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_UIDataFieldSettingServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_UIDataFieldSetting entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIDataFieldSetting>().Where(w => w.UIDFCID == (long)id)
                             .Includes(t => t.tb_uimenupersonalization )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_UIDataFieldSetting>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



