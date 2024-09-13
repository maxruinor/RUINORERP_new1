
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:59
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
    /// 字段权限表
    /// </summary>
    public partial class tb_P4FieldController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_P4FieldServices _tb_P4FieldServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_P4FieldController(ILogger<tb_P4FieldController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_P4FieldServices tb_P4FieldServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_P4FieldServices = tb_P4FieldServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_P4Field info)
        {
            tb_P4FieldValidator validator = new tb_P4FieldValidator();
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
        public async Task<ReturnResults<tb_P4Field>> SaveOrUpdate(tb_P4Field entity)
        {
            ReturnResults<tb_P4Field> rr = new ReturnResults<tb_P4Field>();
            tb_P4Field Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.P4Field_ID > 0)
                {
                    bool rs = await _tb_P4FieldServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_P4FieldServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(entity);
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
            tb_P4Field entity = model as tb_P4Field;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.P4Field_ID > 0)
                {
                    bool rs = await _tb_P4FieldServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_P4FieldServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(entity);
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
            List<T> list = await _tb_P4FieldServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_P4Field entity = item as tb_P4Field;
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
            List<T> list = await _tb_P4FieldServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_P4Field entity = item as tb_P4Field;
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
            tb_P4Field entity = model as tb_P4Field;
            bool rs = await _tb_P4FieldServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_P4Field>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_P4Field> entitys = models as List<tb_P4Field>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_P4Field>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.P4Field_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_P4Field>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_P4FieldValidator validator = new tb_P4FieldValidator();
            ValidationResult results = validator.Validate(info as tb_P4Field);
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
                tb_P4Field entity = model as tb_P4Field;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.P4Field_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_P4Field>(entity as tb_P4Field)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_P4Field>(entity as tb_P4Field)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.P4Field_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_P4Field>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_P4Field entity = model as tb_P4Field;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_P4Field>(m => m.P4Field_ID== entity.P4Field_ID)
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
        
        
        
        public tb_P4Field AddReEntity(tb_P4Field entity)
        {
            tb_P4Field AddEntity =  _tb_P4FieldServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_P4Field> AddReEntityAsync(tb_P4Field entity)
        {
            tb_P4Field AddEntity = await _tb_P4FieldServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_P4Field entity)
        {
            long id = await _tb_P4FieldServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_P4Field> infos)
        {
            List<long> ids = await _tb_P4FieldServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_P4Field entity)
        {
            bool rs = await _tb_P4FieldServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_P4Field>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_P4Field entity)
        {
            bool rs = await _tb_P4FieldServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_P4FieldServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_P4Field>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_P4FieldServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_P4Field>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_P4Field>> QueryAsync()
        {
            List<tb_P4Field> list = await  _tb_P4FieldServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(list);
            return list;
        }
        
        public virtual List<tb_P4Field> Query()
        {
            List<tb_P4Field> list =  _tb_P4FieldServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(list);
            return list;
        }
        
        public virtual List<tb_P4Field> Query(string wheresql)
        {
            List<tb_P4Field> list =  _tb_P4FieldServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(list);
            return list;
        }
        
        public virtual async Task<List<tb_P4Field>> QueryAsync(string wheresql) 
        {
            List<tb_P4Field> list = await _tb_P4FieldServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_P4Field>> QueryAsync(Expression<Func<tb_P4Field, bool>> exp)
        {
            List<tb_P4Field> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4Field>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_P4Field>> QueryByNavAsync()
        {
            List<tb_P4Field> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4Field>()
                               .Includes(t => t.tb_fieldinfo )
                               .Includes(t => t.tb_menuinfo )
                               .Includes(t => t.tb_roleinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_P4Field>> QueryByNavAsync(Expression<Func<tb_P4Field, bool>> exp)
        {
            List<tb_P4Field> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4Field>().Where(exp)
                               .Includes(t => t.tb_fieldinfo )
                               .Includes(t => t.tb_menuinfo )
                               .Includes(t => t.tb_roleinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_P4Field> QueryByNav(Expression<Func<tb_P4Field, bool>> exp)
        {
            List<tb_P4Field> list = _unitOfWorkManage.GetDbClient().Queryable<tb_P4Field>().Where(exp)
                            .Includes(t => t.tb_fieldinfo )
                            .Includes(t => t.tb_menuinfo )
                            .Includes(t => t.tb_roleinfo )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_P4Field>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_P4Field>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_P4FieldServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_P4Field entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4Field>().Where(w => w.P4Field_ID == (long)id)
                             .Includes(t => t.tb_fieldinfo )
                            .Includes(t => t.tb_menuinfo )
                            .Includes(t => t.tb_roleinfo )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_P4Field>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



