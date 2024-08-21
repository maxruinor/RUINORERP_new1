using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.WorkFlow.Steps
{
    /// <summary>
    /// Form窗体标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UserSelectorAttribute : Attribute
    {
        /// <summary>
        /// 描述内容
        /// </summary>
        public string Describe { get; private set; }


        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="type">窗体反射类型</param>
        public UserSelectorAttribute(string describe)
        {
            this.Describe = String.Empty;
        }

    }
}

