using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RUINORERP.Common.CustomAttribute
{


/*
4. 核心作用
精确控制注入范围: 只注入带有[CustPropertyAutowired]标记的属性,避免注入所有属性
减少误注入: 防止一些非服务属性(如UI控件属性、普通属性)被意外注入
提升性能: 只处理标记的属性,减少反射开销
提高可维护性: 明确标识哪些属性需要依赖注入,代码意图更清晰
*/
    /// <summary>
    /// 选择csla的属性
    /// </summary>
    public class CustPropertyAutowiredSelector : IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {

             // 只注入带有 [CustPropertyAutowired] 标记的属性
            return propertyInfo.CustomAttributes.Any(it => it.AttributeType == typeof(CustPropertyAutowiredAttribute));
        }

    }
}
