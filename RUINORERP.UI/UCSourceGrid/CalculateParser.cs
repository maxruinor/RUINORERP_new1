using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RUINORERP.UI.UCSourceGrid
{
    public static class CalculateParser<T>
    {
        /// <summary>
        /// 解析表达式，支持多属性计算
        /// </summary>
        public static CalculateFormula ParserString(Expression expression)
        {
            var formula = new CalculateFormula();
            formula.Parameter = new List<string>();

            // 提取表达式中的所有属性名
            ExtractProperties(expression, formula.Parameter);

            // 处理表达式字符串，移除参数名前缀（如 "a." 或 "b."）
            string expressionStr = expression.ToString();
            foreach (var param in GetParameters(expression))
            {
                expressionStr = expressionStr.Replace($"{param}.", "");
            }

            formula.StringFormula = expressionStr;
            formula.OperandQty = formula.Parameter.Count;
            formula.OriginalExpression = expressionStr;

            return formula;
        }

        // 从表达式中提取参数名
        private static List<string> GetParameters(Expression expression)
        {
            var parameters = new List<string>();

            if (expression is LambdaExpression lambda)
            {
                parameters.AddRange(lambda.Parameters.Select(p => p.Name));
            }
            else if (expression is BinaryExpression binary)
            {
                parameters.AddRange(GetParameters(binary.Left));
                parameters.AddRange(GetParameters(binary.Right));
            }
            else if (expression is MemberExpression member && member.Expression is ParameterExpression param)
            {
                parameters.Add(param.Name);
            }

            return parameters.Distinct().ToList();
        }

        // 从表达式中提取属性名
        private static void ExtractProperties(Expression expression, List<string> properties)
        {
            if (expression is BinaryExpression binary)
            {
                ExtractProperties(binary.Left, properties);
                ExtractProperties(binary.Right, properties);
            }
            else if (expression is MemberExpression member && member.Member is PropertyInfo)
            {
                // 提取属性名（去除参数前缀）
                string propertyName = member.Member.Name;
                if (!properties.Contains(propertyName))
                {
                    properties.Add(propertyName);
                }
            }
            else if (expression is UnaryExpression unary)
            {
                ExtractProperties(unary.Operand, properties);
            }
        }

        // 各种表达式重载
        public static CalculateFormula ParserString(Expression<Func<T, object>> expression)
        {
            return ParserString(expression.Body);
        }

        public static CalculateFormula ParserString(Expression<Func<T, T, object>> expression)
        {
            return ParserString(expression.Body);
        }

        public static CalculateFormula ParserString(Expression<Func<T, T, T, object>> expression)
        {
            return ParserString(expression.Body);
        }
    }
}
