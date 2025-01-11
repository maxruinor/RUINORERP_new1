using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDescriptionAttribute : Attribute
    {
        public string Description { get; }

        public EnumDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
    /// <summary>
    /// 这里定义了可以分割销售的功能模块
    /// </summary>
    public enum GlobalFunctionModule
    {
        [Description("客户管理系统 (CRM)")]
        客户管理系统CRM = 1,

        [Description("生产进销存 (ERP)")]
        生产进销存ERP = 2,

        [Description("多公司经营功能")]
        多公司经营功能 = 3,

        [Description("伙伴数据分享功能")]
        伙伴数据分享功能 = 4
    }

}
