using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Extensions
{
    public static partial class ExtObject
    {

        /*
        //实体集合
        List<Question> list = new List<Question>();
        //重组为需要的字段的新实体集合
        var temp = list.Select(it => new { it.quesId, it.questype }).ToList();
        //获取某个属性值组成数组，并已逗号隔开
        var tempstr = string.Join(",", list.Select(it => it.quesId).ToList());
        //或者
        string resultSeries = list.Aggregate("", (current, s) => current + (s.Name + ","));

        //或者for循环
        string result = "";
for (int i = 0; i<list.Count; i++) {
    result = result + list[i] + ",";
}
    Console.Write("循环方式实现结果: " + result);
//去掉最后一个","使用TrimEnd。
Console.Write("循环方式实现结果: " + result.TrimEnd(','));
 
 */
 
 


        /// <summary>
        /// IEnumerable转换为List'T'
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static List<T> ListToArray<T>(this IList<T> source)
        {
            return new List<T>(source.Cast<T>());
        }


    }
}
