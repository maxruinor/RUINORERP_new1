using AutoMapper;
using CacheManager.Core;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.RowLevelAuthService;
using RUINORERP.Common;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.EnumExt;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Model.Context;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.Repository.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Services;
using SharpYaml.Tokens;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//using Magicodes.ExporterAndImporter.Core.Extension;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Expression = System.Linq.Expressions.Expression;

namespace RUINORERP.Business
{
    /// <summary>
    /// 单表基础资料操作
    /// </summary>
    public class BaseController<T> : BaseController where T : class
    {
        public ApplicationContext _appContext;
        public ITableSchemaManager _tableSchemaManager;
        public IUnifiedStateManager StateManager;
        public IEntityCacheManager _cacheManager;
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public IUnitOfWorkManage _unitOfWorkManage;
        public ILogger<BaseController<T>> _logger;
        public IMapper mapper { get; set; }
        public string BizTypeText { get; set; }
        public int BizTypeInt { get; set; }

        // 查询缓存管理器
        private ICacheManager<object> _queryCacheManager;

        /// <summary>
        /// 行级权限过滤器服务（注入单例）
        /// </summary>
        private readonly SqlSugarRowLevelAuthFilter _rowLevelAuthFilter;
        public BaseController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext = null)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            StateManager = appContext.GetRequiredService<IUnifiedStateManager>();
            _appContext = appContext;
            _rowLevelAuthFilter = _appContext.GetRequiredService<SqlSugarRowLevelAuthFilter>();
            _tableSchemaManager = appContext.GetRequiredService<ITableSchemaManager>();
            BizType bizType = BizMapperService.EntityMappingHelper.GetBizType(typeof(T).Name);
            BizTypeText = bizType.ToString();
            BizTypeInt = (int)bizType;
            _cacheManager = appContext.GetRequiredService<IEntityCacheManager>();
            mapper = appContext.GetRequiredService<IMapper>();

