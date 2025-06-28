using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.CommonModel
{
    // 审计字段特性
    [AttributeUsage(AttributeTargets.Property)]
    public class AuditFieldAttribute : Attribute
    {
        public bool IsAudited { get; set; } = true;
        public string DisplayName { get; set; }
    }
}
