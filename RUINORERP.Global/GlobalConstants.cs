using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global
{

    /// <summary>
    /// 全局常量类
    /// 存储系统中使用的配置相关常量
    /// 注意：新代码应优先使用 IConfigPathResolver 服务获取配置路径
    /// </summary>
    public static class GlobalConstants
    {
        public const string Model_NAME = "RUINORERP.Model";
        public const string ModelDLL_NAME = "RUINORERP.Model.dll";
        public const string UCCellSettingName = "单元设置";

        #region 配置目录常量（已过时，请使用 IConfigPathResolver）

        /// <summary>
        /// 动态配置文件目录（已过时，请使用 IConfigPathResolver.GetConfigDirectory() 方法）
        /// </summary>
        [Obsolete("请使用 IConfigPathResolver.GetConfigDirectory() 方法获取配置目录路径")]
        public const string DynamicConfigFileDirectory = "SysConfigFiles";
        
        #endregion
        
        #region 配置类型常量
        
        /// <summary>
        /// 系统全局配置类型名称
        /// </summary>
        public const string SystemGlobalConfigType = "SystemGlobalConfig";
        
        /// <summary>
        /// 服务器配置类型名称
        /// </summary>
        public const string ServerConfigType = "ServerConfig";
        
        /// <summary>
        /// 全局验证配置类型名称
        /// </summary>
        public const string GlobalValidatorConfigType = "GlobalValidatorConfig";
        
        #endregion
        
        #region 配置同步常量
        
        /// <summary>
        /// 配置同步命令名称
        /// </summary>
        public const string ConfigSyncCommand = "ConfigSync";
        
        /// <summary>
        /// 配置版本时间格式
        /// </summary>
        public const string ConfigVersionDateFormat = "yyyyMMddHHmmssfff";
        
        #endregion
        /// <summary>
        /// 窗体布局个性化配置
        /// </summary>
        public const string LayoutConfigDirectory = "LayoutConfig";


    }

}
