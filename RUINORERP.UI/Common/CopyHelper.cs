using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;


namespace RUINORERP.UI.Common
{
        public static class FastCopy<S, T>
        {
            static Action<S, T> action = CreateCopier();
            /// <summary>
            /// 复制两个对象同名属性值
            /// </summary>
            /// <typeparam name="S"></typeparam>
            /// <typeparam name="T"></typeparam>
            /// <param name="source">源对象</param>
            /// <param name="target">目标对象</param>
            /// <param name="copyNull">源对象属性值为null时，是否将值复制给目标对象</param>
            public static void Copy(S source, T target, bool copyNull = true)
            {
                action(source, target);
            }

            /// <summary>
            /// 为指定的两种类型编译生成属性复制委托
            /// </summary>
            /// <typeparam name="S"></typeparam>
            /// <typeparam name="T"></typeparam>
            /// <param name="copyNull">源对象属性值为null时，是否将值复制给目标对象</param>
            /// <returns></returns>
            private static Action<S, T> CreateCopier()
            {
                ParameterExpression source = Expression.Parameter(typeof(S));
                ParameterExpression target = Expression.Parameter(typeof(T));
                var sourceProps = typeof(S).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead).ToList();
                var targetProps = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite).ToList();

                // 查找可进行赋值的属性
                var copyProps = targetProps.Where(tProp => sourceProps.Where(sProp => sProp.Name == tProp.Name// 名称一致 且
                && (
                sProp.PropertyType == tProp.PropertyType// 属性类型一致
                )).Count() > 0);

                var block = Expression.Block(from p in copyProps select Expression.Assign(Expression.Property(target, p.Name), Expression.Property(source, p.Name)));
                Expression<Action<S, T>> lambda = Expression.Lambda<Action<S, T>>(block, source, target);
                return lambda.Compile();
            }
        }
   
}
