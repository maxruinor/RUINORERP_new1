using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public class CriteriaCollectionHandler
    {
        /* By Harvey Hu. @2016 */

        protected string PropertyName { get; set; }

        protected ComparerEnum Comparer { get; set; }

        protected object Target { get; set; }  //

        public CriteriaCollectionHandler(string propertyName, object target, ComparerEnum comparer)
        {
            this.PropertyName = propertyName;
            this.Comparer = comparer;
            this.Target = target;
        }

        private IQueryable<T> Filter<T>(IQueryable<T> source, string propertyName, ComparerEnum comparer, object target)
        {
            var type = typeof(T);
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);



            var parameter = Expression.Parameter(type, "p");
            Expression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var getValueOrDefault = property.PropertyType.GetMethods().First(p => p.Name == "GetValueOrDefault");
                propertyAccess = Expression.Call(propertyAccess, getValueOrDefault);
            }
            var constExpression = Expression.Constant(ConvertTo(target, property.PropertyType)); // 转换为target的类型，以作比较
            Expression comparisionExpression;
            switch (comparer)
            {
                case ComparerEnum.Eq:
                    comparisionExpression = Expression.Equal(propertyAccess, constExpression);
                    break;
                case ComparerEnum.Ne:
                    comparisionExpression = Expression.NotEqual(propertyAccess, constExpression);
                    break;
                case ComparerEnum.Lt:
                    comparisionExpression = Expression.LessThan(propertyAccess, constExpression);
                    break;
                case ComparerEnum.Gt:
                    comparisionExpression = Expression.GreaterThan(propertyAccess, constExpression);
                    break;
                case ComparerEnum.Le:
                    comparisionExpression = Expression.LessThanOrEqual(propertyAccess, constExpression);
                    break;
                case ComparerEnum.Ge:
                    comparisionExpression = Expression.GreaterThanOrEqual(propertyAccess, constExpression);
                    break;
                case ComparerEnum.StringLike:
                    if (property.PropertyType != typeof(string))
                    {
                        throw new NotSupportedException("StringLike is only suitable for string type property!");
                    }


                    var stringContainsMethod = typeof(CriteriaCollectionHandler).GetMethod("StringContains");

                    comparisionExpression = Expression.Call(stringContainsMethod, propertyAccess, constExpression);

                    break;
                default:
                    comparisionExpression = Expression.Equal(propertyAccess, constExpression);
                    break;
            }


            var compareExp = Expression.Lambda(comparisionExpression, parameter);
            var typeArguments = new Type[] { type };
            var methodName = "Where"; //sortOrder == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(compareExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static bool StringContains(string value, string subValue)
        {
            if (value == null)
            {
                return false;
            }

            return value.Contains(subValue);
        }


        protected object ConvertTo(object convertibleValue, Type targetType)
        {
            if (null == convertibleValue)
            {
                return null;
            }

            if (!targetType.IsGenericType)
            {
                return Convert.ChangeType(convertibleValue, targetType);
            }
            else
            {
                Type genericTypeDefinition = targetType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    var temp = Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(targetType));
                    var result = Activator.CreateInstance(targetType, temp);
                    return result;
                }
            }
            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, targetType.FullName));
        }


        public virtual ICollection<T> Execute<T>(ICollection<T> values)
        {
            var result = Filter(values.AsQueryable(), this.PropertyName, this.Comparer, this.Target).ToList();
            return result;
        }

    }

    public enum ComparerEnum
    {
        Eq,
        Ne,
        Lt,
        Gt,
        Le,
        Ge,
        StringLike,
    }
}
