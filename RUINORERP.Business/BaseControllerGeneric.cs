using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices;
using RUINORERP.Services;
using RUINORERP.Repository.Base;
using RUINORERP.IRepository.Base;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using System.Linq.Expressions;
using RUINORERP.Extensions.Middlewares;
using SqlSugar;
using System.ComponentModel;
using RUINORERP.Business.Processor;
using RUINORERP.Common.Helper;
using System.Reflection;
//using Magicodes.ExporterAndImporter.Core.Extension;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Expression = System.Linq.Expressions.Expression;
using RUINORERP.Common;
using RUINORERP.Global.CustomAttribute;
using System.Linq.Dynamic.Core;
using SharpYaml.Tokens;
using RUINORERP.Business.CommService;
using System.Web.UI.WebControls;
using AutoMapper;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.FMService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 单表基础资料操作
    /// </summary>
    public class BaseController<T> : BaseController where T : class
    {
        public ApplicationContext _appContext;

        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public IUnitOfWorkManage _unitOfWorkManage;
        public ILogger<BaseController<T>> _logger;
        public IMapper mapper { get; set; }
        public string BizTypeText { get; set; }
        public int BizTypeInt { get; set; }
        public BaseController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext = null)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _appContext = appContext;
            BizTypeMapper Bizmapper = new BizTypeMapper();
            BizType bizType = Bizmapper.GetBizType(typeof(T).Name);
            BizTypeText = bizType.ToString();
            BizTypeInt = (int)bizType;
            // mapper = AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
            mapper = appContext.GetRequiredService<IMapper>();
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
            var statusType = FMPaymentStatusHelper.GetStatusType(baseEntity);
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
            if (!FMPaymentStatusHelper.CanSubmit(statusEnum))
            {
                result.ErrorMsg = $"单据当前状态为【{statusEnum}】，无法提交";
                return result;
            }

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

        #endregion



        /// <summary>
        /// 查询指定字段的值是否存在
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool ExistFieldValue(Expression<Func<T, bool>> exp)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp).Any();
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
            return _unitOfWorkManage.GetDbClient().Queryable<T>().Where(whereExp).First();
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
            return await _unitOfWorkManage.GetDbClient().Queryable<T>().Where(whereExp).FirstAsync();
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
        public virtual bool IsExist(Expression<Func<T, bool>> whereLambda = null)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>().WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda).Any();
        }





        ///// <summary>
        ///// 本为私有修改为公有，暴露出来方便使用
        ///// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        //public IBaseServices<T> _IBaseServices { get; set; }


        //public BaseController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,IBaseServices<T> iBaseServices)
        //{
        //    _logger = logger;
        //    _unitOfWorkManage = unitOfWorkManage;
        //    _IBaseServices = iBaseServices;
        //}

        //public BaseController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage, IBaseServices<T> iBaseServices)
        //{
        //    _logger = logger;
        //    _unitOfWorkManage = unitOfWorkManage;
        //    _IBaseServices = iBaseServices;
        //    IBaseRepository<T> baseDal = new BaseRepository<T>();
        //    _IBaseServices = new RUINORERP.Services.BASE.BaseServices<T>(baseDal);
        //}



        #region 方法



        #endregion

        #region 公共虚拟方法

        //public virtual Task<object> BaseUniqueQueryByIdAsync(object id)
        //{
        //    //子类重写
        //    throw new Exception("子类要重写BaseQueryByIdAsync");
        //}

        //public async override Task<object> BaseUniqueQueryByIdAsync(object id)
        //{
        //    object entity = await _tb_BOM_SServices.QueryByIdAsync(id) as object;
        //    return entity;
        //}

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
            throw new Exception("子类要重写SaveOrUpdate");
            //return null;
        }

        public virtual ISugarQueryable<T> BaseGetQueryableAsync()
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>();

            //子类重写
            //throw new Exception("子类要重写BaseGetQueryableAsync");
            //var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
            //     .IncludesAllFirstLayer()
            //    .Where(useLike,dto);
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
            throw new Exception("子类要重写BaseDeleteAsync");
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
            //    .Where(useLike,dto);
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

                /*
                bool isContainIsDeleted = QueryConditionFilter.FilterLimitExpressions[0].Body.ExpContainsCondition(isdeleted, false);
                if (isContainIsDeleted)
                {
                    // 创建一个新的参数表达式
                    var parameter = Expression.Parameter(typeof(T), "t");

                    // 创建一个新的二元表达式，只包含 isdeleted == False 条件
                    var newBody = Expression.MakeBinary(
                        ExpressionType.Equal,
                        Expression.Property(parameter, isdeleted),
                        Expression.Constant(false)
                    );

                    // 创建一个新的 Lambda 表达式
                    var newLambda = Expression.Lambda<Func<T, bool>>(
                        newBody,
                        parameter
                    );

                    // 将新的 Lambda 表达式添加到 FilterLimitExpressions 列表中
                    QueryConditionFilter.FilterLimitExpressions.Clear();//清除之前的,
                    QueryConditionFilter.FilterLimitExpressions.Add(newLambda);
                }
                */
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
                    select = $" EXISTS ( SELECT [{item.SubFilter.QueryTargetType.Name}].{item.FieldName} FROM [{item.SubFilter.QueryTargetType.Name}] WHERE [{typeof(T).Name}].{item.FieldName}= [{item.SubFilter.QueryTargetType.Name}].{item.FieldName}  ";
                    where = expressionToSql.GetSql(item.SubFilter.QueryTargetType, item.SubFilter.GetFilterLimitExpression(item.SubFilter.QueryTargetType));
                    where = $" and ({where})) ";
                    counter++;
                }
                sql = select + where;
                if (!sql.IsNotEmptyOrNull() && !sqlList.Contains(sql))
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
                       .Where("isdeleted=@isdeleted", new { isdeleted = 0 });
                if (UseAutoNavQuery)
                {
                    querySqlQueryable = querySqlQueryable.IncludesAllFirstLayer();
                    //自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                }



            }
            else
            {

                querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
           //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法

           .WhereIF(whereLambda != null, whereLambda)
           .WhereAdv(useLike, queryConditions, dto);
            }
            if (UseAutoNavQuery)
            {
                querySqlQueryable = querySqlQueryable.IncludesAllFirstLayer();
                //自动更新导航 只能两层。这里项目中有时会失效，具体看文档
            }
            foreach (var SqlItem in sqlList)
            {
                if (!string.IsNullOrEmpty(SqlItem))
                {
                    //如果有子查询。暂时这样上面SQL拼接处理。
                    querySqlQueryable = querySqlQueryable.Where(SqlItem);
                }
            }
            return await querySqlQueryable.ToPageListAsync(pageNum, pageSize);
        }






        public async virtual Task<List<T>> BaseQueryByAdvancedNavWithConditionsAsync(bool useLike, QueryFilter QueryConditionFilter, object dto, int pageNum, int pageSize)
        {
            ISugarQueryable<T> querySqlQueryable;

            List<string> queryConditions = QueryConditionFilter.GetQueryConditions();

            Expression<Func<T, bool>> whereLambda = QueryConditionFilter.GetFilterExpression<T>();


            //  .Where(useLike, queryConditions, dto);
            var sb = GetWhereCondition(true, queryConditions, dto, typeof(T));


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
                if (!sql.IsNotEmptyOrNull() && !sqlList.Contains(sql) && sql.ToString().Length > 0)
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
                                .WhereAdv(useLike, queryConditions, dto)
                                .WhereIF(whereLambda != null, whereLambda)
                                .Where("isdeleted=@isdeleted", new { isdeleted = 0 });
                }
                else
                {
                    querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>()
                   //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                   .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
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

            return await querySqlQueryable.ToPageListAsync(pageNum, pageSize) as List<T>;
        }



        #region 2024-7-23再次优化 ,因为无法调试，所以将拼接去掉T单独出来 测试使用

        public static StringBuilder GetWhereCondition(bool useLike, List<string> _queryConditions, object whereObj, Type colType)
        {
            StringBuilder sb = new StringBuilder();
            if (whereObj == null)
            {
                return sb;
            }
            List<string> queryConditions = new List<string>();
            foreach (var item in _queryConditions)
            {

                var conValue = whereObj.GetPropertyValue(item);
                if (conValue == null || conValue.ToString() != "-1")
                {
                    queryConditions.Add(item);
                }
            }

            //这里的思路是将各种情况的where集合分类处理，最后拼接，如果有条件限制，则集合中过滤掉


            var whereObjType = whereObj.GetType();
            Dictionary<string, object> whereDic = new Dictionary<string, object>();      //装载where条件
            Dictionary<string, List<int>> inDic = new Dictionary<string, List<int>>();       //装载in条件
            Dictionary<string, object> dicTimeRange = new Dictionary<string, object>();  //装载时间区间类型的条件
            Dictionary<string, object> dicLike = new Dictionary<string, object>();//装载like类型的条件
            var expSb = new StringBuilder();//条件是拼接的所以声明在前面
            #region 先取扩展特性的字段 自定义的标记解析
            List<AdvExtQueryAttribute> tempAdvExtList = new List<AdvExtQueryAttribute>();
            foreach (PropertyInfo field in whereObj.GetType().GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    if (attr is AdvExtQueryAttribute)
                    {
                        var advLikeAttr = attr as AdvExtQueryAttribute;
                        tempAdvExtList.Add(advLikeAttr);
                    }
                }
            }

            AdvQueryAttribute entityAttr;



            foreach (var property in whereObjType.GetProperties())
            {
                foreach (Attribute attr in property.GetCustomAttributes(true))
                {
                    entityAttr = attr as AdvQueryAttribute;
                    if (null != entityAttr)
                    {
                        if (entityAttr.ColDesc.Trim().Length > 0)
                        {
                            var curName = property.Name;
                            if (property.PropertyType.Name.Equals("List`1"))  //集合
                            {
                                var curValue = property.GetValue(whereObj, null);
                                inDic.Add(curName, (List<int>)curValue);
                            }
                            else
                            {
                                List<AdvExtQueryAttribute> extlist = tempAdvExtList.Where(w => w.RelatedFields == curName).ToList();
                                if (extlist.Count > 0)
                                {
                                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        colType = Nullable.GetUnderlyingType(property.PropertyType);
                                    }
                                    else
                                    {
                                        colType = property.PropertyType;
                                    }

                                    switch (extlist[0].ProcessType)
                                    {
                                        case RUINORERP.Global.AdvQueryProcessType.defaultSelect:

                                            var curValue = property.GetValue(whereObj, null);
                                            if (curValue == null) continue;   //排除参数值为null的查询条件
                                            if (string.IsNullOrEmpty(curValue.ToString())) continue;

                                            //重点代码 这之前为int，实际下拉基本是long
                                            long selectID = 0;
                                            if (long.TryParse(curValue.ToString(), out selectID))
                                            {
                                                if (selectID != -1 && selectID != 0)
                                                {
                                                    whereDic.Add(curName, curValue);
                                                }
                                            }

                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.datetimeRange:
                                            if (whereObj.GetPropertyValue(extlist[0].ColName) != null)
                                            {
                                                // sb.Append(extlist[0].RelatedFields).Append($" >= @{whereObj.GetPropertyValue(extlist[0].ColName)}");
                                                DateTime time1 = Convert.ToDateTime(whereObj.GetPropertyValue(extlist[0].ColName).ToString());
                                                //var v_str1 = string.Format("{0}", time1.ToString("yyyy-MM-dd HH:mm:ss"));
                                                var v_str1 = string.Format("{0}", time1.ToString("yyyy-MM-dd"));
                                                dicTimeRange.Add(extlist[0].ColName, v_str1);
                                            }
                                            if (whereObj.GetPropertyValue(extlist[1].ColName) != null)
                                            {
                                                DateTime time2 = Convert.ToDateTime(whereObj.GetPropertyValue(extlist[1].ColName).ToString());
                                                //                                                var v_str2 = string.Format("{0}", time2.ToString("yyyy-MM-dd HH:mm:ss"));
                                                var v_str2 = string.Format("{0}", time2.ToString("yyyy-MM-dd"));
                                                dicTimeRange.Add(extlist[1].ColName, v_str2 + " 23:59:59");
                                            }
                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.stringLike:
                                            //扩展属性 直接like
                                            var curlikeValue = whereObj.GetPropertyValue(extlist[0].RelatedFields);
                                            if (curlikeValue == null) continue;   //排除参数值为null的查询条件
                                            if (string.IsNullOrEmpty(curlikeValue.ToString())) continue;
                                            dicLike.Add(extlist[0].RelatedFields, curlikeValue);
                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.useYesOrNoToAll:
                                            var UseboolObj = whereObj.GetPropertyValue(extlist[0].ColName);
                                            bool useBool = UseboolObj.ToBool();
                                            if (useBool)
                                            {
                                                var curBoolValue = property.GetValue(whereObj, null);
                                                if (curBoolValue == null) continue;   //排除参数值为null的查询条件
                                                if (string.IsNullOrEmpty(curBoolValue.ToString())) continue;
                                                whereDic.Add(curName, curBoolValue);
                                            }
                                            break;
                                        default:
                                            break;
                                    }


                                }
                                else
                                {
                                    var curValue = property.GetValue(whereObj, null);
                                    if (curValue == null) continue;   //排除参数值为null的查询条件
                                    if (string.IsNullOrEmpty(curValue.ToString())) continue;
                                    whereDic.Add(curName, curValue);
                                }

                            }
                        }

                    }
                }

            }

            #endregion

            #region  LIKE 处理
            StringBuilder sblike = new StringBuilder();
            if (dicLike.Count > 0)
            {
                // 动态拼接

                List<string> para = new List<string>();
                for (int i = 0; i < dicLike.Keys.Count; i++)
                {

                    var keys = dicLike.Keys.ToArray();
                    var values = dicLike.Values.ToArray();
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (queryConditions.Count > 0 && !queryConditions.Where(c => c == keys[i]).Any())
                    {
                        continue;
                    }
                    //自定义查询拼接
                    para.Add(keys[i]);
                    para.Add("like");
                    para.Add("{string}:%" + values[i].ToString() + "%");
                    para.Add("&&");
                }
                if (para.Count > 1 && para.Contains("&&"))
                {
                    para.RemoveAt(para.Count - 1);
                }

                var whereFunc = ObjectFuncModel.Create("Format", para.ToArray());



            }
            #endregion

            #region  时间等区间
            StringBuilder sbr = new StringBuilder();
            if (dicTimeRange.Count > 0)
            {
                // 动态拼接
                for (int i = 0; i < dicTimeRange.Keys.Count; i++)
                {
                    var keys = dicTimeRange.Keys.ToArray();
                    var values = dicTimeRange.Values.ToArray();
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (useLike)
                    {
                        if (queryConditions.Count > 0 && !queryConditions.Where(c => keys[i].Contains(c)).Any())
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (queryConditions.Count > 0 && !queryConditions.Where(c => c == keys[i]).Any())
                        {
                            continue;
                        }
                    }

                    //自定义查询拼接
                    if (keys[i].Contains("_Start"))
                    {
                        // $"{property.Name} = Convert.ToDateTime(\"{value}\") ";
                        sbr.Append($"{tempAdvExtList.FindLast(f => f.ColName == keys[i]).RelatedFields} >=\"{values[i]}\" ").Append(" and ");
                        continue;
                    }

                    if (keys[i].Contains("_End"))
                    {
                        sbr.Append($"{tempAdvExtList.FindLast(f => f.ColName == keys[i]).RelatedFields} <=\"{values[i]}\" ").Append(" and ");
                        continue;
                    }
                    //sbr.Append(fieldNames[i]).Append($" == @{i}").Append(" && ");
                }
                var lambdaStr = sbr.ToString();
                if (lambdaStr.Trim().Length > 0 && lambdaStr.Contains("and"))
                {
                    lambdaStr = lambdaStr.Substring(0, lambdaStr.Length - " and ".Length);
                }
                // 构建表达式
                // var Expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, lambdaStr, dicTimeRange.Values);
                //sugarQueryableWhere = sugarQueryableWhere.Where(Expression);
            }
            #endregion

            #region  in 类型条件处理
            foreach (var item in inDic)
            {
                var key = item.Key;
                var value = item.Value;
                // 如果有条件，并且字段不存在给定的条件中，则忽略
                if (queryConditions.Count > 0 && !queryConditions.Where(c => c == key).Any())
                {
                    continue;
                }

                //转换in条件表达式树
                // var e2 = DynamicExpressionParser.ParseLambda<T, object>(new ParsingConfig(), true, expSb.ToString(), whereObj);

                //https://www.coder.work/article/7717381
                //https://www.cnblogs.com/myzony/p/9143692.html  
                // 构建表达式
                // DynamicExpressionParser.ParseLambda<TEntity, bool>(new ParsingConfig(), false, lambdaStr, parameters.Values.ToArray());


                //sugarQueryableWhere = sugarQueryableWhere.In(e2, value);
            }

            #endregion

            #region  大部分的 where处理
            foreach (var property in colType.GetProperties())
            {
                foreach (var item in whereDic)
                {
                    var key = item.Key;
                    var value = item.Value;
                    if (property.Name != key) continue;
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (queryConditions.Count > 0 && !queryConditions.Where(c => c == key).Any())
                    {
                        continue;
                    }
                    if (value.ToString() == "0001-01-01 00:00:00")
                    {
                        continue;
                    }
                    //如果是时间要特殊处理,下拉值也在whereDic中
                    if (item.Value.GetType().ToString().ToLower().Contains("time"))
                    {
                        if (item.Value.ObjToString().ToDateTime() < System.DateTime.Now.AddYears(-50))
                        {
                            continue;
                        }
                        //只到日期
                        value = string.Format("{0}", item.Value.ObjToString().ToDateTime().ToString("yyyy-MM-dd"));
                        #region 日期处理  当一个时间条件时 会限制到具体时间，思路是将他变为一个区间

                        //DateTime startDate = Convert.ToDateTime(item.Value.ObjToString().ToDateTime().ToString("yyyy-MM-dd"));
                        //DateTime endDate = Convert.ToDateTime(item.Value.ObjToString().ToDateTime().ToString("yyyy-MM-dd"));

                        DateTime startDate = Convert.ToDateTime(value);
                        DateTime endDate = Convert.ToDateTime(value);

                        var formattedEndDate = endDate.Date.AddDays(1);
                        expSb.Append($"{property.Name} >=\"{startDate}\" ").Append(" and ");
                        expSb.Append($"{property.Name} <=\"{formattedEndDate}\" ").Append(" and ");
                        #endregion
                    }
                    else
                    {
                        expSb.Append(SqlSugarHelper.ProcessExp(property, value));          //拼接where条件
                        expSb.Append(" and ");
                    }
                }
            }
            expSb.Append(sb.ToString());
            if (expSb.Length != 0)                                     //转换where条件表达式树
            {
                var exp = expSb.ToString().Remove(expSb.Length - 4, 4);
                System.Console.WriteLine(exp);
                //https://www.coder.work/article/7717381  看这里
                // var e = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, exp, whereObj);
                //ew ParsingConfig(), true, "City = @0", "London"
                //sugarQueryableWhere = sugarQueryable.Where(e);
            }

            #endregion

            return sb;
        }





        #endregion





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
        public virtual List<T> BaseQueryByWhere(Expression<Func<T, bool>> exp)
        {
            //  var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>();
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp);
            return querySqlQueryable.ToList();
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
