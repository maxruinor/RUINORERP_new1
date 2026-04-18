using System;
using System.Collections.Generic;
using RUINORERP.Model.ImportEngine.Enums;

namespace RUINORERP.Model.ImportEngine.Models
{
    /// <summary>
    /// 导入方案配置模型（通用模型，供UI层和Business层共享）
    /// </summary>
    [Serializable]
    public class ImportProfile
    {
        // === 基础信息 ===
        
        /// <summary>
        /// 方案名称
        /// </summary>
        public string ProfileName { get; set; }
        
        /// <summary>
        /// 目标表名
        /// </summary>
        public string TargetTable { get; set; }
        
        /// <summary>
        /// 关联的Excel文件名（为空则使用主文件）
        /// </summary>
        public string SourceExcelFile { get; set; }

        // === 第二套体系字段（ImportEngine/DataMigration）===
        
        /// <summary>
        /// 业务键列表（用于判断记录是否已存在）
        /// </summary>
        public List<string> BusinessKeys { get; set; } = new List<string>();
        
        /// <summary>
        /// 依赖的方案列表
        /// </summary>
        public List<string> Dependencies { get; set; } = new List<string>();
        
        /// <summary>
        /// 主表引用字段
        /// </summary>
        public string MasterTableRefField { get; set; }
        
        /// <summary>
        /// 是否启用ID重映射
        /// </summary>
        public bool EnableIdRemapping { get; set; } = true;
        
        // === 第三套体系字段（BasicDataImport/UCBasicDataImport）===
        
        /// <summary>
        /// 列映射配置（第三套体系使用）
        /// </summary>
        public List<ColumnMapping> ColumnMappings { get; set; } = new List<ColumnMapping>();
        
        /// <summary>
        /// 是否启用去重
        /// </summary>
        public bool EnableDeduplication { get; set; }
        
        /// <summary>
        /// 去重字段列表
        /// </summary>
        public List<string> DeduplicationFields { get; set; } = new List<string>();
        
        /// <summary>
        /// 去重策略
        /// </summary>
        public DeduplicationStrategy DeduplicationStrategy { get; set; } = DeduplicationStrategy.FirstOccurrence;
        
        /// <summary>
        /// Excel文件路径（用于图片提取）
        /// </summary>
        public string ExcelFilePath { get; set; }
        
        /// <summary>
        /// Sheet索引
        /// </summary>
        public int SheetIndex { get; set; } = 0;
        
        /// <summary>
        /// 是否启用外键预加载
        /// </summary>
        public bool EnableForeignKeyPreload { get; set; } = true;
        
        /// <summary>
        /// 是否启用数据验证
        /// </summary>
        public bool EnableValidation { get; set; } = true;
        
        public ImportProfile()
        {
            BusinessKeys = new List<string>();
            Dependencies = new List<string>();
            ColumnMappings = new List<ColumnMapping>();
            DeduplicationFields = new List<string>();
        }
    }
}
