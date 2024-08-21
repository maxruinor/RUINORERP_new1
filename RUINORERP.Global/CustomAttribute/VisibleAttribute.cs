using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Global.CustomAttribute
{

    /// <summary>
    /// 隐藏列
    /// 主要应用于SourceGrid
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class VisibleAttribute : Attribute
    {
        public VisibleAttribute(bool visible)
        {
            Visible = visible;
        }

        /// <summary>
        /// 默认可见
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 事务传播方式
        /// </summary>
        // public Propagation Propagation { get; set; } = Propagation.Required;

    }
}
