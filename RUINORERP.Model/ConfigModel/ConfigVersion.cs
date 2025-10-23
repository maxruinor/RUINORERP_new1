using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RUINORERP.Model.ConfigModel
{
    /// <summary>
    /// 配置版本模型
    /// 用于存储配置的历史版本信息，支持版本控制和回滚
    /// </summary>
    [Serializable]
    [DisplayName("配置版本")]
    public class ConfigVersion
    {
        /// <summary>
        /// 版本ID
        /// </summary>
        [JsonProperty("VersionId")]
        public Guid VersionId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 配置类型
        /// </summary>
        [JsonProperty("ConfigType")]
        public string ConfigType { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty("VersionNumber")]
        public int VersionNumber { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("CreatedTime")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 版本描述
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// 配置内容快照文件路径
        /// </summary>
        [JsonProperty("SnapshotPath")]
        public string SnapshotPath { get; set; }

        /// <summary>
        /// 是否为当前活动版本
        /// </summary>
        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [JsonProperty("Remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// 版本文件路径（兼容其他代码使用）
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public string VersionFilePath { get { return SnapshotPath; } set { SnapshotPath = value; } }

        /// <summary>
        /// 检查版本文件是否存在
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public bool VersionFileExists => !string.IsNullOrEmpty(SnapshotPath) && File.Exists(SnapshotPath);

        /// <summary>
        /// 版本信息的显示文本
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public string DisplayText => $"v{VersionNumber} - {Description} ({CreatedTime:yyyy-MM-dd HH:mm:ss})";

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>版本信息的字符串表示</returns>
        public override string ToString()
        {
            return DisplayText;
        }
    }

    /// <summary>
    /// 配置版本差异结果
    /// 用于存储两个版本之间的差异信息
    /// </summary>
    public class ConfigVersionDiffResult
    {
        /// <summary>
        /// 新增的属性列表
        /// </summary>
        public Dictionary<string, object> AddedProperties { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 删除的属性列表
        /// </summary>
        public Dictionary<string, object> RemovedProperties { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 修改的属性列表
        /// </summary>
        public Dictionary<string, ModifiedPropertyValue> ModifiedProperties { get; set; } = new Dictionary<string, ModifiedPropertyValue>();

        /// <summary>
        /// 是否存在差异
        /// </summary>
        public bool HasDifferences => AddedProperties.Count > 0 || RemovedProperties.Count > 0 || ModifiedProperties.Count > 0;
    }

    /// <summary>
    /// 修改的属性值
    /// 存储属性的旧值和新值
    /// </summary>
    public class ModifiedPropertyValue
    {
        /// <summary>
        /// 属性的旧值
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// 属性的新值
        /// </summary>
        public object NewValue { get; set; }
    }
}