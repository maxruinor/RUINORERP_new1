
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:13
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
    /// 品质检验记录表
    /// </summary>
    public partial class tb_Quality_inspection_recordController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_Quality_inspection_recordServices _tb_Quality_inspection_recordServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_Quality_inspection_recordController(ILogger<tb_Quality_inspection_recordController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_Quality_inspection_recordServices tb_Quality_inspection_recordServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_Quality_inspection_recordServices = tb_Quality_inspection_recordServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_Quality_inspection_record info)
        {

           // tb_Quality_inspection_recordValidator validator = new tb_Quality_inspection_recordValidator();
           tb_Quality_inspection_recordValidator validator = _appContext.GetRequiredService<tb_Quality_inspection_recordValidator>();
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
        public async Task<ReturnResults<tb_Quality_inspection_record>> SaveOrUpdate(tb_Quality_inspection_record entity)
        {
            ReturnResults<tb_Quality_inspection_record> rr = new ReturnResults<tb_Quality_inspection_record>();
            tb_Quality_inspection_record Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.id > 0)
                {
                    bool rs = await _tb_Quality_inspection_recordServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_Quality_inspection_recordServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(entity);
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
            tb_Quality_inspection_record entity = model as tb_Quality_inspection_record;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.id > 0)
                {
                    bool rs = await _tb_Quality_inspection_recordServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_Quality_inspection_recordServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(entity);
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
            List<T> list = await _tb_Quality_inspection_recordServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Quality_inspection_record entity = item as tb_Quality_inspection_record;
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
            List<T> list = await _tb_Quality_inspection_recordServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Quality_inspection_record entity = item as tb_Quality_inspection_record;
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
            tb_Quality_inspection_record entity = model as tb_Quality_inspection_record;
            bool rs = await _tb_Quality_inspection_recordServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Quality_inspection_record>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Quality_inspection_record> entitys = models as List<tb_Quality_inspection_record>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Quality_inspection_record>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Quality_inspection_record>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_Quality_inspection_recordValidator validator = new tb_Quality_inspection_recordValidator();
           tb_Quality_inspection_recordValidator validator = _appContext.GetRequiredService<tb_Quality_inspection_recordValidator>();
            ValidationResult results = validator.Validate(info as tb_Quality_inspection_record);
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
                tb_Quality_inspection_record entity = model as tb_Quality_inspection_record;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Quality_inspection_record>(entity as tb_Quality_inspection_record)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Quality_inspection_record>(entity as tb_Quality_inspection_record)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Quality_inspection_record>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Quality_inspection_record entity = model as tb_Quality_inspection_record;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Quality_inspection_record>(m => m.id== entity.id)
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
        
        
        
        public tb_Quality_inspection_record AddReEntity(tb_Quality_inspection_record entity)
        {
            tb_Quality_inspection_record AddEntity =  _tb_Quality_inspection_recordServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Quality_inspection_record> AddReEntityAsync(tb_Quality_inspection_record entity)
        {
            tb_Quality_inspection_record AddEntity = await _tb_Quality_inspection_recordServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Quality_inspection_record entity)
        {
            long id = await _tb_Quality_inspection_recordServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Quality_inspection_record> infos)
        {
            List<long> ids = await _tb_Quality_inspection_recordServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Quality_inspection_record entity)
        {
            bool rs = await _tb_Quality_inspection_recordServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Quality_inspection_record>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Quality_inspection_record entity)
        {
            bool rs = await _tb_Quality_inspection_recordServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_Quality_inspection_recordServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Quality_inspection_record>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_Quality_inspection_recordServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Quality_inspection_record>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Quality_inspection_record>> QueryAsync()
        {
            List<tb_Quality_inspection_record> list = await  _tb_Quality_inspection_recordServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(list);
            return list;
        }
        
        public virtual List<tb_Quality_inspection_record> Query()
        {
            List<tb_Quality_inspection_record> list =  _tb_Quality_inspection_recordServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(list);
            return list;
        }
        
        public virtual List<tb_Quality_inspection_record> Query(string wheresql)
        {
            List<tb_Quality_inspection_record> list =  _tb_Quality_inspection_recordServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Quality_inspection_record>> QueryAsync(string wheresql) 
        {
            List<tb_Quality_inspection_record> list = await _tb_Quality_inspection_recordServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Quality_inspection_record>> QueryAsync(Expression<Func<tb_Quality_inspection_record, bool>> exp)
        {
            List<tb_Quality_inspection_record> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Quality_inspection_record>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Quality_inspection_record>> QueryByNavAsync()
        {
            List<tb_Quality_inspection_record> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Quality_inspection_record>()
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Quality_inspection_record>> QueryByNavAsync(Expression<Func<tb_Quality_inspection_record, bool>> exp)
        {
            List<tb_Quality_inspection_record> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Quality_inspection_record>().Where(exp)
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Quality_inspection_record> QueryByNav(Expression<Func<tb_Quality_inspection_record, bool>> exp)
        {
            List<tb_Quality_inspection_record> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Quality_inspection_record>().Where(exp)
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Quality_inspection_record>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Quality_inspection_record>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_Quality_inspection_recordServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Quality_inspection_record entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Quality_inspection_record>().Where(w => w.id == (long)id)
                                     .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Quality_inspection_record>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



