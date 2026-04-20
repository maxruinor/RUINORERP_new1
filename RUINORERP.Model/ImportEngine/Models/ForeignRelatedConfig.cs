using System;
using RUINORERP.Global;

namespace RUINORERP.Model.ImportEngine.Models
{
    /// <summary>
    /// 外键关联配置(通用模型,供所有层使用)
    /// </summary>
    [Serializable]
    public class ForeignRelatedConfig
    {
        /// <summary>
        /// 外键表引用(Key=英文表名, Value=中文表名)
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyTable { get; set; }

        /// <summary>
        /// 外键字段引用(Key=英文字段名, Value=中文显示名)
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyField { get; set; }

        /// <summary>
        /// 是否在数据库中不存在时自动创建新记录
        /// </summary>
        public bool AutoCreateIfNotExists { get; set; } = false;

        /// <summary>
        /// 外键来源列配置（用于指定从 Excel 的哪一列获取参考值）
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeySourceColumn { get; set; }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>是否有效</returns>
        public bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;
            if (ForeignKeyTable == null || string.IsNullOrEmpty(ForeignKeyTable.Key))
            {
                errorMessage = "外键关联表不能为空";
                return false;
            }
            if (ForeignKeyField == null || string.IsNullOrEmpty(ForeignKeyField.Key))
            {
                errorMessage = "外键关联字段不能为空";
                return false;
            }
            return true;
        }
    }
}
