using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Cache
{
    /// <summary>
    /// 缓存管理命令枚举
    /// </summary>
    public enum CacheCommand : uint
    {
        /// <summary>
        /// 缓存更新
        /// </summary>
        [Description("缓存更新")]
        CacheUpdate = 0x0200,

        /// <summary>
        /// 缓存清理
        /// </summary>
        [Description("缓存清理")]
        CacheClear = 0x0201,

        /// <summary>
        /// 缓存统计
        /// </summary>
        [Description("缓存统计")]
        CacheStats = 0x0202,

        /// <summary>
        /// 请求缓存
        /// </summary>
        [Description("请求缓存")]
        CacheRequest = 0x0203,

        /// <summary>
        /// 发送缓存数据
        /// </summary>
        [Description("发送缓存数据")]
        CacheDataSend = 0x0204,

        /// <summary>
        /// 删除缓存
        /// </summary>
        [Description("删除缓存")]
        CacheDelete = 0x0205,

        /// <summary>
        /// 缓存数据列表
        /// </summary>
        [Description("缓存数据列表")]
        CacheDataList = 0x0206,

        /// <summary>
        /// 缓存信息列表
        /// </summary>
        [Description("缓存信息列表")]
        CacheInfoList = 0x0207
    }

    /// <summary>
    /// 缓存操作类型
    /// </summary>
    public enum CacheOperationType
    {
        Get = 0,
        Set = 1,
        Delete = 2,
        Update = 3,
        Clear = 4,
        List = 5,
        Info = 6,
        Refresh = 7
    }

    /// <summary>
    /// 缓存数据类型
    /// </summary>
    public enum CacheDataType
    {
        String = 0,
        Object = 1,
        List = 2,
        Dictionary = 3,
        Binary = 4
    }

    /// <summary>
    /// 缓存配置类型
    /// </summary>
    public enum ConfigurationType
    {
        Static = 0,
        Dynamic = 1,
        Runtime = 2,
        Global = 3
    }
}