using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.CustomAttribute
{
    /// <summary>
    /// 动态添加的扩展属性标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class AdvExtQueryAttribute : Attribute
    {
        public AdvExtQueryAttribute(string relatedFields, string caption, string colName, AdvQueryProcessType et)
        {
            this.RelatedFields = relatedFields;
            this.Caption = caption;
            this.ColName = colName;
            this.ProcessType = et;
        }

        public AdvQueryProcessType ProcessType { get; set; }
        public string Caption { get; set; }

        /// <summary>
        /// 关联字段,真实字段
        /// </summary>
        public string RelatedFields { get; set; }

        /// <summary>
        /// 属性名：如xxx_
        /// </summary>
        public string ColName { get; set; }
    }

    /// <summary>
    /// 查询字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class AdvQueryAttribute : Attribute
    {
        public AdvQueryAttribute()
        {

        }
        public AdvQueryAttribute(string _ColName, string _ColDesc)
        {
            ColDesc = _ColDesc;
        }

        public string ColName { get; set; }

        public string ColDesc { get; set; }

        public bool HasSubAttr { get; set; }

    }

}
