using System;
using System.Collections.Generic;
using RUINORERP.Model.ImportEngine.Models;

namespace RUINORERP.Model.ImportEngine.Models
{
    /// <summary>
    /// 宽表导入配置模型
    /// 支持从单个Excel文件导入多个关联表(主表+依赖表+子表)
    /// </summary>
    [Serializable]
    public class WideTableImportProfile
    {
        /// <summary>
        /// 方案名称
        /// </summary>
        public string ProfileName { get; set; }

        /// <summary>
        /// 方案描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 主表配置
        /// </summary>
        public ImportProfile MasterTable { get; set; }

        /// <summary>
        /// 依赖表配置列表(字典表,在主表之前处理)
        /// </summary>
        public List<ImportProfile> DependencyTables { get; set; } = new List<ImportProfile>();

        /// <summary>
        /// 子表配置列表(一对多关系,在主表之后处理)
        /// </summary>
        public List<ChildTableConfig> ChildTables { get; set; } = new List<ChildTableConfig>();

        /// <summary>
        /// 是否启用AI智能映射
        /// </summary>
        public bool EnableAIMapping { get; set; } = false;

        /// <summary>
        /// AI建议的逻辑主键字段名
        /// </summary>
        public string AISuggestedLogicalKey { get; set; }

        public WideTableImportProfile()
        {
            MasterTable = new ImportProfile();
            DependencyTables = new List<ImportProfile>();
            ChildTables = new List<ChildTableConfig>();
        }
    }

    /// <summary>
    /// 子表配置
    /// </summary>
    [Serializable]
    public class ChildTableConfig : ImportProfile
    {
        /// <summary>
        /// 父表引用字段名(外键字段)
        /// </summary>
        public string ParentTableRefField { get; set; }

        /// <summary>
        /// 父表表名
        /// </summary>
        public string ParentTableName { get; set; }
    }
}
