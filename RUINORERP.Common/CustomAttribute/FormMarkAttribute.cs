using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common
{
    /// <summary>
    /// Form窗体标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FormMarkAttribute : Attribute
    {
        /// <summary>
        /// 描述内容
        /// </summary>
        public string Describe { get; private set; }
        /// <summary>
        /// 是否运行注入，默认false
        /// </summary>
        public bool IsIOC { get; private set; }
        /// <summary>
        /// 窗体类型
        /// </summary>
        public Type FormType { get; private set; }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="type">窗体反射类型</param>
        public FormMarkAttribute(Type type)
        {
            this.FormType = type;
            this.IsIOC = false;
            this.Describe = String.Empty;
        }
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="type">窗体反射类型</param>
        /// <param name="describe">窗体描述</param>
        public FormMarkAttribute(Type type, string describe)
        {
            this.Describe = describe;
            this.IsIOC = false;
            this.FormType = type;
        }
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="type">窗体反射类型</param>
        /// <param name="describe">是否需要注入</param>
        public FormMarkAttribute(Type type, bool isIOC)
        {
            this.Describe = String.Empty;
            this.IsIOC = isIOC;
            this.FormType = type;
        }
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="type">窗体反射类型</param>
        /// <param name="describe">窗体描述</param>
        /// <param name="isIOC">是否需要注入</param>
        public FormMarkAttribute(Type type, string describe, bool isIOC)
        {
            this.Describe = describe;
            this.IsIOC = isIOC;
            this.FormType = type;
        }
    }
}
