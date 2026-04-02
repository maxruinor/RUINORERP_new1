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
    /// 全局验证配置类,随时根据业务需求进行修改, 来验证单据输入的数据合法性。
    /// 可以从客户端或服务器端分发这些配置1
    /// </summary>
    [Serializable()]
    [DisplayName("全局验证配置")]
    public class GlobalValidatorConfig : BaseConfig
    {

        #region 采购模块


        /// <summary>
        /// JsonProperty这个属性值要与真的属性值名称一样。不然不起作用。读取出错。
        /// </summary>
        [JsonProperty("预交日期必填")]
        [Category("采购模块")]
        [Description("采购预交日期设置为必须填写。")]
        public bool 预交日期必填 { get; set; }

        #endregion 采购模块

        #region 生产模块

        [JsonProperty("预开工日期必填")]
        [Category("生产模块")]
        [Description("预开工日期必填设置为必须填写。")]
        public bool 预开工日期必填 { get; set; }

        [JsonProperty("预完工日期必填")]
        [Category("生产模块")]
        [Description("预完工日期必填设置为必须填写。")]
        public bool 预完工日期必填 { get; set; }



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
        [Description("中进计划提前多少天提醒, 默认提前1天。")]
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



        [JsonProperty("IsFromPlatform")]
        [Category("销售模块")]
        [Description("销售订单默认平台单为真")]
        public bool IsFromPlatform { get; set; } = true;




        [JsonProperty("NeedInputProjectGroup")]
        [Category("销售模块")]
        [Description("销售订单默认项目组必填写")]
        public bool NeedInputProjectGroup { get; set; } = true;
        #endregion

        #region 财务模块

        /// <summary>
        /// 预收付款单的收付款账户和付款方式是否必须填写
        /// 默认值为false，表示在预收付款单中，账户和付款方式可以为空
        /// 适用于预付款业务场景，允许先创建单据后补充账户信息
        /// </summary>
        [JsonProperty("预收付款单账户必填")]
        [Category("财务模块")]
        [Description("预收付款单的收付款账户和付款方式是否必须填写。默认否，允许为空")]
        public bool 预收付款单账户必填 { get; set; } = false;

        /// <summary>
        /// 收付款单的收付款账户和付款方式是否必须填写
        /// 默认值为true，表示在收付款单中，账户和付款方式必须填写
        /// 适用于实际收付款业务场景，要求必须有明确的收付款账户
        /// </summary>
        [JsonProperty("收付款单账户必填")]
        [Category("财务模块")]
        [Description("收付款单的收付款账户和付款方式是否必须填写。默认真，必须填写")]
        public bool 收付款单账户必填 { get; set; } = true;

        /// <summary>
        /// JsonProperty这个属性值要与真的属性值名称一样。不然不起作用。读取出错。
        /// 【已弃用】请使用 预收付款单账户必填 和 收付款单账户必填 替代
        /// 保留此属性以保持向后兼容性
        /// </summary>
        [JsonProperty("收付款账户必填")]
        [Category("财务模块")]
        [Description("【已弃用】应收款/应付款审核时, 收付款账户信息是否必填。请使用预收付款单账户必填和收付款单账户必填")]
        [Obsolete("请使用 预收付款单账户必填 和 收付款单账户必填 替代")]
        public bool 收付款账户必填 { get; set; } = true;
        
 
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
