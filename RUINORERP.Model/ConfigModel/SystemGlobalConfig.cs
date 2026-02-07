using Newtonsoft.Json;
using RUINORERP.Global;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    [DisplayName("客户端系统全局配置")]
    public class SystemGlobalConfig : BaseConfig
    {

        [JsonProperty("系统单据修改模式")]
        [Category("系统设置")]
        [Description("提交后修改规则模式枚举")]
        public SubmitModifyRuleMode 单据修改模式 { get; set; } =  SubmitModifyRuleMode.灵活模式;



        [JsonProperty("客户端自动更新")]
        [Category("系统设置")]
        //[Description("客户端自动更新")]
        [Display(Name = "客户端自动更新", Description = "消息提醒的时间间隔")]
        public bool 客户端自动更新 { get; set; } = true;




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
        [Description("是否启用直接打印功能")]
        public bool DirectPrinting { get; set; } = false;
        
        [Category("其他设置")]
        [Description("销售出库单生成后自动打印")]
        public bool AutoPrintAfterSave { get; set; }

        [JsonProperty("UseSharedPrinter")]
        [Category("使用共享打印机")]
        [Description("如果为否，则每个客户端指定一个打印机。为真则使用默认服务器的打印机")]
        public bool UseSharedPrinter { get; set; }

        [JsonProperty("SomeSetting")]
        [Category("其他设置")]
        [Description("通用配置设置")]
        public string SomeSetting { get; set; } = "";
        
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
        [Range(1, 60, ErrorMessage = "心跳间隔必须在1000-60000毫秒之间")]
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
        
        //
        [JsonProperty("LogLevel")]
        [Category("日志配置")]
        [Description("日志级别，可选值：Trace、Debug、Information、Warning、Error、Critical、None")]
        public string LogLevel { get; set; }

        [JsonProperty("EnableBillStatusMessage")]
        [Category("消息配置")]
        [Description("是否启用单据状态变化消息发送功能")]
        public bool EnableBillStatusMessage { get; set; } = true;

        [JsonProperty("OllamaApiAddress")]
        [Category("AI配置")]
        [Description("OLLAMA大模型API地址")]
        public string OllamaApiAddress { get; set; } = "http://localhost:11434/api";

        [JsonProperty("OllamaDefaultModel")]
        [Category("AI配置")]
        [Description("OLLAMA大模型默认名称")]
        public string OllamaDefaultModel { get; set; } = "deepseek-coder:6.7b";
    }
}