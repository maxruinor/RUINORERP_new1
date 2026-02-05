using RUINORERP.Common.Helper;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RUINORERP.Extensions.AOP
{
    public abstract class CacheAOPbase : IInterceptor
    {
        /// <summary>
        /// AOP的拦截方法
        /// </summary>
        /// <param name="invocation"></param>
        public abstract void Intercept(IInvocation invocation);

        /// <summary>
        /// 自定义缓存的key
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        protected string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，最多三个

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key = $"{key}{param}:";
            }

            return key.TrimEnd(':');
        }

        /// <summary>
        /// object 转 string
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected static string GetArgumentValue(object arg)
        {
            // 空值处理
            if (arg == null)
                return string.Empty;

            // 基本类型处理
            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            if (arg is string)
                return arg.ToString();

            if (arg is ValueType)
                return arg.ObjToString();

            // 排除不适合缓存的类型
            var argType = arg.GetType();
            var typeName = argType.Name;
            var fullTypeName = argType.FullName;
            
            // 排除SqlSugar相关类型
            if (typeName.Contains("SqlSugar") || fullTypeName.Contains("SqlSugar"))
            {
                return string.Empty;
            }
            
            // 排除数据库连接相关类型
            if (typeName.Contains("Connection") || typeName.Contains("DbClient") || typeName.Contains("UnitOfWork"))
            {
                return string.Empty;
            }
            
            // 排除缓存相关类型
            if (typeName.Contains("Cache") || typeName.Contains("Caching"))
            {
                return string.Empty;
            }
            
            // 排除服务相关类型
            if (typeName.Contains("Service"))
            {
                return string.Empty;
            }
            
            // 排除工具类和辅助类
            if (typeName.Contains("Helper") || typeName.Contains("Util") || typeName.Contains("Tool"))
            {
                return string.Empty;
            }
            
            // 排除系统级组件
            if (fullTypeName.Contains("System.") || fullTypeName.Contains("Microsoft."))
            {
                return string.Empty;
            }
            
            // 排除集合类型
            if (argType.IsArray || argType.GetInterface("ICollection") != null || argType.GetInterface("IEnumerable") != null)
            {
                return string.Empty;
            }
            
            // 表达式树处理
            if (arg is Expression)
            {
                try
                {
                    var obj = arg as Expression;
                    var result = Resolve(obj);
                    return MD5Helper.MD5Encrypt16(result);
                }
                catch
                {
                    return string.Empty;
                }
            }
            
            // 其他复杂类型处理
            if (argType.IsClass)
            {
                try
                {
                    return MD5Helper.MD5Encrypt16(JsonConvert.SerializeObject(arg));
                }
                catch
                {
                    // 序列化失败时返回空值
                    return string.Empty;
                }
            }

            return $"value:{arg.ObjToString()}";
        }

        private static string Resolve(Expression expression)
        {
            ExpressionContext expContext = new ExpressionContext();
            expContext.Resolve(expression, ResolveExpressType.WhereSingle);
            var value = expContext.Result.GetString();
            var pars = expContext.Parameters;

            pars.ForEach(s =>
            {
                value = value.Replace(s.ParameterName, s.Value.ObjToString());
            });

            return value;
        }

        private static string GetOperator(ExpressionType expressiontype)
        {
            switch (expressiontype)
            {
                case ExpressionType.And:
                    return "and";
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                    return "or";
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                default:
                    throw new Exception($"不支持{expressiontype}此种运算符查找！");
            }
        }

        private static string ResolveFunc(Expression left, Expression right, ExpressionType expressiontype)
        {
            var Name = (left as MemberExpression).Member.Name;
            var Value = (right as ConstantExpression).Value;
            var Operator = GetOperator(expressiontype);
            return Name + Operator + Value ?? "null";
        }

        private static string ResolveLinqToObject(Expression expression, object value, ExpressionType? expressiontype = null)
        {
            var MethodCall = expression as MethodCallExpression;
            var MethodName = MethodCall.Method.Name;
            switch (MethodName)
            {
                case "Contains":
                    if (MethodCall.Object != null)
                        return Like(MethodCall);
                    return In(MethodCall, value);
                case "Count":
                    return Len(MethodCall, value, expressiontype.Value);
                case "LongCount":
                    return Len(MethodCall, value, expressiontype.Value);
                default:
                    throw new Exception($"不支持{MethodName}方法的查找！");
            }
        }

        private static string In(MethodCallExpression expression, object isTrue)
        {
            var Argument1 = (expression.Arguments[0] as MemberExpression).Expression as ConstantExpression;
            var Argument2 = expression.Arguments[1] as MemberExpression;
            var Field_Array = Argument1.Value.GetType().GetFields().First();
            object[] Array = Field_Array.GetValue(Argument1.Value) as object[];
            List<string> SetInPara = new List<string>();
            for (int i = 0; i < Array.Length; i++)
            {
                string Name_para = "InParameter" + i;
                string Value = Array[i].ToString();
                SetInPara.Add(Value);
            }
            string Name = Argument2.Member.Name;
            string Operator = Convert.ToBoolean(isTrue) ? "in" : " not in";
            string CompName = string.Join(",", SetInPara);
            string Result = $"{Name} {Operator} ({CompName})";
            return Result;
        }
        private static string Like(MethodCallExpression expression)
        {

            var Temp = expression.Arguments[0];
            LambdaExpression lambda = Expression.Lambda(Temp);
            Delegate fn = lambda.Compile();
            var tempValue = Expression.Constant(fn.DynamicInvoke(null), Temp.Type);
            string Value = $"%{tempValue}%";
            string Name = (expression.Object as MemberExpression).Member.Name;
            string Result = $"{Name} like {Value}";
            return Result;
        }


        private static string Len(MethodCallExpression expression, object value, ExpressionType expressiontype)
        {
            object Name = (expression.Arguments[0] as MemberExpression).Member.Name;
            string Operator = GetOperator(expressiontype);
            string Result = $"len({Name}){Operator}{value.ToString()}";
            return Result;
        }

    }
}
