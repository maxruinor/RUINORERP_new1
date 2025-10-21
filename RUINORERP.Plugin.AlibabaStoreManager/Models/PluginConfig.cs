using System;
using System.Collections.Generic;
using Microsoft.Web.WebView2.Core;

namespace RUINORERP.Plugin.AlibabaStoreManager.Models
{
    /// <summary>
    /// 插件配置模型
    /// </summary>
    public class PluginConfig
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码（加密存储）
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否自动登录
        /// </summary>
        public bool AutoLogin { get; set; }

        /// <summary>
        /// 最后同步时间
        /// </summary>
        public DateTime LastSyncTime { get; set; }

        /// <summary>
        /// 保存的Cookies
        /// </summary>
        public List<CookieData> SavedCookies { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PluginConfig()
        {
            AutoLogin = false;
            LastSyncTime = DateTime.MinValue;
            SavedCookies = new List<CookieData>();
        }
    }

    /// <summary>
    /// Cookie数据模型
    /// </summary>
    public class CookieData
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime Expires { get; set; }
        public bool HttpOnly { get; set; }
        public bool Secure { get; set; }
    }
}