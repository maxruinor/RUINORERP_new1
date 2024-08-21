using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Global.CustomAttribute
{
    /// <summary>
    /// 标记实体字段是否为小计结果列。目前暂时只支持一行一个小计列
    /// 主要应用于SourceGrid
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class SubtotalResultAttribute : Attribute
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
