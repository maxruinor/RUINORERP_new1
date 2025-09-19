using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Cache
{
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
