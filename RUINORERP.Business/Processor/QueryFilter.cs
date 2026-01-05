using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 查询过滤器 很重要的类。业务查询条件  查询字段 子查询都 用这个传入
    /// 2024-6-14 添加一个显示的过滤字段集合
    /// </summary>
    [Serializable]
    public class QueryFilter
    {
        public ApplicationContext _appContext;

        public QueryFilter(ApplicationContext appContext = null)
        {
            _appContext = appContext;
        }

        // 重写FilterLimitExpressions属性以自动处理闭包变量
        private List<LambdaExpression> _filterLimitExpressions = new List<LambdaExpression>();
        public List<LambdaExpression> FilterLimitExpressions 
        { 
            get { return _filterLimitExpressions; }
            set { _filterLimitExpressions = value ?? new List<LambdaExpression>(); }
        }

        /// <summary>
        /// 为了方便查询框架调用，转换一下格式
        /// 并且支持多条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<Expression<Func<T, bool>>> GetFilterExpressions<T>()
        {
            List<Expression<Func<T, bool>>> LimitQueryConditions = new List<Expression<Func<T, bool>>>();
            ExpConverter expConverter = new ExpConverter();
            foreach (var _FilterLimitExpression in FilterLimitExpressions)
            {
                Expression<Func<T, bool>> LimitQueryCondition;
                if (_FilterLimitExpression != null)
                {
                    // 处理闭包变量
                    var processedExpression = ProcessClosureVariables<T>((Expression<Func<T, bool>>)_FilterLimitExpression);
                    var whereExp = expConverter.ConvertToFuncByClassName(typeof(T), processedExpression);
                    LimitQueryCondition = whereExp as Expression<Func<T, bool>>;
                }
                else
                {
                    LimitQueryCondition = c => true;// queryFilter.FieldLimitCondition;
                }
                LimitQueryConditions.Add(LimitQueryCondition);
            }
            return LimitQueryConditions;
        }


        public List<string> GetQueryConditions()
        {
            HashSet<string> queryConditions = new HashSet<string>();

            foreach (var item in QueryFields)
            {
                //如果有代替项就用代替项
                string conditionField = string.IsNullOrEmpty(item.FriendlyFieldNameFormBiz) ? item.FieldName : item.FriendlyFieldNameFormBiz;
                AppendIfNotContains(queryConditions, conditionField);
            }

            return queryConditions.ToList();
        }

        private void AppendIfNotContains(HashSet<string> collection, string item)
        {
            if (!collection.Contains(item))
            {
                collection.Add(item);
            }
        }




        //合并的时候要注意把参数统一 ,这里还是没有统一到。暂时没有处理。可以使用
        public LambdaExpression GetFilterLimitExpression(Type type)
        {
            LambdaExpression lambda1 = null;
            if (FilterLimitExpressions.Count > 0)
            {
                lambda1 = FilterLimitExpressions[0];
            }
            var parameter = Expression.Parameter(type, "t");
            if (FilterLimitExpressions.Count > 1)
            {
                for (int i = 1; i < FilterLimitExpressions.Count; i++)
                {
                    lambda1 = Expression.Lambda(Expression.AndAlso(lambda1.Body, FilterLimitExpressions[i].Body), Expression.Parameter(type, "t"));
                }
            }
            var visitor = new ParameterReplacementVisitor(parameter);
            var parameterReplacedExp = (LambdaExpression)visitor.Visit(lambda1);
            //转换为TODO
            //var lambdaNew = (Func<T, bool>)lambda1.Compile();
            return lambda1;
            //return CombineLambdaList(FilterLimitExpressions);
        }



        /// <summary>
        /// 合并。有问题！！！！
        /// </summary>
        /// <param name="lambdaList"></param>
        /// <returns></returns>
        private static LambdaExpression CombineLambdaList(List<LambdaExpression> lambdaList)
        {
            ParameterExpression parameter = lambdaList[0].Parameters[0];
            Expression body = lambdaList[0].Body;

            foreach (var lambda in lambdaList.Skip(1))
            {
                body = Expression.Invoke(lambda, parameter);
            }

            return Expression.Lambda(body, parameter);
        }

        /*

        public static LambdaExpression ConvertToSingleLambdaExpression(List<LambdaExpression> lambdaExpressions)
        {
            if (lambdaExpressions == null || lambdaExpressions.Count == 0)
            {
                throw new ArgumentException("List of lambda expressions cannot be null or empty.");
            }

            // 创建一个新的 LambdaExpression 对象
            LambdaExpression result = null;

            // 遍历列表中的每个 LambdaExpression
            foreach (var lambda in lambdaExpressions)
            {
                if (result == null)
                {
                    result = lambda;
                }
                else
                {
                    // 获取当前 LambdaExpression 的参数类型
                    var parameterTypes = lambda.Parameters.Select(p => p.Type).ToArray();

                    // 获取当前 LambdaExpression 的表达式主体
                    var body = lambda.Body;

                    //// 创建一个新的合并后的表达式主体
                    //var combinedBody = Expression.Block(
                    //    new ParameterExpression[] { },
                    //    Expression.Call(
                    //        typeof(LambdaExpression).GetMethod(nameof(LambdaExpression.AndAlso), BindingFlags.Static | BindingFlags.Public), parameterTypes),
                    //        result,
                    //        body
                    //    )
            );

                    result = Expression.Lambda(combinedBody, result.Parameters);
                }
            }

            return result;
        }

        */

        /// <summary>
        /// 得到And后的所有条件2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>如果为空则返回null</returns>
        public Expression<Func<T, bool>> GetFilterExpression<T>()
        {
            Expression<Func<T, bool>> expression = null;
            List<Expression<Func<T, bool>>> expressions = GetFilterExpressions<T>();
            for (var i = 0; i < expressions.Count; i++)
            {
                if (i == 0)
                {
                    expression = expressions[0];
                }
                else
                {
                    expression = expression.AndAlso<T>(expressions[i]);
                }
            }
            return expression;
        }





        public List<QueryField> QueryFields { get; set; } = new List<QueryField>();


        /// <summary>
        /// 保存查询结果列表不可见的列
        /// </summary>
        public List<string> InvisibleCols { get; set; } = new List<string>();



        public QueryFilter()
        {

        }

        /// <summary>
        /// 查询的目标类型
        /// </summary>
        public Type QueryTargetType { get; set; }


        /// <summary>
        /// 能默认一次添加的普通字段用这个
        /// 默认添加子过滤
        /// 类似文件框，没有子条件， 可以有限制条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryFieldExp"></param>
        public QueryField SetQueryField<T>(Expression<Func<T, object>> queryFieldExp)
        {
            //注意这里描述值Caption没有取出给到。
            return SetQueryField<T>(queryFieldExp, true);
        }


        /// <summary>
        /// 设置字段的时候，顺便能设置这个字段关联的实体集合的限制条件
        /// 设置R的限制条件（子条件）
        /// </summary>
        /// <typeparam name="T">主实体</typeparam>
        /// <typeparam name="R">下拉相关实体</typeparam>
        /// <param name="queryFieldExp"></param>
        /// <param name="expFieldLimitCondition">限制关联结果集合的条件</param>
        public void SetQueryField<T, R>(Expression<Func<T, object>> queryFieldExp, Expression<Func<R, bool>> expSubFieldLimitCondition)
        {
            QueryField queryField = SetQueryField<T>(queryFieldExp, typeof(R), true, expSubFieldLimitCondition);
        }
        /// <summary>
        /// 设置字段的时候，顺便能设置这个字段关联的实体集合的限制条件
        /// 设置R的限制条件（子条件）
        /// </summary>
        /// <typeparam name="T">主实体</typeparam>
        /// <typeparam name="R">下拉相关实体</typeparam>
        /// <param name="queryFieldExp"></param>
        /// <param name="expFieldLimitCondition">限制关联结果集合的条件</param>
        public void SetQueryField<T>(Expression<Func<T, object>> queryFieldExp, Expression<Func<T, bool>> expSubFieldLimitCondition)
        {
            QueryField queryField = SetQueryField<T>(queryFieldExp, AdvQueryProcessType.None, null, true, expSubFieldLimitCondition);
        }

        /// <summary>
        /// 设置当前过滤器的条件，一般在调用层灵活调用来设置值
        /// 可以多次设置，多个默认为and连接。
        /// </summary>
        /// <typeparam name="T">主实体</typeparam>
        /// <typeparam name="R">下拉相关实体</typeparam>
        /// <param name="queryFieldExp"></param>
        /// <param name="expFieldLimitCondition">限制关联结果集合的条件</param>
        public void SetFieldLimitCondition<R>(Expression<Func<R, bool>> expFieldLimitCondition)
        {
            // 处理表达式中的闭包变量，将其替换为实际值
            expFieldLimitCondition = ProcessClosureVariables(expFieldLimitCondition);
            
            ExpConverter expConverter = new ExpConverter();

            var whereExp = expConverter.ConvertToLambdaExpression<R>(expFieldLimitCondition);
            if (whereExp != null)
            {
                if (!FilterLimitExpressions.Contains(whereExp))
                {
                    if (this.QueryTargetType == null)
                    {
                        this.QueryTargetType = typeof(R);
                    }

                    FilterLimitExpressions.Add(whereExp);
                }
            }
        }


        /// <summary>
        /// 设置不可见的列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryFieldExp"></param>
        public void SetInvisibleCol<T>(Expression<Func<T, object>> ColNameExp)
        {
            if (QueryTargetType == null)
            {
                QueryTargetType = typeof(T);
            }
            //指定到字符类型，方便使用
            string ColName = RuinorExpressionHelper.ExpressionToString<T>(ColNameExp);
            if (InvisibleCols != null && !InvisibleCols.Contains(ColName))
            {
                InvisibleCols.Add(ColName);
            }
        }



        /// <summary>
        /// 能默认一次添加的普通字段用这个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldType">根据情况设计不同的数据类型</param>
        /// <param name="Para">参数，不同数据类型有不同的情况。根据这个传不同参数</param>
        /// <param name="queryFieldExp"></param>
        public QueryField SetQueryField<T>(Expression<Func<T, object>> queryFieldExp, QueryFieldType fieldType, IQueryFieldData Para)
        {
            QueryField queryField = SetQueryField<T>(queryFieldExp, true);
            switch (fieldType)
            {
                case QueryFieldType.String:
                    break;
                case QueryFieldType.Money:
                    break;
                case QueryFieldType.Qty:
                    break;
                case QueryFieldType.CmbEnum:
                    QueryFieldEnumData enumDataStatus = Para as QueryFieldEnumData;

                    queryField.FieldType = QueryFieldType.CmbEnum;
                    enumDataStatus.SetEnumValueColName<T>(queryFieldExp);
                    enumDataStatus.AddSelectItem = true;
                    enumDataStatus.BindDataSource = EnumBindExt.GetListByEnum(enumDataStatus.EnumType, -1);
                    queryField.AdvQueryFieldType = AdvQueryProcessType.EnumSelect;
                    queryField.QueryFieldDataPara = enumDataStatus;
                    break;
                case QueryFieldType.CmbDbList:
                    break;
                case QueryFieldType.DateTime:
                    break;
                case QueryFieldType.DateTimeRange:
                    QueryFieldDateTimeRangeData dateTimeRangeData = Para as QueryFieldDateTimeRangeData;
                    queryField.FieldType = QueryFieldType.DateTimeRange;
                    dateTimeRangeData.Selected = dateTimeRangeData.Selected;
                    queryField.QueryFieldDataPara = dateTimeRangeData;
                    queryField.AdvQueryFieldType = AdvQueryProcessType.datetimeRange;
                    break;
                case QueryFieldType.CheckBox:
                    break;
                case QueryFieldType.RdbYesNo:
                    break;
                default:
                    break;
            }
            return queryField;
        }


        /// <summary>
        /// 能默认一次添加的普通字段用这个
        /// 作废 只是不想修改太多了。用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryFieldExp"></param>
        public QueryField SetQueryField<T>(Expression<Func<T, object>> queryFieldExp, QueryFieldType fieldType, Type fieldOjectType)
        {
            QueryField queryField = SetQueryField<T>(queryFieldExp, true);
            switch (fieldType)
            {
                case QueryFieldType.String:
                    break;
                case QueryFieldType.Money:
                    break;
                case QueryFieldType.Qty:
                    break;
                case QueryFieldType.CmbEnum:
                    QueryFieldEnumData enumDataStatus = new QueryFieldEnumData();
                    queryField.FieldType = QueryFieldType.CmbEnum;
                    enumDataStatus.EnumType = fieldOjectType;
                    enumDataStatus.SetEnumValueColName<T>(queryFieldExp);
                    enumDataStatus.AddSelectItem = true;
                    enumDataStatus.BindDataSource = EnumBindExt.GetListByEnum(fieldOjectType, -1);
                    queryField.AdvQueryFieldType = AdvQueryProcessType.EnumSelect;
                    queryField.QueryFieldDataPara = enumDataStatus;
                    break;
                case QueryFieldType.CmbDbList:
                    break;
                case QueryFieldType.DateTime:
                    break;
                case QueryFieldType.DateTimeRange:
                    break;
                case QueryFieldType.CheckBox:
                    break;
                case QueryFieldType.RdbYesNo:
                    break;
                default:
                    break;
            }
            return queryField;
        }
        /// <summary>
        /// 指定枚举类型 保留这个写法。暂时没有使用
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="queryFieldExp">查询字段表达式</param>
        /// <param name="defaultSelectedValue">默认选中的值</param>
        public QueryField SetQueryField<T, TEnum>(Expression<Func<T, object>> queryFieldExp, TEnum? defaultSelectedValue = null)
            where TEnum : struct, Enum
        {
            QueryField queryField = SetQueryField<T>(queryFieldExp, true);
            QueryFieldEnumData enumDataStatus = new QueryFieldEnumData();
            queryField.FieldType = QueryFieldType.CmbEnum;
            enumDataStatus.EnumType = typeof(TEnum);
            enumDataStatus.SetEnumValueColName<T>(queryFieldExp);
            enumDataStatus.AddSelectItem = true;

            int? selectedValue = defaultSelectedValue.HasValue ? Convert.ToInt32(defaultSelectedValue.Value) : -1;
            enumDataStatus.BindDataSource = EnumBindExt.GetListByEnum(typeof(TEnum), selectedValue);

            queryField.AdvQueryFieldType = AdvQueryProcessType.EnumSelect;
            queryField.QueryFieldDataPara = enumDataStatus;
            return queryField;
        }




        /// <summary>
        /// 能默认一次添加的普通字段用这个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryFieldExp"></param>
        /// <param name="AddSubFilter">如果是关联字段时，是否添加子过滤条件</param>
        /// <returns></returns>
        public QueryField SetQueryField<T>(Expression<Func<T, object>> queryFieldExp, bool AddSubFilter = true)
        {
            return SetQueryField<T>(queryFieldExp, AdvQueryProcessType.None, null, AddSubFilter, null);
        }

        /// <summary>
        /// 能默认一次添加的普通字段用这个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryFieldExp"></param>
        /// <param name="AddSubFilter">如果是关联字段时，是否添加子过滤条件</param>
        /// <returns></returns>
        public QueryField SetQueryField<T>(Expression<Func<T, object>> queryFieldExp, bool AddSubFilter = true, AdvQueryProcessType fieldType = AdvQueryProcessType.None)
        {
            return SetQueryField<T>(queryFieldExp, fieldType, null, AddSubFilter, null);
        }


        /// <summary>
        /// 基本上都会要查询条件。传参数时顺便把加总列的也传过去
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<string> GetSummaryCols<T>()
        {
            BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(typeof(T).Name + "Processor");
            return baseProcessor.GetSummaryCols();
        }



        /// <summary>
        /// 设置查询字段，并且指定了引用的外键表类型，并且指向了最原始的显示字段
        /// 如：销售出库单22222222222222222
        /// </summary>
        /// <typeparam name="T">第一级查询对象</typeparam>
        /// <typeparam name="R">次级查询对象，或是被引用对象</typeparam>
        /// <param name="queryFieldExp">key,ID，：</param>
        /// <param name="queryFieldNameExp"></param>
        /// <param name="queryOriginalFieldNameExp">原始的字段，主要是选择的结果时要从原来实体中取到对应的值。取的时候要用到。</param>
        /// <param name="expSubFieldLimitCondition"></param>
        /// <returns></returns>
        public QueryField SetQueryField<T, R>(Expression<Func<T, object>> queryFieldExp, Expression<Func<T, object>> queryFieldNameExp = null, Expression<Func<R, object>> queryOriginalFieldNameExp = null, bool AddSubFilter = true, Expression<Func<R, bool>> expSubFieldLimitCondition = null)
        {
            QueryField queryField = new QueryField();
            QueryTargetType = typeof(T);
            queryField.QueryTargetType = QueryTargetType;

            queryField.FieldName = RuinorExpressionHelper.ExpressionToString(queryFieldExp);
            queryField.FieldPropertyInfo = typeof(T).GetProperties().FirstOrDefault(c => c.Name == queryField.FieldName);
            //代替字段
            if (queryFieldNameExp != null)
            {
                //指定到字符类型，方便显示给用户时就可以用文本框了
                queryField.AdvQueryFieldType = AdvQueryProcessType.TextSelect;
                string fieldName = RuinorExpressionHelper.ExpressionToString(queryFieldNameExp);
                queryField.FriendlyFieldNameFormBiz = fieldName;
                queryField.FriendlyFieldNameFromSource = fieldName;
            }
            //指定了就覆盖，不然认为是一样的
            if (queryOriginalFieldNameExp != null)
            {
                string OriginalFieldName = RuinorExpressionHelper.ExpressionToString(queryOriginalFieldNameExp);
                queryField.FriendlyFieldNameFromSource = OriginalFieldName;
            }

            if (AddSubFilter)
            {
                AddSubFilterToQueryField(queryField, expSubFieldLimitCondition, typeof(R));
            }
            // 检查是否已存在相同字段（基于FieldName和QueryTargetType）
            var existingField = QueryFields.FirstOrDefault(qf => 
                qf.FieldName == queryField.FieldName && qf.QueryTargetType == queryField.QueryTargetType);
            
            if (existingField != null)
            {
                // 记录重复添加尝试日志
                // 可以在此处添加日志记录，例如：
                // Logger.LogWarning($"尝试重复添加查询字段：{queryField.FieldName}，类型：{queryField.QueryTargetType.Name}，已被阻止");
                
                // 返回已存在的字段，而不是创建新字段
                return existingField;
            }

            // 添加新字段
            QueryFields.Add(queryField);

            return queryField;
        }



        /// <summary>
        /// 通过别名来设置查询字段,比方调拨单中 ，调出仓库和调入仓库是同一个字段，但是显示的时候要区分，所以用别名来区分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="ReferenceFieldExp"></param>
        /// <param name="SourceIDExp"></param>
        /// <param name="SourceNameExp"></param>
        /// <param name="AddSubFilter"></param>
        /// <param name="expSubFieldLimitCondition"></param>
        /// <returns></returns>
        public QueryField SetQueryFieldByAlias<T, R>(
            Expression<Func<T, object>> ReferenceFieldExp,
            Expression<Func<T, object>> ReferenceReplaceFieldExp = null,
            Expression<Func<R, object>> SourceIDExp = null, Expression<Func<R, object>>
            SourceNameExp = null,
            bool AddSubFilter = true,
            Expression<Func<R, bool>> expSubFieldLimitCondition = null)
        {
            QueryField queryField = new QueryField();
            QueryTargetType = typeof(T);
            queryField.QueryTargetType = QueryTargetType;

            //原始的字段。但是和外键表主键不一样。无法加载下拉
            queryField.FieldName = RuinorExpressionHelper.ExpressionToString(ReferenceFieldExp);
            queryField.FieldPropertyInfo = typeof(T).GetProperties().FirstOrDefault(c => c.Name == queryField.FieldName);

            //目前这个情况更适合下拉？
            queryField.AdvQueryFieldType = AdvQueryProcessType.defaultSelect;

            //这里重新覆盖上面的字段。用代替字段
            if (ReferenceReplaceFieldExp != null)
            {
                string fieldName = RuinorExpressionHelper.ExpressionToString(ReferenceReplaceFieldExp);
                queryField.FriendlyFieldNameFormBiz = fieldName;
            }
            // 覆盖显示的。用名称字段
            if (SourceIDExp != null)
            {
                string strSourceIDExp = RuinorExpressionHelper.ExpressionToString(SourceIDExp);
                queryField.FriendlyFieldValueFromSource = strSourceIDExp;
            }

            // 覆盖显示的。用名称字段
            if (SourceNameExp != null)
            {
                string OriginalFieldName = RuinorExpressionHelper.ExpressionToString(SourceNameExp);
                queryField.FriendlyFieldNameFromSource = OriginalFieldName;
            }

            if (AddSubFilter)
            {
                AddSubFilterToQueryField(queryField, expSubFieldLimitCondition, typeof(R));
            }
            // 检查是否已存在相同字段（基于FieldName和QueryTargetType）
            var existingField = QueryFields.FirstOrDefault(qf => 
                qf.FieldName == queryField.FieldName && qf.QueryTargetType == queryField.QueryTargetType);
            
            if (existingField != null)
            {
                // 记录重复添加尝试日志
                // 可以在此处添加日志记录，例如：
                // Logger.LogWarning($"尝试重复添加查询字段：{queryField.FieldName}，类型：{queryField.QueryTargetType.Name}，已被阻止");
                
                // 返回已存在的字段，而不是创建新字段
                return existingField;
            }

            // 添加新字段
            QueryFields.Add(queryField);

            return queryField;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryFieldIDExp"></param>
        /// <param name="fieldType"></param>
        /// <param name="isEnable">如果是时间则默认是否勾选</param>
        /// <returns></returns>
        public QueryField SetQueryField<T>(Expression<Func<T, object>> queryFieldIDExp,
            AdvQueryProcessType fieldType = AdvQueryProcessType.None, bool isEnable = true)
        {
            return SetQueryField<T>(queryFieldIDExp, fieldType, null, false, null, isEnable);
        }


        public QueryField SetQueryField<T>(Expression<Func<T, object>> queryFieldIDExp,
            AdvQueryProcessType fieldType = AdvQueryProcessType.None,
            Expression<Func<T, object>> queryFieldNameExp = null,
            bool AddSubFilter = true,
            LambdaExpression SubFieldLimitExp = null
           )
        {
            return SetQueryField<T>(queryFieldIDExp, fieldType, queryFieldNameExp, AddSubFilter, SubFieldLimitExp, true);
        }

        /// <summary>
        /// 主的添加方法11111111111111111
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryFieldIDExp">查询条件为这个字段时</param>
        /// <param name="queryFieldNameExp">查询出来的显示为这个字段，ID-》name,通常这个编号的字段在主表及引用的表中 字段名要相同，如果不同则用上面的方法T R</param>
        /// <param name="AddSubFilter">如果是关联字段时，是否添加子过滤条件</param>
        /// <param name="SubFieldLimitExp">子条件限制器</param>
        ///<param name="isEnable">如果是时间则查询条件时默认是否勾选</param>
        public QueryField SetQueryField<T>(Expression<Func<T, object>> queryFieldIDExp, AdvQueryProcessType fieldType = AdvQueryProcessType.None,
            Expression<Func<T, object>> queryFieldNameExp = null,
            bool AddSubFilter = true,
            LambdaExpression SubFieldLimitExp = null,
            bool isEnable = true)
        {
            if (queryFieldIDExp == null)
                throw new ArgumentNullException(nameof(queryFieldIDExp));

            QueryField queryField = new QueryField();

            string fieldID = RuinorExpressionHelper.ExpressionToString(queryFieldIDExp);
            queryField.QueryTargetType = typeof(T);
            queryField.FieldName = fieldID;
            queryField.IsEnabled = isEnable;
            queryField.FieldPropertyInfo = typeof(T).GetProperties().FirstOrDefault(c => c.Name == fieldID);
            queryField.AdvQueryFieldType = fieldType;
            //代替字段
            if (queryFieldNameExp != null)
            {
                string fieldName = RuinorExpressionHelper.ExpressionToString(queryFieldNameExp);
                queryField.FriendlyFieldNameFormBiz = fieldName;
            }

            if (AddSubFilter)
            {
                AddSubFilterToQueryField(queryField, SubFieldLimitExp);
            }

            // 检查是否已存在相同字段（基于FieldName和QueryTargetType）
            var existingField = QueryFields.FirstOrDefault(qf => 
                qf.FieldName == queryField.FieldName && qf.QueryTargetType == queryField.QueryTargetType);
            
            if (existingField != null)
            {
                // 记录重复添加尝试日志
                // 可以在此处添加日志记录，例如：
                // Logger.LogWarning($"尝试重复添加查询字段：{queryField.FieldName}，类型：{queryField.QueryTargetType.Name}，已被阻止");
                
                // 返回已存在的字段，而不是创建新字段
                return existingField;
            }

            // 添加新字段
            QueryFields.Add(queryField);

            return queryField;
        }

        /// <summary>
        /// 添加子级查询条件，可以传子过滤条件，也可以传子对象的类型。如果不传，就是直接获取特性中的表名
        /// </summary>
        /// <param name="queryField"></param>
        /// <param name="SubFieldLimitExp"></param>
        /// <param name="SubQueryTargetType">如果有值：如视图就特殊传过来，不然就是用生成时特性中的表名</param>
        /// <exception cref="InvalidOperationException"></exception>
        private void AddSubFilterToQueryField(QueryField queryField, LambdaExpression SubFieldLimitExp, Type SubQueryTargetType = null)
        {
            if (queryField.FieldPropertyInfo != null)
            {
                //获取可空类型，泛型等真实类型,如果是关联外键

                PropertyInfo pi = queryField.FieldPropertyInfo;
                Type newcolType = pi.PropertyType.GetBaseType();
                if (newcolType == typeof(long))
                {
                    object[] attrs = queryField.FieldPropertyInfo.GetCustomAttributes(false);
                    foreach (var attr in attrs)
                    {
                        if (attr is FKRelationAttribute)
                        {
                            FKRelationAttribute fkrattr = attr as FKRelationAttribute;

                            string SubQueryTableName = string.Empty;
                            if (SubQueryTargetType != null)
                            {
                                SubQueryTableName = SubQueryTargetType.Name;
                                queryField.SubQueryTargetType = SubQueryTargetType;//保存子级类型
                            }
                            else
                            {
                                SubQueryTableName = fkrattr.FKTableName;
                            }
                            BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(SubQueryTableName + "Processor");
                            queryField.SubFilter = baseProcessor.GetQueryFilter(SubFieldLimitExp);
                            queryField.ParentFilter = this;
                            queryField.HasSubFilter = true;

                            //这里认为有子级并且是long型默认给类型为下拉
                            if (queryField.AdvQueryFieldType == AdvQueryProcessType.None)
                            {
                                queryField.AdvQueryFieldType = AdvQueryProcessType.defaultSelect;
                            }

                            if (SubQueryTargetType != null)
                            {
                                queryField.SubFilter.QueryTargetType = SubQueryTargetType;//指定子级的目标类型
                            }
                            else
                            {
                                queryField.SubFilter.QueryTargetType = Assembly.LoadFrom("RUINORERP.Model.dll").GetType("RUINORERP.Model." + SubQueryTableName);
                            }
                            
                            // 处理子过滤器中的闭包变量
                            if (SubFieldLimitExp != null && queryField.SubFilter != null)
                            {
                                // 获取正确的泛型参数类型
                                Type targetType = queryField.SubFilter.QueryTargetType ?? SubQueryTargetType;
                                if (targetType != null)
                                {
                                    // 使用反射调用ProcessClosureVariables方法
                                    var method = this.GetType().GetMethod("ProcessClosureVariables", BindingFlags.Public | BindingFlags.Instance);
                                    if (method != null)
                                    {
                                        var genericMethod = method.MakeGenericMethod(targetType);
                                        var processedExpression = genericMethod.Invoke(this, new object[] { SubFieldLimitExp });
                                        
                                        // 更新子过滤器的FilterLimitExpressions
                                        if (processedExpression != null && queryField.SubFilter.FilterLimitExpressions.Count > 0)
                                        {
                                            queryField.SubFilter.FilterLimitExpressions[0] = (LambdaExpression)processedExpression;
                                        }
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }



        /// <summary>
        /// queryFilter.SetQueryField<View_ProdDetail>(c => c.Location_ID, typeof(tb_Location));
        /// 查询对象是视图。KEY字段在视图的实体类中没有指向外键表名这种情况。这里要指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryFieldExp"></param>
        /// <param name="SubFKTableType">queryFieldExp这个字段所在的实体的类型</param>
        /// <param name="AddSubFilter"></param>
        /// <param name="SubFieldLimitExpression"></param>
        /// <returns></returns>
        public QueryField SetQueryField<T>(Expression<Func<T, object>> queryFieldExp, Type SubFKTableType, bool AddSubFilter = true, LambdaExpression? SubFieldLimitExpression = null)
        {
            if (QueryTargetType != null)
            {
                QueryTargetType = typeof(T);
            }
            //指定到字符类型，方便使用
            string fieldName = RuinorExpressionHelper.ExpressionToString<T>(queryFieldExp);
            QueryField queryField = new QueryField(fieldName);
            queryField.QueryTargetType = QueryTargetType;
            queryField.FieldPropertyInfo = typeof(T).GetProperties().FirstOrDefault(c => c.Name == fieldName);
            queryField.SubQueryTargetType = SubFKTableType;
            if (AddSubFilter)
            {
                BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(SubFKTableType.Name + "Processor");
                queryField.SubFilter = baseProcessor.GetQueryFilter(SubFieldLimitExpression);
                queryField.SubFilter.QueryTargetType = SubFKTableType;
                queryField.IsRelated = true;
                queryField.HasSubFilter = true;
                queryField.FKTableName = SubFKTableType.Name;
                queryField.ParentFilter = this;
            }
            if (!QueryFields.Contains(queryField))
            {
                QueryFields.Add(queryField);
            }
            else
            {

            }
            return queryField;
        }

        /// <summary>
        /// 处理表达式中的闭包变量，将其替换为实际值
        /// </summary>
        /// <typeparam name="T">表达式参数类型</typeparam>
        /// <param name="expression">原始表达式</param>
        /// <returns>处理后的表达式</returns>
        /// <summary>
        /// 处理表达式中的闭包变量，将闭包变量替换为实际值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">包含闭包变量的表达式</param>
        /// <returns>处理后的表达式，闭包变量已替换为实际值</returns>
        public Expression<Func<T, bool>> ProcessClosureVariables<T>(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
                return null;

            try
            {
                // 创建闭包变量替换器
                var closureReplacer = new ClosureVariableReplacer();
                var newBody = closureReplacer.Visit(expression.Body);
                
                // 如果表达式没有变化，直接返回原始表达式
                if (newBody == expression.Body)
                    return expression;
                
                // 创建新的Lambda表达式
                return Expression.Lambda<Func<T, bool>>(newBody, expression.Parameters);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理闭包变量失败: {ex.Message}");
                return expression; // 返回原始表达式
            }
        }
        
        /// <summary>
        /// 闭包变量替换器 - 将闭包变量替换为实际值
        /// </summary>
        private class ClosureVariableReplacer : ExpressionVisitor
        {
            protected override Expression VisitMember(MemberExpression node)
            {
                // 检测闭包变量（访问常量对象的成员）
                if (node.Expression is ConstantExpression constExp)
                {
                    try
                    {
                        // 获取闭包变量的实际值
                        var value = GetMemberValue(constExp.Value, node.Member);
                        // 创建常量表达式替换闭包变量引用
                        return Expression.Constant(value, node.Type);
                    }
                    catch
                    {
                        // 如果无法获取值，继续正常处理
                    }
                }
                // 处理嵌套的成员访问，例如 entity.CustomerVendor_ID
                else if (node.Expression is MemberExpression parentMemberExp)
                {
                    try
                    {
                        // 递归获取嵌套成员的值
                        object parentValue = GetNestedMemberValue(parentMemberExp);
                        if (parentValue != null)
                        {
                            // 从父对象获取当前成员的值
                            var value = GetMemberValue(parentValue, node.Member);
                            return Expression.Constant(value, node.Type);
                        }
                    }
                    catch
                    {
                        // 如果无法获取值，继续正常处理
                    }
                }
                
                // 继续处理其他类型的成员访问
                return base.VisitMember(node);
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                // 处理二元表达式中的类型转换
                if (node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual)
                {
                    var left = Visit(node.Left);
                    var right = Visit(node.Right);
                    
                    // 如果左右都是常量表达式，且类型不同，尝试进行类型转换
                    if (left is ConstantExpression leftConst && right is ConstantExpression rightConst)
                    {
                        // 如果是布尔值比较，确保类型一致
                        if (leftConst.Type == typeof(bool) && rightConst.Type == typeof(bool))
                        {
                            return Expression.MakeBinary(node.NodeType, left, right);
                        }
                        // 如果一个是布尔值，另一个是可空布尔值，进行类型转换
                        else if (leftConst.Type == typeof(bool) && rightConst.Type == typeof(bool?))
                        {
                            var convertedLeft = Expression.Convert(left, typeof(bool?));
                            return Expression.MakeBinary(node.NodeType, convertedLeft, right);
                        }
                        else if (leftConst.Type == typeof(bool?) && rightConst.Type == typeof(bool))
                        {
                            var convertedRight = Expression.Convert(right, typeof(bool?));
                            return Expression.MakeBinary(node.NodeType, left, convertedRight);
                        }
                    }
                    
                    // 如果没有类型转换需求，直接返回
                    if (left != node.Left || right != node.Right)
                    {
                        return Expression.MakeBinary(node.NodeType, left, right, node.IsLiftedToNull, node.Method);
                    }
                }
                
                return base.VisitBinary(node);
            }

            /// <summary>
            /// 递归获取嵌套成员的值
            /// </summary>
            /// <param name="memberExp">成员表达式</param>
            /// <returns>成员的值</returns>
            private object GetNestedMemberValue(MemberExpression memberExp)
            {
                // 如果是常量表达式的成员，直接获取
                if (memberExp.Expression is ConstantExpression constExp)
                {
                    return GetMemberValue(constExp.Value, memberExp.Member);
                }
                // 如果是嵌套的成员表达式，递归获取
                else if (memberExp.Expression is MemberExpression parentMemberExp)
                {
                    object parentValue = GetNestedMemberValue(parentMemberExp);
                    if (parentValue != null)
                    {
                        return GetMemberValue(parentValue, memberExp.Member);
                    }
                }
                
                return null;
            }

            private object GetMemberValue(object instance, MemberInfo member)
            {
                if (member is PropertyInfo property)
                    return property.GetValue(instance);
                else if (member is FieldInfo field)
                    return field.GetValue(instance);
                return null;
            }
        }
    }

    /// <summary>
    /// 闭包变量替换访问者 - 将闭包变量替换为它们的实际值（常量表达式）
    /// </summary>
    public class ClosureVariableReplacer : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            // 检测闭包变量（访问常量对象的成员）
            if (node.Expression is ConstantExpression constExp)
            {
                try
                {
                    // 获取闭包变量的实际值
                    var value = GetMemberValue(constExp.Value, node.Member);
                    // 创建常量表达式替换闭包变量引用
                    return Expression.Constant(value, node.Type);
                }
                catch
                {
                    // 如果无法获取值，继续正常处理
                }
            }
            // 继续处理其他类型的成员访问
            return base.VisitMember(node);
        }

        private object GetMemberValue(object instance, MemberInfo member)
        {
            if (member is PropertyInfo property)
                return property.GetValue(instance);
            else if (member is FieldInfo field)
                return field.GetValue(instance);
            return null;
        }
    }

    /// <summary>
    /// 参数替换访问者 - 替换表达式中的参数
    /// </summary>
    public class ParameterReplacementVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        public ParameterReplacementVisitor(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        public ParameterExpression Parameter
        {
            get { return _parameter; }
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Name == _parameter.Name)
            {
                return _parameter;
            }

            return base.VisitParameter(node);
        }
    }
}