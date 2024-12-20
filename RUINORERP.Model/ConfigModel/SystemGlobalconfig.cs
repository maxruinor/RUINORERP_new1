using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ConfigModel
{
    [Serializable()]
    public class SystemGlobalconfig : BaseConfig
    {

        [JsonProperty("预交日期必须填写")]
        [Category("采购模块")]
        [Description("是否需要填写预交日期")]
        public bool 采购日期必填 { get; set; }



        [JsonProperty("SomeSetting")]
        public string SomeSetting { get; set; }
    }
}
