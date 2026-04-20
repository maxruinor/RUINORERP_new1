using System;
using RUINORERP.Common;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    // 注意: ForeignRelatedConfig 已移至 RUINORERP.Model.ImportEngine.Models
    // UI层直接使用 Model 层的类型,避免重复定义
    
    /// <summary>
    /// 外键来源列配置(UI层保留,用于兼容性)
    /// </summary>
    [Serializable]
    public class ForeignKeySourceColumnConfig
    {
        /// <summary>
        /// Excel列名
        /// </summary>
        public string ExcelColumnName { get; set; }

        /// <summary>
        /// 显示名称（Value.Key）
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 数据库真实字段名（Value.Value）
        /// </summary>
        public string DatabaseFieldName { get; set; }
    }
}
