using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig
{

    //    var(siteTitle, titleDescription) = ConfigHelper.GetConfig("SiteTitle");
    //var(cacheDuration, durationDescription) = ConfigHelper.GetConfig("CacheDuration");
    public static class ConfigHelper
    {
        public static (string value, string description) GetConfig(string key)
        {


            // 从数据库或缓存中获取配置项
            var configEntry = Startup.GetFromFac<ConfigManager>().GetValue(key);

            // 假设你有一个方法来获取配置项的描述
            string description = GetConfigDescription(key);

            return (configEntry, description);
        }

        private static string GetConfigDescription(string key)
        {
            // 从某个地方获取配置项的描述，例如数据库或资源文件
            return "Description for " + key;
        }

        public static void UpdateConfigValue(string key, string value)
        {
            // 更新数据库
            //using (var context = new YourDbContext())
            //{
            //    var configEntry = context.GlobalConfigs.FirstOrDefault(c => c.ConfigKey == key);
            //    if (configEntry != null)
            //    {
            //        configEntry.ConfigValue = value;
            //        context.SaveChanges();
            //    }
            //}

            // 更新缓存
            // _configCache[key] = value;
        }

    }
}
