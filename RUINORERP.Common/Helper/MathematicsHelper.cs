using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Dynamic.Core;
namespace RUINORERP.Common.Helper
{
    public static class EvalEx

    {
        /// <summary>
        /// 将一个包含表达式的字符串作为参数,并计算这个表达式的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="propertyPath"></param>
        /// <returns></returns>
        public static T GetEval<T>(Type type, string propertyPath)
        {

            if (type == null)

                return default(T);
            // DynamicExpressionParser.ParseLambda()
            var func = DynamicExpressionParser.ParseLambda(type, null, propertyPath);

            var objParameter = System.Linq.Expressions.Expression.Parameter(typeof(object), "obj");

            var objConvert = System.Linq.Expressions.Expression.Convert(objParameter, type);

            var objInvoke = System.Linq.Expressions.Expression.Invoke(func, objConvert);

            var resultExpression = System.Linq.Expressions.Expression.Lambda<T>(objInvoke, objParameter);

            return resultExpression.Compile();

        }

    }
    /*
    static void testGetEval<TT>()

    {

        var list = new List<TT>();

        for (int i = 0; i < 10000; i++)

        {

            list.Add(new TT() { T1 = i.ToString(), T3 = i });

        }

        Stopwatch sw = new Stopwatch();

        var do1 = EvalEx.GetEval<Func<TT, string>>(typeof(TT), "T1+ (T3*100).ToString() ");

        var do2 = EvalEx.GetEval<Func<object, string>>(typeof(TT), "T1+ (T3*100).ToString() ");

        var do3 = EvalEx.GetEval<Func<object, object>>(typeof(TT), "T1+(T3*100).ToString() ");

        sw.Start();

        list.ForEach(p => do1(p));

        Console.WriteLine(sw.Elapsed);

        sw.Restart();

        list.ForEach(p => do2(p));

        Console.WriteLine(sw.Elapsed);

        sw.Restart();

        list.ForEach(p => do3(p));

        Console.WriteLine(sw.Elapsed);

        sw.Restart();

        list.ForEach(p => do1.DynamicInvoke(p));

        Console.WriteLine(sw.Elapsed);

        sw.Stop();

        Console.Read();

    }
    */
}
