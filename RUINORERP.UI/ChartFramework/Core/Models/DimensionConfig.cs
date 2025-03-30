using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Models
{
    /// <summary>
    /// 维度配置
    /// </summary>
    public class DimensionConfig
    {
        public DimensionConfig(string fieldName, string displayName, DimensionType type)
        {
            FieldName = fieldName;
            DisplayName = displayName;
            Type = type;
        }

        public string FieldName { get; }
        public string DisplayName { get; }
        public DimensionType Type { get; }
    }

}
