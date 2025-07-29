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
    /// 随时根据业务需求进行修改，来让单据输入的默认的数据。
    /// 如设置打印是否直接打印还是显示设计
    /// 这里默认的值还是优化级低点，比方打印是否直接打印。为了简单默认是直接打印
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
    }
}
