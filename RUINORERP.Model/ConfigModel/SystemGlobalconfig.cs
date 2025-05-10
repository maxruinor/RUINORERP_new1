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


        [JsonProperty("SomeSetting")]
        public string SomeSetting { get; set; }
    }
}
