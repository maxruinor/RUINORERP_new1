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


        public virtual QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            return queryFilter;
        }

        /// <summary>
        /// 返回查询当前指定实体的结果，并且提供查询条件
        /// </summary>
        /// <param name="FilterFieldLimitExpression">限制条件<paramref name="FilterFieldLimitExpression"/>
        /// <returns></returns>
        public virtual QueryFilter GetQueryFilter(LambdaExpression FilterFieldLimitExpression)
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter = GetQueryFilter();
            //_logger.LogError(this.ToString() + "没有实现GetQueryFilter，请在Process.cs中实现");
            if (FilterFieldLimitExpression != null)
            {
                queryFilter.FilterLimitExpressions.Add(FilterFieldLimitExpression);
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
            //return list;
            List<T> lastList = new List<T>();
            foreach (var item in list)
            {
                lastList.Add(item);
            }

            // SqlFunc.

            QueryFilter queryFilter = this.GetQueryFilter();
            if (queryFilter.FilterLimitExpressions != null && queryFilter.FilterLimitExpressions.Count > 0)
            {
                if (queryFilter.FilterLimitExpressions.Count == 0)
                {
                    return list;
                }
                //多个条件？
                if (queryFilter.FilterLimitExpressions.Count > 1)
                {

                }

                // 将 LambdaExpression 转换为 BinaryExpression
                //var binaryExpression = queryFilter.FilterLimitExpressions[0].Body as BinaryExpression;//这样转换不行。只会第得左子树
                var binaryExpression = (BinaryExpression)queryFilter.FilterLimitExpressions[0].Body;

                // 逐层转换表达式
                while (binaryExpression != null)
                {
                    //只有一个等式时
                    if (binaryExpression.NodeType == ExpressionType.Equal)
                    {
                        #region 移除不符合条件的
                        string FieldName = binaryExpression.Left.GetMemberInfo().Name;
                        string FiledValue = GetExpressinValueFromExp(binaryExpression.Right);
                        for (int i = 0; i < lastList.Count; i++)
                        {
                            if (lastList[i].GetPropertyValue(FieldName).ToString() != FiledValue)
                            {
                                lastList.Remove(lastList[i]);
                            }
                        }
                        #endregion
                        break;
                    }

                    //有多个条件时
                    if (binaryExpression != null && binaryExpression.Left != null && binaryExpression.NodeType == ExpressionType.AndAlso)
                    {
                        var binaryExpressionLeft = (BinaryExpression)binaryExpression.Left;
                        //(t.Is_enabled == True)  后面包含 等其他形式再来补充
                        //左边是变量，右边是常量
                        if (binaryExpressionLeft.NodeType == ExpressionType.Equal)
                        {
                            #region 移除不符合条件的
                            // 获取成员名称和常量值
                            if (binaryExpressionLeft.Left is MemberExpression memberExpression)
                            {
                                string FieldName = memberExpression.Member.Name;
                                string FiledValue = GetExpressinValueFromExp(binaryExpressionLeft.Right);
                                //string FieldName = binaryExpressionLeft.Left.GetMemberInfo().Name;
                                //string FiledValue = (binaryExpressionright).Value.ToString();
                                for (int i = 0; i < lastList.Count; i++)
                                {
                                    if (lastList[i].GetPropertyValue(FieldName).ToString() != FiledValue)
                                    {
                                        //listInstances.Add(item);
                                        lastList.Remove(lastList[i]);
                                    }
                                }
                            }
                            #endregion
                        }
                    }

                    // 如果右子树也是 BinaryExpression，则继续转换
                    if (binaryExpression.NodeType == ExpressionType.AndAlso &&
                        binaryExpression.Right is BinaryExpression rightt)
                    {
                        binaryExpression = rightt;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return lastList;
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
                    if (value is bool boolValue && boolValue)
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
