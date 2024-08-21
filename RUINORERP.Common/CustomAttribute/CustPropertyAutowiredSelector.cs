using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RUINORERP.Common.CustomAttribute
{

    /// <summary>
    /// 选择csla的属性
    /// </summary>
    public class CustPropertyAutowiredSelector : IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            //需要一个判断的维度；
            return propertyInfo.CustomAttributes.Any(it => it.AttributeType == typeof(CustPropertyAutowiredAttribute));
        }

    }
}
