using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RUINORERP.Model.ConfigModel
{
    [Serializable()]
    public abstract class BaseConfig
    {
        /// <summary>
        /// 获取配置类型名称
        /// </summary>
        [JsonIgnore]
        public virtual string ConfigType => GetType().Name;
        
        /// <summary>
        /// 配置显示名称
        /// </summary>
        [JsonIgnore]
        public virtual string DisplayName => GetType().GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? ConfigType;
        
        /// <summary>
        /// 初始化默认值
        /// </summary>
        public virtual void InitDefault()
        {
            // 子类可以重写此方法设置默认值
        }
        
        /// <summary>
        /// 验证配置
        /// </summary>
        public virtual ConfigValidationResult Validate()
        {
            return new ConfigValidationResult { IsValid = true };
        }
    }
}
