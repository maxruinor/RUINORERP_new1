using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ConfigModel
{
    /// <summary>
    /// 系统全局配置类
    /// 包含客户端业务操作相关的配置项以及服务器配置项
    /// </summary>
    [Serializable()]
    public class SystemGlobalconfig : BaseConfig
    {
        [JsonProperty("预交日期必须填写")]
        [Category("采购模块")]
        [Description("是否需要填写预交日期")]
        public bool 采购日期必填 { get; set; }

        [JsonProperty("IsFromPlatform")]
        [Category("销售模块")]
        [Description("销售订单默认平台单为真")]
        public bool IsFromPlatform { get; set; }

        [JsonProperty("OpenProdTypeForSaleCheck")]
        [Category("销售模块")]
        [Description("销售订单时开启产品待销型检测")]
        public bool OpenProdTypeForSaleCheck { get; set; } = true;

        [JsonProperty("DirectPrinting")]
        [Category("打印设置")]
        [Description("是否直接打印，如果否则会先打开设计功能再打印")]
        public bool DirectPrinting { get; set; } = true;

        [JsonProperty("UseSharedPrinter")]
        [Category("使用共享打印机")]
        [Description("如果为否，则每个客户端指定一个打印机。为真则使用默认服务器的打印机")]
        public bool UseSharedPrinter { get; set; }

        [JsonProperty("SomeSetting")]
        public string SomeSetting { get; set; }
        
        // 添加服务器相关配置属性
        [JsonProperty("ServerName")]
        [Category("服务器配置")]
        [Description("服务器名称")]
        public string ServerName { get; set; }

        [JsonProperty("ServerPort")]
        [Category("服务器配置")]
        [Description("服务器端口")]
        public int ServerPort { get; set; }

        [JsonProperty("MaxConnections")]
        [Category("服务器配置")]
        [Description("最大连接数")]
        public int MaxConnections { get; set; }

        [JsonProperty("HeartbeatInterval")]
        [Category("服务器配置")]
        [Description("心跳间隔(毫秒)")]
        public int HeartbeatInterval { get; set; }

        [JsonProperty("DbConnectionString")]
        [Category("数据库配置")]
        [Description("数据库连接字符串")]
        public string DbConnectionString { get; set; }

        [JsonProperty("DbType")]
        [Category("数据库配置")]
        [Description("数据库类型")]
        public string DbType { get; set; }

        [JsonProperty("CacheType")]
        [Category("缓存配置")]
        [Description("缓存类型")]
        public string CacheType { get; set; }

        [JsonProperty("CacheConnectionString")]
        [Category("缓存配置")]
        [Description("缓存连接字符串")]
        public string CacheConnectionString { get; set; }
        
        [JsonProperty("EnableLogging")]
        [Category("日志配置")]
        [Description("是否启用日志")]
        public bool EnableLogging { get; set; }
        
        [JsonProperty("LogLevel")]
        [Category("日志配置")]
        [Description("日志级别")]
        public string LogLevel { get; set; }
    }
}