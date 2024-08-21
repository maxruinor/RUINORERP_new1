using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Common.CustomAttribute
{

    /// <summary>
    /// 暂时应用于csla框中的 特殊属性自动注入的标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CustPropertyAutowiredAttribute : Attribute
    {

    }
}
