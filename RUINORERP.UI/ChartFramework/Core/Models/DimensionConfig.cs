using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Models
{
    /// <summary>
    /// 维度配置
    /// 创建时间，业务员，区域
    /// </summary>
    public class DimensionConfig
    {
        public DimensionConfig()
        {

        }
        public DimensionConfig(string fieldName, string displayName, DimensionType type)
        {
            FieldName = fieldName;
            DisplayName = displayName;
            Type = type;
        }

        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public DimensionType Type { get; set; }

        public bool IsTimeBased { get; set; }
    }

}
