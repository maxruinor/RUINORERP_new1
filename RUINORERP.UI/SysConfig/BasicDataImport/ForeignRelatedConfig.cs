using System;
using RUINORERP.Common;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 外键来源列配置
    /// 用于存储外键来源列的完整配置信息
    /// </summary>
    [Serializable]
    public class ForeignKeySourceColumnConfig
    {
        /// <summary>
        /// Excel列名（Key）
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

    /// <summary>
    /// 外键关联配置类
    /// 用于存储外部关联类型的列配置信息
    /// 当导入配置映射时遇到外部关联类型的列，将其配置信息存储在该类的实例中
    /// </summary>
    [Serializable]
    public class ForeignRelatedConfig
    {
        /// <summary>
        /// 外键表引用（键值对：Key=英文表名, Value=中文表名）
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyTable { get; set; }

        /// <summary>
        /// 外键字段引用（键值对：Key=英文字段名, Value=中文显示名）
        /// 用于指定关联表中用于匹配的字段（通常是编码字段，如CategoryCode）
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyField { get; set; }

        /// <summary>
        /// 外键来源列（Excel中的列名）
        /// 指定Excel中作为外键关联依据的来源列（如"供应商名称"列）
        /// 用于通过代码值查询关联表获取主键ID
        /// </summary>
        public ForeignKeySourceColumnConfig ForeignKeySourceColumn { get; set; }

        /// <summary>
        /// 是否级联更新
        /// 当关联表数据更新时，是否同步更新外键引用
        /// </summary>
        public bool CascadeUpdate { get; set; }

        /// <summary>
        /// 是否允许为空关联
        /// 当外键值在关联表中找不到对应记录时的处理方式
        /// </summary>
        public bool AllowEmptyRelation { get; set; }

        /// <summary>
        /// 空值时的默认处理方式
        /// 可选值：Error(报错), Skip(跳过), Default(使用默认值)
        /// </summary>
        public string EmptyRelationAction { get; set; } = "Error";

        /// <summary>
        /// 构造函数
        /// </summary>
        public ForeignRelatedConfig()
        {
        }

        /// <summary>
        /// 验证外键关联配置是否完整
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>是否验证通过</returns>
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

        /// <summary>
        /// 获取缓存键
        /// 用于在外键服务中缓存查询结果
        /// </summary>
        /// <returns>缓存键</returns>
        public string GetCacheKey()
        {
            if (ForeignKeyTable != null && ForeignKeyField != null)
            {
                return $"{ForeignKeyTable.Key}_{ForeignKeyField.Key}";
            }
            return string.Empty;
        }

        /// <summary>
        /// 克隆当前配置
        /// </summary>
        /// <returns>克隆的副本</returns>
        public ForeignRelatedConfig Clone()
        {
            return new ForeignRelatedConfig
            {
                ForeignKeyTable = this.ForeignKeyTable,
                ForeignKeyField = this.ForeignKeyField,
                ForeignKeySourceColumn = this.ForeignKeySourceColumn,
                CascadeUpdate = this.CascadeUpdate,
                AllowEmptyRelation = this.AllowEmptyRelation,
                EmptyRelationAction = this.EmptyRelationAction
            };
        }

        public override string ToString()
        {
            if (ForeignKeyTable != null && ForeignKeyField != null)
            {
                return $"{ForeignKeyTable.Value}.{ForeignKeyField.Value}";
            }
            return base.ToString();
        }
    }
}
