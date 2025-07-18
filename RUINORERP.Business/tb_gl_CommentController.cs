
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:43
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
    /// 全局级批注表-对于重点关注的业务帮助记录和跟踪相关的额外信息，提高沟通效率和透明度
    /// </summary>
    public partial class tb_gl_CommentController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_gl_CommentServices _tb_gl_CommentServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_gl_CommentController(ILogger<tb_gl_CommentController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_gl_CommentServices tb_gl_CommentServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_gl_CommentServices = tb_gl_CommentServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_gl_Comment info)
        {

           // tb_gl_CommentValidator validator = new tb_gl_CommentValidator();
           tb_gl_CommentValidator validator = _appContext.GetRequiredService<tb_gl_CommentValidator>();
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
        public async Task<ReturnResults<tb_gl_Comment>> SaveOrUpdate(tb_gl_Comment entity)
        {
            ReturnResults<tb_gl_Comment> rr = new ReturnResults<tb_gl_Comment>();
            tb_gl_Comment Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.CommentID > 0)
                {
                    bool rs = await _tb_gl_CommentServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_gl_CommentServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(entity);
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
            tb_gl_Comment entity = model as tb_gl_Comment;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.CommentID > 0)
                {
                    bool rs = await _tb_gl_CommentServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_gl_CommentServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(entity);
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
            List<T> list = await _tb_gl_CommentServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_gl_Comment entity = item as tb_gl_Comment;
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
            List<T> list = await _tb_gl_CommentServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_gl_Comment entity = item as tb_gl_Comment;
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
            tb_gl_Comment entity = model as tb_gl_Comment;
            bool rs = await _tb_gl_CommentServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_gl_Comment>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_gl_Comment> entitys = models as List<tb_gl_Comment>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_gl_Comment>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.CommentID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_gl_Comment>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_gl_CommentValidator validator = new tb_gl_CommentValidator();
           tb_gl_CommentValidator validator = _appContext.GetRequiredService<tb_gl_CommentValidator>();
            ValidationResult results = validator.Validate(info as tb_gl_Comment);
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

                tb_gl_Comment entity = model as tb_gl_Comment;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.CommentID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_gl_Comment>(entity as tb_gl_Comment)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_gl_Comment>(entity as tb_gl_Comment)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.CommentID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_gl_Comment>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_gl_Comment entity = model as tb_gl_Comment;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_gl_Comment>(m => m.CommentID== entity.CommentID)
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
        
        
        
        public tb_gl_Comment AddReEntity(tb_gl_Comment entity)
        {
            tb_gl_Comment AddEntity =  _tb_gl_CommentServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_gl_Comment> AddReEntityAsync(tb_gl_Comment entity)
        {
            tb_gl_Comment AddEntity = await _tb_gl_CommentServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_gl_Comment entity)
        {
            long id = await _tb_gl_CommentServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_gl_Comment> infos)
        {
            List<long> ids = await _tb_gl_CommentServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_gl_Comment entity)
        {
            bool rs = await _tb_gl_CommentServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_gl_Comment>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_gl_Comment entity)
        {
            bool rs = await _tb_gl_CommentServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_gl_CommentServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_gl_Comment>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_gl_CommentServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_gl_Comment>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_gl_Comment>> QueryAsync()
        {
            List<tb_gl_Comment> list = await  _tb_gl_CommentServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(list);
            return list;
        }
        
        public virtual List<tb_gl_Comment> Query()
        {
            List<tb_gl_Comment> list =  _tb_gl_CommentServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(list);
            return list;
        }
        
        public virtual List<tb_gl_Comment> Query(string wheresql)
        {
            List<tb_gl_Comment> list =  _tb_gl_CommentServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(list);
            return list;
        }
        
        public virtual async Task<List<tb_gl_Comment>> QueryAsync(string wheresql) 
        {
            List<tb_gl_Comment> list = await _tb_gl_CommentServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_gl_Comment>> QueryAsync(Expression<Func<tb_gl_Comment, bool>> exp)
        {
            List<tb_gl_Comment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_gl_Comment>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_gl_Comment>> QueryByNavAsync()
        {
            List<tb_gl_Comment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_gl_Comment>()
                               .Includes(t => t.tb_employee )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_gl_Comment>> QueryByNavAsync(Expression<Func<tb_gl_Comment, bool>> exp)
        {
            List<tb_gl_Comment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_gl_Comment>().Where(exp)
                               .Includes(t => t.tb_employee )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_gl_Comment> QueryByNav(Expression<Func<tb_gl_Comment, bool>> exp)
        {
            List<tb_gl_Comment> list = _unitOfWorkManage.GetDbClient().Queryable<tb_gl_Comment>().Where(exp)
                            .Includes(t => t.tb_employee )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_gl_Comment>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_gl_Comment>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_gl_CommentServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_gl_Comment entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_gl_Comment>().Where(w => w.CommentID == (long)id)
                             .Includes(t => t.tb_employee )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_gl_Comment>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



