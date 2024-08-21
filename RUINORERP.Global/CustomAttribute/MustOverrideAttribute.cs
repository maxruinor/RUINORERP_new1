using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.CustomAttribute
{

    /// <summary>
    /// 标注必须实现的函数的特性，在基类声明PreCheckMustOverrideBaseClass即可
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class MustOverrideAttribute : Attribute
    {

    }

}
