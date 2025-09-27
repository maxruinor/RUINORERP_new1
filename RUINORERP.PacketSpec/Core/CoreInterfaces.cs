using System;

namespace RUINORERP.PacketSpec.Core
{
    

    /// <summary>
    /// 数据包接口 - 定义数据包的基本结构
    /// </summary>
    public interface IPacketData
    {
        /// <summary>
        /// 包标志位
        /// </summary>
        string Flag { get; set; }

        /// <summary>
        /// 包体数据
        /// </summary>
        byte[] Body { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        string SessionId { get; set; }

        /// <summary>
        /// 获取包大小
        /// </summary>
        int GetPackageSize();

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        void ClearSensitiveData();
    }

    /// <summary>
    /// 可跟踪对象接口 - 定义可被跟踪的对象基本结构
    /// </summary>
    public interface ITraceable
    {
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        DateTime CreatedTime { get; set; }

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// 模型版本
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// 更新时间戳
        /// </summary>
        void UpdateTimestamp();
    }

    /// <summary>
    /// 可验证对象接口 - 定义可被验证的对象
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        bool IsValid();
    }

    

    /// <summary>
    /// 基础数据包实现
    /// </summary>
    public class BasePacketData : IPacketData
    {
        /// <summary>
        /// 包标志位
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 包体数据
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 获取包大小
        /// </summary>
        public int GetPackageSize()
        {
            return Body?.Length ?? 0;
        }

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public virtual void ClearSensitiveData()
        {
            // 清理包体数据
            if (Body != null)
            {
                Array.Clear(Body, 0, Body.Length);
                Body = null;
            }
        }
    }
     
  
}
