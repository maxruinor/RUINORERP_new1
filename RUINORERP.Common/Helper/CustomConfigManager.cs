using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace  RUINORERP.Common.Helper
{
    public class CustomConfigManager
    {
        //读取配置文件：appsettings
        //1.Microsoft.Extensions.Configuration；
        //2.Microsoft.Extensions.Configuration.Json；
        ////读取配置文件  
        //string strCon = CustomConfigManager.GetConfig("ConnectionStrings:MySqlCon");
        public static string GetConfig(string key)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json"); //默认读取：当前运行目录
            IConfigurationRoot configuration = builder.Build();
            string configValue = configuration.GetSection(key).Value;
            return configValue;
        }

        //private void UpdateSetting(string key, string value)
        //{
        //    var conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    conf.AppSettings.Settings[key].Value = value;
        //    conf.Save();
        //    ConfigurationManager.RefreshSection("appSettings");

        //}

    }
}
