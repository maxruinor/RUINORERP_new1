
// **************************************
// 生成：Manual
// 项目：打印系统分级配置
// 版权：Copyright RUINOR
// 时间：2025-03-25
// **************************************
using System;
using Newtonsoft.Json;

namespace RUINORERP.Model
{
    /// <summary>
    /// 菜单打印配置数据(用于JSON序列化)
    /// </summary>
    [Serializable]
    public class MenuPrintConfigData
    {
        /// <summary>
        /// 打印机名称
        /// </summary>
        [JsonProperty("PrinterName")]
        public string PrinterName { get; set; }

        /// <summary>
        /// 是否设置了指定打印机
        /// </summary>
        [JsonProperty("PrinterSelected")]
        public bool PrinterSelected { get; set; }

        /// <summary>
        /// 是否横向打印
        /// </summary>
        [JsonProperty("Landscape")]
        public bool Landscape { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        [JsonProperty("TemplateId")]
        public long TemplateId { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        [JsonProperty("TemplateName")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 是否为默认模板
        /// </summary>
        [JsonProperty("IsDefaultTemplate")]
        public bool IsDefaultTemplate { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [JsonProperty("BizType")]
        public int BizType { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        [JsonProperty("BizName")]
        public string BizName { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [JsonProperty("LastModified")]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// 创建默认配置
        /// </summary>
        public static MenuPrintConfigData CreateDefault()
        {
            return new MenuPrintConfigData
            {
                PrinterName = string.Empty,
                PrinterSelected = false,
                Landscape = false,
                TemplateId = 0,
                TemplateName = string.Empty,
                IsDefaultTemplate = true,
                BizType = 0,
                BizName = string.Empty,
                LastModified = DateTime.Now
            };
        }
    }
}
