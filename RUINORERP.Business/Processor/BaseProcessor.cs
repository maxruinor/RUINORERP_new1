using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using System.Windows.Media.Media3D;



namespace RUINORERP.Business.Processor
{

    /// <summary>
    /// GetQueryFilter  在最基础查询时。是否要将条件判断一个等级层次。是枚举类型确认，还是指定固定两个方法来指定条件呢？如 启用可用。应用时生效。基础维护时不生产（不能过滤掉)
    /// </summary>
    public class BaseProcessor
    {
        public ApplicationContext _appContext;
        public ILogger<BaseProcessor> _logger;
        public IUnitOfWorkManage _unitOfWorkManage;
        public BaseProcessor(ILogger<BaseProcessor> logger, IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext = null)
        {
            _logger = logger;
            _appContext = appContext;
            _unitOfWorkManage = unitOfWorkManage;
        }

        /// <summary>
        /// 如果查询条件要根据菜单来判断，则要赋值
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; }


        public virtual QueryFilter GetQueryFilter()
        {
            // 直接使用当前处理器的ApplicationContext创建QueryFilter实例
            // 这样可以避免业务层依赖UI层，保持良好的分层架构
            return new QueryFilter(_appContext);
        }

        /// <summary>
        /// 返回查询当前指定实体的结果，并且提供查询条件
        /// </summary>
        /// <param name="FilterFieldLimitExpression">限制条件<paramref name="FilterFieldLimitExpression"/>
        /// <returns></returns>
        public virtual QueryFilter GetQueryFilter(LambdaExpression FilterFieldLimitExpression)
        {
            // 调用上面修改后的GetQueryFilter()方法获取实例，确保从DI容器获取
            QueryFilter queryFilter = GetQueryFilter();
            if (FilterFieldLimitExpression != null)
            {
                if (!queryFilter.FilterLimitExpressions.Contains(FilterFieldLimitExpression))
                {
                    queryFilter.FilterLimitExpressions.Add(FilterFieldLimitExpression);
                }
            }
            return queryFilter;
        }


        /// <summary>
        /// 用于加载下拉时有过滤条件的情况就子类重写进行过滤操作，没有的话，直接返回本身
        /// 移除不符合条件的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual List<T> GetListByLimitExp<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return list;
            }

            QueryFilter queryFilter = this.GetQueryFilter();
            if (queryFilter.FilterLimitExpressions == null || queryFilter.FilterLimitExpressions.Count == 0)
            {
                return list;
            }

            List<T> lastList = new List<T>(list);

            var filterExpressions = new List<Func<T, bool>>();

            foreach (var filterLimitExpression in queryFilter.FilterLimitExpressions)
            {
                var filterFunc = BuildFilterFunc<T>(filterLimitExpression);
                if (filterFunc != null)
                {
                    filterExpressions.Add(filterFunc);
                }
            }

            if (filterExpressions.Count == 0)
            {
                return list;
            }

            return lastList.Where(item => filterExpressions.All(func => func(item))).ToList();
        }

        private Func<T, bool> BuildFilterFunc<T>(LambdaExpression expression)
        {
            try
            {
                var binaryExpression = expression.Body as BinaryExpression;
                if (binaryExpression == null)
                {
                    return null;
                }

                var conditions = new List<Func<T, bool>>();
                ProcessBinaryExpression(binaryExpression, conditions);

                if (conditions.Count == 0)
                {
                    return null;
                }

                return item => conditions.All(condition => condition(item));
            }
            catch
            {
                return null;
            }
        }

        private void ProcessBinaryExpression<T>(BinaryExpression binaryExpression, List<Func<T, bool>> conditions)
        {
            if (binaryExpression == null)
            {
                return;
            }

            if (binaryExpression.NodeType == ExpressionType.Equal)
            {
                var condition = BuildEqualCondition<T>(binaryExpression);
                if (condition != null)
                {
                    conditions.Add(condition);
                }
            }
            else if (binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                ProcessBinaryExpression<T>(binaryExpression.Left as BinaryExpression, conditions);
                ProcessBinaryExpression<T>(binaryExpression.Right as BinaryExpression, conditions);
            }
        }

        private Func<T, bool> BuildEqualCondition<T>(BinaryExpression binaryExpression)
        {
            try
            {
                if (binaryExpression.Left is MemberExpression memberExpression)
                {
                    string fieldName = memberExpression.Member.Name;
                    string fieldValue = GetExpressinValueFromExp(binaryExpression.Right);

                    return item =>
                    {
                        var propertyValue = item.GetPropertyValue(fieldName);
                        return propertyValue != null && propertyValue.ToString() == fieldValue;
                    };
                }
            }
            catch
            {
            }

            return null;
        }


        public string GetExpressinValueFromExp(Expression expression)
        {
            string expressionValue = string.Empty;
            if (expression is ConstantExpression constant)
            {
                expressionValue = constant.Value.ToString();
            }
            else if (expression is MemberExpression member)
            {
                expressionValue = member.Member.Name;
            }
            else if (expression is UnaryExpression unary)
            {
                // 获取单目运算的参数
                if (unary.Operand is ConstantExpression constExp)
                {
                    var value = constExp.Value;
                    if (value is bool boolValue)
                    {
                        expressionValue = value.ToString();
                    }
                }
            }

            return expressionValue;

        }


        public virtual List<string> GetSummaryCols()
        {
            return new List<string>();
        }


    }


}
