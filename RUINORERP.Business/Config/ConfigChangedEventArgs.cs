using System;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置变更事件参数
    /// 提供配置变更时的详细信息
    /// </summary>
    /// <typeparam name="T">配置类型，必须继承自BaseConfig</typeparam>
    public class ConfigChangedEventArgs<T> : EventArgs where T : BaseConfig
    {
        /// <summary>
        /// 获取或设置新的配置对象
        /// </summary>
        public T NewConfig { get; set; }
        
        /// <summary>
        /// 获取或设置旧的配置对象
        /// </summary>
        public T OldConfig { get; set; }
        
        /// <summary>
        /// 获取或设置配置变更的原因
        /// </summary>
        public string Reason { get; set; }
        
        /// <summary>
        /// 获取配置变更的时间戳
        /// </summary>
        public DateTime Timestamp { get; private set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigChangedEventArgs()
        {
            Timestamp = DateTime.Now;
        }
    }
}