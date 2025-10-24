using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace RUINORERP.Model.ConfigModel
{
    /// <summary>
    /// 服务器配置类
    /// 专注于服务器相关的配置项，包括网络设置、数据库连接、文件存储等
    /// </summary>
    [Serializable()]
    [DisplayName("服务器配置")]
    public class ServerConfig : BaseConfig
    {
        #region 服务器基础配置
        [JsonProperty("ServerName")]
        [Category("服务器基础设置")]
        [Description("服务器名称")]
        public string ServerName { get; set; } = "RUINORERP-Server";

        [JsonProperty("ServerPort")]
        [Category("服务器基础设置")]
        [Description("服务器监听端口")]
        public int ServerPort { get; set; } = 8080;

        [JsonProperty("MaxConnections")]
        [Category("服务器基础设置")]
        [Description("最大连接数")]
        public int MaxConnections { get; set; } = 100;

        [JsonProperty("HeartbeatInterval")]
        [Category("服务器基础设置")]
        [Description("心跳间隔(毫秒)")]
        public int HeartbeatInterval { get; set; } = 60000;
        #endregion

        #region 数据库配置
        [JsonProperty("DbConnectionString")]
        [Category("数据库配置")]
        [Description("数据库连接字符串")]
        public string DbConnectionString { get; set; } = "";

        [JsonProperty("DbType")]
        [Category("数据库配置")]
        [Description("数据库类型(SqlServer/MySQL等)")]
        public string DbType { get; set; } = "SqlServer";
        #endregion

        #region 缓存配置
        [JsonProperty("CacheType")]
        [Category("缓存配置")]
        [Description("缓存类型(Redis/Memory等)")]
        public string CacheType { get; set; } = "Memory";

        [JsonProperty("CacheConnectionString")]
        [Category("缓存配置")]
        [Description("缓存连接字符串")]
        public string CacheConnectionString { get; set; } = "";
        #endregion

        #region 日志配置
        [JsonProperty("EnableLogging")]
        [Category("日志配置")]
        [Description("是否启用日志")]
        public bool EnableLogging { get; set; } = true;

        [JsonProperty("LogLevel")]
        [Category("日志配置")]
        [Description("日志级别(Debug/Info/Warning/Error)")]
        public string LogLevel { get; set; } = "Info";
        #endregion

        #region 文件存储配置
        [JsonProperty("FileStoragePath")]
        [Category("文件存储设置")]
        [Description("服务器文件存储位置，用于保存上传的图片等文件，建议不要设置为程序运行目录。支持使用环境变量，格式: %ENV_VAR%\\path")]
        public string FileStoragePath { get; set; } = "%APPDATA%\\RUINORERP\\FileStorage";

        [JsonProperty("MaxFileSizeMB")]
        [Category("文件存储设置")]
        [Description("单个文件最大上传大小(MB)")]
        public int MaxFileSizeMB { get; set; } = 10;
        
        // 确保与控件中引用的属性一致
        [JsonProperty("SomeSetting")]
        [Category("其他设置")]
        [Description("通用配置设置")]
        public string SomeSetting { get; set; } = "";
        #endregion
        
        #region 文件分类配置
        [JsonProperty("PaymentVoucherPath")]
        [Category("文件分类设置")]
        [Description("付款凭证文件存储路径")]
        public string PaymentVoucherPath { get; set; } = "PaymentVouchers";
        
        [JsonProperty("ProductImagePath")]
        [Category("文件分类设置")]
        [Description("产品图片文件存储路径")]
        public string ProductImagePath { get; set; } = "ProductImages";
        
        [JsonProperty("BOMManualPath")]
        [Category("文件分类设置")]
        [Description("BOM配方手册文件存储路径")]
        public string BOMManualPath { get; set; } = "BOMManuals";
        #endregion
    }
}