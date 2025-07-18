
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:40:50
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
    /// 
    /// </summary>
    public partial class LogDetailsController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public ILogDetailsServices _LogDetailsServices { get; set; }
        // private readonly ApplicationContext _appContext;

        public LogDetailsController(ILogger<LogDetailsController<T>> logger, IUnitOfWorkManage unitOfWorkManage, LogDetailsServices LogDetailsServices, ApplicationContext appContext = null) : base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _LogDetailsServices = LogDetailsServices;
            _appContext = appContext;
        }


        public ValidationResult Validator(LogDetails info)
        {

            // LogDetailsValidator validator = new LogDetailsValidator();
            LogDetailsValidator validator = _appContext.GetRequiredService<LogDetailsValidator>();
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
        public async Task<ReturnResults<LogDetails>> SaveOrUpdate(LogDetails entity)
        {
            ReturnResults<LogDetails> rr = new ReturnResults<LogDetails>();
            LogDetails Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.LogId > 0)
                {
                    bool rs = await _LogDetailsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<LogDetails>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _LogDetailsServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<LogDetails>(entity);
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
        public async override Task<ReturnResults<T>> BaseSaveOrUpdate(T model)
        {
            ReturnResults<T> rr = new ReturnResults<T>();
            LogDetails entity = model as LogDetails;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.LogId > 0)
                {
                    bool rs = await _LogDetailsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<LogDetails>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _LogDetailsServices.AddReEntityAsync(entity) as T;
                    MyCacheManager.Instance.UpdateEntityList<LogDetails>(entity);
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
            List<T> list = await _LogDetailsServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                LogDetails entity = item as LogDetails;
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
            List<T> list = await _LogDetailsServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                LogDetails entity = item as LogDetails;
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
            LogDetails entity = model as LogDetails;
            bool rs = await _LogDetailsServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<LogDetails>(entity);
            }
            return rs;
        }

        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs = false;
            List<LogDetails> entitys = models as List<LogDetails>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<LogDetails>(entitys).ExecuteCommandAsync();
            if (c > 0)
            {
                rs = true;
                ////生成时暂时只考虑了一个主键的情况
                long[] result = entitys.Select(e => e.LogId.ToLong()).ToArray();
                MyCacheManager.Instance.DeleteEntityList<LogDetails>(result);
            }
            return rs;
        }

        public override ValidationResult BaseValidator(T info)
        {
            //LogDetailsValidator validator = new LogDetailsValidator();
            LogDetailsValidator validator = _appContext.GetRequiredService<LogDetailsValidator>();
            ValidationResult results = validator.Validate(info as LogDetails);
            return results;
        }


        public async override Task<List<T>> BaseQueryByAdvancedAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().Where(useLike, dto);
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

                LogDetails entity = model as LogDetails;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                if (entity.LogId > 0)
                {

                    var result = await _unitOfWorkManage.GetDbClient().Updateable<LogDetails>(entity as LogDetails)
       .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                }
                else
                {
                    var result = await _unitOfWorkManage.GetDbClient().Insertable<LogDetails>(entity as LogDetails)
      .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }


                }

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T;
                entity.PrimaryKeyID = entity.LogId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<LogDetails>()
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync() as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model)
        {
            LogDetails entity = model as LogDetails;
            bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<LogDetails>(m => m.LogId == entity.LogId)
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



        public LogDetails AddReEntity(LogDetails entity)
        {
            LogDetails AddEntity = _LogDetailsServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<LogDetails>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }

        public async Task<LogDetails> AddReEntityAsync(LogDetails entity)
        {
            LogDetails AddEntity = await _LogDetailsServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<LogDetails>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }

        public async Task<long> AddAsync(LogDetails entity)
        {
            long id = await _LogDetailsServices.Add(entity);
            if (id > 0)
            {
                MyCacheManager.Instance.UpdateEntityList<LogDetails>(entity);
            }
            return id;
        }

        public async Task<List<long>> AddAsync(List<LogDetails> infos)
        {
            List<long> ids = await _LogDetailsServices.Add(infos);
            if (ids.Count > 0)//成功的个数 这里缓存 对不对呢？
            {
                MyCacheManager.Instance.UpdateEntityList<LogDetails>(infos);
            }
            return ids;
        }


        public async Task<bool> DeleteAsync(LogDetails entity)
        {
            bool rs = await _LogDetailsServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<LogDetails>(entity);

            }
            return rs;
        }

        public async Task<bool> UpdateAsync(LogDetails entity)
        {
            bool rs = await _LogDetailsServices.Update(entity);
            if (rs)
            {
                MyCacheManager.Instance.UpdateEntityList<LogDetails>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _LogDetailsServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<LogDetails>(id);
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _LogDetailsServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<LogDetails>(ids);
            }
            return rs;
        }

        public virtual async Task<List<LogDetails>> QueryAsync()
        {
            List<LogDetails> list = await _LogDetailsServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<LogDetails>(list);
            return list;
        }

        public virtual List<LogDetails> Query()
        {
            List<LogDetails> list = _LogDetailsServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<LogDetails>(list);
            return list;
        }

        public virtual List<LogDetails> Query(string wheresql)
        {
            List<LogDetails> list = _LogDetailsServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<LogDetails>(list);
            return list;
        }

        public virtual async Task<List<LogDetails>> QueryAsync(string wheresql)
        {
            List<LogDetails> list = await _LogDetailsServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<LogDetails>(list);
            return list;
        }



        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<LogDetails>> QueryAsync(Expression<Func<LogDetails, bool>> exp)
        {
            List<LogDetails> list = await _unitOfWorkManage.GetDbClient().Queryable<LogDetails>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<LogDetails>(list);
            return list;
        }



        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual async Task<List<LogDetails>> QueryByNavAsync()
        {
            List<LogDetails> list = await _unitOfWorkManage.GetDbClient().Queryable<LogDetails>()
                                    .ToListAsync();

            foreach (var item in list)
            {
                item.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<LogDetails>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual async Task<List<LogDetails>> QueryByNavAsync(Expression<Func<LogDetails, bool>> exp)
        {
            List<LogDetails> list = await _unitOfWorkManage.GetDbClient().Queryable<LogDetails>().Where(exp)
                                    .ToListAsync();

            foreach (var item in list)
            {
                item.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<LogDetails>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual List<LogDetails> QueryByNav(Expression<Func<LogDetails, bool>> exp)
        {
            List<LogDetails> list = _unitOfWorkManage.GetDbClient().Queryable<LogDetails>().Where(exp)
                                    .ToList();

            foreach (var item in list)
            {
                item.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<LogDetails>(list);
            return list;
        }



        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<LogDetails>> QueryByAdvancedAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<LogDetails>().Where(useLike, dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _LogDetailsServices.QueryByIdAsync(id) as T;
            return entity;
        }



        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            LogDetails entity = await _unitOfWorkManage.GetDbClient().Queryable<LogDetails>().Where(w => w.LogId == (long)id)
                                     .FirstAsync();
            if (entity != null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<LogDetails>(entity);
            return entity as T;
        }






    }
}



