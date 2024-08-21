using RUINORERP.Common.Extensions;
using RUINORERP.Global.CustomAttribute;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common
{
    public class SqlSugarHelper
    {
     


        /// <summary>
        /// 处理表达式树拼接
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ProcessExp(PropertyInfo property, object value)
        {

            //引用类型 此处只考虑string类型的情况
            if (!property.PropertyType.IsValueType) return $"{property.Name} = \"{value}\" ";

            //可空值类型 此处只考虑简单值类型
            if (property.PropertyType.Name.Equals("Nullable`1"))
            {
                if (property.PropertyType.GenericTypeArguments[0].Name.Equals("DateTime"))
                {
                    return $"{property.Name} = Convert.ToDateTime(\"{value}\") ";
                }

                return $"{property.Name} = {value} ";
            }

            //值类型  此处只考虑简单值类型
            if (property.PropertyType.Name.Equals("DateTime"))
            {
                return $"{property.Name} = Convert.ToDateTime(\"{value}\") ";
            }

            return $"{property.Name} = {value} ";

        }
    }
}
