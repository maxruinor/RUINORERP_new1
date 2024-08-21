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
    public class ToolTipAttribute : Attribute
    {
        public ToolTipAttribute(string text)
        {
            _text = text;
        }

        /// <summary>
        /// 默认可见
        /// </summary>
        private string _text = string.Empty;

        public string Text { get => _text; set => _text = value; }

        /// <summary>
        /// 事务传播方式
        /// </summary>
        // public Propagation Propagation { get; set; } = Propagation.Required;

    }
}
