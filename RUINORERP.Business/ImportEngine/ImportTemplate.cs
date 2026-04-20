using System;
using System.Collections.Generic;

namespace RUINORERP.Business
{
    /// <summary>
    /// 数据导入模板 - 用于简化高频业务的配置
    /// </summary>
    public class ImportTemplate
    {
        public string TemplateName { get; set; } // 模板名称（如：标准产品导入）
        public string TargetTableName { get; set; } // 目标表名（如：tb_Prod）
        
        // 预定义的列映射：Key=Excel列名, Value=数据库字段名
        public Dictionary<string, string> ColumnMappings { get; set; } = new Dictionary<string, string>();
        
        // 图片提取策略：Key=数据库图片字段名, Value=Excel中的图片行偏移量（可选）
        public Dictionary<string, int> ImageStrategies { get; set; } = new Dictionary<string, int>();
        
        // 逻辑主键字段（用于去重和更新判断）
        public string LogicalKeyField { get; set; }
        
        // 是否启用子表导入（如 tb_ProdDetail）
        public bool EnableChildImport { get; set; }
        
        // 子表配置
        public RUINORERP.Model.ImportEngine.Models.ChildTableConfig ChildConfig { get; set; }
    }
}
