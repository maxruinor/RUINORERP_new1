using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Global.CustomAttribute
{

    /// <summary>
    /// 标记实体字段是否参与小计
    /// 主要应用于SourceGrid
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class SubtotalAttribute : Attribute
    {
        /// <summary>
        /// 标签类型 样式
        /// </summary>
      //  public string TagType { get; set; }

        /// <summary>
        /// 事务传播方式
        /// </summary>
       // public Propagation Propagation { get; set; } = Propagation.Required;

    }
}
