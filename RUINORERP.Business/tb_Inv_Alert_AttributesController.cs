
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/13/2023 17:38:18
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
    /// 存货预警特性表
    /// </summary>
    public partial class tb_Inv_Alert_AttributesController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_Inv_Alert_AttributesServices _tb_Inv_Alert_AttributesServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_Inv_Alert_AttributesController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_Inv_Alert_AttributesServices tb_Inv_Alert_AttributesServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_Inv_Alert_AttributesServices = tb_Inv_Alert_AttributesServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_Inv_Alert_Attributes info)
        {
            tb_Inv_Alert_AttributesValidator validator = new tb_Inv_Alert_AttributesValidator();
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
        public async Task<ReturnResults<tb_Inv_Alert_Attributes>> SaveOrUpdate(tb_Inv_Alert_Attributes entity)
        {
            ReturnResults<tb_Inv_Alert_Attributes> rr = new ReturnResults<tb_Inv_Alert_Attributes>();
            tb_Inv_Alert_Attributes Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Inv_Alert_ID > 0)
                {
                    bool rs = await _tb_Inv_Alert_AttributesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_Inv_Alert_AttributesServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(entity);
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
            tb_Inv_Alert_Attributes entity = model as tb_Inv_Alert_Attributes;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Inv_Alert_ID > 0)
                {
                    bool rs = await _tb_Inv_Alert_AttributesServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_Inv_Alert_AttributesServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(entity);
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
            List<T> list = await _tb_Inv_Alert_AttributesServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Inv_Alert_Attributes entity = item as tb_Inv_Alert_Attributes;
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
            List<T> list = await _tb_Inv_Alert_AttributesServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Inv_Alert_Attributes entity = item as tb_Inv_Alert_Attributes;
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
            tb_Inv_Alert_Attributes entity = model as tb_Inv_Alert_Attributes;
            bool rs = await _tb_Inv_Alert_AttributesServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Inv_Alert_Attributes>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Inv_Alert_Attributes> entitys = models as List<tb_Inv_Alert_Attributes>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Inv_Alert_Attributes>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Inv_Alert_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Inv_Alert_Attributes>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_Inv_Alert_AttributesValidator validator = new tb_Inv_Alert_AttributesValidator();
            ValidationResult results = validator.Validate(info as tb_Inv_Alert_Attributes);
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
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_Inv_Alert_Attributes entity = model as tb_Inv_Alert_Attributes;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.Inv_Alert_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Inv_Alert_Attributes>(entity as tb_Inv_Alert_Attributes)
            //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Inv_Alert_Attributes>(entity as tb_Inv_Alert_Attributes)
        //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Inv_Alert_ID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                //出错后，取消生成的ID等值
                command.Undo();
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                _logger.Error("BaseSaveOrUpdateWithChild事务回滚");
                rsms.ErrorMsg = "事务回滚=>" + ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attributes>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Inv_Alert_Attributes entity = model as tb_Inv_Alert_Attributes;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Inv_Alert_Attributes>(m => m.Inv_Alert_ID== entity.Inv_Alert_ID)
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
        
        
        
        public tb_Inv_Alert_Attributes AddReEntity(tb_Inv_Alert_Attributes entity)
        {
            tb_Inv_Alert_Attributes AddEntity =  _tb_Inv_Alert_AttributesServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Inv_Alert_Attributes> AddReEntityAsync(tb_Inv_Alert_Attributes entity)
        {
            tb_Inv_Alert_Attributes AddEntity = await _tb_Inv_Alert_AttributesServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Inv_Alert_Attributes entity)
        {
            long id = await _tb_Inv_Alert_AttributesServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Inv_Alert_Attributes> infos)
        {
            List<long> ids = await _tb_Inv_Alert_AttributesServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Inv_Alert_Attributes entity)
        {
            bool rs = await _tb_Inv_Alert_AttributesServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Inv_Alert_Attributes>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Inv_Alert_Attributes entity)
        {
            bool rs = await _tb_Inv_Alert_AttributesServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_Inv_Alert_AttributesServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Inv_Alert_Attributes>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_Inv_Alert_AttributesServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Inv_Alert_Attributes>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Inv_Alert_Attributes>> QueryAsync()
        {
            List<tb_Inv_Alert_Attributes> list = await  _tb_Inv_Alert_AttributesServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(list);
            return list;
        }
        
        public virtual List<tb_Inv_Alert_Attributes> Query()
        {
            List<tb_Inv_Alert_Attributes> list =  _tb_Inv_Alert_AttributesServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(list);
            return list;
        }
        
        public virtual List<tb_Inv_Alert_Attributes> Query(string wheresql)
        {
            List<tb_Inv_Alert_Attributes> list =  _tb_Inv_Alert_AttributesServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Inv_Alert_Attributes>> QueryAsync(string wheresql) 
        {
            List<tb_Inv_Alert_Attributes> list = await _tb_Inv_Alert_AttributesServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Inv_Alert_Attributes>> QueryAsync(Expression<Func<tb_Inv_Alert_Attributes, bool>> exp)
        {
            List<tb_Inv_Alert_Attributes> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attributes>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Inv_Alert_Attributes>> QueryByNavAsync()
        {
            List<tb_Inv_Alert_Attributes> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attributes>()
                               .Includes(t => t.tb_Inventory )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Inv_Alert_Attributes>> QueryByNavAsync(Expression<Func<tb_Inv_Alert_Attributes, bool>> exp)
        {
            List<tb_Inv_Alert_Attributes> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attributes>().Where(exp)
                               .Includes(t => t.tb_Inventory )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Inv_Alert_Attributes> QueryByNav(Expression<Func<tb_Inv_Alert_Attributes, bool>> exp)
        {
            List<tb_Inv_Alert_Attributes> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attributes>().Where(exp)
                            .Includes(t => t.tb_Inventory )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Inv_Alert_Attributes>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attributes>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_Inv_Alert_AttributesServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Inv_Alert_Attributes entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attributes>().Where(w => w.Inv_Alert_ID == (long)id)
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Inv_Alert_Attributes>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



