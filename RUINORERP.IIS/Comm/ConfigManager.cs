using RUINORERP.Business;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.IIS.Comm
{
    //如果是静态类。注入时会出错。
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigManager
    {
        private readonly Dictionary<string, string> _configCache = new Dictionary<string, string>();
        public event Action ConfigChanged;

        public ConfigManager()
        {
            //只会加载一次
            LoadConfigValues();
        }

        private async void LoadConfigValues()
        {
            /*
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "RUINORERP.UI.SysConfig.DynamicConfig.DynamicConfig.config"; // 替换为实际的资源名称（包括命名空间）

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();
                        Console.WriteLine(content);
                    }
                }
                else
                {
                    Console.WriteLine("未找到资源");
                }
            }


            var configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = "SysConfig/DynamicConfig/DynamicConfig.config"
            };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None, false);
            var appSettings = config.AppSettings.Settings["SiteTitle"];
            if (appSettings != null)
            {
                //  appSettings.Value;
            }
            */

            // 从数据库加载配置项
            tb_SysGlobalDynamicConfigController<tb_SysGlobalDynamicConfig> ctr = Startup.GetFromFac<tb_SysGlobalDynamicConfigController<tb_SysGlobalDynamicConfig>>();
            var configEntries = await ctr.QueryAsync();
            foreach (var entry in configEntries)
            {
                if(!_configCache.ContainsKey(entry.ConfigKey))
                {
                    _configCache.Add(entry.ConfigKey, entry.ConfigValue);
                }
            }

        }

        public string GetValue(string key)
        {
            if (_configCache.TryGetValue(key, out var value))
            {
                return value;
            }
            throw new KeyNotFoundException($"The configuration key '{key}' was not found.");
        }


        public void UpdateConfig(string key, string value)
        {
            // 更新缓存
            _configCache[key] = value;
            // 更新数据库和缓存
            // 更新配置...
            ConfigChanged?.Invoke();
        }

        // 组件订阅事件
        //ConfigManager.ConfigChanged += (sender, e) => {   
        //    // 更新组件配置
        //        };

    }


    /// <summary>
    /// 用于动态 参数表中的值的数据类型int
    /// </summary>
    public enum ConfigValueType
    {
        [DescriptionAttribute("文本")]
        String,

        [DescriptionAttribute("整数")]
        Integer,

        [DescriptionAttribute("布尔值")]
        Boolean,

        [DescriptionAttribute("小数")]
        Decimal,

        [DescriptionAttribute("日期")]
        DateTime,

        [DescriptionAttribute("双精度浮点数")]
        Double,

        [DescriptionAttribute("单精度浮点数")]
        Float,

        [DescriptionAttribute("长整数")]
        Long,

        [DescriptionAttribute("短整数")]
        Short,

        [DescriptionAttribute("字节数组")]
        Byte,

        [DescriptionAttribute("时间跨度")]
        TimeSpan,

        [DescriptionAttribute(" GUID")]
        Guid,

        [DescriptionAttribute(" URI")]
        Uri,

        [DescriptionAttribute("版本号")]
        Version,

        [DescriptionAttribute("数组")]
        Array,

        [DescriptionAttribute("列表")]
        List,

        [DescriptionAttribute("字典")]
        Dictionary,

        [DescriptionAttribute("集合")]
        Collection,

        [DescriptionAttribute("对象")]
        Object
    }


}
