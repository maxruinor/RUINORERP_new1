using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.CustomAttribute
{
    //作者：丢子丢
    //链接：https://www.zhihu.com/question/456873136/answer/1858874120
    //来源：知乎
    //著作权归作者所有。商业转载请联系作者获得授权，非商业转载请注明出处。

    /// <summary>
    /// 标注包含必须实现函数的基类，写在基类上
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class PreCheckMustOverrideBaseClassAttribute : Attribute
    {
        public static void CheckAll(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(t => t.GetCustomAttribute<PreCheckMustOverrideBaseClassAttribute>(true) != null);
            foreach (var type in types)
            {
                var methods = type.GetMethods().Where(m => m.GetCustomAttribute<MustOverrideAttribute>(true) != null);

                foreach (var m in methods)
                {
                    if (m.DeclaringType.Name != type.Name)
                    {
                        throw new Exception($"{type.Name}必须实现{m.Name}方法。");
                    }
                }
            }
        }
    }
}
