using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig
{
    internal class ConfigKeys
    {
        public const string SiteTitle = nameof(SiteTitle);
        public const string CacheDuration = nameof(CacheDuration);
        // 其他配置键...

       // string siteTitle = ConfigManager.GetValue(ConfigKeys.SiteTitle);
       // int cacheDuration = int.Parse(ConfigManager.GetValue(ConfigKeys.CacheDuration));
    }
}
