
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 22:58:11
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
    /// UI录入数据预设值表
    /// </summary>
    public partial class tb_UIInputDataFieldController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_UIInputDataFieldServices _tb_UIInputDataFieldServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_UIInputDataFieldController(ILogger<tb_UIInputDataFieldController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_UIInputDataFieldServices tb_UIInputDataFieldServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_UIInputDataFieldServices = tb_UIInputDataFieldServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_UIInputDataField info)
        {

           // tb_UIInputDataFieldValidator validator = new tb_UIInputDataFieldValidator();
           tb_UIInputDataFieldValidator validator = _appContext.GetRequiredService<tb_UIInputDataFieldValidator>();
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
        public async Task<ReturnResults<tb_UIInputDataField>> SaveOrUpdate(tb_UIInputDataField entity)
        {
            ReturnResults<tb_UIInputDataField> rr = new ReturnResults<tb_UIInputDataField>();
            tb_UIInputDataField Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PresetValueID > 0)
                {
                    bool rs = await _tb_UIInputDataFieldServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_UIInputDataFieldServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(entity);
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
            tb_UIInputDataField entity = model as tb_UIInputDataField;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PresetValueID > 0)
                {
                    bool rs = await _tb_UIInputDataFieldServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_UIInputDataFieldServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(entity);
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
            List<T> list = await _tb_UIInputDataFieldServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_UIInputDataField entity = item as tb_UIInputDataField;
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
            List<T> list = await _tb_UIInputDataFieldServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_UIInputDataField entity = item as tb_UIInputDataField;
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
            tb_UIInputDataField entity = model as tb_UIInputDataField;
            bool rs = await _tb_UIInputDataFieldServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_UIInputDataField>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_UIInputDataField> entitys = models as List<tb_UIInputDataField>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_UIInputDataField>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PresetValueID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_UIInputDataField>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_UIInputDataFieldValidator validator = new tb_UIInputDataFieldValidator();
           tb_UIInputDataFieldValidator validator = _appContext.GetRequiredService<tb_UIInputDataFieldValidator>();
            ValidationResult results = validator.Validate(info as tb_UIInputDataField);
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

                tb_UIInputDataField entity = model as tb_UIInputDataField;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PresetValueID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_UIInputDataField>(entity as tb_UIInputDataField)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_UIInputDataField>(entity as tb_UIInputDataField)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PresetValueID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIInputDataField>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_UIInputDataField entity = model as tb_UIInputDataField;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_UIInputDataField>(m => m.PresetValueID== entity.PresetValueID)
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
        
        
        
        public tb_UIInputDataField AddReEntity(tb_UIInputDataField entity)
        {
            tb_UIInputDataField AddEntity =  _tb_UIInputDataFieldServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_UIInputDataField> AddReEntityAsync(tb_UIInputDataField entity)
        {
            tb_UIInputDataField AddEntity = await _tb_UIInputDataFieldServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_UIInputDataField entity)
        {
            long id = await _tb_UIInputDataFieldServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_UIInputDataField> infos)
        {
            List<long> ids = await _tb_UIInputDataFieldServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_UIInputDataField entity)
        {
            bool rs = await _tb_UIInputDataFieldServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIInputDataField>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_UIInputDataField entity)
        {
            bool rs = await _tb_UIInputDataFieldServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_UIInputDataFieldServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIInputDataField>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_UIInputDataFieldServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIInputDataField>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_UIInputDataField>> QueryAsync()
        {
            List<tb_UIInputDataField> list = await  _tb_UIInputDataFieldServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(list);
            return list;
        }
        
        public virtual List<tb_UIInputDataField> Query()
        {
            List<tb_UIInputDataField> list =  _tb_UIInputDataFieldServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(list);
            return list;
        }
        
        public virtual List<tb_UIInputDataField> Query(string wheresql)
        {
            List<tb_UIInputDataField> list =  _tb_UIInputDataFieldServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(list);
            return list;
        }
        
        public virtual async Task<List<tb_UIInputDataField>> QueryAsync(string wheresql) 
        {
            List<tb_UIInputDataField> list = await _tb_UIInputDataFieldServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_UIInputDataField>> QueryAsync(Expression<Func<tb_UIInputDataField, bool>> exp)
        {
            List<tb_UIInputDataField> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIInputDataField>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIInputDataField>> QueryByNavAsync()
        {
            List<tb_UIInputDataField> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIInputDataField>()
                               .Includes(t => t.tb_uimenupersonalization )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIInputDataField>> QueryByNavAsync(Expression<Func<tb_UIInputDataField, bool>> exp)
        {
            List<tb_UIInputDataField> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIInputDataField>().Where(exp)
                               .Includes(t => t.tb_uimenupersonalization )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_UIInputDataField> QueryByNav(Expression<Func<tb_UIInputDataField, bool>> exp)
        {
            List<tb_UIInputDataField> list = _unitOfWorkManage.GetDbClient().Queryable<tb_UIInputDataField>().Where(exp)
                            .Includes(t => t.tb_uimenupersonalization )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_UIInputDataField>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIInputDataField>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_UIInputDataFieldServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_UIInputDataField entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIInputDataField>().Where(w => w.PresetValueID == (long)id)
                             .Includes(t => t.tb_uimenupersonalization )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_UIInputDataField>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



