using System.Collections.Generic;

namespace RUINORERP.Business.ImportEngine.Models
{
    /// <summary>
    /// 验证配置（Business层专用，用于服务间传递）
    /// </summary>
    public class ValidationConfig
    {
        /// <summary>
        /// 必填字段列表
        /// </summary>
        public List<string> RequiredFields { get; set; } = new List<string>();
        
        /// <summary>
        /// 外键字段配置
        /// Key: 字段名, Value: (表名, 来源字段)
        /// </summary>
        public Dictionary<string, (string TableName, string SourceField)> ForeignKeyFields { get; set; } = new Dictionary<string, (string, string)>();
        
        /// <summary>
        /// 唯一性字段列表
        /// </summary>
        public List<string> UniqueFields { get; set; } = new List<string>();
    }
}
