using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ConfigModel
{
    /// <summary>
    /// 全局验证配置类,随时根据业务需求进行修改，来验证单据输入的数据合法性。
    /// 可以从客户端或服务器端分布这些配置
    /// </summary>
    [Serializable()]
    public class GlobalValidatorConfig : BaseConfig
    {
        /// <summary>
        /// JsonProperty这个属性值要与真的属性值名称一样。不然不起作用。读取出错。
        /// </summary>
        [JsonProperty("预交日期必填")]
        [Category("采购模块")]
        [Description("采购预交日期设置为必须填写。")]
        public bool 预交日期必填 { get; set; }

        #region 生产模块

        [JsonProperty("预开工日期必填")]
        [Category("生产模块")]
        [Description("预开工日期必填设置为必须填写。")]
        public bool 预开工日期必填 { get; set; }

        [JsonProperty("预完工日期必填")]
        [Category("生产模块")]
        [Description("预完工日期必填设置为必须填写。")]
        public bool 预完工日期必填 { get; set; }

        [JsonProperty("销售订单默认平台单为真")]
        [Category("销售模块")]
        [Description("销售订单默认平台单为真")]
        public bool IsFromPlatform { get; set; }

        [JsonProperty("返工提醒天数")]
        [Category("生产模块")]
        [Description("超过这个天数系统自动提醒。")]
        public int ReworkTipDays { get; set; } = 3;

        #endregion

        #region 客户关系
        /// <summary>
        /// JsonProperty这个属性值要与真的属性值名称一样。不然不起作用。读取出错。
        /// </summary>
        [JsonProperty("计划提前提示天数")]
        [Category("客户关系")]
        [Description("中进计划提前多少天提醒，默认提前1天。")]
        public int 计划提前提示天数 { get; set; } = 1;
        #endregion

        #region 销售模块
        /// <summary>
        /// JsonProperty这个属性值要与真的属性值名称一样。不然不起作用。读取出错。
        /// </summary>
        [JsonProperty("销售金额精度")]
        [Category("销售模块")]
        [Description("销售出库中金额的精度。")]
        public int MoneyDataPrecision { get; set; } = 4;
        #endregion



        /// <summary>
        /// JsonProperty这个属性值要与真的属性值名称一样。不然不起作用。读取出错。
        /// </summary>
        [JsonProperty("借出单的接收单位必填")]
        [Category("借出")]
        [Description("借出单的接收单位设置为必须填写。")]
        public bool 借出单的接收单位必填 { get; set; }



        [JsonProperty("SomeSetting")]
        public string SomeSetting { get; set; }
    }
}
