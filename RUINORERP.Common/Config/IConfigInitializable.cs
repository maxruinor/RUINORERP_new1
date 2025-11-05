namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 配置初始化接口
    /// 支持配置类在创建后进行自定义初始化
    /// </summary>
    public interface IConfigInitializable
    {
        /// <summary>
        /// 初始化配置
        /// 在创建默认配置时调用，用于设置合理的默认值
        /// </summary>
        void Initialize();
    }
}