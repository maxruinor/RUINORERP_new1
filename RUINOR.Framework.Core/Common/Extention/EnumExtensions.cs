using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINOR.Framework.Core.Common.Extention
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取名字
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetName(this Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }
        /// <summary>
        /// 获取名字和值
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <param name="lowerFirstLetter">是否转化为小写</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetNamesAndValues(this Type enumType, bool lowerFirstLetter)
        {
            //由于扩展方法实际是对一个静态方法的调用，所以CLR不会生成代码对调用方法的表达式的值进行null值检查
            ArgumentValidator.ThrowIfNull(enumType, "enumType");
            //获取枚举名称数组
            var names = Enum.GetNames(enumType);
            //获取枚举值数组
            var values = Enum.GetValues(enumType);
            var d = new Dictionary<string, int>(names.Length);
            for (var i = 0; i < names.Length; i++)
            {
                var name = lowerFirstLetter ? names[i].LowerFirstLetter() : names[i];
                d[name] = Convert.ToInt32(values.GetValue(i));
            }
            return d;
        }
        /// <summary>
        /// 转换为小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string LowerFirstLetter(this string s)
        {
            ArgumentValidator.ThrowIfNull(s, "s");
            return char.ToLowerInvariant(s[0]) + s.Substring(1);
        }
    }
}
