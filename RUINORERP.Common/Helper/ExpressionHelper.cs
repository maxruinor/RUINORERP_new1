using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Helper
{

    /*
     // 当你只需要获取一个属性名称时
string propertyName = ExpressionHelper.GetPropertyName<Person>(p => p.Name);
// propertyName 现在是 "Name"，并且具有编译时检查

// 用于反射操作
typeof(Person).GetProperty(propertyName)?.GetValue(person);

// 用于动态查询
var query = dbContext.Persons
    .OrderBy(ExpressionHelper.GetPropertyName<Person>(p => p.CreatedDate));

// 或者用于你之前的集合场景
var includeProperties = new List<string>
{
    ExpressionHelper.GetPropertyName<Person>(p => p.Name),
    ExpressionHelper.GetPropertyName<Person>(p => p.Age)
};
     
     */
    public static class ExpressionHelper
    {
        //  RUINORERP.Common.Helper.ExpressionHelper.Include<tb_ButtonInfo, string>(includeProperties, c => c.BtnName);
        public static string ToFieldName<T>(
           this string properties,
           Expression<Func<T, object>> propertyExpression)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            return GetPropertyName(propertyExpression);

        }
        public static ICollection<string> Include<T>(
            this ICollection<string> properties,
            Expression<Func<T, object>> propertyExpression)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            properties.Add(GetPropertyName(propertyExpression));
            return properties;
        }


        // 获取单个属性名称的方法
        public static string GetPropertyName<T, TProperty>(
            Expression<Func<T, TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var memberInfo = GetPropertyMemberInfo(propertyExpression.Body);
            return memberInfo.Name;
        }
        


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty">指定的字段的类型</typeparam>
        /// <param name="properties"></param>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICollection<string> Include<T, TProperty>(
            this ICollection<string> properties,
            Expression<Func<T, TProperty>> propertyExpression)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            properties.Add(GetPropertyName(propertyExpression));
            return properties;
        }


        // 表达式解析的核心逻辑
        private static MemberInfo GetPropertyMemberInfo(Expression expression)
        {
            // 处理直接属性访问，如 c => c.Property
            if (expression is MemberExpression memberExpr &&
                memberExpr.Member is PropertyInfo propertyInfo)
            {
                return propertyInfo;
            }
            //if (expression is MemberExpression memberExpr &&
            //    memberExpr.Member.MemberType == MemberTypes.Property)
            //{
            //    return memberExpr.Member;
            //}


            // 处理值类型装箱的情况，如 c => (object)c.IntProperty
            if (expression is UnaryExpression unaryExpr &&
                unaryExpr.NodeType == ExpressionType.Convert &&
                unaryExpr.Operand is MemberExpression operandMemberExpr &&
                operandMemberExpr.Member is PropertyInfo operandPropertyInfo)
            {
                return operandPropertyInfo;
            }

            throw new ArgumentException(
                "Expression must be a simple property access (e.g., c => c.Property)",
                nameof(expression));
        }


        #region 比较重复用的


        // 创建属性值获取器委托，提高反射性能
        public static Func<object, object> CreateGetter(PropertyInfo property)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var convert = Expression.Convert(instance, property.DeclaringType);
            var propertyAccess = Expression.Property(convert, property);
            var convertResult = Expression.Convert(propertyAccess, typeof(object));
            var lambda = Expression.Lambda<Func<object, object>>(convertResult, instance);
            return lambda.Compile();
        }

        // 创建用于比较的键
        public static Tuple<object[]> CreateKey<T>(T item, List<dynamic> propertyAccessors)
        {
            var values = new object[propertyAccessors.Count];
            for (int i = 0; i < propertyAccessors.Count; i++)
            {
                values[i] = propertyAccessors[i].GetPropertyValue(item);
            }
            return Tuple.Create(values);
        }


        #endregion

    }
}