            // 初始化查询缓存管理器
            _queryCacheManager = CacheFactory.Build<object>(settings =>
                settings
                    .WithSystemRuntimeCacheHandle("QueryCache")
                    .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(15))); // 15分钟过期
        }





        #region  打印数据提供相关
        public virtual Task<List<T>> GetPrintDataSource(long id)
        {

            _logger.LogError(typeof(T).FullName + "子类要重写GetPrintData，保证打印数据的完整提供");
            //子类重写
            throw new Exception("子类要重写GetPrintData，保证打印数据的完整提供。");

        }


        #endregion

        #region  


        #endregion

        #region 提交 

        /// <summary>
        /// 提交单据（数据库业务级实现）
        /// </summary>
        /// <param name="entity">待提交的实体</param>
        /// <param name="autoApprove">是否自动审核</param>
        /// <returns>操作结果</returns>
        public virtual async Task<ReturnResults<T>> SubmitAsync(T entity, bool autoApprove = false)
        {
            var result = new ReturnResults<T>();
            // 参数验证
            if (entity == null)
            {
                result.ErrorMsg = "提交的实体不能为空";
                return result;
            }
            BaseEntity baseEntity = entity as BaseEntity;
            // 获取主键值
            string PrimaryKeyColName = baseEntity.GetPrimaryKeyColName();
            object primaryKeyValue = ReflectionHelper.GetPropertyValue(entity, PrimaryKeyColName);

            // 检查实体是否已存在
            bool isNewEntity = primaryKeyValue == null || Convert.ToInt64(primaryKeyValue) <= 0;
            if (isNewEntity)
            {
                result.ErrorMsg = "单据保存成功后再提交。";
                return result;
            }


            // 获取状态类型和值
            var statusType = StateManager.GetStatusType(baseEntity);
            if (statusType == null)
            {
                result.ErrorMsg = "提交的单据状态不能为空";
                return result;
            }

            // 动态获取状态值
            dynamic status = entity.GetPropertyValue(statusType.Name);
            int statusValue = (int)status;
            dynamic statusEnum = Enum.ToObject(statusType, statusValue);

            // 检查是否可以提交
            //if (!StateManager.ValidateBusinessStatusTransitionAsync(statusEnum,))
            //{
            //    result.ErrorMsg = $"单据当前状态为【{statusEnum}】，无法提交";
            //    return result;
            //}

            try
            {
                // 更新实体状态
                int currentStatusValue = GetSubmitStatus(entity, statusEnum);

                var update = await _unitOfWorkManage.GetDbClient().Updateable<object>()
                             .AS(typeof(T).Name)
                             .SetColumns(statusType.Name, currentStatusValue)
                             .Where(PrimaryKeyColName + "=" + primaryKeyValue).ExecuteCommandAsync();
                if (update > 0)
                {


                    // 自动审核逻辑
                    if (autoApprove && CanAutoApprove(entity))
                    {
                        var approvalResult = await ApprovalAsync(entity);
                        if (!approvalResult.Succeeded)
                        {
                            result.ErrorMsg = $"自动审核失败: {approvalResult.ErrorMsg}";
                            return result;
                        }
                    }

                    // 执行子类特定的提交后逻辑
                    //更新UI？
                    await AfterSubmit(entity);


                    result.Succeeded = true;
                }
                else
                {
                    result.Succeeded = false;
                }

                result.Succeeded = true;
                result.ReturnObject = (T)entity;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "提交单据时发生异常");
                result.ErrorMsg = "提交过程中发生异常，请联系管理员";
                return result;
            }
        }

        /// <summary>
        /// 更新实体状态为提交
        /// 由草稿变为确认或待审核
        /// </summary>
        private int GetSubmitStatus<TEnum>(T entity, TEnum status) where TEnum : Enum
        {
            var statusName = status.GetType().Name;
            if (statusName == typeof(DataStatus).Name)
            {
                if (ReflectionHelper.ExistPropertyName<T>(typeof(DataStatus).Name))
                {
                    ReflectionHelper.SetPropertyValue(entity, typeof(DataStatus).Name, DataStatus.新建);
                }
            }
            else if (statusName == typeof(StatementStatus).Name)
            {
                if (ReflectionHelper.ExistPropertyName<T>(typeof(StatementStatus).Name))
                {
                    ReflectionHelper.SetPropertyValue(entity, typeof(StatementStatus).Name, StatementStatus.新建);
                }
            }
            else if (statusName == typeof(PrePaymentStatus).Name)
            {
                if (ReflectionHelper.ExistPropertyName<T>(typeof(PrePaymentStatus).Name))
                {
                    ReflectionHelper.SetPropertyValue(entity, typeof(PrePaymentStatus).Name, PrePaymentStatus.待审核);
                }
            }
            else if (statusName == typeof(ARAPStatus).Name)
            {
                if (ReflectionHelper.ExistPropertyName<T>(typeof(ARAPStatus).Name))
                {
                    ReflectionHelper.SetPropertyValue(entity, typeof(ARAPStatus).Name, ARAPStatus.待审核);
                }
            }
            else if (statusName == typeof(PaymentStatus).Name)
            {
                if (ReflectionHelper.ExistPropertyName<T>(typeof(PaymentStatus).Name))
                {
                    ReflectionHelper.SetPropertyValue(entity, typeof(PaymentStatus).Name, PaymentStatus.待审核);
                }
            }
            else
            {
                _logger.LogError("基类提交时，没有找到对应的状态类型", "提交单据时发生异常");
            }

            int result = ReflectionHelper.GetPropertyValue(entity, statusName).ToInt();
            return result;
        }


        /// <summary>
        /// 检查是否可以自动审核
        /// </summary>
        protected virtual bool CanAutoApprove(T entity)
        {
            // 默认实现：销售订单和采购订单支持自动审核
            if (entity is tb_SaleOrder saleOrder)
            {
                return _appContext.SysConfig.AutoApprovedSaleOrderAmount > 0 &&
                       saleOrder.TotalAmount <= _appContext.SysConfig.AutoApprovedSaleOrderAmount;
            }

            if (entity is tb_PurOrder purOrder)
            {
                return _appContext.SysConfig.AutoApprovedPurOrderAmount > 0 &&
                       purOrder.TotalAmount <= _appContext.SysConfig.AutoApprovedPurOrderAmount;
            }

            return false;
        }





        /// <summary>
        /// 提交后执行的操作（子类可重写）
        /// </summary>
        protected virtual Task AfterSubmit(T entity)
        {
            // 默认实现为空，子类可重写添加特定逻辑
            return Task.CompletedTask;
        }


        /// <summary>
        /// 获取操作描述
        /// </summary>
        /// <param name="updateType">更新类型</param>
        /// <param name="bizType">业务类型</param>
        /// <returns>操作描述</returns>
        private string GetOperationDescription(TodoUpdateType updateType, BizType bizType)
        {
            return updateType switch
            {
                TodoUpdateType.Created => $"创建了{bizType}单据",
                TodoUpdateType.StatusChanged => $"提交了{bizType}单据",
                TodoUpdateType.Approved => $"审核了{bizType}单据",
                TodoUpdateType.Deleted => $"删除了{bizType}单据",
                _ => $"更新了{bizType}单据状态"
            };
        }


        #endregion

        /// <summary>
        /// 查询指定字段的值是否存在
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistFieldValue(Expression<Func<T, bool>> exp)
        {
            return await _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp).AnyAsync();
        }

        /// <summary>
        /// 根据条件获取单个实体对象
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="whereExp"></param>
        /// <returns>结果只能一行，否则会出错</returns>
        public virtual T IsExistEntity(Expression<Func<T, bool>> whereExp)
        {
            // return _unitOfWorkManage.GetDbClient().Queryable<T>().Where(whereExp).Single();
            var existEntity = _unitOfWorkManage.GetDbClient().Queryable<T>().Where(whereExp).First();
            if (existEntity != null && existEntity is BaseEntity baseEntity)
            {
                baseEntity.AcceptChanges();
            }
            return existEntity;
        }

        /// <summary>
        /// 根据条件获取单个实体对象
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="whereExp"></param>
        /// <returns>结果只能一行，否则会出错</returns>
        public async virtual Task<T> IsExistEntityAsync(Expression<Func<T, bool>> whereExp)
        {
            // return _unitOfWorkManage.GetDbClient().Queryable<T>().Where(whereExp).Single();
            var existEntity = await _unitOfWorkManage.GetDbClient().Queryable<T>().Where(whereExp).FirstAsync();
            //因为我们的更新机制中是根据变化的值来更新的
            if (existEntity != null && existEntity is BaseEntity baseEntity)
            {
                baseEntity.AcceptChanges();
            }
            return existEntity;
        }


        /// <summary>
        /// 对象是否存在
        /// </summary>
        /// <param name="whereLambda">条件表达式</param>
        /// <returns>True or False</returns>
        public async virtual Task<bool> IsExistAsync(Expression<Func<T, bool>> whereLambda = null)
        {
            return await _unitOfWorkManage.GetDbClient().Queryable<T>().WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda).AnyAsync();
        }

        /// <summary>
        /// 对象是否存在
        /// </summary>
        /// <param name="whereLambda">条件表达式</param>
        /// <returns>True or False</returns>
        public virtual async Task<bool> IsExist(Expression<Func<T, bool>> whereLambda = null)
        {
            return await _unitOfWorkManage.GetDbClient().Queryable<T>().WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda).AnyAsync();
        }







        #region 方法



        #endregion

        #region 公共虚拟方法



        public virtual Task<T> BaseQueryByIdAsync(object id)
        {

            //子类重写
            throw new Exception("子类要重写BaseQueryByIdAsync");
        }
        public virtual Task<T> BaseQueryByIdNavAsync(object id)
        {
            throw new Exception("子类要重写BaseQueryByIdNavAsync");
        }
        public virtual Task<List<T>> BaseQueryAsync(string wheresql)
        {
            //子类重写
            throw new Exception("子类要重写QueryAsync");
        }




        public virtual Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T entity) where C : class
        {
            //子类重写
            throw new Exception("子类要重写SaveOrUpdate");
            //return null;
        }

        public virtual Task<ReturnMainSubResults<T>> BaseUpdateWithChild(T entity)
        {
            //子类重写
            throw new Exception("子类要重写SaveOrUpdate");
            //return null;
        }


        public virtual Task<ReturnResults<T>> BaseSaveOrUpdate(T entity)
        {
            //子类重写

            //// 只更新变更的列
            //if (entity.HasChanged)
            //{
            //    var changedColumns = entity.GetChangedColumnMappings().Values;
            //    var update = _db.Updateable(model)
            //        .UpdateColumns(changedColumns.ToArray()); // 关键优化

            //    await update.ExecuteCommandAsync();
            //}


            //entity.AcceptChanges(); // 保存后重置状态


            throw new Exception("子类要重写SaveOrUpdate");
            //return null;
        }

        public virtual ISugarQueryable<T> BaseGetQueryable()
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>().With(SqlWith.NoLock);

            //子类重写
            //throw new Exception("子类要重写BaseGetQueryableAsync");
            //var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
            //     .IncludesAllFirstLayer()
            //    .WhereCustom(useLike,dto);
            //return await querySqlQueryable.ToListAsync();
        }
        public async virtual Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> entitys)
        {
            ReturnResults<bool> result = new ReturnResults<bool>();
            await Task.Delay(0); // 模拟异步操作
            return result;
        }

        /// <summary>
        /// 反结案,这个权限在最终
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<bool>> AntiBatchCloseCaseAsync(List<T> entitys)
        {
            ReturnResults<bool> result = new ReturnResults<bool>();
            await Task.Delay(0); // 模拟异步操作
            return result;
        }
        public virtual Task<List<T>> BaseQueryAsync()
        {
            //子类重写
            throw new Exception("子类要重写BaseQueryAsync");
            //return null;
        }
        public virtual Task<bool> BaseDeleteAsync(T model)
        {
            //子类重写
            throw new Exception("子类要重写BaseDeleteAsync");
            //return null;
        }

        public virtual Task<bool> BaseLogicDeleteAsync(T model)
        {
            //逻辑删除
            //await _unitOfWorkManage.GetDbClient().Deleteable(detail.tb_Prod_Attr_Relations).IsLogic().ExecuteCommandAsync();
            //await _unitOfWorkManage.GetDbClient().Deleteable(detail).IsLogic().ExecuteCommandAsync();
            //子类重写
            throw new Exception("子类要重写BaseLogicDeleteAsync");
            //return null;
        }


        public virtual Task<bool> BaseDeleteAsync(List<T> models)
        {
            throw new Exception("子类要重写BaseDeleteAsync");
        }

        public virtual Task<bool> BaseDeleteByNavAsync(T model)
        {
            //子类重写
            throw new Exception("子类要重写BaseDeleteNavAsync");
            //return null;
        }


        /// <summary>
        /// 审核  审核本身就是一个特殊情况。所以不能批量处理
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsAutoApprove">是否自动审核</param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<T>> ApprovalAsync(T entity, bool IsAutoApprove = false)
        {
            return await ApprovalAsync(entity);
            //throw new Exception("子类要重写ApprovalAsync,请联系管理员");
            ReturnResults<T> result = new ReturnResults<T>();
            await Task.Delay(0); // 模拟异步操作
            return result; // 或者根据实际情况返回值
        }

        /// <summary>
        /// 审核  审核本身就是一个特殊情况。所以不能批量处理
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsAutoApprove">是否自动审核</param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<T>> ApprovalAsync(T entity)
        {
            throw new Exception("子类要重写ApprovalAsync,请联系管理员");
            ReturnResults<T> result = new ReturnResults<T>();
            await Task.Delay(0); // 模拟异步操作
            return result; // 或者根据实际情况返回值
        }



        /// <summary>
        /// 反审核  反审核本身就是一个特殊情况。所以不能批量处理
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<T>> AdvancedSave(T entity)
        {
            ReturnResults<T> result = new ReturnResults<T>();
            await Task.Delay(0); // 模拟异步操作
            return result; // 或者根据实际情况返回值
        }


        /// <summary>
        /// 反审核  反审核本身就是一个特殊情况。所以不能批量处理
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<T>> AntiApprovalAsync(T entity)
        {
            throw new Exception("子类要重写AntiApprovalAsync,请联系管理员");
            ReturnResults<T> result = new ReturnResults<T>();
            await Task.Delay(0); // 模拟异步操作
            return result; // 或者根据实际情况返回值
        }


        public virtual Task<bool> BaseSubmitByNavAsync(T model)
        {
            //子类重写
            throw new Exception("子类要重写BaseSubmitByNavAsync");
            //return null;
        }

        public virtual ValidationResult BaseValidator(T model)
        {
            //子类重写
            throw new Exception("子类要重写BaseValidator");
            //return null;

        }


        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<T>> BaseQueryByAdvancedAsync(bool useLike, object dto)
        {
            throw new Exception("子类要重写BaseDeleteAsync");
        }
        #endregion

        #region 方法

        #endregion

        #region 方法

        #endregion

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            //var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
            //     .IncludesAllFirstLayer()
            //    .WhereCustom(useLike,dto);
            //return await querySqlQueryable.ToListAsync();
            throw new Exception("子类要重写BaseQueryByAdvancedNavAsync");
        }




        public async virtual Task<List<T>> BaseQueryByAdvancedNavWithConditionsAsync(bool useLike, List<string> _queryConditions, Expression<Func<T, bool>> whereLambda, object dto)
        {
            ISugarQueryable<T> querySqlQueryable;
            if (_queryConditions == null || _queryConditions.Count == 0)
            {
                if (typeof(T).GetProperties().ContainsProperty("isdeleted"))
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                    .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                     .WhereIF(whereLambda != null, whereLambda)
                    .Where("isdeleted=@isdeleted", new { isdeleted = 0 });
                }
                else
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                  //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                  .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                  .WhereIF(whereLambda != null, whereLambda)
                  ;
                }

            }
            else
            {
                if (typeof(T).GetProperties().ContainsProperty("isdeleted"))
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereAdv(useLike, _queryConditions, dto)
                               .WhereIF(whereLambda != null, whereLambda)
                                .Where("isdeleted=@isdeleted", new { isdeleted = 0 });
                }
                else
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                        //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                        .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                        .WhereIF(whereLambda != null, whereLambda)
                        .WhereAdv(useLike, _queryConditions, dto);

                }
            }

            return await querySqlQueryable.ToListAsync() as List<T>;
        }


        public async virtual Task<List<T>> BaseQueryByAdvancedNavWithConditionsAsync(bool useLike, List<string> _queryConditions, Expression<Func<T, bool>> whereLambda, object dto, int pageNum, int pageSize)
        {
            ISugarQueryable<T> querySqlQueryable;
            if (_queryConditions == null || _queryConditions.Count == 0)
            {
                if (typeof(T).GetProperties().ContainsProperty("isdeleted"))
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                    .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                     .WhereIF(whereLambda != null, whereLambda)
                    .Where("isdeleted=@isdeleted", new { isdeleted = 0 });
                }
                else
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                  //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                  .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                  .WhereIF(whereLambda != null, whereLambda)
                  ;
                }

            }
            else
            {
                if (typeof(T).GetProperties().ContainsProperty("isdeleted"))
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereAdv(useLike, _queryConditions, dto)
                               .WhereIF(whereLambda != null, whereLambda)
                                .Where("isdeleted=@isdeleted", new { isdeleted = 0 });
                }
                else
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                        //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                        .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                        .WhereIF(whereLambda != null, whereLambda)
                        .WhereAdv(useLike, _queryConditions, dto);

                }
            }

            return await querySqlQueryable.ToPageListAsync(pageNum, pageSize) as List<T>;
        }


        /// <summary>
        /// 查询基础数据缓存数据，不用导航。 根据ISugarQueryable IN NOT的语法拼接
        /// </summary>
        /// <param name="useLike"></param>
        /// <param name="QueryConditionFilter"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ISugarQueryable<T> BaseGetISugarQueryable(bool useLike, QueryFilter QueryConditionFilter, object dto)
        {
            ISugarQueryable<T> querySqlQueryable;
            List<string> queryConditions = new List<string>();
            queryConditions = new List<string>(QueryConditionFilter.QueryFields.Select(t => t.FieldName).ToList());
            Expression<Func<T, bool>> whereLambda = QueryConditionFilter.GetFilterExpression<T>();

            //根据ISugarQueryable IN NOT的语法拼接
            List<string> sqlList = new List<string>();
            string sql = string.Empty;
            string where = string.Empty;
            string select = string.Empty;
            ExpressionToSql expressionToSql = new ExpressionToSql();
            int counter = 0;
            foreach (var item in QueryConditionFilter.QueryFields)
            {
                //如果这个item的字段 在T中是？类型 说明可有可无？则不用加子限制条件？如产品中供应商不一定填写了。
                PropertyInfo propertyInfo = typeof(T).GetProperty(item.FieldName);
                if (item.SubFilter.FilterLimitExpressions.Count > 0 && propertyInfo.PropertyType.Name != "Nullable`1")
                {
                    select = $" EXISTS ( SELECT [{item.SubFilter.QueryTargetType.Name}].{item.FieldName} FROM [{item.SubFilter.QueryTargetType.Name}] WHERE [{typeof(T).Name}].{item.FieldName}= [{item.SubFilter.QueryTargetType.Name}].{item.FieldName}  ";
                    where = expressionToSql.GetSql(item.SubFilter.QueryTargetType, item.SubFilter.GetFilterLimitExpression(item.SubFilter.QueryTargetType));
                    where = $" and ({where})) ";
                    counter++;
                }
                sql = select + where;
                if (sql.IsNotEmptyOrNull() && !sqlList.Contains(sql))
                {
                    sqlList.Add(sql);
                }

            }
            //if (counter > 1)
            //{
            //    throw new Exception("多个子限制条件的情况，请联系管理员处理！" + sql);
            //}

            if (queryConditions == null || queryConditions.Count == 0)
            {
                if (typeof(T).GetProperties().ContainsProperty("isdeleted"))
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                     //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                     // .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                     .WhereIF(whereLambda != null, whereLambda)
                    .Where("isdeleted=@isdeleted", new { isdeleted = 0 });
                }
                else
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                  //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法

                  // 查询基础数据缓存数据，不用导航。
                  //.IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                  .WhereIF(whereLambda != null, whereLambda)
                  ;
                }

            }
            else
            {
                if (typeof(T).GetProperties().ContainsProperty("isdeleted"))
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                                // .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereAdv(useLike, queryConditions, dto)
                                .WhereIF(whereLambda != null, whereLambda)
                                .Where("isdeleted=@isdeleted", new { isdeleted = 0 });
                }
                else
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                   //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                   //.IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                   .WhereIF(whereLambda != null, whereLambda)
                   .WhereAdv(useLike, queryConditions, dto);
                    foreach (var SqlItem in sqlList)
                    {
                        if (!string.IsNullOrEmpty(SqlItem))
                        {
                            //如果有子查询。暂时这样上面SQL拼接处理。
                            querySqlQueryable = querySqlQueryable.Where(SqlItem);
                        }
                    }

                }
            }

            return querySqlQueryable;
        }



        /// <summary>
        /// by watson 删除了-> .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
        /// 因为这个方法只用于基础数据的查询列表。不用引用显示太多内容
        /// 更名为BaseQuerySimpleByAdvancedNavWithConditionsAsync
        /// 老方法用于单据,但是中间过程重复。可以优化重构
        /// 2024,7.23优化：基础资料查询应该是不包含逻辑删除的所有条件都可以查出来。
        /// </summary>
        /// <param name="useLike"></param>
        /// <param name="QueryConditionFilter"></param>
        /// <param name="dto"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async virtual Task<List<T>> BaseQuerySimpleByAdvancedNavWithConditionsAsync(bool useLike, QueryFilter QueryConditionFilter, object dto, int pageNum, int pageSize, bool UseAutoNavQuery = false)
        {
            const string isdeleted = "isdeleted";
            ISugarQueryable<T> querySqlQueryable;
            List<string> queryConditions = new List<string>();
            queryConditions = new List<string>(QueryConditionFilter.QueryFields.Select(t => t.FieldName).ToList());
            //这里基本用于基础资料查询，为了去掉可用启用条件，做特殊处理一下。

            if (QueryConditionFilter.FilterLimitExpressions.Count > 1)
            {

            }

            //这个逻辑是为了 基础资料查询时，可用 启用不生效。其他生效
            if (QueryConditionFilter.FilterLimitExpressions.Count > 0)
            {
                //这个也可以！  其他情况可能还需要调试
                //Expression<Func<T, bool>> expression1 = ExpressionUtils.RemoveConditions(QueryConditionFilter.GetFilterExpression<T>(), "Is_enabled", "Is_available");

                //要移除的是  Is_available == false && Is_enabled == false ,可用启用条件是包含在结果中，不用处理。
                //目标就是要全部查出来，然后再过滤（其他条件，如删除和客户供应商标记）
                Expression<Func<T, bool>> expression = RuinorExpressionUtils.RemoveEqualityConditions(QueryConditionFilter.GetFilterExpression<T>(), "Is_enabled", "Is_available");


                QueryConditionFilter.FilterLimitExpressions.Clear();
                QueryConditionFilter.FilterLimitExpressions.Add(expression);


            }


            Expression<Func<T, bool>> whereLambda = QueryConditionFilter.GetFilterExpression<T>();


            //根据ISugarQueryable IN NOT的语法拼接
            List<string> sqlList = new List<string>();
            string sql = string.Empty;
            string where = string.Empty;
            string select = string.Empty;
            ExpressionToSql expressionToSql = new ExpressionToSql();
            int counter = 0;
            foreach (var item in QueryConditionFilter.QueryFields)
            {
                //如果这个item的字段 在T中是？类型 说明可有可无？则不用加子限制条件？如产品中供应商不一定填写了。
                PropertyInfo propertyInfo = typeof(T).GetProperty(item.FieldName);
                if (propertyInfo == null)
                {
                    continue;
                }
                if (item.SubFilter.FilterLimitExpressions.Count > 0 && propertyInfo.PropertyType.Name != "Nullable`1")
                {
                    // 获取子查询的目标类型（从字段的FK特性或属性类型推断）
                    Type? subFilterTargetType = item.SubFilter.QueryTargetType;
                    if (subFilterTargetType == null && propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        // 如果是可空类型，获取实际类型
                        subFilterTargetType = propertyInfo.PropertyType.GetGenericArguments()[0];
                    }
                    
                    if (subFilterTargetType != null)
                    {
                        // 子查询：检查关联表中是否存在符合条件的记录
                        select = $" EXISTS ( SELECT 1 FROM [{subFilterTargetType.Name}] WHERE  [{typeof(T).Name}].{item.FieldName}= [{subFilterTargetType.Name}].{item.FieldName} ";
                        where = expressionToSql.GetSql(subFilterTargetType, item.SubFilter.GetFilterLimitExpression(subFilterTargetType));
                        // 过滤掉空条件
                        if (!string.IsNullOrWhiteSpace(where))
                        {
                            select = $" {select} AND ({where}) ";
                        }
                        select = $"{select}) ";
                        counter++;
                    }
                }
                sql = select + where;
                if (sql.IsNotEmptyOrNull() && !sqlList.Contains(sql))
                {
                    sqlList.Add(sql);
                }

            }

            if (typeof(T).GetProperties().ContainsProperty(isdeleted))
            {
                querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                       //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                       .WhereAdv(useLike, queryConditions, dto)
                       .WhereIF(whereLambda != null, whereLambda)
                       .WhereIF(dto.ContainsProperty(isdeleted), "isdeleted=@isdeleted", new { isdeleted = 0 });
            }
            else
            {
                querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                       //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                       .WhereIF(whereLambda != null, whereLambda)
                       .WhereAdv(useLike, queryConditions, dto);
            }

            // 先添加所有查询条件，包括子查询
            foreach (var SqlItem in sqlList)
            {
                if (!string.IsNullOrEmpty(SqlItem))
                {
                    //如果有子查询。暂时这样上面SQL拼接处理。
                    querySqlQueryable = querySqlQueryable.Where(SqlItem);
                }
            }

            // 所有查询条件构建完成后，再添加导航属性查询
            // 修复：确保 IncludesAllFirstLayer() 在所有 Where 条件之后调用
            if (UseAutoNavQuery)
            {
                querySqlQueryable = querySqlQueryable.IncludesAllFirstLayer();
                //自动更新导航 只能两层。这里项目中有时会失效，具体看文档
            }

            // 使用try-catch捕获DataReader关闭异常,避免短时间查询导致连接问题11
            int retryCount = 0;
            const int maxRetryCount = 1;
            
            while (retryCount <= maxRetryCount)
            {
                try
                {
                    if (retryCount == 0)
                    {
                        _logger.LogDebug($"执行查询: BaseQuerySimpleByAdvancedNavWithConditionsAsync, 实体类型: {typeof(T).Name}, DTO类型: {dto.GetType().Name}");
                        System.Diagnostics.Debug.WriteLine($"BaseQuerySimpleByAdvancedNavWithConditionsAsync:{dto.GetType().Name}");
                    }
                    else
                    {
                        _logger.LogDebug($"执行重试查询: BaseQuerySimpleByAdvancedNavWithConditionsAsync, 重试次数: {retryCount}, 实体类型: {typeof(T).Name}");
                    }
                    
                    using (var dbClient = _unitOfWorkManage.GetDbClient())
                    {
                        ISugarQueryable<T> query;
                        if (typeof(T).GetProperties().ContainsProperty(isdeleted))
                        {
                            query = dbClient.Queryable<T>()
                                   //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                                   .WhereAdv(useLike, queryConditions, dto)
                                   .WhereIF(whereLambda != null, whereLambda)
                                   .WhereIF(dto.ContainsProperty(isdeleted), "isdeleted=@isdeleted", new { isdeleted = 0 });
                        }
                        else
                        {
                            query = dbClient.Queryable<T>()
                                   //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                                   .WhereIF(whereLambda != null, whereLambda)
                                   .WhereAdv(useLike, queryConditions, dto);
                        }

                        // 先添加所有查询条件，包括子查询
                        foreach (var SqlItem in sqlList)
                        {
                            if (!string.IsNullOrEmpty(SqlItem))
                            {
                                //如果有子查询。暂时这样上面SQL拼接处理。
                                query = query.Where(SqlItem);
                            }
                        }

                        // 所有查询条件构建完成后，再添加导航属性查询
                        // 修复：确保 IncludesAllFirstLayer() 在所有 Where 条件之后调用
                        if (UseAutoNavQuery)
                        {
                            query = query.IncludesAllFirstLayer();
                            //自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                        }
                        
                        return await query.ToPageListAsync(pageNum, pageSize);
                    }
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("FieldCount") || ex.Message.Contains("阅读器关闭"))
                {
                    if (retryCount >= maxRetryCount)
                    {
                        _logger.LogError(ex, $"DataReader已关闭,重试次数已达上限。实体类型: {typeof(T).Name}");
                        throw;
                    }
                    
                    _logger.LogError(ex, $"DataReader已关闭,尝试重新查询。实体类型: {typeof(T).Name}, 重试次数: {retryCount + 1}");
                    // 等待一小段时间后重试,可能是连接池繁忙
                    await Task.Delay(100);
                    retryCount++;
                }
            }
            
            // 理论上不会执行到这里，因为while循环中要么返回要么抛出异常
            return new List<T>();
        }





        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="useLike"></param>
        /// <param name="QueryConditionFilter"></param>
        /// <param name="dto"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="RowAuthFilter">行级数据过滤条件</param>
        /// <returns></returns>
        /// <summary>
        /// 基于高级导航条件的异步查询方法
        /// </summary>
        /// <param name="useLike">是否使用LIKE查询</param>
        /// <param name="QueryConditionFilter">查询条件过滤器</param>
        /// <param name="dto">数据传输对象</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="additionalSqlWhere">额外的SQL条件</param>
        /// <returns>查询结果列表</returns>
        /// <summary>
        /// 使用高级导航条件进行基础查询
        /// </summary>
        /// <param name="useLike">是否使用模糊查询</param>
        /// <param name="QueryConditionFilter">查询条件过滤器</param>
        /// <param name="dto">数据传输对象</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="additionalSqlWhere">额外的SQL条件</param>
        /// <returns>查询结果列表</returns>
        public async virtual Task<List<T>> BaseQueryByAdvancedNavWithConditionsAsync(bool useLike,
            QueryFilter QueryConditionFilter, object dto, int pageNum, int pageSize, string additionalSqlWhere = "")
        {
            // 生成查询缓存键
            string cacheKey = GenerateQueryCacheKey(typeof(T).Name, QueryConditionFilter, dto, pageNum, pageSize, additionalSqlWhere);

            // 尝试从缓存获取数据
            var cachedResult = await GetFromQueryCacheAsync<List<T>>(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            if (QueryConditionFilter == null)
            {
                throw new ArgumentNullException(nameof(QueryConditionFilter), "查询条件过滤器不能为空");
            }

            // 获取查询条件和Lambda表达式
            List<string> queryConditions = QueryConditionFilter.GetQueryConditions();
            Expression<Func<T, bool>> whereLambda = QueryConditionFilter.GetFilterExpression<T>();

            // 构建子查询条件
            List<string> subQueryConditions = BuildSubQueryConditions(QueryConditionFilter);

            // 初始化基础查询
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>();

            // 应用查询条件 - 统一处理逻辑，避免重复代码
            if (queryConditions != null && queryConditions.Count > 0)
            {
                querySqlQueryable = querySqlQueryable.WhereAdv(useLike, queryConditions, dto);
            }

            // 应用子查询条件 - 无论是否有主查询条件，都应该应用子查询
            foreach (var subQuery in subQueryConditions)
            {
                if (!string.IsNullOrEmpty(subQuery))
                {
                    querySqlQueryable = querySqlQueryable.Where(subQuery);
                }
            }

            // 应用Lambda表达式条件
            if (whereLambda != null)
            {
                querySqlQueryable = querySqlQueryable.Where(whereLambda);
            }

            // 应用isdeleted条件(如果存在该属性)
            if (typeof(T).GetProperties().ContainsProperty("isdeleted"))
            {
                querySqlQueryable = querySqlQueryable.Where("isdeleted=@isdeleted", new { isdeleted = 0 });
            }

            // ✅ 修复：所有查询条件（包括行级权限）构建完成后，再调用 IncludesAllFirstLayer()
            // 应用额外的SQL条件和行级权限过滤 - 必须在 IncludesAllFirstLayer 之前
            if (!string.IsNullOrEmpty(additionalSqlWhere))
            {
                // 使用 BuildFilterClause 获取过滤条件字符串，然后统一调用 Where
                string filterClause = _rowLevelAuthFilter.BuildFilterClause<T>(additionalSqlWhere);
                if (!string.IsNullOrEmpty(filterClause))
                {
                    querySqlQueryable = querySqlQueryable.Where(filterClause);
                }
            }

            // 自动更新导航关系(最多两层)，但对于基础表可以跳过
            if (!_tableSchemaManager.GetAllTableNames().Contains(typeof(T).Name) || typeof(T).Name == typeof(tb_Prod).Name)
            {
                querySqlQueryable = querySqlQueryable.IncludesAllFirstLayer();
            }

            // 执行查询
            var result = await querySqlQueryable.ToPageListAsync(pageNum, pageSize) as List<T>;

            // 将结果存入缓存
            await PutToQueryCacheAsync(cacheKey, result);

            return result;
        }



        /// <summary>
        /// 构建子查询条件
        /// </summary>
        /// <param name="queryFilter">查询过滤器</param>
        /// <returns>子查询条件列表</returns>
        private List<string> BuildSubQueryConditions(QueryFilter queryFilter)
        {
            List<string> sqlList = new List<string>();
            ExpressionToSql expressionToSql = new ExpressionToSql();

            foreach (var item in queryFilter.QueryFields)
            {
                // 检查字段是否为可空类型，如果是则跳过子查询
                PropertyInfo propertyInfo = typeof(T).GetProperty(item.FieldName);
                if (item.SubFilter?.FilterLimitExpressions?.Count > 0 &&
                    propertyInfo != null &&
                    propertyInfo.PropertyType.Name != "Nullable`1")
                {
                    try
                    {
                        // 构建EXISTS子查询
                        string tableName = typeof(T).Name;
                        string targetTableName = item.SubFilter.QueryTargetType.Name;
                        string fieldName = item.FieldName;
                        string targetFieldName = item.FieldName;
                        if (propertyInfo.PropertyType.Name == "Int64" && item.FriendlyFieldValueFromSource != null)
                        {
                            targetFieldName = item.FriendlyFieldValueFromSource;
                        }
                        if (propertyInfo.PropertyType.Name == "String" && item.FriendlyFieldNameFromSource != null)
                        {
                            targetFieldName = item.FriendlyFieldNameFromSource;
                        }

                        // 构建基础EXISTS子查询
                        StringBuilder subQueryBuilder = new StringBuilder();
                        subQueryBuilder.Append($"EXISTS (SELECT 1 FROM [{targetTableName}] ");
                        subQueryBuilder.Append($"WHERE [{tableName}].[{fieldName}] = [{targetTableName}].[{targetFieldName}] ");

                        // 获取子查询的过滤条件
                        var filterExpression = item.SubFilter.GetFilterLimitExpression(item.SubFilter.QueryTargetType);
                        if (filterExpression != null)
                        {
                            string whereClause = expressionToSql.GetSql(item.SubFilter.QueryTargetType, filterExpression);
                            // 清理并验证whereClause
                            if (!string.IsNullOrEmpty(whereClause))
                            {
                                whereClause = whereClause.Trim();
                                // 移除无效的条件如 'AND ( isdeleted = '0' AND '1' )'
                                if (whereClause != "'1'" && !whereClause.Contains("'1'"))
                                {
                                    // 确保whereClause不以AND或OR开头
                                    if (whereClause.StartsWith("AND ", StringComparison.OrdinalIgnoreCase) ||
                                        whereClause.StartsWith("OR ", StringComparison.OrdinalIgnoreCase))
                                    {
                                        whereClause = whereClause.Substring(4);
                                    }
                                    // 确保whereClause格式正确，包含有效的比较操作符
                                    if (Regex.IsMatch(whereClause, @"[=<>!]") || whereClause.Contains("LIKE") || whereClause.Contains("IN"))
                                    {
                                        subQueryBuilder.Append($"AND ({whereClause}) ");
                                    }
                                }
                            }
                        }

                        // 关闭子查询
                        subQueryBuilder.Append(")");
                        string subQuery = subQueryBuilder.ToString();

                        // 检查子查询是否有效且不重复
                        if (!string.IsNullOrEmpty(subQuery) && !sqlList.Contains(subQuery) && subQuery.Contains("WHERE"))
                        {
                            sqlList.Add(subQuery);
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录子查询构建错误但不中断流程
                        _logger.LogError(ex, $"构建子查询条件时出错: 字段名={item.FieldName}, 目标类型={item.SubFilter?.QueryTargetType?.Name}");
                    }
                }
            }

            return sqlList;
        }







        /// <summary>
        /// where的条件查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async virtual Task<List<T>> BaseQueryByWhereAsync(Expression<Func<T, bool>> exp)
        {
            // var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>();
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp);
            return await querySqlQueryable.ToListAsync();
            // throw new Exception("子类要重写BaseQueryByAdvancedNavAsync");
        }






        /// <summary>
        /// where的条件查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual List<T> BaseQueryByWhereTop(Expression<Func<T, bool>> exp, int top)
        {
            // var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>();
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                .IncludesAllFirstLayer()//自动导航
                .Take(top).Where(exp);
            return querySqlQueryable.ToList();
        }

        #region 查询缓存相关方法

        /// <summary>
        /// 生成查询缓存键
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="queryFilter">查询过滤器</param>
        /// <param name="dto">数据传输对象</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="additionalSqlWhere">额外SQL条件</param>
        /// <returns>缓存键</returns>
        private string GenerateQueryCacheKey(string tableName, QueryFilter queryFilter, object dto, int pageNum, int pageSize, string additionalSqlWhere)
        {
            try
            {
                // 生成查询条件的哈希值作为缓存键的一部分
                var queryConditionsHash = GetQueryConditionsHash(queryFilter, dto, additionalSqlWhere);

                // 构建缓存键
                var cacheKey = $"Query_{tableName}_Page{pageNum}_Size{pageSize}_{queryConditionsHash}";

                // 限制缓存键长度
                if (cacheKey.Length > 200)
                {
                    // 如果键太长，使用哈希值缩短
                    using (var sha256 = System.Security.Cryptography.SHA256.Create())
                    {
                        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(cacheKey));
                        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                        cacheKey = $"Query_{tableName}_{hashString.Substring(0, 16)}_Page{pageNum}_Size{pageSize}";
                    }
                }

                return cacheKey;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "生成查询缓存键时发生错误，使用默认键");
                return $"Query_{tableName}_Page{pageNum}_Size{pageSize}_{DateTime.Now.Ticks}";
            }
        }

        /// <summary>
        /// 获取查询条件的哈希值
        /// </summary>
        /// <param name="queryFilter">查询过滤器</param>
        /// <param name="dto">数据传输对象</param>
        /// <param name="additionalSqlWhere">额外SQL条件</param>
        /// <returns>查询条件哈希值</returns>
        private string GetQueryConditionsHash(QueryFilter queryFilter, object dto, string additionalSqlWhere)
        {
            var conditionsString = new StringBuilder();

            // 添加查询过滤器条件
            if (queryFilter != null && queryFilter.QueryFields != null)
            {
                foreach (var field in queryFilter.QueryFields.OrderBy(f => f.FieldName))
                {
                    conditionsString.Append($"{field.FieldName}:{field.Value},");
                }
            }

            // 添加DTO属性值
            if (dto != null)
            {
                var properties = dto.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in properties.OrderBy(p => p.Name))
                {
                    // 跳过索引器属性
                    if (prop.GetIndexParameters().Length > 0)
                    {
                        continue;
                    }

                    // 检查是否有公共getter
                    if (!prop.CanRead)
                    {
                        continue;
                    }

                    try
                    {
                        var value = prop.GetValue(dto);
                        if (value != null)
                        {
                            conditionsString.Append($"{prop.Name}:{value},");
                        }
                    }
                    catch (Exception ex)
                    {
                        // 单个属性获取失败不影响整个方法
                        _logger.LogDebug(ex, $"获取属性值失败: {prop.Name}");
                    }
                }
            }

            // 添加额外SQL条件
            if (!string.IsNullOrEmpty(additionalSqlWhere))
            {
                conditionsString.Append($"Additional:{additionalSqlWhere}");
            }

            // 计算哈希值
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(conditionsString.ToString()));
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                return hashString.Substring(0, 16); // 取前16个字符
            }
        }

        /// <summary>
        /// 从查询缓存异步获取数据
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="cacheKey">缓存键</param>
        /// <returns>缓存中的数据，如果不存在则返回null</returns>
        private async Task<TResult> GetFromQueryCacheAsync<TResult>(string cacheKey)
        {
            try
            {
                var cachedData = _queryCacheManager.Get(cacheKey);
                if (cachedData != null)
                {
                    // 检查类型兼容性并转换
                    if (cachedData is TResult result)
                    {
                        return result;
                    }
                    else if (cachedData is Newtonsoft.Json.Linq.JArray jArray)
                    {
                        // 将JArray转换为指定类型
                        return jArray.ToObject<TResult>();
                    }
                    else
                    {
                        // 尝试使用JSON序列化/反序列化进行类型转换
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(cachedData);
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<TResult>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "从查询缓存获取数据时发生错误");
            }

            return default(TResult);
        }

        /// <summary>
        /// 将数据异步存入查询缓存
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="data">要缓存的数据</param>
        private async Task PutToQueryCacheAsync<TResult>(string cacheKey, TResult data)
        {
            try
            {
                _queryCacheManager.Put(cacheKey, data);
                _logger.LogDebug($"查询结果已存入缓存，键: {cacheKey}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "将数据存入查询缓存时发生错误");
            }
        }

        /// <summary>
        /// 清除指定表的查询缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        public void ClearQueryCache(string tableName)
        {
            try
            {
                // 通过表名清除相关缓存（这里可以实现更精确的清除逻辑）
                _logger.LogDebug($"清除表 {tableName} 的查询缓存");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "清除查询缓存时发生错误");
            }
        }

        #endregion


        #region 查询参数设置





        /// <summary>
        /// 获取查询生成的字段,传到公共查询UI生成，有时个别字段也要限制性条件，如下拉数据源的限制，这里也可以传入
        /// 暂时不处理？
        /// </summary>
        /// <returns></returns>
        public virtual List<KeyValuePair<string, Expression<Func<T, bool>>>> GetQueryConditionsListWithlimited(Expression<Func<T, bool>> Conditions)
        {
            List<KeyValuePair<string, Expression<Func<T, bool>>>> keyValues = new List<KeyValuePair<string, Expression<Func<T, bool>>>>();

            List<Expression<Func<T, object>>> QueryConditions = new List<Expression<Func<T, object>>>();

            //Expression<Func<T, bool>> expCondition,

            /*
             //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer == true)
                             .And(t => t.isdeleted == false)
                              .And(t => t.Is_available == true)
                               .And(t => t.Is_enabled == true)
                               .AndIF(AppContext.SysConfig.SaleBizLimited && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID) 
                            .ToExpression(); 
             */

            List<string> qlist = Common.Helper.RuinorExpressionHelper.ExpressionListToStringList(QueryConditions);
            return keyValues;
        }




        #endregion

    }
}
